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
