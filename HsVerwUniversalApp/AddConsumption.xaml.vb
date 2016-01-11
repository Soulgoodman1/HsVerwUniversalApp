' Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
''' 

Imports HsVerwUniversalApp.MainHub

Public NotInheritable Class AddConsumption
    Inherits Page

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.


        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim _hhkatresult As Task(Of ObservableCollection(Of HsVerwSvc.Haushaltskategorie)) = vlo_client.GetHaushaltskategorienAsync

        cbo_verbrauchstyp.ItemsSource = _hhkatresult.Result

    End Sub

    Private Sub speichernVerbrauch(sender As Object, e As RoutedEventArgs)

    End Sub

End Class
