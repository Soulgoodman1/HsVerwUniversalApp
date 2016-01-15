Imports HsVerwUniversalApp.MainHub
' Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
''' 

Public NotInheritable Class ListConsumption
    Inherits Page

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    End Sub

    Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)

        Dim vlo_client As New HsVerwSvc.Service1Client

        MyBase.OnNavigatedTo(e)
        Dim vlo_parameter As Object = e.Parameter
        Dim _hhVerbresult As ObservableCollection(Of HsVerwSvc.Verbrauch) = Await vlo_client.GetVerbrauchAsync

        'Prüfen, ob Suchparameter übergeben wurden

        If vlo_parameter Is Nothing Then
            ListviewConsumption.ItemsSource = _hhVerbresult.Where(Function(vlo_verbrauch) (vlo_verbrauch.Datum > "31.12." & DateTime.Now.Year - 2) And (vlo_verbrauch.Datum < "01.02." & DateTime.Now.Year)).OrderBy(Function(vlo_verbrauch) vlo_verbrauch.Haushaltsunterkategorie).ThenBy(Function(vlo_verbrauch) vlo_verbrauch.Datum)
        Else
            Dim vlo_searchparam As SearchParam = e.Parameter

            If vlo_searchparam.StartDatum <> "" And vlo_searchparam.EndDatum <> "" Then
                If vlo_searchparam.Unterkategorie <> 0 Then
                    ListviewConsumption.ItemsSource = _hhVerbresult.Where(Function(vlo_verbrauch) (vlo_verbrauch.Datum > vlo_searchparam.StartDatum) And (vlo_verbrauch.Datum < vlo_searchparam.EndDatum) And (vlo_verbrauch.HaushaltsunterkategorieID = vlo_searchparam.Unterkategorie)).OrderBy(Function(vlo_verbrauch) vlo_verbrauch.Haushaltsunterkategorie).ThenBy(Function(vlo_verbrauch) vlo_verbrauch.Datum)
                Else
                    ListviewConsumption.ItemsSource = _hhVerbresult.Where(Function(vlo_verbrauch) (vlo_verbrauch.Datum > vlo_searchparam.StartDatum) And (vlo_verbrauch.Datum < vlo_searchparam.EndDatum)).OrderBy(Function(vlo_verbrauch) vlo_verbrauch.Haushaltsunterkategorie).ThenBy(Function(vlo_verbrauch) vlo_verbrauch.Datum)
                End If

            End If

            If vlo_searchparam.StartDatum <> "" And vlo_searchparam.EndDatum = "" Then
                If vlo_searchparam.Unterkategorie <> 0 Then
                    ListviewConsumption.ItemsSource = _hhVerbresult.Where(Function(vlo_verbrauch) (vlo_verbrauch.Datum > vlo_searchparam.StartDatum) And (vlo_verbrauch.HaushaltsunterkategorieID = vlo_searchparam.Unterkategorie)).OrderBy(Function(vlo_verbrauch) vlo_verbrauch.Haushaltsunterkategorie).ThenBy(Function(vlo_verbrauch) vlo_verbrauch.Datum)
                Else
                    ListviewConsumption.ItemsSource = _hhVerbresult.Where(Function(vlo_verbrauch) (vlo_verbrauch.Datum > vlo_searchparam.StartDatum)).OrderBy(Function(vlo_verbrauch) vlo_verbrauch.Haushaltsunterkategorie).ThenBy(Function(vlo_verbrauch) vlo_verbrauch.Datum)
                End If
            End If

            If vlo_searchparam.StartDatum = "" And vlo_searchparam.EndDatum <> "" Then
                If vlo_searchparam.Unterkategorie <> 0 Then
                    ListviewConsumption.ItemsSource = _hhVerbresult.Where(Function(vlo_verbrauch) (vlo_verbrauch.Datum < vlo_searchparam.EndDatum) And (vlo_verbrauch.HaushaltsunterkategorieID = vlo_searchparam.Unterkategorie)).OrderBy(Function(vlo_verbrauch) vlo_verbrauch.Haushaltsunterkategorie).ThenBy(Function(vlo_verbrauch) vlo_verbrauch.Datum)
                Else
                    ListviewConsumption.ItemsSource = _hhVerbresult.Where(Function(vlo_verbrauch) (vlo_verbrauch.Datum < vlo_searchparam.EndDatum)).OrderBy(Function(vlo_verbrauch) vlo_verbrauch.Haushaltsunterkategorie).ThenBy(Function(vlo_verbrauch) vlo_verbrauch.Datum)
                End If
            End If

            If vlo_searchparam.StartDatum = "" And vlo_searchparam.EndDatum = "" Then
                If vlo_searchparam.Unterkategorie <> 0 Then
                    ListviewConsumption.ItemsSource = _hhVerbresult.Where(Function(vlo_verbrauch) (vlo_verbrauch.HaushaltsunterkategorieID = vlo_searchparam.Unterkategorie)).OrderBy(Function(vlo_verbrauch) vlo_verbrauch.Haushaltsunterkategorie).ThenBy(Function(vlo_verbrauch) vlo_verbrauch.Datum)
                Else
                    ListviewConsumption.ItemsSource = _hhVerbresult.Where(Function(vlo_verbrauch) (vlo_verbrauch.Datum > "31.12." & DateTime.Now.Year - 2) And (vlo_verbrauch.Datum < "01.02." & DateTime.Now.Year)).OrderBy(Function(vlo_verbrauch) vlo_verbrauch.Haushaltsunterkategorie).ThenBy(Function(vlo_verbrauch) vlo_verbrauch.Datum)
                End If
            End If

        End If

        vlo_client = Nothing

    End Sub

    Private Sub hinzufuegenVerbrauch(sender As Object, e As RoutedEventArgs)

        Frame.Navigate(GetType(AddConsumption))

    End Sub

    Private Sub loeschenVerbrauch(sender As Object, e As RoutedEventArgs)

        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim vlo_verbrauchselected As HsVerwSvc.Verbrauch


        For Each vlo_verbrauchselected In ListviewConsumption.SelectedItems

            Dim _hhsetresult As Task(Of Boolean) = vlo_client.DeleteVerbrauchAsync(vlo_verbrauchselected)

        Next

        Dim _hhVerbresult As Task(Of ObservableCollection(Of HsVerwSvc.Verbrauch)) = vlo_client.GetVerbrauchAsync

        ListviewConsumption.ItemsSource = Nothing
        ListviewConsumption.ItemsSource = _hhVerbresult.Result.Where(Function(vlo_verbrauch) (vlo_verbrauch.Datum > "31.12." & DateTime.Now.Year - 2) And (vlo_verbrauch.Datum < "01.02." & DateTime.Now.Year)).OrderBy(Function(vlo_verbrauch) vlo_verbrauch.Haushaltsunterkategorie).ThenBy(Function(vlo_verbrauch) vlo_verbrauch.Datum)

        vlo_verbrauchselected = Nothing
        vlo_client = Nothing

    End Sub

    Private Sub aendernVerbrauch(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub suchenVerbrauch(sender As Object, e As RoutedEventArgs)

        Frame.Navigate(GetType(Search), "Verbrauch")

    End Sub

End Class
