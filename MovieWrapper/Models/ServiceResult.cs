namespace MovieWrapper.Models
{
    /// <summary>
    /// Show status, wrap data, message (in case error or other purpose)
    /// </summary>
    /// <typeparam name="T">Type of data (ex: movie, session, ...)</typeparam>
    public abstract class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
