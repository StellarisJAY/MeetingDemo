namespace Meeting.Core.Models
{
    public class SessionModel
    {
        public string SessionID { get; set; } = string.Empty;
        public string UserID { get; set; } = string.Empty;
        public DateTime ExpireTime { get; set; }

        public static string CacheKey(string sessionID) => $"/meeting/session/{sessionID}";
    }
}
