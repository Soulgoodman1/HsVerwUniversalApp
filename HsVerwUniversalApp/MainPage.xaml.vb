' Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
Public NotInheritable Class MainPage
    Inherits Page

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.


        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim _hhkatresult As Task(Of ObservableCollection(Of HsVerwSvc.Haushaltskategorie)) = vlo_client.GetHaushaltskategorienAsync

        ListviewHsVw.ItemsSource = _hhkatresult.Result

    End Sub

    Private Sub btn_weiterzutest_Click(sender As Object, e As RoutedEventArgs) Handles btn_weiterzutest.Click

        ' Zur neuen Seite navigieren
        Frame.Navigate(GetType(MainHub))

    End Sub
End Class
