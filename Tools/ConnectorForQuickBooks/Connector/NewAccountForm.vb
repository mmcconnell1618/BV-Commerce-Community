' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt

Public Class NewAccountForm

    Private Sub NewAccountForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim a As New QBAccountInfo

        a.Name = "Bank"
        a.AccountType = QBAccountType.Bank
        Me.lstAccountType.Items.Add(a)

        a = New QBAccountInfo
        a.Name = "Cost Of Goods Sold"
        a.AccountType = QBAccountType.CostOfGoodSold
        Me.lstAccountType.Items.Add(a)

        a = New QBAccountInfo
        a.Name = "Credit Card"
        a.AccountType = QBAccountType.CreditCard
        Me.lstAccountType.Items.Add(a)

        a = New QBAccountInfo
        a.Name = "Equity"
        a.AccountType = QBAccountType.Equity
        Me.lstAccountType.Items.Add(a)

        a = New QBAccountInfo
        a.Name = "Expense"
        a.AccountType = QBAccountType.Expense
        Me.lstAccountType.Items.Add(a)

        a = New QBAccountInfo
        a.Name = "Income"
        a.AccountType = QBAccountType.Income
        Me.lstAccountType.Items.Add(a)

        a = New QBAccountInfo
        a.Name = "Other Expense"
        a.AccountType = QBAccountType.OtherExpense
        Me.lstAccountType.Items.Add(a)

        a = New QBAccountInfo
        a.Name = "Other Income"
        a.AccountType = QBAccountType.OtherIncome
        Me.lstAccountType.Items.Add(a)

        Me.lstAccountType.DisplayMember = "Name"
        Me.lstAccountType.SelectedItem = Me.lstAccountType.Items(0)
    End Sub

    Private Sub ButtonOkay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOkay.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub
End Class