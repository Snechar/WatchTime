using System.Collections.Generic;

namespace video_service.Model
{
    public class ChannelModel
    {
        public int Id { get; set; }
        public UserModel Channel { get; set; }
        public List<VideoModel> Videos { get; set; }
    } 
}
