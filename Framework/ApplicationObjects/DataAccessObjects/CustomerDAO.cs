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
    class CustomerDAO
    {
        #region "VARIABLES"
        string lId;
        string lName;
        string lAddress;
        string lContactPerson;
        string lContactNo;
        int lTerms;
        decimal lCreditLimit;
        string lRemarks;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public CustomerDAO()
        {
            lId = "";
            lName = "";
            lAddress = "";
            lContactPerson = "";
            lContactNo = "";
            lTerms = 0;
            lCreditLimit = 0;
            lRemarks = "";
        }
        #endregion "END OF CONSTRUCTORS"

        #region "METHODS"
        public void loadAttributes(object pObject)
        {
            try
            {
                lId = pObject.GetType().GetProperty("Id").GetValue(pObject, null).ToString();
            }
            catch
            { }
            lName = pObject.GetType().GetProperty("Name").GetValue(pObject, null).ToString();
            lAddress = pObject.GetType().GetProperty("Address").GetValue(pObject, null).ToString();
            lContactPerson = pObject.GetType().GetProperty("ContactPerson").GetValue(pObject, null).ToString();
            lContactNo = pObject.GetType().GetProperty("ContactNo").GetValue(pObject, null).ToString();
            lTerms = int.Parse(pObject.GetType().GetProperty("Terms").GetValue(pObject, null).ToString());
            lCreditLimit = decimal.Parse(pObject.GetType().GetProperty("CreditLimit").GetValue(pObject, null).ToString());
            lRemarks = pObject.GetType().GetProperty("Remarks").GetValue(pObject, null).ToString();
        }

        public DataTable getCustomers(string pDisplayType, string pSearchString)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetCustomers('" + pDisplayType + "','" + pSearchString + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable getCustomerTransactions(DateTime pFromDate, DateTime pToDate, string pCustomerId)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetCustomerTransactions('" + string.Format("{0:yyyy-MM-dd}", pFromDate) + "','" + string.Format("{0:yyyy-MM-dd}", pToDate) + "'," + pCustomerId + ")", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable getCustomersNotForBilling()
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetCustomersNotForBilling()", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable getCustomersForBilling()
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetCustomersForBilling()", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable getCustomer(string pId)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetCustomer(" + pId + ")", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string insertCustomer(object pObject, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            try
            {
                loadAttributes(pObject);
                MySqlCommand _cmd = new MySqlCommand("call spInsertCustomer('" + lName + "','" +
                                                                               lAddress + "','" +
                                                                               lContactPerson + "','" +
                                                                               lContactNo + "','" +
                                                                               lTerms + "','" +
                                                                               lCreditLimit + "','" +
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

        public string updateCustomer(object pObject, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            try
            {
                loadAttributes(pObject);
                MySqlCommand _cmd = new MySqlCommand("call spUpdateCustomer('" + lId + "','" +
                                                                            lName + "','" +
                                                                            lAddress + "','" +
                                                                            lContactPerson + "','" +
                                                                            lContactNo + "','" +
                                                                            lTerms + "','" +
                                                                            lCreditLimit + "','" +
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

        public bool removeCustomer(string pId, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                MySqlCommand _cmd = new MySqlCommand("call spRemoveCustomer('" + pId + "','" +
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
