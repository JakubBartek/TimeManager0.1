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
        // TODO: Save totalTime as ulong in seconds instead of string
        // // Nobody have lived for 584942417355 years so far, so ulong is okay
        // ulong totalSeconds = 0;

        readonly string stayHardLogo = @"
        ░██████╗████████╗░█████╗░██╗░░░██╗  ██╗░░██╗░█████╗░██████╗░██████╗░
        ██╔════╝╚══██╔══╝██╔══██╗╚██╗░██╔╝  ██║░░██║██╔══██╗██╔══██╗██╔══██╗
        ╚█████╗░░░░██║░░░███████║░╚████╔╝░  ███████║███████║██████╔╝██║░░██║
        ░╚═══██╗░░░██║░░░██╔══██║░░╚██╔╝░░  ██╔══██║██╔══██║██╔══██╗██║░░██║
        ██████╔╝░░░██║░░░██║░░██║░░░██║░░░  ██║░░██║██║░░██║██║░░██║██████╔╝
        ╚═════╝░░░░╚═╝░░░╚═╝░░╚═╝░░░╚═╝░░░  ╚═╝░░╚═╝╚═╝░░╚═╝╚═╝░░╚═╝╚═════╝░";

        TerminalAnimationSlider slider;
        public StudyClock(string courseFolder = "default")
        {
            slider = new(stayHardLogo, 49);

            // TODO
            // Idea: Make a selector which could be controlled with arrow keys
            string? courseName;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter course code: ");
                courseName = Console.ReadLine();

                if (courseName != null && courseName.Length > 0) break;

                TerminalSelector.Select(
                    new string[] {
                        "OK",
                    },
                    "Course name cannot be empty\n"
                );
            }

            this.courseName = courseName;
            if (courseFolder.Equals("default")) this.courseFolder = "TimeManagerPersonalData/" + courseName; // if no directory is selected use the current one

            courseFile = this.courseFolder + $"/{courseName}.txt";
        }

        public void Start(bool animatedMode = false)
        {
            if (!Directory.Exists(courseFolder))
            {
                if (GetAnswer("Course not found. Do you want to start a timer on a new course? (y/n)") == "n")
                {
                    return;
                }
                Console.WriteLine("Making new course directory...");

                DirectoryHandler.MakeNewDirectory(courseFolder);
                DirectoryHandler.MakeFileInDirectory(courseFile, "00:00:00:00");
            }
            GetTotalTime();

            if (animatedMode)
                RunClockAnimated();
            else
                RunClock();
        }

        void RunClock()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            ClockState clockState = ClockState.Running;

            while (true)
            {
                UpdateConsole(clockState, stopwatch);

                clockState = HandleInput(Console.ReadKey(true).Key.ToString(), stopwatch);

                if (clockState == ClockState.End) return;
            }
        }

        void RunClockAnimated()
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();
            ClockState clockState = ClockState.Running;

            Stopwatch consoleUpdateTimer = new();
            consoleUpdateTimer.Start();

            while (true)
            {
                if (consoleUpdateTimer.ElapsedMilliseconds > 50)
                {
                    consoleUpdateTimer.Restart();
                    UpdateConsole(clockState, stopwatch, animatedMode: true);
                }

                if (Console.KeyAvailable)
                {
                    // True to not display the pressed key
                    clockState = HandleInput(Console.ReadKey(true).Key.ToString(), stopwatch);
                }

                if (clockState == ClockState.End) return;
            }
        }

        void UpdateConsole(ClockState clockState, Stopwatch stopwatch, bool animatedMode = false)
        {
            Console.Clear();
            Console.WriteLine(
                $"Total study time on course {courseName} is {totalTime}\n" +
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

        ClockState HandleInput(string userInput, Stopwatch stopwatch)
        {
            switch (userInput.ToLower())
            {
                case "q":
                    stopwatch.Stop();
                    totalTime = AddSecondsToTime(stopwatch.ElapsedMilliseconds / 1000);
                    DirectoryHandler.SaveTotalTime(courseFile, totalTime);
                    return ClockState.End;

                case "p":
                    stopwatch.Stop();
                    return ClockState.Stopped;

                default:
                    stopwatch.Start();
                    return ClockState.Running;
            }
        }

        string AddSecondsToTime(long secondsToAdd)
        {
            // Parse the existing time string to a TimeSpan
            if (TimeSpan.TryParse(totalTime, out TimeSpan currentTime))
            {
                // Add seconds to the existing time
                TimeSpan updatedTime = currentTime.Add(TimeSpan.FromSeconds(secondsToAdd));

                // Format the updated time as dd:hh:mm:ss
                string formattedTime = $"{updatedTime.Days:D2}:{updatedTime.Hours:D2}:{updatedTime.Minutes:D2}:{updatedTime.Seconds:D2}";

                return formattedTime;
            }
            else
            {
                throw new ArgumentException("Invalid time format. Expected format: dd:hh:mm:ss");
            }
        }

        void GetTotalTime()
        {
            string? totalTime = DirectoryHandler.ReadLineFromFile(courseFile);
            this.totalTime = totalTime ?? "";
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
