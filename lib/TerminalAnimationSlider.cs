namespace timeManager
{
    class TerminalAnimationSlider
    {
        string[] lines;
        int textLength;
        // Time to wait between each print
        int milliseconds;
        System.Diagnostics.Stopwatch updateTimer = new();
        // Index of the left column of the text
        // Begins at the right side of the text
        int leftColumnIndex;
        // Number of milliseconds to wait when whole text is visible
        int wholeTextWaitMilliseconds = 2000;
        System.Diagnostics.Stopwatch wholeTextWaitTimer = new();

        /// <summary>
        /// Create a new slider animation (from left to right)
        /// </summary>
        public TerminalAnimationSlider(string text, int milliseconds)
        {
            this.milliseconds = milliseconds;
            this.lines = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            textLength = lines.Max(line => line.Length);

            // Make all lines the same length
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].PadRight(textLength);
            }

            this.leftColumnIndex = textLength;

            updateTimer.Start();
        }

        public void Print()
        {
            if (leftColumnIndex == 0 || leftColumnIndex <= -textLength + 1)
            {
                wholeTextWaitTimer.Start();

                if (wholeTextWaitTimer.ElapsedMilliseconds < wholeTextWaitMilliseconds)
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (leftColumnIndex == 0)
                            Console.WriteLine(lines[i]);
                        else
                            Console.WriteLine();
                    }
                    return;
                }

                wholeTextWaitTimer.Restart();
            }

            if (updateTimer.ElapsedMilliseconds >= milliseconds)
            {
                updateTimer.Restart();
                --leftColumnIndex;
            }

            if (leftColumnIndex == textLength || leftColumnIndex <= -textLength)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    Console.WriteLine();
                }

                if (leftColumnIndex <= -textLength)
                {
                    leftColumnIndex = textLength;
                }

                return;
            }

            for (int i = 0; i < lines.Length; i++)
            {
                if (leftColumnIndex >= 0)
                {
                    Console.Write(new string(' ', Math.Max(leftColumnIndex - 1, 0)));
                    Console.WriteLine(lines[i].Substring(0, textLength - leftColumnIndex));
                }
                else
                {
                    Console.WriteLine(lines[i].Substring(-leftColumnIndex, textLength + leftColumnIndex));
                }
            }
        }
    }
}
