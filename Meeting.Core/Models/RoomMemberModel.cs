namespace Meeting.Core.Models
{
    [SqlSugar.SugarTable("room_member")]
    public class RoomMemberModel: CommonModel
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true)]
        public long UserID { get; set; }
        [SqlSugar.SugarColumn(IsPrimaryKey = true)]
        public long RoomID { get; set; }
    }
}
