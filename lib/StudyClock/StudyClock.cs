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
        readonly string totalTimeSpentFile = string.Empty;
        // Nobody have lived for 584942417355 years so far, so ulong is okay
        private ulong totalTime;
        // The time of the current session
        private ulong sessionTime = 0;

        Stopwatch stopwatch;

        readonly string stayHardLogo = @"
        ░██████╗████████╗░█████╗░██╗░░░██╗  ██╗░░██╗░█████╗░██████╗░██████╗░
        ██╔════╝╚══██╔══╝██╔══██╗╚██╗░██╔╝  ██║░░██║██╔══██╗██╔══██╗██╔══██╗
        ╚█████╗░░░░██║░░░███████║░╚████╔╝░  ███████║███████║██████╔╝██║░░██║
        ░╚═══██╗░░░██║░░░██╔══██║░░╚██╔╝░░  ██╔══██║██╔══██║██╔══██╗██║░░██║
        ██████╔╝░░░██║░░░██║░░██║░░░██║░░░  ██║░░██║██║░░██║██║░░██║██████╔╝
        ╚═════╝░░░░╚═╝░░░╚═╝░░╚═╝░░░╚═╝░░░  ╚═╝░░╚═╝╚═╝░░╚═╝╚═╝░░╚═╝╚═════╝░";

        TerminalAnimationSlider slider;
        // TODO: Add settings to the constructor, so that the user can select the course folder
        public StudyClock(string courseFolder = "default")
        {
            stopwatch = new();

            slider = new(stayHardLogo, 49);

            // TODO
            // Idea: Make a selector which could be controlled with arrow keys
            string? courseName;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter course code: ");
                courseName = Console.ReadLine();

                if (courseName == null || courseName.Length == 0)
                {
                    TerminalSelector.Select(
                        new string[] {
                            "OK",
                        },
                        "Course name cannot be empty\n"
                    );
                }
                else if (courseName.Contains(","))
                {
                    TerminalSelector.Select(
                        new string[] {
                            "OK",
                        },
                        "Course name cannot contain a comma\n"
                    );
                }
                else
                {
                    break;
                }
            }

            this.courseName = courseName;
            if (courseFolder.Equals("default"))
                // If no directory is selected, use the current one
                // TODO: Change with settings
                this.courseFolder = "data/" + courseName; 

            // The name of the course file is the current date
            courseFile = this.courseFolder + $"/{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
            totalTimeSpentFile = this.courseFolder + "/totalTimeSpent.txt";
        }

        public void Start(bool animatedMode = false)
        {
            if (!Directory.Exists(courseFolder))
            {
                if (GetAnswer("Course not found. " + 
                    "Do you want to start a timer on a new course? (y/n)") == "n")
                {
                    return;
                }
                Console.WriteLine("Making new course directory...");

                DirectoryHandler.MakeNewDirectory(courseFolder);
            }

            if (!File.Exists(totalTimeSpentFile))
            {
                DirectoryHandler.MakeFileInDirectory(totalTimeSpentFile, "0");
            }

            if (!File.Exists(courseFile))
            {
                DirectoryHandler.MakeFileInDirectory(courseFile, "0");
            }

            ParseTotalTime();

            stopwatch.Start();

            if (animatedMode)
                RunClockAnimated();
            else
                RunClock();
        }

        void RunClock()
        {
            ClockState clockState = ClockState.Running;

            while (true)
            {
                UpdateConsole(clockState);

                clockState = HandleInput(Console.ReadKey(true));

                if (clockState == ClockState.End) return;
            }
        }

        void RunClockAnimated()
        {
            ClockState clockState = ClockState.Running;

            Stopwatch consoleUpdateTimer = new();
            consoleUpdateTimer.Start();

            while (true)
            {
                if (consoleUpdateTimer.ElapsedMilliseconds > 50)
                {
                    consoleUpdateTimer.Restart();
                    UpdateConsole(clockState, animatedMode: true);
                }

                if (Console.KeyAvailable)
                {
                    // True to not display the pressed key
                    clockState = HandleInput(Console.ReadKey(true));
                }

                if (clockState == ClockState.End) return;
            }
        }

        void UpdateConsole(ClockState clockState, bool animatedMode = false)
        {
            Console.Clear();
            Console.WriteLine(
                $"Total study time on course {courseName} is {GetTimeAsString()}\n\n" +
                 "q to exit, p to pause stopwatch, any key to resume\n\n\n\n" +
                 $"Current session: {stopwatch.Elapsed.ToString().Substring(0, 8)}\n"
            );

            if (clockState == ClockState.Stopped)
            {
                Console.Write("Clock is ");
                ConsoleWriter.Write("paused", ConsoleColor.White, ConsoleColor.Red);
                Console.WriteLine("!");
            }
            else if (clockState == ClockState.Running)
            {
                Console.Write("Clock is ");
                ConsoleWriter.Write("running", ConsoleColor.Green);
                Console.WriteLine("!");
            }

            Console.WriteLine();

            if (animatedMode)
                slider.Print();
            else
                Console.WriteLine(stayHardLogo);
        }

        private void UpdateTotalTime()
        {
            totalTime += Convert.ToUInt64(stopwatch.ElapsedMilliseconds / 1000);
        }

        private void UpdateSessionTime()
        {
            sessionTime = Convert.ToUInt64(stopwatch.ElapsedMilliseconds / 1000);
        }

        ClockState HandleInput(ConsoleKeyInfo userInput)
        {
            UpdateSessionTime();

            switch (userInput.Key.ToString().ToLower())
            {
                case "q":
                    stopwatch.Stop();
                    UpdateTotalTime();
                    DirectoryHandler.SaveTotalTime(totalTimeSpentFile, totalTime.ToString());
                    DirectoryHandler.AddToTime(courseFile, sessionTime);

                    CalendarSaver.SaveCourseToday(courseName);

                    return ClockState.End;

                case "p":
                    stopwatch.Stop();
                    return ClockState.Stopped;

                default:
                    stopwatch.Start();
                    return ClockState.Running;
            }
        }

        private string GetTimeAsString()
        {
            // Transfer totalTime (in seconds) to a TimeSpan
            TimeSpan timeSpan = TimeSpan.FromSeconds(totalTime + sessionTime);
            return $"Days: {timeSpan.Days}, Hours: {timeSpan.Hours}, " + 
                    $"Minutes: {timeSpan.Minutes}, Seconds: {timeSpan.Seconds}";
            // return $"{timeSpan.Days:D2}:{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }

        private string SecondsToTimeString(ulong seconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            return $"{timeSpan.Days:D2}:{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }

        void ParseTotalTime()
        {
            string? totalTimeStr = DirectoryHandler.ReadLineFromFile(totalTimeSpentFile);

            if (totalTimeStr == null)
            {
                throw new Exception("Error reading from file");
            }

            totalTime = Convert.ToUInt64(totalTimeStr);
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
