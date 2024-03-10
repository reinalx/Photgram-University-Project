using Es.Udc.DotNet.PracticaMaD.Model.DAOs.UserProfileDao;
using Es.Udc.DotNet.PracticaMaD.Model.Services.UserService;
using Es.Udc.DotNet.PracticaMaD.Model.Services.UserService.Exceptions;
using Es.Udc.DotNet.PracticaMaD.Model.Services.UserService.Util;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.FollowDao;
using Es.Udc.DotNet.PracticaMaD.Model;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.CategoryDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.PostDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.TagDao;
using Es.Udc.DotNet.PracticaMaD.Model.Services.PostService;
using Es.Udc.DotNet.PracticaMaD.Model.Services.TagService;

namespace Es.Udc.DotNet.PracticaMaD.Test
{
    /// <summary>s
    /// This is a test class for IUserServiceTest and is intended to contain all IUserServiceTest
    /// Unit Tests
    /// </summary>
    [TestClass]
    public class ITagServiceTest
    {
        private static IKernel kernel;
        private static ITagService tagService;
        private static IPostService postService;

        private static IUserProfileDao userProfileDao;
        private static ICategoryDao categoryDao;
        private static IPostDao postDao;
        private static ITagDao tagDao;

        private TransactionScope transactionScope;

        private TestContext testContextInstance;

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

        //TAG
        private const String tagName = "testTag";
        private const String NOT_EXISTING_TAGNAME = "doesntExist";

        private const int count = 5;
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            kernel = TestManager.ConfigureNInjectKernel();

            postDao = kernel.Get<IPostDao>();
            userProfileDao = kernel.Get<IUserProfileDao>();
            categoryDao = kernel.Get<ICategoryDao>();
            tagService = kernel.Get<ITagService>();
            postService = kernel.Get<IPostService>();
            tagDao = kernel.Get<ITagDao>();
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

        #region function test tags

        #region aux functions

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
            post.img = imgTest + usrId.ToString() + aux.ToString();
            post.description = description + usrId.ToString() + aux.ToString();
            post.date = date.AddDays(usrId).AddMinutes(aux);
            post.usrId = usrId;
            post.categoryId = categoryId;

            postDao.Create(post);

            return post;
        }

        #endregion

        [TestMethod]
        public void CreateTagTest()
        {
            using (var scope = new TransactionScope())
            {
                Tag tag = tagService.CreateTag(tagName);

                Assert.IsTrue(tag.tagName == tagName);
            }

        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateInstanceException))]
        public void CreateTagAlreadyExistTest()
        {
            using (var scope = new TransactionScope())
            {
                tagService.CreateTag(tagName);
                tagService.CreateTag(tagName);
            }
        }


        [TestMethod]
        public void FindByTagNameTest()
        {
            using (var scope = new TransactionScope())
            {
                Tag tag = tagService.CreateTag(tagName);

                Tag tagTest = tagService.FindTagByName(tagName);

                Assert.IsTrue(tag.tagName == tagTest.tagName);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void FindByTagNameIncorrect()
        {
            using (var scope = new TransactionScope())
            {
                tagService.FindTagByName(NOT_EXISTING_TAGNAME);
            }
        }

        public List<Tag> tagss = new List<Tag>();
        [TestMethod]
        public void GetAllTagsTest()
        {
            using (var scope = new TransactionScope())
            {
                List<Tag> tags = new List<Tag>();
                for (int i = 0; i < count; i++)
                {
                    tagss.Add(tagService.CreateTag(tagName + i.ToString()));
                }

                List<Tag> tagsTest = tagService.GetAllTags();

                for (int i = 0; i < count; i++)
                {
                    Assert.AreEqual(tagsTest.ElementAt(i), tagss.ElementAt(i));
                }
            }

            Console.WriteLine(tagss.ElementAt(2).tagName);
        }

        [TestMethod]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void GetAllTagsNotExistTagTest()
        {
            using (var scope = new TransactionScope())
            {
                tagService.GetAllTags();
            }
        }

        [TestMethod]
        public void TestPostTest()
        {
            using (var scope = new TransactionScope())
            {
                UserProfile user = CreateUserProfile(loginName, clearPassword, firstName, lastName, email, language,
                    country, 1);
                Category category = CreateCategory(categoryName);
                Post post = CreatePost(title, imgTest, description, date, user.usrId, category.categoryId, 1);

                List<Tag> tags = new List<Tag>();
                for (int i = 0; i < count; i++)
                {
                    tags.Add(tagService.CreateTag(loginName + i.ToString()));
                }

                postService.TagPost(post.postId, tags);

                for (int i = 0; i < count; i++)
                {
                    Assert.IsTrue(post.Tag.ElementAt(i).tagName == tags.ElementAt(i).tagName);
                }
            }


            #endregion
        }
    }
}