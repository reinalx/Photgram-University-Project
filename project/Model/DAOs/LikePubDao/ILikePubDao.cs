using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMaD.Model.DAOs.LikePubDao
{
    public interface ILikePubDao : IGenericDao<LikePub, Int64>
    {


        /// <summary>Finds LikePub based on a specific user and post</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="postId">The post identifier.</param>
        /// <returns>
        ///   A likePub entity if it is found, null otherwise
        /// </returns>
        /// <exception cref="InstanceNotFoundException"/>
        LikePub FindByUserAndPost(long userId, long postId);

        /// <summary>Checks if there is a LikePub from an specific user to an specific post</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="postId">The post identifier.</param>
        /// <returns>
        ///   True if there is, false otherwise
        /// </returns>
        bool ExistsByUserIdAndPostPostId(long userId, long postId);

        /// <summary>Finds all instances of LikePub regarding a specific post</summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns>
        ///   A list containing all instances of LikePub of an specific post
        /// </returns>
        List<LikePub> FindAllByPost(long postId);
	}
}
