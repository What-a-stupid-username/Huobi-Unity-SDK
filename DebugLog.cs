using System;

#if MCS

namespace UnityEngine
{
    static class Debug
    {
        public static void Log(uint value)
        {
            Console.WriteLine(value);
        }
        public static void Log(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }
        public static void Log()
        {
            Console.WriteLine();
        }
        public static void Log(bool value)
        {
            Console.WriteLine(value);
        }
        public static void Log(char[] buffer)
        {
            Console.WriteLine(buffer);
        }
        public static void Log(char[] buffer, int index, int count)
        {
            Console.WriteLine(buffer, index, count);
        }
        public static void Log(decimal value)
        {
            Console.WriteLine(value);
        }
        public static void Log(double value)
        {
            Console.WriteLine(value);
        }
        public static void Log(int value)
        {
            Console.WriteLine(value);
        }
        public static void Log(long value)
        {
            Console.WriteLine(value);
        }
        public static void Log(object value)
        {
            Console.WriteLine(value);
        }
        public static void Log(float value)
        {
            Console.WriteLine(value);
        }
        public static void Log(string value)
        {
            Console.WriteLine(value);
        }
        public static void Log(string format, object arg0)
        {
            Console.WriteLine(format, arg0);
        }
        public static void Log(string format, object arg0, object arg1)
        {
            Console.WriteLine(format, arg0, arg1);
        }
        public static void Log(string format, object arg0, object arg1, object arg2)
        {
            Console.WriteLine(format, arg0, arg1, arg2);
        }

        public static void Log(string format, object arg0, object arg1, object arg2, object arg3)
        {
            Console.WriteLine(format, arg0, arg1, arg2, arg3);
        }

        public static void Log(ulong value)
        {
            Console.WriteLine(value);
        }
        public static void Log(char value)
        {
            Console.WriteLine(value);
        }


        public static void LogWarning(uint value)
        {
            Console.WriteLine(value);
        }
        public static void LogWarning(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }
        public static void LogWarning()
        {
            Console.WriteLine();
        }
        public static void LogWarning(bool value)
        {
            Console.WriteLine(value);
        }
        public static void LogWarning(char[] buffer)
        {
            Console.WriteLine(buffer);
        }
        public static void LogWarning(char[] buffer, int index, int count)
        {
            Console.WriteLine(buffer, index, count);
        }
        public static void LogWarning(decimal value)
        {
            Console.WriteLine(value);
        }
        public static void LogWarning(double value)
        {
            Console.WriteLine(value);
        }
        public static void LogWarning(int value)
        {
            Console.WriteLine(value);
        }
        public static void LogWarning(long value)
        {
            Console.WriteLine(value);
        }
        public static void LogWarning(object value)
        {
            Console.WriteLine(value);
        }
        public static void LogWarning(float value)
        {
            Console.WriteLine(value);
        }
        public static void LogWarning(string value)
        {
            Console.WriteLine(value);
        }
        public static void LogWarning(string format, object arg0)
        {
            Console.WriteLine(format, arg0);
        }
        public static void LogWarning(string format, object arg0, object arg1)
        {
            Console.WriteLine(format, arg0, arg1);
        }
        public static void LogWarning(string format, object arg0, object arg1, object arg2)
        {
            Console.WriteLine(format, arg0, arg1, arg2);
        }

        public static void LogWarning(string format, object arg0, object arg1, object arg2, object arg3)
        {
            Console.WriteLine(format, arg0, arg1, arg2, arg3);
        }

        public static void LogWarning(ulong value)
        {
            Console.WriteLine(value);
        }
        public static void LogWarning(char value)
        {
            Console.WriteLine(value);
        }



        public static void LogError(uint value)
        {
            Console.WriteLine(value);
        }
        public static void LogError(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }
        public static void LogError()
        {
            Console.WriteLine();
        }
        public static void LogError(bool value)
        {
            Console.WriteLine(value);
        }
        public static void LogError(char[] buffer)
        {
            Console.WriteLine(buffer);
        }
        public static void LogError(char[] buffer, int index, int count)
        {
            Console.WriteLine(buffer, index, count);
        }
        public static void LogError(decimal value)
        {
            Console.WriteLine(value);
        }
        public static void LogError(double value)
        {
            Console.WriteLine(value);
        }
        public static void LogError(int value)
        {
            Console.WriteLine(value);
        }
        public static void LogError(long value)
        {
            Console.WriteLine(value);
        }
        public static void LogError(object value)
        {
            Console.WriteLine(value);
        }
        public static void LogError(float value)
        {
            Console.WriteLine(value);
        }
        public static void LogError(string value)
        {
            Console.WriteLine(value);
        }
        public static void LogError(string format, object arg0)
        {
            Console.WriteLine(format, arg0);
        }
        public static void LogError(string format, object arg0, object arg1)
        {
            Console.WriteLine(format, arg0, arg1);
        }
        public static void LogError(string format, object arg0, object arg1, object arg2)
        {
            Console.WriteLine(format, arg0, arg1, arg2);
        }

        public static void LogError(string format, object arg0, object arg1, object arg2, object arg3)
        {
            Console.WriteLine(format, arg0, arg1, arg2, arg3);
        }

        public static void LogError(ulong value)
        {
            Console.WriteLine(value);
        }
        public static void LogError(char value)
        {
            Console.WriteLine(value);
        }
    }
}

#endif