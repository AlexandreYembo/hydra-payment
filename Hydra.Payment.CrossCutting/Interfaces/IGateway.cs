using Hydra.Payment.CrossCutting.Models;

namespace Hydra.Payment.CrossCutting.Interfaces
{
    public interface IGateway
    {
         void BuildGateway(GatewayConfiguration configuration);
    }
}