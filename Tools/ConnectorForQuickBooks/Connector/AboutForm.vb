' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt

Public Class AboutForm

    Private Sub AboutForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.NameField.Text = Application.ProductName
        Me.VersionField.Text = Application.ProductVersion
    End Sub

    Private Sub OkayButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkayButton.Click
        Me.Hide()
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim lf As New LogForm
        lf.ShowDialog()
        lf.Dispose()
    End Sub
End Class