using Meeting.Core.Models;
using Meeting.Core.Models.Conditional;
using Meeting.Core.Models.DTO;
using SqlSugar;

namespace Meeting.Core.DAO
{
    public class RoomDAO
    {
        private readonly ILogger<RoomDAO> _logger;
        private readonly ISugarUnitOfWork<MeetingDbContext> _dbContext;

        public RoomDAO(ILogger<RoomDAO> logger, ISugarUnitOfWork<MeetingDbContext> dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<bool> AddRoom(RoomModel model)
        {
            using var ctx = _dbContext.CreateContext();
            long id = await ctx.RoomSet.AsInsertable(model).ExecuteReturnSnowflakeIdAsync();
            if (id == 0)
            {
                ctx.Dispose();
                return false;
            }
            ctx.Commit();
            model.RoomID = id;
            return true;
        }

        public async Task<RoomModel?> GetRoomDetail(long roomID)
        {
            using var ctx = _dbContext.CreateContext();
            return await ctx.RoomSet
                .AsQueryable()
                .Where(r => r.RoomID == roomID)
                .FirstAsync();
        }

        public async Task<PageList<RoomModel>> Index(RoomQuery query)
        {
            using var ctx = _dbContext.CreateContext();
            var queryable = ctx.RoomSet.AsQueryable();
            if (query.Host != null)
            {
                queryable = queryable.Where(r=>r.Host == query.Host);
            }
            if (query.Accessible != null)
            {
                queryable = queryable.Where(r=>r.Accessible == query.Accessible);
            }
            if (!string.IsNullOrEmpty(query.RoomName))
            {
                queryable = queryable.Where(r=>r.RoomName.Contains(query.RoomName));
            }
            if (query.IsPage)
            {
                RefAsync<int> total = 0;
                RefAsync<int> totalPage = 0;
                var data = await queryable.ToPageListAsync(query.PageNum, query.PageSize, total, totalPage);
                return new()
                {
                    PageNum = query.PageNum,
                    PageSize = query.PageSize,
                    Total = total,
                    TotalPages = totalPage,
                    Data = data
                };
            }else
            {
                var data = await queryable.ToListAsync();
                return new()
                {
                    PageNum = 1,
                    PageSize = data.Count,
                    Total = data.Count,
                    TotalPages = 1,
                    Data = data
                };
            }
        }
    }
}
