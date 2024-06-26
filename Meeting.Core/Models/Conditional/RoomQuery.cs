namespace Meeting.Core.Models.Conditional
{
    public class RoomQuery: CommonQuery
    {
        public string? RoomName {  get; set; }
        public int? Accessible {  get; set; }
        public long? Host {  get; set; }
    }
}
