Imports Windows.Security.Cryptography
Imports Windows.Security.Cryptography.Core
Imports Windows.Storage.Streams
Imports System.Text


Public Class Crypto

    Public Function CreateHash(ByVal password As String) As String

        'Append Salt
        password = password & GenerateRandomData()

        'Hash Password und Salt
        Dim objMd5Hasher As CryptographicHash = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5).CreateHash()

        Dim inputBuffer As IBuffer = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf16LE)

        objMd5Hasher.Append(inputBuffer)

        Dim buffHash As IBuffer = objMd5Hasher.GetValueAndReset()

        Dim sb As StringBuilder = New StringBuilder()

        Dim data As Byte() = Nothing

        CryptographicBuffer.CopyToByteArray(buffHash, data)

        For i As Integer = 0 To i < data.Length Step 1
            sb.Append(data(i).ToString("x2"))
        Next i

        'Return salted Hash
        Return sb.ToString()

    End Function

    'Generate a random salt
    Public Function GenerateRandomData() As String

        'Define the length, in bytes, of the buffer.
        Dim length As Integer = 32

        'Generate random data And copy it to a buffer
        Dim Buffer As IBuffer = CryptographicBuffer.GenerateRandom(length)

        'Encode the buffer to a hexadecimal string (for display)
        Dim randomHex As String = CryptographicBuffer.EncodeToHexString(Buffer)

        Return randomHex
    End Function



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
