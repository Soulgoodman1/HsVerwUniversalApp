Imports HsVerwUniversalApp.MainHub
' Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
''' 

Public NotInheritable Class Search
    Inherits Page

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    End Sub

    Private Sub filtern(sender As Object, e As RoutedEventArgs)


    End Sub

    Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)

        Dim vlo_suchtyp As String = e.Parameter.ToString
        Dim vlo_client As New HsVerwSvc.Service1Client

        MyBase.OnNavigatedTo(e)

        Dim _hhkatesult As ObservableCollection(Of HsVerwSvc.Haushaltskategorie) = Await vlo_client.GetHaushaltskategorienAsync

        cbo_haushaltskategorie.ItemsSource = _hhkatesult
        cbo_haushaltskategorie.SelectedValuePath = "ID"
        cbo_haushaltskategorie.SelectedIndex = 0

        Dim _hhukatresult As ObservableCollection(Of HsVerwSvc.Haushaltsunterkategorie) = Await vlo_client.GetHaushaltsunterkategorienAsync

        cbo_haushaltsunterkategorie.ItemsSource = _hhukatresult
        cbo_haushaltsunterkategorie.SelectedValuePath = "ID"
        cbo_haushaltsunterkategorie.SelectedIndex = 0

        vlo_client = Nothing

        Select Case vlo_suchtyp
            Case "Verbrauch"
                txt_kategorie.Visibility = Visibility.Collapsed
                cbo_haushaltskategorie.Visibility = Visibility.Collapsed
                'Filter für Unterkategorien

            Case "Ausgabe"
                txt_von.Visibility = Visibility.Collapsed
                ctrl_datepickerbis.Visibility = Visibility.Collapsed

                txt_bis.Visibility = Visibility.Collapsed
                ctrl_datepickervon.Visibility = Visibility.Collapsed

                txt_unterkategorie.Visibility = Visibility.Collapsed
                cbo_haushaltsunterkategorie.Visibility = Visibility.Collapsed

                'Filter für Kategorien(Ausgabe variabel, fix)

            Case Else

        End Select

    End Sub

End Class
