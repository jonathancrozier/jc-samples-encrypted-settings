using System;
using System.Security.Cryptography;
using System.Text;

namespace JC.Samples.EncryptedSettings
{
    /// <summary>
    /// Provides encryption services.
    /// </summary>
    internal class EncryptionProvider
    {
        #region Methods

        /// <summary>
        /// Decrypts the specified clear text.
        /// </summary>
        /// <param name="encryptedText">The encrypted text to decrypt</param>
        /// <param name="entropy">Optional entropy key</param>
        /// <returns>The decrypted text</returns>
        public static string Decrypt(string encryptedText, byte[] entropy)
        {
            if (encryptedText == null) throw new ArgumentNullException(nameof(encryptedText));
            
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] clearBytes     = ProtectedData.Unprotect(encryptedBytes, entropy, DataProtectionScope.LocalMachine);

            return Encoding.UTF8.GetString(clearBytes);
        }

        /// <summary>
        /// Encrypts the specified clear text.
        /// </summary>
        /// <param name="clearText">The clear text to encrypt</param>
        /// <param name="entropy">Optional entropy key</param>
        /// <returns>The encrypted text</returns>
        public static string Encrypt(string clearText, byte[] entropy)
        {
            if (clearText == null) throw new ArgumentNullException(nameof(clearText));

            byte[] clearBytes     = Encoding.UTF8.GetBytes(clearText);
            byte[] encryptedBytes = ProtectedData.Protect(clearBytes, entropy, DataProtectionScope.LocalMachine);

            return Convert.ToBase64String(encryptedBytes);
        }

        /// <summary>
        /// Determines if a given string of text is encrypted.
        /// </summary>
        /// <param name="text">The text to check</param>
        /// <returns>True if the text is encrypted, otherwise false</returns>
        public static bool IsEncrypted(string text) => text.StartsWith(EncryptionConstants.EncryptedValuePrefix, StringComparison.OrdinalIgnoreCase);

        #endregion
    }
}