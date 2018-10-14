using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Reflection;
using MySql.Data.MySqlClient;

using NSites.Global;
using NSites.ApplicationObjects.DataAccessObjects;

namespace NSites.ApplicationObjects.Classes
{
    class Category
    {
        #region "VARIABLES"
        CategoryDAO lCategoryDAO;
        #endregion

        #region "CONSTRUCTORS"
        public Category()
        {
            lCategoryDAO = new CategoryDAO();
        }
        #endregion

        #region "PROPERTIES"
        public string Id
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public string InventoryGroupId
        {
            get;
            set;
        }
        public string Remarks
        {
            get;
            set;
        }
        #endregion

        #region "METHODS"
        public DataTable getAllData(string pDisplayType, string pSearchString)
        {
            return lCategoryDAO.getCategorys(pDisplayType, pSearchString);
        }

        public DataTable getCategory(string pId)
        {
            return lCategoryDAO.getCategory(pId);
        }

        public string save(GlobalVariables.Operation pOperation, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            switch (pOperation)
            {
                case GlobalVariables.Operation.Add:
                    _Id = lCategoryDAO.insertCategory(this, ref pTrans);
                    break;
                case GlobalVariables.Operation.Edit:
                    _Id = lCategoryDAO.updateCategory(this, ref pTrans);
                    break;
            }
            return _Id;
        }

        public bool remove(string pId,ref MySqlTransaction pTrans)
        {
            return lCategoryDAO.removeCategory(pId, ref pTrans);
        }
        #endregion
    }
}
