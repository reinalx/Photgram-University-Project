using Microsoft.VisualStudio.TestTools.UnitTesting;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.FollowDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.PostDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.UserProfileDao;
using Es.Udc.DotNet.PracticaMaD.Model.Services.FeedUserService;

using System;
using Ninject;
using System.Transactions;
using Es.Udc.DotNet.PracticaMaD.Model.Services.Utils;
using Es.Udc.DotNet.PracticaMaD.Model;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System.Linq;
using Es.Udc.DotNet.PracticaMaD.Model.Services.UserService;
using Es.Udc.DotNet.PracticaMaD.Model.Services.UserService.Util;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.CategoryDao;
using System.Collections.Generic;

namespace Es.Udc.DotNet.PracticaMaD.Test.Services
{
    [TestClass]
    public class IFeedUserServiceTest
    {
        private static IKernel kernel;
        private static IFeedUserService feedUserService;
        private static IUserProfileDao userProfileDao;
        private static IPostDao postDao;
        private static IFollowDao followDao;
        private static ICategoryDao categoryDao;

        //Variables para la creacion de objetos en la BD

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
        private readonly DateTime date = new DateTime(2023, 11, 10, 20, 30, 0);

        //CATEGORY
        private const String categoryName = "category";
        

        private const long NON_EXISTENT_ID = -1;
        private const string NON_EXISTENT_NAME = "non_existent_name";
        

        
        private TransactionScope transaction;

        /// <summary>
        /// Gets or sets the test context which provides information about and functionality for the
        /// current test run.
        /// </summary>
        public TestContext TestContext { get; set; }


        private static UserProfile CreateUserProfile(String loginName, String clearPassword, String firstName,
            String lasName, String email, String language, String country, int aux)
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

        #region FUN 3 Tests

        [TestMethod]
        public void TestFeedUserDetails()
        {
            using (var scope = new TransactionScope())
            {
                UserProfile user = CreateUserProfile(loginName, clearPassword, firstName, lastName, email, language, country, 1 );

                Category category = CreateCategory(categoryName);

                for(int j = 1; j < 5; j++)
                {
                    CreatePost(title, imgTest, description, date, user.usrId, category.categoryId, j);
                }


                int i = 1;

                FeedUserDetails actual = feedUserService.FindFeedUserDetails(user.usrId, 0, 4);

                Assert.IsTrue(actual.UsrId == user.usrId);
                Assert.IsTrue(actual.LoginName == "usuario1");
                Assert.IsTrue(actual.NumFollowers == 2);
                Assert.IsTrue(actual.NumFollows == 2);
                Assert.IsTrue(actual.PostUser.Items.Count == 4);
                Assert.IsTrue(actual.PostUser.ExistMoreItems == false);

                foreach (Post post in actual.PostUser.Items)
                {
                    Assert.IsTrue(post.usrId == user.usrId);
                    Assert.IsTrue(post.title == "img" + actual.UsrId.ToString() + i.ToString());
                    Assert.IsTrue(post.description == "description" + actual.UsrId.ToString() +  i.ToString());
                    Assert.IsTrue(post.likes == 0);
                    Assert.IsTrue(post.categoryId == category.categoryId);
                    Assert.IsTrue(post.img == imgTest + actual.UsrId.ToString() + i.ToString());
                    Assert.IsTrue(post.date == date.AddDays(actual.UsrId).AddMinutes(i));
                    i++;
                }
            }


           

        }

        [TestMethod]
        public void TetsPost_Block()
        {
            using (var scope = new TransactionScope())
            {
                UserProfile user = CreateUserProfile(loginName, clearPassword, firstName, lastName, email, language, country, 1);

                Category category = CreateCategory(categoryName);

                for (int j = 1; j < 5; j++)
                {
                    CreatePost(title, imgTest, description, date, user.usrId, category.categoryId, j);
                }

                Block<Post> actual = feedUserService.FindPostUser(user.usrId, 0, 3);

                Assert.IsTrue(actual.ExistMoreItems == true);
                Assert.IsTrue(actual.Items.Count == 3);

                Block<Post> next = feedUserService.FindPostUser(user.usrId, 3, 6);

                Assert.IsTrue(next.ExistMoreItems == false);
                Assert.IsTrue(next.Items.Count == 1);
            }

                
        }

        [TestMethod]
        public void TestNoPostUser()
        {
            using (var scope = new TransactionScope())
            {
                UserProfile user = CreateUserProfile(loginName, clearPassword, firstName, lastName, email, language, country, 1);

                Block<Post> actual = feedUserService.FindPostUser(user.usrId, 0, 4);

                Assert.IsTrue(actual.ExistMoreItems == false);
                Assert.IsTrue(actual.Items.Count == 0);
            }
            
        }


        [TestMethod]
        [ExpectedException(typeof(InstanceNotFoundException))]
        public void TestFindUserProfileDetailsNotExistingUser()
        {
            using (var scope = new TransactionScope())
            {
                feedUserService.FindFeedUserDetails(NON_EXISTENT_ID, 1, 1);
            }
        }


        [TestMethod]
        public void TestFindUserFollows()
        {
            using (var scope = new TransactionScope())
            {
                List<UserProfile> listUser = new List<UserProfile>();
                for (int i = 1; i < 4; i++)
                {
                    listUser.Add(CreateUserProfile(loginName, clearPassword, firstName, lastName,
                    email, language, country, i));
                }

                followDao.Create(new Follow(listUser.ElementAt(0).usrId, listUser.ElementAt(1).usrId));
                followDao.Create(new Follow(listUser.ElementAt(0).usrId, listUser.ElementAt(2).usrId));


                Block<FollowsUserDetails> actual = feedUserService.FindUserFollows(listUser.ElementAt(0).usrId, 0, 5);

                Assert.IsTrue(actual.ExistMoreItems == false);
                Assert.IsTrue(actual.Items.Count == 2);
                Assert.IsTrue(actual.Items.ElementAt(0).UsrId == listUser.ElementAt(1).usrId);
                Assert.IsTrue(actual.Items.ElementAt(0).LoginNameUser == "usuario2");
                Assert.IsTrue(actual.Items.ElementAt(1).UsrId == listUser.ElementAt(2).usrId);
                Assert.IsTrue(actual.Items.ElementAt(1).LoginNameUser == "usuario3");

            }
                
        }

        [TestMethod]
        public void TestFindUserFollowsNoFollows()
        {
            using (var scope = new TransactionScope())
            {
                UserProfile user = CreateUserProfile(loginName, clearPassword, firstName, lastName, email, language, country, 1);

                Block<FollowsUserDetails> actual = feedUserService.FindUserFollows(user.usrId, 0, 2);
                Assert.IsTrue(actual.Items.Count == 0);
                Assert.IsTrue(actual.ExistMoreItems == false);
            }
            
        }

        [TestMethod]
        public void TestFindUserFollowers()
        {
            using (var scope = new TransactionScope())
            {
            
                List<UserProfile> listUser = new List<UserProfile>();
                for (int i = 1; i < 4; i++)
                {
                    listUser.Add(CreateUserProfile(loginName, clearPassword, firstName, lastName,
                    email, language, country, i));
                }

                followDao.Create(new Follow(listUser.ElementAt(1).usrId, listUser.ElementAt(0).usrId));
                followDao.Create(new Follow(listUser.ElementAt(2).usrId, listUser.ElementAt(0).usrId));


                Block<FollowsUserDetails> actual = feedUserService.FindUserFollowers(listUser.ElementAt(0).usrId, 0, 5);

                Assert.IsTrue(actual.ExistMoreItems == false);
                Assert.IsTrue(actual.Items.Count == 2);
                Assert.IsTrue(actual.Items.ElementAt(0).UsrId == listUser.ElementAt(1).usrId);
                Assert.IsTrue(actual.Items.ElementAt(0).LoginNameUser == "usuario2");
                Assert.IsTrue(actual.Items.ElementAt(1).UsrId == listUser.ElementAt(2).usrId);
                Assert.IsTrue(actual.Items.ElementAt(1).LoginNameUser == "usuario3");
            }
        }   
        /*1*/
        [TestMethod]
        public void TestFindUserFollowersNoFollowers()
        {
            using (var scope = new TransactionScope())
            {
                UserProfile user = CreateUserProfile(loginName, clearPassword, firstName, lastName, email, language, country, 1);

                Block<FollowsUserDetails> actual = feedUserService.FindUserFollowers(user.usrId, 0, 2);
                Assert.IsTrue(actual.Items.Count == 0);
                Assert.IsTrue(actual.ExistMoreItems == false);
            }
        }

        #endregion


        #region FUN 4 Tests

        [TestMethod]
        public void TestFindDefaultFeedUser()
        {

            using (var scope = new TransactionScope())
            {
               
                Category category = CreateCategory(categoryName);
                List<UserProfile> listUser = new List<UserProfile>();
                
                for (int i = 1; i < 4; i++)
                {
                    listUser.Add(CreateUserProfile(loginName, clearPassword, firstName, lastName,
                    email, language, country, i));
                    for (int j = 1; j < 5; j++)
                    {
                        CreatePost(title, imgTest, description, date, listUser.ElementAt(i-1).usrId, category.categoryId, j);
                    }
                }

                followDao.Create(new Follow(listUser.ElementAt(0).usrId, listUser.ElementAt(1).usrId));
                followDao.Create(new Follow(listUser.ElementAt(0).usrId, listUser.ElementAt(2).usrId));

                Block<Post> feedUser = feedUserService.FindDefaultFeedUser(listUser.ElementAt(0).usrId, 0, 9);
                Assert.IsTrue(feedUser.ExistMoreItems == false);
                Assert.IsTrue(feedUser.Items.Count == 8);
                
                for(int i = 4; i > 0; i--)
                {
                    Assert.IsTrue(feedUser.Items.ElementAt(4 - i).title == "img" + listUser.ElementAt(2).usrId.ToString() + i.ToString());
                    Assert.IsTrue(feedUser.Items.ElementAt(4 - i).usrId == listUser.ElementAt(2).usrId);
                }
                for(int i = 4; i < 8; i++)
                {
                    Assert.IsTrue(feedUser.Items.ElementAt(i).title == "img" + listUser.ElementAt(1).usrId.ToString() + (8 - i).ToString());
                    Assert.IsTrue(feedUser.Items.ElementAt(i).usrId == listUser.ElementAt(1).usrId);
                }
                

            }
        }

        #endregion

        #region Additional test attributes

        //Use ClassInitialize to run code before running the first test in the class
        #region New region

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {

            kernel = TestManager.ConfigureNInjectKernel();

            postDao = kernel.Get<IPostDao>();
            categoryDao = kernel.Get<ICategoryDao>();
            followDao = kernel.Get<IFollowDao>();
            userProfileDao = kernel.Get<IUserProfileDao>();
            feedUserService = kernel.Get<IFeedUserService>();

        }

        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup]
        public static void MyClassCleanup()
        {
            TestManager.ClearNInjectKernel(kernel);
        }

        //Use TestInitialize to run code before running each test
        [TestInitialize]
        public void MyTestInitialize()
        {
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void MyTestCleanup()
        {
        }

        #endregion

        #endregion Additional test attributes


    }
}
