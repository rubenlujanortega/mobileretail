using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientProducts.Domain._Ddd;

namespace ClientProducts.Domain.Contributions
{
    public class ClientOTP : Entity<string>
    {
        private readonly string _newOTP;
        private readonly string _email;
        private readonly string _phone;

        private ClientOTP(string userId, string newOTP, string email, string phone)
        {
            Id = userId;
            _newOTP = newOTP;
            _email = email;
            _phone = phone;
        }

        public string NewOTP => _newOTP;
        public string Email => _email;
        public string Phone => _phone;

        public static ClientOTP Create(string userId, string newOTP, string email, string phone)
        {
            if (string.IsNullOrEmpty(userId)) { throw new ArgumentException("userId no puede ser nulo o vacío."); }
            if (newOTP == null) { throw new ArgumentException("newOTP no puede ser nulo."); }

            return new ClientOTP(userId, newOTP, email, phone);
        }


    }
}
