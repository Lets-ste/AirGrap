namespace AirGrap.Exceptions;

public class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException()
        : base("invalid login or password"){}
}
public class SessionNotFoundException : Exception
{
    public SessionNotFoundException()
        : base("session is not found"){}
}
