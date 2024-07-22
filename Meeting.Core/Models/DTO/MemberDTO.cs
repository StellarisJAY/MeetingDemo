using AutoMapper;

namespace Meeting.Core.Models.DTO
{
    public class MemberDetailDTO
    {
        public long UserID { get; set; }
        public long RoomID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public DateTime RegisterTime { get; set; }
    }

    public class MemberDTOProfile : Profile
    {
        public MemberDTOProfile()
        {
            CreateMap<RoomMemberModel, MemberDetailDTO>();
        }
    }
}
