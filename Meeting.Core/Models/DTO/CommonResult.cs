namespace Meeting.Core.Models.DTO
{
    public class CommonResult<T>
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
