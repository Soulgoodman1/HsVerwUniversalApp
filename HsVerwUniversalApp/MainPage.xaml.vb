Imports System.Net.NetworkInformation
Imports Windows.Networking.Sockets
Imports Windows.UI
' Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
''' 


Public NotInheritable Class MainPage
    Inherits Page

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

    End Sub

    Private Sub App_BackHardware(sender As Object, e As Windows.Phone.UI.Input.BackPressedEventArgs)

        If Frame.CanGoBack Then
            e.Handled = True
            Frame.GoBack()
        End If

    End Sub

    Private Sub App_Backrequested(sender As Object, e As Windows.UI.Core.BackRequestedEventArgs)

        If Frame.CanGoBack Then
            e.Handled = True
            Frame.GoBack()
        End If

    End Sub

    Private Async Sub MainPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        'Falls bereits Handler vorhanden zuerst löschen, sonst mehrfaches Auslösen des BackPressed
        RemoveHandler Windows.Phone.UI.Input.HardwareButtons.BackPressed, AddressOf App_BackHardware
        RemoveHandler Windows.UI.Core.SystemNavigationManager.GetForCurrentView.BackRequested, AddressOf App_Backrequested

        If Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons") Then
            AddHandler Windows.Phone.UI.Input.HardwareButtons.BackPressed, AddressOf App_BackHardware
        Else
            AddHandler Windows.UI.Core.SystemNavigationManager.GetForCurrentView.BackRequested, AddressOf App_Backrequested
        End If

        'Test for Internet-Connection
        If NetworkInterface.GetIsNetworkAvailable Then
            'Internet available
            ckb_internet.Content = "Internet erreichbar"
            ckb_internet.IsChecked = True
            ckb_server.Content = "Server erreichbar"
            ckb_server.IsChecked = True
            Dim _serveravailable As Boolean = Await TestServer()

            If _serveravailable Then
                'Server available
                ' Zur neuen Seite navigieren
                Frame.Navigate(GetType(MainHub))
            Else
                'Server Down
                ckb_server.IsChecked = False
            End If

        Else
            'No Internet Connection
            ckb_internet.IsChecked = False
            ckb_server.IsChecked = False
        End If


        'To Store a Password
        '1.Generate a long random salt using a CSPRNG.
        '2.Prepend the salt to the password And hash it with a standard cryptographic hash function such as SHA256.
        '3.Save both the salt And the hash in the user's database record.

        'To Validate a Password
        '1.Retrieve the user's salt and hash from the database.
        '2.Prepend the salt to the given password And hash it using the same hash function.
        '3.Compare the hash of the given password with the hash from the database. If they match, the password Is correct. Otherwise, the password Is incorrect.


        '        Using System;
        'Using System.Text;
        'Using System.Security.Cryptography;

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


    End Sub
    Private Async Function TestServer() As Task(Of Boolean)

        Using _client As StreamSocket = New StreamSocket
            Try
                Await _client.ConnectAsync(New Windows.Networking.HostName("www.ralfabels.de"), "80")
            Catch ex As Exception
                Return False
            End Try
        End Using

        Return True
    End Function

End Class
