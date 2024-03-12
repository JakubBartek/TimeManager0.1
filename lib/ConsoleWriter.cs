namespace timeManager
{
    class ConsoleWriter
    {
        public static void Write(string text)
        {
            Console.Write(text);
        }

        public static void Write(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void Write(string text, ConsoleColor color, ConsoleColor background)
        {
            Console.ForegroundColor = color;
            Console.BackgroundColor = background;
            Console.Write(text);
            Console.ResetColor();
        }

        public static void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public static void WriteLine(string text, ConsoleColor color)
        {
            Write(text + "\n", color);
        }

        public static void WriteLine(string text, ConsoleColor color, ConsoleColor background)
        {
            Write(text + "\n", color, background);
        }
    }    
}