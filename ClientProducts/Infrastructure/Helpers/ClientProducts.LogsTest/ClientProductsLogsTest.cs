using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Moq;
using Serilog;
using ClientProducts.Logs;

namespace ClientProducts.LogsTest
{
    [TestClass]
    public class ClientProductsLogsTest
    {
        private readonly ISeriLogHelper _seriLogHelper;

        public ClientProductsLogsTest()
        {
            _seriLogHelper = new SeriLogHelper();
        }

        [TestMethod]
        public void TraceLog()
        {
            _seriLogHelper.Trace("Test trace log");
        }


        [TestMethod]
        public void TraceLogWithParameters()
        {
            _seriLogHelper.Trace("Test trace log with parameters {Objeto1}, {Objeto2}", new object[] { "Objeto1", "Objeto3" });
        }

        [TestMethod]
        public void DebugLog()
        {
            _seriLogHelper.Debug("Test debug log");
        }


        [TestMethod]
        public void DebugLogWithParameters()
        {
            _seriLogHelper.Debug("Test debug log with parameters {Objeto1}, {Objeto2}", new object[] { "Objeto1", "Objeto3" });
        }

        [TestMethod]
        public void InfoLog()
        {
            _seriLogHelper.Info("Test information log");
        }


        [TestMethod]
        public void InfoLogWithParameters()
        {
            _seriLogHelper.Info("Test information log with parameters {Objeto1}, {Objeto2}", new object[] { "Objeto1", "Objeto3" });
        }

        [TestMethod]
        public void WarnLog()
        {
            _seriLogHelper.Warn("Test warning log");
        }


        [TestMethod]
        public void WarnLogWithParameters()
        {
            _seriLogHelper.Warn("Test warning log with parameters {Objeto1}, {Objeto2}", new object[] { "Objeto1", "Objeto3" });
        }


        [TestMethod]
        public void ErrorLog()
        {
            _seriLogHelper.Error(new Exception("Application error"), "Test error log");
        }


        [TestMethod]
        public void ErrorLogWithParameters()
        {
            _seriLogHelper.Error(new Exception("Application error"), "Test error log {Objeto1}, {Objeto2}", new object[] { "Objeto1", "Objeto3" });
        }

        [TestMethod]
        public void FatalLog()
        {
            _seriLogHelper.Fatal(new Exception("Application fatal"), "Test fatal log");
        }


        [TestMethod]
        public void FatalLogWithParameters()
        {
            _seriLogHelper.Fatal(new Exception("Application fatal"), "Test fatal log", new object[] { "Objeto1", "Objeto3" });
        }

    }
}
