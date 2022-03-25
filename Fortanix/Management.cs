using System;
using System.IO;

using Keyfactor.Logging;
using Keyfactor.Orchestrators.Extensions;
using Keyfactor.Orchestrators.Common.Enums;

using Microsoft.Extensions.Logging;

//namespace Keyfactor.Extensions.Orchestrator.Fortanix
//{
    //public class Management : IManagementJobExtension
    //{
    //    public string ExtensionName => "";

    //    //Job Entry Point
    //    public JobResult ProcessJob(ManagementJobConfiguration config)
    //    {
    //        //METHOD ARGUMENTS...
    //        //config - contains context information passed from KF Command to this job run:
    //        //
    //        // config.Server.Username, config.Server.Password - credentials for orchestrated server - use to authenticate to certificate store server.
    //        //
    //        // config.Store.ClientMachine - server name or IP address of orchestrated server
    //        // config.Store.StorePath - location path of certificate store on orchestrated server
    //        // config.Store.StorePassword - if the certificate store has a password, it would be passed here
    //        //
    //        // config.Store.Properties - JSON object containing certain reserved values for Discovery or custom properties set up in the Certificate Store Type
    //        // config.Store.Properties.dirs.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) - Certificate Store Discovery Job Scheduler - Directories to search
    //        // config.Store.Properties.extensions.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) - Certificate Store Discovery Job Scheduler - Extensions
    //        // config.Store.Properties.ignoreddirs.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) - Certificate Store Discovery Job Scheduler - Directories to ignore
    //        // config.Store.Properties.patterns.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries) - Certificate Store Discovery Job Scheduler - File name patterns to match
    //        //
    //        // config.Job.EntryContents - PKCS12 or PEM representation of certificate being added (Management job only)
    //        // config.Job.Alias - optional string value of certificate alias (used in java keystores and some other store types)
    //        // config.Job.OpeerationType - enumeration representing function with job type.  Used only with Management jobs where this value determines whether the Management job is a CREATE/ADD/REMOVE job.
    //        // config.Job.Overwrite - Boolean value telling the AnyAgent whether to overwrite an existing certificate in a store.  How you determine whether a certificate is "the same" as the one provided is AnyAgent implementation dependent
    //        // config.Job.PfxPassword - For a Management Add job, if the certificate being added includes the private key (therefore, a pfx is passed in config.Job.EntryContents), this will be the password for the pfx.


    //        //NLog Logging to c:\CMS\Logs\CMS_Agent_Log.txt
    //        ILogger logger = LogHandler.GetClassLogger<Inventory>();
    //        logger.LogDebug($"Begin Management...");

    //        bool hasPassword = !string.IsNullOrEmpty(config.JobCertificate.PrivateKeyPassword);
            

    //        try
    //        {
    //            //Management jobs, unlike Discovery, Inventory, and Reenrollment jobs can have 3 different purposes:
    //            switch (config.OperationType)
    //            {
    //                case CertStoreOperationType.Add:
    //                    //OperationType == Add - Add a certificate to the certificate store passed in the config object
    //                    //Code logic to:
    //                    // 1) Connect to the orchestrated server (config.Store.ClientMachine) containing the certificate store
    //                    // 2) Custom logic to add certificate to certificate store (config.Store.StorePath) possibly using alias as an identifier if applicable (config.Job.Alias).  Use alias and overwrite flag (config.Job.Overwrite)
    //                    //     to determine if job should overwrite an existing certificate in the store, for example a renewal.
    //                    break;
    //                case CertStoreOperationType.Remove:
    //                    //OperationType == Remove - Delete a certificate from the certificate store passed in the config object
    //                    //Code logic to:
    //                    // 1) Connect to the orchestrated server (config.Store.ClientMachine) containing the certificate store
    //                    // 2) Custom logic to remove the certificate in a certificate store (config.Store.StorePath), possibly using alias (config.Job.Alias) or certificate thumbprint to identify the certificate (implementation dependent)
    //                    break;
    //                case CertStoreOperationType.Create:
    //                    //OperationType == Create - Create an empty certificate store in the provided location
    //                    //Code logic to:
    //                    // 1) Connect to the orchestrated server (config.Store.ClientMachine) where the certificate store (config.Store.StorePath) will be located
    //                    // 2) Custom logic to first check if the store already exists and add it if not.  If it already exists, implementation dependent as to how to handle - error, warning, success
    //                    break;
    //                default:
    //                    //Invalid OperationType.  Return error.  Should never happen though
    //                    return new JobResult() { Result = OrchestratorJobStatusJobResult.Failure, FailureMessage = $"Site {config.CertificateStoreDetails.StorePath} on server {config.CertificateStoreDetails.ClientMachine}: Unsupported operation: {config.OperationType.ToString()}" };
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            //Status: 2=Success, 3=Warning, 4=Error
    //            return new JobResult() { Result = OrchestratorJobStatusJobResult.Failure, JobHistoryId = config.JobHistoryId, FailureMessage = ExceptionHandler.FlattenExceptionMessages(ex, $"Site {config.CertificateStoreDetails.StorePath} on server {config.CertificateStoreDetails.ClientMachine}:") };
    //        }

    //        //Status: 2=Success, 3=Warning, 4=Error
    //        return new JobResult() { Result = OrchestratorJobStatusJobResult.Success, JobHistoryId = config.JobHistoryId };
    //    }
    //}
//}