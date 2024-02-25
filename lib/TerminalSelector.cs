namespace timeManager
{
    class TerminalSelector
    {
        /// <summary>
        /// Select an option from a list of options
        /// </summary>
        /// <param name="options"> List of options </param>
        /// <param name="printBefore"> Text to print before the options </param>
        /// <param name="printAfter"> Text to print after the options </param>
        /// <returns Index of the selected option </returns>
        public static int Select(string[] options, string printBefore = "", string printAfter = "")
        {
            int selected = 0;
            ConsoleKeyInfo key;
            do
            {
                Console.Clear();
                Console.WriteLine(printBefore);

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(options[i]);
                    Console.ResetColor();
                }

                Console.WriteLine(printAfter);
                key = WaitForUserKey();

                if (key.Key == ConsoleKey.DownArrow)
                {
                    selected++;
                    if (selected == options.Length) selected = 0;
                }
                else if (key.Key == ConsoleKey.UpArrow)
                {
                    selected--;
                    if (selected == -1) selected = options.Length - 1;
                }
            } while (key.Key != ConsoleKey.Enter);
            return selected;
        }

        // This method is nicer than ReadLine() because 
        // it doesn't require the user to press enter
        public static ConsoleKeyInfo WaitForUserKey()
        {
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    return Console.ReadKey(true);
                }
            }
        }
    }
}