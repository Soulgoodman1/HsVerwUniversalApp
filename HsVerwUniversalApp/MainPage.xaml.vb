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
                ShowLogin()
            Else
                'Server Down
                ckb_server.IsChecked = False
            End If

        Else
            'No Internet Connection
            ckb_internet.IsChecked = False
            ckb_server.IsChecked = False
        End If


        splashProgressRing.IsActive = False
        splashProgressRing.Visibility = Visibility.Collapsed

        'To Store a Password
        '1.Generate a long random salt using a CSPRNG.
        '2.Prepend the salt to the password And hash it with a standard cryptographic hash function such as SHA256.
        '3.Save both the salt And the hash in the user's database record.

        'To Validate a Password
        '1.Retrieve the user's salt and hash from the database.
        '2.Prepend the salt to the given password And hash it using the same hash function.
        '3.Compare the hash of the given password with the hash from the database. If they match, the password Is correct. Otherwise, the password Is incorrect.

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

    Private Sub ShowLogin()
        'Status-Elemente unsichtbar machen
        splashProgressRing.Visibility = Visibility.Collapsed
        splashProgressRing.IsActive = False
        ckb_internet.Visibility = Visibility.Collapsed
        ckb_server.Visibility = Visibility.Collapsed

        'Login-Elemente sichtbar machen
        username.Visibility = Visibility.Visible
        pwdbox.Visibility = Visibility.Visible
        btn_login.Visibility = Visibility.Visible
        btn_login.IsEnabled = True
        btn_register.Visibility = Visibility.Visible
        btn_register.IsEnabled = True
    End Sub

    Private Sub btn_login_Click(sender As Object, e As RoutedEventArgs) Handles btn_login.Click

        Frame.Navigate(GetType(MainHub))

    End Sub

    Private Sub pwdbox_Tapped(sender As Object, e As TappedRoutedEventArgs) Handles pwdbox.Tapped
        pwdbox.PasswordRevealMode = PasswordRevealMode.Hidden
        pwdbox.Password = ""
    End Sub

    Private Sub pwdbox_PointerEntered(sender As Object, e As PointerRoutedEventArgs) Handles pwdbox.PointerEntered
        pwdbox.PasswordRevealMode = PasswordRevealMode.Hidden
        pwdbox.Password = ""
    End Sub

    Private Sub btn_register_Click(sender As Object, e As RoutedEventArgs) Handles btn_register.Click
        'Prüfung Kennwort Komplexität
        'Prüfung, ob Username eMail Adresse entspricht xxxx@xx.xx


        'nach CreateHash Salt, Hash, Username, isactive = false wegschreiben über Backend
        'incl. Benachrichtigung 

    End Sub
End Class
