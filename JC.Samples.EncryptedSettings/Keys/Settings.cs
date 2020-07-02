namespace JC.Samples.EncryptedSettings.Keys
{
    /// <summary>
    /// Settings keys.
    /// </summary>
    public class Settings
    {
        #region Readonlys

        /// <summary>
        /// Demo key.
        /// </summary>
        public static readonly byte[] Entropy = new byte[32] // 32 bytes = 256-bit.
        {
            56, 67, 49, 69, 51, 54, 56, 69, 69, 53, 65, 57, 67, 69, 69, 54, 56, 49, 68, 65, 53, 69, 57, 65, 54, 56, 51, 53, 69, 75, 50, 49
        };

        #endregion
    }
}