using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;

namespace AzureInfraProvisioning.Provisioning.Powershell
{
    /// <summary>
    /// 
    /// </summary>
    public class InvokePowershell
    {

        public async Task<string> RunPowershellScript(string scriptName, Dictionary<string, string> parameters)
        {
            using (var ps = PowerShell.Create())
            {
               // var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                var script =
                    $@"{scriptName} {string.Join(" ", parameters.Select(p => "-" + p.Key + " '" + p.Value + "'"))}";
                const string path = @"C:\git-hack2017\FuncAzureInfraProvisioning\InfraTemplates\InfraTemplates";
                string completePath = string.Concat(path + "\\"+ script);
                //  log.Debug("Running the following script:\r\n " + script);
                ps.AddScript(completePath);
               // ps.AddScript(script);

                var task = Task<PSDataCollection<PSObject>>.Factory.FromAsync(ps.BeginInvoke(), ps.EndInvoke);
                var objs = await task;

                if (ps.Streams.Error.Count > 0)
                {
                    throw new Exception(ps.Streams.Error[0].ToString());
                }
                else
                {
                    if (objs.Count > 0)
                    {
                        var result = objs[0].ToString();
                        return string.IsNullOrEmpty(result) ? null : result;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Sample execution scenario 2: Asynchronous
        /// </summary>
        /// <remarks>
        /// Executes a PowerShell script asynchronously with script output and event handling.
        /// </remarks>
        public async Task<PSDataCollection<PSObject>> ExecuteAsynchronously()
        {

            using (var powerShellInstance = PowerShell.Create())
            {
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                powerShellInstance.AddScript(path + "\\Deploy-AzureResourceGroup.ps1");

                // this script has a sleep in it to simulate a long running script
                //  PowerShellInstance.AddScript("$s1 = 'test1'; $s2 = 'test2'; $s1; write-error 'some error';start-sleep -s 7; $s2");
               // Task<PSDataCollection<PSObject>> asyncCollection;
                // prepare a new collection to store output stream objects
                var outputCollection = new PSDataCollection<PSObject>();
                outputCollection.DataAdded += outputCollection_DataAdded;

                // the streams (Error, Debug, Progress, etc) are available on the PowerShell instance.
                // we can review them during or after execution.
                // we can also be notified when a new item is written to the stream (like this):
                powerShellInstance.Streams.Error.DataAdded += Error_DataAdded;

                // begin invoke execution on the pipeline
                // use this overload to specify an output stream buffer
                //  IAsyncResult result = powerShellInstance.BeginInvoke<PSObject, PSObject>(null, outputCollection);

                outputCollection = await Task.Factory.FromAsync(powerShellInstance.BeginInvoke<PSObject, PSObject>(null, outputCollection), pResult => powerShellInstance.EndInvoke(pResult)); //  BLOCKS here No await Keyword so it waits for this call to complete

                //await taskQue;

                //// do something else until execution has completed.
                //// this could be sleep/wait, or perhaps some other work
                //while (result.IsCompleted == false)
                //{
                //    Console.WriteLine("Waiting for pipeline to finish...");
                //    Thread.Sleep(1000);

                //    // might want to place a timeout here...
                //}

                Console.WriteLine("Execution has stopped. The pipeline state: " + powerShellInstance.InvocationStateInfo.State);

                foreach (PSObject outputItem in outputCollection)
                {
                    //TODO: handle/process the output items if required
                    Console.WriteLine(outputItem.BaseObject.ToString());
                }
                return outputCollection;
            }
        }

        /// <summary>
        /// Event handler for when data is added to the output stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        private void outputCollection_DataAdded(object sender, DataAddedEventArgs e)
        {
            // do something when an object is written to the output stream
            Console.WriteLine("Object added to output.");
        }

        /// <summary>
        /// Event handler for when Data is added to the Error stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all error output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        private void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            // do something when an error is written to the error stream
            Console.WriteLine("An error was written to the Error stream!");
        }
    }
}
