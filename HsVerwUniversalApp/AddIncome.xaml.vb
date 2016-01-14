Imports HsVerwUniversalApp.MainHub
' Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
''' 

Public NotInheritable Class AddIncome
    Inherits Page

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    End Sub

    Private Sub speichernEinnahme(sender As Object, e As RoutedEventArgs)

        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim vlo_einnahme As New HsVerwSvc.Einnahme
        Dim vlo_zahlungsrythmus As HsVerwSvc.Zahlungsrythmus = cbo_zahlungsrythmus.SelectedItem

        vlo_einnahme.ZahlungsrythmusID = cbo_zahlungsrythmus.SelectedValue
        vlo_einnahme.EinheitID = cbo_einheit.SelectedValue
        vlo_einnahme.HaushaltskategorieID = 3

        If Double.TryParse(txt_wert.Text, New Double) Then

            vlo_einnahme.Wert = txt_wert.Text

            If Not String.IsNullOrEmpty(txt_einnahmeart.Text) Then

                vlo_einnahme.Einnahmentyp = txt_einnahmeart.Text

                Dim _hhsetresult As Task(Of String) = vlo_client.SetEinnahmeNewAsync(vlo_einnahme)

                If _hhsetresult.Result <> "" Then
                    Frame.GoBack()
                End If

            End If

        End If

        vlo_zahlungsrythmus = Nothing
        vlo_einnahme = Nothing
        vlo_client = Nothing

    End Sub

    Protected Overrides Async Sub OnNavigatedTo(ByVal e As NavigationEventArgs)

        MyBase.OnNavigatedTo(e)
        Dim vlo_client As New HsVerwSvc.Service1Client

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
