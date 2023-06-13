using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SocialNetworkWithSignalR.Entities
{
    public class CustomIdentityDbContext:IdentityDbContext<CustomIdentityUser,CustomIdentityRole,string>
    {
        public CustomIdentityDbContext(DbContextOptions<CustomIdentityDbContext>options)
            :base(options)
        {

        }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
    }
}
