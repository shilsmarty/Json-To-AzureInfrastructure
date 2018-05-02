using System.Collections.Generic;

namespace AzureInfraProvisioning.Model
{
    public class ProvisioningRequest
    {
        public Provisioning Provisioning { get; set; }

    }
        public class Resource
        {
            public string Name { get; set; }
            public int Id { get; set; }
            public string Location { get; set; }
            public Properties Properties { get; set; }
        }

        public class Provisioning
        {
            public int request_id { get; set; }
            public string subscription_id { get; set; }
            public string resourcegroup { get; set; }
            public string description { get; set; }
            public string created_at { get; set; }
            public string updated_at { get; set; }
            public string settings { get; set; }
            public string Defaultlocation { get; set; }
            public List<Resource> Resources { get; set; }
        }

        public class Properties
        {
        }
    }
