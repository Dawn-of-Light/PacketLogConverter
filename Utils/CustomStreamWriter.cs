using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PacketLogConverter.Utils
{
	/// <summary>
	/// Converts various new line sequences to currently used sequence.
	/// </summary>
	public class CustomStreamWriter : StreamWriter
	{
		private static readonly string DOS_NEW_LINE = "\r\n";

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CustomStreamWriter"/> class.
		/// </summary>
		/// <param name="path">The complete file path to write to.</param>
		/// <param name="append">Determines whether data is to be appended to the file. If the file exists and append is false, the file is overwritten. If the file exists and append is true, the data is appended to the file. Otherwise, a new file is created.</param>
		/// <param name="encoding">The character encoding to use.</param>
		/// <param name="bufferSize">Sets the buffer size.</param>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive. </exception>
		/// <exception cref="T:System.IO.IOException">path includes an incorrect or invalid syntax for file name, directory name, or volume label syntax. </exception>
		/// <exception cref="T:System.ArgumentNullException">path or encoding is null. </exception>
		/// <exception cref="T:System.ArgumentException">path is an empty string (""). -or-path contains the name of a system device (com1, com2, etc).</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <exception cref="T:System.UnauthorizedAccessException">Access is denied. </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">The file cannot be found. </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">bufferSize is negative. </exception>
		/// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. </exception>
		public CustomStreamWriter(string path, bool append, Encoding encoding, int bufferSize) : base(path, append, encoding, bufferSize)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CustomStreamWriter"/> class.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="append">if set to <c>true</c> [append].</param>
		/// <param name="encoding">The encoding.</param>
		public CustomStreamWriter(string path, bool append, Encoding encoding) : base(path, append, encoding)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CustomStreamWriter"/> class.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="append">if set to <c>true</c> [append].</param>
		public CustomStreamWriter(string path, bool append) : base(path, append)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CustomStreamWriter"/> class.
		/// </summary>
		/// <param name="path">The path.</param>
		public CustomStreamWriter(string path) : base(path)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CustomStreamWriter"/> class.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="encoding">The encoding.</param>
		/// <param name="bufferSize">Size of the buffer.</param>
		public CustomStreamWriter(Stream stream, Encoding encoding, int bufferSize) : base(stream, encoding, bufferSize)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CustomStreamWriter"/> class.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="encoding">The encoding.</param>
		public CustomStreamWriter(Stream stream, Encoding encoding) : base(stream, encoding)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:CustomStreamWriter"/> class.
		/// </summary>
		/// <param name="stream">The stream.</param>
		public CustomStreamWriter(Stream stream) : base(stream)
		{
		}


		///<summary>
		///Writes a string to the stream.
		///</summary>
		///
		///<param name="value">The string to write to the stream. If value is null, nothing is written. </param>
		///<exception cref="T:System.ObjectDisposedException"><see cref="P:System.IO.StreamWriter.AutoFlush"></see> is true or the <see cref="T:System.IO.StreamWriter"></see> buffer is full, and current writer is closed. </exception>
		///<exception cref="T:System.NotSupportedException"><see cref="P:System.IO.StreamWriter.AutoFlush"></see> is true or the <see cref="T:System.IO.StreamWriter"></see> buffer is full, and the contents of the buffer cannot be written to the underlying fixed size stream because the <see cref="T:System.IO.StreamWriter"></see> is at the end the stream. </exception>
		///<exception cref="T:System.IO.IOException">An I/O error occurs. </exception><filterpriority>1</filterpriority>
		public override void Write(string value)
		{
			// Fix new line chars
			value = value.Replace(DOS_NEW_LINE, NewLine);
			base.Write(value);
		}

		///<summary>
		///Writes a string followed by a line terminator to the text stream.
		///</summary>
		///
		///<param name="value">The string to write. If value is null, only the line termination characters are written. </param>
		///<exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
		///<exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter"></see> is closed. </exception><filterpriority>1</filterpriority>
		public override void WriteLine(string value)
		{
			// Fix new line chars
			value = value.Replace(DOS_NEW_LINE, NewLine);
			base.WriteLine(value);
		}
	}
}
