using System;

namespace Hydra.Payment.CrossCutting.Models
{
    public class GatewayConfigurationKeys
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}