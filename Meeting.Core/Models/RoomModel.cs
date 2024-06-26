namespace Meeting.Core.Models
{
    [SqlSugar.SugarTable("room")]
    public class RoomModel: CommonModel
    {
        public static readonly int PublicAccess = 1;
        public static readonly int PrivateAccess = 0;

        [SqlSugar.SugarColumn(IsPrimaryKey = true)]
        public long RoomID { get; set; }

        public string RoomName { get; set; } = string.Empty;

        public long Host {  get; set; }

        public int Accessible { get; set; }

        public string? Password { get; set; }
    }
}
