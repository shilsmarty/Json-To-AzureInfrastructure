using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FuncAzureInfraProvisioning
{
    /// <summary>
    /// 
    /// </summary>
    public static class FunctionAzureInfraBlobTrigger
    {
        [FunctionName("AzureInfraBlobTrigger")]
        public static async Task Run([BlobTrigger("extracted/{name}")]CloudBlockBlob myBlob , string name, TraceWriter log)
        {
            // log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob} Bytes");
            log.Info($"BlockBlob destination name: {myBlob.Name}");
            log.Info($"Block blob of length {myBlob.Properties.Length}: {myBlob.Uri}");

            var blobContent = myBlob.DownloadText();

            //[TODO][shilsr] : Replace this with Unity Initialization.
            var initializeProvisioningInfra = new AzureInfraProvisioning.ProvisioningInfra();

            var output = await initializeProvisioningInfra.InvokeFunctionAsync(blobContent);
            if (output != null)
            {
                foreach (var resource in output)
                {
                    log.Info($"Resource : {resource.Key} , Provisioning Output : {resource.Value}");
                }
            }
            log.Info("Execution of function AzureInfraBlobTrigger completed.");
            //var value = new ProvisioningConfig();
            //var b = value.DataStorageAccountConnectionString;
        }
    }
}