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
    class InventoryHeader
    {
        #region "VARIABLES"
        InventoryHeaderDAO lInventoryHeaderDAO;
        #endregion

        #region "CONSTRUCTORS"
        public InventoryHeader()
        {
            lInventoryHeaderDAO = new InventoryHeaderDAO();
        }
        #endregion

        #region "PROPERTIES"
        public string HeaderId
        {
            get;
            set;
        }
        public DateTime Date
        {
            get;
            set;
        }
        public string Type
        {
            get;
            set;
        }
        public string Final
        {
            get;
            set;
        }
        public string Reference
        {
            get;
            set;
        }
        public string CustomerId
        {
            get;
            set;
        }
        public string SupplierId
        {
            get;
            set;
        }
        public decimal TotalIN
        {
            get;
            set;
        }
        public decimal TotalOUT
        {
            get;
            set;
        }
        public decimal TotalAmount
        {
            get;
            set;
        }
        public string Username
        {
            get;
            set;
        }
        public DateTime FinalDate
        {
            get;
            set;
        }
        public string FinalizedBy
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
        public DataTable getAllData(string pDisplayType,string pPrimaryKey, string pSearchString, string pType)
        {
            return lInventoryHeaderDAO.getInventoryHeaders(pDisplayType, pPrimaryKey, pSearchString, pType);
        }

        public DataTable getInventoryHeader(string pId)
        {
            return lInventoryHeaderDAO.getInventoryHeader(pId);
        }

        public DataTable getNextInventoryHeaderNo()
        {
            return lInventoryHeaderDAO.getNextInventoryHeaderNo();
        }

        public DataTable getStockReceivingForCheckVoucher(string pSupplierId)
        {
            return lInventoryHeaderDAO.getStockReceivingForCheckVoucher(pSupplierId);
        }

        public DataTable getStockReceivingDetailPaid(string pCheckVoucherHeaderId)
        {
            return lInventoryHeaderDAO.getStockReceivingDetailPaid(pCheckVoucherHeaderId);
        }

        public DataTable getYearInInventoryHeader()
        {
            return lInventoryHeaderDAO.getYearInInventoryHeader();
        }

        public DataTable getInventoryHeaderMonthlySummary(string pYear, string pType)
        {
            return lInventoryHeaderDAO.getInventoryHeaderMonthlySummary(pYear, pType);
        }

        public DataTable getAccountPayableReport()
        {
            return lInventoryHeaderDAO.getAccountPayableReport();
        }

        public DataTable getAccountPayableOverDue()
        {
            return lInventoryHeaderDAO.getAccountPayableOverDue();
        }

        public string save(GlobalVariables.Operation pOperation, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            switch (pOperation)
            {
                case GlobalVariables.Operation.Add:
                    _Id = lInventoryHeaderDAO.insertInventoryHeader(this, ref pTrans);
                    break;
                case GlobalVariables.Operation.Edit:
                    _Id = lInventoryHeaderDAO.updateInventoryHeader(this, ref pTrans);
                    break;
            }
            return _Id;
        }

        public bool remove(string pHeaderId,ref MySqlTransaction pTrans)
        {
            return lInventoryHeaderDAO.removeInventoryHeader(pHeaderId, ref pTrans);
        }

        public bool finalize(string pHeaderId, ref MySqlTransaction pTrans)
        {
            return lInventoryHeaderDAO.finalizeInventoryHeader(pHeaderId, ref pTrans);
        }

        public bool addToAccountPayable(string pDate,string pStockReceivingId, string pReference, string pSupplierId, decimal pTotalAmount, string pDueDate, ref MySqlTransaction pTrans)
        {
            return lInventoryHeaderDAO.addToAccountPayable(pDate, pStockReceivingId, pReference, pSupplierId, pTotalAmount, pDueDate,ref pTrans);
        }
        #endregion
    }
}
