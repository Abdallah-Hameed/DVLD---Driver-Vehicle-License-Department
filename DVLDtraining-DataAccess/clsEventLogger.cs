using System;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public static class EventLogger
    {
        private const string SOURCE_NAME = "DVLD_System";

        private const string LOG_NAME = "Application";

        private static bool isInitialized = false;

        private static readonly object lockObj = new object();

        static EventLogger()
        {
            Initialize();
        }

        private static void Initialize()
        {
            if (!isInitialized)
            {
                lock (lockObj)
                {
                    if (!isInitialized)
                    {
                        try
                        {
                            if (!EventLog.SourceExists(SOURCE_NAME, "."))
                            {
                                EventLog.CreateEventSource(SOURCE_NAME, LOG_NAME);
                            }

                            isInitialized = true;
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine($"Warning: Could not initialize EventLog: {ex.Message}");
                        }
                    }
                }
            }
        }

        public static void LogSqlError(string methodName, int recordId, Exception ex)
        {
            try
            {
                string message = $"SQL Error in {methodName}\n" + $"Record ID: {recordId}\n" + $"Error: {ex.Message}";

                EventLog.WriteEntry(SOURCE_NAME, message, EventLogEntryType.Error, 2001);
            }

            catch { }
        }

        public static void LogError(string methodName, string message, Exception ex = null)
        {
            try
            {
                string fullMessage = $"Error in {methodName}\n{message}";

                if (ex != null)
                    fullMessage += $"\nException: {ex.Message}";

                EventLog.WriteEntry(SOURCE_NAME, fullMessage, EventLogEntryType.Error, 2000);
            }

            catch { }
        }

        public static void LogWarning(string methodName, string message)
        {
            try
            {
                EventLog.WriteEntry(SOURCE_NAME, $"{methodName}: {message}", EventLogEntryType.Warning, 3001);
            }

            catch { }
        }

        public static void LogInformation(string methodName, string message)
        {
            try
            {
                EventLog.WriteEntry(SOURCE_NAME, $"{methodName}: {message}", EventLogEntryType.Information, 1001);
            }
            catch { }
        }

        public static void LogDataOperation(string operation, string tableName, int recordId)
        {
            try
            {
                string message = $"Data Operation: {operation}\n" + $"Table: {tableName}\n" + $"Record ID: {recordId}";

                EventLog.WriteEntry(SOURCE_NAME, message, EventLogEntryType.Information, 1002);
            }
            catch { }
        }
    }
}