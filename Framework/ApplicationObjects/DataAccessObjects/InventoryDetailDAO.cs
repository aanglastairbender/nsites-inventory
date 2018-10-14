using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Reflection;

using NSites.Global;

namespace NSites.ApplicationObjects.DataAccessObjects
{
    class InventoryDetailDAO
    {
        #region "VARIABLES"
        string lDetailId;
        string lHeaderId;
        string lStockId;
        string lStockDescription;
        string lUnit;
        string lLocationId;
        decimal lIN;
        decimal lOUT;
        decimal lBalance;
        decimal lUnitPrice;
        decimal lTotalPrice;
        string lRemarks;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public InventoryDetailDAO()
        {
            lDetailId = "";
            lHeaderId = "";
            lStockId = "";
            lStockDescription = "";
            lUnit = "";
            lLocationId = "";
            lIN = 0;
            lOUT = 0;
            lBalance = 0;
            lUnitPrice = 0;
            lTotalPrice = 0;
            lRemarks = "";
        }
        #endregion "END OF CONSTTRUCTORS"

        #region "METHODS"
        public void loadAttributes(object pObject)
        {
            try
            {
                lDetailId = pObject.GetType().GetProperty("DetailId").GetValue(pObject, null).ToString();
            }
            catch
            {
                lDetailId = "";
            }
            lHeaderId = pObject.GetType().GetProperty("HeaderId").GetValue(pObject, null).ToString();
            lStockId = pObject.GetType().GetProperty("StockId").GetValue(pObject, null).ToString();
            lStockDescription = pObject.GetType().GetProperty("StockDescription").GetValue(pObject, null).ToString();
            lUnit = pObject.GetType().GetProperty("Unit").GetValue(pObject, null).ToString();
            lLocationId = pObject.GetType().GetProperty("LocationId").GetValue(pObject, null).ToString();
            lIN = decimal.Parse(pObject.GetType().GetProperty("IN").GetValue(pObject, null).ToString());
            lOUT = decimal.Parse(pObject.GetType().GetProperty("OUT").GetValue(pObject, null).ToString());
            lBalance = decimal.Parse(pObject.GetType().GetProperty("Balance").GetValue(pObject, null).ToString());
            lUnitPrice = decimal.Parse(pObject.GetType().GetProperty("UnitPrice").GetValue(pObject, null).ToString());
            lTotalPrice = decimal.Parse(pObject.GetType().GetProperty("TotalPrice").GetValue(pObject, null).ToString());
            lRemarks = pObject.GetType().GetProperty("Remarks").GetValue(pObject, null).ToString();
        }

        public DataTable getInventoryDetails(string pHeaderId)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetInventoryDetails('" + pHeaderId + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getInventoryDetail(string pHeaderId)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetInventoryDetail('" + pHeaderId + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getStockInventory(string pLocationId)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetStockInventory('" + pLocationId + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getStockInventoryList(string pLocationId, string pSearchString)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetStockInventoryList('" + pLocationId + "','" + pSearchString + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool insertInventoryDetail(object pObject, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                loadAttributes(pObject);
                MySqlCommand _cmd = new MySqlCommand("call spInsertInventoryDetail('" + lHeaderId + "', '" +
                                                                           lStockId + "','" +
                                                                           lStockDescription + "','" +
                                                                           lUnit + "','" +
                                                                           lLocationId + "','" +
                                                                           lIN + "','" +
                                                                           lOUT + "','" +
                                                                           lBalance + "','" +
                                                                           lUnitPrice + "','" +
                                                                           lTotalPrice + "','" +
                                                                           lRemarks + "','" +
                                                                           GlobalVariables.Username + "','" +
                                                                           GlobalVariables.Hostname + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = pTrans;
                    int _RowsAffected = _cmd.ExecuteNonQuery();
                    if (_RowsAffected > 0)
                    {
                        _success = true;
                    }
                    else
                    {
                        _success = false;
                    }
                    return _success;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool updateInventoryDetail(object pObject, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                loadAttributes(pObject);
                MySqlCommand _cmd = new MySqlCommand("call spUpdateInventoryDetail('" + lDetailId + "', '" +
                                                                           lHeaderId + "','" +                                                       
                                                                           lStockId + "','" +
                                                                           lStockDescription + "','" +
                                                                           lUnit + "','" +
                                                                           lLocationId + "','" +
                                                                           lIN + "','" +
                                                                           lOUT + "','" +
                                                                           lBalance + "','" +
                                                                           lUnitPrice + "','" +
                                                                           lTotalPrice + "','" +
                                                                           lRemarks + "','" +
                                                                           GlobalVariables.Username + "','" +
                                                                           GlobalVariables.Hostname + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = pTrans;
                    int _RowsAffected = _cmd.ExecuteNonQuery();
                    if (_RowsAffected > 0)
                    {
                        _success = true;
                    }
                    else
                    {
                        _success = false;
                    }
                    return _success;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool removeInventoryDetail(string pDetailId,ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                MySqlCommand _cmd = new MySqlCommand("call spRemoveInventoryDetail('" + pDetailId + "','" +
                                                                           GlobalVariables.Username + "','" +
                                                                           GlobalVariables.Hostname + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = pTrans;
                    int _RowsAffected = _cmd.ExecuteNonQuery();
                    if (_RowsAffected > 0)
                    {
                        _success = true;
                    }
                    else
                    {
                        _success = false;
                    }
                    return _success;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool updateQtyOnHand(string pDetailId, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                MySqlCommand _cmd = new MySqlCommand("call spUpdateQtyOnHand('" + pDetailId + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = pTrans;
                    int _RowsAffected = _cmd.ExecuteNonQuery();
                    if (_RowsAffected > 0)
                    {
                        _success = true;
                    }
                    else
                    {
                        _success = false;
                    }
                    return _success;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion "END OF METHODS"
    }
}
