using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection;
using System.IO;
using System.Windows.Forms;

using NSites.Global;

namespace NSites.ApplicationObjects.DataAccessObjects
{
    public class UserDAO
    {
        #region "VARIABLES"
        MySqlConnection conn;
        MySqlConnection connBackup;
        CryptorEngine loCryptoEngine;
        string myConnectionString;
        string lUsername;
        string lPassword;
        string lFullname;
        string lPosition;
        string lUserGroupId;
        string lRemarks;
        #endregion

        #region "CONSTRUCTORS"
        public UserDAO()
        {
            myConnectionString = "";
            lUsername = "";
            lPassword = "";
            lFullname = "";
            lPosition = "";
            lUserGroupId = "";
            lRemarks = "";
            loCryptoEngine = new CryptorEngine();
        }
        #endregion

        #region "METHODS"
        public void loadAttributes(object pObject)
        {
            lUsername = pObject.GetType().GetProperty("Username").GetValue(pObject, null).ToString();
            lPassword = pObject.GetType().GetProperty("Password").GetValue(pObject, null).ToString();
            lFullname = pObject.GetType().GetProperty("Fullname").GetValue(pObject, null).ToString();
            lPosition = pObject.GetType().GetProperty("Position").GetValue(pObject, null).ToString();
            lUserGroupId = pObject.GetType().GetProperty("UserGroupId").GetValue(pObject, null).ToString();
            lRemarks = pObject.GetType().GetProperty("Remarks").GetValue(pObject, null).ToString();
        }

        public DataTable getUsers(string pDisplayType, string pSearchString)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetUsers('"+pDisplayType+"','"+pSearchString+"')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getUserInfo(string pUsername)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetUserInfo('" + pUsername + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getSupervisorRights(string pEmployeeCode)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetSupervisorRights('" + pEmployeeCode + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool connectDatabase()
        {
            string line = null;
            System.IO.TextReader readFile = new StreamReader("...\\databaseconnection.txt");
            line = readFile.ReadLine();
            if (line != null)
            {
                string _StringToRead = loCryptoEngine.DecryptString(line);
                myConnectionString = _StringToRead;
            }
            readFile.Close();
            readFile = null;

            try
            {
                conn = new MySqlConnection(myConnectionString);
                conn.Open();
                GlobalVariables.Connection = conn;
                char[] splitter1 = { ';' };
                char[] splitter2 = { '=' };
                string[] database = myConnectionString.Split(splitter1);
                for (int i = 0; i < database.Length; i++)
                {
                    string[] detail = null;
                    detail = database[i].Split(splitter2);
                    if (detail[0].ToString().Replace(" ","") == "SERVER")
                    {
                        GlobalVariables.DatabaseServer = detail[1].ToString();
                    }
                    if (detail[0].ToString().Replace(" ", "") == "DATABASE")
                    {
                        GlobalVariables.DatabaseName = detail[1].ToString();
                    }
                    if (detail[0].ToString().Replace(" ", "") == "UID")
                    {
                        GlobalVariables.DatabaseUID = detail[1].ToString();
                    }
                    if (detail[0].ToString().Replace(" ", "") == "PWD")
                    {
                        GlobalVariables.DatabasePWD = detail[1].ToString();
                    }
                    if (detail[0].ToString().Replace(" ", "") == "PORT")
                    {
                        GlobalVariables.DatabasePort = detail[1].ToString();
                    }
                }


                return true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool authenticateUser(string pUsername, string pPassword)
        {
            DataTable _dt = new DataTable();
            try
            {
                GlobalVariables.Hostname = System.Net.Dns.GetHostName().Remove(15);
            }
            catch
            {
                GlobalVariables.Hostname = System.Net.Dns.GetHostName();
            }
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spAuthenticateUser('" + pUsername + "','" + pPassword + "','" + GlobalVariables.Hostname + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                if (_dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool checkUserPassword(string pCurrentPassword)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spCheckUserPassword('" + pCurrentPassword + "','" + GlobalVariables.Username + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                if (_dt.Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool changePassword(string pNewPassword, string pCurrentPassword)
        {
            bool _success = false;
            MySqlTransaction _myTrans;
            try
            {
                _myTrans = GlobalVariables.Connection.BeginTransaction();
                MySqlCommand _cmd = new MySqlCommand("call spChangePassword('" + GlobalVariables.Username + "','" +
                                pNewPassword + "','" + pCurrentPassword + "','" + GlobalVariables.Hostname + "')", GlobalVariables.Connection);
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
        
        public bool insertUser(object pObject, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                loadAttributes(pObject);
                MySqlCommand _cmd = new MySqlCommand("call spInsertUser('" + lUsername + "', '" +
                                                                             lPassword + "','" +                                                         
                                                                             lFullname + "','" +
                                                                             lPosition + "','" +
                                                                             lUserGroupId + "','" +
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
        public bool updateUser(object pObject, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                loadAttributes(pObject);
                MySqlCommand _cmd = new MySqlCommand("call spUpdateUser('" + lUsername + "', '" +
                                                                             lFullname + "','" +
                                                                             lPosition + "','" +
                                                                             lUserGroupId + "','" +
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
        public bool removeUser(string pUsername, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                MySqlCommand _cmd = new MySqlCommand("call spRemoveUser('" + pUsername + "','" +
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
        #endregion

        #region "BACK UP TRANSACTIONAL"
        public bool connectdbBackup(string pServer, string pDatabase, string pUsername, string pPassword, string pPort)
        {
            bool status = false;
            try
            {
                string myConnectionString = "SERVER=" + pServer + "; DATABASE=" + pDatabase + "; UID=" + pUsername + "; PWD=" + pPassword + "; PORT=" + pPort;
                connBackup = new MySqlConnection(myConnectionString);
                connBackup.Open();
                GlobalVariables.ConnectionBackup = connBackup;
                status = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return status;
        }

        public bool backupTransactional(ref MySqlTransaction pTransBackup, ref MySqlTransaction pTrans)
        {
            bool status = false;
            try
            {
                DataTable _dtTableName = new DataTable();
                DataTable _dtTableItem = new DataTable();
                DataTable _dtTableItemSequence = new DataTable();
                //get the transactional table.
                _dtTableName = getBackupTableName("Transactional");
                foreach (DataRow _drTableName in _dtTableName.Rows)
                {
                    string name = _drTableName["TableName"].ToString();
                    string updatebackup = _drTableName["UpdateBackup"].ToString();
                    //insert to the backup table
                    _dtTableItem = getBackupTableItem(_drTableName["TableName"].ToString());
                    foreach (DataRow _drTableItem in _dtTableItem.Rows)
                    {
                        string values = "";
                        string where = "";
                        for (int i = 0; i < _dtTableItem.Columns.Count; i++)
                        {
                            try
                            {
                                DateTime dt = Convert.ToDateTime(_drTableItem[i].ToString());
                                values += "'" + String.Format("{0:yyyy-M-d HH:mm:ss}", dt) + "',";
                            }
                            catch
                            {
                                if (updatebackup == "N")
                                {
                                    values += "'" + _drTableItem[i].ToString() + "',";
                                }
                                else
                                {
                                    if (i == 0)
                                    {
                                        if (name == "Sequence")
                                        {
                                            where = " WHERE `" + _dtTableItem.Columns[i].ColumnName + "`='" + _drTableItem[i].ToString() + "' AND " + _dtTableItem.Columns[1].ColumnName + "='" + _drTableItem[1].ToString() + "'";
                                        }
                                        else
                                        {
                                            where = " WHERE " + _dtTableItem.Columns[i].ColumnName + "='" + _drTableItem[i].ToString() + "'";
                                        }
                                    }
                                    else
                                    {
                                        values += _dtTableItem.Columns[i].ColumnName + "='" + _drTableItem[i].ToString() + "',";
                                    }
                                }
                            }
                        }
                        try
                        {
                            if (updatebackup == "N")
                            {
                                insertBackupTableItem(_drTableName["TableName"].ToString(), values.Remove(values.Length - 1, 1), ref pTransBackup);
                            }
                            else
                            {
                                updateBackupTableItem(_drTableName["TableName"].ToString(), values.Remove(values.Length - 1, 1) + where, ref pTransBackup);
                            }
                        }
                        catch
                        {
                        }
                    }
                    if (updatebackup == "N")
                    {
                        updateBackupTableItem(_drTableName["TableName"].ToString(), ref pTrans);
                    }
                }
                status = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return status;
        }

        public DataTable getBackupTableName(string pTableType)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetBackupTableName('" + pTableType + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable getRestoreTableName(string pTableType)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetBackupTableName('" + pTableType + "')", GlobalVariables.ConnectionBackup);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable getBackupTableItem(string pTableName)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetBackupTableItem('" + pTableName + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable getRestoreTableItem(string pTableName)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetRestoreTableItem('" + pTableName + "')", GlobalVariables.ConnectionBackup);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool insertBackupTableItem(string pTable, string pValues, ref MySqlTransaction pTransBackup)
        {
            bool _success = false;
            string sp = "INSERT INTO " + pTable + " VALUES(" + pValues + ");";
            try
            {
                MySqlCommand _cmd = new MySqlCommand(sp, GlobalVariables.ConnectionBackup);
                try
                {
                    _cmd.Transaction = pTransBackup;
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
        public bool insertRestoreTableItem(string pTable, string pValues, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            string sp = "call spInsert" + pTable + "(" + pValues + ");";
            try
            {
                MySqlCommand _cmd = new MySqlCommand(sp, GlobalVariables.Connection);
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
        public bool updateBackupTableItem(string pTable, string pValues, ref MySqlTransaction pTransBackup)
        {
            bool _success = false;
            string sp = "UPDATE " + pTable + " SET " + pValues + ";";
            try
            {
                MySqlCommand _cmd = new MySqlCommand(sp, GlobalVariables.ConnectionBackup);
                try
                {
                    _cmd.Transaction = pTransBackup;
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

        public bool updateBackupSequence(string pTable, string pOutletCode, int pLastNumber, ref MySqlTransaction pTransBackup)
        {
            bool _success = false;
            try
            {
                MySqlCommand _cmd = new MySqlCommand("call spUpdateBackupSequence('" + pTable + "','" +
                                                                        pOutletCode + "','" +
                                                                        pLastNumber + "')", GlobalVariables.ConnectionBackup);
                try
                {
                    _cmd.Transaction = pTransBackup;
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

        public bool updateBackupTableItem(string pTable, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                MySqlCommand _cmd = new MySqlCommand("call spUpdateBackupTable('" + pTable + "')", GlobalVariables.Connection);
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
        #endregion
    }
}
