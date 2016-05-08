Option Strict Off
Option Explicit On
Friend Class Registry
	
	Private Const HKEY_CLASSES_ROOT As Integer = &H80000000
	Private Const HKEY_CURRENT_USER As Integer = &H80000001
	Private Const HKEY_LOCAL_MACHINE As Integer = &H80000002
	Private Const HKEY_USERS As Integer = &H80000003
	Private Const HKEY_PERFORMANCE_DATA As Integer = &H80000004
	Private Const HKEY_CURRENT_CONFIG As Integer = &H80000005
	Private Const HKEY_DYN_DATA As Integer = &H80000006
	
	Private Const REG_OPTION_NON_VOLATILE As Integer = 0
	
	Private Const KEY_QUERY_VALUE As Integer = &H1s
	Private Const KEY_SET_VALUE As Integer = &H2s
	Private Const KEY_CREATE_SUB_KEY As Integer = &H4s
	Private Const KEY_ENUMERATE_SUB_KEYS As Integer = &H8s
	Private Const KEY_NOTIFY As Integer = &H10s
	Private Const KEY_CREATE_LINK As Integer = &H20s
	Private Const KEY_ALL_ACCESS As Integer = &H3Fs
	
	Private Const ERROR_SUCCESS As Integer = 0
	Private Const REG_SZ As Integer = 1
	Private Const REG_BINARY As Integer = 3
	Private Const REG_DWORD As Integer = 4
	
	
	Private Declare Function RegCreateKeyEx Lib "advapi32.dll"  Alias "RegCreateKeyExA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByVal Reserved As Integer, ByVal lpClass As String, ByVal dwOptions As Integer, ByVal samDesired As Integer, ByRef lpSecurityAttributes As Integer, ByRef phkResult As Integer, ByRef lpdwDisposition As Integer) As Integer
	Private Declare Function RegDeleteKey Lib "advapi32.dll"  Alias "RegDeleteKeyA"(ByVal hKey As Integer, ByVal lpSubKey As String) As Integer
	Private Declare Function RegOpenKeyEx Lib "advapi32.dll"  Alias "RegOpenKeyExA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
	Private Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hKey As Integer) As Integer
	Private Declare Function RegQueryValueExByte Lib "advapi32.dll"  Alias "RegQueryValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByRef lpData As Byte, ByRef lpcbData As Integer) As Integer
	Private Declare Function RegQueryValueExStr Lib "advapi32.dll"  Alias "RegQueryValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer
	Private Declare Function RegQueryValueExLong Lib "advapi32.dll"  Alias "RegQueryValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByRef lpData As Integer, ByRef lpcbData As Integer) As Integer
	Private Declare Function RegSetValueExByte Lib "advapi32.dll"  Alias "RegSetValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByRef lpData As Byte, ByVal cbData As Integer) As Integer
	Private Declare Function RegSetValueExStr Lib "advapi32.dll"  Alias "RegSetValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByVal lpData As String, ByVal cbData As Integer) As Integer
	Private Declare Function RegSetValueExLong Lib "advapi32.dll"  Alias "RegSetValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByRef lpData As Integer, ByVal cbData As Integer) As Integer
	Private Declare Function RegDeleteValue Lib "advapi32.dll"  Alias "RegDeleteValueA"(ByVal hKey As Integer, ByVal lpValueName As String) As Integer
	
	Private Function GetBase(ByRef sName As String) As Integer
		Dim Base As String
		
		Base = UCase(Left(sName, InStr(sName, "\") - 1))
		sName = Mid(sName, InStr(sName, "\") + 1)
		Select Case Base
			Case "HKEY_CLASSES_ROOT" : GetBase = HKEY_CLASSES_ROOT
			Case "HKEY_CURRENT_USER" : GetBase = HKEY_CURRENT_USER
			Case "HKEY_LOCAL_MACHINE" : GetBase = HKEY_LOCAL_MACHINE
			Case "HKEY_USERS" : GetBase = HKEY_USERS
			Case "HKEY_PERFORMANCE_DATA" : GetBase = HKEY_PERFORMANCE_DATA
			Case "HKEY_CURRENT_CONFIG" : GetBase = HKEY_CURRENT_CONFIG
			Case "HKEY_DYN_DATA" : GetBase = HKEY_DYN_DATA
			Case Else : GetBase = &H88888888
		End Select
		
	End Function
	
	Friend Function Create(ByVal sName As String) As Boolean
		Dim ret As Boolean
		Dim sKey As Integer
		Dim Base As Integer
		Dim Ris As Integer
		
		Base = GetBase(sName)
		ret = (RegCreateKeyEx(Base, sName, 0, vbNullString, REG_OPTION_NON_VOLATILE, KEY_QUERY_VALUE, 0, sKey, Ris) = ERROR_SUCCESS)
		If ret Then ret = CloseReg(sKey)
		Create = ret
	End Function
	
	Friend Function Delete(ByVal sName As String) As Boolean
		Dim Base As Integer
		
		Base = GetBase(sName)
		Delete = (RegDeleteKey(Base, sName) = ERROR_SUCCESS)
	End Function
	
	Private Function OpenReg(ByVal sName As String, ByRef hKey As Integer, ByVal Accesso As Integer) As Boolean
		Dim Base As Integer
		
		Base = GetBase(sName)
		OpenReg = (RegOpenKeyEx(Base, sName, 0, Accesso, hKey) = ERROR_SUCCESS)
	End Function
	
	Private Function CloseReg(ByVal hKey As Integer) As Boolean
		CloseReg = (RegCloseKey(hKey) = ERROR_SUCCESS)
	End Function
	
	Friend Function ReadStr(ByVal sKey As String, ByVal sName As String, ByRef sValue As String, Optional ByVal sDefault As String = vbNullString) As Boolean
		Dim hKey As Integer
		Dim Dimensione As Integer
		Dim Tipo As Integer
		
		If OpenReg(sKey, hKey, KEY_QUERY_VALUE) Then
			If (RegQueryValueExStr(hKey, sName, 0, Tipo, CStr(0), Dimensione) = ERROR_SUCCESS) And (Tipo = REG_SZ) Then
				sValue = Space(Dimensione)
				If RegQueryValueExStr(hKey, sName, 0, 0, sValue, Dimensione) = ERROR_SUCCESS Then
					sValue = Left(sValue, InStr(sValue, vbNullChar) - 1)
					CloseReg(hKey)
					ReadStr = True
				End If
			End If
			CloseReg(hKey)
		End If
		sValue = sDefault
		ReadStr = False
	End Function
	
	Friend Function ReadByte(ByVal sKey As String, ByVal sName As String, ByRef aValue() As Byte) As Boolean
		Dim ret As Boolean
		Dim hKey As Integer
		Dim Dimensione As Integer
		Dim Tipo As Integer
		
		ret = False
		If OpenReg(sKey, hKey, KEY_QUERY_VALUE) Then
			If (RegQueryValueExByte(hKey, sName, 0, Tipo, 0, Dimensione) = ERROR_SUCCESS) And (Tipo = REG_BINARY) Then
				ReDim aValue(Dimensione - 1)
				ret = (RegQueryValueExByte(hKey, sName, 0, 0, aValue(0), Dimensione) = ERROR_SUCCESS)
			End If
			CloseReg(hKey)
		End If
		ReadByte = ret
	End Function
	
	Friend Function ReadLng(ByVal sKey As String, ByVal sName As String, ByRef iValue As Integer, Optional ByVal iDefault As Integer = 0) As Boolean
		Dim ret As Boolean
		Dim hKey As Integer
		Dim Tipo As Integer
		
		ret = False
		If OpenReg(sKey, hKey, KEY_QUERY_VALUE) Then
			ret = ((RegQueryValueExLong(hKey, sName, 0, Tipo, iValue, 4) = ERROR_SUCCESS) And (Tipo = REG_DWORD))
			CloseReg(hKey)
		End If
		iValue = iDefault
		ReadLng = ret
	End Function
	
	Friend Function WriteStr(ByVal sKey As String, ByVal sName As String, ByVal sValue As String, Optional ByVal sDefault As String = "") As Boolean
		Dim ret As Boolean
		Dim hKey As Integer
		
		ret = False
		If OpenReg(sKey, hKey, KEY_SET_VALUE) Then
			If sValue <> sDefault Then
				ret = (RegSetValueExStr(hKey, sName, 0, REG_SZ, sValue, Len(sValue) + 1) = ERROR_SUCCESS)
			Else
				ret = DeleteValue(hKey, sName)
			End If
			CloseReg(hKey)
		End If
		WriteStr = ret
	End Function
	
	Friend Function WriteByte(ByVal sKey As String, ByVal sName As String, ByRef aValue() As Byte) As Boolean
		Dim ret As Boolean
		Dim hKey As Integer
		Dim Dimensione As Integer
		
		ret = False
		If OpenReg(sKey, hKey, KEY_SET_VALUE) Then
			Dimensione = UBound(aValue) - LBound(aValue) + 1
			ret = (RegSetValueExByte(hKey, sName, 0, REG_BINARY, aValue(0), Dimensione) = ERROR_SUCCESS)
			CloseReg(hKey)
		End If
		WriteByte = ret
	End Function
	
	Friend Function WriteLng(ByVal sKey As String, ByVal sName As String, ByVal iValue As Integer, Optional ByVal iDefault As Integer = 0) As Boolean
		Dim ret As Boolean
		Dim hKey As Integer
		
		ret = False
		If OpenReg(sKey, hKey, KEY_SET_VALUE) Then
			If iValue <> iDefault Then
				ret = (RegSetValueExLong(hKey, sName, 0, REG_DWORD, iValue, 4) = ERROR_SUCCESS)
			Else
				ret = DeleteValue(hKey, sName)
			End If
			CloseReg(hKey)
		End If
		WriteLng = ret
	End Function
	
	Private Function DeleteValue(ByVal hKey As Integer, ByVal sName As String) As Boolean
		DeleteValue = (RegDeleteValue(hKey, sName) = ERROR_SUCCESS)
	End Function
End Class