using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMaD.Model.DAOs.FollowDao
{
    public class FollowDaoEntityFramework : GenericDaoEntityFramework<Follow, Int64>, IFollowDao
    {
        #region Public Constructors

        /// <summary>
        /// Public Constructor
        /// </summary>
        public FollowDaoEntityFramework()
        {

        }
        #endregion Public Constructors


        #region FollowDao Members. Specific Operations

        public long ExistsByUserId1AndUserId2(long userId1, long userId2)
        {
            Follow  exist = null;

            DbSet<Follow> follow = Context.Set<Follow>();

            var result =
                (from f in follow
                 where f.usrId1 == userId1 && f.usrId2 == userId2
                 select f
                );

            exist = result.FirstOrDefault();
            if (exist == null)
            {
                return -1;

            }
            else
            {
                return exist.followId;
            }
        }

        public List<Follow> FindByUserId1ID(long userId1, int startIndex, int count)
        {
            DbSet<Follow> follow = Context.Set<Follow>();

            List<Follow> result = (from f in follow
                                   where f.usrId1 == userId1
                                   select f)
                         .OrderBy(f => f.followId)
                         .Skip(startIndex)
                         .Take(count)
                         .ToList();


            return result;
        }

        public List<Follow> FindByUserId2ID(long userId2, int startIndex, int count)
        {
            DbSet<Follow> follow = Context.Set<Follow>();

            List<Follow> result = (from f in follow
                                   where f.usrId2 == userId2
                                   select f)
                         .OrderBy(f => f.followId)
                         .Skip(startIndex)
                         .Take(count)
                         .ToList();


            return result;
        }

    }
    #endregion FollowDao Members. Specific Operations


}
