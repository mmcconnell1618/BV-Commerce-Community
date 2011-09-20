' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt

Public Class QBConnectionForm

    Private Sub BrowseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BrowseButton.Click
        Me.rbCurrentFile.Checked = False
        Me.rbUserFile.Checked = True
        Dim fd As New OpenFileDialog
        If fd.ShowDialog = Windows.Forms.DialogResult.OK Then
            Me.CompanyFileField.Text = fd.FileName
        End If
        fd.Dispose()
        fd = Nothing
    End Sub

    Private Sub OkayButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkayButton.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
End Class