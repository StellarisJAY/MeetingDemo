using Meeting.Core.Models;
using Meeting.Core.Models.Conditional;
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
        private readonly IMemberService _memberService;

        public RoomController(ILogger<RoomController> logger, IRoomService roomService, IMemberService memberService)
        {
            _logger = logger;
            _roomService = roomService;
            _memberService = memberService;
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

        [HttpPut]
        public async Task<IActionResult> UpdateRoom([FromBody] UpdateRoomDTO dto)
        {
            CommonResult<RoomModel> result = await _roomService.UpdateRoom(dto);
            return Ok(result);
        }

        [HttpGet("Member")]
        public async Task<IActionResult> ListMembers([FromQuery] MemberQuery query)
        {
            var result = await _memberService.ListMembers(query);
            return Ok(result);
        }
    }
}
