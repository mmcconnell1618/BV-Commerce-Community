' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt

Public Class QBAccountBrowser

    Private q As QBFC7Lib.QBSessionManager
    Private v As QBVersionInfo

    Public Sub New(ByRef ses As QBFC7Lib.QBSessionManager, ByRef ver As QBVersionInfo)
        Me.InitializeComponent()
        q = ses
        v = ver
    End Sub

    Private Sub QBAccountBrowser_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadAccounts()
    End Sub

    Public Sub LoadAccounts()
        Dim u As New QuickBooksUtils()

        Dim accounts As QBAccountInfo() = u.AccountList(q, v)

        If Not accounts Is Nothing Then
            Me.lstAccounts.Items.Clear()

            Me.lstAccounts.DisplayMember = "Name"
            Me.lstAccounts.ValueMember = "ListID"

            For i As Integer = 0 To accounts.Length - 1
                If accounts(i) IsNot Nothing Then
                    Me.lstAccounts.Items.Add(accounts(i))
                End If
            Next
        End If

        accounts = Nothing
        u = Nothing
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim naf As New NewAccountForm
        If naf.ShowDialog = System.Windows.Forms.DialogResult.OK Then

            Dim u As New QuickBooksUtils()

            Dim ainfo As QBAccountInfo

            ainfo = CType(naf.lstAccountType.SelectedItem, QBAccountInfo)
            ainfo.Name = naf.NewAccountNameField.Text.Trim
            ainfo = u.AccountCreate(ainfo, q, v)

            If ainfo.ListId = "-1" Then
                MsgBox("Error: Unable to Create Account")
            End If

            u = Nothing

        End If
        naf = Nothing
        LoadAccounts()
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click

    End Sub

    Private Sub ButtonOkay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOkay.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
End Class