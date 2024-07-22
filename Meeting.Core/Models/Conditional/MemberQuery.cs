namespace Meeting.Core.Models.Conditional
{
    public class MemberQuery: CommonQuery
    {
        public long? UserID { get; set; }
        public long RoomID { get; set; }
        public string? UserName { get; set; }
    }
}
