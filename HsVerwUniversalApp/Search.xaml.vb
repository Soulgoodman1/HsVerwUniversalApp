Imports HsVerwUniversalApp.MainHub
' Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
''' 

Public NotInheritable Class Search
    Inherits Page

    Private vmo_suchtyp As String = ""

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    End Sub

    Private Sub filtern(sender As Object, e As RoutedEventArgs)
        Dim vlo_searchparameter As New SearchParam

        If cbo_haushaltskategorie.Visibility = Visibility.Visible Then
            vlo_searchparameter.Kategorie = cbo_haushaltskategorie.SelectedValue
        End If

        If cbo_haushaltsunterkategorie.Visibility = Visibility.Visible Then
            vlo_searchparameter.Unterkategorie = cbo_haushaltsunterkategorie.SelectedValue
        End If

        If ctrl_datepickervon.Visibility = Visibility.Visible Then
            vlo_searchparameter.StartDatum = ctrl_datepickervon.Date.Day & "." & ctrl_datepickervon.Date.Month & "." & ctrl_datepickervon.Date.Year
        End If

        If ctrl_datepickerbis.Visibility = Visibility.Visible Then
            vlo_searchparameter.EndDatum = ctrl_datepickerbis.Date.Day & "." & ctrl_datepickerbis.Date.Month & "." & ctrl_datepickerbis.Date.Year
        End If

        Select Case vmo_suchtyp
            Case "Verbrauch"
                Frame.Navigate(GetType(ListConsumption), vlo_searchparameter)

            Case "Ausgabe"
                Frame.Navigate(GetType(ListExpense), vlo_searchparameter)

        End Select

    End Sub

    Public Sub reset()
        Select Case vmo_suchtyp
            Case "Verbrauch"
                opt_datepickerbis.IsChecked = True
                opt_datepickervon.IsChecked = True
                ctrl_datepickerbis.Date = DateTimeOffset.Now
                ctrl_datepickerbis.Date = DateTimeOffset.Now
                cbo_haushaltsunterkategorie.SelectedIndex = 0

            Case "Ausgabe"
                cbo_haushaltskategorie.SelectedIndex = 0

        End Select

    End Sub

    Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)


        Dim vlo_client As New HsVerwSvc.Service1Client

        MyBase.OnNavigatedTo(e)
        vmo_suchtyp = e.Parameter.ToString

        Select Case vmo_suchtyp
            Case "Verbrauch"
                txt_kategorie.Visibility = Visibility.Collapsed
                cbo_haushaltskategorie.Visibility = Visibility.Collapsed

                Dim _hhukatresult As ObservableCollection(Of HsVerwSvc.Haushaltsunterkategorie) = Await vlo_client.GetHaushaltsunterkategorienAsync

                cbo_haushaltsunterkategorie.ItemsSource = _hhukatresult.Where(Function(vlo_unterkategorie) (vlo_unterkategorie.KategorieID = 1 Or vlo_unterkategorie.KategorieID = 4))
                cbo_haushaltsunterkategorie.SelectedValuePath = "ID"
                cbo_haushaltsunterkategorie.SelectedIndex = 0

                cbo_haushaltskategorie.ItemsSource = Nothing

            Case "Ausgabe"
                txt_von.Visibility = Visibility.Collapsed
                ctrl_datepickerbis.Visibility = Visibility.Collapsed
                opt_datepickerbis.Visibility = Visibility.Collapsed

                txt_bis.Visibility = Visibility.Collapsed
                ctrl_datepickervon.Visibility = Visibility.Collapsed
                opt_datepickervon.Visibility = Visibility.Collapsed

                txt_unterkategorie.Visibility = Visibility.Collapsed
                cbo_haushaltsunterkategorie.Visibility = Visibility.Collapsed

                Dim _hhkatesult As ObservableCollection(Of HsVerwSvc.Haushaltskategorie) = Await vlo_client.GetHaushaltskategorienAsync

                cbo_haushaltskategorie.ItemsSource = _hhkatesult.Where(Function(vlo_kategorie) (vlo_kategorie.ID = 2 Or vlo_kategorie.ID = 5))
                cbo_haushaltskategorie.SelectedValuePath = "ID"
                cbo_haushaltskategorie.SelectedIndex = 0

                cbo_haushaltsunterkategorie.ItemsSource = Nothing

            Case Else

        End Select

        vlo_client = Nothing

    End Sub

    Private Sub setVisibilitydatevon()
        ctrl_datepickervon.Visibility = Visibility.Visible
    End Sub

    Private Sub unsetVisibilitydatevon()
        ctrl_datepickervon.Visibility = Visibility.Collapsed
    End Sub

    Private Sub setVisibilitydatebis()
        ctrl_datepickervon.Visibility = Visibility.Visible
    End Sub

    Private Sub unsetVisibilitydatebis()
        ctrl_datepickervon.Visibility = Visibility.Collapsed
    End Sub

End Class

Public Class SearchParam

    Private _startdate As String = ""
    Private _enddate As String = ""
    Private _kat As Integer = 0
    Private _ukat As Integer = 0

    Public Sub New()

        MyBase.New
        Me.StartDatum = ""
        Me.EndDatum = ""
        Me.Kategorie = 0
        Me.Unterkategorie = 0

    End Sub

    Public Sub New(ByVal Optional startdate As String = "", ByVal Optional enddate As String = "", ByVal Optional kat As Integer = 0, ByVal Optional ukat As Integer = 0)

        Me.StartDatum = startdate
        Me.EndDatum = enddate
        Me.Kategorie = kat
        Me.Unterkategorie = ukat

    End Sub

    Public Property StartDatum As String
        Get
            Return _startdate
        End Get
        Set(value As String)
            _startdate = value
        End Set
    End Property

    Public Property EndDatum As String
        Get
            Return _enddate
        End Get
        Set(value As String)
            _enddate = value
        End Set
    End Property

    Public Property Kategorie As Integer
        Get
            Return _kat
        End Get
        Set(value As Integer)
            _kat = value
        End Set
    End Property

    Public Property Unterkategorie As Integer
        Get
            Return _ukat
        End Get
        Set(value As Integer)
            _ukat = value
        End Set
    End Property

End Class
