using Meeting.Core.DAO.Cache;
using Meeting.Core.Models;
using SqlSugar;

namespace Meeting.Core.DAO
{
    public class SessionDAO: CommonDAO
    {
        private readonly ILogger<SessionDAO> _logger;
        private readonly ISugarUnitOfWork<MeetingDbContext> _dbContext;
        private readonly ICacheHelper _cacheHelper;

        public SessionDAO(ILogger<SessionDAO> logger, ISugarUnitOfWork<MeetingDbContext> dbContext, ICacheHelper cacheHelper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _cacheHelper = cacheHelper;
        }

        public async Task<SessionModel?> GetUserSession(string sessionID)
        {
            var model = await _cacheHelper.GetAsync<SessionModel>(SessionModel.CacheKey(sessionID));
            if (model == null || string.IsNullOrEmpty(model.SessionID))
            {
                return null;
            }
            return model;
        }

        public async Task AddUserSession(SessionModel model)
        {
            await _cacheHelper.SetAsync(SessionModel.CacheKey(model.SessionID), model);
        }
    }
}
