using SqlSugar;

namespace Meeting.Core.Models
{
    public class MeetingDbContext: SugarUnitOfWork
    {
        public DbSet<RoomModel> RoomSet { get; set; } = new DbSet<RoomModel>();
    }

    public class DbSet<T>: SimpleClient<T> where T: class, new()
    {

    }
}
