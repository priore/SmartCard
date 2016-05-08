Option Explicit On 

#Region " Imports "
Imports System.Drawing
Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Threading
#End Region

Namespace Priore.Controls

    Public Class SmartCard
        Inherits System.Windows.Forms.Timer

#Region " Costanti pubbliche "
        ' tipi di procotollo
        <Description("Type of Protocols.")> _
        Public Enum ProtocolTypeConstants
            PROTOCOL_T0 = SCARD_PROTOCOL_T0
            PROTOCOL_T1 = SCARD_PROTOCOL_T1
            PROTOCOL_RAW = SCARD_PROTOCOL_RAW
            PROTOCOL_DEFAULT = SCARD_PROTOCOL_DEFAULT
            PROTOCOL_ALL = SCARD_PROTOCOL_T0 Or SCARD_PROTOCOL_T1 Or SCARD_PROTOCOL_RAW
        End Enum

        ' tipi di CSC
        <Description("Types of CSC (Card Secret Code).")> _
        Public Enum CSCTypeConstants
            CSC_USER1 = 1
            CSC_USER2 = 2
            CSC_ADMIN = 3
        End Enum

        ' stati del lettore/carta
        <Description("SmartCard states.")> _
        Public Enum StateConstants
            STATE_ABSENT = SCARD_ABSENT
            STATE_NEGOTIABLE = SCARD_NEGOTIABLE
            STATE_POWERED = SCARD_POWERED
            STATE_PRESENT = SCARD_PRESENT
            STATE_SPECIFIC = SCARD_SPECIFIC
            STATE_SWALLOWED = SCARD_SWALLOWED
            STATE_UNKNOWN = SCARD_UNKNOWN
        End Enum

        <Description("Response of a command.")> _
        Public Enum StatusWordConstants
            SWORD_CARD_LOCKED = &H9840S
            SWORD_COMMAND_SUCCESS = &H9000S
            SWORD_CSC_LOCKED = &H6983S
            SWORD_DEBIT_NOT_AUTHORIZATION = &H9804S
            SWORD_FILE_DESCRIPTION_ERROR = &H6284S
            SWORD_FILE_NOTFOUND = &H6A82S
            SWORD_FILE_NOSPACE_AVAILABLE = &H6984S
            SWORD_FILE_NOTSELECTED = &H6985S
            SWORD_FILE_ORGANIZATION_NOTCOMPATIBLE = &H6981S
            SWORD_IADF_ERROR = &H6281S
            SWORD_INCORRECT_APP = &H6E00S
            SWORD_INCORRECT_LEN = &H6700S
            SWORD_INCORRECT_PARAM = &H6A80S
            SWORD_INCORRECT_SECRETCODE = &H6300S
            SWORD_INVALID_CSC_1 = &H63C1S
            SWORD_INVALID_CSC_2 = &H63C2S
            SWORD_INVALID_CSC_3 = &H63C3S
            SWORD_INVALID_DF = &H6283S
            SWORD_INVALID_FUNCTION = &H6A81S
            SWORD_INVALID_INSTRUCTION = &H6D00S
            SWORD_INVALID_PARAM = &H6B00S
            SWORD_INVALID_PURSE = &H9406S
            SWORD_INVALID_SECURITY = &H6982S
            SWORD_KEY_SELECTION_ERROR = &H6A88S
            SWORD_KEY_FILE_SELECTION_ERROR = &H9408S
            SWORD_NOT_AUTHORIZATION = &H9804S
            SWORD_NOT_ENOUGHT_MEMORY = &H6A84S
            SWORD_PURSE_BALANCE_ERROR = &H9100S
            SWORD_PURSE_REPLACE_BALANCE_ERROR = &H9102S
            SWORD_PURSE_SELECTION_ERROR = &H9404S
            SWORD_RECORD_NOTFOUND = &H6A83S
            SWORD_SECURE_COMMAND_SUCCESS = &H6103S
            SWORD_TEMP_TRANSACTION_ERROR = &H9820S
            SWORD_TRANSACTION_ERROR = &H6900S
            SWORD_UNKNOW_MODE = &H6581S
            SWORD_WRITE_ERROR = &H6581S
            SWORD_WRONG_CONTEXT = &H6400S
            SWORD_WRONG_LEN = &H6C40S
        End Enum

        ' codici di errore
        <Description("Type of errors.")> _
        Public Enum ErrorConstants
            ' Winscard
            ERROR_INTERNAL = SCARD_F_INTERNAL_ERROR
            ERROR_CANCELLED = SCARD_E_CANCELLED
            ERROR_INVALID_HANDLE = SCARD_E_INVALID_HANDLE
            ERROR_INVALID_PARAMETER = SCARD_E_INVALID_PARAMETER
            ERROR_INVALID_TARGET = SCARD_E_INVALID_TARGET
            ERROR_NO_MEMORY = SCARD_E_NO_MEMORY
            ERROR_WAITED_TOO_LONG = SCARD_F_WAITED_TOO_LONG
            ERROR_INSUFFICIENT_BUFFER = SCARD_E_INSUFFICIENT_BUFFER
            ERROR_UNKNOWN_READER = SCARD_E_UNKNOWN_READER
            ERROR_TIMEOUT = SCARD_E_TIMEOUT
            ERROR_SHARING_VIOLATION = SCARD_E_SHARING_VIOLATION
            ERROR_NO_SMARTCARD = SCARD_E_NO_SMARTCARD
            ERROR_UNKNOWN_CARD = &H8010000D
            ERROR_CANT_DISPOSE = SCARD_E_UNKNOWN_CARD
            ERROR_PROTOCOL_MISMATCH = SCARD_E_PROTO_MISMATCH
            ERROR_NOT_READY = SCARD_E_NOT_READY
            ERROR_INVALID_VALUE = SCARD_E_INVALID_VALUE
            ERROR_SYSTEM_CANCELLED = SCARD_E_SYSTEM_CANCELLED
            ERROR_COMM_ERROR = SCARD_F_COMM_ERROR
            ERROR_UNKNOWN_ERROR = SCARD_F_UNKNOWN_ERROR
            ERROR_INVALID_ATR = SCARD_E_INVALID_ATR
            ERROR_NOT_TRANSACTED = SCARD_E_NOT_TRANSACTED
            ERROR_READER_UNAVAILABLE = SCARD_E_READER_UNAVAILABLE
            ERROR_SHUTDOWN = SCARD_P_SHUTDOWN
            ERROR_PCI_TOO_SMALL = SCARD_E_PCI_TOO_SMALL
            ERROR_READER_UNSUPPORTED = SCARD_E_READER_UNSUPPORTED
            ERROR_DUPLICATE_READER = SCARD_E_DUPLICATE_READER
            ERROR_CARD_UNSUPPORTED = SCARD_E_CARD_UNSUPPORTED
            ERROR_NO_SERVICE = SCARD_E_NO_SERVICE
            ERROR_SERVICE_STOPPED = SCARD_E_SERVICE_STOPPED
            ERROR_UNSUPPORTED_CARD = SCARD_W_UNSUPPORTED_CARD
            ERROR_UNRESPONSIVE_CARD = SCARD_W_UNRESPONSIVE_CARD
            ERROR_UNPOWERED_CARD = SCARD_W_UNPOWERED_CARD
            ERROR_RESET_CARD = SCARD_W_RESET_CARD
            ERROR_REMOVED_CARD = SCARD_W_REMOVED_CARD
        End Enum
#End Region

#Region " Variabili e oggetti privati "
        Private m_ATR As String = vbNullString
        Private m_State As Integer = 0
        Private m_ReaderName As String = vbNullString
        Private m_ErrorNumber As Integer = 0
        Private m_ErrorDescription As String = vbNullString
        Private m_Connected As Boolean = False
        Private m_Interval As Integer = 1000
        Private m_Enabled As Boolean
        Private m_ProtocolType As ProtocolTypeConstants = ProtocolTypeConstants.PROTOCOL_ALL

        Private bLimited As Boolean = False
        Private hContext As Integer = 0
        Private hCard As Integer = 0
        Private lInOut As Integer = 0
        Private GCM As GCMClass

        Private rdrStatus As ReaderStatusControl = New ReaderStatusControl
#End Region

#Region " Eventi pubblici "
        <Description("Occurs when the smartcard is present in to reader.")>
        Public Event CardIn(ByVal Sender As Object)
        <Description("Occurs when the smartcard is removed from reader.")>
        Public Event CardOut(ByVal Sender As Object)
        <Description("Occurs when the smartcard is not put correctly in to reader or for other generic errors.")>
        Public Event Errors(ByVal Sender As Object, ByVal Number As Integer, ByVal Description As String)
#End Region

#Region " Costruttori/Distruttori "
        Public Sub New()
            MyBase.New()

            Dim ret As Integer
            Dim iProt As Integer

            ' imposta alcuni valori di default
            lInOut = SCARD_STATE_UNAWARE
            MyBase.Interval = m_Interval
            m_Enabled = MyBase.Enabled

            ' crea l'oggetto GemClubMemo
            GCM = New GCMClass

            ' recupera il primo lettore disponibile
            ret = FirstReader(m_ReaderName)
            SetError(ret)
            If ret <> SCARD_S_SUCCESS Then
                Exit Sub
            End If

            ' recupera lo stato attuale della carta
            ret = ReaderStatus(m_State, iProt, m_ATR, m_ReaderName)
            lInOut = m_State
        End Sub

        Protected Overloads Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                rdrStatus.StopTimer()
                rdrStatus = Nothing
                GCM = Nothing
            End If
            MyBase.Dispose(disposing)
        End Sub

        Protected Overrides Sub Finalize()
            Dispose()
            MyBase.Finalize()
        End Sub
#End Region

#Region " Funzioni private "
        Private Sub SetError(ByVal lError As ErrorConstants)
            ' routine per la gestione degli errori
            If lError <> SCARD_S_SUCCESS Then ' controlla se c'è stato un errore
                m_ErrorNumber = lError ' restituisce il codice dell'errore
                m_ErrorDescription = SCardErrDescription(lError) ' restituisce la descrizione dell'errore
                If lError = SCARD_W_REMOVED_CARD Then ' se la carta è stata rimossa
                    m_State = SCARD_ABSENT ' cambia lo stato
                Else ' se è un altro tipo di errore
                    If lError = SCARD_E_UNKNOWN_CARD Then m_State = SCARD_UNKNOWN ' carta non riconosciuta
                    RaiseEvent Errors(Me, m_ErrorNumber, m_ErrorDescription) ' genera l'evento
                End If
            Else
                m_ErrorNumber = SCARD_S_SUCCESS ' non cè stato errore
                m_ErrorDescription = vbNullString ' azzera le proprietà di errore
            End If
        End Sub
#End Region

#Region " Proprietà pubbliche "

        <Browsable(True), _
        Description("Return or set the current protocol type.")> _
        Public Property ProtocolType() As ProtocolTypeConstants
            Get
                Return m_ProtocolType
            End Get
            Set(ByVal Value As ProtocolTypeConstants)
                m_ProtocolType = Value
            End Set
        End Property

        <Browsable(True), _
        Description("Return or set the reader name.")> _
        Public Property ReaderName() As String
            Get
                Return m_ReaderName
            End Get
            Set(ByVal Value As String)
                lInOut = -1
                m_ReaderName = Value
                rdrStatus.ReaderName = Value
            End Set
        End Property

        <Browsable(True), _
        Description("Return the smartcard state.")> _
        Public ReadOnly Property State() As StateConstants
            Get
                SetError(rdrStatus.Result)
                m_State = rdrStatus.Status
                Return m_State
            End Get
        End Property

        <Browsable(True), _
        Description("Return the ATR of the current smartcard inserted in to reader.")> _
        Public ReadOnly Property ATR() As String
            Get
                SetError(rdrStatus.Result)
                m_ATR = rdrStatus.ATR
                Return m_ATR
            End Get
        End Property

        <Browsable(True), _
        Description("Return the error number.")> _
        Public ReadOnly Property ErrorNumber() As ErrorConstants
            Get
                Return m_ErrorNumber
            End Get
        End Property

        <Browsable(True), _
        Description("Return the error description.")> _
        Public ReadOnly Property ErrorDescription() As String
            Get
                Return m_ErrorDescription
            End Get
        End Property

        <Browsable(True), _
        Description("Return the command response.")> _
        Public ReadOnly Property StatusWord() As StatusWordConstants
            Get
                Return modSmartCard.StatusWord
            End Get
        End Property

        <Browsable(True), _
        Description("Return if the reader is conected to the smartcard.")> _
        Public ReadOnly Property Connected() As Boolean
            Get
                Return m_Connected
            End Get
        End Property

        <Browsable(True), _
        Description("Disable/Enable the timer for verify the status of the smartcard.")> _
        Public Shadows Property Enabled() As Boolean
            Get
                Return m_Enabled
            End Get
            Set(ByVal Value As Boolean)
                m_Enabled = Value
                With rdrStatus
                    .Enabled = Value
                    If Value Then .Start() Else .StopTimer()
                End With
                MyBase.Enabled = Value
            End Set
        End Property

        <Browsable(True), _
        Description("Return or set the interval to verify the smartcard presence.")> _
        Public Shadows Property Interval() As Integer
            Get
                Return m_Interval
            End Get
            Set(ByVal Value As Integer)
                m_Interval = Value
                MyBase.Interval = Value
                rdrStatus.Interval = Value
            End Set
        End Property
#End Region

#Region " Funzioni pubbliche "

        <Description("Connect to current smartcard inserted in to reader.")> _
        Public Function Connect() As Integer
            Dim ret As Integer
            Dim iProt As Integer

            ' blocca l'intercettamento dello stato
            rdrStatus.StopTimer()

            ' stabilisce un contesto con il dispositivo
            ret = SCardEstablishContext(SCARD_SCOPE_USER, &H0S, &H0S, hContext)
            If ret <> SCARD_S_SUCCESS Then GoTo No_Connect
            ' si connette alla carta
            ret = SCardConnect(hContext, m_ReaderName, SCARD_SHARE_SHARED, m_ProtocolType, hCard, iProt)
            If ret <> SCARD_S_SUCCESS Then GoTo No_Connect
            m_Connected = True              ' imposta la proprietà Connected
            rdrStatus.CardHandle = hCard    ' passa l'handle della carta connessa
            rdrStatus.Connected = True      ' indica che deve verificare la carta già connessa
            rdrStatus.Start()               ' riavvia l'intercettamento dello stato
            Return StatusWordConstants.SWORD_COMMAND_SUCCESS

No_Connect:
            SCardFreeMemory(hContext, m_ReaderName) ' libera la memoria
            Call SCardReleaseContext(hContext)      ' libera il contesto
            m_Connected = False                     ' imposta la proprietà Connected
            SetError(ret)                           ' imposta l'errore
            rdrStatus.Connected = False             ' stato carta non connessa
            rdrStatus.Start()                       ' riavvia l'intercettamento dello stato
            Return ret                              ' restituisce l'errore
        End Function

        <Description("Disconnect reader from smartcard.")> _
        Public Function Disconnect() As Integer
            Dim ret As Integer

            ret = SCardDisconnect(hCard, SCARD_UNPOWER_CARD)    ' disconnette la carta
            SCardFreeMemory(hContext, m_ReaderName)             ' libera la memoria
            Call SCardReleaseContext(hContext)                  ' libera il contesto
            SetError(ret)                                       ' imposta eventuale errore
            m_Connected = False                                 ' imposta la proprietà Connected
            rdrStatus.Connected = False                         ' stato non connesso
            rdrStatus.Start()                                   ' riavvia l'intercettamento dello stato
            Return ret                                          ' restituisce il risultato
        End Function

        <Description("Read data from the current smartcard.")> _
        Public Overridable Function ReadCard() As String
            'If bLimited Then Return vbNullString
            Return GCM.ReadCard(hCard)
        End Function

        Public Overridable Function ReadCard(ByVal iStartAddr As Integer) As String
            'If bLimited Then Return vbNullString
            Return GCM.ReadCard(hCard, iStartAddr)
        End Function

        Public Overridable Function ReadCard(ByVal iStartAddr As Integer, ByVal iEndAddr As Integer) As String
            'If bLimited Then Return vbNullString
            Return GCM.ReadCard(iStartAddr, iEndAddr)
        End Function

        <Description("Write data in to current smartcard.")> _
        Public Overridable Function WriteCard(ByVal sData As String) As Integer
            'If bLimited Then Return ErrorConstants.ERROR_CANCELLED
            Return GCM.WriteCard(hCard, sData)
        End Function

        Public Overridable Function WriteCard(ByVal iAddr As Integer, ByVal sData As String) As Integer
            'If bLimited Then Return ErrorConstants.ERROR_CANCELLED
            Return GCM.WriteCard(hCard, sData, iAddr)
        End Function

        <Description("Send the CSC to the current smartcard.")> _
        Public Overridable Function SendCSC(ByVal sCSC As String) As Integer
            'If bLimited Then Return ErrorConstants.ERROR_CANCELLED
            Return GCM.SendCSC(hCard, sCSC)
        End Function

        Public Overridable Function SendCSC(ByVal sCSC As String, ByVal iCSCType As CSCTypeConstants) As Integer
            Return GCM.SendCSC(hCard, iCSCType, sCSC)
        End Function

        <Description("Change the CSC of the current smartcard.")> _
        Public Overridable Function SetCSC(ByVal sNewCSC As String) As Integer
            Return GCM.SetCSC(hCard, sNewCSC)
        End Function

        Public Overridable Function SetCSC(ByVal sNewCSC As String, ByVal iCSCType As CSCTypeConstants) As Integer
            Return GCM.SetCSC(hCard, iCSCType, sNewCSC)
        End Function

        <Description("Verify the CSC.")> _
        Public Overridable Function VerifyCSC(ByVal sCSC As String, ByVal iCSCType As CSCTypeConstants) As Integer
            Return GCM.VerifyCSC(hCard, iCSCType, sCSC)
        End Function

        <Description("Send a reset to card (disconnect/reconnect).")>
        Public Overridable Sub Reset()
            If hCard <> 0 And m_Connected Then
                Me.Disconnect()
                Me.Connect()
            End If
        End Sub

        <Description("Return string array with list of readers installed in to system.")>
        Public Function Readers() As String()
            Return ReaderList()
        End Function

        <Description("Send directly the APDU commands.")> _
        Public Function APDUCommand(ByVal iCLA As Byte, ByVal iINS As Byte, ByVal iP1 As Byte, ByVal iP2 As Byte, ByVal iLen As Byte, ByRef aSendData() As Byte, ByRef aRecvData() As Byte) As Long

            Dim lSW0 As Integer
            Dim lSW1 As Integer
            Dim ret As Integer
            Dim aSend() As Byte = {iCLA, iINS, iP1, iP2, iLen}

            Try
                Dim lSLen As Short = aSendData.Length   ' se ci sono dati da inviare 
                ReDim Preserve aSend(4 + lSLen)         ' ridimensiona il buffer di output +4 (0 to 4) per l'APDU
                For i As Short = 0 To lSLen - 1         ' cicla per la qta di bytes aggiuntivi
                    aSend(5 + i) = aSendData(i)         ' li aggiunge al buffer di output
                Next
            Catch ex As Exception
                ' se non ci sono dati aggiuntivi da inviare non fa niente
            End Try

            Try
                Dim lRLen As Short = aRecvData.Length   ' se ci sono dati da ricevere
                ReDim aRecvData(lRLen + 1)              ' ridimensiona per aggiungere lo StatusWord
            Catch ex As Exception                       ' se non è stato ridim neanche questo buffer
                ReDim aRecvData(1)                      ' ridimensiona a 2 (0 to 1) per lo statusword
            End Try

            ' invia il comando alla carta
            ret = SCardIO(hCard, aSend, aRecvData, lSW0, lSW1)
            SetError(ret)               ' restituisce ventuali errori
            APDUCommand = ret           ' restituisce lo stato dell'operazione
        End Function

#End Region

#Region " Eventi degli oggetti privati "

        Protected Overrides Sub OnTick(ByVal e As System.EventArgs)

            MyBase.OnTick(e)
            m_State = rdrStatus.Status          ' recupera lo stato della smartcard
            SetError(rdrStatus.Result)          ' imposta eventuale errore
            If m_State <> lInOut Then           ' controlla se non c'è stato errore
                lInOut = m_State                ' memorizza lo stato attuale
                Select Case m_State             ' controlla lo stato
                    Case SCARD_ABSENT           ' carta assente o rimossa
                        If m_Connected Then Disconnect()
                        RaiseEvent CardOut(Me)  ' genera evento CardOut
                    Case Else                   ' carta presente o inserita
                        RaiseEvent CardIn(Me)   ' genera evento CardIn
                End Select
            End If
        End Sub
#End Region

    End Class
End Namespace
