' Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
Public NotInheritable Class ListConsumption
    Inherits Page

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.


        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim _hhkatresult As Task(Of ObservableCollection(Of HsVerwSvc.Verbrauch)) = vlo_client.GetVerbrauchAsync

        ListviewConsumption.ItemsSource = _hhkatresult.Result

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

    Private Sub ListConsumption_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        If Frame.CanGoBack Then
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible
        Else
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Collapsed
        End If

        If Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons") Then
            AddHandler Windows.Phone.UI.Input.HardwareButtons.BackPressed, AddressOf App_BackHardware
        Else
            AddHandler Windows.UI.Core.SystemNavigationManager.GetForCurrentView.BackRequested, AddressOf App_Backrequested
        End If
    End Sub

End Class
