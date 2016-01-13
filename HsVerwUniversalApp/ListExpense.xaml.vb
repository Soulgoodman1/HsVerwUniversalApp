' Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
Public NotInheritable Class ListExpense
    Inherits Page

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.


        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim _hhkatresult As Task(Of ObservableCollection(Of HsVerwSvc.Ausgabe)) = vlo_client.GetAusgabenAsync
        Dim vlo_ausgabe As New HsVerwSvc.Ausgabe

        ListviewExpense.ItemsSource = _hhkatresult.Result

    End Sub

    Private Sub hinzufuegenAusgabe(sender As Object, e As RoutedEventArgs)

        Frame.Navigate(GetType(AddExpense))

    End Sub

    Private Sub loeschenAusgabe(sender As Object, e As RoutedEventArgs)

        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim vlo_ausgabe As HsVerwSvc.Ausgabe


        For Each vlo_ausgabe In ListviewExpense.SelectedItems

            Dim _hhsetresult As Task(Of Boolean) = vlo_client.DeleteAusgabeAsync(vlo_ausgabe)

        Next

        vlo_ausgabe = Nothing
        vlo_client = Nothing

    End Sub

    Private Sub aendernAusgabe(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub suchenAusgabe(sender As Object, e As RoutedEventArgs)

        Frame.Navigate(GetType(Search), "Ausgabe")

    End Sub

End Class
