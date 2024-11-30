using System;
using System.IO;
using Newtonsoft.Json;

namespace TaskSchedulerApp.Utilities
{
    /// <summary>
    /// Singleton class for managing configuration settings from a JSON file.
    /// </summary>
    public class Configuration
    {
        private static Configuration? _instance;
        private readonly dynamic? _settings;

        /// <summary>
        /// Private constructor to load settings from the JSON configuration file.
        /// </summary>
        /// <param name="configFilePath">Path to the configuration file.</param>
        private Configuration(string configFilePath = "appsettings.json")
        {
            try
            {
                // Get the current working directory
                string currentDirectory = Directory.GetCurrentDirectory();

                // Combine directory path with the config file name
                string fullPath = Path.Combine(currentDirectory, configFilePath);

                // Read and deserialize the JSON configuration
                var json = File.ReadAllText(fullPath);
                _settings = JsonConvert.DeserializeObject<dynamic>(json);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error loading configuration: {ex.Message}");
                _settings = null;
            }
        }

        /// <summary>
        /// Gets the singleton instance of the Configuration class.
        /// </summary>
        public static Configuration Instance => _instance ??= new Configuration();

        /// <summary>
        /// Retrieves a path setting from the JSON configuration.
        /// </summary>
        /// <param name="key">The key of the path setting (e.g., "ActiveFolder").</param>
        /// <returns>The path value as a string.</returns>
        public string GetPath(string key) => _settings?.Paths?[key]?.ToString() ?? string.Empty;

        /// <summary>
        /// Retrieves a timer setting in minutes from the JSON configuration.
        /// </summary>
        /// <param name="key">The key of the timer setting (e.g., "BackupIntervalInMinutes").</param>
        /// <returns>The timer value as an integer.</returns>
        public int GetTimer(string key) => (int)(_settings?.Timers?[key] ?? 0);
    }
}
