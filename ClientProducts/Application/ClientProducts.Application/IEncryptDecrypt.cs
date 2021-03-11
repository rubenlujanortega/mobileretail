using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientProducts.Application
{
    public interface IEncryptDecrypt
    {
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);
    }
}
