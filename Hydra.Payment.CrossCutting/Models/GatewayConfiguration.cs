using System;
using System.Collections.Generic;

namespace Hydra.Payment.CrossCutting.Models
{
    /// <summary>
    /// This model read from a Gateway Configuration Table
    /// </summary>
    public class GatewayConfiguration
    {
        public Guid Id { get; set; }

        public string GatewayName { get; set; }
        public IEnumerable<GatewayConfigurationKeys> Keys {get; set; }
    }
}