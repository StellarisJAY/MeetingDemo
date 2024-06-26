using Meeting.Core.Models;
using Meeting.Core.Models.DTO;

namespace Meeting.Core.Services
{
    public interface IRoomService
    {
        Task<CommonResult<RoomModel>> CreateRoom(AddRoomDTO dto);

        Task<CommonResult<RoomModel>> GetRoomDetail(long roomID);
    }
}
