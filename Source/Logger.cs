using System;
using System.Text;
using UnityEngine;

namespace DistrictStyleManager
{
    internal static class Logger
    {
        private const string ModName = nameof(DistrictStyleManager);

        internal static void Error(params string[] messages)
        {
            Write("ERROR: ", messages);
        }

        internal static void Exception(Exception exception, params string[] messages)
        {
            var message = new StringBuilder(ModName);
            message.Append(": ");
            foreach (var logMessage in messages) { message.Append(logMessage); }
            message.AppendLine();
            message.AppendLine("Exception: ");
            message.AppendLine(exception.Message);
            message.AppendLine(exception.Source);
            message.AppendLine(exception.StackTrace);
            if (exception.InnerException != null)
            {
                message.AppendLine("Inner exception:");
                message.AppendLine(exception.InnerException.Message);
                message.AppendLine(exception.InnerException.Source);
                message.AppendLine(exception.InnerException.StackTrace);
            }
            Debug.Log(message);
        }

        internal static void Info(params string[] messages)
        {
            Write("INFO: ", messages);
        }

        private static void Write(string prefix, params string[] messages)
        {
            var message = new StringBuilder(ModName);
            message.Append(": ");
            message.Append(prefix);
            foreach (var logMessage in messages) { message.Append(logMessage); }
            message.Append(".");
            Debug.Log(message);
        }
    }
}