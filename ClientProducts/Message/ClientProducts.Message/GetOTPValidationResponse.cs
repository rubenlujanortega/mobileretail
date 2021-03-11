using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Domain;

namespace ClientProducts.Message
{
    public class GetOTPValidationResponse
    {
        public GetOTPValidationResponse()
        {
            Result = new OperationResult();
        }

        public OperationResult Result { get; set; }
        public bool PinValid { get; set; }
        public bool PinExpired { get; set; }
    }
}
