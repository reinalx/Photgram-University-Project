using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMaD.Model.DAOs.PostDao
{
    public interface IPostDao : IGenericDao<Post, Int64>
    {
        /// <summary>Finds 'count' posts starting from startIndex, ordered by the date of the post in an ascending order</summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        /// <returns>A list containing posts</returns>
        List<Post> FindAllOrderByPostDateAsc(int startIndex, int count);

        /// <summary>Finds 'count' posts, starting from startIndex, from the specified user, from newest to oldest.</summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        /// <returns>A list of posts</returns>
        List<Post> FindPostByUserProfileUsrId(long userId, int startIndex, int count);

        /// <summary>Finds 'count' posts, starting from startIndex, which have the specified keyword/s in the title or description, from newest to oldest.</summary>
        /// <param name="keyword">The keyword/s to find.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        /// <returns>A list of posts</returns>
        List<Post> FindPostByKeyword(string keyword, int startIndex, int count);
        List<Post> FindPostByKeyword(string keyword, long categoryId, int startIndex, int count);

        /// <summary>Finds 'count' posts, starting from startIndex, from the users followed by the specified user, from newest to oldest.</summary>
        /// <param name="usrId">The tag identifier.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        /// <returns>A list of posts</returns>
        List<Post> FindPostFollowsByUserId(long usrId, int startIndex, int count);

        /// <summary>Finds 'count' posts, starting from startIndex, which have the specified tag, from newest to oldest.</summary>
        /// <param name="tagId">The tag identifier.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        /// <returns>A list of posts</returns>
        List<Post> FindPostByTag(long tagId, int startIndex, int count);
    }
}
