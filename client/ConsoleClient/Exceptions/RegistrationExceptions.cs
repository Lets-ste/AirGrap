namespace AirGrap.Exceptions;
public class UserAlreadyExistsException : Exception
{
    public string Login { get; }
    public UserAlreadyExistsException(string _login)
        : base($"login '{_login}' already exists") => Login = _login;
}
public class RegistrationFailedException : Exception
{
    public RegistrationFailedException()
        : base("registration failed try again"){}
}