using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

using NSites.Global;
using NSites.ApplicationObjects.DataAccessObjects;

namespace NSites.ApplicationObjects.Classes
{
    public class User
    {
        #region "VARIABLES"
        UserDAO lUserDAO;
        #endregion

        #region "CONSTRUCTORS"
        public User()
        {
            lUserDAO = new UserDAO();
        }
        #endregion

        #region "PROPERTIES"
        public string Username
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public string Fullname
        {
            get;
            set;
        }
        public string Position
        {
            get;
            set;
        }
        public string UserGroupId
        {
            get;
            set;
        }
        public string Remarks
        {
            get;
            set;
        }
        #endregion

        #region "METHODS"
        public DataTable getAllData(string pDisplayType, string pSearchString)
        {
            return lUserDAO.getUsers(pDisplayType, pSearchString);
        }
        public DataTable getUserInfo(string pUsername)
        {
            return lUserDAO.getUserInfo(pUsername);
        }
        public DataTable getSupervisorRights(string pEmployeeCode)
        {
            return lUserDAO.getSupervisorRights(pEmployeeCode);
        }
        public bool connectDatabase()
        {
            return lUserDAO.connectDatabase();
        }
        public bool connectdbBackup(string pServer, string pDatabase, string pUsername, string pPassword, string pPort)
        {
            try
            {
                return lUserDAO.connectdbBackup(pServer, pDatabase, pUsername, pPassword, pPort);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool autenticateUser(string pUserName, string pPassword)
        {
            return lUserDAO.authenticateUser(pUserName, pPassword);
        }
        public bool checkUserPassword(string pCurrentPassword)
        {
            return lUserDAO.checkUserPassword(pCurrentPassword);
        }
        public bool changePassword(string pNewPassword, string pCurrentPassword)
        {
            return lUserDAO.changePassword(pNewPassword, pCurrentPassword);
        }
        public bool save(GlobalVariables.Operation pOperation,ref MySqlTransaction pTrans)
        {
            bool _Status = false;
            try
            {
                switch (pOperation)
                {
                    case GlobalVariables.Operation.Add:
                        _Status = lUserDAO.insertUser(this, ref pTrans);
                        break;
                    case GlobalVariables.Operation.Edit:
                        _Status = lUserDAO.updateUser(this, ref pTrans);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBoxUI mb = new MessageBoxUI(ex, GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
            }
            return _Status;
        }
        public bool remove(string pUsername, ref MySqlTransaction pTrans)
        {
            bool _Status = false;
            try
            {
                _Status = lUserDAO.removeUser(pUsername,ref pTrans);
            }
            catch (Exception ex)
            {
                MessageBoxUI mb = new MessageBoxUI(ex, GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
            }
            return _Status;
        }

        #endregion
    }
}
