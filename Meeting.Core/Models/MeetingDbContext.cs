using SqlSugar;

namespace Meeting.Core.Models
{
    public class MeetingDbContext: SugarUnitOfWork
    {
        public DbSet<RoomModel> RoomSet { get; set; } = new DbSet<RoomModel>();
        public DbSet<RoomMemberModel> RoomMemberSet { get; set;} = new DbSet<RoomMemberModel>();
        public DbSet<UserModel> UserSet { get; set; } = new DbSet<UserModel>();
    }

    public class DbSet<T>: SimpleClient<T> where T: class, new()
    {

    }
}
