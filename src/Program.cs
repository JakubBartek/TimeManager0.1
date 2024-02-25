namespace timeManager
{
    class Program
    {
        static void Main()
        {
            SettingsPage settingsPage = new();

            string[] options = new string[] {
                "StudyClock",
                "Calendar",
                "Settings",
                "Exit",
            };

            string errorMsg = "";

            while (true)
            {
                Console.Clear();
                string chosenOption = 
                    options[TerminalSelector.Select(
                        options,
                        "Welcome to TimeManager0.1\n",
                        "\n" + errorMsg
                    )];

                switch (chosenOption.ToLower())
                {
                    case "studyclock":
                        Console.WriteLine("Running StudyClock!");

                        StudyClock clock = new();
                        clock.Start();

                        Console.WriteLine("Exiting StudyClock!");
                        Console.Clear();
                        break;

                    case "calendar":
                        errorMsg = "Calendar app is not finished yet";
                        break;

                    case "exit":
                        Console.WriteLine("Exiting program (Terminated by user)");
                        return;

                    case "settings":
                        settingsPage.EnterSettingsPage();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
