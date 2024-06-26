namespace Meeting.Core.Models.DTO
{
    public class PageList<T>
    {
        public int PageNum { get; set; }
        public int PageSize {  get; set; }
        public int Total {  get; set; }
        public int TotalPages { get; set; }

        public List<T> Data { get; set; } = new List<T>();
    }
}
