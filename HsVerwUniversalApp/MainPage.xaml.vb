Imports System.Net.NetworkInformation
Imports Windows.Networking.Sockets
Imports System.Text.RegularExpressions
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
        Dim vlo_crypto As New Crypto
        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim vlo_user As New HsVerwSvc.User

        Dim _hhsetresultuser As Task(Of HsVerwSvc.User) = vlo_client.GetUserAsync(username.Text)
        vlo_user = CType(_hhsetresultuser.Result, HsVerwSvc.User)

        If vlo_user.IDUser <> 0 Then
            If vlo_crypto.TestPassword(pwdbox.Password, vlo_user.hash, vlo_user.salt) Then
                If vlo_user.isactive Then
                    Frame.Navigate(GetType(MainHub))
                Else
                    'Nutzer nach Registrierung noch nicht aktiviert
                    CType(usernameflyout.Content, TextBox).Text = "Nutzer noch nicht aktiviert"
                    usernameflyout.ShowAt(username)
                End If
            Else
                'Falsches Kennwort
                CType(passwordflyout.Content, TextBox).Text = "Falsches Kennwort"
                passwordflyout.ShowAt(pwdbox)
            End If
        Else
            'Nutzer unbekannt
            CType(usernameflyout.Content, TextBox).Text = "Nutzer unbekannt"
            usernameflyout.ShowAt(username)
        End If


    End Sub

    Private Sub btn_register_Click(sender As Object, e As RoutedEventArgs) Handles btn_register.Click
        Dim vlo_crypto As New Crypto
        Dim vlo_user As New HsVerwSvc.User
        Dim vlo_client As New HsVerwSvc.Service1Client
        'Pürfen, ob User bereits vorhanden
        Dim _hhsetresultuser As Task(Of HsVerwSvc.User) = vlo_client.GetUserAsync(username.Text)

        If CType(_hhsetresultuser.Result, HsVerwSvc.User).IDUser = 0 Then 'ID = 0 heisst kein Nutzer mit diesem NAmen gefunden

            'Prüfung Kennwort Komplexität
            If ValidatePassword(pwdbox.Password) Then
                'Prüfung, ob Username eMail Adresse entspricht xxxx@xx.xx
                If IsValidEmail(username.Text) Then
                    'Create Hash
                    vlo_user = vlo_crypto.CreateHash(pwdbox.Password)
                    vlo_user.isactive = False
                    vlo_user.username = username.Text

                    'nach CreateHash Salt, Hash, Username, isactive = false wegschreiben über Backend
                    'incl. Benachrichtigung 

                    Dim _hhsetresult As Task(Of Boolean) = vlo_client.SetUserNewAsync(vlo_user)

                    If Not _hhsetresult.Result Then
                        Frame.GoBack()
                    End If
                Else
                    'Meldung keine gültige eMail Adresse
                    CType(usernameflyout.Content, TextBox).Text = "eMail-Adresse prüfen"
                    usernameflyout.ShowAt(username)
                End If 'Username gültig
            Else
                'Meldung Kennwort nicht komplex genug(mind. 8, 1*Groß, 1*Klein, 1*Nummer)
                CType(passwordflyout.Content, TextBox).Text = "Länge 8, mind. je 1 Groß/ Kleinbuchstabe, Nummer"
                passwordflyout.ShowAt(pwdbox)
            End If 'Password Complexity 
        Else
            'Meldung Nutzer bereits vorhanden
            CType(usernameflyout.Content, TextBox).Text = "User bereits vorhanden"
            usernameflyout.ShowAt(username)
        End If 'Username vorhanden

        vlo_client = Nothing

    End Sub

    Private Function IsValidEmail(strIn As String) As Boolean
        ' Return true if strIn is in valid e-mail format.
        Return Regex.IsMatch(strIn,
              "^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
              "(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$")
    End Function

    ''' <summary>Determines if a password is sufficiently complex.</summary>
    ''' <param name="pwd">Password to validate</param>
    ''' <param name="minLength">Minimum number of password characters.</param>
    ''' <param name="numUpper">Minimum number of uppercase characters.</param>
    ''' <param name="numLower">Minimum number of lowercase characters.</param>
    ''' <param name="numNumbers">Minimum number of numeric characters.</param>
    ''' <param name="numSpecial">Minimum number of special characters.</param>
    ''' <returns>True if the password is sufficiently complex.</returns>
    Private Function ValidatePassword(ByVal pwd As String,
        Optional ByVal minLength As Integer = 8,
        Optional ByVal numUpper As Integer = 1,
        Optional ByVal numLower As Integer = 1,
        Optional ByVal numNumbers As Integer = 2,
        Optional ByVal numSpecial As Integer = 0) As Boolean

        ' Replace [A-Z] with \p{Lu}, to allow for Unicode uppercase letters.
        Dim upper As New System.Text.RegularExpressions.Regex("[A-Z]")
        Dim lower As New System.Text.RegularExpressions.Regex("[a-z]")
        Dim number As New System.Text.RegularExpressions.Regex("[0-9]")
        ' Special is "none of the above".
        Dim special As New System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]")

        ' Check the length.
        If pwd.Length < minLength Then Return False
        ' Check for minimum number of occurrences.
        If upper.Matches(pwd).Count < numUpper Then Return False
        If lower.Matches(pwd).Count < numLower Then Return False
        If number.Matches(pwd).Count < numNumbers Then Return False
        If special.Matches(pwd).Count < numSpecial Then Return False

        ' Passed all checks.
        Return True
    End Function

End Class
