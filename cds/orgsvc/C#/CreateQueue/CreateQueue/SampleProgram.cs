﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerApps.Samples
{
    public partial class SampleProgram
    {
        [STAThread] // Required to support the interactive login experience
        static void Main(string[] args)
        {
            CrmServiceClient service = null;
            try
            {
                service = SampleHelpers.Connect("Connect");
                if (service.IsReady)
                {
                    // Create any entity records that the demonstration code requires
                    SetUpSample(service);
                    #region Demonstrate

                    // Define some anonymous types to define the range of possible 
                    // queue property values.
                    var IncomingEmailDeliveryMethods = new
                    {
                        None = 0,
                        EmailRouter = 2,
                        ForwardMailbox = 3
                    };

                    var IncomingEmailFilteringMethods = new
                    {
                        AllEmailMessages = 0,
                        EmailMessagesInResponseToCrmEmail = 1,
                        EmailMessagesFromCrmLeadsContactsAndAccounts = 2
                    };

                    var OutgoingEmailDeliveryMethods = new
                    {
                        None = 0,
                        EmailRouter = 2
                    };

                    var QueueViewType = new
                    {
                        Public = 0,
                        Private = 1
                    };
                    // Create a queue instance and set its property values.
                    var newQueue = new Queue()
                    {
                        Name = "Example Queue.",
                        Description = "This is an example queue.",
                        IncomingEmailDeliveryMethod = new OptionSetValue(
                            IncomingEmailDeliveryMethods.None),
                        IncomingEmailFilteringMethod = new OptionSetValue(
                            IncomingEmailFilteringMethods.AllEmailMessages),
                        OutgoingEmailDeliveryMethod = new OptionSetValue(
                            OutgoingEmailDeliveryMethods.None),
                        QueueViewType = new OptionSetValue(
                            QueueViewType.Private)
                    };

                    // Create a new queue instance.
                    _queueId = service.Create(newQueue);
                    Console.WriteLine("Created {0}", newQueue.Name);

                    #endregion Demonstrate

                    #region Clean up
                    CleanUpSample(service);
                    #endregion Clean up
                }
                else
                {
                    const string UNABLE_TO_LOGIN_ERROR = "Unable to Login to Common Data Service";
                    if (service.LastCrmError.Equals(UNABLE_TO_LOGIN_ERROR))
                    {
                        Console.WriteLine("Check the connection string values in cds/App.config.");
                        throw new Exception(service.LastCrmError);
                    }
                    else
                    {
                        throw service.LastCrmException;
                    }
                }
            }
            catch (Exception ex)
            {
                SampleHelpers.HandleException(ex);
            }

            finally
            {
                if (service != null)
                    service.Dispose();

                Console.WriteLine("Press <Enter> to exit.");
                Console.ReadLine();
            }

        }
    }
}
