' Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

''' <summary>
''' Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
''' </summary>
Public NotInheritable Class MainHub
    Inherits Page

    Private _userdatacontext As UserDataContext

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        Dim vlo_client As New HsVerwSvc.Service1Client
        Dim _hhauswertung As Task(Of HsVerwSvc.Auswertung) = vlo_client.GetAuswertungAsync

        _userdatacontext = New UserDataContext(_hhauswertung.Result)
        _userdatacontext.SectionWidth = Window.Current.Bounds.Width

        Me.DataContext = _userdatacontext
    End Sub

    Private Sub App_BackHardware(sender As Object, e As Windows.Phone.UI.Input.BackPressedEventArgs)

        If Frame.CanGoBack Then
            e.Handled = True
            Frame.GoBack()
        End If

    End Sub

    Private Sub App_Backrequested(sender As Object, e As Windows.UI.Core.BackRequestedEventArgs)

        If Frame.CanGoBack Then
            e.Handled = True
            Frame.GoBack()
        End If

    End Sub

    Private Sub MainHub_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        If Frame.CanGoBack Then
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible
        Else
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Collapsed
        End If

        'Falls bereits Handler vorhanden zuerst löschen, sonst mehrfaches Auslösen des BackPressed
        RemoveHandler Windows.Phone.UI.Input.HardwareButtons.BackPressed, AddressOf App_BackHardware
        RemoveHandler Windows.UI.Core.SystemNavigationManager.GetForCurrentView.BackRequested, AddressOf App_Backrequested

        If Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons") Then
            AddHandler Windows.Phone.UI.Input.HardwareButtons.BackPressed, AddressOf App_BackHardware
        Else
            AddHandler Windows.UI.Core.SystemNavigationManager.GetForCurrentView.BackRequested, AddressOf App_Backrequested
        End If

    End Sub

    Private Sub hinzufuegenEinnahme()

    End Sub

    Private Sub auflistenEinnahme(sender As Object, e As RoutedEventArgs)

        Frame.Navigate(GetType(ListIncome))

    End Sub

    Private Sub hinzufuegenAusgabe()

    End Sub

    Private Sub auflistenAusgabe(sender As Object, e As RoutedEventArgs)

        Frame.Navigate(GetType(ListExpense))

    End Sub

    Private Sub hinzufuegenVerbrauch()

    End Sub

    Private Sub auflistenVerbrauch(sender As Object, e As RoutedEventArgs)

        Frame.Navigate(GetType(ListConsumption))

    End Sub
    Private Sub auflistenAuswertung(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub Page_SizeChanged(sender As Object, e As SizeChangedEventArgs) Handles Me.SizeChanged

        _userdatacontext.SectionWidth = Window.Current.Bounds.Width

    End Sub

    Private Sub HausverwaltungHub_SectionsInViewChanged(sender As Object, e As SectionsInViewChangedEventArgs) Handles HausverwaltungHub.SectionsInViewChanged

        Try
            Select Case e.AddedSections(0).Header
                Case "Einnahmen"

                    btn_hinzufuegenEinnahme.Visibility = Visibility.Visible
                    btn_auflistenEinnahme.Visibility = Visibility.Visible
                    btn_hinzufuegenVerbrauch.Visibility = Visibility.Collapsed
                    btn_auflistenVerbrauch.Visibility = Visibility.Collapsed
                    btn_hinzufuegenAusgabe.Visibility = Visibility.Collapsed
                    btn_auflistenAusgabe.Visibility = Visibility.Collapsed
                    btn_auflistenAuswertung.Visibility = Visibility.Collapsed

                Case "Ausgaben"

                    btn_hinzufuegenEinnahme.Visibility = Visibility.Collapsed
                    btn_auflistenEinnahme.Visibility = Visibility.Collapsed
                    btn_hinzufuegenVerbrauch.Visibility = Visibility.Collapsed
                    btn_auflistenVerbrauch.Visibility = Visibility.Collapsed
                    btn_hinzufuegenAusgabe.Visibility = Visibility.Visible
                    btn_auflistenAusgabe.Visibility = Visibility.Visible
                    btn_auflistenAuswertung.Visibility = Visibility.Collapsed

                Case "Verbrauch"

                    btn_hinzufuegenEinnahme.Visibility = Visibility.Collapsed
                    btn_auflistenEinnahme.Visibility = Visibility.Collapsed
                    btn_hinzufuegenVerbrauch.Visibility = Visibility.Visible
                    btn_auflistenVerbrauch.Visibility = Visibility.Visible
                    btn_hinzufuegenAusgabe.Visibility = Visibility.Collapsed
                    btn_auflistenAusgabe.Visibility = Visibility.Collapsed
                    btn_auflistenAuswertung.Visibility = Visibility.Collapsed

                Case "Auswertung"

                    btn_hinzufuegenEinnahme.Visibility = Visibility.Collapsed
                    btn_auflistenEinnahme.Visibility = Visibility.Collapsed
                    btn_hinzufuegenVerbrauch.Visibility = Visibility.Collapsed
                    btn_auflistenVerbrauch.Visibility = Visibility.Collapsed
                    btn_hinzufuegenAusgabe.Visibility = Visibility.Collapsed
                    btn_auflistenAusgabe.Visibility = Visibility.Collapsed
                    btn_auflistenAuswertung.Visibility = Visibility.Visible

            End Select

        Catch ex As Exception

        End Try

    End Sub

    Public Class UserDataContext
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


End Class
