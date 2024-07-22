using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Meeting.Core.Models.DTO
{
    public class AddUserDTO
    {
        [StringLength(maximumLength: 20, MinimumLength = 3, ErrorMessage = "用户名长度最大20,最小3")]
        public string UserName { get; set; } = string.Empty;
        [StringLength(maximumLength: 16, ErrorMessage = "密码最多16位")]
        public string? Password { get; set; }
    }

    public class UserDTOProfile : Profile
    {
        public UserDTOProfile()
        {
            CreateMap<AddUserDTO, UserModel>();
        }
    }
}
