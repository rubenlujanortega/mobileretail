using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain.ClientSolvenciaDataAggregate;
using Common.Domain;

namespace ClientProducts.Repository
{
    public interface ISecurityGlobalAppDa
    {
        SolvenciaData GetClientDatosSolvencia(string clientIdentifier, int portalId);

        string GetActivePIN(string userName, int portalId, int minutes);
        object GetParameterValue(string param, int portalId);
        void SetPinUser(string pin, string userName, int portalId, int minutes);
    }
}
