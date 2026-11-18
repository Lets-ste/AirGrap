namespace AirGrap.Exceptions;

public class RegistrationFailedException : Exception
{
    public RegistrationFailedException()
        : base("registration failed try again"){}
}
