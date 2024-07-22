using Meeting.Core.DAO.Cache;
using Meeting.Core.Models;
using Meeting.Core.Models.Conditional;
using Meeting.Core.Models.DTO;
using SqlSugar;

namespace Meeting.Core.DAO
{
    public class RoomMemberDAO: CommonDAO
    {
        private readonly ILogger<RoomMemberDAO> _logger;
        private readonly ISugarUnitOfWork<MeetingDbContext> _dbContext;
        private readonly ICacheHelper _cacheHelper;

        public RoomMemberDAO(ILogger<RoomMemberDAO> logger, ISugarUnitOfWork<MeetingDbContext> dbContext, ICacheHelper cacheHelper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _cacheHelper = cacheHelper;
        }

        public async Task<PageList<MemberDetailDTO>> ListRoomMembers(MemberQuery query)
        {
            using var ctx = _dbContext.CreateContext();
            ISugarQueryable<RoomMemberModel, UserModel> condQuery = ctx.RoomMemberSet.AsQueryable()
                .InnerJoin<UserModel>((m, u) => m.UserID == u.UserID)
                .Where((m, u) => m.RoomID == query.RoomID);
            if (query.UserID != null)
            {
                condQuery = condQuery.Where((m,u) => m.UserID == query.UserID);
            }
            if (!string.IsNullOrEmpty(query.UserName))
            {
                condQuery = condQuery.Where((m, u)=>u.UserName.Contains(query.UserName));
            }
            var queryable = condQuery.Select((m, u) => new MemberDetailDTO() { 
                UserID = u.UserID,
                UserName = u.UserName,
                RoomID = m.RoomID,
                AvatarUrl = u.AvatarUrl,
                RegisterTime = u.AddTime,
            });

            return await ToPageListAsync(queryable, query);
        }

        public async Task<bool> AddRoomMember(RoomMemberModel model)
        {
            using var ctx = _dbContext.CreateContext();
            model.AddTime = DateTime.Now;
            model.UpdateTime = DateTime.Now;
            long id = await ctx.RoomMemberSet.AsInsertable(model).ExecuteReturnSnowflakeIdAsync();
            ctx.Commit();
            return true;
        }

        public async Task<bool> DelRoomMember(MemberQuery query)
        {
            using var ctx = _dbContext.CreateContext();
            int deleted = await ctx.RoomMemberSet.AsDeleteable()
                .Where(m => m.RoomID == query.RoomID && m.UserID == query.UserID)
                .ExecuteCommandAsync();
            ctx.Commit();
            return deleted > 0;
        }
    }
}
