using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using PacketLogConverter.Utils;

namespace PacketLogConverter.LogFilters
{
	/// <summary>
	/// Manages packet property variables.
	/// </summary>
	internal class PacketPropertyVariablesManager
	{
		#region Management of contained data

		private readonly IDictionary<string, VariableEntry>				variableEntriesByName	= new Dictionary<string, VariableEntry>();
		private readonly List<VariableEntry>							variableEntriesList			= new List<VariableEntry>();

		#region Variables

		private delegate bool InternalVariableOperation(PacketPropertyVariable variable);

		/// <summary>
		/// Variable and its linked internal data.
		/// </summary>
		private class VariableEntry
		{
			public			PacketPropertyVariable		variable;
			public event	VariableValueUpdateHandler	valueUpdatingEvent;
			private			object						value;

			public object Value
			{
				get { return value; }
				set
				{
					// Fire event
					VariableValueUpdateHandler e = valueUpdatingEvent;
					if (null != e)
					{
						e(variable, this.value, value);
					}
					this.value = value;
				}
			}
		}

		/// <summary>
		/// Does the variable operation.
		/// </summary>
		/// <param name="variableName">Name of the variable.</param>
		/// <param name="operationHandler">The operation handler.</param>
		/// <param name="successHandler">The success handler.</param>
		/// <returns><code>true</code> if variable exists and <paramref name="operationHandler"/> returned <code>true</code>.</returns>
		private bool DoVariableOperation(string variableName,
										InternalVariableOperation operationHandler,
										VariableOperationEventHandler successHandler)
		{
			bool ret;
			try
			{
				// Check if variable exists
				VariableEntry entry;
				ret = variableEntriesByName.TryGetValue(variableName, out entry);

				PacketPropertyVariable variable = (ret ? entry.variable : null);

				if (ret)
				{
					// Do operation
					ret = operationHandler(variable);
				}

				if (ret)
				{
					// Fire event only on successful modification of list
					successHandler(variable);
				}
			}
			catch (Exception e)
			{
				ret = false;
				// Format message
				string message = string.Format("Error in variable operation with params: variableName={0}, operationHandler={1}, successHandler={2}"
					, variableName ?? "null"
					, operationHandler ?? (object)"null"
					, successHandler ?? (object)"null");

				Log.Error(message, e);
			}

			return ret;
		}

		/// <summary>
		/// Adds the variable.
		/// </summary>
		/// <param name="variable">The variable.</param>
		/// <returns><code>true</code> if variable is added successfully, <code>false</code> otherwise.</returns>
		public bool AddVariable(PacketPropertyVariable variable)
		{
			bool ret = false;

			try
			{
				ret = !variableEntriesByName.ContainsKey(variable.Name);
				if (ret)
				{
					// Add variable to collections
					VariableEntry entry = new VariableEntry();
					entry.variable = variable;
					variableEntriesByName.Add(variable.Name, entry);
					int oldCount = variableEntriesList.Count;
					variableEntriesList.Add(entry);

					// Check for errors
					ret = (oldCount != variableEntriesList.Count);
					if (!ret)
					{
						string message = "Internal lists are in inconsistent state after adding of variable " + (variable ?? (object)"(null)");
						Log.Error(message);
						throw new Exception(message);
					}
					else
					{
						// Fire event
						FireVariableOperationEvent(variable, VariableAdded);
					}
				}
			}
			catch (Exception e)
			{
				ret = false;
				Log.Error("Error adding a variable: variable=" + (variable ?? (object)"null"), e);
			}

			return ret;
		}

		/// <summary>
		/// Renames the variable.
		/// </summary>
		/// <param name="oldName">The old name.</param>
		/// <param name="newName">The new name.</param>
		/// <returns><code>true</code> </returns>
		public bool RenameVariable(string oldName, string newName)
		{
			bool ret = DoVariableOperation(
				oldName,
				// Change the name
				delegate(PacketPropertyVariable variable)
					{
						bool opRet = !variableEntriesByName.ContainsKey(newName);
						if (opRet)
						{
							// Get old variable entry
							VariableEntry entry;
							opRet = variableEntriesByName.TryGetValue(oldName, out entry);
							if (opRet)
							{
								// Remove old variable entry
								opRet = variableEntriesByName.Remove(oldName);
							}

							if (opRet)
							{
								// Add old variable entry by new name and update variable name
								variable.Name = newName;
								variableEntriesByName.Add(variable.Name, entry);
							}
						}
						return opRet;
					},
				// Fire event on success
				delegate(PacketPropertyVariable variable) { FireVariableOperationEvent(variable, VariableModified); })
			;
			return ret;
		}

		/// <summary>
		/// Removes the variable.
		/// </summary>
		/// <param name="variableName">The name.</param>
		/// <returns><code>true</code> if variable with specified variableName existed, <code>false</code> otherwise.</returns>
		public bool RemoveVariable(string variableName)
		{
			bool ret = DoVariableOperation(
				variableName,
				// Remove existing variable
				delegate(PacketPropertyVariable variable)
					{
						// Get variable entry by name
						VariableEntry entry;
						bool opRet = variableEntriesByName.TryGetValue(variableName, out entry);

						if (opRet)
						{
							// Remove variable by name
							variableEntriesByName.Remove(variableName);
						}

						if (opRet)
						{
							// Remove variable entry from flat list
							opRet = variableEntriesList.Remove(entry);
							if (!opRet)
							{
								string message = "Internal lists are in inconsistent state after removal of variable " + (variableName ?? "(null)");
								Log.Error(message);
								throw new Exception(message);
							}
						}
						return opRet;
					},
				// Fire event on success
				delegate(PacketPropertyVariable variable) { FireVariableOperationEvent(variable, VariableRemoved); })
			;
			return ret;
		}

		/// <summary>
		/// Clears the variables.
		/// </summary>
		public void ClearVariables()
		{
			for (int i = variableEntriesList.Count - 1; 0 <= i; --i)
			{
				VariableEntry entry = variableEntriesList[i];
				RemoveVariable(entry.variable.Name);
			}
		}

		#endregion

		#region Variable links

		/// <summary>
		/// Adds the variable link.
		/// </summary>
		/// <param name="variableName">Name of the variable.</param>
		/// <param name="variableLink">The variable link.</param>
		/// <returns><code>true</code> if link is added successfully, <code>false</code> otherwise.</returns>
		public bool AddVariableLink(string variableName, PacketPropertyVariableLink variableLink)
		{
			bool ret = DoVariableOperation(
				variableName,
				// Add the link
				delegate(PacketPropertyVariable variable)
					{
						bool opRet = !IsVariableLinkedToPacket(variable, variableLink.PacketClass.type);
						if (opRet)
						{
							variable.Links.Add(variableLink);
						}
						return opRet;
					},
				// Fire event on success
				delegate(PacketPropertyVariable variable) { FireVariableLinkOperationEvent(variable, variableLink, VariableLinkAdded); })
			;
			return ret;
		}

		/// <summary>
		/// Determines whether variable is linked to packet.
		/// </summary>
		/// <param name="variable">The variable.</param>
		/// <param name="packetType">Type of the packet.</param>
		/// <returns>
		/// 	<c>true</c> if variable is linked to packet; otherwise, <c>false</c>.
		/// </returns>
		private bool IsVariableLinkedToPacket(PacketPropertyVariable variable, Type packetType)
		{
			bool ret = false;

			IList<PacketPropertyVariableLink> links = variable.Links;
			for (int i = links.Count - 1; 0 <= i; --i)
			{
				PacketPropertyVariableLink link = links[i];
				if (link.PacketClass.type == packetType)
				{
					ret = true;
				}
			}

			return ret;
		}

		/// <summary>
		/// Replaces the variable link.
		/// </summary>
		/// <param name="variableName">Name of the variable.</param>
		/// <param name="oldVariableLink">The old variable link.</param>
		/// <param name="newVariableLink">The new variable link.</param>
		/// <returns><code>true</code> if old variable and link exists, <code>false</code> otherwise.</returns>
		public bool ReplaceVariableLink(string variableName, PacketPropertyVariableLink oldVariableLink, PacketPropertyVariableLink newVariableLink)
		{
			bool ret = DoVariableOperation(
				variableName,
				// Replace the link
				delegate(PacketPropertyVariable variable)
					{
						// Check if variable is already linked to new packet and that link is not the link we are replacing
						// The fact that only single packet can be linked to single variable provides needed guarantees
						bool opRet = !IsVariableLinkedToPacket(variable, newVariableLink.PacketClass.type)
							|| newVariableLink.PacketClass.type == oldVariableLink.PacketClass.type;

						if (opRet)
						{
							int index = variable.Links.IndexOf(oldVariableLink);
							opRet = (0 <= index);
							if (opRet)
							{
								variable.Links[index] = newVariableLink;
							}
						}

						return opRet;
					},
				// Fire event on success
				delegate(PacketPropertyVariable variable) { FireVariableLinkOperationEvent(variable, newVariableLink, VariableLinkReplaced); })
			;
			return ret;
		}

		/// <summary>
		/// Removes the variable link.
		/// </summary>
		/// <param name="variableName">Name of the variable.</param>
		/// <param name="variableLink">The variable link.</param>
		/// <returns><code>true</code> if like is removed, <code>false</code> otherwise.</returns>
		public bool RemoveVariableLink(string variableName, PacketPropertyVariableLink variableLink)
		{
			bool ret = DoVariableOperation(
				variableName,
				// Add the link
				delegate(PacketPropertyVariable variable) { return variable.Links.Remove(variableLink); },
				// Fire event on success
				delegate(PacketPropertyVariable variable) { FireVariableLinkOperationEvent(variable, variableLink, VariableLinkRemoved); })
			;
			return ret;
		}

		#endregion

		#endregion

		#region Events

		#region Variables

		/// <summary>
		/// Generic handler of operations on variable.
		/// </summary>
		/// <param name="variable">Variable operations on which caused an event.</param>
		public delegate void VariableOperationEventHandler(PacketPropertyVariable variable);

		/// <summary>
		/// Occurs when a new variable is added.
		/// </summary>
		public event VariableOperationEventHandler VariableAdded;

		/// <summary>
		/// Occurs when a variable is modified.
		/// </summary>
		public event VariableOperationEventHandler VariableModified;

		/// <summary>
		/// Occurs when a variable is removed.
		/// </summary>
		public event VariableOperationEventHandler VariableRemoved;

		/// <summary>
		/// Fires the variable operation event.
		/// </summary>
		/// <param name="variable">The variable.</param>
		/// <param name="eventHandler">The event handler.</param>
		private void FireVariableOperationEvent(PacketPropertyVariable variable, VariableOperationEventHandler eventHandler)
		{
			if (null != eventHandler)
			{
				eventHandler(variable);
			}
		}

		#endregion

		#region Variable links

		/// <summary>
		/// Generic handler of operations on variable links.
		/// </summary>
		/// <param name="variable">Variable operations on which caused an event.</param>
		/// <param name="link">Variable link operations on which caused an event.</param>
		public delegate void VariableLinkOperationEventHandler(PacketPropertyVariable variable, PacketPropertyVariableLink link);

		/// <summary>
		/// Occurs when a new variable link is added.
		/// </summary>
		public event VariableLinkOperationEventHandler VariableLinkAdded;

		/// <summary>
		/// Occurs when a variable link is modified.
		/// </summary>
		public event VariableLinkOperationEventHandler VariableLinkReplaced;

		/// <summary>
		/// Occurs when a variable link is removed.
		/// </summary>
		public event VariableLinkOperationEventHandler VariableLinkRemoved;

		/// <summary>
		/// Fires the variable operation event.
		/// </summary>
		/// <param name="variable">The variable.</param>
		/// <param name="eventHandler">The event handler.</param>
		private void FireVariableLinkOperationEvent(PacketPropertyVariable variable, PacketPropertyVariableLink variableLink, VariableLinkOperationEventHandler eventHandler)
		{
			if (null != eventHandler)
			{
				eventHandler(variable, variableLink);
			}
		}

		#endregion

		#region Variable value

		public delegate void VariableValueUpdateHandler(PacketPropertyVariable variable, object oldValue, object newValue);

		/// <summary>
		/// Registers the variable value update handler.
		/// </summary>
		/// <param name="variableName">Name of the variable.</param>
		/// <param name="handler">The handler.</param>
		/// <returns></returns>
		public bool RegisterVariableValueUpdateHandler(string variableName, VariableValueUpdateHandler handler)
		{
			bool ret = false;
			try
			{
				VariableEntry entry;
				ret = variableEntriesByName.TryGetValue(variableName, out entry);
				if (ret)
				{
					entry.valueUpdatingEvent += handler;
				}
			}
			catch (Exception e)
			{
				Log.Error("Error", e);
			}
			return ret;
		}

		/// <summary>
		/// Unregisters the variable value update handler.
		/// </summary>
		/// <param name="variableName">Name of the variable.</param>
		/// <param name="handler">The handler.</param>
		/// <returns></returns>
		public bool UnregisterVariableValueUpdateHandler(string variableName, VariableValueUpdateHandler handler)
		{
			bool ret = false;
			try
			{
				VariableEntry entry;
				ret = variableEntriesByName.TryGetValue(variableName, out entry);
				if (ret)
				{
					entry.valueUpdatingEvent -= handler;
				}
			}
			catch (Exception e)
			{
				Log.Error("Error", e);
			}
			return ret;
		}

		#endregion

		#endregion

		#region Packet processing, varible value changes

		/// <summary>
		/// Resets the variables values.
		/// </summary>
		public void ResetVariablesValues()
		{
#warning TODO: Implement
		}

		public void ProcessPacket(Packet packet)
		{
			// Cache type of packet
			Type packetType = packet.GetType();

			// Each variable has multiple links
			variableEntriesList.ForEach(delegate(VariableEntry entry)
			{
				// Variable can link only to single property of a packet hence only single check
				PacketPropertyVariableLink link = GetVariableLinkToPacket(entry.variable, packetType);
				if (null != link)
				{
					// Find first reachable packet property value
					ObjectReflectionProcessor processor = new ObjectReflectionProcessor(delegate(object o)
						{
							// Property value found - save it
							entry.Value = o;

							// Always stop iteration
							return true;
						});
					processor.IterateObjectTreePathValues(packet, link.PacketClassMemberPath.members, PacketPropertyValueFilterForm.RECURSIVE_SEARCH_DEPTH);
				}
			});
		}

		/// <summary>
		/// Gets the variable link to packet.
		/// </summary>
		/// <param name="variable">The variable.</param>
		/// <param name="packetType">Type of the packet.</param>
		/// <returns>Found variable link, or <code>null</code> otherwise.</returns>
		private PacketPropertyVariableLink GetVariableLinkToPacket(PacketPropertyVariable variable, Type packetType)
		{
			PacketPropertyVariableLink ret = null;

			// Check every link of a variable
			IList<PacketPropertyVariableLink> links = variable.Links;
			for (int i = links.Count - 1; 0 <= i; --i)
			{
				PacketPropertyVariableLink link = links[i];
				if (link.PacketClass.type.IsAssignableFrom(packetType))
				{
					// Found link - stop the loop
					ret = link;
					break;
				}
			}

			return ret;
		}

		#endregion
	}
}
