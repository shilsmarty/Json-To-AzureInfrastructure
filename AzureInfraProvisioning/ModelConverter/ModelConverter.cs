using System;
using AzureInfraProvisioning.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AzureInfraProvisioning.ModelConverter
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blobContent"></param>
        /// <returns></returns>
        public ProvisioningRequest GetSerializedModel(string blobContent)
        {
            if (string.IsNullOrEmpty(blobContent))
            {
                throw new NullReferenceException();
            }

            return JsonConvert.DeserializeObject<ProvisioningRequest>(blobContent);
        }

        public Dictionary<string, string> GetPropertyBag(Properties propBag)
        {
            if (propBag == null)
            {
                throw new NullReferenceException();
            }
            var propertyNames = typeof(Properties).GetProperties()
                .Select(p => new KeyValuePair<string, string>(p.Name, p.GetValue(propBag).ToString())).ToList();

            return propertyNames.ToDictionary(x => x.Key, x => x.Value);
        }

        public Dictionary<string, string> GetPowershellParameters(Dictionary<string, string> generalProp , string resourceType)
        {
            if (generalProp == null)
            {
                throw new NullReferenceException();
            }
            return new Dictionary<string, string>
            {
                {"ResourceGroupLocation", generalProp["ResourceGroupLocation"]},
                {"ResourceGroupName", generalProp["ResourceGroupName"]},
                {"ResourceType", resourceType},
                {"SubscriptionId", generalProp["SubscriptionId"]}
            };
        }
    }
}
