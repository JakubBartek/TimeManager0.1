using System;

namespace timeManager
{
    public class CalendarSaver
    {
        public static string CalendarDirectory = 
            Path.Join(Directory.GetCurrentDirectory(), "data", "calendar");

        /// <summary>
        /// Save the name of the course that the user is taking today
        /// The file name is the current year and month (YYYY-MM.txt)
        /// </summary>
        /// <param name="courseName"> Name of the course </param>
        public static void SaveCourseToday(string courseName)
        {
            if (!Directory.Exists(CalendarDirectory))
            {
                Directory.CreateDirectory(CalendarDirectory);
            }

            AddCourseToDay(courseName, DateTime.Now.Day);
        }

        /// <summary>
        /// Add a course to a specific day
        /// Put the course name in the month file into
        /// the (day - 1)th line. The courses are separated by a comma
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="day"></param>
        private static void AddCourseToDay(string courseName, int day)
        {
            string fileName = Path.Join(
                CalendarDirectory, $"{DateTime.Now.ToString("yyyy-MM")}.txt");

            if (!File.Exists(fileName))
            {
                using (File.Create(fileName)) { }
            }

            string[] lines = File.ReadAllLines(fileName);

            if (lines.Length < day)
            {
                Array.Resize(ref lines, day);
                // Fill the new lines with empty strings
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i] == null)
                    {
                        lines[i] = "";
                    }
                }
            }

            if (CourseInLine(courseName, lines[day - 1]))
                return;

            // If non-empty, add a comma
            if (lines[day - 1] != "")
                lines[day - 1] += ",";

            lines[day - 1] += courseName;

            File.WriteAllLines(fileName, lines);
        }

        private static bool CourseInLine(string courseName, string line)
        {
            return line.Contains(courseName);
        }
    }
}  