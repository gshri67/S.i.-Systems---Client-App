namespace Shared.Core
{
    /// <summary>
    /// This is used to deserialize System.Web.Http.HttpErrors when
    /// they are returned from the Web API after an exception.
    /// </summary>
    public class HttpError
    {
        public string Message { get; set; }

        public string ExceptionMessage { get; set; }

        public string ExceptionType { get; set; }

        public string StackTrace { get; set; }
    }
}
