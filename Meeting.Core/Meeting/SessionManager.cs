using System.Collections.Concurrent;

namespace Meeting.Core.Meeting
{
    public class SessionManager
    {
        public ConcurrentDictionary<long, MeetingSession> RoomSessions { get; private set; } = new ConcurrentDictionary<long, MeetingSession>();
    }
}
