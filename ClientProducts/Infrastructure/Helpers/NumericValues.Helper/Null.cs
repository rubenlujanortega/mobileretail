using System;

namespace NumericValues.Helper
{
    public static class Null
    {
        public static SByte NullByte
        {
            get { return -1; }
        }

        public static int NullInteger
        {
            get { return -1; }
        }

        public static Single NullSingle
        {
            get { return Single.MinValue; }
        }

        public static Double NullDouble
        {
            get { return Double.MinValue; }
        }

        public static Decimal NullDecimal
        {
            get { return 0M ; }
        }

        public static DateTime NullDate
        {
            get { return DateTime.MinValue; }
        }

        public static String NullString
        {
            get { return ""; }
        }

        public static Boolean NullBoolean
        {
            get { return false; }
        }

        public static Guid NullGuid
        {
            get { return Guid.Empty; }
        }

        public static Object SetNull(Object objValue, Object objField)
        {
            if (objValue == DBNull.Value)
            {
                if (objField.GetType() == typeof(Byte))
                {
                    return NullByte;
                }
                else if (objField.GetType() == typeof(int))
                {
                    return NullInteger;
                }
                else if (objField.GetType() == typeof(Single))
                {
                    return NullSingle;
                }
                else if (objField.GetType() == typeof(Double))
                {
                    return NullDouble;
                }
                else if (objField.GetType() == typeof(Decimal))
                    return NullDecimal;
                else if (objField.GetType() == typeof(DateTime))
                {
                    return NullDate;
                }
                else if (objField.GetType() == typeof(String))
                {
                    return NullString;
                }
                else if (objField.GetType() == typeof(Boolean))
                {
                    return NullBoolean;
                }
                else if (objField.GetType() == typeof(Guid))
                {
                    return NullGuid;
                }
                else // complex object
                    return null;
            }
            else    // return value
                return objValue;
        }
    }
}
