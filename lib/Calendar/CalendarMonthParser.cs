using System;

namespace timeManager
{
    /// <summary>
    /// Class to parse the month file
    /// </summary>
    public class CalendarMonthParser
    {
        public string CalendarDirectory = 
            Path.Join(Directory.GetCurrentDirectory(), "data", "calendar");

        public int CurrentYear { get; set; }
        public int CurrentMonth { get; set; }

        public CalendarMonthParser(int year, int month)
        {
            CurrentYear = year;
            CurrentMonth = month;
        }

        public CalendarMonthParser()
        {
            CurrentYear = DateTime.Now.Year;
            CurrentMonth = DateTime.Now.Month;
        }
        
        private string[] GetLines()
        {
            string fileName = Path.Join(CalendarDirectory,
                                        $"{CurrentYear}-{CurrentMonth:00}.txt");

            string[] lines;
            if (!File.Exists(fileName))
            {
                // Do not create the file if it does not exist
                lines = new string[0];
            }
            else
            {
                lines = File.ReadAllLines(fileName);
            }

            // Resize the array to the number of days in the month
            ResizeLines(ref lines, DateTime.DaysInMonth(CurrentYear, CurrentMonth));

            return lines;
        }

        private void ResizeLines(ref string[] lines, int day)
        {
            if (lines.Length < day)
            {
                Array.Resize(ref lines, day);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i] == null)
                    {
                        lines[i] = "";
                    }
                }
            }
        }

        private string[] GetCourses(string[] lines, int day)
        {
            return lines[day - 1].Split(',');
        }

        /// <summary>
        /// Get a dictionary containing day and total time spent on all courses
        /// </summary>
        /// <returns> Dictionary containing day and total time spent on all courses </returns>
        public Dictionary<int, ulong> GetDayAndTotalTimeSpent()
        {
            string[] lines = GetLines();
            Dictionary<int, ulong> dayAndTotalTimeSpent = new Dictionary<int, ulong>();

            for (int day = 1; day <= lines.Length; day++)
            {
                string[] courses = GetCourses(lines, day);
                ulong totalTimeSpent = 0;

                foreach (string course in courses)
                {
                    totalTimeSpent += CourseTimeSpent(course, day);
                }

                dayAndTotalTimeSpent.Add(day, totalTimeSpent);
            }

            return dayAndTotalTimeSpent;
        }

        private ulong CourseTimeSpent(string courseName, int day)
        {
            if (courseName == "")
            {
                return 0;
            }

            return StudyClockParser.ParseCourseTimeSpent(
                courseName, CurrentYear, CurrentMonth, day);
        }

        public static void DebugPrint()
        {
            CalendarMonthParser cmp = new();
            var dict =
                cmp.GetDayAndTotalTimeSpent();
            
            foreach (var item in dict)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }
    }
}