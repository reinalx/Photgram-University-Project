using Es.Udc.DotNet.PracticaMaD.Model.DAOs.UserProfileDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.PostDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.FollowDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.CategoryDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.LikePubDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.CommentDao;
using Es.Udc.DotNet.PracticaMaD.Model.Services.UserService;
using Es.Udc.DotNet.PracticaMaD.Model.Services.FeedUserService;
using Es.Udc.DotNet.PracticaMaD.Model.Services.PostService;
using Ninject;
using System.Configuration;
using System.Data.Entity;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.TagDao;
using Es.Udc.DotNet.PracticaMaD.Model.Services.TagService;

namespace Es.Udc.DotNet.PracticaMaD.Test
{
    public class TestManager
    {
        /// <summary>
        /// Configures and populates the Ninject kernel
        /// </summary>
        /// <returns>The NInject kernel</returns>
        public static IKernel ConfigureNInjectKernel()
        {
            NinjectSettings settings = new NinjectSettings() { LoadExtensions = true };

            IKernel kernel = new StandardKernel(settings);

            kernel.Bind<IUserProfileDao>().
                To<UserProfileDaoEntityFramework>();
            kernel.Bind<IPostDao>().
                To<PostDaoEntityFramework>();
            kernel.Bind<IFollowDao>().
                To<FollowDaoEntityFramework>();
            kernel.Bind<ICategoryDao>().
                To<CategoryDaoEntityFramework>();
            kernel.Bind<ICommentDao>().
                To<CommentDaoEntityFramework>();
            kernel.Bind<ILikePubDao>().
                To<LikePubDaoEntityFramework>();
            kernel.Bind<ITagDao>().
                To<TagDaoEntityFramework>();

            kernel.Bind<IFeedUserService>().
                To<FeedUserService>();
            kernel.Bind<IUserService>().
                  To<UserService>();
            kernel.Bind<IPostService>().
                To<PostService>();
            kernel.Bind<ITagService>().
                To<TagService>();

            string connectionString =
                ConfigurationManager.ConnectionStrings["practicamadEntities"].ConnectionString;

            kernel.Bind<DbContext>().
                ToSelf().
                InSingletonScope().
                WithConstructorArgument("nameOrConnectionString", connectionString);

            return kernel;
        }

        /// <summary>
        /// Configures the Ninject kernel from an external module file.
        /// </summary>
        /// <param name="moduleFilename">The module filename.</param>
        /// <returns>The NInject kernel</returns>
        public static IKernel ConfigureNInjectKernel(string moduleFilename)
        {
            NinjectSettings settings = new NinjectSettings() { LoadExtensions = true };
            IKernel kernel = new StandardKernel(settings);

            kernel.Load(moduleFilename);

            return kernel;
        }

        public static void ClearNInjectKernel(IKernel kernel)
        {
            kernel.Dispose();
        }
    }
}