using Es.Udc.DotNet.ModelUtil.Dao;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Es.Udc.DotNet.ModelUtil.Exceptions;

namespace Es.Udc.DotNet.PracticaMaD.Model.DAOs.CategoryDao
{
    public class CategoryDaoEntityFramework : GenericDaoEntityFramework<Category, Int64>, ICategoryDao
    {

        #region Public Constructors

        /// <summary>
        /// Public Constructor
        /// </summary>
        public CategoryDaoEntityFramework()
        {
        }

        #endregion Public Constructors

        #region ICategoryDao Members. Specific Operations

        Category ICategoryDao.FindByCategoryName(string categoryName)   
        {
            Category cat = null;

            DbSet<Category> category = Context.Set<Category>();

            var result =
                (from c in category
                 where c.categoryName == categoryName
                 select c);

            cat = result.FirstOrDefault();

            if (cat == null)
                throw new InstanceNotFoundException(cat, "No se encontro ninguna instancia de categorias para ese nombre");

            return cat;
        
        }

        #endregion ICategoryDao Members. Specific Operations

    }
}
