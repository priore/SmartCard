Imports System.Threading

Friend Class ReaderStatusControl

    Friend ATR As String = String.Empty
    Friend Result As Integer = SCARD_S_SUCCESS
    Friend ReaderName As String = String.Empty
    Friend Status As Integer = SCARD_STATE_UNAWARE
    Friend Interval As Integer = 1000
    Friend Connected As Boolean = False
    Friend CardHandle As Integer
    Friend Enabled As Boolean = False

    Private aTimer As Timer
    Private Prot As Integer
    Private tCallback As TimerCallback
    Private autoEvt As AutoResetEvent

    Friend Sub Start()
        StopTimer()
        If Enabled Then
            autoEvt = New AutoResetEvent(False)
            tCallback = New TimerCallback(AddressOf GetReaderStatus)
            aTimer = New Timer(tCallback, autoEvt, 0, Interval)
            autoEvt.Set()
        End If
    End Sub

    Friend Sub StopTimer()
        If Not aTimer Is Nothing Then
            autoEvt.WaitOne()
            aTimer.Dispose()
            TimerCallback.RemoveAll(tCallback, Nothing)
            aTimer = Nothing
            tCallback = Nothing
            autoEvt = Nothing
        End If
    End Sub

    Private Sub GetReaderStatus(ByVal state As Object)
        If Enabled Then
            ' se non è connesso e il nome del lettore è valido
            If Not Connected And ReaderName.Length > 0 Then
                Dim autoEvent As AutoResetEvent = DirectCast(state, AutoResetEvent)
                autoEvt.WaitOne()
                Result = ReaderStatus(Status, Prot, ATR, ReaderName)
                autoEvent.Set()
            ElseIf Connected And CardHandle <> 0 Then
                ' se è connesso e l'handle della carta è valido
                Dim autoEvent As AutoResetEvent = DirectCast(state, AutoResetEvent)
                autoEvt.WaitOne()
                Result = SCardStatus(CardHandle, Status)
                autoEvent.Set()
            End If
        End If
    End Sub
End Class
