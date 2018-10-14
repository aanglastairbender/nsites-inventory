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
    class AuditTrailDAO
    {
        #region "VARIABLES"
        string lLogDescription;
        string lUsername;
        DateTime lDate;
        string lHostname;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public AuditTrailDAO()
        {
            lLogDescription = "";
            lUsername = "";
            lHostname = "";
        }
        #endregion "END OF CONSTTRUCTORS"

        #region "METHODS"
        public void loadAttributes(object pObject)
        {
            lLogDescription = pObject.GetType().GetProperty("LogDescription").GetValue(pObject, null).ToString();
        }
        public DataTable getAuditTrailByDate(DateTime pFrom, DateTime pTo)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetAuditTrailByDate('" + 
                                            String.Format("{0:yyyy-MM-dd}", pFrom) + "','" + 
                                            String.Format("{0:yyyy-MM-dd}", pTo) + "')", 
                                            GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool insertAuditTrail(object pObject)
        {
            bool _success = false;
            MySqlTransaction _myTrans;
            try
            {
                loadAttributes(pObject);
                _myTrans = GlobalVariables.Connection.BeginTransaction();
                MySqlCommand _cmd = new MySqlCommand("call spInsertAuditTrail('" + lLogDescription + "','" +
                                                                           GlobalVariables.Username + "','" +
                                                                           GlobalVariables.Hostname + "')", GlobalVariables.Connection);
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
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool removeAuditTrail(DateTime pFrom, DateTime pTo)
        {
            bool _success = false;
            MySqlTransaction _myTrans;
            try
            {
                _myTrans = GlobalVariables.Connection.BeginTransaction();
                MySqlCommand _cmd = new MySqlCommand("call spRemoveAuditTrail('" + String.Format("{0:yyyy-MM-dd}", pFrom) + "','" + 
                                                                            String.Format("{0:yyyy-MM-dd}", pTo) + "')", GlobalVariables.Connection);
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
