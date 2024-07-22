using AutoMapper;
using Meeting.Core.DAO;
using Meeting.Core.Models.Conditional;
using Meeting.Core.Models.DTO;

namespace Meeting.Core.Services
{
    public class MemberService: IMemberService
    {
        private readonly ILogger<MemberService> _logger;
        private readonly IMapper _mapper;
        private readonly RoomMemberDAO _memberDAO;

        public MemberService(ILogger<MemberService> logger, IMapper mapper, RoomMemberDAO memberDAO)
        {
            _logger = logger;
            _mapper = mapper;
            _memberDAO = memberDAO;
        }

        public async Task<CommonResult<PageList<MemberDetailDTO>>> ListMembers(MemberQuery query)
        {
            var result = new CommonResult<PageList<MemberDetailDTO>>() { Code = 200, Message = "查询成功" };
            var list =  await _memberDAO.ListRoomMembers(query);
            result.Data = list;
            return result;
        }
    }
}
