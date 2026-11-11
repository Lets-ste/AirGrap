namespace AirGrap.Exceptions;
public class NullInputFieldException : Exception
{
    public NullInputFieldException()
        : base("There is no mandatory value"){}
}
public class UserAlreadyExistsException : Exception
{
    public string Login { get; }
    public UserAlreadyExistsException(string _login)
        : base($"login '{_login}' already exists") => Login = _login;
}