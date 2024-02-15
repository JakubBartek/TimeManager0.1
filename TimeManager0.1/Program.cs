namespace timeManager
{
    class Program
    {
        static void Main()
        {
            Console.Clear();
            Console.WriteLine("Welcome to TimeManager0.1 \n\nChoose app -> StudyClock(s) | Calendar(c) !Work in progress! \n\nq to exit program");

            while (true)
            {   
                string? chosenApp = Console.ReadLine();
                chosenApp = chosenApp ??= "default";
                chosenApp = chosenApp.ToLower();
                if (chosenApp.ToLower().Equals("s")) chosenApp = "studyclock";
                if (chosenApp.ToLower().Equals("c")) chosenApp = "calendar";

                Console.Clear();
                switch (chosenApp)
                {
                    case "studyclock":
                        Console.WriteLine("Running StudyClock!");

                        StudyClock clock = new();
                        clock.Start();

                        Console.WriteLine("Exiting StudyClock!");
                        Console.Clear();
                        break;

                    case "calendar":
                        Console.WriteLine("Calendar app is not finished yet");
                        break;

                    case "q":
                        Console.WriteLine("Exiting program (Terminated by user)");
                        return;

                    default:
                        break;
                }
                Console.WriteLine("Write shortcut or name of the app you want to use\n\nStudyClock(s) | Calendar(c) !Work in progress! \n\nq to exit program");
            }
        }
    }
}
