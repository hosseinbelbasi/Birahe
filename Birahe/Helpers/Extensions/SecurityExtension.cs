using System.Security.Cryptography;
using System.Text;

public static class StringExtensions {
    public static string Hash(this string input) {
        using var sha512 = SHA512.Create(); // Create SHA-512 instance
        var bytes = Encoding.UTF8.GetBytes(input); // Convert string to bytes
        var hashBytes = sha512.ComputeHash(bytes); // Compute hash
        return BitConverter.ToString(hashBytes) // Convert bytes to hex string
            .Replace("-", "") // Remove dashes
            .ToLower(); // Lowercase for consistency
    }
}