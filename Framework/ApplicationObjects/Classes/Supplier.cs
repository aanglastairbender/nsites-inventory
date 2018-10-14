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
    class Supplier
    {
        #region "VARIABLES"
        SupplierDAO lSupplierDAO;
        #endregion

        #region "CONSTRUCTORS"
        public Supplier()
        {
            lSupplierDAO = new SupplierDAO();
        }
        #endregion

        #region "PROPERTIES"
        public string Id
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Address
        {
            get;
            set;
        }
        public string ContactPerson
        {
            get;
            set;
        }
        public string ContactNo
        {
            get;
            set;
        }
        public int Terms
        {
            get;
            set;
        }
        public decimal CreditLimit
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
            return lSupplierDAO.getSuppliers(pDisplayType, pSearchString);
        }

        public DataTable getSupplier(string pId)
        {
            return lSupplierDAO.getSupplier(pId);
        }

        public string save(GlobalVariables.Operation pOperation, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            switch (pOperation)
            {
                case GlobalVariables.Operation.Add:
                    _Id = lSupplierDAO.insertSupplier(this, ref pTrans);
                    break;
                case GlobalVariables.Operation.Edit:
                    _Id = lSupplierDAO.updateSupplier(this, ref pTrans);
                    break;
            }
            return _Id;
        }

        public bool remove(string pId,ref MySqlTransaction pTrans)
        {
            return lSupplierDAO.removeSupplier(pId, ref pTrans);
        }
        #endregion
    }
}
