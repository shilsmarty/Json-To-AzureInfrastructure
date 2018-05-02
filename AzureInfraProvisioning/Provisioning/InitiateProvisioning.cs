using AzureInfraProvisioning.Provisioning.Powershell;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureInfraProvisioning.Provisioning
{
    public class InitiateProvisioning
    {
        private readonly InvokePowershell _invokePs = new InvokePowershell();
        private readonly ModelConverter.ModelConverter _converter = new ModelConverter.ModelConverter();

        public async Task<Dictionary<string,string>> InitiateDeployment(Dictionary<string, Dictionary<string, string>> resourceBag)
        {
            var generalProperties = resourceBag["general"];
            var response = new Dictionary<string,string>();
            foreach (var resource in resourceBag)
            {
                if (resource.Key == "general") continue;
                var psParam = _converter.GetPowershellParameters(generalProperties, resource.Key);

                if (psParam != null)
                {
                    var outputText = await _invokePs.RunPowershellScript("Deploy-AzureResourceGroup.ps1", psParam);
                    response.Add(resource.Key,outputText);
                }
            }
            return response;
        }
    }
}
