Option Strict Off
Option Explicit On

Friend Class GCMClass

    ' Comandi (SetCommand procedure)
    Private Const SEND_CSC_USER1 As Short = 1 ' (NOTE: uguale in CSCTypeConstants)
    Private Const SEND_CSC_USER2 As Short = 2 ' (NOTE: uguale in CSCTypeConstants)
    Private Const SEND_CSC_ADMIN As Short = 3 ' (NOTE: uguale in CSCTypeConstants)
    Private Const READ_CARD As Short = 5
    Private Const WRITE_CARD As Short = 7
    Private Const VERIFY_CSC As Short = 8

    ' Errori
    Private Const SW_INVALID_CSC_1 As Short = &H63C1S
    Private Const SW_INVALID_CSC_2 As Short = &H63C2S
    Private Const SW_INVALID_CSC_3 As Short = &H63C3S

    ' GCM indirizzi vari
    Private Const ADDR_CSC_USER1 As Byte = &H3AS
    Private Const ADDR_CSC_USER2 As Byte = &H38S
    Private Const ADDR_CSC_ADMIN As Byte = &H6S

    ' GCM comandi APDU (CLA, INS, P1, P2)
    Private Const GCM_CSC_USER1 As String = "00200039"
    Private Const GCM_CSC_USER2 As String = "0020003B"
    Private Const GCM_CSC_ADMIN As String = "00200007"
    Private Const GCM_READ_CARD As String = "80BE0000"
    Private Const GCM_WRITE_CARD As String = "80DE0000"
    Private Const GCM_VERIFY_CSC As String = "00200000"

    ' GCM ATR
    Private Const GCM_ATR As String = "3B025201"
    Private Const GCM_ATRMask As String = "FFFFFFFF"

    ' GCM lunghezza dei dati
    Private Const GCM_LEN As Byte = &H4S

    ' Chiave nel registro di Window sper la registrazione dell'ATR della GemClubMemo
    Private Const REG_GCM As String = "GemClubMemo"

    ' Variaibli varie
    Private m_CLA As Byte
    Private m_INS As Byte
    Private m_P1 As Byte
    Private m_P2 As Byte
    Private m_SW1 As Byte
    Private m_SW2 As Byte

    Private Sub SetCommand(ByVal iFunction As Short)
        Const HexChar As String = "&H"
        Dim sHex As String = String.Empty
        Select Case iFunction ' controlla quale operazione deve fare
            Case SEND_CSC_USER1
                sHex = GCM_CSC_USER1 ' invio CSC User Area 1
            Case SEND_CSC_USER2
                sHex = GCM_CSC_USER2 ' invio CSC User Area 2
            Case SEND_CSC_ADMIN
                sHex = GCM_CSC_ADMIN ' invio CSC Admin
            Case READ_CARD
                sHex = GCM_READ_CARD ' lettura carta
            Case WRITE_CARD
                sHex = GCM_WRITE_CARD ' scrittura carta
            Case VERIFY_CSC
                sHex = GCM_VERIFY_CSC ' verifica del CSC
        End Select
        m_CLA = CByte(HexChar & Mid(sHex, 1, 2)) ' classe di comando
        m_INS = CByte(HexChar & Mid(sHex, 3, 2)) ' comando
        m_P1 = CByte(HexChar & Mid(sHex, 5, 2)) ' parametro 1
        m_P2 = CByte(HexChar & Mid(sHex, 7, 2)) ' parametro 2
    End Sub

    Friend ReadOnly Property ATR() As String
        Get
            Return GCM_ATR
        End Get
    End Property

    Friend ReadOnly Property ATRMask() As String
        Get
            Return GCM_ATRMask
        End Get
    End Property

    Friend ReadOnly Property StatusWord() As Integer
        Get
            Return GetStatusWord(m_SW1, m_SW2)
        End Get
    End Property

    Friend Overridable Function ReadCard(ByVal hCard As Integer) As String
        Dim sz1 As String
        Dim sz2 As String
        Dim i As Short
        Dim ret As Integer
        Dim sRecv As String

        sz1 = vbNullString ' svuota il buffer usato per l'User Area 1
        sz2 = vbNullString ' svuota il buffer usato per l'User Area 2
        Call SetCommand(READ_CARD) ' imposta i comandi per la carta
        For i = &H10S To &H1FS ' word nella User Area 1
            m_P2 = i ' imposta l'indirizzo per l'User Area 1
            sRecv = vbNullString ' inizializza la stringa buffer
            ret = SCardRead(hCard, m_CLA, m_INS, m_P1, m_P2, GCM_LEN, sRecv, m_SW1, m_SW2)
            If ret <> SCARD_S_SUCCESS Or GetStatusWord(m_SW1, m_SW2) <> SWORD_E_SUCCESS Then Exit For
            sz1 = sz1 & sRecv ' compone la stringa per la User Area 1
            m_P2 = i + &H18S ' imposta l'indirizzo per l'User Area 2
            sRecv = vbNullString ' inizializza la stringa buffer
            ret = SCardRead(hCard, m_CLA, m_INS, m_P1, m_P2, GCM_LEN, sRecv, m_SW1, m_SW2)
            If ret <> SCARD_S_SUCCESS Or GetStatusWord(m_SW1, m_SW2) <> SWORD_E_SUCCESS Then Exit For
            sz2 = sz2 & sRecv ' compone la stringa per la User Area 2
        Next
        Return sz1 & sz2 ' restituisce i dati
    End Function

    Friend Overridable Function ReadCard(ByVal hCard As Integer, ByVal iStartAddr As Byte) As String
        Dim ret As Integer
        Dim sRecv As String = String.Empty

        Call SetCommand(READ_CARD) ' imposta i comandi per la carta
        m_P2 = CByte(iStartAddr) ' imposta l'indirizzo
        ' recupera solo 4 charatteri dall'indirizzo specificato in iStartAddr
        ret = SCardRead(hCard, m_CLA, m_INS, m_P1, m_P2, GCM_LEN, sRecv, m_SW1, m_SW2)
        Return sRecv
    End Function

    Friend Overridable Function ReadCard(ByVal hCard As Integer, ByVal iStartAddr As Byte, ByVal iEndAddr As Byte) As String
        Dim i As Short
        Dim ret As Integer
        Dim sChrs As String = String.Empty
        Dim sRecv As String

        sRecv = vbNullString ' svuota il buffer di output
        Call SetCommand(READ_CARD) ' imposta i comandi per la carta
        For i = iStartAddr To iEndAddr ' cicla su tutto il range degli indirizzi
            m_P2 = CByte(i) ' imposta l'indirizzo
            ' recupera 4 charatteri per volta fino alla fine specificata in iEndAddr
            ret = SCardRead(hCard, m_CLA, m_INS, m_P1, m_P2, GCM_LEN, sChrs, m_SW1, m_SW2)
            If ret <> SCARD_S_SUCCESS Or GetStatusWord(m_SW1, m_SW2) <> SWORD_E_SUCCESS Then Exit For
            sRecv = sRecv & sChrs ' compone il buffer di lettura
        Next
        Return sRecv ' restituisce i dati recuperati
    End Function

    Friend Overridable Function WriteCard(ByVal hCard As Integer, ByVal sData As String) As Integer
        Dim c As Short
        Dim i As Short
        Dim ret As Integer

        c = 1 ' inizializza la puntatore di partenza della sub-stringa
        sData = Mid(sData, 1, 128) ' limita a 128 charatteri (la lunghezza massima delle due User Aree)
        Do While Len(sData) < 128 ' aggiunge i caratteri mancanti
            sData = sData & vbNullChar
        Loop
        SetCommand(WRITE_CARD)
        For i = &H10S To &H1FS ' words nella User Area 1
            m_P2 = i ' imposta l'indirizzo per l'User Area 1
            ret = SCardSend(hCard, m_CLA, m_INS, m_P1, m_P2, Mid(sData, c, 4), m_SW1, m_SW2)
            If ret <> SCARD_S_SUCCESS Or GetStatusWord(m_SW1, m_SW2) <> SWORD_E_SUCCESS Then Exit For
            m_P2 = i + &H18S ' imposta l'indirizzo per l'User Area 2
            ret = SCardSend(hCard, m_CLA, m_INS, m_P1, m_P2, Mid(sData, c + 64, 4), m_SW1, m_SW2)
            If ret <> SCARD_S_SUCCESS Or GetStatusWord(m_SW1, m_SW2) <> SWORD_E_SUCCESS Then Exit For
            c = c + 4 ' incrementa il puntatore della sub-stringa
        Next
        Return ret
    End Function

    Friend Overridable Function WriteCard(ByVal hCard As Integer, ByVal sData As String, ByVal iAddr As Byte) As Integer
        Dim i As Short
        Dim ret As Integer

        Do While Len(sData) < 4 ' aggiunge i caratteri mancanti
            sData = sData & vbNullChar
        Loop
        SetCommand(WRITE_CARD)
        m_P2 = CByte(iAddr) ' imposta il secondo parametro
        For i = 1 To Len(sData) Step 4 ' cicla per tutta la lunghezza dei dati da scrivere
            ' scrive i dati 4 charatteri per volta
            ret = SCardSend(hCard, m_CLA, m_INS, m_P1, m_P2, Mid(sData, i, 4), m_SW1, m_SW2)
            If ret <> SCARD_S_SUCCESS Or GetStatusWord(m_SW1, m_SW2) <> SWORD_E_SUCCESS Then Exit For
        Next
        Return ret
    End Function

    Friend Overridable Function VerifyCSC(ByVal hCard As Integer, ByVal iCSCType As Integer, ByVal sCSC As String) As Integer
        Dim ret As Integer

        If Len(sCSC) = 0 Then ' se non è specificato il CSC
            Select Case iCSCType ' controlla il tipo di CSC
                Case SEND_CSC_USER1 : ret = SW_INVALID_CSC_1
                Case SEND_CSC_USER2 : ret = SW_INVALID_CSC_2
                Case SEND_CSC_ADMIN : ret = SW_INVALID_CSC_3
            End Select
            Return ret ' restituisce l'errore
            Exit Function ' esce dalla funzione
        End If

        ' imposta i comandi per la carta
        Call SetCommand(VERIFY_CSC)

        ' controlla se il CSC è un tipo valido e imposta il P2
        Select Case iCSCType
            Case SEND_CSC_USER1
                m_P2 = &H39
            Case SEND_CSC_USER2
                m_P2 = &H3B
            Case SEND_CSC_ADMIN
                m_P2 = &H7
            Case Else
                Return SWORD_E_INVALID_CSC ' restituisce l'errore
                Exit Function ' esce dalla funzione
        End Select

        ' invia i comandi alla carta
        Return SCardSend(hCard, m_CLA, m_INS, m_P1, m_P2, sCSC, m_SW1, m_SW2) ' invia i comandi alla carta
    End Function

    Friend Overridable Function SendCSC(ByVal hCard As Integer, ByVal sCSC As String) As Integer
        If Len(sCSC) = 0 Then ' se non è specificato il CSC
            Return SWORD_E_INVALID_CSC
        End If

        Call SetCommand(SEND_CSC_ADMIN) ' imposta i comandi per la carta
        Return SCardSend(hCard, m_CLA, m_INS, m_P1, m_P2, sCSC, m_SW1, m_SW2) ' invia i comandi alla carta
    End Function

    Friend Overridable Function SendCSC(ByVal hCard As Integer, ByVal iCSCType As Integer, ByVal sCSC As String) As Integer
        Dim ret As Integer

        If Len(sCSC) = 0 Then ' se non è specificato il CSC
            Select Case iCSCType ' controlla il tipo di CSC
                Case SEND_CSC_USER1 : ret = SW_INVALID_CSC_1
                Case SEND_CSC_USER2 : ret = SW_INVALID_CSC_2
                Case SEND_CSC_ADMIN : ret = SW_INVALID_CSC_3
            End Select
            Return ret ' restituisce l'errore
            Exit Function ' esce dalla funzione
        End If

        ' controlla se il CSC è un tipo valido
        Select Case iCSCType
            Case SEND_CSC_USER1, SEND_CSC_USER2, SEND_CSC_ADMIN
            Case Else
                Return SWORD_E_INVALID_CSC ' restituisce l'errore
                Exit Function ' esce dalla funzione
        End Select

        Call SetCommand(iCSCType) ' imposta i comandi per la carta
        Return SCardSend(hCard, m_CLA, m_INS, m_P1, m_P2, sCSC, m_SW1, m_SW2) ' invia i comandi alla carta
    End Function

    Friend Overridable Function SetCSC(ByVal hCard As Integer, ByVal sNewCSC As String) As Integer
        Dim ret As Integer

        If Len(sNewCSC) = 0 Then ' se non è specificato il CSC
            Return SWORD_E_INVALID_CSC
        End If

        Call SetCommand(WRITE_CARD) ' imposta i comandi per la carta
        m_P2 = ADDR_CSC_ADMIN ' imposta l'indirizzo del CSC per l'Admin Area
        ret = SCardSend(hCard, m_CLA, m_INS, m_P1, m_P2, sNewCSC, m_SW1, m_SW2)
        Return ret ' restituisce l'errore API
    End Function

    Friend Overridable Function SetCSC(ByVal hCard As Integer, ByVal iCSCType As Short, ByVal sNewCSC As String) As Integer
        Dim ret As Integer

        If Len(sNewCSC) = 0 Then ' se non è specificato il CSC
            Select Case iCSCType ' controlla il tipo di CSC
                Case SEND_CSC_USER1 : ret = SW_INVALID_CSC_1
                Case SEND_CSC_USER2 : ret = SW_INVALID_CSC_2
                Case SEND_CSC_ADMIN : ret = SW_INVALID_CSC_3
            End Select
            Return ret ' restituisce l'errore
        End If

        Call SetCommand(WRITE_CARD) ' imposta i comandi per la carta
        Select Case iCSCType ' controlla quale CSC impostare
            Case SEND_CSC_USER1 : m_P2 = ADDR_CSC_USER1 ' imposta l'indirizzo del CSC per l'User Area 1
            Case SEND_CSC_USER2 : m_P2 = ADDR_CSC_USER2 ' imposta l'indirizzo del CSC per l'User Area 2
            Case SEND_CSC_ADMIN : m_P2 = ADDR_CSC_ADMIN ' imposta l'indirizzo del CSC per l'Admin Area
            Case Else
                Return SWORD_E_INVALID_CSC ' restituisce l'errore
        End Select

        ret = SCardSend(hCard, m_CLA, m_INS, m_P1, m_P2, sNewCSC, m_SW1, m_SW2)
        Return ret ' restituisce l'errore API
    End Function

    Public Sub New()
        MyBase.New()
        SetATR(REG_GCM, ATR, ATRMask)
    End Sub
End Class