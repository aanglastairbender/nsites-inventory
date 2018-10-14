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
    class AuditTrail
    {
        #region "VARIABLES"
        AuditTrailDAO lAuditTrailDAO;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public AuditTrail()
        {
            lAuditTrailDAO = new AuditTrailDAO();
        }
        #endregion "END OF CONSTTRUCTORS"

        #region "PROPERTIES"
        public string LogDescription
        {
            get;
            set;
        }
        public string Username
        {
            get;
            set;
        }
        public DateTime Date
        {
            get;
            set;
        }
        public string Hostname
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        #region "METHODS"
        public DataTable getAuditTrailByDate(DateTime pFrom,DateTime pTo)
        {
            return lAuditTrailDAO.getAuditTrailByDate(pFrom,pTo);
        }
        public bool saveAuditTrail(GlobalVariables.Operation pOperation)
        {
            bool _status = false;
            switch (pOperation)
            {
                case GlobalVariables.Operation.Add:
                    _status = lAuditTrailDAO.insertAuditTrail(this);
                    break;
                default:
                    break;
            }
            return _status;
        }

        public bool removeAuditTrail(DateTime pFrom, DateTime pTo)
        {
            return lAuditTrailDAO.removeAuditTrail(pFrom,pTo);
        }
        #endregion "END OF METHODS"
    }
}
