' Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
Public NotInheritable Class ListIncome
    Inherits Page

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.


        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim _hhkatresult As Task(Of ObservableCollection(Of HsVerwSvc.Einnahme)) = vlo_client.GetEinnahmenAsync

        ListviewIncome.ItemsSource = _hhkatresult.Result

    End Sub


    Private Sub hinzufuegenEinnahme(sender As Object, e As RoutedEventArgs)

        Frame.Navigate(GetType(AddIncome))

    End Sub

    Private Sub loeschenEinnahme(sender As Object, e As RoutedEventArgs)

        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim vlo_einnahme As HsVerwSvc.Einnahme


        For Each vlo_einnahme In ListviewIncome.SelectedItems

            Dim _hhsetresult As Task(Of Boolean) = vlo_client.DeleteEinnahmeAsync(vlo_einnahme)

        Next

        vlo_einnahme = Nothing
        vlo_client = Nothing

    End Sub

    Private Sub aendernEinnahme(sender As Object, e As RoutedEventArgs)

        Frame.Navigate(GetType(EditIncome), CType(ListviewIncome.SelectedItems(0), HsVerwSvc.Einnahme).ID)

    End Sub


End Class
