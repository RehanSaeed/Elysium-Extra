namespace Framework.IO
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// <see cref="Stream"/> extension methods.
    /// </summary>
    public static class Streams
    {
        #region ReadAllBytes

        /// <summary>
        /// Reads data from a <see cref="Stream"/> until the end is reached.
        /// </summary>
        /// <param name="stream">The stream to read data from.</param>
        /// <returns>All bytes from the stream.</returns>
        public static byte[] ReadAllBytes(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            return ReadAllBytes(stream, stream.Length);
        }

        /// <summary>
        /// Reads data from a stream until the end is reached.
        /// </summary>
        /// <param name="stream">The stream to read data from.</param>
        /// <param name="length">The length of the stream.</param>
        /// <returns>All bytes from the stream.</returns>
        public static byte[] ReadAllBytes(this Stream stream, long length)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (!stream.CanRead)
            {
                throw new ArgumentException("Cannot read from stream.", "stream");
            }

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            byte[] buffer = new byte[length];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;

                // If end of buffer reached, check to see if there's any more information.
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();

                    // If end of stream.
                    if (nextByte == -1)
                    {
                        return buffer;
                    }

                    // Else resize the buffer, put in the byte we've just read, and continue.
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }

            // Buffer is now too big. Shrink it.
            byte[] bytes = new byte[read];
            Array.Copy(buffer, bytes, read);
            return bytes;
        }

        #endregion

        #region ReadAllText

        /// <summary>
        /// Reads all text from the stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The text read from the stream.</returns>
        public static string ReadAllText(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            StreamReader streamReader = new StreamReader(stream);

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            return streamReader.ReadToEnd();
        }

        /// <summary>
        /// Reads all text from a stream with the given encoding.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="encoding">The text encoding.</param>
        /// <returns>The text read from the stream.</returns>
        public static string ReadAllText(this Stream stream, Encoding encoding)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (!stream.CanRead)
            {
                throw new ArgumentException("Cannot read from stream.", "stream");
            }

            StreamReader streamReader = new StreamReader(stream, encoding);

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            return streamReader.ReadToEnd();
        }

        #endregion

        #region WriteAllBytes

        /// <summary>
        /// Writes all bytes to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream to write data to.</param>
        /// <param name="value">The bytes to write.</param>
        public static void WriteAllBytes(this Stream stream, byte[] value)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (!stream.CanWrite)
            {
                throw new ArgumentException("Cannot write to stream.", "stream");
            }

            stream.Write(value, 0, value.Length);
        }

        #endregion

        #region WriteAllText

        /// <summary>
        /// Writes all specified text to a stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="text">The text to write.</param>
        public static void WriteAllText(this Stream stream, string text)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (!stream.CanWrite)
            {
                throw new ArgumentException("Cannot write to stream.", "stream");
            }

            if (!string.IsNullOrEmpty(text))
            {
                StreamWriter streamWriter = new StreamWriter(stream);

                if (stream.CanSeek)
                {
                    stream.Position = 0;
                }

                streamWriter.Write(text);
                streamWriter.Flush();
            }
        }

        /// <summary>
        /// Writes all specified text to a stream with the given encoding.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="text">The text to write.</param>
        /// <param name="encoding">The text encoding.</param>
        public static void WriteAllText(this Stream stream, string text, Encoding encoding)
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (!stream.CanWrite)
            {
                throw new ArgumentException("Cannot write to stream.", "stream");
            }

            if (!string.IsNullOrEmpty(text))
            {
                StreamWriter streamWriter = new StreamWriter(stream, encoding);

                if (stream.CanSeek)
                {
                    stream.Position = 0;
                }

                streamWriter.Write(text);
                streamWriter.Flush();
            }
        }

        #endregion
    }
}
