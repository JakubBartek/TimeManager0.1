using System;

namespace timeManager
{
    public class Calendar
    {
        // The selected day, month and year
        // At the beginning, it's set to the current date
        private int selectedDay;
        private int selectedMonth;
        private int selectedYear;

        private int daysInSelectedMonth;
        // Index of the starting day of the month
        // (0 - Monday, 1 - Tuesday, ..., 6 - Sunday)
        private int startDayIndex;

        private int selectedRowIndex;
        private int selectedColumnIndex;

        // Number of rows in the calendar
        private int numOfRows;

        // The bottom message to be displayed
        private static string bottomMessage = "";

        public Calendar()
        {
            selectedDay = DateTime.Now.Day;
            selectedMonth = DateTime.Now.Month;
            selectedYear = DateTime.Now.Year;

            UpdateSelectionVariables();
        }

        private void UpdateSelectionVariables()
        {
            daysInSelectedMonth = GetDaysInMonth(selectedYear, selectedMonth);
            startDayIndex = GetStartDayIndex(selectedYear, selectedMonth);
            UpdateRowIndexAndColumnIndex();
            numOfRows = GetNumberOfRows(startDayIndex, daysInSelectedMonth);
        }

        private int GetDaysInMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }

        private int GetNumberOfRows(int startDayIndex, int daysInSelectedMonth)
        {
            return 
                (startDayIndex == 5 && daysInSelectedMonth == 31) 
                || (startDayIndex == 6 && daysInSelectedMonth >= 30) ? 6 : 5;
        }

        private int GetStartDayIndex(int year, int month)
        {
            return (int)new DateTime(year, month, 1).DayOfWeek;
        }

        private void UpdateRowIndexAndColumnIndex()
        {
            selectedRowIndex = GetSelectedRowIndex(selectedDay, startDayIndex);
            selectedColumnIndex = GetSelectedColumnIndex(selectedDay, startDayIndex);
        }

        private int GetSelectedRowIndex(int selectedDay, int startDayIndex)
        {
            return (selectedDay + startDayIndex - 1) / 7;
        }

        private int GetSelectedColumnIndex(int selectedDay, int startDayIndex)
        {
            return (selectedDay + startDayIndex - 1) % 7;
        }

        private void PrintDebug()
        {
            Console.WriteLine("Selected day: " + selectedDay);
            Console.WriteLine("Selected month: " + selectedMonth);
            Console.WriteLine("Selected year: " + selectedYear);
            Console.WriteLine("Days in selected month: " + daysInSelectedMonth);
            Console.WriteLine("Start day index: " + startDayIndex);
            Console.WriteLine("Selected row index: " + selectedRowIndex);
            Console.WriteLine("Selected column index: " + selectedColumnIndex);
            Console.WriteLine("Number of rows: " + numOfRows);
        }

        public void Print()
        {
            // Get the name of the month
            string monthName = new DateTime(selectedYear, selectedMonth, 1).ToString("MMMM");

            var calendarMonthParser = new CalendarMonthParser(selectedYear, selectedMonth);
            Dictionary<int, ulong> dayAndTotalTimeSpent =
                calendarMonthParser.GetDayAndTotalTimeSpent();

            Console.WriteLine(" " + monthName + " " + selectedYear.ToString());

            Console.WriteLine(" ┌───┬───┬───┬───┬───┬───┬───┐");
            Console.WriteLine(" │ Mo│ Tu│ We│ Th│ Fr│ Sa│ Su│");
            Console.WriteLine(" ├───┼───┼───┼───┼───┼───┼───┤");

            // If the first day of the month is Sunday and we have 
            // at least 30 days in the month,
            // we need 6 rows to display all the days
            numOfRows = GetNumberOfRows(startDayIndex, daysInSelectedMonth);

            for (int i = 0; i < numOfRows; i++)
            {
                Console.Write(" ");

                for (int j = 0; j < 7; j++)
                {
                    int day = i * 7 + j + 1 - startDayIndex;
                    if (day <= 0 || day > daysInSelectedMonth)
                    {
                        Console.Write("│   ");
                    }
                    else
                    {
                        Console.Write("│");

                        // Highlight day if the user spent time on it
                        if (dayAndTotalTimeSpent.ContainsKey(day)
                            && dayAndTotalTimeSpent[day] > 0)
                        {
                            // TODO: Change the threshold (it's in seconds)
                            if (dayAndTotalTimeSpent[day] <= 2)
                            {
                                Console.BackgroundColor = ConsoleColor.Green;
                                Console.ForegroundColor = ConsoleColor.Black;
                            }
                            else
                            {
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }

                        if (day == selectedDay)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        Console.Write($" {day,2}");

                        Console.ResetColor();
                    }
                }
                Console.WriteLine("│");
                if (i < numOfRows - 1)
                    Console.WriteLine(" ├───┼───┼───┼───┼───┼───┼───┤");
            }

            Console.WriteLine(" └───┴───┴───┴───┴───┴───┴───┘");

            PrintKeyPressInfo();
        }

        private void PrintKeyPressInfo()
        {
            Console.WriteLine("Arrow keys to navigate the calendar");
            Console.WriteLine("Enter to look at the selected day");
            Console.WriteLine("A to go to the previous month");
            Console.WriteLine("D to go to the next month");
            Console.WriteLine("F to search for a specific date");
            Console.WriteLine("Q to exit\n");
        }

        /// <summary>
        /// Handle key press
        /// </summary>
        /// <param name="keyInfo"> Key info </param>
        public void HandleKeyPress(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    selectedDay = Math.Max(selectedDay - 7, 1);
                    UpdateRowIndexAndColumnIndex();
                    break;

                case ConsoleKey.DownArrow:
                    selectedDay = Math.Min(selectedDay + 7, daysInSelectedMonth);
                    UpdateRowIndexAndColumnIndex();
                    break;

                case ConsoleKey.LeftArrow:
                    selectedDay = Math.Max(selectedDay - 1, 1);
                    UpdateRowIndexAndColumnIndex();
                    break;

                case ConsoleKey.RightArrow:
                    selectedDay = Math.Min(selectedDay + 1, daysInSelectedMonth);
                    UpdateRowIndexAndColumnIndex();
                    break;
                
                // Go to the previous month
                case ConsoleKey.A:
                    MoveMonth(-1);
                    UpdateSelectionVariables();
                    break;

                // Go to the next month
                case ConsoleKey.D:
                    MoveMonth(+1);
                    UpdateSelectionVariables();
                    break;

                // Search for a specific date
                case ConsoleKey.F:
                    int day, month, year;
                    GetDateInputFromUser(out day, out month, out year);
                    selectedDay = day;
                    selectedMonth = month;
                    selectedYear = year;
                    UpdateSelectionVariables();
                    break;

                case ConsoleKey.Enter:
                    bottomMessage = "Enter TODO";
                    break;
            }
        }

        private void MoveMonth(int monthOffset)
        {
            selectedMonth += monthOffset;
            if (selectedMonth > 12)
            {
                selectedMonth -= 12;
                selectedYear++;
            }
            else if (selectedMonth < 1)
            {
                selectedMonth += 12;
                selectedYear--;
            }
        }

        private bool GetDateInputFromUser(out int day, out int month, out int year)
        {
            Console.WriteLine("Enter the day:");
            string? dayStr = Console.ReadLine();
            Console.WriteLine("Enter the month:");
            string? monthStr = Console.ReadLine();
            Console.WriteLine("Enter the year:");
            string? yearStr = Console.ReadLine();

            if (!int.TryParse(dayStr, out day))
            {
                Console.WriteLine("Invalid day");
                month = 0;
                year = 0;
                return false;
            }

            if (!int.TryParse(monthStr, out month))
            {
                Console.WriteLine("Invalid month");
                year = 0;
                return false;
            }

            if (!int.TryParse(yearStr, out year))
            {
                Console.WriteLine("Invalid year");
                return false;
            }

            return true;
        }

        public static void Run()
        {
            Console.CursorVisible = false;
            var calendar = new Calendar();
            ConsoleKeyInfo keyInfo;
            do
            {
                Console.Clear();
                calendar.Print();
                Console.WriteLine(bottomMessage);

                keyInfo = Console.ReadKey(true);
                calendar.HandleKeyPress(keyInfo);

            } while (keyInfo.Key != ConsoleKey.Q);
        }
    }
}