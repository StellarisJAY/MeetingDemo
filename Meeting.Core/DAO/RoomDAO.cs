using Meeting.Core.DAO.Cache;
using Meeting.Core.Models;
using Meeting.Core.Models.Conditional;
using Meeting.Core.Models.DTO;
using SqlSugar;

namespace Meeting.Core.DAO
{
    public class RoomDAO: CommonDAO
    {
        private readonly ILogger<RoomDAO> _logger;
        private readonly ISugarUnitOfWork<MeetingDbContext> _dbContext;
        private readonly ICacheHelper _cacheHelper;

        public RoomDAO(ILogger<RoomDAO> logger, ISugarUnitOfWork<MeetingDbContext> dbContext, ICacheHelper cacheHelper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _cacheHelper = cacheHelper;
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
            return await _cacheHelper.GetAsync<RoomModel>(RoomModel.CacheKey(roomID), async () => {
                using var ctx = _dbContext.CreateContext();
                return await ctx.RoomSet
                    .AsQueryable()
                    .Where(r => r.RoomID == roomID)
                    .FirstAsync();
            });
        }

        public async Task<bool> UpdateRoomDetail(RoomModel model)
        {
            using var ctx = _dbContext.CreateContext(isTran: false);
            bool flag = await ctx.RoomSet.UpdateAsync(model);
            if (flag)
            {
                await _cacheHelper.DelAsync(RoomModel.CacheKey(model.RoomID));
            }
            return flag;
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
            return await ToPageListAsync(queryable, query);
        }
    }
}
