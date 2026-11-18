using System;

namespace AirGrap.Datasurse;

public class ConectionString
{
    public string Host { get; set; } = "localhost" ;
    public int Port { get; set; } = 5432;
    public string Database { get; set; } = "airgrap";
    public string Username { get; set; } = "postgres";
    public string Password { get; set; } = "postgre";
        
}
