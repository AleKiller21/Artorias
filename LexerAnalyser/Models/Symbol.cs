namespace LexerAnalyser.Models
{
    internal class Symbol
    {
        public readonly int RowCount;
        public readonly int ColCount;
        public readonly char Character;

        public Symbol(int rowCount, int colCount, char character)
        {
            RowCount = rowCount;
            ColCount = colCount;
            Character = character;
        }
    }
}
