using System;
using System.Collections.Generic;

namespace video_service.Model
{
    public class CommentModel
    {
        public string Id { get; set; }
        public string VideoId { get; set; }

        public UserModel User { get; set; }

        public DateTime dateTime { get; set; }

        public string Comment { get; set; }

        public List<ReplyModel> Replies { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }
        public List<LikeModel> LikeList { get; set; }
    }
}
