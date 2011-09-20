' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt

Imports System.IO

Public Class Logging

    Private Shared writer As StreamWriter = Nothing
    Private Shared logFileOpen As Boolean = False

    Private Shared Sub OpenLogFile()
        If logFileOpen = False Then
            writer = New StreamWriter(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "BVLog.txt"), False)
            logFileOpen = True
        End If
    End Sub

    Public Shared Sub CloseLogFile()
        If Not writer Is Nothing Then
            writer.Close()
            logFileOpen = False
        End If
    End Sub

    Public Shared Sub Write(ByVal message As String)
        If logFileOpen = False Then
            OpenLogFile()
        End If
        writer.Write(message)
    End Sub

    Public Shared Sub WriteLine(ByVal message As String)
        If logFileOpen = False Then
            OpenLogFile()
        End If
        writer.Write(message & vbNewLine)
    End Sub

    Public Shared Sub LogException(ByVal ex As Exception)
        WriteLine("###################################################")
        WriteLine("Exception!")
        WriteLine(ex.Message)
        WriteLine("")
        WriteLine(ex.StackTrace)
        WriteLine("###################################################")
    End Sub

End Class
