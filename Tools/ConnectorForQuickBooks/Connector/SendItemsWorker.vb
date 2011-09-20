' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt

Public Class SendItemsWorker
    Private _Items As New Collection(Of QBInventoryItem)
    Private _Errors As New System.Collections.Specialized.StringCollection
    Private _ProcessComplete As Boolean = False

    Private Sub w(ByVal message As String)
        Logging.Write(message)
    End Sub

    Private Sub wln(ByVal message As String)
        Logging.WriteLine(message)
    End Sub

    Public Sub SendItemsToWeb()
        Me.ButtonCancel.Enabled = False
        Me.btnClose.Enabled = False

        _ProcessComplete = False
        _Errors.Clear()
        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = 100
        Me.lblStatus.Text = "Loading Items From QuickBooks..."
        Me.lblExportCount.Text = String.Empty
        Application.DoEvents()
        Dim pack As New SendItemsPacket
        pack.Items = New Collection(Of QBInventoryItem)
        pack.Password = My.Settings.WebPassword
        pack.UserName = My.Settings.WebUsename
        pack.Url = My.Settings.WebStoreUrl
        pack.service5 = MainForm.WebStore5
        pack.Token5 = MainForm.WebToken5
        Me.BackgroundWorker1.RunWorkerAsync(pack)
    End Sub

    'Private Function GetBVC2004Order(ByVal orderID As Integer) As BVC2004WebServices.Order
    '    Dim result As BVC2004WebServices.Order

    '    wln("Starting BVC 2004 Order Download at: " & Now.ToString)
    '    result = MainForm.WebStore2004.OrderServices_GetExistingOrder(MainForm.WebToken2004, orderID)
    '    wln("Finished BVC 2004 Order Download at: " & Now.ToString)

    '    If result Is Nothing Then
    '        wln("ERROR: Order Object was not Created!")
    '        Throw New Exception("Failed to load Order Ref# " & orderID & " from store.")
    '    End If

    '    Return result
    'End Function

    'Public Function GetBVC5OrderByBvin(ByVal bvin As String) As BVC5WebServices.Order
    '    Dim result As New BVC5WebServices.Order
    '    wln("Finding BVC5 Order by Bvin")
    '    result = MainForm.WebStore5.Orders_Order_FindByBvin(MainForm.WebToken5, bvin)
    '    wln("Finished Finding BVC5 Order by Bvin")

    '    If result Is Nothing Then
    '        wln("ERROR: Order from BVC5 was nothing")
    '        result = New BVC5WebServices.Order
    '    End If

    '    If result.Bvin = String.Empty Then
    '        wln("ERROR: Order Object was not Found in BVC5!")
    '    End If

    '    Return result
    'End Function

    Private Sub BackgroundWorker1_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

        Dim q As New QBFC7Lib.QBSessionManager
        Dim _utils As New QuickBooksUtils

        wln("Attempting to Open QBFC Connection")
        q.OpenConnection2("", My.Settings.QBApplicationName, QBFC7Lib.ENConnectionType.ctLocalQBD)
        wln("QBFC Connection Opened")

        If My.Settings.UseCompanyFile = True Then
            wln("Using Company File Name: " & My.Settings.CompanyFileName)
            q.BeginSession(My.Settings.CompanyFileName, QBFC7Lib.ENOpenMode.omDontCare)
        Else
            wln("Using currently open QB File")
            q.BeginSession("", QBFC7Lib.ENOpenMode.omDontCare)
        End If

        Dim ver As QBVersionInfo = _utils.GetQuickBooksVersion(q)
        wln("QuickBooks Version = " & ver.MajorVersion & "." & ver.MinorVersion & " - " & ver.CountryCode)

        Dim packet As SendItemsPacket = CType(e.Argument, SendItemsPacket)
        packet.Items = QuickBooksUtils.ListInventoryItems(q, ver)
        e.Result = packet
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Dim pack As SendItemsPacket = CType(e.Result, SendItemsPacket)
        If pack IsNot Nothing Then
            Dim foundItems As Collection(Of QBInventoryItem) = pack.Items
            If foundItems IsNot Nothing Then
                Me._Items = foundItems
                Me.ButtonCancel.Enabled = True
                Me.lblStatus.Text = "Sending " & _Items.Count & " items to Web Store"
                Me.ProgressBar1.Minimum = 0
                Me.ProgressBar1.Maximum = 100
                Me.lblExportCount.Text = "Exporting Items..."
                Application.DoEvents()
                BackgroundWorker2.RunWorkerAsync(pack)
            Else
                Me.btnClose.Enabled = True
                Me.ButtonCancel.Enabled = False
                Throw New ArgumentException("Items list from QuickBooks was nothing!")
            End If
        Else
            Me.btnClose.Enabled = True
            Me.ButtonCancel.Enabled = False
            Throw New ArgumentException("Packet was nothing from Worker1")
        End If
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
        Me.BackgroundWorker2.CancelAsync()
    End Sub

    Private Sub BackgroundWorker2_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker2.DoWork

        Dim packet As SendItemsPacket = CType(e.Argument, SendItemsPacket)
        If packet IsNot Nothing Then
            Dim items As Collection(Of QBInventoryItem) = packet.Items
            If items IsNot Nothing Then

                Dim percentComplete As Integer = 0

                For i As Integer = 0 To items.Count - 1

                    ' check cancel at start of loop
                    If BackgroundWorker2.CancellationPending Then
                        e.Cancel = True
                        Return
                    End If

                    ' report progress
                    percentComplete = CInt(((i + 1) / items.Count) * 100)
                    If percentComplete > 100 Then
                        percentComplete = 100
                    End If
                    BackgroundWorker2.ReportProgress(percentComplete, i + 1)


                    ' Update Data on Web Site
                    SendItemData(items(i), packet)

                    ' check cancel at end of loop
                    If BackgroundWorker2.CancellationPending Then
                        e.Cancel = True
                        Return
                    End If
                Next
            End If
        Else
            Me.btnClose.Enabled = True
            Me.ButtonCancel.Enabled = False
        End If

    End Sub

    Private Sub BackgroundWorker2_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker2.ProgressChanged
        Me.lblExportCount.Text = "Exporting Item " & CInt(e.UserState)
        Me.ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker2_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker2.RunWorkerCompleted
        Me.btnClose.Enabled = True
        Me.ButtonCancel.Enabled = False
        Me.lblStatus.Text = "Finished with Updates!"
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub SendItemsWorker_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.SendItemsToWeb()
    End Sub

    Private Sub SendItemData(ByVal item As QBInventoryItem, ByVal p As SendItemsPacket)
        If item IsNot Nothing Then
            ' Make sure we have a SKU, otherwise skip the product
            If item.SKU.Trim <> String.Empty Then
                SendItemData5(item, p)
            End If        
        End If
    End Sub

    Private Sub SendItemData5(ByVal item As QBInventoryItem, ByVal p As SendItemsPacket)

        Dim bvitem As New BVC5WebServices.Product
        bvitem = p.service5.Catalog_InternalProduct_FindBySku(p.Token5, item.SKU)
        If bvitem IsNot Nothing Then
            If bvitem.Sku.Trim.ToLower = item.SKU.Trim.ToLower Then
                If My.Settings.SendPricesToWeb Then
                    Dim tempP As New BVC5WebServices.Product
                    tempP = bvitem
                    tempP.Saved = True
                    tempP.GlobalProduct.Saved = True
                    tempP.GlobalProduct.SitePrice = item.Price1
                    tempP.GlobalProduct.SiteCost = item.Cost
                    tempP.SitePrice = item.Price1
                    tempP.SiteCost = item.Cost
                    Dim result As Boolean = p.service5.Catalog_InternalProduct_Update(p.Token5, tempP)
                End If
                If My.Settings.SendInventoryToWeb Then
                    p.service5.Catalog_ProductInventory_SetAvailableQuantity(p.Token5, bvitem.Bvin, item.OnHandQuantity)
                End If
            End If
        End If
    End Sub

    Public Shared Sub InitProduct(ByRef result As BVC5WebServices.InternalProduct)
        'result.AdditionalImages
        result.Bvin = String.Empty
        result.CreationDate = DateTime.Now
        result.ExtraShipFee = 0D
        result.GiftCertificateCodePattern = String.Empty
        result.GiftWrapAllowed = True
        result.ImageFileMedium = String.Empty
        result.ImageFileSmall = String.Empty
        result.Keywords = String.Empty
        result.LastUpdated = DateTime.Now
        result.ListPrice = 0D
        result.LongDescription = String.Empty
        result.ManufacturerId = String.Empty
        result.MetaDescription = String.Empty
        result.MetaKeywords = String.Empty
        result.MetaTitle = String.Empty
        result.MinimumQty = 0
        result.NonShipping = False
        result.ParentId = String.Empty
        result.PreTransformLongDescription = String.Empty
        result.ProductName = String.Empty
        result.ProductTypeId = String.Empty
        result.ShippingHeight = 0D
        result.ShippingLength = 0D
        result.ShippingMode = BVC5WebServices.ShippingMode.ShipFromSite
        result.ShippingWeight = 0D
        result.ShippingWidth = 0D
        result.ShipSeparately = False
        result.ShortDescription = String.Empty
        result.SiteCost = 0D
        result.SitePrice = 0D
        result.Sku = String.Empty
        result.SpecialProductType = BVC5WebServices.SpecialProductTypes.Normal
        result.Status = BVC5WebServices.ProductStatus.Active
        result.TaxClass = String.Empty
        result.TaxExempt = False
        result.TemplateName = "Bvc5"
        result.TrackInventory = False
        result.VariantDisplay = BVC5WebServices.VariantDisplayMode.IndividualFields
        result.VendorId = String.Empty

        result.SitePriceOverrideText = String.Empty
        result.RewriteUrl = String.Empty
        result.PreContentColumnId = String.Empty
        result.PostContentColumnId = String.Empty
        result.ProductURL = String.Empty

        '          AdditionalImages	Nothing	BvcImportTool.BVweb.ProductImage()
        'Categories	Nothing	String()
        'ChoiceCombinations	Nothing	BvcImportTool.BVweb.ProductChoiceCombination()
        'PostContentColumnId	Nothing	String
        'PreContentColumnId	Nothing	String
        'ProductURL	Nothing	String
        'Reviews	Nothing	BvcImportTool.BVweb.ProductReview()
        'RewriteUrl	Nothing	String
        'SitePriceOverrideText	Nothing	String


    End Sub

End Class