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
    class InventoryGroupDAO
    {
        #region "VARIABLES"
        string lId;
        string lDescription;
        string lRemarks;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public InventoryGroupDAO()
        {
            lId = "";
            lDescription = "";
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
            lRemarks = pObject.GetType().GetProperty("Remarks").GetValue(pObject, null).ToString();
        }

        public DataTable getInventoryGroups(string pDisplayType,string pSearchString)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetInventoryGroups('" + pDisplayType + "','" + pSearchString + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string insertInventoryGroup(object pObject, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            try
            {
                loadAttributes(pObject);
                MySqlCommand _cmd = new MySqlCommand("call spInsertInventoryGroup('" + lDescription + "','" +
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

        public string updateInventoryGroup(object pObject, ref MySqlTransaction pTrans)
        {
            string _Id = "";
            try
            {
                loadAttributes(pObject);
                MySqlCommand _cmd = new MySqlCommand("call spUpdateInventoryGroup('" + lId + "','" +
                                                                            lDescription + "','" +
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

        public bool removeInventoryGroup(string pId, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                MySqlCommand _cmd = new MySqlCommand("call spRemoveInventoryGroup('" + pId + "','" +
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
