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
    class Customer
    {
        #region "VARIABLES"
        CustomerDAO lCustomerDAO;
        #endregion

        #region "CONSTRUCTORS"
        public Customer()
        {
            lCustomerDAO = new CustomerDAO();
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
            return lCustomerDAO.getCustomers(pDisplayType, pSearchString);
        }

        public DataTable getCustomerTransactions(DateTime pFromDate, DateTime pToDate,string pCustomerId)
        {
            return lCustomerDAO.getCustomerTransactions(pFromDate,pToDate,pCustomerId);
        }

        public DataTable getCustomersNotForBilling()
        {
            return lCustomerDAO.getCustomersNotForBilling();
        }

        public DataTable getCustomersForBilling()
        {
            return lCustomerDAO.getCustomersForBilling();
        }

        public DataTable getCustomer(string pId)
        {
            return lCustomerDAO.getCustomer(pId);
        }

        public string save(GlobalVariables.Operation pOperation, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            switch (pOperation)
            {
                case GlobalVariables.Operation.Add:
                    _Id = lCustomerDAO.insertCustomer(this, ref pTrans);
                    break;
                case GlobalVariables.Operation.Edit:
                    _Id = lCustomerDAO.updateCustomer(this, ref pTrans);
                    break;
            }
            return _Id;
        }

        public bool remove(string pId,ref MySqlTransaction pTrans)
        {
            return lCustomerDAO.removeCustomer(pId, ref pTrans);
        }
        #endregion
    }
}
