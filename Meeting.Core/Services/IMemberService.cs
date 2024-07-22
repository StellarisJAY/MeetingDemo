using Meeting.Core.Models.Conditional;
using Meeting.Core.Models.DTO;

namespace Meeting.Core.Services
{
    public interface IMemberService
    {
        Task<CommonResult<PageList<MemberDetailDTO>>> ListMembers(MemberQuery query);
    }
}
