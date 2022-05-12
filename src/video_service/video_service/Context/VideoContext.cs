using video_service.Context;
using Microsoft.EntityFrameworkCore;
using video_service.Model;

namespace video_service.Context
{
    public class VideoContext :DbContext
    {
        public VideoContext(DbContextOptions<VideoContext> options): base(options) { }

        public DbSet<ChannelModel>  channelModels { get; set; }  
        public DbSet<VideoModel> videoModels { get; set; }
        public DbSet<UserModel> userModels { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
