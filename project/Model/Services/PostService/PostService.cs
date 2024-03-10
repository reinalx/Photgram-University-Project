using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.CategoryDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.CommentDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.LikePubDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.PostDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.UserProfileDao;
using Es.Udc.DotNet.PracticaMaD.Model.Services.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Caching;
using Ninject;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.TagDao;
using Es.Udc.DotNet.PracticaMaD.Model.Objetos;

namespace Es.Udc.DotNet.PracticaMaD.Model.Services.PostService
{
    public class PostService : IPostService
    {
        [Inject]
        public IPostDao postDao { private get; set; }

        [Inject]
        public IUserProfileDao userProfileDao { private get; set; }

        [Inject]
        public ICategoryDao categoryDao { private get; set; }

        [Inject]
        public ILikePubDao likePubDao { private get; set; }

        [Inject]
        public ICommentDao commentDao { private get; set; }

        [Inject]
        public ITagDao tagDao { private get; set; }

        private MemoryCache _Cache = new MemoryCache("cache");
        public MemoryCache Cache { get => _Cache; }

        [Transactional]
        public long CreatePost(string title, string img, string description, long categoryId, long usrId,
            double diaphragmOpen, double timeExp, double ISO, double whiteBal)
        {
            Post postNew = new Post();

            postNew.title = title;
            postNew.img = img;
            postNew.description = description;
            postNew.date = DateTime.Now;
            postNew.likes = 0;
            postNew.diaphragmOpen = diaphragmOpen;
            postNew.timeExp = timeExp;
            postNew.ISO = ISO;
            postNew.whiteBal = whiteBal;
            postNew.categoryId = categoryId;
            postNew.usrId = usrId;

            postDao.Create(postNew);

            return postNew.postId;
        }


        //      FUN 6 -Búsqueda de imagenes
        //
        //      Version provisional
        //      Devuelve el tipo PostPreview, pero no es eficiente porque recupera
        //      de la base de datos todo y despues lo convierte al tipo PostPreview

        //      Estas dos funciones son parches por ahora

        [Transactional]
        public Block<PostPreview> FindPosts(string keyword, string categoryName, int startIndex, int count)
        {
            long categoryId = categoryDao.FindByCategoryName(categoryName).categoryId;
            bool existsMorePosts ;

            CacheManager search = new CacheManager(keyword, null, startIndex, count);
            List<CacheManager> lastSearches = (List<CacheManager>)Cache["lastSearches"];

            List<PostPreview> postsPreview = new List<PostPreview>();
            Block<PostPreview> blockPreview;

            if (lastSearches == null)
                lastSearches = new List<CacheManager>();
            if ( lastSearches.Contains(search))
            {
                blockPreview = (Block<PostPreview>)_Cache.Get(keyword + categoryId.ToString());
                lastSearches.Remove(search);

            }
            else
            {
                if (keyword == null) keyword = "";

                //recuperamos un elemento mas para comprobar si todavia hay mas
                List<Post> posts = postDao.FindPostByKeyword(keyword, startIndex, count + 1);
                postsPreview = new List<PostPreview>();

                foreach (Post post in posts)
                {
                    string loginName = userProfileDao.Find(post.usrId).loginName;
                    bool hasComments = (post.Comment.Count != 0);
                    postsPreview.Add(new PostPreview(post.postId, post.usrId, post.title, post.img, post.likes, loginName, hasComments));
                }

                //comprobamos si quedan todavia mas posts, y borramos el ultimo que habiamos
                //cargado de mas de la lista
                existsMorePosts = (posts.Count == count + 1);
                if (existsMorePosts) postsPreview.RemoveAt(count);

                blockPreview = new Block<PostPreview>(postsPreview, existsMorePosts);
            }

            lastSearches.Add(search);

            _Cache.Add(new CacheItem("lastSearches", lastSearches), new CacheItemPolicy());

            _Cache.Add(new CacheItem(keyword, blockPreview), new CacheItemPolicy());


            return blockPreview;
        }

        [Transactional]
        public Block<PostPreview> FindPosts(string keyword, int startIndex, int count)
        {

            bool existsMorePosts;

            CacheManager search = new CacheManager(keyword, null, startIndex, count);
            List<CacheManager> lastSearches = (List<CacheManager>)Cache["lastSearches"];

            List<PostPreview> postsPreview = new List<PostPreview>();

            Block<PostPreview> blockPreview;

            if (keyword == null) keyword = "";
            if (lastSearches == null)
                lastSearches = new List<CacheManager>();
            if (lastSearches.Contains(search))
            {
                blockPreview = (Block<PostPreview>)_Cache.Get(keyword);
                lastSearches.Remove(search);
            }
            else
            {
                List<Post> posts = postDao.FindPostByKeyword(keyword, startIndex, count + 1);

                foreach (Post post in posts)
                {
                    string loginName = userProfileDao.Find(post.usrId).loginName;
                    bool hasComments = (post.Comment.Count != 0);
                    postsPreview.Add(new PostPreview(post.postId, post.usrId, post.title, post.img, post.likes, loginName, hasComments));
                }

                existsMorePosts = (posts.Count == count + 1);
                if (existsMorePosts) postsPreview.RemoveAt(count);

                blockPreview = new Block<PostPreview>(postsPreview, existsMorePosts);
            }
            lastSearches.Add(search);

            _Cache.Add(new CacheItem("lastSearches", lastSearches), new CacheItemPolicy());

            _Cache.Add(new CacheItem(keyword, blockPreview), new CacheItemPolicy());



            return blockPreview;
        }

        [Transactional]
        public Block<PostPreview> FindPostsByTags(long tagId, int startIndex, int count)
        {
            //recuperamos un elemento mas para comprobar si todavia hay mas
            List<Post> posts = postDao.FindPostByTag( tagId, startIndex, count + 1);
            List<PostPreview> postsPreview = new List<PostPreview>();

            foreach (Post post in posts)
            {
                string loginName = userProfileDao.Find(post.usrId).loginName;
                bool hasComments = (post.Comment.Count != 0);
                postsPreview.Add(new PostPreview(post.postId, post.usrId, post.title, post.img, post.likes, loginName, hasComments));
            }

            //comprobamos si quedan todavia mas posts, y borramos el ultimo que habiamos
            //cargado de mas de la lista
            bool existsMorePosts = (posts.Count == count + 1);
            if (existsMorePosts) postsPreview.RemoveAt(count);

            return new Block<PostPreview>(postsPreview, existsMorePosts);
        }

        [Transactional]
        public List<Category> FindAllCategories()
        {
            return categoryDao.GetAllElements();
        }

        [Transactional]
        public Category GetCategoryByName(string categoryName)
        {
            return categoryDao.FindByCategoryName(categoryName);
        }

        [Transactional]
        public PostDetails GetPostDetails(long postId)
        {
            Post post = postDao.Find(postId);
            string categoryName = categoryDao.Find(post.categoryId).categoryName;
            string loginName = userProfileDao.Find(post.usrId).loginName;

            PostDetails postDetails = new PostDetails
                (
                post.postId,
                loginName,
                post.usrId,
                post.title,
                post.img,
                post.description,
                post.likes,
                post.diaphragmOpen,
                post.timeExp,
                post.whiteBal,
                post.ISO,
                categoryName,
                post.date,
                post.Comment.Count != 0
                );

            return postDetails;
        }

        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        public void likePost(long userId, long postId)
        {

            Post post = postDao.Find(postId);
            try
            {
                LikePub likePub = likePubDao.FindByUserAndPost(userId, postId);
                likePubDao.Remove(likePub.likeId);

                post.likes--;
                postDao.Update(post);


            }
            catch (InstanceNotFoundException)
            {
                LikePub like = new LikePub();
                like.usrId = userId;
                like.postId = postId;
                likePubDao.Create(like);

                post.likes++;
                postDao.Update(post);
            }

        }

        public bool isPostLiked(long userId, long postId)
        {
            return likePubDao.ExistsByUserIdAndPostPostId(userId, postId);
        }

        [Transactional]
        public Comment addComent(long userId, long postId, string commentText)
        {

            Comment comment = new Comment();

            comment.postId = postDao.Find(postId).postId;
            comment.usrId = userId;
            comment.date = DateTime.Now;
            comment.text = commentText;

            commentDao.Create(comment);

            return comment;
        }


        /// <exception cref="UnauthorizedAccessException"/>

        [Transactional]
        public void DeleteComment(long userId, long commentId)
        {
            Comment comment = commentDao.FindById(commentId);

            if (!comment.usrId.Equals(userId))
            {
                throw new UnauthorizedAccessException();

            }

            commentDao.Remove(commentId);

        }

        /// <exception cref="UnauthorizedAccessException"/>
        [Transactional]
        public Comment EditComment(long userId, long commentId, string commentTextNew)
        {
            Comment comment = commentDao.FindById(commentId);

            if (!comment.usrId.Equals(userId))
            {
                throw new UnauthorizedAccessException();

            }
            comment.text = commentTextNew;
            commentDao.Update(comment);
            return comment;
        }
        

        //      FUN 9 -Ver comentarios de un post
        [Transactional]
        public Block<Comment> ShowComments(long postId, int startIndex, int count)
        {

            List<Comment> comments = commentDao.FindByPublicationId(postId, startIndex, count + 1);

            bool existsMoreComments = (comments.Count == count + 1);
            if (existsMoreComments) comments.RemoveAt(count);

            return new Block<Comment>(comments, existsMoreComments);
        }

        /// <exception cref="UnauthorizedAccessException"/>
        [Transactional] 
        public void DeletePost(long userId, long postId)
        {
            Post post = postDao.Find(postId);

            if (!post.usrId.Equals(userId))
            {
                throw new UnauthorizedAccessException();

            }
            foreach (Tag tag in post.Tag.ToList())
            {
                tag.timesUsed--;
                tagDao.Update(tag);
                post.Tag.Remove(tag);

            }

            if (post.LikePub.Count != 0)
            {
                List<LikePub> likesToRemove = post.LikePub.ToList();
                foreach (LikePub like in likesToRemove)
                {
                    likePubDao.Remove(like.likeId);
                }
            }
            if (post.Comment.Count != 0)
            {
                List<Comment> commentsToRemove = post.Comment.ToList();
                foreach (Comment c in commentsToRemove)
                {
                    commentDao.Remove(c.commentId);
                }
            }

            

            postDao.Remove(postId);

        }

        public Comment GetComment(long commentId)
        {
            return commentDao.FindById(commentId);
        }

        [Transactional]
        public void TagPost(long postId, List<Tag> tags)
        {
            Post post = postDao.Find(postId);

            foreach (Tag tag in tags)
            {
                if (!tagDao.existsByTagName(tag.tagName))
                {
                    tag.timesUsed = 1;
                    tagDao.Create(tag);
                }
                else
                {
                    tag.timesUsed += 1;
                }
                post.Tag.Add(tag);
            }
            postDao.Update(post);
        }
    }


}
