using Es.Udc.DotNet.PracticaMaD.Model.DAOs.UserProfileDao;
using Es.Udc.DotNet.PracticaMaD.Model.Services.UserService;
using Es.Udc.DotNet.ModelUtil.IoC;
using Ninject;
using System.Configuration;
using System.Data.Entity;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.CategoryDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.CommentDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.FollowDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.LikePubDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.PostDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.TagDao;
using Es.Udc.DotNet.PracticaMaD.Model.Services.FeedUserService;
using Es.Udc.DotNet.PracticaMaD.Model.Services.PostService;
using Es.Udc.DotNet.PracticaMaD.Model.Services.TagService;

namespace Es.Udc.DotNet.PracticaMaD.HTTP.Util.IoC
{
    internal class IoCManagerNinject : IIoCManager
    {
        private static IKernel kernel;
        private static NinjectSettings settings;

        public void Configure()
        {
            settings = new NinjectSettings() { LoadExtensions = true };
            kernel = new StandardKernel(settings);

            /* UserProfileDao */
            kernel.Bind<IUserProfileDao>().
                To<UserProfileDaoEntityFramework>();
            
            /* CategoryDao */
            kernel.Bind<ICategoryDao>().To<CategoryDaoEntityFramework>();

            /* CommentDao */
            kernel.Bind<ICommentDao>().To<CommentDaoEntityFramework>();

            /* FollowDao */
            kernel.Bind<IFollowDao>().To<FollowDaoEntityFramework>();

            /* LikePubDao */
            kernel.Bind<ILikePubDao>().To<LikePubDaoEntityFramework>();

            /* PostDao */
            kernel.Bind<IPostDao>().To<PostDaoEntityFramework>();

            /* TagDao */
            kernel.Bind<ITagDao>().To<TagDaoEntityFramework>();

            /* UserService */
            kernel.Bind<IUserService>().
                To<UserService>();

            /* FeedUserService */
            kernel.Bind<IFeedUserService>().To<FeedUserService>();

            /* PostService */
            kernel.Bind<IPostService>().To<PostService>();

            /* TagService */
            kernel.Bind<ITagService>().To<TagService>();

            /* DbContext */
            string connectionString =
                ConfigurationManager.ConnectionStrings["practicamadEntities"].ConnectionString;

            kernel.Bind<DbContext>().
                ToSelf().
                InSingletonScope().
                WithConstructorArgument("nameOrConnectionString", connectionString);
        }

        public T Resolve<T>()
        {
            return kernel.Get<T>();
        }
    }
}