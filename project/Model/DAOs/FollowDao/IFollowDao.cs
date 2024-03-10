using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMaD.Model.DAOs.FollowDao
{
    public interface IFollowDao : IGenericDao<Follow, Int64>
    {
        /// <summary>Checks if there exists a follow relation between user1 and user2</summary>
        /// <param name="userId1">The user1 id.</param>
        /// <param name="userId2">The user2 id.</param>
        /// <returns>The id of the relation if it exists, -1 if it doesn't</returns>
        long ExistsByUserId1AndUserId2(long userId1, long userId2);

        /// <summary>Finds the followers of the user</summary>
        /// <param name="userId1">The user id.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        /// <returns>A list with 'count' instances of follow, starting from startIndex</returns>
        List<Follow> FindByUserId1ID(long userId1, int startIndex, int count);

        /// <summary>Finds the followings of the user</summary>
        /// <param name="userId2">The user id.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        /// <returns>A list with 'count' instances of follow, starting from startIndex</returns>
        List<Follow> FindByUserId2ID(long userId2, int startIndex, int count);


    }
}
