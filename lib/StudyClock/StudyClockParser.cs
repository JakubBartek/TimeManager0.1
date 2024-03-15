using System.Diagnostics;

namespace timeManager
{
    public static class StudyClockParser
    {
        public static ulong 
            ParseCourseTimeSpent(string courseName, int year, int month, int day)
        {
            // TODO: make general data directory
            string courseFile = Path.Join(
                Directory.GetCurrentDirectory(), "data", courseName,
                $"{year}-{month:00}-{day:00}.txt");
            
            if (!File.Exists(courseFile))
            {
                return 0;
            }

            string timeSpent = File.ReadAllText(courseFile);
            return Convert.ToUInt64(timeSpent);
        }
    }
}