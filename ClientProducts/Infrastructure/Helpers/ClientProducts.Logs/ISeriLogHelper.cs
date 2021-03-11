using System;

namespace ClientProducts.Logs
{
    public interface ISeriLogHelper
    {
        void Trace(string messageTemplate);
        void Trace(string messageTemplate, params object[] propertyValues);
        void Debug(string messageTemplate);
        void Debug(string messageTemplate, params object[] propertyValues);
        void Info(string messageTemplate);
        void Info(string messageTemplate, params object[] propertyValues);
        void Warn(string messageTemplate);
        void Warn(string messageTemplate, params object[] propertyValues);
        void Error(Exception exception, string messageTemplate);
        void Error(Exception exception, string messageTemplate, params object[] propertyValues);
        void Fatal(Exception exception, string messageTemplate);
        void Fatal(Exception exception, string messageTemplate, params object[] propertyValues);
    }
}
