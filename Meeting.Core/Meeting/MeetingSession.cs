using SIPSorcery.Net;
using System.Collections.Concurrent;
using System.Net;

namespace Meeting.Core.Meeting
{
    public class MeetingSession
    {
        public long RoomID { get; private set; }
        public ConcurrentDictionary<long, UserConnection> Connections { get; private set; } = new();
        
        public void OnRtpPacketReceived(long senderID, IPEndPoint endpoint, SDPMediaTypesEnum mediaType, RTPPacket packet)
        {

        }

        public static MeetingSession Create(long roomID)
        {
            return new() { RoomID = roomID };
        }
    }
}
