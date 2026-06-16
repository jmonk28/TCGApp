namespace TCGApp.Server.Utilities;
using System;
using System.Security.Cryptography;
using System.Text;

public class SHA256Hash
{

    public SHA256Hash() { }

    public string ComputeSHA256Hash(string input)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("Input cannot be null or empty.", nameof(input));

        // Create a SHA256 instance
        using (SHA256 sha256 = SHA256.Create())
        {
            // Convert the input string to bytes
            byte[] bytes = Encoding.UTF8.GetBytes(input);

            // Compute the hash
            byte[] hashBytes = sha256.ComputeHash(bytes);

            // Convert hash bytes to a hex string
            StringBuilder builder = new StringBuilder();
            foreach (byte b in hashBytes)
                builder.Append(b.ToString("x2")); // "x2" for lowercase hex

            return builder.ToString();
        }
    }

}