
Public Class Crypto


    'Namespace PasswordHash
    '{
    '    /// <summary>
    '    /// Salted password hashing with PBKDF2-SHA1.
    '    /// Author: havoc AT defuse.ca
    '    /// www: http://crackstation.net/hashing-security.htm
    '    /// Compatibility: .NET 3.0 And later.
    '    /// </summary>
    '    Public Class PasswordHash
    '    {
    '        // The following constants may be changed without breaking existing hashes.
    '        Public Const int SALT_BYTE_SIZE = 24;
    '        Public Const int HASH_BYTE_SIZE = 24;
    '        Public Const int PBKDF2_ITERATIONS = 1000;

    '        Public Const int ITERATION_INDEX = 0;
    '        Public Const int SALT_INDEX = 1;
    '        Public Const int PBKDF2_INDEX = 2;

    '        /// <summary>
    '        /// Creates a salted PBKDF2 hash of the password.
    '        /// </summary>
    '        /// <param name="password">The password to hash.</param>
    '        /// <returns>The hash of the password.</returns>
    '        Public Static String CreateHash(String password)
    '        {
    '            // Generate a random salt
    '            RNGCryptoServiceProvider csprng = New RNGCryptoServiceProvider();
    '            Byte[] salt = New Byte[SALT_BYTE_SIZE];
    '            csprng.GetBytes(salt);

    '            // Hash the password And encode the parameters
    '            Byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
    '            Return PBKDF2_ITERATIONS + ":" +
    '                Convert.ToBase64String(salt) + ":" +
    '                Convert.ToBase64String(hash);
    '        }

    '        /// <summary>
    '        /// Validates a password given a hash of the correct one.
    '        /// </summary>
    '        /// <param name="password">The password to check.</param>
    '        /// <param name="correctHash">A hash of the correct password.</param>
    '        /// <returns>True if the password Is correct. False otherwise.</returns>
    '        Public Static bool ValidatePassword(String password, String correctHash)
    '        {
    '            // Extract the parameters from the hash
    '            Char[] delimiter = { ':' };
    '            String[] split = correctHash.Split(delimiter);
    '            int iterations = Int32.Parse(split[ITERATION_INDEX]);
    '            Byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
    '            Byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

    '            Byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
    '            Return SlowEquals(hash, testHash);
    '        }

    '        /// <summary>
    '        /// Compares two byte arrays in length-constant time. This comparison
    '        /// method Is used so that password hashes cannot be extracted from
    '        /// on-line systems using a timing attack And then attacked off-line.
    '        /// </summary>
    '        /// <param name="a">The first byte array.</param>
    '        /// <param name="b">The second byte array.</param>
    '        /// <returns>True if both byte arrays are equal. False otherwise.</returns>
    '        Private Static bool SlowEquals(Byte[] a, Byte[] b)
    '        {
    '            uint diff = (uint)a.Length ^ (uint)b.Length;
    '            For (int i = 0; i < a.Length && i < b.Length; i++)
    '                diff |= (uint)(a[i] ^ b[i]);
    '            Return diff == 0;
    '        }

    '        /// <summary>
    '        /// Computes the PBKDF2-SHA1 hash of a password.
    '        /// </summary>
    '        /// <param name="password">The password to hash.</param>
    '        /// <param name="salt">The salt.</param>
    '        /// <param name="iterations">The PBKDF2 iteration count.</param>
    '        /// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
    '        /// <returns>A hash of the password.</returns>
    '        Private Static Byte[] PBKDF2(String password, Byte[] salt, int iterations, int outputBytes)
    '        {
    '            Rfc2898DeriveBytes pbkdf2 = New Rfc2898DeriveBytes(password, salt);
    '            pbkdf2.IterationCount = iterations;
    '            Return pbkdf2.GetBytes(outputBytes);
    '        }
    '    }
    '}
End Class
