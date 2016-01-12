Imports HsVerwUniversalApp.MainHub
' Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
''' 

Public NotInheritable Class AddConsumption
    Inherits Page

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim _hhkatresult As Task(Of ObservableCollection(Of HsVerwSvc.IService1Verbrauchstyp)) = vlo_client.GetVerbrauchsTypAsync(1)

        cbo_verbrauchstyp.ItemsSource = _hhkatresult.Result
        cbo_verbrauchstyp.SelectedValuePath = "ID"
        cbo_verbrauchstyp.SelectedIndex = 0

        vlo_client = Nothing

    End Sub

    Private Sub speichernVerbrauch(sender As Object, e As RoutedEventArgs)

        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim vlo_verbrauch As New HsVerwSvc.Verbrauch
        Dim vlo_verbrauchstyp As HsVerwSvc.IService1Verbrauchstyp = cbo_verbrauchstyp.SelectedItem

        vlo_verbrauch.Datum = ctrl_datepicker.Date.Date
        vlo_verbrauch.HaushaltsunterkategorieID = cbo_verbrauchstyp.SelectedValue
        vlo_verbrauch.HaushaltskategorieID = vlo_verbrauchstyp.HaushaltskategorieID
        If Double.TryParse(txt_wert.Text, New Double) Then

            vlo_verbrauch.Wert = txt_wert.Text

            Dim _hhsetresult As Task(Of Boolean) = vlo_client.SetVerbrauchNewAsync(vlo_verbrauch)

            If _hhsetresult.Result Then
                Frame.GoBack()
            End If

        End If

        vlo_verbrauchstyp = Nothing
        vlo_verbrauch = Nothing
        vlo_client = Nothing

    End Sub

End Class
