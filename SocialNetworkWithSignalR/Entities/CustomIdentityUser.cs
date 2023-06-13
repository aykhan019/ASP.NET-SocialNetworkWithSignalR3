﻿using Microsoft.AspNetCore.Identity;

namespace SocialNetworkWithSignalR.Entities
{
    public class CustomIdentityUser:IdentityUser
    {
        public CustomIdentityUser()
        {
            Friends = new List<Friend>();
            FriendRequests = new List<FriendRequest>();
            Chats = new List<Chat>();
        }
        public virtual ICollection<Friend> Friends { get; set; }
        public virtual ICollection<FriendRequest> FriendRequests { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
        public string ImageUrl { get; set; }
        public bool IsOnline { get; set; }
        public bool IsFriend { get; set; } = false;
        public bool HasRequestPending { get; set; } = false;
        public DateTime DisConnectTime { get; set; } = DateTime.Now;
        public string ConnectTime { get; set; } = "";
    }
}
