using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Collections.Generic;
using System.Transactions;
using Es.Udc.DotNet.PracticaMaD.Model;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.CategoryDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.UserProfileDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.PostDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.CommentDao;
using Es.Udc.DotNet.PracticaMaD.Model.Services.PostService;
using Es.Udc.DotNet.PracticaMaD.Model.Services.UserService.Util;
using Es.Udc.DotNet.PracticaMaD.Model.Services.Utils;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;

namespace Es.Udc.DotNet.PracticaMaD.Test.Services
{
    [TestClass]
    public class IPostServiceTest
    {

        private static IKernel kernel;
        private static IPostService postService;
        private static ICategoryDao categoryDao;
        private static IPostDao postDao;
        private static IUserProfileDao userProfileDao;
        private static ICommentDao commentDao;

        //USER
        private const String loginName = "usuario";
        private const String clearPassword = "password";
        private const String firstName = "name";
        private const String lastName = "lastName";
        private const String email = "user@udc.es";
        private const String language = "es";
        private const String country = "ES";
        //POST
        private const String title = "img";
        private const String imgTest = "image";
        private const String description = "description";
        private const String descriptionFind = "description find";
        private readonly DateTime date = new DateTime(2023, 12, 31, 0, 0, 0);

        //CATEGORY
        private const String categoryName = "category";
        private const String categoryName2 = "category2";

        //COMMENT
        private const String commentText = "Comentario";
        private String loginName2 = "usuario";

        private const long NON_EXISTENT_ID = -1;

        private TransactionScope transactionScope;

        private TestContext testContextInstance;



        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            kernel = TestManager.ConfigureNInjectKernel();
            postService = kernel.Get<IPostService>();
            categoryDao = kernel.Get<ICategoryDao>();
            userProfileDao = kernel.Get<IUserProfileDao>();
            postDao = kernel.Get<IPostDao>();
            commentDao = kernel.Get<ICommentDao>();
        }

        [ClassCleanup]
        public static void MyClassCleanup()
        {
            TestManager.ClearNInjectKernel(kernel);
        }

        [TestInitialize]
        public void MyTestInitialize()
        {
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
        }

        #endregion


        #region Aux functions
        private static UserProfile CreateUserProfile(String loginName, String clearPassword, String firstName,
    String lastName, String email, String language, String country, int aux)
        {
            UserProfile userProfile = new UserProfile();
            userProfile.loginName = loginName + aux.ToString();
            userProfile.enPassword = PasswordEncrypter.Crypt(clearPassword + aux.ToString());
            userProfile.firstName = firstName + aux.ToString();
            userProfile.lastName = lastName + aux.ToString();
            userProfile.email = email + aux.ToString();
            userProfile.language = language;
            userProfile.country = country;
            userProfile.numFollowers = 2;
            userProfile.numFollows = 2;

            userProfileDao.Create(userProfile);

            return userProfile;
        }

        private static Category CreateCategory(String categoryName)
        {

            Category category = new Category();

            category.categoryName = categoryName;

            categoryDao.Create(category);

            return category;
        }

        private static Post CreatePost(String title, String img, String description,
            DateTime date, long usrId, long categoryId, int aux)
        {
            Post post = new Post();
            post.title = title + usrId.ToString() + aux.ToString();
            post.img = img + usrId.ToString() + aux.ToString();
            post.description = description + " " + usrId.ToString() + aux.ToString();
            post.date = date.AddDays(usrId).AddMinutes(aux);
            post.usrId = usrId;
            post.categoryId = categoryId;

            postDao.Create(post);

            return post;
        }

        private static Comment CreateComment(String text, DateTime date, long usrId, long postId, int aux)
        {
            Comment comment = new Comment();

            comment.text = text;
            comment.date = date.AddDays(usrId).AddMinutes(aux);
            comment.usrId = usrId;
            comment.postId = postId;

            commentDao.Create(comment);

            return comment;
        }

        #endregion


        #region Tests FUN-5
        [TestMethod]
        public void CreatePostTest()
        {
            using (var scope = new TransactionScope())
            {
                UserProfile user = CreateUserProfile(loginName, clearPassword, firstName, lastName, email, language, country, 0);
                Category cat = CreateCategory(categoryName);
                Post post = CreatePost(title, imgTest, description, date, user.usrId, cat.categoryId, 0);
                loginName2 = "gola";

                Assert.IsTrue(post.title == title + user.usrId.ToString() + 0.ToString());
                Assert.IsTrue(post.img == imgTest + user.usrId.ToString() + 0.ToString());
                Assert.IsTrue(post.description == description + " " + user.usrId.ToString() + 0.ToString());
                Assert.IsTrue(post.date == date.AddDays(user.usrId));
                Assert.IsTrue(post.likes == 0);
                Assert.IsTrue(post.diaphragmOpen == null);
                Assert.IsTrue(post.timeExp == null);
                Assert.IsTrue(post.ISO == null);
                Assert.IsTrue(post.whiteBal == null);
                Assert.IsTrue(post.usrId == user.usrId);
                Assert.IsTrue(post.categoryId == cat.categoryId);

            }

        }

        [TestMethod]
        public void CreatePostWithEXIFDataTest()
        {
            using (var scope = new TransactionScope())
            {
                UserProfile user = CreateUserProfile(loginName2, clearPassword, firstName, lastName, email, language, country, 0);
                Category cat = CreateCategory(categoryName);
                Post post = new Post();
                
                post.title = title;
                post.img = imgTest;
                post.description = description;
                post.date = date;
                post.likes = 0;
                post.diaphragmOpen = 1.0;
                post.timeExp = 1.0;
                post.ISO = 1.0;
                post.whiteBal = 1.0;
                post.categoryId = cat.categoryId;
                post.usrId = user.usrId;

                postDao.Create(post);

                Post foundPost = postDao.FindPostByKeyword(title, 0, 1)[0];

                Assert.IsTrue(foundPost.title == title);
                Assert.IsTrue(foundPost.img == imgTest);
                Assert.IsTrue(foundPost.description == description);
                Assert.IsTrue(foundPost.date == date);
                Assert.IsTrue(foundPost.likes == 0);
                Assert.IsTrue(foundPost.diaphragmOpen == 1.0);
                Assert.IsTrue(foundPost.timeExp == 1.0);
                Assert.IsTrue(foundPost.ISO == 1.0);
                Assert.IsTrue(foundPost.whiteBal == 1.0);
                Assert.IsTrue(foundPost.usrId == user.usrId);
                Assert.IsTrue(foundPost.categoryId == cat.categoryId);

            }

        }


        [TestMethod]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void DeletePostTest()
        {
            using (var scope = new TransactionScope())
            {

                UserProfile user = CreateUserProfile(loginName, clearPassword, firstName, lastName, email, language, country, 0);
                Category cat = CreateCategory(categoryName);
                Post post = CreatePost(title, imgTest, description, date, user.usrId, cat.categoryId, 0);

                postDao.Remove(post.postId);

                postDao.Find(post.postId);
            }

        }

        #endregion

        #region Tests FUN-6

        [TestMethod]
        public void FindPostByKeywordTest()
        {
            using (var scope = new TransactionScope())
            {
                UserProfile user = CreateUserProfile(loginName, clearPassword, firstName, lastName, email, language, country, 0);
                Category cat = CreateCategory(categoryName);
                Post post1 = CreatePost(title, imgTest, description, date, user.usrId, cat.categoryId, 0);
                PostPreview postPreview1 = new PostPreview(post1.postId, user.usrId, post1.title, post1.img, post1.likes, user.loginName, false);
                Post post2 = CreatePost(title, imgTest, description, date, user.usrId, cat.categoryId, 1);
                PostPreview postPreview2 = new PostPreview(post2.postId, user.usrId, post2.title, post2.img, post2.likes, user.loginName, false);
                Post post3 = CreatePost(title, imgTest, description, date, user.usrId, cat.categoryId, 2);
                PostPreview postPreview3 = new PostPreview(post3.postId, user.usrId, post3.title, post3.img, post3.likes, user.loginName, false);

                Block<PostPreview> postPreview = postService.FindPosts("img", 0, 5);

                Assert.AreEqual(postPreview1, postPreview.Items[0]);
                Assert.AreEqual(postPreview2, postPreview.Items[1]);
                Assert.AreEqual(postPreview3, postPreview.Items[2]);
            }
        }

        [TestMethod]
        public void FindPostByKeywordTest2()
        {
            using (var scope = new TransactionScope())
            {

                UserProfile user = CreateUserProfile(loginName, clearPassword, firstName, lastName, email, language, country, 0);
                Category cat = CreateCategory(categoryName);
                Post post1 = CreatePost(title, imgTest, descriptionFind, date, user.usrId, cat.categoryId, 0);
                PostPreview postPreview1 = new PostPreview(post1.postId, user.usrId, post1.title, post1.img, post1.likes, user.loginName, false);
                Post post2 = CreatePost(title, imgTest, description, date, user.usrId, cat.categoryId, 0);
                PostPreview postPreview2 = new PostPreview(post2.postId, user.usrId, post2.title, post2.img, post2.likes, user.loginName, false);
                Post post3 = CreatePost(title, imgTest, descriptionFind, date, user.usrId, cat.categoryId, 0);
                PostPreview postPreview3 = new PostPreview(post3.postId, user.usrId, post3.title, post3.img, post3.likes, user.loginName, false);

                Block<PostPreview> postPreview = postService.FindPosts("find", 0, 5);

                Assert.AreEqual(postPreview1, postPreview.Items[0]);
                Assert.AreEqual(postPreview3, postPreview.Items[1]);
            }
        }

        public Block<PostPreview> poste; 
        [TestMethod]
        public void FindPostByKeywordNotFoundTest()
        {
            using (var scope = new TransactionScope())
            {
                poste = postService.FindPosts("no existe", 0, 5);
                Assert.IsTrue(poste.Items.Count == 0);
            }
            Console.WriteLine(poste.Items.Count.ToString() +"holaaaaaaa");
            
        }
        [TestMethod]
        public void FindPostByKeywordAndCategoryTest()
        {
            using (var scope = new TransactionScope())
            {
                UserProfile user = CreateUserProfile(loginName, clearPassword, firstName, lastName, email, language, country, 0);
                Category cat = CreateCategory(categoryName);
                Category cat2 = CreateCategory(categoryName2);
                Post post1 = CreatePost(title, imgTest, descriptionFind, date, user.usrId, cat.categoryId, 0);
                PostPreview postPreview1 = new PostPreview(post1.postId, user.usrId, post1.title, post1.img, post1.likes, user.loginName, false);
                Post post2 = CreatePost(title, imgTest, descriptionFind, date, user.usrId, cat2.categoryId, 0);
                PostPreview postPreview2 = new PostPreview(post2.postId, user.usrId, post2.title, post2.img, post2.likes, user.loginName, false);
                Post post3 = CreatePost(title, imgTest, descriptionFind, date, user.usrId, cat.categoryId, 0);
                PostPreview postPreview3 = new PostPreview(post3.postId, user.usrId, post3.title, post3.img, post3.likes, user.loginName, false);

                Block<PostPreview> postPreview = postService.FindPosts("find", cat2.categoryName, 0, 5);

                Assert.AreEqual(postPreview2, postPreview.Items[0]);
            }

        }

        [TestMethod]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void FindPostByKeywordAndCategoryDoesntExistTest()
        {
            using (var scope = new TransactionScope())
            {
                Block<PostPreview> posts = postService.FindPosts("description", "Category3", 0, 5);
            }
        }

        #endregion

        #region Tests FUN-7

        [TestMethod]
        public void LikeTest()
        {
            using (var scope = new TransactionScope())
            {
                UserProfile user = CreateUserProfile(loginName, clearPassword, firstName, lastName, email, language, country, 0);
                UserProfile user2 = CreateUserProfile(loginName + "2", clearPassword, firstName, lastName, email, language, country, 0);
                Category cat = CreateCategory(categoryName);
                Post post = CreatePost(title, imgTest, description, date, user.usrId, cat.categoryId, 0);

                postService.likePost(user.usrId, post.postId);
                postService.likePost(user2.usrId, post.postId);

                Post foundPost = postDao.Find(post.postId);

                Assert.IsTrue(foundPost.likes == 2);

            }
        }

        [TestMethod]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void likeInstanceNotFoundExceptionTest()
        {
            postService.likePost(1, 20);
        }

        #endregion

        #region Tests FUN-8
        [TestMethod]
        public void addCommentTest()
        {
            using (var scope = new TransactionScope())
            {
                UserProfile user = CreateUserProfile(loginName, clearPassword, firstName, lastName, email, language, country, 0);
                Category cat = CreateCategory(categoryName);
                Post post = CreatePost(title, imgTest, description, date, user.usrId, cat.categoryId, 0);
                Comment comment = CreateComment(commentText, date, user.usrId, post.postId, 1);

                Assert.IsTrue(comment.text == commentText);
            }

        }

        [TestMethod]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void addCommentInstanceNotFoundExceptionTest()
        {

            using (var scope = new TransactionScope())
            {
                String textComment = "Comentario prueba";

                postService.addComent(1, 20, textComment);
            }
        }

        [TestMethod]
        public void editCommentTest()
        {
            using (var scope = new TransactionScope())
            {
                UserProfile user = CreateUserProfile(loginName, clearPassword, firstName, lastName, email, language, country, 0);
                Category cat = CreateCategory(categoryName);
                Post post = CreatePost(title, imgTest, description, date, user.usrId, cat.categoryId, 0);
                Comment comment = CreateComment(commentText, date, user.usrId, post.postId, 1);

                String textEdited = "Comentario editado";

                Comment commentEdited = postService.EditComment(comment.usrId, comment.commentId, textEdited);

                Assert.IsTrue(comment.text ==  textEdited);
            }


        }

        [TestMethod]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void deleteCommentTest()
        {
            using (var scope = new TransactionScope())
            {
                UserProfile user = CreateUserProfile(loginName, clearPassword, firstName, lastName, email, language, country, 0);
                Category cat = CreateCategory(categoryName);
                Post post = CreatePost(title, imgTest, description, date, user.usrId, cat.categoryId, 0);
                Comment comment = CreateComment(commentText, date, user.usrId, post.postId, 1);

                postService.DeleteComment(comment.usrId, comment.commentId);

                commentDao.FindById(comment.usrId);
            }
        }

        #endregion

        #region Tests FUN-9


        [TestMethod]
        public void ShowCommentsTest()
        {
            using (var scope = new TransactionScope())
            {
                UserProfile user = CreateUserProfile(loginName, clearPassword, firstName, lastName, email, language, country, 0);
                Category cat = CreateCategory(categoryName);
                Post post = CreatePost(title, imgTest, description, date, user.usrId, cat.categoryId, 0);
                Comment comment1 = CreateComment("Comentario 1", DateTime.Now, user.usrId, post.postId, 1);
                Comment comment2 = CreateComment("Comentario 2", DateTime.Now, user.usrId, post.postId, 2);
                Comment comment3 = CreateComment("Comentario 3", DateTime.Now, user.usrId, post.postId, 3);

                Block<Comment> comments = postService.ShowComments(post.postId, 0, 5);

                // No se crear un Equals en los DAOs usando el EntityFramework
                Assert.IsTrue(
                    comments.Items.Count == 3 &&
                    comments.Items[0].text == comment3.text &&
                    comments.Items[1].text == comment2.text &&
                    comments.Items[2].text == comment1.text 
                    );
            }
        }

        [TestMethod]
        public void ShowCommentsEmptyTest()
        {
            using (var scope = new TransactionScope())
            {
                Block<Comment> comments = postService.ShowComments(-1, 0, 5);
                Assert.IsTrue(comments.Items.Count == 0);
            }
        }

        #endregion

    }
}
