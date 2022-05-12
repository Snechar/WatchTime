using Microsoft.EntityFrameworkCore;

namespace video_service.Model
{
    public class LikeModel
    {
        public int Id { get; set; }
        public UserModel User { get; set; }
        public string VideoModelID { get; set; }
        public string Type { get; set; }


    }
}
