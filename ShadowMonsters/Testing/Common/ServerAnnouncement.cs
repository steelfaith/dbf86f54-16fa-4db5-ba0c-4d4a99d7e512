using Common.Enums;
using System;

//TODO: this doesnt seem to follow an existing pattern, think about it.
namespace Common
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