namespace Direct_Ferries_Test_App
{
    public class Logger : ILogger
    {
        public void WriteLine(string content, bool newLine = false)
        {
            Console.WriteLine(content);

            if (newLine)
                Console.WriteLine();
        }

    }
}
