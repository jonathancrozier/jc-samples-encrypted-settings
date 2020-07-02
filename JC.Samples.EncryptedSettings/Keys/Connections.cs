namespace JC.Samples.EncryptedSettings.Keys
{
    /// <summary>
    /// Settings keys.
    /// </summary>
    public class Connections
    {
        #region Readonlys

        /// <summary>
        /// Demo key.
        /// </summary>
        public static readonly byte[] Entropy = new byte[32] // 32 bytes = 256-bit.
        {
            54, 52, 69, 69, 55, 54, 55, 49, 53, 67, 65, 55, 54, 65, 50, 50, 53, 51, 52, 50, 49, 49, 65, 57, 50, 66, 57, 51, 51, 70, 55, 50
        };

        #endregion
    }
}