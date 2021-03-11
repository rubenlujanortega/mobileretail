using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.ClientSolvenciaDataAggregate
{
    public class SolvenciaData : Entity<string>
    {
        private readonly string _email = string.Empty;
        private readonly string _cellPhone = string.Empty;
        private readonly string _fullName = string.Empty;

        private SolvenciaData(string user, string email, string fullName, string cellPhone)
        {
            Id = user;
            _email = email;
            _cellPhone = cellPhone;
            _fullName = fullName;
        }

        public string User => Id;
        public string Email => _email;
        public string CellPhone => _cellPhone;
        public string FullName => _fullName;

        public static SolvenciaData Create(string user, string email, string fullName, string cellPhone)
        {
            if (string.IsNullOrEmpty(user)) { throw new ArgumentException("user no puede ser nulo ni vacío"); }
            if (string.IsNullOrEmpty(fullName)) { throw new ArgumentException("fullName no puede ser nulo ni vacío"); }

            return new SolvenciaData(user, email, fullName, cellPhone);
        }
    }
}
