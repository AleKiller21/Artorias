using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LexerAnalyser.Interfaces;
using LexerAnalyser.Models;

namespace LexerAnalyser
{
    public class FileInputStream : IInputStream
    {
        private StreamReader _reader;
        private int _currentRow;
        private int _currentColumn;

        public FileInputStream(string file)
        {
            _reader = new StreamReader(new FileStream(file, FileMode.Open));
            _currentRow = 1;
            _currentColumn = 1;
        }

        public Symbol GetNextSymbol()
        {
            if (!_reader.EndOfStream)
            {
                var character = (char)_reader.Read();

                switch (character)
                {
                    case '\n':
                        _currentRow++;
                        _currentColumn = 1;
                        return new Symbol(_currentRow, _currentColumn, character);
                    case '\r':
                        return new Symbol(_currentRow, _currentColumn, character);
                }

                return new Symbol(_currentRow, _currentColumn++, character);
            }

            _reader.Dispose();
            return new Symbol(_currentRow, _currentColumn, '$');
        }
    }
}
