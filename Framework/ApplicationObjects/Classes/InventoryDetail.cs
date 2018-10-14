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
    class InventoryDetail
    {
        #region "VARIABLES"
        InventoryDetailDAO loInventoryDetailDAO;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public InventoryDetail()
        {
            loInventoryDetailDAO = new InventoryDetailDAO();
        }
        #endregion "END OF CONSTTRUCTORS"

        #region "PROPERTIES"
        public string DetailId
        {
            get;
            set;
        }
        public string HeaderId
        {
            get;
            set;
        }
        public string StockId
        {
            get;
            set;
        }
        public string StockDescription
        {
            get;
            set;
        }
        public string Unit
        {
            get;
            set;
        }
        public string LocationId
        {
            get;
            set;
        }
        public decimal IN
        {
            get;
            set;
        }
        public decimal OUT
        {
            get;
            set;
        }
        public decimal Balance
        {
            get;
            set;
        }
        public decimal UnitPrice
        {
            get;
            set;
        }
        public decimal TotalPrice
        {
            get;
            set;
        }
        public string Remarks
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        #region "METHODS"
        public DataTable getAllData(string pInventoryId)
        {
            return loInventoryDetailDAO.getInventoryDetails(pInventoryId);
        }

        public DataTable getInventoryDetail(string pInventoryId)
        {
            return loInventoryDetailDAO.getInventoryDetail(pInventoryId);
        }

        public DataTable getStockInventory(string pLocationId)
        {
            return loInventoryDetailDAO.getStockInventory(pLocationId);
        }

        public DataTable getStockInventoryList(string pLocationId, string pSearchString)
        {
            return loInventoryDetailDAO.getStockInventoryList(pLocationId, pSearchString);
        }

        public bool save(GlobalVariables.Operation pOperation, ref MySqlTransaction pTrans)
        {
            bool _status = false;
            switch (pOperation)
            {
                case GlobalVariables.Operation.Add:
                    _status = loInventoryDetailDAO.insertInventoryDetail(this, ref pTrans);
                    break;
                case GlobalVariables.Operation.Edit:
                    _status = loInventoryDetailDAO.updateInventoryDetail(this, ref pTrans);
                    break;
                default:
                    _status = false;
                    break;
            }
            return _status;
        }

        public bool remove(string pDetailId, ref MySqlTransaction pTrans)
        {
            bool _Status = false;
            try
            {
                _Status = loInventoryDetailDAO.removeInventoryDetail(pDetailId, ref pTrans);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _Status;
        }

        public bool updateQtyOnHand(string pDetailId, ref MySqlTransaction pTrans)
        {
            bool _Status = false;
            try
            {
                _Status = loInventoryDetailDAO.updateQtyOnHand(pDetailId, ref pTrans);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _Status;
        }
        #endregion "END OF METHODS"
    }
}
