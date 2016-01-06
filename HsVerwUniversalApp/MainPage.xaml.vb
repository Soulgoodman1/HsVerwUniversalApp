' Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
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

    Private Sub MainPage_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        'Falls bereits Handler vorhanden zuerst löschen, sonst mehrfaches Auslösen des BackPressed
        RemoveHandler Windows.Phone.UI.Input.HardwareButtons.BackPressed, AddressOf App_BackHardware
        RemoveHandler Windows.UI.Core.SystemNavigationManager.GetForCurrentView.BackRequested, AddressOf App_Backrequested

        If Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons") Then
            AddHandler Windows.Phone.UI.Input.HardwareButtons.BackPressed, AddressOf App_BackHardware
        Else
            AddHandler Windows.UI.Core.SystemNavigationManager.GetForCurrentView.BackRequested, AddressOf App_Backrequested
        End If

    End Sub

    Private Sub btn_weiterzutest_Click(sender As Object, e As RoutedEventArgs) Handles btn_weiterzutest.Click

        ' Zur neuen Seite navigieren
        Frame.Navigate(GetType(MainHub))

    End Sub
End Class
