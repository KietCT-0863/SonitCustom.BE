namespace SonitCustom.BLL.Exceptions
{
    public class InvalidRefreshTokenException : Exception
    {
        public string TokenMessage { get; }

        public InvalidRefreshTokenException(string message) : base(message) 
        {
            TokenMessage = message;
        }
    }
}
