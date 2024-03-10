using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMaD.Model.DAOs.PostDao
{

    /// <summary>
    /// Specific Operations for UserProfile
    /// </summary>
    public class PostDaoEntityFramework :
        GenericDaoEntityFramework<Post, Int64>, IPostDao
    {
        public PostDaoEntityFramework()
        {

        }

        public List<Post> FindAllOrderByPostDateAsc(int startIndex, int count)
        {
            DbSet<Post> post = Context.Set<Post>();

            List<Post> result = (from p in post
                                 select p)
                                    .OrderBy(p => p.date)
                                    .Skip(startIndex)
                                    .Take(count)
                                    .ToList();

            return result;
        }

        public List<Post> FindPostByUserProfileUsrId(long userId, int startIndex, int count)
        {
            DbSet<Post> post = Context.Set<Post>();

            List<Post> result = (from p in post
                                 where p.usrId == userId
                                 select p)
                                    .OrderBy(p => p.date)
                                    .Skip(startIndex)
                                    .Take(count)
                                    .ToList();

            return result;
        }

        public List<Post> FindPostByKeyword(string keyword, int startIndex, int count)
        {
            DbSet<Post> post = Context.Set<Post>();

            List<Post> result = (from p in post
                                 where p.title.Contains(keyword) || p.description.Contains(keyword)
                                 select p)
                                    .OrderBy(p => p.date)
                                    .Skip(startIndex)
                                    .Take(count)
                                    .ToList();

            return result;
        }
        public List<Post> FindPostByKeyword(string keyword, long categoryId, int startIndex, int count)
        {
            DbSet<Post> post = Context.Set<Post>();

            List<Post> result = (from p in post
                                 where (p.title.Contains(keyword) || p.description.Contains(keyword)) && p.categoryId == categoryId
                                 select p)
                                    .OrderBy(p => p.date)
                                    .Skip(startIndex)
                                    .Take(count)
                                    .ToList();

            return result;
        }

        public List<Post> FindPostFollowsByUserId(long usrId, int startIndex, int count)
        {
            DbSet<Post> post = Context.Set<Post>();
            DbSet<Follow> follow = Context.Set<Follow>();

            List<Post> result = (from f in follow
                    join p in post on f.usrId2 equals p.usrId
                    where f.usrId1 == usrId
                    select p)
                .OrderByDescending(p => p.date)
                .Skip(startIndex)
                .Take(count)
                .ToList();

            return result;
        }

        public List<Post> FindPostByTag(long tagId, int startIndex, int count)
        {
            DbSet<Post> post = Context.Set<Post>();
            DbSet<Tag> tag = Context.Set<Tag>();

            List<Post> result = (
                    from p in post
                    where p.Tag.Any(t => t.tagId == tagId)
                    select p
                ).OrderByDescending(p => p.date)
                .Skip(startIndex)
                .Take(count)
                .ToList();

            return result;
        }
    }

}
