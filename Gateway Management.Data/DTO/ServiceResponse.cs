namespace Gateway_Management.Data.DTO
{
    public class ServiceResponse<T>
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
