using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using AirGrap.log;
namespace AirGrap;
public static class Logger
{
    static JsonSerializerOptions options = new JsonSerializerOptions
    {
        WriteIndented = true,
    };
    public static void Init()
    {
        if (!Directory.Exists("AirGrapLogs"))
            Directory.CreateDirectory("AirGrapLogs");
        if (!File.Exists("AirGrapLogs/ErrorLog.json"))
            File.Create("AirGrapLogs/ErrorLog.json");
        if (!File.Exists("AirGrapLogs/Log.json"))
            File.Create("AirGrapLogs/Log.json");
    }
    static object locker = new();
    
    public static void AddErrorLog(string text,string context = "")
    {
        ErrorLogFormat log = new();

        StackFrame frame = new StackFrame(1);
        MethodBase? Method = frame.GetMethod();

        if (Method != null)
        {
            var Class = Method.DeclaringType;
            
            if (Class == null || Class.FullName == null)
                log.Class = "null";
            else 
                log.Class = Class.FullName.Replace("\u002B\u003C",".").Replace("\u003Ed__3","()");
        }
        log.Time = DateTime.Now;
        log.Error = text;
        if (context != string.Empty)
            log.Context = context; 
        
        string json = JsonSerializer.Serialize(log,options);
        lock (locker)
            File.AppendAllText("AirGrapLogs/ErrorLog.json",string.Concat(json,"\n"));
        
    }
    public static void AddLog(string text,string context = "")
    {
        LogFormat log = new();
        log.Time = DateTime.Now;
        log.Message = text;
        if (context != string.Empty)
            log.Context = context; 
        
        string json = JsonSerializer.Serialize(log,options);
        lock (locker)
            File.AppendAllText("AirGrapLogs/Log.json",string.Concat(json,"\n"));
        
    }

}
