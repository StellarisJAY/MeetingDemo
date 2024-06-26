using Meeting.Core.Models;
using Meeting.Core.Models.DTO;
using Meeting.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace Meeting.Core.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class RoomController: ControllerBase
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomService _roomService;

        public RoomController(ILogger<RoomController> logger, IRoomService roomService)
        {
            _logger = logger;
            _roomService = roomService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] AddRoomDTO dto)
        {
            CommonResult<RoomModel> result = await _roomService.CreateRoom(dto);
            return Ok(result);
        }

        [HttpGet("{roomID}")]
        public async Task<IActionResult> GetRoomDetail(long roomID)
        {
            CommonResult<RoomModel> result = await _roomService.GetRoomDetail(roomID);
            return Ok(result);
        }
    }
}
