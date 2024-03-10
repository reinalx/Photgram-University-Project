using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMaD.Model.DAOs.CommentDao
{
    public interface ICommentDao : IGenericDao<Comment, Int64>
	{

        /// <summary>Finds a comment from a specific user in a specific post</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="postId">The post identifier.</param>
        /// <returns>Either the comment, or null</returns>
        /// <exception cref="InstanceNotFoundException"/>
        Comment FindByUserAndPost(long userId, long postId);


        /// <summary>Verifies if there exists a comment from a specific user in a specific post</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="postId">The post identifier.</param>
        /// <returns>True if there exists, false otherwise</returns>
        bool ExistsByUserIdAndPostPostId(long userId, long postId);


        /// <summary>Finds the by publication identifier.</summary>
        /// <param name="postId">The post identifier.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        /// <returns>The collection of 'count' instances of comments, starting from startIndex, in a post</returns>
        List<Comment> FindByPublicationId(long postId, int startIndex, int count);

        /// <summary>Finds all by publication identifier.</summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns>The collection of all instances of comments in a post</returns>
        List<Comment> FindAllByPublicationId(long postId);


        /// <summary>Finds the by identifier.</summary>
        /// <param name="commentId">The comment identifier.</param>
        /// <returns>The associated comment</returns>
        /// <exception cref="InstanceNotFoundException"/>
        Comment FindById(long commentId); 

	}
}

