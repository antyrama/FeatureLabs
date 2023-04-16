namespace FeatureLabs.AppServices.Configuration.Exceptions
{
    public class CustomValidationException : ApplicationException
    {
        public CustomValidationException(string message, params string[] errorMessages)
            : base(message)
        {
            ErrorMessages = errorMessages;
        }

        public string[] ErrorMessages { get; }
    }
}
