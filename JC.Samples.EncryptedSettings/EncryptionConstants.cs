namespace JC.Samples.EncryptedSettings
{
    /// <summary>
    /// Encryption constants.
    /// </summary>
    public class EncryptionConstants
    {
        #region Constants

        /// <summary>
        /// The name of the Connection Strings section within the App.config file.
        /// </summary>
        public const string ConnectionStringsSectionName = "connectionStrings";

        /// <summary>
        /// The name of the Encrypted Settings section within the App.config file.
        /// </summary>
        public const string EncryptedSettingsSectionName = "encryptedSettings";

        /// <summary>
        /// The prefix text to add to the start of encrypted data.
        /// </summary>
        public const string EncryptedValuePrefix = "CipherValue:";

        #endregion
    }
}