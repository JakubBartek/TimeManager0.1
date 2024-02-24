namespace timeManager
{
    class SettingsPage {
        public string DataDirectory {get; set;}

        public SettingsPage() {
            this.DataDirectory = Directory.GetCurrentDirectory();
        }

        public void EnterSettingsPage() {
            string? userInput;

            while (true) {
                Console.Clear();
                Console.WriteLine("------ Settings Page ------\n\n");
                Console.WriteLine("p --> select path");
                Console.WriteLine("q --> add qote");
                Console.WriteLine("b --> back to main page");

                userInput = Console.ReadLine();
                userInput ??= "default";

                Console.Clear();
                switch (userInput.ToLower()) {
                    case "p":
                        Console.WriteLine("Enter path to directory to store app data");
                        userInput = Console.ReadLine();
                        if (Directory.Exists(userInput)) {
                            this.DataDirectory = userInput;
                        } else {
                            Console.WriteLine("\nGiven path is invalid (going back in 3s)");
                            Thread.Sleep(3000);
                        }
                        continue;

                    case "q":
                        Console.WriteLine("Add logic here");
                        continue;
                    
                    case "b":
                        Console.WriteLine("Exiting settings page");
                        return;
                    default:
                        continue;
                }
            }
        }
    }
}