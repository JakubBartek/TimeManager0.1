namespace timeManager
{
    class SettingsPage
    {
        public string DataDirectory { get; set; }
        private string dataFolderName = "data";
        private string DataParentDirectory = Directory.GetCurrentDirectory();

        public SettingsPage()
        {
            this.DataDirectory = Path.Join(
                DataParentDirectory, dataFolderName);
        }

        /// <summary>
        /// Enter settings page
        /// </summary>
        /// <param name="animatedMode"> True if animated mode is enabled </param>
        public void EnterSettingsPage(ref bool animatedMode)
        {
            const string selectPathStr = "Select Path";
            const string addQuoteStr = "Add Quote";
            const string switchAnimatedModeStr = "Switch Animated Mode";
            const string backToMainPageStr = "Back to the Main Page";

            string[] options = new string[] {
                selectPathStr,
                addQuoteStr,
                switchAnimatedModeStr,
                backToMainPageStr,
            };

            while (true)
            {
                Console.Clear();
                string chosenOption = options[TerminalSelector.Select(
                    options,
                    "Settings Page\n",
                    ""
                )];

                switch (chosenOption)
                {
                    case selectPathStr:
                        GetPathFromUserDialog();
                        break;

                    case addQuoteStr:
                        TerminalSelector.Select(
                            new string[] {
                                "OK",
                            },
                            "Not implemented yet"
                        );
                        break;
                    
                    case switchAnimatedModeStr:
                        animatedMode = !animatedMode;
                        TerminalSelector.Select(
                            new string[] {
                                "OK",
                            },
                            "Animated mode is now " + (animatedMode ? "enabled" : "disabled")
                        );
                        break;

                    case backToMainPageStr:
                        return;

                    default:
                        throw new Exception("Invalid option");
                }
            }
        }

        /// <summary>
        /// Get user input for path to store app data
        /// </summary>
        /// <returns> True if user input is valid </returns>
        private bool GetPathFromUserDialog()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter path to directory to store app data");
                string? userInput = Console.ReadLine();

                if (Directory.Exists(userInput))
                {
                    this.DataDirectory = userInput;
                    return true;
                }
                else
                {
                    Console.Clear();
                    int idx = TerminalSelector.Select(
                        new string[] {
                            "OK",
                            "Cancel",
                        },
                        "Given path is invalid"
                    );

                    // If user selects "Cancel"
                    if (idx == 1) return false;
                }
            }
        }

    }
}
