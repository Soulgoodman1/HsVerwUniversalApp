Imports HsVerwUniversalApp.MainHub
' Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
''' 

Public NotInheritable Class ListAnalysis
    Inherits Page
    Private _resultverbrauch As ObservableCollection(Of HsVerwSvc.Verbrauch)
    Private _userdatacontextanalysisexplicit As UserDataContextAnalysisExplicit
    Private _jahr As String = "Alle"
    Private _monat As String = "Alle"
    Private _ukat As Long = 0
    Private _filtern As System.Func(Of HsVerwSvc.Verbrauch, Boolean) = AddressOf Verbrauchfiltern
    Private _initialisierung As Boolean = True

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.


    End Sub

    Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)

        Dim vlo_client As New HsVerwSvc.Service1Client
        'Dim vlo_ausgabeid As Long = CLng(e.Parameter)

        _resultverbrauch = Await vlo_client.GetVerbrauchAsync

        ListviewConsumption.ItemsSource = _resultverbrauch

        Dim _hhukatresult As ObservableCollection(Of HsVerwSvc.IService1Verbrauchstyp) = Await vlo_client.GetVerbrauchsTypAsync(1)

        Dim _alle As New HsVerwSvc.IService1Verbrauchstyp
        _alle.ID = 0
        _alle.Haushaltsunterkategorie = "Alle"
        _hhukatresult.Add(_alle)
        '"Alle" nach oben schieben
        _hhukatresult.Move(_hhukatresult.Count - 1, 0)

        cbo_Verbrauchsart.ItemsSource = _hhukatresult
        cbo_verbrauchsart.SelectedValuePath = "ID"
        cbo_Verbrauchsart.SelectedIndex = 0

        Dim _hhjahreresult As ObservableCollection(Of String) = Await vlo_client.GetAnalyseJahreAsync

        _hhjahreresult.Add("Alle")
        '"Alle" nach oben schieben
        _hhjahreresult.Move(_hhjahreresult.Count - 1, 0)
        cbo_Jahr.ItemsSource = _hhjahreresult
        cbo_Jahr.SelectedIndex = 0

        Dim _hhmonateresult As New ObservableCollection(Of String)
        _hhmonateresult.Add("Alle")
        _hhmonateresult.Add("Januar")
        _hhmonateresult.Add("Februar")
        _hhmonateresult.Add("März")
        _hhmonateresult.Add("April")
        _hhmonateresult.Add("Mai")
        _hhmonateresult.Add("Juni")
        _hhmonateresult.Add("Juli")
        _hhmonateresult.Add("August")
        _hhmonateresult.Add("September")
        _hhmonateresult.Add("Oktober")
        _hhmonateresult.Add("November")
        _hhmonateresult.Add("Dezember")

        cbo_Monat.ItemsSource = _hhmonateresult
        cbo_Monat.SelectedIndex = 0

        'Erstinitialisierung abgeschlossen
        _initialisierung = False

        vlo_client = Nothing

    End Sub

    Private Sub cbo_Jahr_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

        'Nur nach Erstinitialisierung filtern
        If _initialisierung Then Exit Sub

        _jahr = e.AddedItems(0).ToString

        ListviewConsumption.ItemsSource = _resultverbrauch.Where(_filtern)
    End Sub

    Private Sub cbo_Monat_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

        'Nur nach Erstinitialisierung filtern
        If _initialisierung Then Exit Sub

        _monat = e.AddedItems(0).ToString

        ListviewConsumption.ItemsSource = _resultverbrauch.Where(_filtern)

    End Sub

    Private Sub cbo_verbrauchsart_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

        'Nur nach Erstinitialisierung filtern
        If _initialisierung Then Exit Sub

        _ukat = CType(e.AddedItems(0), HsVerwSvc.IService1Verbrauchstyp).ID

        ListviewConsumption.ItemsSource = _resultverbrauch.Where(_filtern)

    End Sub

    Private Function Verbrauchfiltern(ByVal vlo_verbrauch As HsVerwSvc.Verbrauch) As Boolean
        Dim vlo_filter As Boolean = False
        Dim vlo_filter_jahr As Boolean = False
        Dim vlo_filter_monat As Boolean = False
        Dim vlo_filter_ukat As Boolean = False

        If _jahr <> "Alle" Then
            If vlo_verbrauch.Datum > CDate("01.01." & _jahr) And vlo_verbrauch.Datum < CDate("01.02." & (CInt(_jahr) + 1).ToString) Then
                vlo_filter_jahr = True
            Else
                vlo_filter_jahr = False
            End If
        Else
            vlo_filter_jahr = True
        End If

        If _monat <> "Alle" Then
            If vlo_verbrauch.Monat = _monat Then
                vlo_filter_monat = True
            Else
                vlo_filter_monat = False
            End If
        Else
            vlo_filter_monat = True
        End If

        If _ukat <> 0 Then
            If vlo_verbrauch.HaushaltsunterkategorieID = _ukat Then
                vlo_filter_ukat = True
            Else
                vlo_filter_ukat = False
            End If
        Else
            vlo_filter_ukat = True
        End If

        If vlo_filter_jahr And vlo_filter_monat And vlo_filter_ukat Then
            vlo_filter = True
        End If

        Return vlo_filter

    End Function

End Class

Public Class UserDataContextAnalysisExplicit
    Inherits HsVerwSvc.Auswertung
    Implements INotifyPropertyChanged

    Public Shadows Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    Private _sectionwidth As Double

    Public Sub New()
        MyBase.New
    End Sub

    Public Sub New(ByVal vlo_auswertung As HsVerwSvc.Auswertung)

        Me.AusgabenFixproJahr = vlo_auswertung.AusgabenFixproJahr
        Me.AusgabenFixproMonat = vlo_auswertung.AusgabenFixproMonat
        Me.AusgabenVarproJahr = vlo_auswertung.AusgabenVarproJahr
        Me.AusgabenVarproMonat = vlo_auswertung.AusgabenVarproMonat
        Me.AuswertungproJahr = vlo_auswertung.AuswertungproJahr
        Me.AuswertungproMonat = vlo_auswertung.AuswertungproMonat
        Me.EinnahmenproJahr = vlo_auswertung.EinnahmenproJahr
        Me.EinnahmenproMonat = vlo_auswertung.EinnahmenproMonat
        Me.VerbrauchFixproJahr = vlo_auswertung.VerbrauchFixproJahr
        Me.VerbrauchFixproMonat = vlo_auswertung.VerbrauchFixproMonat
        Me.VerbrauchVarproJahr = vlo_auswertung.VerbrauchVarproJahr
        Me.VerbrauchVarproMonat = vlo_auswertung.VerbrauchVarproMonat

    End Sub

    Public Property SectionWidth As Double

        Get
            Return _sectionwidth
        End Get
        Set(ByVal Value As Double)
            _sectionwidth = Value
            Me.OnPropertyChanged("SectionWidth")
        End Set

    End Property

    Protected Sub OnPropertyChanged(ByVal propertyName As String)
        If Not PropertyChangedEvent Is Nothing Then
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End If
    End Sub

End Class
