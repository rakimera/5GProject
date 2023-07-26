namespace Application.CustomExceptions;

public class UserException : Exception
{
    public override string Message { get; }

    public UserException(string message)
    {
        Message = message;
    }
}