namespace Meeting.Core.Models
{
    [SqlSugar.SugarTable("user")]
    public class UserModel: CommonModel
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true)]
        public long UserID { get; set; }
        [SqlSugar.SugarColumn(ColumnDataType = "nvarchar", Length = 50)]
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;

        public static string CacheKey(long userID) => $"/meeting/user/{userID}";
    }
}
