using System.Diagnostics;

namespace timeManager
{
    public enum ClockState
    {
        Running,
        Stopped,
        End
    }
    class StudyClock
    {
        readonly string courseName = string.Empty;
        readonly string courseFolder = string.Empty;
        readonly string courseFile = string.Empty;
        string totalTime = string.Empty;

        public StudyClock(string courseFolder = "default")
        {
            Console.Clear();
            Console.WriteLine("Enter course code: ");
            string? courseName = Console.ReadLine();

            this.courseName = courseName ??= "default";
            if (courseFolder.Equals("default")) this.courseFolder = Directory.GetCurrentDirectory() + courseName; // if no directory is selected use the current one
            courseFile = this.courseFolder + $"\\{courseName}.txt";
        }

        public void Start()
        {
            if (!Directory.Exists(courseFolder))
            {
                if (GetAnswer("Course not found. Do you want to start a timer on a new course? (y/n)") == "n")
                {
                    return;
                }
                Console.WriteLine("Making new course directory...");

                DirectoryHandler.MakeNewDirectory(courseFolder);
                DirectoryHandler.MakeFileInDirectory(courseFile, "00:00:00");
            }
            GetTotalTime();
            RunClock();
        }

        void RunClock()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            ClockState clockState = ClockState.Running;
            string ui;

            while (true)
            {
                Console.Clear();
                ui = $"Total study time on course {courseName} is {totalTime}\n" +
                      "q to exit, p to pause stopwatch, r to resume, any key to update time \n\n\n\n\n" +
                      $"Current session: {stopwatch.Elapsed.ToString().Substring(0, 8)}\n";

                if (clockState == ClockState.Stopped) ui += "Clock is stopped!\n";
                if (clockState == ClockState.Running) ui += "Clock is running!\n";

                ui += @"
                                    ░██████╗████████╗░█████╗░██╗░░░██╗  ██╗░░██╗░█████╗░██████╗░██████╗░
                                    ██╔════╝╚══██╔══╝██╔══██╗╚██╗░██╔╝  ██║░░██║██╔══██╗██╔══██╗██╔══██╗
                                    ╚█████╗░░░░██║░░░███████║░╚████╔╝░  ███████║███████║██████╔╝██║░░██║
                                    ░╚═══██╗░░░██║░░░██╔══██║░░╚██╔╝░░  ██╔══██║██╔══██║██╔══██╗██║░░██║
                                    ██████╔╝░░░██║░░░██║░░██║░░░██║░░░  ██║░░██║██║░░██║██║░░██║██████╔╝
                                    ╚═════╝░░░░╚═╝░░░╚═╝░░╚═╝░░░╚═╝░░░  ╚═╝░░╚═╝╚═╝░░╚═╝╚═╝░░╚═╝╚═════╝░";

                Console.WriteLine(ui);
                clockState = HandleInput(Console.ReadLine(), stopwatch);

                if (clockState == ClockState.End) return;
            }
        }

        ClockState HandleInput(string? userInput, Stopwatch stopwatch)
        {
            switch (userInput)
            {
                case "q":
                    stopwatch.Stop();
                    totalTime = AddSecondsToTime((int)stopwatch.Elapsed.TotalSeconds);
                    SaveTotalTime();
                    return ClockState.End;

                case "p":
                    stopwatch.Stop();
                    return ClockState.Stopped;

                default:
                    stopwatch.Start();
                    return ClockState.Running;
            }
        }

        void SaveTotalTime()
        {
            try
            {
                StreamWriter sw = new(courseFile);
                sw.WriteLine(totalTime);
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing in file: {ex.Message}");
            }
        }

        string AddSecondsToTime(int secondsToAdd)
        {
            // Parse the existing time string to a TimeSpan
            if (TimeSpan.TryParse(totalTime, out TimeSpan currentTime))
            {
                // Add seconds to the existing time
                TimeSpan updatedTime = currentTime.Add(TimeSpan.FromSeconds(secondsToAdd));

                // Format the updated time as hh:mm:ss
                string formattedTime = $"{updatedTime.Hours:D2}:{updatedTime.Minutes:D2}:{updatedTime.Seconds:D2}";

                return formattedTime;
            }
            else
            {
                throw new ArgumentException("Invalid time format. Expected format: hh:mm:ss");
            }
        }

        void GetTotalTime()
        {
            string? totalTime = "";
            try
            {
                StreamReader sr = new(courseFile);
                totalTime = sr.ReadLine();
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            this.totalTime = totalTime ?? ""; // Use null-conditional operator to assign an empty string if totalTime is null
        }

        static string GetAnswer(string message)
        {
            Console.WriteLine(message);
            string? answer;
            while (true)
            {
                answer = Console.ReadLine();
                if (answer == "y" || answer == "n") return answer;
                Console.WriteLine("Try again..." + message);
            }
        }
    }
}
