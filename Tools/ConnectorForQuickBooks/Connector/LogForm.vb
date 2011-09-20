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

Public Class LogForm

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub LogForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadLog()
    End Sub

    Private Sub LoadLog()

        Dim logFileName As String = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "BVLog.txt")
        If File.Exists(logFileName) Then
            Me.OutputField.Text = File.ReadAllText(logFileName)
        Else
            Me.OutputField.Text = "Log file could not be located in folder: " & Application.ExecutablePath
        End If

    End Sub
End Class