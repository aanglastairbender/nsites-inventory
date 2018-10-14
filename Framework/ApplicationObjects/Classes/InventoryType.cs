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
    class InventoryType
    {
        #region "VARIABLES"
        InventoryTypeDAO lInventoryTypeDAO;
        #endregion

        #region "CONSTRUCTORS"
        public InventoryType()
        {
            lInventoryTypeDAO = new InventoryTypeDAO();
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
        public string Qty
        {
            get;
            set;
        }
        public string Source
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
            return lInventoryTypeDAO.getInventoryTypes(pDisplayType, pSearchString);
        }

        public DataTable getInventoryType(string pId)
        {
            return lInventoryTypeDAO.getInventoryType(pId);
        }

        public string save(GlobalVariables.Operation pOperation, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            switch (pOperation)
            {
                case GlobalVariables.Operation.Add:
                    _Id = lInventoryTypeDAO.insertInventoryType(this, ref pTrans);
                    break;
                case GlobalVariables.Operation.Edit:
                    _Id = lInventoryTypeDAO.updateInventoryType(this, ref pTrans);
                    break;
            }
            return _Id;
        }

        public bool remove(string pId,ref MySqlTransaction pTrans)
        {
            return lInventoryTypeDAO.removeInventoryType(pId, ref pTrans);
        }
        #endregion
    }
}
