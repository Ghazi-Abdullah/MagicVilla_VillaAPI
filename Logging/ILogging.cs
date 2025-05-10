namespace MagicVilla_VillaAPI.Logging
{
    public interface ILogging
    {
        //This is a public method to write massage and type in the log file
        public void Log(string message, string type);
    }
}
