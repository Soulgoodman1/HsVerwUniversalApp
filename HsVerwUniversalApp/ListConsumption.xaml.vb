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


        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim _hhkatresult As Task(Of ObservableCollection(Of HsVerwSvc.Verbrauch)) = vlo_client.GetVerbrauchAsync

        ListviewConsumption.ItemsSource = _hhkatresult.Result.Where(Function(vlo_verbrauch) (vlo_verbrauch.Datum > "31.12." & DateTime.Now.Year - 2) And (vlo_verbrauch.Datum < "01.02." & DateTime.Now.Year)).OrderBy(Function(vlo_verbrauch) vlo_verbrauch.Haushaltsunterkategorie).ThenBy(Function(vlo_verbrauch) vlo_verbrauch.Datum)

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

        Dim _hhkatresult As Task(Of ObservableCollection(Of HsVerwSvc.Verbrauch)) = vlo_client.GetVerbrauchAsync

        ListviewConsumption.ItemsSource = Nothing
        ListviewConsumption.ItemsSource = _hhkatresult.Result.Where(Function(vlo_verbrauch) (vlo_verbrauch.Datum > "31.12." & DateTime.Now.Year - 2) And (vlo_verbrauch.Datum < "01.02." & DateTime.Now.Year)).OrderBy(Function(vlo_verbrauch) vlo_verbrauch.Haushaltsunterkategorie).ThenBy(Function(vlo_verbrauch) vlo_verbrauch.Datum)

        vlo_verbrauchselected = Nothing
        vlo_client = Nothing

    End Sub

    Private Sub aendernVerbrauch(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub suchenVerbrauch(sender As Object, e As RoutedEventArgs)

    End Sub

End Class
