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

        ListviewConsumption.ItemsSource = _hhkatresult.Result.Where(Function(vlo_verbrauch) (vlo_verbrauch.Datum > "31.12." & DateTime.Now.Year - 2) And (vlo_verbrauch.Datum < "01.01." & DateTime.Now.Year)).OrderBy(Function(vlo_verbrauch) vlo_verbrauch.Haushaltsunterkategorie).ThenBy(Function(vlo_verbrauch) vlo_verbrauch.Datum)

    End Sub

    Private Sub hinzufuegenVerbrauch(sender As Object, e As RoutedEventArgs)

        Frame.Navigate(GetType(AddConsumption))

    End Sub

    Private Sub loeschenVerbrauch(sender As Object, e As RoutedEventArgs)

        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim vlo_verbrauch As HsVerwSvc.Verbrauch


        For Each vlo_verbrauch In ListviewConsumption.SelectedItems

            Dim _hhsetresult As Task(Of Boolean) = vlo_client.DeleteVerbrauchAsync(vlo_verbrauch)

        Next

        vlo_verbrauch = Nothing
        vlo_client = Nothing

    End Sub

    Private Sub aendernVerbrauch(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub suchenVerbrauch(sender As Object, e As RoutedEventArgs)

    End Sub

End Class
