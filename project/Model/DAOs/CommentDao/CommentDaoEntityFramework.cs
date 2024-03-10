using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMaD.Model.DAOs.CommentDao
{
    public class CommentDaoEntityFramework : GenericDaoEntityFramework<Comment, Int64>, ICommentDao
    {

        #region Public Constructors

        /// <summary>
        /// Public Constructor
        /// </summary>
        public CommentDaoEntityFramework()
        {
        }

        #endregion Public Constructors

        #region ICommentDao Members. Specific Operations

        public bool ExistsByUserIdAndPostPostId(long userId, long postId)
        {

            DbSet<Comment> comment = Context.Set<Comment>();

            var result =
                (from c in comment
                 where c.usrId == userId && c.postId == postId
                 select c 
                );
            if (result == null)
            {
                return false;

            }
            else
            {
                return true;
            }
            
        }

        public Comment FindByUserAndPost(long userId, long postId)
        {
            Comment commentResult = null;

            DbSet<Comment> comment = Context.Set<Comment>();

            var result =
                (from c in comment
                 where c.usrId == userId && c.postId == postId
                 select c
                );

            commentResult = result.FirstOrDefault();

            if (commentResult == null)
            {
                throw new InstanceNotFoundException(commentResult, "No se encontró ninguna instancia para el usuario y la publicación especificados.");

            }
            return commentResult;

        }

        public Comment FindById(long commentId)
        {
            Comment commentResult = null;

            DbSet<Comment> comment = Context.Set<Comment>();

            var result =
                (from c in comment
                 where c.commentId == commentId
                 select c
                );

            commentResult = result.FirstOrDefault();

            if (commentResult == null)
            {
                throw new InstanceNotFoundException(commentResult, "No se encontró ninguna instancia para el id del comentario.");

            }
            return commentResult;
        }

        public List<Comment> FindByPublicationId(long postId, int startIndex, int count)
        {

           
            DbSet<Comment> comment = Context.Set<Comment>();

            List<Comment> result =
                (from c in comment
                 where c.postId == postId
                 select c).OrderByDescending(c => c.date).Skip(startIndex).Take(count).ToList();

            return result;
        }

        public List<Comment> FindAllByPublicationId(long postId)
        {


            DbSet<Comment> comment = Context.Set<Comment>();

            List<Comment> result =
                (from c in comment
                 where c.postId == postId
                 select c).ToList();

            return result;
        }

        #endregion ICommentDao Members. Specific Operations

    }
}
