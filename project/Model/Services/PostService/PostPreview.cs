using System;
using System.Collections.Generic;

namespace Es.Udc.DotNet.PracticaMaD.Model.Services.PostService
{
    [Serializable()]
    public class PostPreview
    {

        public long postId { get; set; }

        public long userId { get; set; }

        public string title { get; set; }

        public string image { get; set; }

        public int likes { get; set; }

        public string loginName { get; set; }

        public bool hasComments { get; set; }

        public PostPreview(long postId, long userId, string title, string image, int likes, string loginName, bool hasComments)
        {
            this.postId = postId;
            this.userId = userId;
            this.title = title;
            this.image = image;
            this.likes = likes;
            this.loginName = loginName;
            this.hasComments = hasComments;
        }

        public override bool Equals(object obj)
        {
            return obj is PostPreview preview &&
                   postId == preview.postId &&
                   userId == preview.userId &&
                   title == preview.title &&
                   image == preview.image &&
                   likes == preview.likes &&
                   loginName == preview.loginName &&
                   hasComments == preview.hasComments;
        }

        public override int GetHashCode()
        {
            int hashCode = -1180714375;
            hashCode = hashCode * -1521134295 + postId.GetHashCode();
            hashCode = hashCode * -1521134295 + userId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(image);
            hashCode = hashCode * -1521134295 + likes.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(loginName);
            hashCode = hashCode * -1521134295 + hasComments.GetHashCode();
            return hashCode;
        }
    }
}
