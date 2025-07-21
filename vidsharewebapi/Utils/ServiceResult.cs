namespace VidShareWebApi.Utils
{
    public class ServiceResult<T>
    {
        public string? Message { get; set; }
        public bool Success { get; set; }
        public T? Result { get; set; } 
    }
}