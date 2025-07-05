namespace TaskApi.Models
{
    /// <summary>
    /// Standard error response for APIs
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// HTTP status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Human-readable message
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
