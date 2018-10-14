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
    class Location
    {
        #region "VARIABLES"
        LocationDAO lLocationDAO;
        #endregion

        #region "CONSTRUCTORS"
        public Location()
        {
            lLocationDAO = new LocationDAO();
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
            return lLocationDAO.getLocations(pDisplayType, pSearchString);
        }

        public DataTable getLocation(string pId)
        {
            return lLocationDAO.getLocation(pId);
        }

        public string save(GlobalVariables.Operation pOperation, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            switch (pOperation)
            {
                case GlobalVariables.Operation.Add:
                    _Id = lLocationDAO.insertLocation(this, ref pTrans);
                    break;
                case GlobalVariables.Operation.Edit:
                    _Id = lLocationDAO.updateLocation(this, ref pTrans);
                    break;
            }
            return _Id;
        }

        public bool remove(string pId,ref MySqlTransaction pTrans)
        {
            return lLocationDAO.removeLocation(pId, ref pTrans);
        }
        #endregion
    }
}
