using JC.Samples.EncryptedSettings.Keys;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace JC.Samples.EncryptedSettings
{
    /// <summary>
    /// Provides methods for encrypting/decrypting encrypted configuration file settings.
    /// 
    /// Add the following '<section>' element within the '<configSections>' element of the App.config file.
    /// <![CDATA[
    /// <section name="encryptedSettings" type="System.Configuration.AppSettingsSection" />
    /// ]]>
    /// 
    /// Add the following XML within the '<configuration>' element to hold the encrypted settings.
    /// <![CDATA[
    /// <encryptedSettings>
    ///   <add key="MyEncryptedSetting" value="secret"/>
    /// </encryptedSettings>
    /// ]]>
    /// </summary>
    public class EncryptedSettings
    {
        #region Methods

        #region Public

        /// <summary>
        /// Gets the decrypted value for the specified encrypted setting key.
        /// Automatically encrypts the setting if it is not already encrypted.
        /// </summary>
        /// <param name="key">The key/name of the encrypted setting</param>
        /// <returns>The decrypted value as a string</returns>
        public static string Get(string key)
        {
            string value = GetEncryptedSettings()[key];

            // There's no need to decrypt/encrypt empty values.
            if (string.IsNullOrEmpty(value)) return value;

            if (EncryptionProvider.IsEncrypted(value))
            {
                // Get the encrypted data from the value (i.e. strip out the 'CipherValue:' prefix).
                value = value.Substring(EncryptionConstants.EncryptedValuePrefix.Length, value.Length - EncryptionConstants.EncryptedValuePrefix.Length);

                // Decrypt the data.
                return EncryptionProvider.Decrypt(value, Settings.Entropy);
            }
            else
            {
                // If the setting is not already encrypted, encrypt it before returning the decrypted data.
                EncryptSetting(key, value);

                return value;
            }
        }

        /// <summary>
        /// Sets the encrypted value for the specified encrypted setting key.
        /// </summary>
        /// <param name="key">The key/name of the encrypted setting</param>
        /// <param name="value">The value to set for the encrypted setting</param>
        public static void Set(string key, object value) => EncryptSetting(key, value);

        #endregion

        #region Private

        /// <summary>
        /// Encrypts the value for the specified key.
        /// </summary>
        /// <param name="key">The key/name of the encrypted setting</param>
        /// <param name="value">The value to set for the encrypted setting</param>
        private static void EncryptSetting(string key, object value)
        {
            // Open the configuration file and set the encrypted value for the specified setting key.
            Configuration configuration          = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection encryptedSection  = configuration.GetSection(EncryptionConstants.EncryptedSettingsSectionName) as AppSettingsSection;
            encryptedSection.Settings[key].Value = EncryptionConstants.EncryptedValuePrefix + EncryptionProvider.Encrypt(Convert.ToString(value), Settings.Entropy);

            // Save changes to the configuration file.
            configuration.Save(ConfigurationSaveMode.Modified);

            // Refresh the Encrypted Settings section within the configuration 
            // file to update the configuration details in-memory.
            ConfigurationManager.RefreshSection(EncryptionConstants.EncryptedSettingsSectionName);
        }

        /// <summary>
        /// Gets a name/value collection of encrypted settings.
        /// </summary>
        /// <returns><see cref="NameValueCollection"/></returns>
        private static NameValueCollection GetEncryptedSettings() => ConfigurationManager.GetSection(EncryptionConstants.EncryptedSettingsSectionName) as NameValueCollection;

        #endregion

        #endregion
    }
}