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

    End Sub

    Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)

        Dim vlo_client As New HsVerwSvc.Service1Client

        MyBase.OnNavigatedTo(e)
        Dim vlo_parameter As Object = e.Parameter
        Dim _hhAusgresult As ObservableCollection(Of HsVerwSvc.Ausgabe) = Await vlo_client.GetAusgabenAsync

        'Prüfen, ob Suchparameter übergeben wurden

        If vlo_parameter Is Nothing Then
            ListviewExpense.ItemsSource = _hhAusgresult
        Else
            Dim vlo_searchparam As SearchParam = e.Parameter

            If vlo_searchparam.Kategorie <> 0 Then
                ListviewExpense.ItemsSource = _hhAusgresult.Where(Function(vlo_ausgabe) (vlo_ausgabe.HaushaltskategorieID = vlo_searchparam.Kategorie)).OrderBy(Function(vlo_ausgabe) vlo_ausgabe.Haushaltsunterkategorie)
            Else
                ListviewExpense.ItemsSource = _hhAusgresult
            End If

        End If

        vlo_client = Nothing

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

        Frame.Navigate(GetType(EditExpense), CType(ListviewExpense.SelectedItems(0), HsVerwSvc.Ausgabe).ID)

    End Sub

    Private Sub suchenAusgabe(sender As Object, e As RoutedEventArgs)

        Frame.Navigate(GetType(Search), "Ausgabe")

    End Sub

End Class
