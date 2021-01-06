using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExtJsNamespaceDeployer
{
    /// <summary>
    /// Modified copypaste from https://stackoverflow.com/questions/520722/unbuffered-streamreader
    /// This in case default steamreader not suitable for our purposes
    /// This reader can only read seekable streams.
    /// </summary>
    public class UnbufferedStreamReader : TextReader
    {
        private Stream _stream;

        public Stream BaseStream => _stream;

        public bool EndOfStream => _stream.Position == _stream.Length;

        public UnbufferedStreamReader(string path)
        {
            _stream = new FileStream(path, FileMode.Open);
        }

        public UnbufferedStreamReader(Stream stream)
        {
            if (!stream.CanSeek)
            {
                throw new ArgumentException("This reader can only read seekable streams.");
            }

            _stream = stream;
        }

        // This method assumes lines end with a line feed.
        // You may need to modify this method if your stream
        // follows the Windows convention of \r\n or some other 
        // convention that isn't just \n
        public override string ReadLine()
        {
            List<byte> bytes = new List<byte>();
            int current;
            while ((current = Read()) != -1 && current != (int)'\n')
            {
                byte b = (byte)current;
                bytes.Add(b);
            }
            return Encoding.ASCII.GetString(bytes.ToArray());
        }

        public override int Read()
        {
            return _stream.ReadByte();
        }

        public override void Close()
        {
            _stream.Close();
        }
        protected override void Dispose(bool disposing)
        {
            _stream.Dispose();
        }

        public override int Peek()
        {
            var value = _stream.ReadByte();
            _stream.Seek(-1, SeekOrigin.Current);
            return value;
        }

        public override int Read(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override int ReadBlock(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override string ReadToEnd()
        {
            throw new NotImplementedException();
        }
    }
}
