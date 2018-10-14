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
    class InventoryHeaderDAO
    {
        #region "VARIABLES"
        string lHeaderId;
        DateTime lDate;
        string lType;
        string lReference;
        string lCustomerId;
        string lSupplierId;
        decimal lTotalIN;
        decimal lTotalOUT;
        decimal lTotalAmount;
        string lRemarks;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public InventoryHeaderDAO()
        {
            lHeaderId = "";
            lType = "";
            lReference = "";
            lCustomerId = "";
            lSupplierId = "";
            lTotalIN = 0;
            lTotalOUT = 0;
            lTotalAmount = 0;
            lRemarks = "";
        }
        #endregion "END OF CONSTRUCTORS"

        #region "METHODS"
        public void loadAttributes(object pObject)
        {
            try
            {
                lHeaderId = pObject.GetType().GetProperty("HeaderId").GetValue(pObject, null).ToString();
            }
            catch
            { }
            lDate = DateTime.Parse(pObject.GetType().GetProperty("Date").GetValue(pObject, null).ToString());
            lType = pObject.GetType().GetProperty("Type").GetValue(pObject, null).ToString();
            lReference = pObject.GetType().GetProperty("Reference").GetValue(pObject, null).ToString();
            lCustomerId = pObject.GetType().GetProperty("CustomerId").GetValue(pObject, null).ToString();
            lSupplierId = pObject.GetType().GetProperty("SupplierId").GetValue(pObject, null).ToString();
            lTotalIN = decimal.Parse(pObject.GetType().GetProperty("TotalIN").GetValue(pObject, null).ToString());
            lTotalOUT = decimal.Parse(pObject.GetType().GetProperty("TotalOUT").GetValue(pObject, null).ToString());
            lTotalAmount = decimal.Parse(pObject.GetType().GetProperty("TotalAmount").GetValue(pObject, null).ToString());
            lRemarks = pObject.GetType().GetProperty("Remarks").GetValue(pObject, null).ToString();
        }

        public DataTable getInventoryHeaders(string pDisplayType,string pPrimaryKey, string pSearchString, string pType)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetInventoryHeaders('" + pDisplayType + "','" + pPrimaryKey + "','" + pSearchString + "','" + pType + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable getInventoryHeader(string pId)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetInventoryHeader(" + pId + ")", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable getNextInventoryHeaderNo()
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetNextInventoryHeaderNo()", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable getStockReceivingForCheckVoucher(string pSupplierId)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetStockReceivingForCheckVoucher('" + pSupplierId + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable getStockReceivingDetailPaid(string pCheckVoucherHeaderId)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetStockReceivingDetailPaid('" + pCheckVoucherHeaderId + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable getYearInInventoryHeader()
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetYearInInventoryHeader()", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable getInventoryHeaderMonthlySummary(string pYear, string pType)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetInventoryHeaderMonthlySummary('" +
                                                                               pYear + "','" +
                                                                               pType + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable getAccountPayableReport()
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetAccountPayableReport()", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable getAccountPayableOverDue()
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetAccountPayableOverDue()", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string insertInventoryHeader(object pObject, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            try
            {
                loadAttributes(pObject);
                MySqlCommand _cmd = new MySqlCommand("call spInsertInventoryHeader('" + string.Format("{0:yyyy-MM-dd}", lDate) + "','" +
                                                                               lType + "','" +
                                                                               lReference + "','" +
                                                                               lCustomerId + "','" +
                                                                               lSupplierId + "','" +
                                                                               lTotalIN + "','" +
                                                                               lTotalOUT + "','" + 
                                                                               lTotalAmount + "','" +
                                                                               lRemarks + "','" +
                                                                               GlobalVariables.Username + "','" +
                                                                               GlobalVariables.Hostname + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = pTrans;
                    _Id = _cmd.ExecuteScalar().ToString();

                    return _Id;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string updateInventoryHeader(object pObject, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            try
            {
                loadAttributes(pObject);
                MySqlCommand _cmd = new MySqlCommand("call spUpdateInventoryHeader('" + lHeaderId + "','" +
                                                                            string.Format("{0:yyyy-MM-dd}", lDate) + "','" +
                                                                            lType + "','" +
                                                                            lReference + "','" +
                                                                            lCustomerId + "','" +
                                                                            lSupplierId + "','" +
                                                                            lTotalIN + "','" +
                                                                            lTotalOUT + "','" +
                                                                            lTotalAmount + "','" +
                                                                            lRemarks + "','" +
                                                                            GlobalVariables.Username + "','" +
                                                                            GlobalVariables.Hostname + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = pTrans;
                    _Id = _cmd.ExecuteScalar().ToString();

                    return _Id;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool removeInventoryHeader(string pHeaderId, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                MySqlCommand _cmd = new MySqlCommand("call spRemoveInventoryHeader('" + pHeaderId + "','" +
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
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool finalizeInventoryHeader(string pHeaderId, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                MySqlCommand _cmd = new MySqlCommand("call spFinalizeInventoryHeader('" + pHeaderId + "','" +
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
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool addToAccountPayable(string pDate, string pStockReceivingId, string pReference, string pSupplierId, decimal pTotalAmount,string pDueDate, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                MySqlCommand _cmd = new MySqlCommand("call spAddToAccountPayable('" + pDate + "','" +
                                                                            pStockReceivingId + "','" +
                                                                            pReference + "','" +
                                                                            pSupplierId + "','" +
                                                                            pTotalAmount + "','" +
                                                                            pDueDate + "','" +
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
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion "END OF METHODS"
    }
}
