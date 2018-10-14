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
    class CommonDAO
    {
        #region "VARIABLES"
        MySqlConnection connBackup;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public CommonDAO()
        {
            
        }
        #endregion "END OF CONSTTRUCTORS"

        #region "SEARCH METHOD"
        public DataTable getDataFromSearch(string pQuery)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter(pQuery, GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion "END OF SEARCH METHODS"

        #region "BACK UP TRANSACTIONAL"
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
                throw ex;
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
                throw ex;
            }
        }
        public bool connectDatabaseBackup(string pServer, string pDatabase, string pUsername, string pPassword, string pPort)
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
                throw ex;
            }
            return status;
        }
        
        public bool insertBackupTableItem(string pTable, string pValues, ref MySqlTransaction pTransBackup)
        {
            bool _success = false;
            string sp = "call spInsertBackup" + pTable + "(" + pValues + ");";
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
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        //search
        #region "SEARCH"
        string lMenuName = "";
        string lTemplateName = "";
        string lDisplayFields = "";
        string lFields = "";
        string lOperator = "";
        string lValues = "";
        string lCheckAnd = "";
        string lCheckOr = "";
        int lSequenceNo;
        string lPrivate = "";
        string lFieldName = "";
        string lGroupBy = "";
        string lSortBy = "";
        public DataTable getTemplateNames(string pMenuName)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetTemplateNames('" + pMenuName + "','" + GlobalVariables.Username + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getDisplayFields(string pTableName, string pTemplateName)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetDisplayFields('" + pTableName + "','" + pTemplateName + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void loadDisplayFieldsAttributes(object pObject)
        {
            lMenuName = pObject.GetType().GetProperty("MenuName").GetValue(pObject, null).ToString();
            lTemplateName = pObject.GetType().GetProperty("TemplateName").GetValue(pObject, null).ToString();
            lDisplayFields = pObject.GetType().GetProperty("DisplayFields").GetValue(pObject, null).ToString();
            lSequenceNo = int.Parse(pObject.GetType().GetProperty("SequenceNo").GetValue(pObject, null).ToString());
            lPrivate = pObject.GetType().GetProperty("Private").GetValue(pObject, null).ToString();
        }
        public bool insertDisplayFields(object pObject, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            //MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();
            try
            {
                loadDisplayFieldsAttributes(pObject);
                MySqlCommand _cmd = new MySqlCommand("call spInsertSearchDisplayFields('" + lMenuName + "', '" +
                                                                           lTemplateName + "','" +
                                                                           lDisplayFields + "','" +
                                                                           lSequenceNo + "','" +
                                                                           lPrivate + "','" +
                                                                           GlobalVariables.Username + "')", GlobalVariables.Connection);
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
                    //_Trans.Rollback();
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public bool removeSearchFields(string pMenuName, string pTemplateName, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                MySqlCommand _cmd = new MySqlCommand("call spRemoveSearchFields('" + pMenuName + "', '" +
                                                                           pTemplateName + "')", GlobalVariables.Connection);
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
        public bool removeTemplateName(string pMenuName, string pTemplateName, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                MySqlCommand _cmd = new MySqlCommand("call spRemoveTemplateName('" + pMenuName + "', '" +
                                                                           pTemplateName + "')", GlobalVariables.Connection);
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
        public bool renameTemplateName(string pTableName, string pTemplateName, string pNewTemplateName, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            try
            {
                MySqlCommand _cmd = new MySqlCommand("call spRenameTemplateName('" + pTableName + "', '" +
                                                                           pTemplateName + "','" +
                                                                           pNewTemplateName + "')", GlobalVariables.Connection);
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
        //filters
        public DataTable getFilters(string pTableName, string pTemplateName)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetFilters('" + pTableName + "','" + pTemplateName + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void loadFiltersAttributes(object pObject)
        {
            lMenuName = pObject.GetType().GetProperty("MenuName").GetValue(pObject, null).ToString();
            lTemplateName = pObject.GetType().GetProperty("TemplateName").GetValue(pObject, null).ToString();
            lFields = pObject.GetType().GetProperty("Fields").GetValue(pObject, null).ToString();
            lOperator = pObject.GetType().GetProperty("Operator").GetValue(pObject, null).ToString();
            //lOperatorSign = pObject.GetType().GetProperty("OperatorSign").GetValue(pObject, null).ToString();
            lValues = pObject.GetType().GetProperty("Values").GetValue(pObject, null).ToString();
            lCheckAnd = pObject.GetType().GetProperty("CheckAnd").GetValue(pObject, null).ToString();
            lCheckOr = pObject.GetType().GetProperty("CheckOr").GetValue(pObject, null).ToString();
            lSequenceNo = int.Parse(pObject.GetType().GetProperty("SequenceNo").GetValue(pObject, null).ToString());
            lPrivate = pObject.GetType().GetProperty("Private").GetValue(pObject, null).ToString();
        }
        public bool insertFilters(object pObject, ref MySqlTransaction pTrans)
        {
            bool _success = false;
            //MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();
            try
            {
                loadFiltersAttributes(pObject);
                MySqlCommand _cmd = new MySqlCommand("call spInsertSearchFilters('" + lMenuName + "', '" +
                                                                           lTemplateName + "','" +
                                                                           lFields + "','" +
                                                                           lOperator + "','" +
                                                                           lValues + "','" +
                                                                           lCheckAnd + "','" +
                                                                           lCheckOr + "','" +
                                                                           lSequenceNo + "','" +
                                                                           lPrivate + "','" +
                                                                           GlobalVariables.Username + "')", GlobalVariables.Connection);
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
                    //_Trans.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        //groupings
        public DataTable getGroups(string pTableName, string pTemplateName)
        {
            DataTable _dt = new DataTable();
            try
            {
                MySqlDataAdapter _da = new MySqlDataAdapter("call spGetGroups('" + pTableName + "','" + pTemplateName + "')", GlobalVariables.Connection);
                _da.Fill(_dt);

                return _dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void loadGroupAttributes(object pObject)
        {
            lMenuName = pObject.GetType().GetProperty("MenuName").GetValue(pObject, null).ToString();
            lTemplateName = pObject.GetType().GetProperty("TemplateName").GetValue(pObject, null).ToString();
            lFieldName = pObject.GetType().GetProperty("FieldName").GetValue(pObject, null).ToString();
            lGroupBy = pObject.GetType().GetProperty("GroupBy").GetValue(pObject, null).ToString();
            lSortBy = pObject.GetType().GetProperty("SortBy").GetValue(pObject, null).ToString();
            lSequenceNo = int.Parse(pObject.GetType().GetProperty("SequenceNo").GetValue(pObject, null).ToString());
            lPrivate = pObject.GetType().GetProperty("Private").GetValue(pObject, null).ToString();
        }

        public bool insertGroupings(object pObject)
        {
            bool _success = false;
            MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();
            try
            {
                loadGroupAttributes(pObject);
                MySqlCommand _cmd = new MySqlCommand("call spInsertSearchGroupings('" + lMenuName + "', '" +
                                                                           lTemplateName + "','" +
                                                                           lFieldName + "','" +
                                                                           lGroupBy + "','" +
                                                                           lSortBy + "','" +
                                                                           lSequenceNo + "','" +
                                                                           lPrivate + "','" +
                                                                           GlobalVariables.Username + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = _Trans;
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
                    _Trans.Rollback();
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool updateGroupings(object pObject)
        {
            bool _success = false;
            MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();
            try
            {
                loadGroupAttributes(pObject);
                MySqlCommand _cmd = new MySqlCommand("call spUpdateSearchGroupings('" + lMenuName + "', '" +
                                                                           lTemplateName + "','" +
                                                                           lFieldName + "','" +
                                                                           lGroupBy + "','" +
                                                                           lSortBy + "','" +
                                                                           lSequenceNo + "','" +
                                                                           lPrivate + "','" +
                                                                           GlobalVariables.Username + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = _Trans;
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
                    _Trans.Rollback();
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string insertSearchTemplate(string pTemplateName, string pItemName, string pPrivate)
        {
            string _Id = "";
            MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();
            try
            {
                MySqlCommand _cmd = new MySqlCommand("call spInsertSearchTemplate('" + pTemplateName + "', '" +
                                                                           pItemName + "','" +
                                                                           pPrivate + "','" +
                                                                           GlobalVariables.Username + "','" +
                                                                           GlobalVariables.Hostname + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = _Trans;
                    _Id = _cmd.ExecuteScalar().ToString();

                    return _Id;
                }
                catch (Exception ex)
                {
                    _Trans.Rollback();
                    throw new Exception(ex.Message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool removeSearchFilter(string pTemplateId)
        {
            bool _success = false;
            try
            {
                MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();
                MySqlCommand _cmd = new MySqlCommand("call spRemoveSearchFilter('" + pTemplateId + "','" +
                                                                             GlobalVariables.Username + "','" +
                                                                             GlobalVariables.Hostname + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = _Trans;
                    int _RowsAffected = _cmd.ExecuteNonQuery();
                    _Trans.Commit();

                    if (_RowsAffected < 0)
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
                    _Trans.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool insertSearchFilter(string pTemplateId, string pField, string pOperator, string pValue, string pCheckAnd, string pCheckOr, int pSequence)
        {
            bool _success = false;
            try
            {
                MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();

                MySqlCommand _cmd = new MySqlCommand("call spInsertSearchFilter('" + pTemplateId + "','" +
                                                                            pField + "','" +
                                                                            pOperator + "','" +
                                                                            pValue + "','" +
                                                                            pCheckAnd + "','" +
                                                                            pCheckOr + "','" +
                                                                            pSequence + "','" +
                                                                            GlobalVariables.Username + "','" +
                                                                            GlobalVariables.Hostname + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = _Trans;
                    _Trans.Commit();
                    int _RowsAffected = _cmd.ExecuteNonQuery();
                    if (_RowsAffected < 0)
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
                    _Trans.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool removeSearchTemplate(string pId)
        {
            bool _success = false;
            try
            {
                MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();

                MySqlCommand _cmd = new MySqlCommand("call spRemoveSearchTemplate('" + pId + "','" +
                                                                            GlobalVariables.Username + "','" +
                                                                            GlobalVariables.Hostname + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = _Trans;
                    int _RowsAffected = _cmd.ExecuteNonQuery();
                    _Trans.Commit();

                    if (_RowsAffected < 0)
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
                    _Trans.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool renameSearchTemplate(string pId, string pTemplateName)
        {
            bool _success = false;
            try
            {
                MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();

                MySqlCommand _cmd = new MySqlCommand("call spRenameSearchTemplate('" + pId + "','" +
                                                                            pTemplateName + "','" +
                                                                            GlobalVariables.Username + "','" +
                                                                            GlobalVariables.Hostname + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = _Trans;
                    int _RowsAffected = _cmd.ExecuteNonQuery();
                    _Trans.Commit();

                    if (_RowsAffected < 0)
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
                    _Trans.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool updateSearchTemplate(string pId, string pTemplateName, string pItemName, string pPrivate)
        {
            bool _success = false;
            try
            {
                MySqlTransaction _Trans = GlobalVariables.Connection.BeginTransaction();

                MySqlCommand _cmd = new MySqlCommand("call spUpdateSearchTemplate('" + pId + "','" +
                                                                            pTemplateName + "','" +
                                                                            pItemName + "','" +
                                                                            pPrivate + "','" +
                                                                            GlobalVariables.Username + "','" +
                                                                            GlobalVariables.Hostname + "')", GlobalVariables.Connection);
                try
                {
                    _cmd.Transaction = _Trans;
                    int _RowsAffected = _cmd.ExecuteNonQuery();
                    _Trans.Commit();
                    
                    if (_RowsAffected < 0)
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
                    _Trans.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion "END OF SEARCH"
    }
}
