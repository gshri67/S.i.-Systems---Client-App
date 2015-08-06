namespace Shared.Core
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }

        public string Message { get; set; }

        public ValidationResult() { }

        public ValidationResult(bool isValid)
        {
            this.IsValid = isValid;
        }

        public ValidationResult(bool isValid, string message) : this(isValid)
        {
            this.Message = message;
        }
    }
}
