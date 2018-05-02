using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncAzureInfraProvisioning.Config
{
    public class ProvisioningConfig
    {
        public string DataStorageAccountConnectionString => GetValue(nameof(DataStorageAccountConnectionString));

    
        protected string GetValue(string key)
        {
            string value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value))
            {
                throw new NullReferenceException("Configuration keys can not be null.");
            }
            return value;
        }
    }
}
