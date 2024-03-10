using System;
using Ninject;

using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.CategoryDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.PostDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.CommentDao;
using Es.Udc.DotNet.PracticaMaD.Model.Services.Utils;
using System.Collections.Generic;
using Es.Udc.DotNet.ModelUtil.Exceptions;

namespace Es.Udc.DotNet.PracticaMaD.Model.Services.PostService
{
    public interface IPostService
    {
        [Inject]
        IPostDao postDao { set; }

        [Inject]
        ICategoryDao categoryDao { set; }


        [Transactional]
        long CreatePost(string title, string img, string description, long categoryId, long usrId,
            double diaphragmOpen, double timeExp, double ISO, double whiteBal);

        [Transactional]
        Block<PostPreview> FindPosts(string keyword, string categoryName, int startIndex, int count);

        [Transactional]
        Block<PostPreview> FindPosts(string keyword, int startIndex, int count);

        [Transactional]
        Block<PostPreview> FindPostsByTags(long tagId, int startIndex, int count);

        
       
        [Transactional]
        List<Category> FindAllCategories();

        [Transactional]
        Category GetCategoryByName(string categoryName);

        [Transactional]
        PostDetails GetPostDetails(long postId);

        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        void likePost(long userId, long postId);

        [Transactional]
        bool isPostLiked(long userId, long postId);

        [Transactional]
        Comment addComent(long userId, long postId, string commentText);

        /// <exception cref="UnauthorizedAccessException"/>
        [Transactional]
        void DeleteComment(long userId, long commentId);

        /// <exception cref="UnauthorizedAccessException"/>
        [Transactional]
        Comment EditComment(long userId, long commentId, string commentTextNew);

        [Transactional]
        Comment GetComment(long commentId);

        [Transactional]
        Block<Comment> ShowComments(long postId, int startIndex, int count);

        /// <exception cref="UnauthorizedAccessException"/>
        [Transactional]
        void DeletePost(long userId, long postId);
        
        [Transactional]
        void TagPost(long postId, List<Tag> tags);
    }
}
