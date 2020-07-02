using System;
using System.Configuration;
using System.Linq;

namespace JC.Samples.EncryptedSettings
{
    /// <summary>
	/// Main Program class.
	/// </summary>
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The command-line arguments passed to the program</param>
        static void Main(string[] args)
        {
            // Get the value of the unencrypted setting.
            var value = ConfigurationManager.AppSettings.Get("MyPlainSetting");

            Console.WriteLine($"MyPlainSetting: {value}");

            // Get the value of the encrypted setting.
            // The setting value will be automatically encrypted within the config file when first accessed.
            // i.e. at this location: <project_path>\bin\Debug\netcoreapp3.1\JC.Samples.EncryptedSettings.dll.config
            var decryptedValue = EncryptedSettings.Get("MyEncryptedSetting");

            Console.WriteLine($"MyEncryptedSetting (Before Set): {decryptedValue}");

            // Set a new encrypted value.
            EncryptedSettings.Set("MyEncryptedSetting", "This is the new encrypted text.");

            Console.WriteLine($"MyEncryptedSetting (After Set): {decryptedValue}");

            // Inform the user that the program has completed.
            Console.WriteLine();
            Console.WriteLine("Press any key to exit");

            Console.ReadKey();
        }
    }
}