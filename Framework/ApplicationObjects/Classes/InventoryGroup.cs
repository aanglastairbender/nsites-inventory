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
    class InventoryGroup
    {
        #region "VARIABLES"
        InventoryGroupDAO lInventoryGroupDAO;
        #endregion

        #region "CONSTRUCTORS"
        public InventoryGroup()
        {
            lInventoryGroupDAO = new InventoryGroupDAO();
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
        public string Remarks
        {
            get;
            set;
        }
        #endregion

        #region "METHODS"
        public DataTable getAllData(string pDisplayType, string pSearchString)
        {
            return lInventoryGroupDAO.getInventoryGroups(pDisplayType, pSearchString);
        }

        public string save(GlobalVariables.Operation pOperation, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            switch (pOperation)
            {
                case GlobalVariables.Operation.Add:
                    _Id = lInventoryGroupDAO.insertInventoryGroup(this, ref pTrans);
                    break;
                case GlobalVariables.Operation.Edit:
                    _Id = lInventoryGroupDAO.updateInventoryGroup(this, ref pTrans);
                    break;
            }
            return _Id;
        }

        public bool remove(string pId,ref MySqlTransaction pTrans)
        {
            return lInventoryGroupDAO.removeInventoryGroup(pId, ref pTrans);
        }
        #endregion
    }
}
