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
    class Unit
    {
        #region "VARIABLES"
        UnitDAO lUnitDAO;
        #endregion

        #region "CONSTRUCTORS"
        public Unit()
        {
            lUnitDAO = new UnitDAO();
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
            return lUnitDAO.getUnits(pDisplayType, pSearchString);
        }

        public string save(GlobalVariables.Operation pOperation, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            switch (pOperation)
            {
                case GlobalVariables.Operation.Add:
                    _Id = lUnitDAO.insertUnit(this, ref pTrans);
                    break;
                case GlobalVariables.Operation.Edit:
                    _Id = lUnitDAO.updateUnit(this, ref pTrans);
                    break;
            }
            return _Id;
        }

        public bool remove(string pId,ref MySqlTransaction pTrans)
        {
            return lUnitDAO.removeUnit(pId, ref pTrans);
        }
        #endregion
    }
}
