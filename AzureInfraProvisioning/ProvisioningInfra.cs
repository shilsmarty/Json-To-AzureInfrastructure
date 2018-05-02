using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AzureInfraProvisioning.Mapping;
using AzureInfraProvisioning.Provisioning;
using System.Collections.Generic;

namespace AzureInfraProvisioning
{
    /// <summary>
    /// Provisioning class library which will do all the facade activities.
    /// </summary>
    public class ProvisioningInfra
    {
        private readonly ResourceMapping _mapping = new ResourceMapping();
        private readonly InitiateProvisioning _initiateProvisioning = new InitiateProvisioning();

        public async Task<Dictionary<string, string>> InvokeFunctionAsync(string blobContent)
        {
            var methodName = GetMethodName();
            Dictionary<string, string> response = null;
            //Call Model Converter
            var provisioningmodel = new ModelConverter.ModelConverter().GetSerializedModel(blobContent);
            if (provisioningmodel != null)
            {
                var resourceProperties = _mapping.GetResourceProperties(provisioningmodel);
                if (resourceProperties != null)
                {
                    response = await _initiateProvisioning.InitiateDeployment(resourceProperties);
                }
            }
            if (response != null)
            {
                foreach (var resource in response)
                {
                    Console.WriteLine($"Resource : {0} , Provisioning Output : {1}", resource.Key, resource.Value);
                }
                return response;
            }
            return null;
        }

        public static string GetMethodName([CallerMemberName] string method = "")
        {
            return method;
        }
    }
}
