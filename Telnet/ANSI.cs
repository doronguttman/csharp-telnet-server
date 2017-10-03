namespace TelnetServer.Telnet
{
    public static class ANSI
    {
        // based on: http://www.isthe.com/chongo/tech/comp/ansi_escapes.html

        public static readonly string EscapeSequence = $"{(char)27}[";

        public static class Cursor
        {
            public static class Move
            {
                public static string To(int row, int col) => $"{EscapeSequence}{row};{col}H";
                public static string Up(int rows = 1) => $"{EscapeSequence}{rows}A";
                public static string Down(int rows = 1) => $"{EscapeSequence}{rows}B";
                public static string Forward(int spaces = 1) => $"{EscapeSequence}{spaces}C";
                public static string Back(int spaces = 1) => $"{EscapeSequence}{spaces}D";
            }

            public static class Position
            {
                //        public static string Report() => $"{EscapeSequence}#;#R";
                public static string Save { get; } = $"{EscapeSequence}s";
                public static string Return { get; } = $"{EscapeSequence}u";
            }
        }
        
        public static class Erase
        {
            public static string Screen { get; } = $"{EscapeSequence}2J";
            public static string EndOfLine { get; } = $"{EscapeSequence}K";
        }
    }
}
