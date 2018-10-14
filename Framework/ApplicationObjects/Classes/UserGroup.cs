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
    class UserGroup
    {
        #region "VARIABLES"
        UserGroupDAO lUserGroupDAO;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public UserGroup()
        {
            lUserGroupDAO = new UserGroupDAO();
        }
        #endregion "END OF CONSTTRUCTORS"

        #region "PROPERTIES"
        public string Id
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string Remarks
        {
            get;
            set;
        }
        #endregion "END OF PROPERTIES"

        #region "METHODS"
        public DataTable getAllData(string pDisplayType, string pSearchString)
        {
            return lUserGroupDAO.getUserGroups(pDisplayType,pSearchString);
        }
        public DataTable getUserGroupMenuItems()
        {
            return lUserGroupDAO.getUserGroupMenuItems();
        }
        public DataTable getUserGroupRights()
        {
            return lUserGroupDAO.getUserGroupRights();
        }
        public DataTable getMenuItemsByGroup(string pUserGroupId)
        {
            return lUserGroupDAO.getMenuItemsByGroup(pUserGroupId);
        }
        public DataTable getAllMenuItems()
        {
            return lUserGroupDAO.getAllMenuItems();
        }
        public DataTable getAllRights(string pItemName)
        {
            return lUserGroupDAO.getAllRights(pItemName);
        }
        public DataTable getEnableRights(string pItemName, string pUserGroupId)
        {
            return lUserGroupDAO.getEnableRights(pItemName, pUserGroupId);
        }
        public string save(GlobalVariables.Operation pOperation,ref MySqlTransaction pTrans)
        {
            string _UserGroupId = "";
            try
            {
                switch (pOperation)
                {
                    case GlobalVariables.Operation.Add:
                        _UserGroupId = lUserGroupDAO.insertUserGroup(this, ref pTrans);
                        break;
                    case GlobalVariables.Operation.Edit:
                        _UserGroupId = lUserGroupDAO.updateUserGroup(this, ref pTrans);
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
            return _UserGroupId;
        }
        public bool updateUserGroupMenuItem(string pUserGroupId, DataTable pMenuItems)
        {
            bool _Status = false;
            try
            {
                _Status = lUserGroupDAO.updateUserGroupMenuItem(pUserGroupId, pMenuItems);
            }
            catch (Exception ex)
            {
                MessageBoxUI mb = new MessageBoxUI(ex, GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
            }
            return _Status;
        }
        public bool updateUserGroupRights(string pUserGroupId, string pItemName, DataTable pRights)
        {
            bool _Status = false;
            try
            {
                _Status = lUserGroupDAO.updateUserGroupRights(pUserGroupId, pItemName, pRights);
            }
            catch (Exception ex)
            {
                MessageBoxUI mb = new MessageBoxUI(ex, GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
            }
            return _Status;
        }
        public bool removeUserGroup(string pUserGroupId, ref MySqlTransaction pTrans)
        {
            bool _Status = false;
            try
            {
                _Status = lUserGroupDAO.remove(pUserGroupId,ref pTrans);
            }
            catch (Exception ex)
            {
                MessageBoxUI mb = new MessageBoxUI(ex, GlobalVariables.Icons.Error, GlobalVariables.Buttons.OK);
                mb.ShowDialog();
            }
            return _Status;
        }
        #endregion "END OF METHODS"
    }
}
