using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Keyfactor.Logging;
using Keyfactor.Orchestrators.Extensions;
using Keyfactor.Orchestrators.Common.Enums;

using Keyfactor.Extensions.Orchestrator.Fortanix.API;
using Keyfactor.Extensions.Orchestrator.Fortanix.Models;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace Keyfactor.Extensions.Orchestrator.Fortanix
{
    public class Inventory : IInventoryJobExtension
    {
        public string ExtensionName => "";

        //Job Entry Point
        public JobResult ProcessJob(InventoryJobConfiguration config, SubmitInventoryUpdate submitInventory)
        {
            ILogger logger = LogHandler.GetClassLogger<Inventory>();
            logger.LogDebug($"Begin Fortanix Inventory job for job id {config.JobId}...");
            logger.MethodEntry(LogLevel.Debug);

            List<CurrentInventoryItem> inventoryItems = new List<CurrentInventoryItem>();

            try
            {
                if (config.CertificateStoreDetails.StorePath == null || string.IsNullOrEmpty(config.CertificateStoreDetails.StorePath))
                    throw new Exception("Missing Store Path.  Please provide a valid store path that should map to the Fortanix API base URL used to manage your Fortanix certificate store.");

                if (config.CertificateStoreDetails.StorePassword == null || string.IsNullOrEmpty(config.CertificateStoreDetails.StorePassword))
                    throw new Exception("Missing Fortanix API key.  Please provide a valid Fortanix API key in the Keyfactor Command Fortanix certificate store you are working with as the store password.");

                APIProcessor api = new APIProcessor();
                api.Initialize(config.CertificateStoreDetails.StorePath, config.CertificateStoreDetails.StorePassword).GetAwaiter().GetResult();

                List<SecurityObject> certificates = api.GetCertificates().Result;

                foreach(SecurityObject certificate in certificates)
                {
                    inventoryItems.Add(new CurrentInventoryItem()
                    {
                        ItemStatus = OrchestratorInventoryItemStatus.Unknown,
                        Alias = certificate.Name,
                        PrivateKeyEntry = false,
                        UseChainLevel = false,
                        Certificates = new string[] { certificate.Certificate }
                    });
                }
            }
            catch (Exception ex)
            {
                logger.MethodExit(LogLevel.Debug);
                return new JobResult() { Result = OrchestratorJobStatusJobResult.Failure, JobHistoryId = config.JobHistoryId, FailureMessage = ExceptionHandler.FlattenExceptionMessages(ex, $"Site {config.CertificateStoreDetails.StorePath} on server {config.CertificateStoreDetails.ClientMachine}:") };
            }

            try
            {
                submitInventory.Invoke(inventoryItems);
                return new JobResult() { Result = OrchestratorJobStatusJobResult.Success, JobHistoryId = config.JobHistoryId };
            }
            catch (Exception ex)
            {
                return new JobResult() { Result = OrchestratorJobStatusJobResult.Failure, FailureMessage = ExceptionHandler.FlattenExceptionMessages(ex, $"Site {config.CertificateStoreDetails.StorePath} on server {config.CertificateStoreDetails.ClientMachine}:") };
            }
            finally
            {
                logger.MethodExit(LogLevel.Debug);
            }
        }
    }
}