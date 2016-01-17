Imports HsVerwUniversalApp.MainHub
' Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
''' 

Public NotInheritable Class EditConsumption
    Inherits Page

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    End Sub

    Private Sub speichernVerbrauch(sender As Object, e As RoutedEventArgs)

        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim vlo_verbrauch As HsVerwSvc.Verbrauch = CType(Me.DataContext, HsVerwSvc.Verbrauch)
        Dim vlo_verbrauchstyp As HsVerwSvc.IService1Verbrauchstyp = cbo_verbrauchstyp.SelectedItem

        vlo_verbrauch.Datum = ctrl_datepicker.Date.Date
        vlo_verbrauch.HaushaltsunterkategorieID = cbo_verbrauchstyp.SelectedValue

        If Double.TryParse(txt_wert.Text, New Double) Then

            vlo_verbrauch.Wert = txt_wert.Text

            Dim _hhsetresult As Task(Of String) = vlo_client.SetVerbrauchAsync(vlo_verbrauch)

            If _hhsetresult.Result <> "" Then
                Frame.GoBack()
            End If

        End If

        vlo_verbrauchstyp = Nothing
        vlo_verbrauch = Nothing
        vlo_client = Nothing

    End Sub

    Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)

        MyBase.OnNavigatedTo(e)

        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim vlo_verbrauchid As Long = CLng(e.Parameter)

        Dim _hhverbrresult As HsVerwSvc.Verbrauch = Await vlo_client.GetVerbrauchbyIDAsync(vlo_verbrauchid)
        Me.DataContext = _hhverbrresult

        Dim _hhkatresult As ObservableCollection(Of HsVerwSvc.IService1Verbrauchstyp) = Await vlo_client.GetVerbrauchsTypAsync(1)

        cbo_verbrauchstyp.ItemsSource = _hhkatresult
        cbo_verbrauchstyp.SelectedValuePath = "ID"
        cbo_verbrauchstyp.SelectedIndex = 0
        cbo_verbrauchstyp.SelectedValue = _hhverbrresult.HaushaltsunterkategorieID

        ctrl_datepicker.Date = CType(_hhverbrresult, HsVerwSvc.Verbrauch).Datum

        vlo_client = Nothing

    End Sub
End Class
