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
        private readonly StreamReader _reader;
        private int _currentRow;
        private int _currentColumn;
        private bool _finished;

        public FileInputStream(string file)
        {
            _reader = new StreamReader(new FileStream(file, FileMode.Open));
            _finished = false;
            _currentRow = 1;
            _currentColumn = 1;
        }

        public Symbol GetNextSymbol()
        {
            if(_finished) return new Symbol(_currentRow, _currentColumn, '\0');

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
            _finished = true;
            return new Symbol(_currentRow, _currentColumn, '\0');
        }
    }
}
