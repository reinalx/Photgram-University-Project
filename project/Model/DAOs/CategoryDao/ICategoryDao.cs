using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMaD.Model.DAOs.CategoryDao
{
    public interface ICategoryDao : IGenericDao<Category, Int64>
    {
        /// <summary>
        /// Finds a Category by categoryName
        /// </summary>
        /// <param name="categoryName">The category name</param>
        /// <returns>The Category</returns>
        /// <exception cref="InstanceNotFoundException"/>
        Category FindByCategoryName(string categoryName);

    }
}
