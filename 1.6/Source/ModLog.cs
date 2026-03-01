using rjw;
using Verse;

namespace SlimeGirl
{
    public static class ModLog
    {
        /// <summary>
		/// Logs the given message with [DataStorage.ModId] appended.
		/// </summary>
		public static void Error(string message)
        {
            Log.Error($"[{DataStorage.ModId}] {message}");
        }

        /// <summary>
        /// Logs the given message with [DataStorage.ModId] appended.
        /// </summary>
        public static void Message(string message)
        {
            Log.Message($"[{DataStorage.ModId}] {message}");
        }

        /// <summary>
        /// Logs the given message with [DataStorage.ModId] appended.
        /// </summary>
        public static void Warning(string message)
        {
            Log.Warning($"[{DataStorage.ModId}] {message}");
        }
    }
}
