using Common.Enums;
using System;

namespace Common.Messages
{
    [Serializable]
    public class ServerAnnouncement
    {
        public ServerAnnouncement(string message)
        {
            AnnouncementType = AnnouncementType.System;
            Message = message;
        }
        public string Message { get; set; }
        public AnnouncementType AnnouncementType { get; set; }
    }
}
