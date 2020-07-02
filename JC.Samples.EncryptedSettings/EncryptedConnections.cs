using JC.Samples.EncryptedSettings.Keys;
using System.Configuration;
using System.IO;

namespace JC.Samples.EncryptedSettings
{
    /// <summary>
    /// Provides methods for encrypting/decrypting Connection Strings.
    /// </summary>
    public class EncryptedConnections
    {
        #region Methods

        /// <summary>
        /// Decrypts the specified Connection String.
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt</param>
        /// <returns>The decrypted Connection String</returns>
        public static string DecryptConnectionString(string encryptedText)
        {
            // There's no need to decrypt empty values.
            if (string.IsNullOrEmpty(encryptedText)) return encryptedText;

            // Only attempt decryption if the text is currently encrypted.
            if (!EncryptionProvider.IsEncrypted(encryptedText)) return encryptedText;

            // Decrypt the Connection String and return it.
            string text = encryptedText.Substring(EncryptionConstants.EncryptedValuePrefix.Length, 
                                                  encryptedText.Length - EncryptionConstants.EncryptedValuePrefix.Length);

            return EncryptionProvider.Decrypt(text, Connections.Entropy);
        }

        /// <summary>
        /// Encrypts all Connection Strings contained within the specified configuration file.
        /// </summary>
        /// <param name="config">The Configuration object which represents the file containing the Connection Strings which are to be encrypted</param>
        public static void EncryptConfigurationConnectionStrings(Configuration config = null)
        {
            // Default to the main application App.config file, if a specific configuration file has not been specified explicitly.
            if (config == null) config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // Cycle each Connection String.
            foreach (ConnectionStringSettings connectionString in config.ConnectionStrings.ConnectionStrings)
            {
                // Ignore 'Machine.config' Connection Strings.
                if (connectionString.ElementInformation.Source != null)
                {
                    if (connectionString.ConnectionString?.Length > 0 && !EncryptionProvider.IsEncrypted(connectionString.ConnectionString))
                    {
                        // Encrypt the Connection String.
                        connectionString.ConnectionString = EncryptionConstants.EncryptedValuePrefix + 
                                                            EncryptionProvider.Encrypt(connectionString.ConnectionString, Connections.Entropy);
                    }
                }
            }

            // Save changes to the configuration file.
            config.Save(ConfigurationSaveMode.Modified);

            // Refresh the Connection Strings section within the configuration 
            // file to update the configuration details in-memory.
            ConfigurationManager.RefreshSection(EncryptionConstants.ConnectionStringsSectionName);
        }

        /// <summary>
        /// Gets a decrypted Connection String for the specified configuration file path and Connection Name.
        /// </summary>
        /// <param name="connectionStringConfigPath">File path to the connection configuration file</param>
        /// <param name="connectionStringName">The name of the connection string</param>
        /// <returns>The decrypted Connection String</returns>
        public static string GetConnectionString(string connectionStringConfigPath, string connectionStringName)
        {
            if (File.Exists(connectionStringConfigPath))
            {
                // Open the connection configuration file.
                var configMap = new ExeConfigurationFileMap { ExeConfigFilename = connectionStringConfigPath };

                Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

                // Decrypt and return the Connection String.
                string encryptedConnectionString = configuration.ConnectionStrings.ConnectionStrings[connectionStringName].ConnectionString;
                string decryptedConnectionString = DecryptConnectionString(encryptedConnectionString);
                return decryptedConnectionString;
            }

            return null;
        }

        #endregion
    }
}