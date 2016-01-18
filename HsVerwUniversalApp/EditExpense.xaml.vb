Imports HsVerwUniversalApp.MainHub
' Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
''' 

Public NotInheritable Class EditExpense
    Inherits Page

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    End Sub

    Private Sub speichernAusgabe(sender As Object, e As RoutedEventArgs)

        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim vlo_ausgabe As HsVerwSvc.Ausgabe = CType(Me.DataContext, HsVerwSvc.Ausgabe)
        Dim vlo_zahlungsrythmus As HsVerwSvc.Zahlungsrythmus = cbo_zahlungsrythmus.SelectedItem

        'mögliche Änderungen
        vlo_ausgabe.ZahlungsrythmusID = cbo_zahlungsrythmus.SelectedValue
        vlo_ausgabe.Wert = txt_wert.Text
        vlo_ausgabe.Haushaltsunterkategorie = txt_ausgabeart.Text
        vlo_ausgabe.Bemerkung = txt_bemerkung.Text

        Dim _hhsetresult As Task(Of String) = vlo_client.SetAusgabeAsync(vlo_ausgabe)

        If _hhsetresult.Result <> "" Then
            Frame.GoBack()
        End If

        vlo_zahlungsrythmus = Nothing
        vlo_ausgabe = Nothing
        vlo_client = Nothing

    End Sub

    Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)

        MyBase.OnNavigatedTo(e)

        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim vlo_ausgabeid As Long = CLng(e.Parameter)

        Dim _hhausgresult As HsVerwSvc.Ausgabe = Await vlo_client.GetAusgabebyIDAsync(vlo_ausgabeid)
        Me.DataContext = _hhausgresult

        Dim _hhrhtresult As ObservableCollection(Of HsVerwSvc.Zahlungsrythmus) = Await vlo_client.GetZahlungsrythmenAsync

        cbo_zahlungsrythmus.ItemsSource = _hhrhtresult
        cbo_zahlungsrythmus.SelectedValuePath = "ID"
        cbo_zahlungsrythmus.SelectedIndex = 0
        cbo_zahlungsrythmus.SelectedValue = _hhausgresult.ZahlungsrythmusID

        vlo_client = Nothing

    End Sub

End Class
