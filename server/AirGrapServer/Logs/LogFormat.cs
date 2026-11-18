namespace AirGrap.log;
class ErrorLogFormat
{
    public DateTime Time { get; set; }
    public string Class { get; set; } = "";
    public string Error { get; set; } = "";
    public string Context {get; set;} = "NotContext";        
}
class LogFormat
{
    public DateTime Time { get; set; }
    public string Message { get; set; } = "";
    public string Context {get; set;} = "NotContext";        
}