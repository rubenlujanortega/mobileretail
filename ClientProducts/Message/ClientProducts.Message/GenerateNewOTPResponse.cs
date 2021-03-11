using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain;
using ClientProducts.Domain.Contributions;

namespace ClientProducts.Message
{
    public class GenerateNewOTPResponse
    {
        public GenerateNewOTPResponse()
        {
            Result = new OperationResult();
        }

        public OperationResult Result { get; set; }
        public ClientOTP OTPData { get; set; }
    }
}
