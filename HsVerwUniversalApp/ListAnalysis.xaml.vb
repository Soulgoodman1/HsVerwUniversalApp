Imports HsVerwUniversalApp.MainHub
' Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
''' 

Public NotInheritable Class ListAnalysis
    Inherits Page

    Private _userdatacontextanalysisexplicit As UserDataContextAnalysisExplicit

    Public Sub New()
        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    End Sub

    Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)

        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim vlo_ausgabeid As Long = CLng(e.Parameter)

        Dim _hhausgresult As HsVerwSvc.Ausgabe = Await vlo_client.GetAusgabebyIDAsync(vlo_ausgabeid)
        Me.DataContext = _hhausgresult

        Dim _hhrhtresult As ObservableCollection(Of HsVerwSvc.Zahlungsrythmus) = Await vlo_client.GetZahlungsrythmenAsync

        'cbo_zahlungsrythmus.ItemsSource = _hhrhtresult
        'cbo_zahlungsrythmus.SelectedValuePath = "ID"
        'cbo_zahlungsrythmus.SelectedIndex = 0
        'cbo_zahlungsrythmus.SelectedValue = _hhausgresult.ZahlungsrythmusID

        vlo_client = Nothing

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
