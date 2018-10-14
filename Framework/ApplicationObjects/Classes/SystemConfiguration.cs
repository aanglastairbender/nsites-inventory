using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using NSites.Global;
using NSites.ApplicationObjects.DataAccessObjects;

namespace NSites.ApplicationObjects.Classes
{
    class SystemConfiguration
    {
        #region "VARIABLES"
        SystemConfigurationDAO lSystemConfigurationDAO;
        #endregion "END OF VARIABLES"

        #region "CONSTRUCTORS"
        public SystemConfiguration()
        {
            lSystemConfigurationDAO = new SystemConfigurationDAO();
        }
        #endregion "END OF CONSTTRUCTORS"

        #region "PROPERTIES"
        public string Key
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        }

        #endregion "END OF PROPERTIES"

        #region "METHODS"
        public DataTable getAllData()
        {
            return lSystemConfigurationDAO.getSystemConfigurations();
        }
        public DataTable getSystemConfiguration(string pKey)
        {
            return lSystemConfigurationDAO.getSystemConfiguration(pKey);
        }
        public bool saveSystemConfiguration(GlobalVariables.Operation pOperation)
        {
            bool _Status = false;
            try
            {
                switch (pOperation)
                {
                    case GlobalVariables.Operation.Add:
                        _Status = lSystemConfigurationDAO.insertSystemConfiguration(this);
                        break;
                    case GlobalVariables.Operation.Edit:
                        _Status = lSystemConfigurationDAO.updateSystemConfiguration(this);
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
        public bool removeSystemConfiguration(string pKey)
        {
            bool _Status = false;
            try
            {
                _Status = lSystemConfigurationDAO.removeSystemConfiguration(pKey);
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
