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

        cbo_verbrauchsart.ItemsSource = _hhukatresult
        cbo_verbrauchsart.SelectedValuePath = "ID"
        'cbo_verbrauchsart.SelectedIndex = 0

        Dim _hhjahreresult As ObservableCollection(Of String) = Await vlo_client.GetAnalyseJahreAsync

        _hhjahreresult.Add("Alle")
        cbo_Jahr.ItemsSource = _hhjahreresult
        'cbo_Jahr.SelectedIndex = 0

        vlo_client = Nothing

    End Sub

    Private Sub cbo_Jahr_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

        Dim vlo_jahr As String = e.AddedItems(0).ToString

        ListviewConsumption.ItemsSource = _resultverbrauch.Where(Function(vlo_verbrauch) (vlo_verbrauch.Datum > CDate("01.01." & vlo_jahr)) And (vlo_verbrauch.Datum < CDate("01.02." & (CInt(vlo_jahr) + 1).ToString))).OrderBy(Function(vlo_verbrauch) vlo_verbrauch.Datum)

    End Sub

    Private Sub cbo_verbrauchsart_SelectionChanged(sender As Object, e As SelectionChangedEventArgs)

        Dim vlo_haushaltsunterkategorie As HsVerwSvc.IService1Verbrauchstyp = e.AddedItems(0)

        ListviewConsumption.ItemsSource = _resultverbrauch.Where(Function(vlo_verbrauch) (vlo_verbrauch.HaushaltsunterkategorieID = vlo_haushaltsunterkategorie.ID))


    End Sub
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
