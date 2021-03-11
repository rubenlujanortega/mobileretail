using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClientProducts.Application;

namespace ClientProducts.ApplicationTest
{
    [TestClass]
    public class EncryptDecryptTest
    {
        private EncryptDecrypt _encryptDecrypt;

        public EncryptDecryptTest()
        {
            _encryptDecrypt = new EncryptDecrypt("P@SsW0rD", "sR|mItDT|KTNiUwiLlfBk9=Vn5cE2H91Q34ri%u=iKMHs~f60Uld*rd9Lugk", "Ajbd7sozZcwz/FR0+jMUNw==");
        }

        [TestMethod, TestCategory("ParamError")]
        [ExpectedException(typeof(ArgumentException))]
        public void DecryptParamError()
        {
            _encryptDecrypt.Decrypt("");
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void DecryptSuccess()
        {
            var data = _encryptDecrypt.Decrypt("SmuwVXFibs/JC9wg2LYQGw==");
            Assert.IsTrue(data.Length > 0);
        }

        [TestMethod, TestCategory("ParamError")]
        [ExpectedException(typeof(ArgumentException))]
        public void EncryptParamError()
        {
            _encryptDecrypt.Encrypt("");
        }

        [TestMethod, TestCategory("ProcessSuccessful")]
        public void EncryptSuccess()
        {
            var data = _encryptDecrypt.Encrypt("123456");
            Assert.IsTrue(data.Length > 0);
        }
    }
}
