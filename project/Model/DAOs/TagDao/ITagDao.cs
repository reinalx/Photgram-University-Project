using System;
using System.Collections.Generic;
using Es.Udc.DotNet.ModelUtil.Dao;

namespace Es.Udc.DotNet.PracticaMaD.Model.DAOs.TagDao
{
    public interface ITagDao : IGenericDao<Tag, Int64>
    {
        /// <summary>
        /// Finds a tag by tag name.
        /// </summary>
        /// <param name="tagName"> The name tag. </param>
        /// <returns>
        /// A tag if found, null otherwise
        /// </returns>
        Tag FindByTagName(String tagName);

        /// <summary>
        /// Checks for a tag by tag name.
        /// </summary>
        /// <param name="tagName"> The name tag. </param>
        /// <returns>
        /// True if it exists, false otherwise
        /// </returns>
        Boolean existsByTagName(String tagName);
    }
}