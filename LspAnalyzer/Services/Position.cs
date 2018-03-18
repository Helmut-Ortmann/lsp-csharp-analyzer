namespace LspAnalyzer.Services
{
    public class Position
    {
        public int Line { get; set; }
        public int Character { get; set; }

        public Position(int line, int character)
        {
            Line = line;
            Character = character;
        }
    }
}
