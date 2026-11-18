namespace AirGrap.Log;
public static class Logger
{
    public static void AddLog(string text)
    {
        File.AppendAllText("Logs/log.txt",text);
    }
}