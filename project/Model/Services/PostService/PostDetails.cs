using System;
using System.Collections.Generic;

namespace Es.Udc.DotNet.PracticaMaD.Model.Services.PostService
{
    public class PostDetails
    {

        public long postId { get; set; }
        public string loginName { get; set; }

        public long userId { get; set; }

        public string title { get; set; }

        public string image { get; set; }

        public int likes { get; set; }

        public string description;

		public double? diaphragmOpen;

		public double? timeExp;

		public double? whiteBal;

		public double? iso;

		public string categoryName;

		public DateTime date;

        public bool hasComments;

        public PostDetails(long postId, string loginName, long userId, string title, string image, string description, int likes, double? diaphragmOpen, double? timeExp, double? whiteBal, double? iso, string categoryName, DateTime date, bool hasComments)
        {
            this.postId = postId;
            this.loginName = loginName;
            this.userId = userId;
            this.title = title;
            this.image = image;
			this.description = description;
			this.likes = likes;
			this.diaphragmOpen = diaphragmOpen;
			this.timeExp = timeExp;
			this.whiteBal = whiteBal;
			this.iso = iso;
			this.categoryName = categoryName;
			this.date = date;
            this.hasComments = hasComments;
        }

        public override bool Equals(object obj)
        {
            return obj is PostDetails details &&
                   postId == details.postId &&
                   loginName == details.loginName &&
                   userId == details.userId &&
                   title == details.title &&
                   image == details.image &&
                   likes == details.likes &&
                   description == details.description &&
                   diaphragmOpen == details.diaphragmOpen &&
                   timeExp == details.timeExp &&
                   whiteBal == details.whiteBal &&
                   iso == details.iso &&
                   categoryName == details.categoryName &&
                   date == details.date &&
                   hasComments == details.hasComments;
        }

        public override int GetHashCode()
        {
            int hashCode = 1095470051;
            hashCode = hashCode * -1521134295 + postId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(loginName);
            hashCode = hashCode * -1521134295 + userId.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(image);
            hashCode = hashCode * -1521134295 + likes.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(description);
            hashCode = hashCode * -1521134295 + diaphragmOpen.GetHashCode();
            hashCode = hashCode * -1521134295 + timeExp.GetHashCode();
            hashCode = hashCode * -1521134295 + whiteBal.GetHashCode();
            hashCode = hashCode * -1521134295 + iso.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(categoryName);
            hashCode = hashCode * -1521134295 + date.GetHashCode();
            hashCode = hashCode * -1521134295 + hasComments.GetHashCode();
            return hashCode;
        }
    }
}
