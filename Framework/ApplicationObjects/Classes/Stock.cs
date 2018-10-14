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
    class Stock
    {
        #region "VARIABLES"
        StockDAO lStockDAO;
        #endregion

        #region "CONSTRUCTORS"
        public Stock()
        {
            lStockDAO = new StockDAO();
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
        public string CategoryId
        {
            get;
            set;
        }
        public string UnitId
        {
            get;
            set;
        }
        public decimal UnitCost
        {
            get;
            set;
        }
        public decimal UnitPrice
        {
            get;
            set;
        }
        public decimal ReorderLevel
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
            return lStockDAO.getStocks(pDisplayType, pSearchString);
        }

        public DataTable getStock(string pId)
        {
            return lStockDAO.getStock(pId);
        }

        public DataTable getStockReservation(string pId)
        {
            return lStockDAO.getStockReservation(pId);
        }

        public DataTable getStockReorderLevel()
        {
            return lStockDAO.getStockReorderLevel();
        }

        public DataTable getStockCard(DateTime pFromDate, DateTime pToDate, string pStockId)
        {
            return lStockDAO.getStockCard(pFromDate, pToDate, pStockId);
        }

        public DataTable getStockCardBegBal(DateTime pFromDate, string pStockId)
        {
            return lStockDAO.getStockCardBegBal(pFromDate, pStockId);
        }

        public DataTable getStockQtyOnHand(string pLocationId, string pStockId)
        {
            return lStockDAO.getStockQtyOnHand(pLocationId, pStockId);
        }

        public string save(GlobalVariables.Operation pOperation, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            switch (pOperation)
            {
                case GlobalVariables.Operation.Add:
                    _Id = lStockDAO.insertStock(this, ref pTrans);
                    break;
                case GlobalVariables.Operation.Edit:
                    _Id = lStockDAO.updateStock(this, ref pTrans);
                    break;
            }
            return _Id;
        }

        public bool remove(string pId,ref MySqlTransaction pTrans)
        {
            return lStockDAO.removeStock(pId, ref pTrans);
        }
        #endregion
    }
}
