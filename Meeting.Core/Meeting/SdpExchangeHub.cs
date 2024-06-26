using Meeting.Core.Models;
using Meeting.Core.Models.DTO;
using Meeting.Core.Services;
using Microsoft.AspNetCore.SignalR;

namespace Meeting.Core.Meeting
{
    public class SdpExchangeHub: Hub
    {
        private readonly SessionManager _sessionManager;
        private readonly ILogger<SdpExchangeHub> _logger;
        private readonly IRoomService _roomService;

        public SdpExchangeHub(SessionManager sessionManager, ILogger<SdpExchangeHub> logger, IRoomService roomService)
        {
            _sessionManager = sessionManager;
            _logger = logger;
            _roomService = roomService;
        }

        public async Task SDPOffer(string message, long roomID, long userID)
        {
            var caller = Clients.Caller;
            // TODO check room member
            CommonResult<RoomModel> result = await _roomService.GetRoomDetail(roomID);
            if (result.Data == null)
            {
                // TODO error code and error message
                await Clients.Caller.SendAsync("SdpExchangeError", "room not found");
                return;
            }

            var session = _sessionManager.RoomSessions.GetOrAdd(roomID, _ => MeetingSession.Create(roomID));
            var conn = UserConnection.CreateUserConnection(userID, message);
            if (conn == null)
            {
                // TODO error code and error message
                await Clients.Caller.SendAsync("SdpExchangeError", "can't create peer connection");
                return;
            }

            // 设置服务器端ICE Candidate处理
            conn.SetOnIceCandidate(async (candidate) =>
            {
                await caller.SendAsync("ServerIceCandidate", candidate.toJSON());
            });

            // 设置RTP转发
            conn.SetRTPPacketReceiver((endpoint, media, packet) =>
            {
                session.OnRtpPacketReceived(userID, endpoint, media, packet);
            });

            _logger.LogDebug("peer connection created, room: {roomID}, user: {userID}, sdp answer: {sdpAnswer}", roomID, userID, conn.SDPAnswerString);
            await Clients.Caller.SendAsync("SDPAnswer", conn.SDPAnswerString, roomID, userID);
        }

        public async Task ICECandidate(string message, long roomID, long userID)
        {
            // TODO check room member
            CommonResult<RoomModel> result = await _roomService.GetRoomDetail(roomID);
            if (result.Data == null)
            {
                // TODO error code and error message
                await Clients.Caller.SendAsync("SdpExchangeError", "room not found");
                return;
            }

            var session = _sessionManager.RoomSessions[roomID];
            if (session == null)
            {
                await Clients.Caller.SendAsync("SdpExchangeError", "session not created");
                return;
            }

            var userConn = session.Connections[userID];
            if (userConn == null)
            {
                await Clients.Caller.SendAsync("SdpExchangeError", "peer connection not created");
                return;
            }
        }
    }
}
