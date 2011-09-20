' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt

Public Class OptionsForm

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        If Me._SessionReady = True Then
            Me.DisposeSession()
        End If
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private _SessionReady As Boolean = False
    Private q As QBFC7Lib.QBSessionManager
    Private ver As QBVersionInfo
    Private utils As QuickBooksUtils

    Private Sub OptionsForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadSettings()
    End Sub

    Public Sub LoadSettings()

        Me.chkUsernamePrefix.Checked = My.Settings.UseUsernamePrefix
        Me.chkOrderPrefix.Checked = My.Settings.UseOrderPrefix
        Me.UsernamePrefixField.Text = My.Settings.UsernamePrefix
        Me.OrderNumberPrefixField.Text = My.Settings.OrderPrefix

        Select Case My.Settings.ImportOrderMode
            Case 0
                Me.rbImportPaid.Checked = True
            Case 1
                Me.rbImportAll.Checked = True
            Case 2
                Me.rbImportAuth.Checked = True
            Case Else
                Me.rbImportPaid.Checked = True
        End Select

        Select Case My.Settings.ImportToMode
            Case 0 'invoice
                Me.rbInvoices.Checked = True
            Case 1 ' sales receipt
                Me.rbSalesReceipts.Checked = True
            Case 2 ' sales order
                Me.rbSalesOrders.Checked = True
            Case Else
                Me.rbInvoices.Checked = True
        End Select

        Me.chkMarkOrdersToBePrinted.Checked = My.Settings.MarkOrdersToBePrinted
        Me.chkMarkPaidOrdersToBePrinted.Checked = My.Settings.MarkPaidOrdersToBePrinted
        Me.chkUseProductPrefix.Checked = My.Settings.UseProductPrefix
        Me.ProductPrefixField.Text = My.Settings.ProductPrefix
        Me.NewItemIncomeAccountField.Text = My.Settings.NewProductIncomeAccount
        Me.SalesReceiptDepositAccountField.Text = My.Settings.SalesReceiptDepositAccount
        Me.InvoiceDepositAccountField.Text = My.Settings.InvoiceDepostAccount
        Me.chkDepositInvoices.Checked = My.Settings.DepositInvoices
        Me.chkDepositSalesReceipts.Checked = My.Settings.DepositSalesReceipts
        Me.InvoiceAcctsRecField.Text = My.Settings.InvoiceAccountsReceivableAccount
        Me.chkUseShipMethodPrefix.Checked = My.Settings.UseShipMethodPrefix
        Me.ShipMethodPrefixField.Text = My.Settings.ShipMethodPrefix
        Me.ShippingItemIncomeAccount.Text = My.Settings.ShippingItemIncomeAccount
        Me.ShippingItemName.Text = My.Settings.ShippingItemName

        Me.HandlingIncomeAccountField.Text = My.Settings.HandlineIncomeAccount
        Me.HandlingItemNameField.Text = My.Settings.HandlingItemName

        Me.chkUseInlineTax.Checked = My.Settings.UseInlineTax
        Me.InlineTaxItemField.Text = My.Settings.InlineTaxItemName
        Me.InlineTaxVendorField.Text = My.Settings.InlineTaxVendorName

        Me.rbExportProductsSKU.Checked = My.Settings.ExportProductsAsSKU


        Me.rbExportUserName.Checked = My.Settings.ExportUsersAsName
        If My.Settings.ExportReverseUserName = True Then
            Me.rbExportUserName.Checked = False
            Me.rbExportReverseUserName.Checked = True
        End If

        Me.chkCreateCustomersAsCompany.Checked = My.Settings.CreateCustomersAsCompany

        Me.chkUseQuickBooksDescription.Checked = My.Settings.UseQuickBooksItemDescription


        VisaNameField.Text = My.Settings.PaymentVisa
        MastercardNameField.Text = My.Settings.PaymentMasterCard
        DiscoverNameField.Text = My.Settings.PaymentDiscover
        AmexNameField.Text = My.Settings.PaymentAmex
        DinersClubNameField.Text = My.Settings.PaymentDiners
        JCBNameField.Text = My.Settings.PaymentJCB
        TelephoneNameField.Text = My.Settings.PaymentTelephone
        PayPalNameField.Text = My.Settings.PaymentPayPal
        CheckNameField.Text = My.Settings.PaymentCheck
        EmailNameField.Text = My.Settings.PaymentEmail
        PONameField.Text = My.Settings.PaymentPO
        CashNameField.Text = My.Settings.PaymentCash
        FaxNameField.Text = My.Settings.PaymentFax
        OtherNameField.Text = My.Settings.PaymentOther

        Me.chkRecordCreditCardInfo.Checked = My.Settings.SendCreditCardInfo
        Me.chkMarkUnpaidOrdersAsPending.Checked = My.Settings.MarkUnpaidOrdersAsPending
        Me.chkUseExportDate.Checked = My.Settings.UseExportDateInsteadOfOrderDate
        Me.chkUseExportDateForShipping.Checked = My.Settings.UseExportDateForShipping

        Me.chkUseSubProductNameMatching.Checked = My.Settings.UseSubProductNameMatching

        ' Inventory - 3.10
        Me.chkCreateAsInventory.Checked = My.Settings.CreateInventoryItems
        Me.chkSendPrices.Checked = My.Settings.SendPricesToWeb
        Me.chkSendInventory.Checked = My.Settings.SendInventoryToWeb
        Me.COGAccountField.Text = My.Settings.COGAccount
        Me.InventoryAssetAccountField.Text = My.Settings.InventoryAssetAccount

        Me.chkProductUseTitleOnly.Checked = My.Settings.ProductUseTitleOnly
        Me.chkDisableTax.Checked = My.Settings.DisableTaxes

    End Sub



    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOkay.Click

        ' Close Down Session
        If Me._SessionReady = True Then
            Me.DisposeSession()
        End If


        My.Settings.UseUsernamePrefix = Me.chkUsernamePrefix.Checked
        My.Settings.UseOrderPrefix = Me.chkOrderPrefix.Checked
        My.Settings.UsernamePrefix = Me.UsernamePrefixField.Text.Trim
        My.Settings.OrderPrefix = Me.OrderNumberPrefixField.Text.Trim

        If Me.rbImportAll.Checked Then
            My.Settings.ImportOrderMode = 1
        Else
            If Me.rbImportAuth.Checked Then
                My.Settings.ImportOrderMode = 2
            Else
                My.Settings.ImportOrderMode = 0
            End If
        End If

        If Me.rbSalesOrders.Checked Then
            My.Settings.ImportToMode = 2 ' Sales order
        Else
            If Me.rbSalesReceipts.Checked Then
                My.Settings.ImportToMode = 1 ' Sales Receipt
            Else
                My.Settings.ImportToMode = 0 ' Invoice
            End If
        End If

        My.Settings.MarkOrdersToBePrinted = Me.chkMarkOrdersToBePrinted.Checked
        My.Settings.MarkPaidOrdersToBePrinted = Me.chkMarkPaidOrdersToBePrinted.Checked
        My.Settings.UseProductPrefix = Me.chkUseProductPrefix.Checked
        My.Settings.ProductPrefix = Me.ProductPrefixField.Text.Trim
        My.Settings.NewProductIncomeAccount = Me.NewItemIncomeAccountField.Text.Trim
        My.Settings.SalesReceiptDepositAccount = Me.SalesReceiptDepositAccountField.Text.Trim
        My.Settings.InvoiceDepostAccount = Me.InvoiceDepositAccountField.Text.Trim
        My.Settings.DepositInvoices = Me.chkDepositInvoices.Checked
        My.Settings.DepositSalesReceipts = Me.chkDepositSalesReceipts.Checked
        My.Settings.InvoiceAccountsReceivableAccount = Me.InvoiceAcctsRecField.Text.Trim
        My.Settings.UseShipMethodPrefix = Me.chkUseShipMethodPrefix.Checked
        My.Settings.ShipMethodPrefix = Me.ShipMethodPrefixField.Text.Trim
        My.Settings.ShippingItemName = Me.ShippingItemName.Text.Trim
        My.Settings.ShippingItemIncomeAccount = Me.ShippingItemIncomeAccount.Text.Trim
        My.Settings.HandlingItemName = Me.HandlingItemNameField.Text.Trim
        My.Settings.HandlineIncomeAccount = Me.HandlingIncomeAccountField.Text.Trim
        My.Settings.UseInlineTax = Me.chkUseInlineTax.Checked
        My.Settings.InlineTaxVendorName = Me.InlineTaxVendorField.Text.Trim
        My.Settings.InlineTaxItemName = Me.InlineTaxItemField.Text.Trim
        My.Settings.ExportProductsAsSKU = Me.rbExportProductsSKU.Checked

        My.Settings.ExportUsersAsName = Me.rbExportUserName.Checked
        If Me.rbExportReverseUserName.Checked = True Then
            My.Settings.ExportReverseUserName = True
        Else
            My.Settings.ExportReverseUserName = False
        End If

        My.Settings.CreateCustomersAsCompany = Me.chkCreateCustomersAsCompany.Checked

        My.Settings.UseQuickBooksItemDescription = Me.chkUseQuickBooksDescription.Checked

        My.Settings.PaymentVisa = VisaNameField.Text
        My.Settings.PaymentMasterCard = MastercardNameField.Text
        My.Settings.PaymentDiscover = DiscoverNameField.Text
        My.Settings.PaymentAmex = AmexNameField.Text
        My.Settings.PaymentDiners = DinersClubNameField.Text
        My.Settings.PaymentJCB = JCBNameField.Text
        My.Settings.PaymentTelephone = TelephoneNameField.Text
        My.Settings.PaymentPayPal = PayPalNameField.Text
        My.Settings.PaymentCheck = CheckNameField.Text
        My.Settings.PaymentEmail = EmailNameField.Text
        My.Settings.PaymentPO = PONameField.Text
        My.Settings.PaymentCash = CashNameField.Text
        My.Settings.PaymentFax = FaxNameField.Text
        My.Settings.PaymentOther = OtherNameField.Text

        My.Settings.SendCreditCardInfo = Me.chkRecordCreditCardInfo.Checked

        My.Settings.MarkUnpaidOrdersAsPending = Me.chkMarkUnpaidOrdersAsPending.Checked

        My.Settings.UseExportDateInsteadOfOrderDate = Me.chkUseExportDate.Checked
        My.Settings.UseExportDateForShipping = Me.chkUseExportDateForShipping.Checked
        My.Settings.UseSubProductNameMatching = Me.chkUseSubProductNameMatching.Checked

        'Inventory - Version 3.10
        My.Settings.SendInventoryToWeb = Me.chkSendInventory.Checked
        My.Settings.SendPricesToWeb = Me.chkSendPrices.Checked
        My.Settings.CreateInventoryItems = Me.chkCreateAsInventory.Checked
        My.Settings.COGAccount = Me.COGAccountField.Text.Trim
        My.Settings.InventoryAssetAccount = Me.InventoryAssetAccountField.Text.Trim

        My.Settings.ProductUseTitleOnly = Me.chkProductUseTitleOnly.Checked
        My.Settings.DisableTaxes = Me.chkDisableTax.Checked

        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Function CreateSession() As Boolean
        If _SessionReady = True Then
            Return True
        Else
            Dim result As Boolean = False

            ' Request Connection Options
            Dim qcon As New QBConnectionForm
            If My.Settings.UseCompanyFile = True Then
                qcon.rbUserFile.Checked = True
                qcon.rbCurrentFile.Checked = False
            End If
            qcon.CompanyFileField.Text = My.Settings.CompanyFileName

            If qcon.ShowDialog = Windows.Forms.DialogResult.OK Then

                ' Save Connection Settings
                My.Settings.UseCompanyFile = qcon.rbUserFile.Checked
                My.Settings.CompanyFileName = qcon.CompanyFileField.Text.Trim

                utils = New QuickBooksUtils()
                q = New QBFC7Lib.QBSessionManager
                Try
                    q.OpenConnection2("", My.Settings.QBApplicationName, QBFC7Lib.ENConnectionType.ctLocalQBD)

                    If My.Settings.UseCompanyFile = True Then
                        q.BeginSession(My.Settings.CompanyFileName, QBFC7Lib.ENOpenMode.omDontCare)
                    Else
                        q.BeginSession("", QBFC7Lib.ENOpenMode.omDontCare)
                    End If

                    ver = utils.GetQuickBooksVersion(q)

                    Me._SessionReady = True
                    result = True

                Catch ex As Exception
                    Logging.LogException(ex)
                    MsgBox("Error: " & ex.Message)
                    result = False
                End Try
            Else
                result = False
            End If

            Return result
        End If
    End Function

    Private Sub DisposeSession()
        utils = Nothing
        q.EndSession()
        q.CloseConnection()
        q = Nothing
        Me._SessionReady = False
    End Sub

    Private Sub btnBrowse2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse2.Click

        If CreateSession() = True Then
            Dim ab As New QBAccountBrowser(Me.q, Me.ver)
            If ab IsNot Nothing Then
                If ab.ShowDialog = Windows.Forms.DialogResult.OK Then
                    If Not ab.lstAccounts.SelectedItem Is Nothing Then
                        Me.SalesReceiptDepositAccountField.Text = CType(ab.lstAccounts.SelectedItem, QBAccountInfo).Name
                    End If
                End If
            End If
            ab.Dispose()
        End If

    End Sub

    Private Sub btnBrowse1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse1.Click

        If CreateSession() = True Then
            Dim ab As New QBAccountBrowser(Me.q, Me.ver)
            If ab.ShowDialog = Windows.Forms.DialogResult.OK Then
                If Not ab.lstAccounts.SelectedItem Is Nothing Then
                    Me.InvoiceDepositAccountField.Text = CType(ab.lstAccounts.SelectedItem, QBAccountInfo).Name
                End If
            End If
            ab.Dispose()
        End If

    End Sub

    Private Sub btnBrowseNewItemIncome_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseNewItemIncome.Click

        If CreateSession() = True Then
            Dim ab As New QBAccountBrowser(Me.q, Me.ver)
            If ab.ShowDialog = Windows.Forms.DialogResult.OK Then
                If Not ab.lstAccounts.SelectedItem Is Nothing Then
                    Me.NewItemIncomeAccountField.Text = CType(ab.lstAccounts.SelectedItem, QBAccountInfo).Name
                End If
            End If
            ab.Dispose()
        End If

    End Sub

    Private Sub btnBrowseAcctsRec_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseAcctsRec.Click

        If CreateSession() = True Then
            Dim ab As New QBAccountBrowser(Me.q, Me.ver)
            If ab.ShowDialog = Windows.Forms.DialogResult.OK Then
                If Not ab.lstAccounts.SelectedItem Is Nothing Then
                    Me.InvoiceAcctsRecField.Text = CType(ab.lstAccounts.SelectedItem, QBAccountInfo).Name
                End If
            End If
            ab.Dispose()
        End If

    End Sub

    Private Sub btnBrowseShippingAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseShippingAccount.Click

        If CreateSession() = True Then
            Dim ab As New QBAccountBrowser(Me.q, Me.ver)
            If ab.ShowDialog = Windows.Forms.DialogResult.OK Then
                If Not ab.lstAccounts.SelectedItem Is Nothing Then
                    Me.ShippingItemIncomeAccount.Text = CType(ab.lstAccounts.SelectedItem, QBAccountInfo).Name
                End If
            End If
            ab.Dispose()
        End If

    End Sub

    Private Sub btnBrowseHandlingAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseHandlingAccount.Click

        If CreateSession() = True Then
            Dim ab As New QBAccountBrowser(Me.q, Me.ver)
            If ab.ShowDialog = Windows.Forms.DialogResult.OK Then
                If Not ab.lstAccounts.SelectedItem Is Nothing Then
                    Me.HandlingIncomeAccountField.Text = CType(ab.lstAccounts.SelectedItem, QBAccountInfo).Name
                End If
            End If
            ab.Dispose()
        End If

    End Sub

    Private Sub chkUsernamePrefix_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUsernamePrefix.CheckedChanged
        If Me.chkUsernamePrefix.Checked = True Then
            Me.UsernamePrefixField.Enabled = True
        Else
            Me.UsernamePrefixField.Enabled = False
        End If
    End Sub

    Private Sub chkOrderPrefix_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOrderPrefix.CheckedChanged
        If Me.chkOrderPrefix.Checked = True Then
            Me.OrderNumberPrefixField.Enabled = True
        Else
            Me.OrderNumberPrefixField.Enabled = False
        End If
    End Sub

    Private Sub chkUseProductPrefix_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseProductPrefix.CheckedChanged
        If Me.chkUseProductPrefix.Checked = True Then
            Me.ProductPrefixField.Enabled = True
        Else
            Me.ProductPrefixField.Enabled = False
        End If
    End Sub

    Private Sub chkUseShipMethodPrefix_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseShipMethodPrefix.CheckedChanged
        If Me.chkUseShipMethodPrefix.Checked = True Then
            Me.ShipMethodPrefixField.Enabled = True
        Else
            Me.ShipMethodPrefixField.Enabled = False
        End If
    End Sub

    Private Sub rbSalesReceipts_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSalesReceipts.CheckedChanged
        If Me.rbSalesReceipts.Checked = True Then
            Me.chkDepositSalesReceipts.Enabled = True
            Me.SalesReceiptDepositAccountField.Enabled = True
            Me.btnBrowse2.Enabled = True
        Else
            Me.chkDepositSalesReceipts.Enabled = False
            Me.SalesReceiptDepositAccountField.Enabled = False
            Me.btnBrowse2.Enabled = False
        End If
    End Sub

    Private Sub rbInvoices_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbInvoices.CheckedChanged
        If Me.rbInvoices.Checked = True Then
            Me.chkDepositInvoices.Enabled = True
            Me.InvoiceAcctsRecField.Enabled = True
            Me.InvoiceDepositAccountField.Enabled = True
            Me.btnBrowse1.Enabled = True
            Me.btnBrowseAcctsRec.Enabled = True
        Else
            Me.chkDepositInvoices.Enabled = False
            Me.InvoiceAcctsRecField.Enabled = False
            Me.InvoiceDepositAccountField.Enabled = False
            Me.btnBrowse1.Enabled = False
            Me.btnBrowseAcctsRec.Enabled = False
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If CreateSession() = True Then
            Dim ab As New QBAccountBrowser(Me.q, Me.ver)
            If ab.ShowDialog = Windows.Forms.DialogResult.OK Then
                If Not ab.lstAccounts.SelectedItem Is Nothing Then
                    Me.COGAccountField.Text = CType(ab.lstAccounts.SelectedItem, QBAccountInfo).Name
                End If
            End If
            ab.Dispose()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If CreateSession() = True Then
            Dim ab As New QBAccountBrowser(Me.q, Me.ver)
            If ab.ShowDialog = Windows.Forms.DialogResult.OK Then
                If Not ab.lstAccounts.SelectedItem Is Nothing Then
                    Me.InventoryAssetAccountField.Text = CType(ab.lstAccounts.SelectedItem, QBAccountInfo).Name
                End If
            End If
            ab.Dispose()
        End If
    End Sub

End Class