using System;
using System.Collections.Generic;

namespace video_service.Model
{
    public class VideoModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime dateTime { get; set; }
        public UserModel Author { get; set; }
        public List<CommentModel> Comments { get; set; }
        public List<LikeModel> LikeList { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public List<QualityVotesModel> QualityVotes { get; set; }
        public double Quality { get; set; }

    }
}
