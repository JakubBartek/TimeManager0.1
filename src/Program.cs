﻿namespace timeManager
{
    class Program
    {
        static void Main()
        {
            SettingsPage settingsPage = new();
            // Animated mode is false by default
            bool animatedMode = false;

            string[] options = new string[] {
                "StudyClock",
                "Calendar",
                "Settings",
                "Exit",
            };

            string bottomMessage = "";

            string dataDirectory = settingsPage.DataDirectory;

            while (true)
            {
                Console.Clear();

                string chosenOption =
                    options[TerminalSelector.Select(
                        options,
                        "Welcome to TimeManager0.1\n",
                        "\n" + bottomMessage
                    )];

                switch (chosenOption.ToLower())
                {
                    case "studyclock":
                        Console.WriteLine("Running StudyClock!");

                        StudyClock clock = new();
                        clock.Start(animatedMode: animatedMode);

                        Console.WriteLine("Exiting StudyClock!");
                        Console.Clear();
                        bottomMessage = "Course saved to Calendar!";
                        break;

                    case "calendar":
                        Calendar.Run();
                        break;

                    case "exit":
                        Console.WriteLine("Exiting program (Terminated by user)");
                        return;

                    case "settings":
                        settingsPage.EnterSettingsPage(ref animatedMode);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
