using System;
using System.Collections.Generic;

namespace video_service.Model
{
    public class ReplyModel
    {

        public string ReplyModelId { get; set; }
        public string CommentId { get; set; }
        public string ReplyText { get; set; }
        public UserModel User { get; set; }
        public DateTime dateTime { get; set; }
        public List<LikeModel> LikeList { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }

    }
}
