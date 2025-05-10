namespace MagicVilla_VillaAPI.Logging
{
    public class Logging : ILogging
    {
        //This means print the log in the console inside the api i can see this if i did "Get"
        public void Log(string message, string type)
        {
            if (type == "error")
            {
                Console.WriteLine("ERROR - " + message);
            }
            else 
            {
                Console.WriteLine(message);
            }
        }
    }
}
