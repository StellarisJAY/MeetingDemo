using AutoMapper;
using Meeting.Core.DAO;
using Meeting.Core.Models;
using Meeting.Core.Models.DTO;
using System.Security.Cryptography;

namespace Meeting.Core.Services
{
    public class RoomService : IRoomService
    {
        private readonly ILogger<RoomService> _logger;
        private readonly IMapper _mapper;
        private readonly RoomDAO _roomDAO;

        public RoomService(ILogger<RoomService> logger, IMapper mapper, RoomDAO roomDAO)
        {
            _logger = logger;
            _mapper = mapper;
            _roomDAO = roomDAO;
        }

        async Task<CommonResult<RoomModel>> IRoomService.CreateRoom(AddRoomDTO dto)
        {
            CommonResult<RoomModel> result = new() { Code = 200, Message = "添加成功" };
            var model = _mapper.Map<RoomModel>(dto);
            if (model.Accessible == RoomModel.PrivateAccess)
            {
                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    result.Message = "添加失败，密码不能为空";
                    return result;
                }
                // TODO MD5(password+salt)
            }
            var list = await _roomDAO.Index(new() { Host = model.Host, RoomName = model.RoomName });
            if (list.Data.Any(r=>r.RoomName == model.RoomName))
            {
                result.Code = 400;
                result.Message = "添加失败，不能创建相同名字的房间";
                return result;
            }
            var addTime = DateTime.Now;
            model.AddTime = addTime;
            model.UpdateTime = addTime;
            bool flag = await _roomDAO.AddRoom(model);
            if (!flag)
            {
                result.Message = "添加失败";
            }
            result.Data = model;
            return result;
        }

        async Task<CommonResult<RoomModel>> IRoomService.GetRoomDetail(long roomID)
        {
            CommonResult<RoomModel> result = new() { Code = 200, Message = "查询成功" };
            RoomModel? model = await _roomDAO.GetRoomDetail(roomID);
            if (model == null)
            {
                result.Code = 404;
                result.Message = "查询失败，房间不存在";
            }
            result.Data = model;
            return result;
        }
    }
}
