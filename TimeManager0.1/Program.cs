namespace timeManager
{
    class Program
    {
        static void Main()
        {
            Console.Clear();
            Console.WriteLine("Welcome to TimeManager0.1 \n\nChoose app -> StudyClock | Calendar(work in progress) \n\nq to exit program");

            while (true)
            {   
                string? chosenApp = Console.ReadLine();
                chosenApp = chosenApp ??= "default";

                Console.Clear();
                switch (chosenApp.ToLower())
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
                        Console.WriteLine("Exiting program");
                        return;

                    default:
                        Console.WriteLine("Write the name of the app you want to use\n\nStudyClock | Calendar(work in progress) \n\nq to exit program");
                        break;
                }
            }
        }
    }
}
