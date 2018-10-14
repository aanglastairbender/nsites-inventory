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
    class SystemConfigurationDAO
    {
        #region "VARIABLES"
        string lKey = "";
        string lValue = "";
        #endregion "END OF VARIABLES"
        #region "CONSTRUCTORS"
        public SystemConfigurationDAO()
        {

        }
        #endregion "END OF CONSTTRUCTORS"
        #region "METHODS"
        public void loadAttributes(object pObject)
        {
            lKey = pObject.GetType().GetProperty("Key").GetValue(pObject, null).ToString();
            lValue = pObject.GetType().GetProperty("Value").GetValue(pObject, null).ToString();
        }
        public DataTable getSystemConfigurations()
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetSystemConfigurations()", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getSystemConfiguration(string pKey)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetSystemConfiguration('" + pKey + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool insertSystemConfiguration(object pObject)
        {
            bool _success = false;
            MySqlTransaction _myTrans;
            try
            {
                loadAttributes(pObject);
                _myTrans = GlobalVariables.Connection.BeginTransaction();
                MySqlCommand _cmd = new MySqlCommand("call spInsertSystemConfiguration('" + lKey + "', '" +
                                                                           lValue + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = _myTrans;
                    int _RowsAffected = _cmd.ExecuteNonQuery();
                    _myTrans.Commit();
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
                    _myTrans.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool updateSystemConfiguration(object pObject)
        {
            bool _success = false;
            MySqlTransaction _myTrans;
            try
            {
                loadAttributes(pObject);
                _myTrans = GlobalVariables.Connection.BeginTransaction();
                MySqlCommand _cmd = new MySqlCommand("call spUpdateSystemConfiguration('" + lKey + "', '" +
                                                                             lValue + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = _myTrans;
                    int _RowsAffected = _cmd.ExecuteNonQuery();
                    _myTrans.Commit();
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
                    _myTrans.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool removeSystemConfiguration(string pKey)
        {
            bool _success = false;
            MySqlTransaction _myTrans;
            try
            {
                _myTrans = GlobalVariables.Connection.BeginTransaction();
                MySqlCommand _cmd = new MySqlCommand("call spRemoveSystemConfiguration('" + pKey + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = _myTrans;
                    int _RowsAffected = _cmd.ExecuteNonQuery();
                    _myTrans.Commit();
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
                    _myTrans.Rollback();
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
