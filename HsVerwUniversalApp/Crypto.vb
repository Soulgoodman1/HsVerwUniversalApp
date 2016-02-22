Imports Windows.Security.Cryptography
Imports Windows.Security.Cryptography.Core
Imports Windows.Storage.Streams
Imports System.Text


Public Class Crypto

    Public Function CreateHash(ByVal password As String) As HsVerwSvc.User
        Dim vlo_user As New HsVerwSvc.User

        'Append Salt
        vlo_user.salt = GenerateRandomData()

        password = password & vlo_user.salt

        'Hash Password und Salt
        Dim objMd5Hasher As CryptographicHash = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5).CreateHash()

        Dim inputBuffer As IBuffer = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf16LE)

        objMd5Hasher.Append(inputBuffer)

        Dim buffHash As IBuffer = objMd5Hasher.GetValueAndReset()

        Dim sb As StringBuilder = New StringBuilder()

        Dim data As Byte() = Nothing

        CryptographicBuffer.CopyToByteArray(buffHash, data)

        For i As Integer = 0 To (data.Length - 1) Step 1
            sb.Append(data(i).ToString("x2"))
        Next i

        vlo_user.hash = sb.ToString()

        'Return salted Hash
        Return vlo_user

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

    Public Function TestPassword(ByVal password As String, ByVal hash As String, ByVal salt As String) As Boolean
        Dim vlo_user As New HsVerwSvc.User

        password = password & salt

        'Hash Password und Salt
        Dim objMd5Hasher As CryptographicHash = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5).CreateHash()

        Dim inputBuffer As IBuffer = CryptographicBuffer.ConvertStringToBinary(password, BinaryStringEncoding.Utf16LE)

        objMd5Hasher.Append(inputBuffer)

        Dim buffHash As IBuffer = objMd5Hasher.GetValueAndReset()

        Dim sb As StringBuilder = New StringBuilder()

        Dim data As Byte() = Nothing

        CryptographicBuffer.CopyToByteArray(buffHash, data)

        For i As Integer = 0 To (data.Length - 1) Step 1
            sb.Append(data(i).ToString("x2"))
        Next i

        If hash = sb.ToString() Then
            Return True
        Else
            Return False
        End If

    End Function

End Class
