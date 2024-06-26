using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Meeting.Core.Models.DTO
{
    public class AddRoomDTO
    {
        [StringLength(maximumLength: 20, MinimumLength = 3, ErrorMessage = "房间名长度最大20,最小3")]
        public string RoomName { get; set; } = string.Empty;
        [Required(ErrorMessage="访问权限不能为空")]
        [Range(minimum:0,maximum:1,ErrorMessage = "访问权限必须是0或1")]
        public int Accessible {  get; set; }
        [StringLength(maximumLength:16, ErrorMessage = "密码最多16位")]
        public string? Password {  get; set; }
    }

    public class UpdateRoomDTO
    {
        [Required(ErrorMessage = "房间ID不能为空")]
        public long RoomID { get; set; }
        [StringLength(maximumLength: 20, MinimumLength = 3, ErrorMessage = "房间名长度最大20,最小3")]
        public string RoomName { get; set; } = string.Empty;
        [Range(minimum: 0, maximum: 1, ErrorMessage = "访问权限必须是0或1")]
        public int Accessible { get; set; }
        [StringLength(maximumLength: 16, ErrorMessage = "密码最多16位")]
        public string? Password { get; set; }
    }

    public class RoomDTOMapperProfile: Profile
    {
        public RoomDTOMapperProfile()
        {
            CreateMap<AddRoomDTO, RoomModel>();
            CreateMap<UpdateRoomDTO, RoomModel>();
        }
    }
}
