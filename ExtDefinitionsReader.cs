using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ExtJsNamespaceDeployer
{
    class ExtDefinitionsReader
    {
        private FileStream _fileStream;
        private UnbufferedStreamReader _reader;

        public ExtDefinitionsReader(string filePath)
        {
            if(Path.GetExtension(filePath) != ".js")
            {
                throw new ArgumentException("File must be a javascript file.");
            }

            _fileStream = File.OpenRead(filePath);
            _reader = new UnbufferedStreamReader(_fileStream);
        }

        public string ReadNextDefinition()
        {
            var defineSkip = _reader.ReadToFirstMatch("Ext.define");

            // Reading to bracket && reading to closing definition && reading ';'
            var toOpeningBracket = _reader.ReadToFirstMatch("(");
            var toClosingBracket = _reader.ReadToClosingBracket(excludeBrackets: '<');
            var lastChar = (char)_reader.Read();
            var classDefine = $"{toOpeningBracket}{toClosingBracket}{lastChar}";
            return classDefine;
        }
    }
}
