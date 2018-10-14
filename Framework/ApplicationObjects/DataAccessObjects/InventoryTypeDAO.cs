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
    class InventoryTypeDAO
    {
        #region "VARIABLES"
        string lId;
        string lDescription;
        string lQty;
        string lSource;
        string lRemarks;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public InventoryTypeDAO()
        {
            lId = "";
            lDescription = "";
            lQty = "";
            lSource = "";
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
            lDescription = pObject.GetType().GetProperty("Description").GetValue(pObject, null).ToString();
            lQty = pObject.GetType().GetProperty("Qty").GetValue(pObject, null).ToString();
            lSource = pObject.GetType().GetProperty("Source").GetValue(pObject, null).ToString();
            lRemarks = pObject.GetType().GetProperty("Remarks").GetValue(pObject, null).ToString();
        }

        public DataTable getInventoryTypes(string pDisplayType, string pSearchString)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetInventoryTypes('" + pDisplayType + "','" + pSearchString + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable getInventoryType(string pId)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetInventoryType(" + pId + ")", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string insertInventoryType(object pObject, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            try
            {
                loadAttributes(pObject);
                MySqlCommand _cmd = new MySqlCommand("call spInsertInventoryType('" + lDescription + "','" +
                                                                               lQty + "','" +
                                                                               lSource + "','" +
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

        public string updateInventoryType(object pObject, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            try
            {
                loadAttributes(pObject);
                MySqlCommand _cmd = new MySqlCommand("call spUpdateInventoryType('" + lId + "','" +
                                                                            lDescription + "','" +
                                                                            lQty + "','" +
                                                                            lSource + "','" +
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

        public bool removeInventoryType(string pId, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                MySqlCommand _cmd = new MySqlCommand("call spRemoveInventoryType('" + pId + "','" +
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
