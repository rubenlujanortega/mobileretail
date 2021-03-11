using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientProducts.Message
{
    public class GetOTPValidationRequest
    {
        public GetOTPValidationRequest() { }
        public string UserId { get; set; }
        public string PIN { get; set; }
    }
}
