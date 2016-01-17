Imports HsVerwUniversalApp.MainHub
' Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
''' 

Public NotInheritable Class EditIncome
    Inherits Page

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    End Sub

    Private Sub speichernEinnahme(sender As Object, e As RoutedEventArgs)

        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim vlo_einnahme As HsVerwSvc.Einnahme = CType(Me.DataContext, HsVerwSvc.Einnahme)

        'mögliche Änderungen
        vlo_einnahme.ZahlungsrythmusID = cbo_zahlungsrythmus.SelectedValue
        vlo_einnahme.Wert = txt_wert.Text
        vlo_einnahme.Haushaltsunterkategorie = txt_einnahmeart.Text

        Dim _hhsetresult As Task(Of String) = vlo_client.SetEinnahmeAsync(vlo_einnahme)

        If _hhsetresult.Result <> "" Then
            Frame.GoBack()
        End If

        vlo_einnahme = Nothing
        vlo_client = Nothing

    End Sub

    Protected Overrides Async Sub OnNavigatedTo(ByVal e As NavigationEventArgs)

        MyBase.OnNavigatedTo(e)
        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim vlo_einnhameid As Long = CLng(e.Parameter)

        Dim _hheinhresult As HsVerwSvc.Einnahme = Await vlo_client.GetEinnahmebyIDAsync(vlo_einnhameid)
        Me.DataContext = _hheinhresult

        Dim _hhrhtresult As ObservableCollection(Of HsVerwSvc.Zahlungsrythmus) = Await vlo_client.GetZahlungsrythmenAsync

        cbo_zahlungsrythmus.ItemsSource = _hhrhtresult
        cbo_zahlungsrythmus.SelectedValuePath = "ID"
        cbo_zahlungsrythmus.SelectedIndex = 0
        cbo_zahlungsrythmus.SelectedValue = _hheinhresult.ZahlungsrythmusID

    End Sub

End Class
