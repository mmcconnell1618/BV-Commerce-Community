<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OptionsForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OptionsForm))
        Me.ButtonOkay = New System.Windows.Forms.Button
        Me.ButtonCancel = New System.Windows.Forms.Button
        Me.TabPage9 = New System.Windows.Forms.TabPage
        Me.Label26 = New System.Windows.Forms.Label
        Me.Label25 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.InventoryAssetAccountField = New System.Windows.Forms.TextBox
        Me.COGAccountField = New System.Windows.Forms.TextBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.chkCreateAsInventory = New System.Windows.Forms.CheckBox
        Me.GroupBox6 = New System.Windows.Forms.GroupBox
        Me.chkSendPrices = New System.Windows.Forms.CheckBox
        Me.chkSendInventory = New System.Windows.Forms.CheckBox
        Me.TabPage7 = New System.Windows.Forms.TabPage
        Me.chkUseInlineTax = New System.Windows.Forms.CheckBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.Label24 = New System.Windows.Forms.Label
        Me.InlineTaxVendorField = New System.Windows.Forms.TextBox
        Me.InlineTaxItemField = New System.Windows.Forms.TextBox
        Me.TabPage6 = New System.Windows.Forms.TabPage
        Me.Label21 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.btnBrowseShippingAccount = New System.Windows.Forms.Button
        Me.btnBrowseHandlingAccount = New System.Windows.Forms.Button
        Me.HandlingIncomeAccountField = New System.Windows.Forms.TextBox
        Me.HandlingItemNameField = New System.Windows.Forms.TextBox
        Me.ShippingItemIncomeAccount = New System.Windows.Forms.TextBox
        Me.ShippingItemName = New System.Windows.Forms.TextBox
        Me.TabPage5 = New System.Windows.Forms.TabPage
        Me.chkProductUseTitleOnly = New System.Windows.Forms.CheckBox
        Me.chkUseSubProductNameMatching = New System.Windows.Forms.CheckBox
        Me.chkUseQuickBooksDescription = New System.Windows.Forms.CheckBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.rbExportProductsSKU = New System.Windows.Forms.RadioButton
        Me.rbExportProductsName = New System.Windows.Forms.RadioButton
        Me.btnBrowseNewItemIncome = New System.Windows.Forms.Button
        Me.NewItemIncomeAccountField = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.ShipMethodPrefixField = New System.Windows.Forms.TextBox
        Me.ProductPrefixField = New System.Windows.Forms.TextBox
        Me.UsernamePrefixField = New System.Windows.Forms.TextBox
        Me.OrderNumberPrefixField = New System.Windows.Forms.TextBox
        Me.chkUseShipMethodPrefix = New System.Windows.Forms.CheckBox
        Me.chkUseProductPrefix = New System.Windows.Forms.CheckBox
        Me.chkUsernamePrefix = New System.Windows.Forms.CheckBox
        Me.chkOrderPrefix = New System.Windows.Forms.CheckBox
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.FaxNameField = New System.Windows.Forms.TextBox
        Me.CashNameField = New System.Windows.Forms.TextBox
        Me.TelephoneNameField = New System.Windows.Forms.TextBox
        Me.EmailNameField = New System.Windows.Forms.TextBox
        Me.PONameField = New System.Windows.Forms.TextBox
        Me.CheckNameField = New System.Windows.Forms.TextBox
        Me.PayPalNameField = New System.Windows.Forms.TextBox
        Me.OtherNameField = New System.Windows.Forms.TextBox
        Me.JCBNameField = New System.Windows.Forms.TextBox
        Me.DinersClubNameField = New System.Windows.Forms.TextBox
        Me.AmexNameField = New System.Windows.Forms.TextBox
        Me.DiscoverNameField = New System.Windows.Forms.TextBox
        Me.MastercardNameField = New System.Windows.Forms.TextBox
        Me.VisaNameField = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.chkRecordCreditCardInfo = New System.Windows.Forms.CheckBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.chkCreateCustomersAsCompany = New System.Windows.Forms.CheckBox
        Me.rbExportReverseUserName = New System.Windows.Forms.RadioButton
        Me.rbExportUserName = New System.Windows.Forms.RadioButton
        Me.rbExportUserEmail = New System.Windows.Forms.RadioButton
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.chkUseExportDateForShipping = New System.Windows.Forms.CheckBox
        Me.chkMarkPaidOrdersToBePrinted = New System.Windows.Forms.CheckBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.rbSalesOrders = New System.Windows.Forms.RadioButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnBrowseAcctsRec = New System.Windows.Forms.Button
        Me.InvoiceAcctsRecField = New System.Windows.Forms.TextBox
        Me.btnBrowse1 = New System.Windows.Forms.Button
        Me.SalesReceiptDepositAccountField = New System.Windows.Forms.TextBox
        Me.btnBrowse2 = New System.Windows.Forms.Button
        Me.InvoiceDepositAccountField = New System.Windows.Forms.TextBox
        Me.chkDepositInvoices = New System.Windows.Forms.CheckBox
        Me.chkDepositSalesReceipts = New System.Windows.Forms.CheckBox
        Me.rbInvoices = New System.Windows.Forms.RadioButton
        Me.rbSalesReceipts = New System.Windows.Forms.RadioButton
        Me.chkUseExportDate = New System.Windows.Forms.CheckBox
        Me.chkMarkOrdersToBePrinted = New System.Windows.Forms.CheckBox
        Me.chkMarkUnpaidOrdersAsPending = New System.Windows.Forms.CheckBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbImportAuth = New System.Windows.Forms.RadioButton
        Me.rbImportAll = New System.Windows.Forms.RadioButton
        Me.rbImportPaid = New System.Windows.Forms.RadioButton
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.chkDisableTax = New System.Windows.Forms.CheckBox
        Me.TabPage9.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.TabPage7.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonOkay
        '
        Me.ButtonOkay.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonOkay.Location = New System.Drawing.Point(478, 337)
        Me.ButtonOkay.Name = "ButtonOkay"
        Me.ButtonOkay.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOkay.TabIndex = 1
        Me.ButtonOkay.Text = "OK"
        Me.ButtonOkay.UseVisualStyleBackColor = True
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonCancel.Location = New System.Drawing.Point(397, 337)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCancel.TabIndex = 2
        Me.ButtonCancel.Text = "Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'TabPage9
        '
        Me.TabPage9.Controls.Add(Me.Label26)
        Me.TabPage9.Controls.Add(Me.Label25)
        Me.TabPage9.Controls.Add(Me.Button1)
        Me.TabPage9.Controls.Add(Me.InventoryAssetAccountField)
        Me.TabPage9.Controls.Add(Me.COGAccountField)
        Me.TabPage9.Controls.Add(Me.Button2)
        Me.TabPage9.Controls.Add(Me.chkCreateAsInventory)
        Me.TabPage9.Controls.Add(Me.GroupBox6)
        Me.TabPage9.Location = New System.Drawing.Point(4, 22)
        Me.TabPage9.Name = "TabPage9"
        Me.TabPage9.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage9.Size = New System.Drawing.Size(530, 286)
        Me.TabPage9.TabIndex = 9
        Me.TabPage9.Text = "Inventory"
        Me.TabPage9.UseVisualStyleBackColor = True
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Location = New System.Drawing.Point(77, 147)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(117, 13)
        Me.Label26.TabIndex = 20
        Me.Label26.Text = "Cost of Goods Account"
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Location = New System.Drawing.Point(118, 174)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(76, 13)
        Me.Label25.TabIndex = 19
        Me.Label25.Text = "Asset Account"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(359, 170)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 18
        Me.Button1.Text = "Browse"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'InventoryAssetAccountField
        '
        Me.InventoryAssetAccountField.Location = New System.Drawing.Point(200, 171)
        Me.InventoryAssetAccountField.Name = "InventoryAssetAccountField"
        Me.InventoryAssetAccountField.Size = New System.Drawing.Size(153, 20)
        Me.InventoryAssetAccountField.TabIndex = 17
        '
        'COGAccountField
        '
        Me.COGAccountField.Location = New System.Drawing.Point(200, 144)
        Me.COGAccountField.Name = "COGAccountField"
        Me.COGAccountField.Size = New System.Drawing.Size(153, 20)
        Me.COGAccountField.TabIndex = 15
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(359, 144)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 16
        Me.Button2.Text = "Browse"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'chkCreateAsInventory
        '
        Me.chkCreateAsInventory.AutoSize = True
        Me.chkCreateAsInventory.Location = New System.Drawing.Point(135, 107)
        Me.chkCreateAsInventory.Name = "chkCreateAsInventory"
        Me.chkCreateAsInventory.Size = New System.Drawing.Size(263, 17)
        Me.chkCreateAsInventory.TabIndex = 9
        Me.chkCreateAsInventory.Text = "Create Products as Inventory Items in QuickBooks"
        Me.chkCreateAsInventory.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.chkSendPrices)
        Me.GroupBox6.Controls.Add(Me.chkSendInventory)
        Me.GroupBox6.Location = New System.Drawing.Point(135, 19)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(263, 63)
        Me.GroupBox6.TabIndex = 8
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Synchronization Options"
        '
        'chkSendPrices
        '
        Me.chkSendPrices.AutoSize = True
        Me.chkSendPrices.Location = New System.Drawing.Point(26, 38)
        Me.chkSendPrices.Name = "chkSendPrices"
        Me.chkSendPrices.Size = New System.Drawing.Size(121, 17)
        Me.chkSendPrices.TabIndex = 1
        Me.chkSendPrices.Text = "Send Prices to Web"
        Me.chkSendPrices.UseVisualStyleBackColor = True
        '
        'chkSendInventory
        '
        Me.chkSendInventory.AutoSize = True
        Me.chkSendInventory.Location = New System.Drawing.Point(26, 19)
        Me.chkSendInventory.Name = "chkSendInventory"
        Me.chkSendInventory.Size = New System.Drawing.Size(136, 17)
        Me.chkSendInventory.TabIndex = 0
        Me.chkSendInventory.Text = "Send Inventory to Web"
        Me.chkSendInventory.UseVisualStyleBackColor = True
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.chkDisableTax)
        Me.TabPage7.Controls.Add(Me.chkUseInlineTax)
        Me.TabPage7.Controls.Add(Me.Label23)
        Me.TabPage7.Controls.Add(Me.Label24)
        Me.TabPage7.Controls.Add(Me.InlineTaxVendorField)
        Me.TabPage7.Controls.Add(Me.InlineTaxItemField)
        Me.TabPage7.Location = New System.Drawing.Point(4, 22)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage7.Size = New System.Drawing.Size(530, 286)
        Me.TabPage7.TabIndex = 6
        Me.TabPage7.Text = "Tax"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'chkUseInlineTax
        '
        Me.chkUseInlineTax.AutoSize = True
        Me.chkUseInlineTax.Location = New System.Drawing.Point(185, 161)
        Me.chkUseInlineTax.Name = "chkUseInlineTax"
        Me.chkUseInlineTax.Size = New System.Drawing.Size(181, 17)
        Me.chkUseInlineTax.TabIndex = 12
        Me.chkUseInlineTax.Text = "Add Tax as a Line Item to Orders"
        Me.chkUseInlineTax.UseVisualStyleBackColor = True
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Location = New System.Drawing.Point(86, 123)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(93, 13)
        Me.Label23.TabIndex = 11
        Me.Label23.Text = "Tax Vendor Name"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Location = New System.Drawing.Point(100, 97)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(79, 13)
        Me.Label24.TabIndex = 10
        Me.Label24.Text = "Tax Item Name"
        '
        'InlineTaxVendorField
        '
        Me.InlineTaxVendorField.Location = New System.Drawing.Point(185, 120)
        Me.InlineTaxVendorField.Name = "InlineTaxVendorField"
        Me.InlineTaxVendorField.Size = New System.Drawing.Size(205, 20)
        Me.InlineTaxVendorField.TabIndex = 9
        '
        'InlineTaxItemField
        '
        Me.InlineTaxItemField.Location = New System.Drawing.Point(185, 94)
        Me.InlineTaxItemField.Name = "InlineTaxItemField"
        Me.InlineTaxItemField.Size = New System.Drawing.Size(205, 20)
        Me.InlineTaxItemField.TabIndex = 8
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.Label21)
        Me.TabPage6.Controls.Add(Me.Label22)
        Me.TabPage6.Controls.Add(Me.Label20)
        Me.TabPage6.Controls.Add(Me.Label19)
        Me.TabPage6.Controls.Add(Me.btnBrowseShippingAccount)
        Me.TabPage6.Controls.Add(Me.btnBrowseHandlingAccount)
        Me.TabPage6.Controls.Add(Me.HandlingIncomeAccountField)
        Me.TabPage6.Controls.Add(Me.HandlingItemNameField)
        Me.TabPage6.Controls.Add(Me.ShippingItemIncomeAccount)
        Me.TabPage6.Controls.Add(Me.ShippingItemName)
        Me.TabPage6.Location = New System.Drawing.Point(4, 22)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage6.Size = New System.Drawing.Size(530, 286)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "Shipping"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(30, 185)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(130, 13)
        Me.Label21.TabIndex = 9
        Me.Label21.Text = "Handling Income Account"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Location = New System.Drawing.Point(57, 159)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(103, 13)
        Me.Label22.TabIndex = 8
        Me.Label22.Text = "Handling Item Name"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(30, 100)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(129, 13)
        Me.Label20.TabIndex = 7
        Me.Label20.Text = "Shipping Income Account"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(57, 74)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(102, 13)
        Me.Label19.TabIndex = 6
        Me.Label19.Text = "Shipping Item Name"
        '
        'btnBrowseShippingAccount
        '
        Me.btnBrowseShippingAccount.Location = New System.Drawing.Point(376, 95)
        Me.btnBrowseShippingAccount.Name = "btnBrowseShippingAccount"
        Me.btnBrowseShippingAccount.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseShippingAccount.TabIndex = 5
        Me.btnBrowseShippingAccount.Text = "Browse"
        Me.btnBrowseShippingAccount.UseVisualStyleBackColor = True
        '
        'btnBrowseHandlingAccount
        '
        Me.btnBrowseHandlingAccount.Location = New System.Drawing.Point(376, 180)
        Me.btnBrowseHandlingAccount.Name = "btnBrowseHandlingAccount"
        Me.btnBrowseHandlingAccount.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseHandlingAccount.TabIndex = 4
        Me.btnBrowseHandlingAccount.Text = "Browse"
        Me.btnBrowseHandlingAccount.UseVisualStyleBackColor = True
        '
        'HandlingIncomeAccountField
        '
        Me.HandlingIncomeAccountField.Location = New System.Drawing.Point(165, 182)
        Me.HandlingIncomeAccountField.Name = "HandlingIncomeAccountField"
        Me.HandlingIncomeAccountField.Size = New System.Drawing.Size(205, 20)
        Me.HandlingIncomeAccountField.TabIndex = 3
        '
        'HandlingItemNameField
        '
        Me.HandlingItemNameField.Location = New System.Drawing.Point(165, 156)
        Me.HandlingItemNameField.Name = "HandlingItemNameField"
        Me.HandlingItemNameField.Size = New System.Drawing.Size(205, 20)
        Me.HandlingItemNameField.TabIndex = 2
        '
        'ShippingItemIncomeAccount
        '
        Me.ShippingItemIncomeAccount.Location = New System.Drawing.Point(165, 97)
        Me.ShippingItemIncomeAccount.Name = "ShippingItemIncomeAccount"
        Me.ShippingItemIncomeAccount.Size = New System.Drawing.Size(205, 20)
        Me.ShippingItemIncomeAccount.TabIndex = 1
        '
        'ShippingItemName
        '
        Me.ShippingItemName.Location = New System.Drawing.Point(165, 71)
        Me.ShippingItemName.Name = "ShippingItemName"
        Me.ShippingItemName.Size = New System.Drawing.Size(205, 20)
        Me.ShippingItemName.TabIndex = 0
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.chkProductUseTitleOnly)
        Me.TabPage5.Controls.Add(Me.chkUseSubProductNameMatching)
        Me.TabPage5.Controls.Add(Me.chkUseQuickBooksDescription)
        Me.TabPage5.Controls.Add(Me.GroupBox3)
        Me.TabPage5.Controls.Add(Me.btnBrowseNewItemIncome)
        Me.TabPage5.Controls.Add(Me.NewItemIncomeAccountField)
        Me.TabPage5.Controls.Add(Me.Label18)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5.Size = New System.Drawing.Size(530, 286)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "Products"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'chkProductUseTitleOnly
        '
        Me.chkProductUseTitleOnly.AutoSize = True
        Me.chkProductUseTitleOnly.Location = New System.Drawing.Point(169, 247)
        Me.chkProductUseTitleOnly.Name = "chkProductUseTitleOnly"
        Me.chkProductUseTitleOnly.Size = New System.Drawing.Size(233, 17)
        Me.chkProductUseTitleOnly.TabIndex = 6
        Me.chkProductUseTitleOnly.Text = "Use Title Only when Creating New Products"
        Me.chkProductUseTitleOnly.UseVisualStyleBackColor = True
        '
        'chkUseSubProductNameMatching
        '
        Me.chkUseSubProductNameMatching.AutoSize = True
        Me.chkUseSubProductNameMatching.Location = New System.Drawing.Point(169, 223)
        Me.chkUseSubProductNameMatching.Name = "chkUseSubProductNameMatching"
        Me.chkUseSubProductNameMatching.Size = New System.Drawing.Size(262, 17)
        Me.chkUseSubProductNameMatching.TabIndex = 5
        Me.chkUseSubProductNameMatching.Text = "Match QuickBooks Items ending in Product Name"
        Me.chkUseSubProductNameMatching.UseVisualStyleBackColor = True
        '
        'chkUseQuickBooksDescription
        '
        Me.chkUseQuickBooksDescription.AutoSize = True
        Me.chkUseQuickBooksDescription.Location = New System.Drawing.Point(169, 200)
        Me.chkUseQuickBooksDescription.Name = "chkUseQuickBooksDescription"
        Me.chkUseQuickBooksDescription.Size = New System.Drawing.Size(208, 17)
        Me.chkUseQuickBooksDescription.TabIndex = 4
        Me.chkUseQuickBooksDescription.Text = "Use QuickBooks Description For Items"
        Me.chkUseQuickBooksDescription.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.rbExportProductsSKU)
        Me.GroupBox3.Controls.Add(Me.rbExportProductsName)
        Me.GroupBox3.Location = New System.Drawing.Point(169, 100)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(162, 79)
        Me.GroupBox3.TabIndex = 3
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Product Create Mode"
        '
        'rbExportProductsSKU
        '
        Me.rbExportProductsSKU.AutoSize = True
        Me.rbExportProductsSKU.Checked = True
        Me.rbExportProductsSKU.Location = New System.Drawing.Point(6, 42)
        Me.rbExportProductsSKU.Name = "rbExportProductsSKU"
        Me.rbExportProductsSKU.Size = New System.Drawing.Size(140, 17)
        Me.rbExportProductsSKU.TabIndex = 1
        Me.rbExportProductsSKU.TabStop = True
        Me.rbExportProductsSKU.Text = "Create Products by SKU"
        Me.rbExportProductsSKU.UseVisualStyleBackColor = True
        '
        'rbExportProductsName
        '
        Me.rbExportProductsName.AutoSize = True
        Me.rbExportProductsName.Location = New System.Drawing.Point(6, 19)
        Me.rbExportProductsName.Name = "rbExportProductsName"
        Me.rbExportProductsName.Size = New System.Drawing.Size(146, 17)
        Me.rbExportProductsName.TabIndex = 0
        Me.rbExportProductsName.Text = "Create Products by Name"
        Me.rbExportProductsName.UseVisualStyleBackColor = True
        '
        'btnBrowseNewItemIncome
        '
        Me.btnBrowseNewItemIncome.Location = New System.Drawing.Point(366, 53)
        Me.btnBrowseNewItemIncome.Name = "btnBrowseNewItemIncome"
        Me.btnBrowseNewItemIncome.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseNewItemIncome.TabIndex = 2
        Me.btnBrowseNewItemIncome.Text = "Browse"
        Me.btnBrowseNewItemIncome.UseVisualStyleBackColor = True
        '
        'NewItemIncomeAccountField
        '
        Me.NewItemIncomeAccountField.Location = New System.Drawing.Point(169, 55)
        Me.NewItemIncomeAccountField.Name = "NewItemIncomeAccountField"
        Me.NewItemIncomeAccountField.Size = New System.Drawing.Size(191, 20)
        Me.NewItemIncomeAccountField.TabIndex = 1
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(13, 58)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(150, 13)
        Me.Label18.TabIndex = 0
        Me.Label18.Text = "New Product Income Account"
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.ShipMethodPrefixField)
        Me.TabPage4.Controls.Add(Me.ProductPrefixField)
        Me.TabPage4.Controls.Add(Me.UsernamePrefixField)
        Me.TabPage4.Controls.Add(Me.OrderNumberPrefixField)
        Me.TabPage4.Controls.Add(Me.chkUseShipMethodPrefix)
        Me.TabPage4.Controls.Add(Me.chkUseProductPrefix)
        Me.TabPage4.Controls.Add(Me.chkUsernamePrefix)
        Me.TabPage4.Controls.Add(Me.chkOrderPrefix)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(530, 286)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Prefixes"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'ShipMethodPrefixField
        '
        Me.ShipMethodPrefixField.Location = New System.Drawing.Point(253, 164)
        Me.ShipMethodPrefixField.Name = "ShipMethodPrefixField"
        Me.ShipMethodPrefixField.Size = New System.Drawing.Size(100, 20)
        Me.ShipMethodPrefixField.TabIndex = 9
        '
        'ProductPrefixField
        '
        Me.ProductPrefixField.Location = New System.Drawing.Point(253, 138)
        Me.ProductPrefixField.Name = "ProductPrefixField"
        Me.ProductPrefixField.Size = New System.Drawing.Size(100, 20)
        Me.ProductPrefixField.TabIndex = 8
        '
        'UsernamePrefixField
        '
        Me.UsernamePrefixField.Location = New System.Drawing.Point(253, 112)
        Me.UsernamePrefixField.Name = "UsernamePrefixField"
        Me.UsernamePrefixField.Size = New System.Drawing.Size(100, 20)
        Me.UsernamePrefixField.TabIndex = 5
        '
        'OrderNumberPrefixField
        '
        Me.OrderNumberPrefixField.Location = New System.Drawing.Point(253, 86)
        Me.OrderNumberPrefixField.Name = "OrderNumberPrefixField"
        Me.OrderNumberPrefixField.Size = New System.Drawing.Size(100, 20)
        Me.OrderNumberPrefixField.TabIndex = 4
        '
        'chkUseShipMethodPrefix
        '
        Me.chkUseShipMethodPrefix.AutoSize = True
        Me.chkUseShipMethodPrefix.Location = New System.Drawing.Point(128, 166)
        Me.chkUseShipMethodPrefix.Name = "chkUseShipMethodPrefix"
        Me.chkUseShipMethodPrefix.Size = New System.Drawing.Size(118, 17)
        Me.chkUseShipMethodPrefix.TabIndex = 7
        Me.chkUseShipMethodPrefix.Text = "Use Shipping Prefix"
        Me.chkUseShipMethodPrefix.UseVisualStyleBackColor = True
        '
        'chkUseProductPrefix
        '
        Me.chkUseProductPrefix.AutoSize = True
        Me.chkUseProductPrefix.Location = New System.Drawing.Point(128, 140)
        Me.chkUseProductPrefix.Name = "chkUseProductPrefix"
        Me.chkUseProductPrefix.Size = New System.Drawing.Size(114, 17)
        Me.chkUseProductPrefix.TabIndex = 6
        Me.chkUseProductPrefix.Text = "Use Product Prefix"
        Me.chkUseProductPrefix.UseVisualStyleBackColor = True
        '
        'chkUsernamePrefix
        '
        Me.chkUsernamePrefix.AutoSize = True
        Me.chkUsernamePrefix.Location = New System.Drawing.Point(128, 114)
        Me.chkUsernamePrefix.Name = "chkUsernamePrefix"
        Me.chkUsernamePrefix.Size = New System.Drawing.Size(121, 17)
        Me.chkUsernamePrefix.TabIndex = 1
        Me.chkUsernamePrefix.Text = "Use Customer Prefix"
        Me.chkUsernamePrefix.UseVisualStyleBackColor = True
        '
        'chkOrderPrefix
        '
        Me.chkOrderPrefix.AutoSize = True
        Me.chkOrderPrefix.Location = New System.Drawing.Point(128, 88)
        Me.chkOrderPrefix.Name = "chkOrderPrefix"
        Me.chkOrderPrefix.Size = New System.Drawing.Size(103, 17)
        Me.chkOrderPrefix.TabIndex = 0
        Me.chkOrderPrefix.Text = "Use Order Prefix"
        Me.chkOrderPrefix.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Label9)
        Me.TabPage2.Controls.Add(Me.Label10)
        Me.TabPage2.Controls.Add(Me.Label11)
        Me.TabPage2.Controls.Add(Me.Label12)
        Me.TabPage2.Controls.Add(Me.Label13)
        Me.TabPage2.Controls.Add(Me.Label14)
        Me.TabPage2.Controls.Add(Me.Label15)
        Me.TabPage2.Controls.Add(Me.Label8)
        Me.TabPage2.Controls.Add(Me.Label7)
        Me.TabPage2.Controls.Add(Me.Label6)
        Me.TabPage2.Controls.Add(Me.Label5)
        Me.TabPage2.Controls.Add(Me.Label4)
        Me.TabPage2.Controls.Add(Me.Label3)
        Me.TabPage2.Controls.Add(Me.Label17)
        Me.TabPage2.Controls.Add(Me.Label16)
        Me.TabPage2.Controls.Add(Me.FaxNameField)
        Me.TabPage2.Controls.Add(Me.CashNameField)
        Me.TabPage2.Controls.Add(Me.TelephoneNameField)
        Me.TabPage2.Controls.Add(Me.EmailNameField)
        Me.TabPage2.Controls.Add(Me.PONameField)
        Me.TabPage2.Controls.Add(Me.CheckNameField)
        Me.TabPage2.Controls.Add(Me.PayPalNameField)
        Me.TabPage2.Controls.Add(Me.OtherNameField)
        Me.TabPage2.Controls.Add(Me.JCBNameField)
        Me.TabPage2.Controls.Add(Me.DinersClubNameField)
        Me.TabPage2.Controls.Add(Me.AmexNameField)
        Me.TabPage2.Controls.Add(Me.DiscoverNameField)
        Me.TabPage2.Controls.Add(Me.MastercardNameField)
        Me.TabPage2.Controls.Add(Me.VisaNameField)
        Me.TabPage2.Controls.Add(Me.Label2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(530, 286)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Payment"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(248, 211)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(74, 23)
        Me.Label9.TabIndex = 42
        Me.Label9.Text = "Fax"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(248, 185)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(74, 23)
        Me.Label10.TabIndex = 41
        Me.Label10.Text = "Cash"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(248, 159)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(74, 23)
        Me.Label11.TabIndex = 40
        Me.Label11.Text = "Telephone"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(248, 133)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(74, 23)
        Me.Label12.TabIndex = 39
        Me.Label12.Text = "Email"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(248, 107)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(74, 23)
        Me.Label13.TabIndex = 38
        Me.Label13.Text = "PO"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(248, 81)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(74, 23)
        Me.Label14.TabIndex = 37
        Me.Label14.Text = "Check"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(248, 55)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(74, 23)
        Me.Label15.TabIndex = 36
        Me.Label15.Text = "PayPal"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(21, 211)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(74, 23)
        Me.Label8.TabIndex = 35
        Me.Label8.Text = "Other"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(21, 185)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(74, 23)
        Me.Label7.TabIndex = 34
        Me.Label7.Text = "JCB"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(21, 159)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(74, 23)
        Me.Label6.TabIndex = 33
        Me.Label6.Text = "Diner's Club"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(21, 133)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(74, 23)
        Me.Label5.TabIndex = 32
        Me.Label5.Text = "Amex"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(21, 107)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(74, 23)
        Me.Label4.TabIndex = 31
        Me.Label4.Text = "Discover"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(21, 81)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(74, 23)
        Me.Label3.TabIndex = 30
        Me.Label3.Text = "MasterCard"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(327, 33)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(119, 13)
        Me.Label17.TabIndex = 29
        Me.Label17.Text = "QuickBooks Item Name"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(99, 33)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(119, 13)
        Me.Label16.TabIndex = 28
        Me.Label16.Text = "QuickBooks Item Name"
        '
        'FaxNameField
        '
        Me.FaxNameField.Location = New System.Drawing.Point(328, 208)
        Me.FaxNameField.Name = "FaxNameField"
        Me.FaxNameField.Size = New System.Drawing.Size(125, 20)
        Me.FaxNameField.TabIndex = 27
        '
        'CashNameField
        '
        Me.CashNameField.Location = New System.Drawing.Point(328, 182)
        Me.CashNameField.Name = "CashNameField"
        Me.CashNameField.Size = New System.Drawing.Size(125, 20)
        Me.CashNameField.TabIndex = 25
        '
        'TelephoneNameField
        '
        Me.TelephoneNameField.Location = New System.Drawing.Point(328, 156)
        Me.TelephoneNameField.Name = "TelephoneNameField"
        Me.TelephoneNameField.Size = New System.Drawing.Size(125, 20)
        Me.TelephoneNameField.TabIndex = 23
        '
        'EmailNameField
        '
        Me.EmailNameField.Location = New System.Drawing.Point(328, 130)
        Me.EmailNameField.Name = "EmailNameField"
        Me.EmailNameField.Size = New System.Drawing.Size(125, 20)
        Me.EmailNameField.TabIndex = 21
        '
        'PONameField
        '
        Me.PONameField.Location = New System.Drawing.Point(328, 104)
        Me.PONameField.Name = "PONameField"
        Me.PONameField.Size = New System.Drawing.Size(125, 20)
        Me.PONameField.TabIndex = 19
        '
        'CheckNameField
        '
        Me.CheckNameField.Location = New System.Drawing.Point(328, 78)
        Me.CheckNameField.Name = "CheckNameField"
        Me.CheckNameField.Size = New System.Drawing.Size(125, 20)
        Me.CheckNameField.TabIndex = 17
        '
        'PayPalNameField
        '
        Me.PayPalNameField.Location = New System.Drawing.Point(328, 52)
        Me.PayPalNameField.Name = "PayPalNameField"
        Me.PayPalNameField.Size = New System.Drawing.Size(125, 20)
        Me.PayPalNameField.TabIndex = 15
        '
        'OtherNameField
        '
        Me.OtherNameField.Location = New System.Drawing.Point(101, 208)
        Me.OtherNameField.Name = "OtherNameField"
        Me.OtherNameField.Size = New System.Drawing.Size(125, 20)
        Me.OtherNameField.TabIndex = 13
        '
        'JCBNameField
        '
        Me.JCBNameField.Location = New System.Drawing.Point(101, 182)
        Me.JCBNameField.Name = "JCBNameField"
        Me.JCBNameField.Size = New System.Drawing.Size(125, 20)
        Me.JCBNameField.TabIndex = 11
        '
        'DinersClubNameField
        '
        Me.DinersClubNameField.Location = New System.Drawing.Point(101, 156)
        Me.DinersClubNameField.Name = "DinersClubNameField"
        Me.DinersClubNameField.Size = New System.Drawing.Size(125, 20)
        Me.DinersClubNameField.TabIndex = 9
        '
        'AmexNameField
        '
        Me.AmexNameField.Location = New System.Drawing.Point(101, 130)
        Me.AmexNameField.Name = "AmexNameField"
        Me.AmexNameField.Size = New System.Drawing.Size(125, 20)
        Me.AmexNameField.TabIndex = 7
        '
        'DiscoverNameField
        '
        Me.DiscoverNameField.Location = New System.Drawing.Point(101, 104)
        Me.DiscoverNameField.Name = "DiscoverNameField"
        Me.DiscoverNameField.Size = New System.Drawing.Size(125, 20)
        Me.DiscoverNameField.TabIndex = 5
        '
        'MastercardNameField
        '
        Me.MastercardNameField.Location = New System.Drawing.Point(101, 78)
        Me.MastercardNameField.Name = "MastercardNameField"
        Me.MastercardNameField.Size = New System.Drawing.Size(125, 20)
        Me.MastercardNameField.TabIndex = 3
        '
        'VisaNameField
        '
        Me.VisaNameField.Location = New System.Drawing.Point(101, 52)
        Me.VisaNameField.Name = "VisaNameField"
        Me.VisaNameField.Size = New System.Drawing.Size(125, 20)
        Me.VisaNameField.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(21, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(74, 23)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Visa"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.chkRecordCreditCardInfo)
        Me.TabPage3.Controls.Add(Me.GroupBox4)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(530, 286)
        Me.TabPage3.TabIndex = 7
        Me.TabPage3.Text = "Customers"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'chkRecordCreditCardInfo
        '
        Me.chkRecordCreditCardInfo.AutoSize = True
        Me.chkRecordCreditCardInfo.Location = New System.Drawing.Point(117, 42)
        Me.chkRecordCreditCardInfo.Name = "chkRecordCreditCardInfo"
        Me.chkRecordCreditCardInfo.Size = New System.Drawing.Size(238, 17)
        Me.chkRecordCreditCardInfo.TabIndex = 1
        Me.chkRecordCreditCardInfo.Text = "Save Credit Card Information With Customers"
        Me.chkRecordCreditCardInfo.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.chkCreateCustomersAsCompany)
        Me.GroupBox4.Controls.Add(Me.rbExportReverseUserName)
        Me.GroupBox4.Controls.Add(Me.rbExportUserName)
        Me.GroupBox4.Controls.Add(Me.rbExportUserEmail)
        Me.GroupBox4.Location = New System.Drawing.Point(27, 103)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(431, 90)
        Me.GroupBox4.TabIndex = 0
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Customer Creation Options"
        '
        'chkCreateCustomersAsCompany
        '
        Me.chkCreateCustomersAsCompany.AutoSize = True
        Me.chkCreateCustomersAsCompany.Location = New System.Drawing.Point(260, 54)
        Me.chkCreateCustomersAsCompany.Name = "chkCreateCustomersAsCompany"
        Me.chkCreateCustomersAsCompany.Size = New System.Drawing.Size(160, 17)
        Me.chkCreateCustomersAsCompany.TabIndex = 2
        Me.chkCreateCustomersAsCompany.Text = "or as COMPANY if not blank"
        Me.chkCreateCustomersAsCompany.UseVisualStyleBackColor = True
        '
        'rbExportReverseUserName
        '
        Me.rbExportReverseUserName.AutoSize = True
        Me.rbExportReverseUserName.Location = New System.Drawing.Point(6, 65)
        Me.rbExportReverseUserName.Name = "rbExportReverseUserName"
        Me.rbExportReverseUserName.Size = New System.Drawing.Size(248, 17)
        Me.rbExportReverseUserName.TabIndex = 2
        Me.rbExportReverseUserName.TabStop = True
        Me.rbExportReverseUserName.Text = "Create Customers as FIRSTNAME LASTNAME"
        Me.rbExportReverseUserName.UseVisualStyleBackColor = True
        '
        'rbExportUserName
        '
        Me.rbExportUserName.AutoSize = True
        Me.rbExportUserName.Location = New System.Drawing.Point(6, 42)
        Me.rbExportUserName.Name = "rbExportUserName"
        Me.rbExportUserName.Size = New System.Drawing.Size(251, 17)
        Me.rbExportUserName.TabIndex = 1
        Me.rbExportUserName.TabStop = True
        Me.rbExportUserName.Text = "Create Customers as LASTNAME, FIRSTNAME"
        Me.rbExportUserName.UseVisualStyleBackColor = True
        '
        'rbExportUserEmail
        '
        Me.rbExportUserEmail.AutoSize = True
        Me.rbExportUserEmail.Location = New System.Drawing.Point(6, 19)
        Me.rbExportUserEmail.Name = "rbExportUserEmail"
        Me.rbExportUserEmail.Size = New System.Drawing.Size(212, 17)
        Me.rbExportUserEmail.TabIndex = 0
        Me.rbExportUserEmail.TabStop = True
        Me.rbExportUserEmail.Text = "Create Customers as EMAIL ADDRESS"
        Me.rbExportUserEmail.UseVisualStyleBackColor = True
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.chkUseExportDateForShipping)
        Me.TabPage1.Controls.Add(Me.chkMarkPaidOrdersToBePrinted)
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.chkUseExportDate)
        Me.TabPage1.Controls.Add(Me.chkMarkOrdersToBePrinted)
        Me.TabPage1.Controls.Add(Me.chkMarkUnpaidOrdersAsPending)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(530, 286)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Import Selection"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'chkUseExportDateForShipping
        '
        Me.chkUseExportDateForShipping.AutoSize = True
        Me.chkUseExportDateForShipping.Location = New System.Drawing.Point(299, 84)
        Me.chkUseExportDateForShipping.Name = "chkUseExportDateForShipping"
        Me.chkUseExportDateForShipping.Size = New System.Drawing.Size(173, 17)
        Me.chkUseExportDateForShipping.TabIndex = 6
        Me.chkUseExportDateForShipping.Text = "Use Current Date for Ship Date"
        Me.chkUseExportDateForShipping.UseVisualStyleBackColor = True
        '
        'chkMarkPaidOrdersToBePrinted
        '
        Me.chkMarkPaidOrdersToBePrinted.AutoSize = True
        Me.chkMarkPaidOrdersToBePrinted.Location = New System.Drawing.Point(426, 38)
        Me.chkMarkPaidOrdersToBePrinted.Name = "chkMarkPaidOrdersToBePrinted"
        Me.chkMarkPaidOrdersToBePrinted.Size = New System.Drawing.Size(88, 17)
        Me.chkMarkPaidOrdersToBePrinted.TabIndex = 5
        Me.chkMarkPaidOrdersToBePrinted.Text = "for PAID only"
        Me.chkMarkPaidOrdersToBePrinted.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.rbSalesOrders)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.btnBrowseAcctsRec)
        Me.GroupBox2.Controls.Add(Me.InvoiceAcctsRecField)
        Me.GroupBox2.Controls.Add(Me.btnBrowse1)
        Me.GroupBox2.Controls.Add(Me.SalesReceiptDepositAccountField)
        Me.GroupBox2.Controls.Add(Me.btnBrowse2)
        Me.GroupBox2.Controls.Add(Me.InvoiceDepositAccountField)
        Me.GroupBox2.Controls.Add(Me.chkDepositInvoices)
        Me.GroupBox2.Controls.Add(Me.chkDepositSalesReceipts)
        Me.GroupBox2.Controls.Add(Me.rbInvoices)
        Me.GroupBox2.Controls.Add(Me.rbSalesReceipts)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 103)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(518, 177)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Import Mode"
        '
        'rbSalesOrders
        '
        Me.rbSalesOrders.AutoSize = True
        Me.rbSalesOrders.Location = New System.Drawing.Point(6, 141)
        Me.rbSalesOrders.Name = "rbSalesOrders"
        Me.rbSalesOrders.Size = New System.Drawing.Size(129, 17)
        Me.rbSalesOrders.TabIndex = 14
        Me.rbSalesOrders.Text = "Import to Sales Orders"
        Me.rbSalesOrders.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(39, 116)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(121, 13)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Accounts Receivable to"
        '
        'btnBrowseAcctsRec
        '
        Me.btnBrowseAcctsRec.Location = New System.Drawing.Point(396, 112)
        Me.btnBrowseAcctsRec.Name = "btnBrowseAcctsRec"
        Me.btnBrowseAcctsRec.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseAcctsRec.TabIndex = 12
        Me.btnBrowseAcctsRec.Text = "Browse"
        Me.btnBrowseAcctsRec.UseVisualStyleBackColor = True
        '
        'InvoiceAcctsRecField
        '
        Me.InvoiceAcctsRecField.Location = New System.Drawing.Point(169, 113)
        Me.InvoiceAcctsRecField.Name = "InvoiceAcctsRecField"
        Me.InvoiceAcctsRecField.Size = New System.Drawing.Size(221, 20)
        Me.InvoiceAcctsRecField.TabIndex = 11
        '
        'btnBrowse1
        '
        Me.btnBrowse1.Location = New System.Drawing.Point(396, 86)
        Me.btnBrowse1.Name = "btnBrowse1"
        Me.btnBrowse1.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse1.TabIndex = 10
        Me.btnBrowse1.Text = "Browse"
        Me.btnBrowse1.UseVisualStyleBackColor = True
        '
        'SalesReceiptDepositAccountField
        '
        Me.SalesReceiptDepositAccountField.Location = New System.Drawing.Point(169, 42)
        Me.SalesReceiptDepositAccountField.Name = "SalesReceiptDepositAccountField"
        Me.SalesReceiptDepositAccountField.Size = New System.Drawing.Size(221, 20)
        Me.SalesReceiptDepositAccountField.TabIndex = 9
        '
        'btnBrowse2
        '
        Me.btnBrowse2.Location = New System.Drawing.Point(396, 40)
        Me.btnBrowse2.Name = "btnBrowse2"
        Me.btnBrowse2.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse2.TabIndex = 8
        Me.btnBrowse2.Text = "Browse"
        Me.btnBrowse2.UseVisualStyleBackColor = True
        '
        'InvoiceDepositAccountField
        '
        Me.InvoiceDepositAccountField.Location = New System.Drawing.Point(169, 86)
        Me.InvoiceDepositAccountField.Name = "InvoiceDepositAccountField"
        Me.InvoiceDepositAccountField.Size = New System.Drawing.Size(221, 20)
        Me.InvoiceDepositAccountField.TabIndex = 7
        '
        'chkDepositInvoices
        '
        Me.chkDepositInvoices.AutoSize = True
        Me.chkDepositInvoices.Location = New System.Drawing.Point(40, 88)
        Me.chkDepositInvoices.Name = "chkDepositInvoices"
        Me.chkDepositInvoices.Size = New System.Drawing.Size(123, 17)
        Me.chkDepositInvoices.TabIndex = 6
        Me.chkDepositInvoices.Text = "Deposit Payments to"
        Me.chkDepositInvoices.UseVisualStyleBackColor = True
        '
        'chkDepositSalesReceipts
        '
        Me.chkDepositSalesReceipts.AutoSize = True
        Me.chkDepositSalesReceipts.Location = New System.Drawing.Point(40, 42)
        Me.chkDepositSalesReceipts.Name = "chkDepositSalesReceipts"
        Me.chkDepositSalesReceipts.Size = New System.Drawing.Size(123, 17)
        Me.chkDepositSalesReceipts.TabIndex = 5
        Me.chkDepositSalesReceipts.Text = "Deposit Payments to"
        Me.chkDepositSalesReceipts.UseVisualStyleBackColor = True
        '
        'rbInvoices
        '
        Me.rbInvoices.AutoSize = True
        Me.rbInvoices.Location = New System.Drawing.Point(6, 65)
        Me.rbInvoices.Name = "rbInvoices"
        Me.rbInvoices.Size = New System.Drawing.Size(109, 17)
        Me.rbInvoices.TabIndex = 1
        Me.rbInvoices.Text = "Import to Invoices"
        Me.rbInvoices.UseVisualStyleBackColor = True
        '
        'rbSalesReceipts
        '
        Me.rbSalesReceipts.AutoSize = True
        Me.rbSalesReceipts.Checked = True
        Me.rbSalesReceipts.Location = New System.Drawing.Point(6, 19)
        Me.rbSalesReceipts.Name = "rbSalesReceipts"
        Me.rbSalesReceipts.Size = New System.Drawing.Size(140, 17)
        Me.rbSalesReceipts.TabIndex = 0
        Me.rbSalesReceipts.TabStop = True
        Me.rbSalesReceipts.Text = "Import to Sales Receipts"
        Me.rbSalesReceipts.UseVisualStyleBackColor = True
        '
        'chkUseExportDate
        '
        Me.chkUseExportDate.AutoSize = True
        Me.chkUseExportDate.Location = New System.Drawing.Point(299, 61)
        Me.chkUseExportDate.Name = "chkUseExportDate"
        Me.chkUseExportDate.Size = New System.Drawing.Size(225, 17)
        Me.chkUseExportDate.TabIndex = 4
        Me.chkUseExportDate.Text = "Use Current Date Instead of Time of Order"
        Me.chkUseExportDate.UseVisualStyleBackColor = True
        '
        'chkMarkOrdersToBePrinted
        '
        Me.chkMarkOrdersToBePrinted.AutoSize = True
        Me.chkMarkOrdersToBePrinted.Location = New System.Drawing.Point(299, 38)
        Me.chkMarkOrdersToBePrinted.Name = "chkMarkOrdersToBePrinted"
        Me.chkMarkOrdersToBePrinted.Size = New System.Drawing.Size(121, 17)
        Me.chkMarkOrdersToBePrinted.TabIndex = 3
        Me.chkMarkOrdersToBePrinted.Text = "use ""To Be Printed"""
        Me.chkMarkOrdersToBePrinted.UseVisualStyleBackColor = True
        '
        'chkMarkUnpaidOrdersAsPending
        '
        Me.chkMarkUnpaidOrdersAsPending.AutoSize = True
        Me.chkMarkUnpaidOrdersAsPending.Checked = True
        Me.chkMarkUnpaidOrdersAsPending.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMarkUnpaidOrdersAsPending.Location = New System.Drawing.Point(299, 15)
        Me.chkMarkUnpaidOrdersAsPending.Name = "chkMarkUnpaidOrdersAsPending"
        Me.chkMarkUnpaidOrdersAsPending.Size = New System.Drawing.Size(177, 17)
        Me.chkMarkUnpaidOrdersAsPending.TabIndex = 2
        Me.chkMarkUnpaidOrdersAsPending.Text = "Mark Unpaid Orders as Pending"
        Me.chkMarkUnpaidOrdersAsPending.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbImportAuth)
        Me.GroupBox1.Controls.Add(Me.rbImportAll)
        Me.GroupBox1.Controls.Add(Me.rbImportPaid)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(171, 96)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Order Status"
        '
        'rbImportAuth
        '
        Me.rbImportAuth.AutoSize = True
        Me.rbImportAuth.Location = New System.Drawing.Point(6, 42)
        Me.rbImportAuth.Name = "rbImportAuth"
        Me.rbImportAuth.Size = New System.Drawing.Size(136, 17)
        Me.rbImportAuth.TabIndex = 2
        Me.rbImportAuth.Text = "Import PAID and AUTH"
        Me.rbImportAuth.UseVisualStyleBackColor = True
        '
        'rbImportAll
        '
        Me.rbImportAll.AutoSize = True
        Me.rbImportAll.Location = New System.Drawing.Point(6, 65)
        Me.rbImportAll.Name = "rbImportAll"
        Me.rbImportAll.Size = New System.Drawing.Size(102, 17)
        Me.rbImportAll.TabIndex = 1
        Me.rbImportAll.Text = "Import All Orders"
        Me.rbImportAll.UseVisualStyleBackColor = True
        '
        'rbImportPaid
        '
        Me.rbImportPaid.AutoSize = True
        Me.rbImportPaid.Checked = True
        Me.rbImportPaid.Location = New System.Drawing.Point(6, 19)
        Me.rbImportPaid.Name = "rbImportPaid"
        Me.rbImportPaid.Size = New System.Drawing.Size(104, 17)
        Me.rbImportPaid.TabIndex = 0
        Me.rbImportPaid.TabStop = True
        Me.rbImportPaid.Text = "Import PAID only"
        Me.rbImportPaid.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Controls.Add(Me.TabPage6)
        Me.TabControl1.Controls.Add(Me.TabPage7)
        Me.TabControl1.Controls.Add(Me.TabPage9)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Multiline = True
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(538, 312)
        Me.TabControl1.TabIndex = 0
        '
        'chkDisableTax
        '
        Me.chkDisableTax.AutoSize = True
        Me.chkDisableTax.Location = New System.Drawing.Point(185, 53)
        Me.chkDisableTax.Name = "chkDisableTax"
        Me.chkDisableTax.Size = New System.Drawing.Size(103, 17)
        Me.chkDisableTax.TabIndex = 13
        Me.chkDisableTax.Text = "Do Not Use Tax"
        Me.chkDisableTax.UseVisualStyleBackColor = True
        '
        'OptionsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(565, 372)
        Me.ControlBox = False
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonOkay)
        Me.Controls.Add(Me.TabControl1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "OptionsForm"
        Me.Text = "Options"
        Me.TabPage9.ResumeLayout(False)
        Me.TabPage9.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.TabPage7.ResumeLayout(False)
        Me.TabPage7.PerformLayout()
        Me.TabPage6.ResumeLayout(False)
        Me.TabPage6.PerformLayout()
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage5.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage4.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonOkay As System.Windows.Forms.Button
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents TabPage9 As System.Windows.Forms.TabPage
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents InventoryAssetAccountField As System.Windows.Forms.TextBox
    Friend WithEvents COGAccountField As System.Windows.Forms.TextBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents chkCreateAsInventory As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents chkSendPrices As System.Windows.Forms.CheckBox
    Friend WithEvents chkSendInventory As System.Windows.Forms.CheckBox
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents chkUseInlineTax As System.Windows.Forms.CheckBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents InlineTaxVendorField As System.Windows.Forms.TextBox
    Friend WithEvents InlineTaxItemField As System.Windows.Forms.TextBox
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents btnBrowseShippingAccount As System.Windows.Forms.Button
    Friend WithEvents btnBrowseHandlingAccount As System.Windows.Forms.Button
    Friend WithEvents HandlingIncomeAccountField As System.Windows.Forms.TextBox
    Friend WithEvents HandlingItemNameField As System.Windows.Forms.TextBox
    Friend WithEvents ShippingItemIncomeAccount As System.Windows.Forms.TextBox
    Friend WithEvents ShippingItemName As System.Windows.Forms.TextBox
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents chkUseSubProductNameMatching As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseQuickBooksDescription As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents rbExportProductsSKU As System.Windows.Forms.RadioButton
    Friend WithEvents rbExportProductsName As System.Windows.Forms.RadioButton
    Friend WithEvents btnBrowseNewItemIncome As System.Windows.Forms.Button
    Friend WithEvents NewItemIncomeAccountField As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents ShipMethodPrefixField As System.Windows.Forms.TextBox
    Friend WithEvents ProductPrefixField As System.Windows.Forms.TextBox
    Friend WithEvents UsernamePrefixField As System.Windows.Forms.TextBox
    Friend WithEvents OrderNumberPrefixField As System.Windows.Forms.TextBox
    Friend WithEvents chkUseShipMethodPrefix As System.Windows.Forms.CheckBox
    Friend WithEvents chkUseProductPrefix As System.Windows.Forms.CheckBox
    Friend WithEvents chkUsernamePrefix As System.Windows.Forms.CheckBox
    Friend WithEvents chkOrderPrefix As System.Windows.Forms.CheckBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents FaxNameField As System.Windows.Forms.TextBox
    Friend WithEvents CashNameField As System.Windows.Forms.TextBox
    Friend WithEvents TelephoneNameField As System.Windows.Forms.TextBox
    Friend WithEvents EmailNameField As System.Windows.Forms.TextBox
    Friend WithEvents PONameField As System.Windows.Forms.TextBox
    Friend WithEvents CheckNameField As System.Windows.Forms.TextBox
    Friend WithEvents PayPalNameField As System.Windows.Forms.TextBox
    Friend WithEvents OtherNameField As System.Windows.Forms.TextBox
    Friend WithEvents JCBNameField As System.Windows.Forms.TextBox
    Friend WithEvents DinersClubNameField As System.Windows.Forms.TextBox
    Friend WithEvents AmexNameField As System.Windows.Forms.TextBox
    Friend WithEvents DiscoverNameField As System.Windows.Forms.TextBox
    Friend WithEvents MastercardNameField As System.Windows.Forms.TextBox
    Friend WithEvents VisaNameField As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents chkRecordCreditCardInfo As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents chkCreateCustomersAsCompany As System.Windows.Forms.CheckBox
    Friend WithEvents rbExportReverseUserName As System.Windows.Forms.RadioButton
    Friend WithEvents rbExportUserName As System.Windows.Forms.RadioButton
    Friend WithEvents rbExportUserEmail As System.Windows.Forms.RadioButton
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents chkUseExportDateForShipping As System.Windows.Forms.CheckBox
    Friend WithEvents chkMarkPaidOrdersToBePrinted As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents rbSalesOrders As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnBrowseAcctsRec As System.Windows.Forms.Button
    Friend WithEvents InvoiceAcctsRecField As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse1 As System.Windows.Forms.Button
    Friend WithEvents SalesReceiptDepositAccountField As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowse2 As System.Windows.Forms.Button
    Friend WithEvents InvoiceDepositAccountField As System.Windows.Forms.TextBox
    Friend WithEvents chkDepositInvoices As System.Windows.Forms.CheckBox
    Friend WithEvents chkDepositSalesReceipts As System.Windows.Forms.CheckBox
    Friend WithEvents rbInvoices As System.Windows.Forms.RadioButton
    Friend WithEvents rbSalesReceipts As System.Windows.Forms.RadioButton
    Friend WithEvents chkUseExportDate As System.Windows.Forms.CheckBox
    Friend WithEvents chkMarkOrdersToBePrinted As System.Windows.Forms.CheckBox
    Friend WithEvents chkMarkUnpaidOrdersAsPending As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbImportAuth As System.Windows.Forms.RadioButton
    Friend WithEvents rbImportAll As System.Windows.Forms.RadioButton
    Friend WithEvents rbImportPaid As System.Windows.Forms.RadioButton
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents chkProductUseTitleOnly As System.Windows.Forms.CheckBox
    Friend WithEvents chkDisableTax As System.Windows.Forms.CheckBox
End Class
