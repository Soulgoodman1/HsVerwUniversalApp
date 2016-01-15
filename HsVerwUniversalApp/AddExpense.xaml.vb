Imports HsVerwUniversalApp.MainHub
' Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
''' 

Public NotInheritable Class AddExpense
    Inherits Page

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    End Sub

    Private Sub speichernAusgabe(sender As Object, e As RoutedEventArgs)

        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim vlo_ausgabe As New HsVerwSvc.Ausgabe
        Dim vlo_zahlungsrythmus As HsVerwSvc.Zahlungsrythmus = cbo_zahlungsrythmus.SelectedItem

        vlo_ausgabe.ZahlungsrythmusID = cbo_zahlungsrythmus.SelectedValue
        vlo_ausgabe.EinheitID = cbo_einheit.SelectedValue
        vlo_ausgabe.HaushaltskategorieID = cbo_ausgabenart.SelectedValue

        If Double.TryParse(txt_wert.Text, New Double) Then

            vlo_ausgabe.Wert = txt_wert.Text

            If Not String.IsNullOrEmpty(txt_ausgabeart.Text) Then

                vlo_ausgabe.Haushaltsunterkategorie = txt_ausgabeart.Text

                Dim _hhsetresult As Task(Of String) = vlo_client.SetAusgabeNewAsync(vlo_ausgabe)

                If _hhsetresult.Result <> "" Then
                    Frame.GoBack()
                End If

            End If

        End If

        vlo_zahlungsrythmus = Nothing
        vlo_ausgabe = Nothing
        vlo_client = Nothing

    End Sub

    Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)

        Dim vlo_client As New HsVerwSvc.Service1Client

        MyBase.OnNavigatedTo(e)

        Dim _hhkatresult As ObservableCollection(Of HsVerwSvc.Haushaltskategorie) = Await vlo_client.GetHaushaltskategorienAsync

        cbo_ausgabenart.ItemsSource = _hhkatresult.Where(Function(vlo_ausgabe) (vlo_ausgabe.ID = 2 Or vlo_ausgabe.ID = 5))
        cbo_ausgabenart.SelectedValuePath = "ID"
        cbo_ausgabenart.SelectedIndex = 0

        Dim _hhrhtresult As ObservableCollection(Of HsVerwSvc.Zahlungsrythmus) = Await vlo_client.GetZahlungsrythmenAsync

        cbo_zahlungsrythmus.ItemsSource = _hhrhtresult
        cbo_zahlungsrythmus.SelectedValuePath = "ID"
        cbo_zahlungsrythmus.SelectedIndex = 0

        Dim _hheinhresult As ObservableCollection(Of HsVerwSvc.Einheit) = Await vlo_client.GetEinheitenAsync

        cbo_einheit.ItemsSource = _hheinhresult
        cbo_einheit.SelectedValuePath = "ID"
        cbo_einheit.SelectedIndex = 0

        vlo_client = Nothing

    End Sub

End Class
