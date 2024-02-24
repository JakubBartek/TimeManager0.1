namespace timeManager
{
    class DirectoryHandler
    {
        public static void SelectDirectory()
        {
            Console.WriteLine("Enter path of directory you want to select");
            string? directoryStr = Console.ReadLine();
        }

        public static void MakeNewDirectory(string folderPath)
        {
            try
            {
                Directory.CreateDirectory(folderPath);
                Console.WriteLine("Course imported successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating the folder: {ex.Message}");
            }
        }

        public static void MakeFileInDirectory(string fileName, string text)
        {
            using (File.Create(fileName)) { }
            StreamWriter sw = new(fileName);
            sw.WriteLine(text);
            sw.Close();
        }

        public static void SaveTotalTime(string fileName, string text)
        {
            try
            {
                StreamWriter sw = new(fileName);
                sw.WriteLine(text);
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing in file: {ex.Message}");
            }
        }

        public static string? ReadLineFromFile(string fileName)
        {
            try
            {
                StreamReader sr = new(fileName);
                string? totalTime = sr.ReadLine();
                sr.Close();
                return totalTime;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            return "";
        }
    }
}