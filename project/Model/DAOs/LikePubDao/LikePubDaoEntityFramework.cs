using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMaD.Model.DAOs.LikePubDao
{
    public class LikePubDaoEntityFramework : GenericDaoEntityFramework<LikePub, Int64>, ILikePubDao
    {
        public LikePubDaoEntityFramework()
        {

        }

        public bool ExistsByUserIdAndPostPostId(long userId, long postId)
        {

            DbSet<LikePub> likePub = Context.Set<LikePub>();

            List<LikePub> result =
                (from l in likePub
                 where l.usrId == userId && l.postId == postId
                 select l
                ).ToList();

            if (result.Count == 0)
            {
                return false;

            }
            else
            {
                return true;
            }
        }

        public List<LikePub> FindAllByPost(long postId)
        {
            DbSet<LikePub> likePub = Context.Set<LikePub>();

            List<LikePub> result =
                (from l in likePub
                 where l.postId == postId
                 select l).ToList();

            return result;
        }

        public LikePub FindByUserAndPost(long userId, long postId)
        {
            LikePub likePubResult = null;

            DbSet<LikePub> likePub = Context.Set<LikePub>();

            var result =
                (from l in likePub
                 where l.usrId == userId && l.postId == postId
                 select l
                );

            likePubResult = result.FirstOrDefault();

            if (likePubResult == null)
            {
                throw new InstanceNotFoundException(likePubResult, "No se encontró ninguna instancia para el usuario y la publicación especificados.");

            }
            return likePubResult;
        }
    }
}
