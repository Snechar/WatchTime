using System;

namespace video_service.Model
{
    public class ReplyModel
    {
        public string ReplyId { get; set; }
        public string CommentId { get; set; }
        public string ReplyText { get; set; }
        public UserModel User { get; set; }
        public DateTime dateTime { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }

    }
}
