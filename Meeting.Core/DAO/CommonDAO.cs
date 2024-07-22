using Meeting.Core.Models.Conditional;
using Meeting.Core.Models.DTO;
using SqlSugar;

namespace Meeting.Core.DAO
{
    public class CommonDAO
    {
        public async Task<PageList<T>> ToPageListAsync<T>(ISugarQueryable<T> queryable, CommonQuery pageQuery)
        {
            if (pageQuery.IsPage)
            {
                RefAsync<int> total = 0;
                RefAsync<int> totalPage = 0;
                var data = await queryable.ToPageListAsync(pageQuery.PageNum, pageQuery.PageSize, total, totalPage);
                return new()
                {
                    PageNum = pageQuery.PageNum,
                    PageSize = pageQuery.PageSize,
                    Total = total,
                    TotalPages = totalPage,
                    Data = data
                };
            }
            else
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
