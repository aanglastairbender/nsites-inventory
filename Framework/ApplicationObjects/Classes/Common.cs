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
    class Common
    {
        #region "VARIABLES"
        CommonDAO loCommonDAO;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public Common()
        {
            loCommonDAO = new CommonDAO();
        }
        #endregion "END OF CONSTTRUCTORS"

        #region "PROPERTIES"

        #endregion "END OF PROPERTIES"

        #region "METHODS"
        public DataTable getDataFromSearch(string pQuery)
        {
            try
            {
                return loCommonDAO.getDataFromSearch(pQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion "END OF METHODS"

        #region "DATABASE BACK UP"
        public bool connectDatabaseBackup(string pServer, string pDatabase, string pUsername, string pPassword, string pPort)
        {
            try
            {
                return loCommonDAO.connectDatabaseBackup(pServer, pDatabase, pUsername, pPassword, pPort);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool insertBackupTableItem(string pTable, string pValues, ref MySqlTransaction pTransBackup)
        {
            return loCommonDAO.insertBackupTableItem(pTable, pValues, ref pTransBackup);
        }
        public bool updateBackupSequence(string pTable, string pOutletCode, int pLastNumber, ref MySqlTransaction pTransBackup)
        {
            return loCommonDAO.updateBackupSequence(pTable, pOutletCode, pLastNumber, ref pTransBackup);
        }
        public bool updateBackupTableItem(string pTable, ref MySqlTransaction pTrans)
        {
            return loCommonDAO.updateBackupTableItem(pTable, ref pTrans);
        }
        #endregion "END OF DATABASE BACK UP"

        #region "SEARCH"
        //displayfields
        public string MenuName
        {
            get;
            set;
        }
        public string TemplateName
        {
            get;
            set;
        }
        public string DisplayFields
        {
            get;
            set;
        }
        public int SequenceNo
        {
            get;
            set;
        }
        public string Private
        {
            get;
            set;
        }
        //filters
        public string Fields
        {
            get;
            set;
        }
        public string Operator
        {
            get;
            set;
        }
        public string OperatorSign
        {
            get;
            set;
        }
        public string Values
        {
            get;
            set;
        }
        public string CheckAnd
        {
            get;
            set;
        }
        public string CheckOr
        {
            get;
            set;
        }
        //groupings
        public string FieldName
        {
            get;
            set;
        }
        public string GroupBy
        {
            get;
            set;
        }
        public string SortBy
        {
            get;
            set;
        }
        public DataTable getTemplateNames(string pMenuName)
        {
            try
            {
                return loCommonDAO.getTemplateNames(pMenuName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable getDisplayFields(string pTableName, string pTemplateName)
        {
            try
            {
                return loCommonDAO.getDisplayFields(pTableName, pTemplateName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool saveDisplayField(GlobalVariables.Operation pOperation, ref MySqlTransaction pTrans)
        {
            bool _status = false;
            switch (pOperation)
            {
                case GlobalVariables.Operation.Add:
                    _status = loCommonDAO.insertDisplayFields(this,ref pTrans);
                    break;
                default:
                    break;
            }
            return _status;
        }
        public bool removeSearchFields(string pTableName, string pTemplateName, ref MySqlTransaction pTrans)
        {
            bool _Status = false;
            try
            {
                _Status = loCommonDAO.removeSearchFields(pTableName, pTemplateName, ref pTrans);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _Status;
        }
        public bool removeTemplateName(string pTableName, string pTemplateName, ref MySqlTransaction pTrans)
        {
            bool _Status = false;
            try
            {
                _Status = loCommonDAO.removeTemplateName(pTableName, pTemplateName, ref pTrans);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _Status;
        }
        public bool renameTemplateName(string pTableName, string pTemplateName, string pNewTemplateName, ref MySqlTransaction pTrans)
        {
            bool _Status = false;
            try
            {
                _Status = loCommonDAO.renameTemplateName(pTableName, pTemplateName, pNewTemplateName, ref pTrans);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _Status;
        }
        //filters
        public DataTable getFilters(string pTableName, string pTemplateName)
        {
            try
            {
                return loCommonDAO.getFilters(pTableName, pTemplateName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool saveFilters(GlobalVariables.Operation pOperation, ref MySqlTransaction pTrans)
        {
            bool _status = false;
            switch (pOperation)
            {
                case GlobalVariables.Operation.Add:
                    _status = loCommonDAO.insertFilters(this, ref pTrans);
                    break;
                default:
                    break;
            }
            return _status;
        }
        //groupings
        public DataTable getGroups(string pTableName, string pTemplateName)
        {
            try
            {
                return loCommonDAO.getGroups(pTableName, pTemplateName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool saveGroupings(GlobalVariables.Operation pOperation)
        {
            bool _status = false;
            switch (pOperation)
            {
                case GlobalVariables.Operation.Add:
                    _status = loCommonDAO.insertGroupings(this);
                    break;
                default:
                    break;
            }
            return _status;
        }

        public string insertSearchTemplate(string pTemplateName, string pItemName, string pPrivate)
        {
            string _Id = "";
            try
            {
                _Id = loCommonDAO.insertSearchTemplate(pTemplateName, pItemName, pPrivate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _Id;
        }

        public bool removeSearchFilter(string pTemplateId)
        {
            bool _Status = false;
            try
            {
                _Status = loCommonDAO.removeSearchFilter(pTemplateId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _Status;
        }

        public bool insertSearchFilter(string pTemplateId, string pField, string pOperator, string pValue, string pCheckAnd, string pCheckOr, int pSequence)
        {
            bool _Status = false;
            try
            {
                _Status = loCommonDAO.insertSearchFilter(pTemplateId, pField, pOperator, pValue, pCheckAnd, pCheckOr, pSequence);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _Status;
        }

        public bool removeSearchTemplate(string pId)
        {
            bool _Status = false;
            try
            {
                _Status = loCommonDAO.removeSearchTemplate(pId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _Status;
        }

        public bool renameSearchTemplate(string pId, string pTemplateName)
        {
            bool _Status = false;
            try
            {
                _Status = loCommonDAO.renameSearchTemplate(pId, pTemplateName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _Status;
        }

        public bool updateSearchTemplate(string pId, string pTemplateName, string pItemName, string pPrivate)
        {
            bool _Status = false;
            try
            {
                _Status = loCommonDAO.updateSearchTemplate(pId, pTemplateName, pItemName, pPrivate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _Status;
        }
        #endregion "END OF SEARCH"
    }
}
