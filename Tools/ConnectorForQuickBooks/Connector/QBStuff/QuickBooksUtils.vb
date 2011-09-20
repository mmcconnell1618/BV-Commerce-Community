' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt


Public Class QuickBooksUtils

    Public Const UnitedStatesBVC5Guid As String = "bf7389a2-9b21-4d33-b276-23c9c18ea0c0"

    Private Shared Function SafeDouble(ByVal input As Decimal) As Double
        Dim result As Decimal = Math.Round(input, 2)
        Return CDbl(result)
    End Function

    Private Shared Function SafeDouble(ByVal input As Double) As Double
        Return SafeDouble(CDec(input))
    End Function

    Private Shared Function SafeDouble(ByVal input As Integer) As Double
        Return SafeDouble(CDec(input))
    End Function

    Private Shared Sub log(ByVal message As String)
        Logging.WriteLine(message)
    End Sub

    Public Function GetQuickBooksVersion(ByRef ses As QBFC7Lib.QBSessionManager) As QBVersionInfo

        Dim result As New QBVersionInfo
        result.CountryCode = "US"
        result.MinorVersion = "0"
        result.MajorVersion = "1"

        Dim answer() As String
        answer = CType(ses.QBXMLVersionsForSession(), String())

        Dim vinfo As String = CType(answer(answer.Length - 1), String)

        If vinfo.StartsWith("CA") Then
            result.CountryCode = "CA"
            result.MajorVersion = "6"
            result.MinorVersion = "0"
            ' Verion 6.0 SDK only supports non US with version 6,0
            'result.MajorVersion = vinfo.Substring(2, 1)
            'result.MinorVersion = vinfo.Substring(4, 1)
        Else
            If vinfo.StartsWith("UK") Then
                result.CountryCode = "UK"
                result.MajorVersion = "6"
                result.MinorVersion = "0"
                'result.MajorVersion = vinfo.Substring(0, 1)
                'result.MinorVersion = vinfo.Substring(2, 1)
            Else
                result.CountryCode = "US"
                result.MajorVersion = vinfo.Substring(0, 1)
                result.MinorVersion = vinfo.Substring(2, 1)
            End If

        End If

        Debug.Assert(result.CountryCode.Length > 0)
        Debug.Assert(result.MinorVersion.Length > 0)
        Debug.Assert(result.MajorVersion.Length > 0)

        Return result
    End Function

    Public Shared Function TrimToLength(ByVal input As String, ByVal maxLength As Integer) As String
        If input Is Nothing Then
            Return String.Empty
            'Throw New ArgumentNullException("input", "TrimToLength: Input parameter was null")
        Else
            If input.Length < 1 Then
                Return input
            Else
                If maxLength < 0 Then
                    maxLength = input.Length
                End If

                If input.Length > maxLength Then
                    Return input.Substring(0, maxLength)
                Else
                    Return input
                End If
            End If
        End If

    End Function

    Private Function GetFinalOrderID(ByVal inputID As String) As String
        Dim result As String = inputID

        If My.Settings.UseOrderPrefix = True Then
            result = My.Settings.OrderPrefix & inputID
            result = TrimToLength(result, 11)
        End If

        Return result
    End Function

    Private Function GetFinalUserName(ByVal input As String) As String
        Dim result As String = input

        If My.Settings.UseUsernamePrefix = True Then
            result = My.Settings.UseUsernamePrefix & input
            result = TrimToLength(result, 41)
        End If

        Return result
    End Function

    Private Function GetFinalProductName(ByVal input As String) As String
        Dim result As String = input

        If My.Settings.UseProductPrefix = True Then
            result = My.Settings.ProductPrefix & input
            result = TrimToLength(result, 31)
        End If
        Return result
    End Function

#Region " Customer "

    Public Function CustomerExists(ByVal username As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBCustomerInfo
        Dim result As QBCustomerInfo = New QBCustomerInfo
        result.ListID = "-1"

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.ICustomerQuery = rs.AppendCustomerQueryRq

        Dim fullname As String = GetFinalUserName(username)
        req.ORCustomerListQuery.FullNameList.Add(fullname)

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim customerInfo As QBFC7Lib.ICustomerRetList = CType(responseList.Detail, QBFC7Lib.ICustomerRetList)

            If Not customerInfo Is Nothing Then
                If customerInfo.Count >= 1 Then

                    If Not customerInfo.GetAt(0).ListID Is Nothing Then
                        result.ListID = customerInfo.GetAt(0).ListID.GetValue
                    End If

                    If Not customerInfo.GetAt(0).Name Is Nothing Then
                        result.Username = customerInfo.GetAt(0).Name.GetValue
                    End If

                    If Not customerInfo.GetAt(0).FirstName Is Nothing Then
                        result.FirstName = customerInfo.GetAt(0).FirstName.GetValue
                    End If

                    If Not customerInfo.GetAt(0).LastName Is Nothing Then
                        result.Lastname = customerInfo.GetAt(0).LastName.GetValue
                    End If

                    If Not customerInfo.GetAt(0).Email Is Nothing Then
                        result.Email = customerInfo.GetAt(0).Email.GetValue
                    End If

                    If Not customerInfo.GetAt(0).BillAddress Is Nothing Then
                        result.BillingAddress.LoadQBFCAddress(customerInfo.GetAt(0).BillAddress, ver)
                    End If

                    If Not customerInfo.GetAt(0).ShipAddress Is Nothing Then
                        result.ShippingAddress.LoadQBFCAddress(customerInfo.GetAt(0).ShipAddress, ver)
                    End If

                    If Not customerInfo.GetAt(0).EditSequence Is Nothing Then
                        result.EditSequence = customerInfo.GetAt(0).EditSequence.GetValue
                    End If

                    If Not customerInfo.GetAt(0).Phone Is Nothing Then
                        result.PhoneNumber = customerInfo.GetAt(0).Phone.GetValue
                    End If

                End If
            End If
        End If

        Return result
    End Function

    Public Function CustomerCreate(ByVal username As String, ByVal accountnumber As String, ByRef customer As QBCustomerInfo, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBCustomerInfo
        Dim result As New QBCustomerInfo
        result.ListID = "-1"

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.ICustomerAdd = rs.AppendCustomerAddRq

        req.Name.SetValue(GetFinalUserName(username))

        req.AccountNumber.SetValue(TrimToLength(accountnumber, 99))
        req.BillAddress.Addr1.SetValue(customer.BillingAddress.Addr1)
        req.BillAddress.Addr2.SetValue(customer.BillingAddress.Addr2)
        req.BillAddress.City.SetValue(customer.BillingAddress.City)
        req.BillAddress.State.SetValue(customer.BillingAddress.State)
        req.BillAddress.PostalCode.SetValue(customer.BillingAddress.PostalCode)
        req.BillAddress.Country.SetValue(customer.BillingAddress.Country)
        'req.BillAddress.Addr3.SetValue(customer.BillingAddress.Addr3)
        'req.BillAddress.Addr4.SetValue(customer.BillingAddress.Addr4)
        'req.BillAddress.Addr5.SetValue(customer.BillingAddress.Addr5)

        If customer.BillingAddress.Company.Length > 0 Then
            req.CompanyName.SetValue(customer.BillingAddress.Company)
        End If

        req.Email.SetValue(TrimToLength(customer.Email, 99))
        req.FirstName.SetValue(TrimToLength(customer.FirstName, 25))
        If customer.MiddleInitial <> String.Empty Then
            req.MiddleName.SetValue(TrimToLength(customer.MiddleInitial, 5))
        End If
        req.LastName.SetValue(TrimToLength(customer.Lastname, 25))
        req.Phone.SetValue(TrimToLength(customer.PhoneNumber, 21))

        req.ShipAddress.Addr1.SetValue(customer.ShippingAddress.Addr1)
        req.ShipAddress.Addr2.SetValue(customer.ShippingAddress.Addr2)
        req.ShipAddress.City.SetValue(customer.ShippingAddress.City)
        req.ShipAddress.State.SetValue(customer.ShippingAddress.State)
        req.ShipAddress.PostalCode.SetValue(customer.ShippingAddress.PostalCode)
        req.ShipAddress.Country.SetValue(customer.ShippingAddress.Country)
        'req.ShipAddress.Addr3.SetValue(customer.ShippingAddress.Addr3)
        'req.ShipAddress.Addr4.SetValue(customer.ShippingAddress.Addr4)
        'req.ShipAddress.Addr5.SetValue(customer.ShippingAddress.Addr5)

        ' Tax Code Listing
        Dim taxID As String = "-1"
        taxID = Me.TaxItemExists(My.Settings.InlineTaxItemName, ses, ver).ListId
        If taxID = "-1" Then
            taxID = Me.TaxItemCreate(My.Settings.InlineTaxItemName, ses, ver).ListId
        End If
        If taxID <> "-1" Then

            If customer.TaxExempt Then
                req.SalesTaxCodeRef.FullName.SetValue("NON")
            Else
                req.SalesTaxCodeRef.FullName.SetValue("TAX")
            End If
            req.ItemSalesTaxRef.FullName.SetValue(My.Settings.InlineTaxItemName)
            req.ItemSalesTaxRef.ListID.SetValue(taxID)

            If ver.CountryCode = "UK" Or ver.CountryCode = "CA" Then
                req.SalesTaxCountry.SetValue(ver.CountryCode)
            End If

        End If

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim customerInfo As QBFC7Lib.ICustomerRet = CType(responseList.Detail, QBFC7Lib.ICustomerRet)

            If Not customerInfo Is Nothing Then
                result.ListID = customerInfo.ListID.GetValue
                result.Username = customerInfo.Name.GetValue
                result.FirstName = customerInfo.FirstName.GetValue
                result.Lastname = customerInfo.LastName.GetValue
                result.Email = customerInfo.Email.GetValue
                If Not customerInfo.BillAddress Is Nothing Then
                    result.BillingAddress.LoadQBFCAddress(customerInfo.BillAddress, ver)
                End If
                If Not customerInfo.ShipAddress Is Nothing Then
                    result.ShippingAddress.LoadQBFCAddress(customerInfo.ShipAddress, ver)
                End If
                If Not customerInfo.Phone Is Nothing Then
                    result.PhoneNumber = customerInfo.Phone.GetValue
                End If
                If Not customerInfo.EditSequence Is Nothing Then
                    result.EditSequence = customerInfo.EditSequence.GetValue
                End If
            End If

            If responseList.StatusSeverity = "Error" Then
                log("Error: " & responseList.StatusCode & "-" & responseList.StatusMessage)
                If customerInfo IsNot Nothing Then
                    result.ListID = "ERROR:" & result.ListID
                End If
            End If

        End If

        Return result
    End Function

    Public Function CustomerUpdate(ByVal listID As String, ByRef customer As QBCustomerInfo, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As Boolean
        Dim result As Boolean = False

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.ICustomerMod = rs.AppendCustomerModRq

        req.ListID.SetValue(listID)
        req.EditSequence.SetValue(customer.EditSequence)

        If customer.BillingAddress.Company.Length > 0 Then
            req.CompanyName.SetValue(TrimToLength(customer.BillingAddress.Company, 41))
        End If

        req.BillAddress.Addr1.SetValue(customer.BillingAddress.Addr1)
        req.BillAddress.Addr2.SetValue(customer.BillingAddress.Addr2)
        req.BillAddress.City.SetValue(customer.BillingAddress.City)
        req.BillAddress.State.SetValue(customer.BillingAddress.State)
        req.BillAddress.PostalCode.SetValue(customer.BillingAddress.PostalCode)
        req.BillAddress.Country.SetValue(customer.BillingAddress.Country)
        'req.BillAddress.Addr3.SetValue(customer.BillingAddress.Addr3)
        'req.BillAddress.Addr4.SetValue(customer.BillingAddress.Addr4)
        'req.BillAddress.Addr5.SetValue(customer.BillingAddress.Addr5)

        req.Email.SetValue(TrimToLength(customer.Email, 99))
        req.FirstName.SetValue(TrimToLength(customer.FirstName, 25))
        If customer.MiddleInitial <> String.Empty Then
            req.MiddleName.SetValue(TrimToLength(customer.MiddleInitial, 5))
        End If
        req.LastName.SetValue(TrimToLength(customer.Lastname, 25))
        If Not customer.PhoneNumber Is Nothing Then
            req.Phone.SetValue(TrimToLength(customer.PhoneNumber, 21))
        End If

        req.ShipAddress.Addr1.SetValue(customer.ShippingAddress.Addr1)
        req.ShipAddress.Addr2.SetValue(customer.ShippingAddress.Addr2)
        req.ShipAddress.City.SetValue(customer.ShippingAddress.City)
        req.ShipAddress.State.SetValue(customer.ShippingAddress.State)
        req.ShipAddress.PostalCode.SetValue(customer.ShippingAddress.PostalCode)
        req.ShipAddress.Country.SetValue(customer.ShippingAddress.Country)
        'req.ShipAddress.Addr3.SetValue(customer.ShippingAddress.Addr3)
        'req.ShipAddress.Addr4.SetValue(customer.ShippingAddress.Addr4)
        'req.ShipAddress.Addr5.SetValue(customer.ShippingAddress.Addr5)


        If My.Settings.SendCreditCardInfo = True Then
            Dim IsValidCard As Boolean = True

            If customer.CreditCardInfo.ExpirationMonth < 1 Then
                IsValidCard = False
            End If
            If (IsValidCard = True) And (customer.CreditCardInfo.ExpirationYear < 1900) Then
                IsValidCard = False
            End If
            If (IsValidCard = True) And (customer.CreditCardInfo.NameOnCard <> Nothing) Then
                If customer.CreditCardInfo.NameOnCard.Trim.Length < 1 Then
                    IsValidCard = False
                End If
            End If
            If (IsValidCard = True) And (customer.CreditCardInfo.CreditCardNumber <> Nothing) Then
                If customer.CreditCardInfo.CreditCardNumber.Trim.Length < 15 Then
                    IsValidCard = False
                End If
            End If

            If IsValidCard = True Then
                req.CreditCardInfo.CreditCardAddress.SetValue(customer.CreditCardInfo.CreditCardAddress)
                req.CreditCardInfo.CreditCardNumber.SetValue(customer.CreditCardInfo.CreditCardNumber)
                req.CreditCardInfo.CreditCardPostalCode.SetValue(customer.CreditCardInfo.CreditCardPostalCode)
                req.CreditCardInfo.ExpirationMonth.SetValue(customer.CreditCardInfo.ExpirationMonth)
                req.CreditCardInfo.ExpirationYear.SetValue(customer.CreditCardInfo.ExpirationYear)
                req.CreditCardInfo.NameOnCard.SetValue(customer.CreditCardInfo.NameOnCard)
            End If

        End If

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            If responseList.StatusSeverity = "Error" Then
                log("Error: " & responseList.StatusCode & "-" & responseList.StatusMessage)
                result = False
            Else
                result = True
            End If
            Dim customerInfo As QBFC7Lib.ICustomerRet = CType(responseList.Detail, QBFC7Lib.ICustomerRet)
        End If

        Return result
    End Function

#End Region

#Region " Invoices "

    Public Function InvoiceExists(ByVal orderID As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As Boolean
        Dim result As Boolean = False

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IInvoiceQuery = rs.AppendInvoiceQueryRq

        If My.Settings.UseOrderPrefix = True Then
            req.ORInvoiceQuery.RefNumberList.Add(TrimToLength(My.Settings.OrderPrefix & orderID, 11))
        Else
            req.ORInvoiceQuery.RefNumberList.Add(TrimToLength(orderID, 11))
        End If

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)


        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.IInvoiceRetList = CType(responseList.Detail, QBFC7Lib.IInvoiceRetList)

            If Not info Is Nothing Then
                If info.Count >= 1 Then
                    result = True
                End If
            End If
        End If

        Return result
    End Function

    Public Function InvoiceCreate5(ByRef o As BVC5WebServices.Order, ByVal listID As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As Boolean
        Dim result As Boolean = False

        log("Starting Invoice Create")

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IInvoiceAdd = rs.AppendInvoiceAddRq

        req.CustomerRef.ListID.SetValue(listID)

        ' Order Number
        Dim newOrderID As String = GetFinalOrderID(o.OrderNumber)
        log("Order Number = " & newOrderID)
        req.RefNumber.SetValue(TrimToLength(newOrderID, 11))

        ' Transaction Info
        If My.Settings.UseExportDateInsteadOfOrderDate = True Then
            req.TxnDate.SetValue(System.DateTime.Now.Date)
        Else
            req.TxnDate.SetValue(o.TimeOfOrder.Date)
        End If
        If My.Settings.UseExportDateForShipping = True Then
            req.ShipDate.SetValue(System.DateTime.Now.Date)
        Else
            req.ShipDate.SetValue(o.TimeOfOrder.Date)
        End If

        req.Memo.SetValue("Auto created by " & Application.ProductName)
        If My.Settings.MarkOrdersToBePrinted = True Then
            If My.Settings.MarkPaidOrdersToBePrinted = True Then
                If o.PaymentStatus = BVC5WebServices.OrderPaymentStatus.Paid Then
                    req.IsToBePrinted.SetValue(True)
                Else
                    req.IsToBePrinted.SetValue(False)
                End If
            Else
                req.IsToBePrinted.SetValue(True)
            End If
        Else
            req.IsToBePrinted.SetValue(False)
        End If

        ' Mark Unpaid Orders
        If o.PaymentStatus <> BVC5WebServices.OrderPaymentStatus.Paid Then
            req.IsPending.SetValue(My.Settings.MarkUnpaidOrdersAsPending)
        Else
            req.IsPending.SetValue(False)
        End If

        Dim billA As New QBAddress(o.BillingAddress, CInt(ver.MajorVersion))
        req.BillAddress.Addr1.SetValue(billA.Addr1)
        req.BillAddress.Addr2.SetValue(billA.Addr2)
        req.BillAddress.City.SetValue(billA.City)
        req.BillAddress.State.SetValue(billA.State)
        req.BillAddress.PostalCode.SetValue(billA.PostalCode)
        req.BillAddress.Country.SetValue(billA.Country)
        'req.BillAddress.Addr3.SetValue(customer.BillingAddress.Addr3)
        'req.BillAddress.Addr4.SetValue(customer.BillingAddress.Addr4)
        'req.BillAddress.Addr5.SetValue(customer.BillingAddress.Addr5)

        Dim shipA As New QBAddress(o.ShippingAddress, CInt(ver.MajorVersion))
        req.ShipAddress.Addr1.SetValue(shipA.Addr1)
        req.ShipAddress.Addr2.SetValue(shipA.Addr2)
        req.ShipAddress.City.SetValue(shipA.City)
        req.ShipAddress.State.SetValue(shipA.State)
        req.ShipAddress.PostalCode.SetValue(shipA.PostalCode)
        req.ShipAddress.Country.SetValue(shipA.Country)
        'req.ShipAddress.Addr3.SetValue(shipA.Addr3)
        'req.ShipAddress.Addr4.SetValue(shipA.Addr4)
        'req.ShipAddress.Addr5.SetValue(shipA.Addr5)    


        ' Items
        log("Adding Items to Invoice")
        If Not o.Items Is Nothing Then
            For i As Integer = 0 To o.Items.Length - 1
                With o.Items(i)

                    ' Get ListID for item
                    Dim itemInfo As New QBItemInfo
                    If My.Settings.ExportProductsAsSKU = True Then
                        itemInfo = Me.ProductExists(o.Items(i).ProductSku, ses, ver)
                    Else
                        itemInfo = Me.ProductExists(.ProductName, ses, ver)
                    End If

                    If itemInfo.ListId = "-1" Then
                        log("Creating New Item in QuickBooks: " & .ProductSku)
                        Dim newItemPrice As Double = .LineTotal / .Quantity
                        If My.Settings.ExportProductsAsSKU = True Then
                            itemInfo = ProductCreate(.ProductSku, .ProductName, SafeDouble(newItemPrice), ses, ver)
                        Else
                            itemInfo = ProductCreate(.ProductName, .ProductSku & " " & .ProductName, SafeDouble(newItemPrice), ses, ver)
                        End If
                    End If

                    ' Create the first line item for the invoice
                    Dim lineItem As QBFC7Lib.IInvoiceLineAdd
                    lineItem = req.ORInvoiceLineAddList.Append.InvoiceLineAdd

                    lineItem.ItemRef.ListID.SetValue(itemInfo.ListId)

                    lineItem.Amount.SetValue(SafeDouble(.LineTotal))

                    ' Version 3, Set Adjusted Site Price and Line Totals
                    'lineItem.ORRate.Rate.SetValue(CDbl(.AdjustedSitePrice))

                    lineItem.Quantity.SetValue(CDbl(.Quantity))

                    Dim desc As String = ""
                    If My.Settings.ExportProductsAsSKU = True Then
                        desc = .ProductName
                    Else
                        desc = .ProductSku
                    End If

                    If Not My.Settings.ProductUseTitleOnly Then
                        If .ProductShortDescription <> "" Then
                            desc += " - " & .ProductShortDescription
                        End If
                    End If

                    desc = desc.Replace("<br>", ", ")
                    desc = desc.Replace("<b>", "")
                    desc = desc.Replace("</b>", "")

                    If My.Settings.UseQuickBooksItemDescription = False Then
                        lineItem.Desc.SetValue(TrimToLength(desc, 4095))
                    End If

                    desc = Nothing

                    ' Tag items as taxable or not if applicable                    
                    If Not My.Settings.DisableTaxes Then
                        If My.Settings.UseInlineTax = False Then
                            If .AssociatedProduct.TaxExempt = True Then
                                lineItem.SalesTaxCodeRef.FullName.SetValue("NON")
                            Else
                                lineItem.SalesTaxCodeRef.FullName.SetValue("TAX")
                            End If
                        Else
                            ' Using Inline Tax so Set everyting to "NON"
                            lineItem.SalesTaxCodeRef.FullName.SetValue("NON")
                        End If
                    End If

                End With
            Next
        End If

        ' Shipping
        log("Adding Shipping to Invoice")
        Dim shipMethodID As String = "-1"
        shipMethodID = Me.ShipMethodExists(o.ShippingMethodDisplayName, ses, ver)
        If shipMethodID = "-1" Then
            shipMethodID = Me.ShipMethodCreate(o.ShippingMethodDisplayName, ses, ver)
        End If
        If shipMethodID <> "-1" Then
            req.ShipMethodRef.ListID.SetValue(shipMethodID)
        End If

        ' Create or get shipping charge item
        Dim ShipItemInfo As New QBItemInfo
        ShipItemInfo = Me.ShipItemExists(My.Settings.ShippingItemName, ses, ver)
        If ShipItemInfo.ListId = "-1" Then
            ShipItemInfo = Me.ShippingItemCreate(My.Settings.ShippingItemName, ses, ver)
        End If

        If (ShipItemInfo.ListId <> "-1") Then
            ' Version 3 adds explicit shipping line support
            'If CDbl(ver.MajorVersion) >= 4 Then
            '    req.ShippingLineAdd.Amount.SetValue(o.Packages(0).ShippingCost)
            '    req.ShippingLineAdd.AccountRef.ListID.SetValue(ShipItemInfo.ListId)
            'Else
            ' Add Shipping Charge Line Item
            ' Create the first line item for the invoice
            Dim shiplineItem As QBFC7Lib.IInvoiceLineAdd
            shiplineItem = req.ORInvoiceLineAddList.Append.InvoiceLineAdd
            shiplineItem.ItemRef.ListID.SetValue(ShipItemInfo.ListId)
            shiplineItem.Amount.SetValue(SafeDouble(o.ShippingTotal))
            shiplineItem.Desc.SetValue(TrimToLength(o.ShippingMethodDisplayName, 4095))
            shiplineItem.Quantity.SetValue(1)
            'End If
        End If

        'Handling
        log("Adding Handling to Invoice")
        If o.HandlingTotal > 0 Then
            Dim handlingID As String = "-1"
            handlingID = Me.HandlingItemExists(My.Settings.HandlingItemName, ses, ver).ListId
            If handlingID = "-1" Then
                handlingID = Me.HandlingItemCreate(My.Settings.HandlingItemName, ses, ver).ListId
            End If
            If handlingID <> "-1" Then
                Dim hlineItem As QBFC7Lib.IInvoiceLineAdd
                hlineItem = req.ORInvoiceLineAddList.Append.InvoiceLineAdd
                hlineItem.ItemRef.ListID.SetValue(handlingID)
                hlineItem.Amount.SetValue(SafeDouble(o.HandlingTotal))
                hlineItem.Desc.SetValue(TrimToLength("Handling Charges", 4095))
                hlineItem.Quantity.SetValue(1)
            End If
        End If

        'Special Instructions
        log("Adding Special Instructions to Invoice")
        If o.Instructions.Trim.Length > 0 Then
            Dim memoID As String = "-1"
            memoID = Me.MemoItemExists(ses, ver).ListId
            If memoID = "-1" Then
                memoID = Me.MemoItemCreate(ses, ver).ListId
            End If
            If memoID <> "-1" Then
                Dim hlineItem As QBFC7Lib.IInvoiceLineAdd
                hlineItem = req.ORInvoiceLineAddList.Append.InvoiceLineAdd
                hlineItem.ItemRef.ListID.SetValue(memoID)
                hlineItem.Amount.SetValue(SafeDouble(0))
                hlineItem.Desc.SetValue(TrimToLength(o.Instructions, 4095))
                hlineItem.Quantity.SetValue(1)
            End If
        End If

        log("Adding Tax to Invoice")
        If Not My.Settings.DisableTaxes Then
            If o.TaxTotal > 0 Then
                If My.Settings.UseInlineTax = True Then

                    Dim taxID As String = "-1"
                    taxID = Me.TaxItemExists(My.Settings.InlineTaxItemName, ses, ver).ListId
                    If taxID = "-1" Then
                        taxID = Me.TaxItemCreate(My.Settings.InlineTaxItemName, ses, ver).ListId
                    End If
                    If taxID <> "-1" Then
                        Dim taxlineItem As QBFC7Lib.IInvoiceLineAdd
                        taxlineItem = req.ORInvoiceLineAddList.Append.InvoiceLineAdd
                        taxlineItem.ItemRef.ListID.SetValue(taxID)
                        taxlineItem.Amount.SetValue(SafeDouble(o.TaxTotal))
                        taxlineItem.Desc.SetValue(TrimToLength("BV Calculated Taxes", 4095))
                        'taxlineItem.SalesTaxCodeRef.FullName.SetValue("NON")
                        'taxlineItem.Quantity.SetValue(1)
                    End If
                End If
            End If
        End If


        'Total discount applied to order.
        log("Adding Discounts to Invoice")

        Dim OrderDiscountAmount As Decimal = 0
        OrderDiscountAmount += o.OrderDiscounts
        OrderDiscountAmount += o.ShippingDiscounts
        If o.CustomProperties IsNot Nothing Then
            For q As Integer = 0 To o.CustomProperties.Length - 1
                If o.CustomProperties(q).DeveloperId = "bvsoftware" Then
                    If o.CustomProperties(q).Key = "postorderadjustment" Then
                        Dim postOrderAdjustment As Decimal = 0
                        Decimal.TryParse(o.CustomProperties(q).Value, postOrderAdjustment)
                        OrderDiscountAmount += postOrderAdjustment
                    End If
                End If
            Next
        End If

        If OrderDiscountAmount > 0 Then
            Dim orderDiscountTotalID As String = "-1"
            orderDiscountTotalID = Me.OrderDiscountItemExists(ses, ver).ListId
            If orderDiscountTotalID = "-1" Then
                orderDiscountTotalID = Me.OrderDiscountItemCreate(ses, ver).ListId
                'MsgBox("Discount item does not exist")
            End If
            If orderDiscountTotalID <> "-1" Then

                Dim lineItem As QBFC7Lib.IInvoiceLineAdd
                lineItem = req.ORInvoiceLineAddList.Append.InvoiceLineAdd
                lineItem.ItemRef.ListID.SetValue(orderDiscountTotalID)
                lineItem.Amount.SetValue(-1 * SafeDouble(OrderDiscountAmount))
                lineItem.Quantity.SetValue(SafeDouble(1))
                lineItem.Desc.SetValue(TrimToLength("BV Commerce Discounts", 4095))
                'If ver.CountryCode <> "CA" Then
                If Not My.Settings.DisableTaxes Then
                    lineItem.SalesTaxCodeRef.FullName.SetValue("NON")
                End If
                'End If
            End If
        End If

        ' Set Account Receivable Account
        log("Setting Accounts Receivable to: " & My.Settings.InvoiceAccountsReceivableAccount)
        If My.Settings.InvoiceAccountsReceivableAccount.Trim.Length > 0 Then
            If AccountExists(My.Settings.InvoiceAccountsReceivableAccount, ses, ver).ListId <> "-1" Then
                req.ARAccountRef.FullName.SetValue(TrimToLength(My.Settings.InvoiceAccountsReceivableAccount, 31))
            End If
        End If

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        log("Attempting Transaction")
        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            If responseList.StatusSeverity = "Error" Then
                Logging.WriteLine("ERROR: " & responseList.StatusMessage)
            End If

            If responseList.StatusCode <> 0 Then
                log("Error: " & o.OrderNumber & " " & responseList.StatusMessage)
                Trace.Write(o.OrderNumber & " " & responseList.StatusMessage)
                result = False
            Else
                Dim ret As QBFC7Lib.IInvoiceRet = CType(responseList.Detail, QBFC7Lib.IInvoiceRet)

                If Not ret Is Nothing Then
                    If ret.RefNumber.GetValue = req.RefNumber.GetValue Then
                        log("Setting Result to True")
                        result = True
                    Else
                        log("Ref Numbers Did not Match")
                    End If
                Else
                    If responseList.StatusCode <> 0 Then
                        If responseList.StatusSeverity = "Error" Then
                            'Me.errors += "(" & o.ID & " Error) " & responseList.StatusMessage
                            log("Error Code: " & o.OrderNumber & " " & responseList.StatusMessage)
                            result = False
                        End If
                    End If
                End If
            End If
        End If

        ' Receive Payments if all went well
        log("Starting Receive Payment for Invoice")
        If result = True Then
            If (o.PaymentStatus = BVC5WebServices.OrderPaymentStatus.Paid) And (o.GrandTotal > 0) Then

                ' Get Pay Info ID
                Dim PayInfo As New QBItemInfo
                PayInfo = GetPaymentMethodInformationForOrder5(o, ses, ver, False)
                log("PayInfo ListID: " & PayInfo.ListId)

                ' Get Deposit Account Name
                Dim depositAccountName As String = ""
                If My.Settings.DepositInvoices = True Then
                    If My.Settings.InvoiceDepostAccount.Trim.Length > 0 Then
                        If AccountExists(My.Settings.InvoiceDepostAccount, ses, ver).ListId <> "-1" Then
                            depositAccountName = TrimToLength(My.Settings.InvoiceDepostAccount, 31)
                            log("Deposit Account: " & depositAccountName)
                        Else
                            log("No Deposit Account Found")
                        End If
                    End If
                End If

                log("Calling RecordPayment: " & listID & "," & o.GrandTotal & "," & PayInfo.ListId & "," & depositAccountName & "," & o.TimeOfOrder)
                Me.RecordPayment(listID, o.GrandTotal, PayInfo.ListId, "Auto Created By " & Application.ProductName, depositAccountName, o.TimeOfOrder, ses, ver)

            End If
        End If

        Return result
    End Function

#End Region

#Region " Sales Receipts "

    Public Function SalesReceiptExists(ByVal orderID As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As Boolean
        Dim result As Boolean = False

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.ISalesReceiptQuery = rs.AppendSalesReceiptQueryRq

        If My.Settings.UseOrderPrefix = True Then
            req.ORTxnQuery.RefNumberList.Add(TrimToLength(My.Settings.OrderPrefix & orderID, 11))
        Else
            req.ORTxnQuery.RefNumberList.Add(TrimToLength(orderID, 11))
        End If

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.ISalesReceiptRetList = CType(responseList.Detail, QBFC7Lib.ISalesReceiptRetList)

            If Not info Is Nothing Then
                If info.Count >= 1 Then
                    result = True
                End If
            End If
        End If

        Return result
    End Function

    Public Function SalesReceiptCreate5(ByRef o As BVC5WebServices.Order, ByVal listID As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As Boolean
        Dim result As Boolean = False

        ' Session Setup
        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        ' Create Request
        Dim req As QBFC7Lib.ISalesReceiptAdd = rs.AppendSalesReceiptAddRq

        req.CustomerRef.ListID.SetValue(listID)

        ' Order Number
        Dim newOrderID As String = GetFinalOrderID(o.OrderNumber)
        req.RefNumber.SetValue(TrimToLength(newOrderID, 11))

        ' Transaction Info
        If My.Settings.UseExportDateInsteadOfOrderDate = True Then
            req.TxnDate.SetValue(System.DateTime.Now.Date)
        Else
            req.TxnDate.SetValue(o.TimeOfOrder)
        End If
        If My.Settings.UseExportDateForShipping = True Then
            req.ShipDate.SetValue(System.DateTime.Now.Date)
        Else
            req.ShipDate.SetValue(o.TimeOfOrder.Date)
        End If

        req.Memo.SetValue("Auto created by " & Application.ProductName)
        If My.Settings.MarkOrdersToBePrinted = True Then
            If My.Settings.MarkPaidOrdersToBePrinted = True Then
                If o.PaymentStatus = BVC5WebServices.OrderPaymentStatus.Paid Then
                    req.IsToBePrinted.SetValue(True)
                Else
                    req.IsToBePrinted.SetValue(False)
                End If
            Else
                req.IsToBePrinted.SetValue(True)
            End If
        Else
            req.IsToBePrinted.SetValue(False)
        End If

        ' Mark Unpaid Orders
        If o.PaymentStatus <> BVC5WebServices.OrderPaymentStatus.Paid Then
            req.IsPending.SetValue(My.Settings.MarkUnpaidOrdersAsPending)
        Else
            req.IsPending.SetValue(False)
            If My.Settings.DepositSalesReceipts = True Then
                If My.Settings.SalesReceiptDepositAccount.Trim.Length > 0 Then
                    If AccountExists(My.Settings.SalesReceiptDepositAccount, ses, ver).ListId <> "-1" Then
                        req.DepositToAccountRef.FullName.SetValue(TrimToLength(My.Settings.SalesReceiptDepositAccount, 31))
                    End If
                End If
            End If
        End If


        Dim billA As New QBAddress(o.BillingAddress, CInt(ver.MajorVersion))
        req.BillAddress.Addr1.SetValue(billA.Addr1)
        req.BillAddress.Addr2.SetValue(billA.Addr2)
        req.BillAddress.City.SetValue(billA.City)
        req.BillAddress.State.SetValue(billA.State)
        req.BillAddress.PostalCode.SetValue(billA.PostalCode)
        req.BillAddress.Country.SetValue(billA.Country)
        'req.BillAddress.Addr3.SetValue(customer.BillingAddress.Addr3)
        'req.BillAddress.Addr4.SetValue(customer.BillingAddress.Addr4)
        'req.BillAddress.Addr5.SetValue(customer.BillingAddress.Addr5)

        Dim shipA As New QBAddress(o.ShippingAddress, CInt(ver.MajorVersion))
        req.ShipAddress.Addr1.SetValue(shipA.Addr1)
        req.ShipAddress.Addr2.SetValue(shipA.Addr2)
        req.ShipAddress.City.SetValue(shipA.City)
        req.ShipAddress.State.SetValue(shipA.State)
        req.ShipAddress.PostalCode.SetValue(shipA.PostalCode)
        req.ShipAddress.Country.SetValue(shipA.Country)
        'req.ShipAddress.Addr3.SetValue(shipA.Addr3)
        'req.ShipAddress.Addr4.SetValue(shipA.Addr4)
        'req.ShipAddress.Addr5.SetValue(shipA.Addr5)

        ' Payment Info
        Dim PayInfo As New QBItemInfo
        PayInfo = GetPaymentMethodInformationForOrder5(o, ses, ver, True)
        If PayInfo.ListId <> "-1" Then
            req.PaymentMethodRef.ListID.SetValue(PayInfo.ListId)
        End If

        ' Items
        If Not o.Items Is Nothing Then
            For i As Integer = 0 To o.Items.Length - 1
                With o.Items(i)

                    ' Get ListID for item
                    Dim itemInfo As New QBItemInfo
                    If My.Settings.ExportProductsAsSKU = True Then
                        itemInfo = Me.ProductExists(.ProductSku, ses, ver)
                    Else
                        itemInfo = Me.ProductExists(.ProductName, ses, ver)
                    End If

                    If itemInfo.ListId = "-1" Then
                        Dim newItemPrice As Decimal = Math.Round((.LineTotal / .Quantity), 2)
                        If My.Settings.ExportProductsAsSKU = True Then
                            itemInfo = ProductCreate(.ProductSku, .ProductName, newItemPrice, ses, ver)
                        Else
                            itemInfo = ProductCreate(.ProductName, .ProductSku & " " & .ProductName, newItemPrice, ses, ver)
                        End If
                    End If

                    ' Create the first line item for the invoice
                    Dim lineItem As QBFC7Lib.ISalesReceiptLineAdd
                    lineItem = req.ORSalesReceiptLineAddList.Append.SalesReceiptLineAdd

                    lineItem.ItemRef.ListID.SetValue(itemInfo.ListId)

                    lineItem.Amount.SetValue(SafeDouble(.LineTotal))

                    Dim desc As String = ""
                    If My.Settings.ExportProductsAsSKU = True Then
                        desc = .ProductName
                        If Not My.Settings.ProductUseTitleOnly Then
                            If .ProductShortDescription.Trim.Length > 0 Then
                                desc += " - " & .ProductShortDescription
                            End If
                        End If
                    Else
                        desc = .ProductSku
                        If Not My.Settings.ProductUseTitleOnly Then
                            If .ProductShortDescription.Trim.Length > 0 Then
                                desc += " - " & .ProductShortDescription
                            End If
                        End If
                    End If

                    desc = desc.Replace("<br>", ", ")
                    desc = desc.Replace("<b>", "")
                    desc = desc.Replace("</b>", "")

                    If My.Settings.UseQuickBooksItemDescription = False Then
                        lineItem.Desc.SetValue(TrimToLength(desc, 4095))
                    End If

                    desc = Nothing

                    lineItem.Quantity.SetValue(CInt(.Quantity))

                    ' Tag items as taxable or not if applicable
                    If Not My.Settings.DisableTaxes Then
                        If My.Settings.UseInlineTax = False Then
                            If .AssociatedProduct.TaxExempt = True Then
                                lineItem.SalesTaxCodeRef.FullName.SetValue("NON")
                            Else
                                lineItem.SalesTaxCodeRef.FullName.SetValue("TAX")
                            End If
                        Else
                            ' Using Inline Tax so Set everyting to "NON"
                            lineItem.SalesTaxCodeRef.FullName.SetValue("NON")
                        End If
                    End If

                End With
            Next
        End If

        ' Shipping
        Dim shipMethodID As String = "-1"
        shipMethodID = Me.ShipMethodExists(o.ShippingMethodDisplayName, ses, ver)
        If shipMethodID = "-1" Then
            shipMethodID = Me.ShipMethodCreate(o.ShippingMethodDisplayName, ses, ver)
        End If
        If shipMethodID <> "-1" Then
            req.ShipMethodRef.ListID.SetValue(shipMethodID)
        End If

        ' Create or get shipping charge item
        Dim ShipItemInfo As New QBItemInfo
        ShipItemInfo = Me.ShipItemExists(My.Settings.ShippingItemName, ses, ver)
        If ShipItemInfo.ListId = "-1" Then
            ShipItemInfo = Me.ShippingItemCreate(My.Settings.ShippingItemName, ses, ver)
        End If
        If (ShipItemInfo.ListId <> "-1") Then
            ' Version 3 adds explicit shipping line support
            'If CDbl(ver.MajorVersion) >= 4 Then
            '    req.ShippingLineAdd.Amount.SetValue(o.Packages(0).ShippingCost)
            '    req.ShippingLineAdd.AccountRef.ListID.SetValue(ShipItemInfo.ListId)
            'Else
            ' Add Shipping Charge Line Item
            ' Create the first line item for the invoice
            Dim shiplineItem As QBFC7Lib.ISalesReceiptLineAdd
            shiplineItem = req.ORSalesReceiptLineAddList.Append.SalesReceiptLineAdd
            shiplineItem.ItemRef.ListID.SetValue(ShipItemInfo.ListId)
            shiplineItem.Amount.SetValue(SafeDouble(o.ShippingTotal))
            shiplineItem.Desc.SetValue(TrimToLength(o.ShippingMethodDisplayName, 4095))
            shiplineItem.Quantity.SetValue(1)
            'End If
        End If

        'Handling
        If o.HandlingTotal > 0 Then
            Dim handlingID As String = "-1"
            handlingID = Me.HandlingItemExists(My.Settings.HandlingItemName, ses, ver).ListId
            If handlingID = "-1" Then
                handlingID = Me.HandlingItemCreate(My.Settings.HandlingItemName, ses, ver).ListId
            End If
            If handlingID <> "-1" Then
                Dim hlineItem As QBFC7Lib.ISalesReceiptLineAdd
                hlineItem = req.ORSalesReceiptLineAddList.Append.SalesReceiptLineAdd
                hlineItem.ItemRef.ListID.SetValue(handlingID)
                hlineItem.Amount.SetValue(SafeDouble(o.HandlingTotal))
                hlineItem.Desc.SetValue(TrimToLength("Handling Charges", 4095))
                hlineItem.Quantity.SetValue(1)
            End If
        End If


        'Special Instructions
        If o.Instructions.Trim.Length > 0 Then
            Dim memoID As String = "-1"
            memoID = Me.MemoItemExists(ses, ver).ListId
            If memoID = "-1" Then
                memoID = Me.MemoItemCreate(ses, ver).ListId
            End If
            If memoID <> "-1" Then
                Dim hlineItem As QBFC7Lib.ISalesReceiptLineAdd
                hlineItem = req.ORSalesReceiptLineAddList.Append.SalesReceiptLineAdd
                hlineItem.ItemRef.ListID.SetValue(memoID)
                hlineItem.Amount.SetValue(SafeDouble(0))
                hlineItem.Desc.SetValue(TrimToLength(o.Instructions, 4095))
                hlineItem.Quantity.SetValue(1)
            End If
        End If

        'If ver.CountryCode = "CA" Then
        '    req.Tax1Total.SetValue(o.TaxTotal)
        'Else
        If Not My.Settings.DisableTaxes Then
            If o.TaxTotal > 0 Then
                If My.Settings.UseInlineTax = True Then

                    Dim taxID As String = "-1"
                    taxID = Me.TaxItemExists(My.Settings.InlineTaxItemName, ses, ver).ListId
                    If taxID = "-1" Then
                        taxID = Me.TaxItemCreate(My.Settings.InlineTaxItemName, ses, ver).ListId
                    End If
                    If taxID <> "-1" Then
                        Dim taxlineItem As QBFC7Lib.ISalesReceiptLineAdd
                        taxlineItem = req.ORSalesReceiptLineAddList.Append.SalesReceiptLineAdd
                        taxlineItem.ItemRef.ListID.SetValue(taxID)
                        taxlineItem.Amount.SetValue(SafeDouble(o.TaxTotal))
                        taxlineItem.Desc.SetValue(TrimToLength("BV Calculated Taxes", 4095))
                        'taxlineItem.SalesTaxCodeRef.FullName.SetValue("NON")
                        'taxlineItem.Quantity.SetValue(1)
                    End If
                End If
            End If
        End If

        'Total discount applied to order.
        Dim OrderDiscountAmount As Decimal = 0
        OrderDiscountAmount += o.OrderDiscounts
        OrderDiscountAmount += o.ShippingDiscounts
        If o.CustomProperties IsNot Nothing Then
            For q As Integer = 0 To o.CustomProperties.Length - 1
                If o.CustomProperties(q).DeveloperId = "bvsoftware" Then
                    If o.CustomProperties(q).Key = "postorderadjustment" Then
                        Dim postOrderAdjustment As Decimal = 0
                        Decimal.TryParse(o.CustomProperties(q).Value, postOrderAdjustment)
                        OrderDiscountAmount += postOrderAdjustment
                    End If
                End If
            Next
        End If

        If OrderDiscountAmount > 0 Then
            Dim orderDiscountTotalID As String = "-1"
            orderDiscountTotalID = Me.OrderDiscountItemExists(ses, ver).ListId
            If orderDiscountTotalID = "-1" Then
                orderDiscountTotalID = Me.OrderDiscountItemCreate(ses, ver).ListId
            End If
            If orderDiscountTotalID <> "-1" Then

                Dim lineItem As QBFC7Lib.ISalesReceiptLineAdd
                lineItem = req.ORSalesReceiptLineAddList.Append.SalesReceiptLineAdd
                lineItem.ItemRef.ListID.SetValue(orderDiscountTotalID)
                lineItem.Amount.SetValue(-1 * SafeDouble(OrderDiscountAmount))
                lineItem.Quantity.SetValue(SafeDouble(1))
                lineItem.Desc.SetValue(TrimToLength("BV Commerce Discounts", 4095))
                'If ver.CountryCode <> "CA" Then
                If Not My.Settings.DisableTaxes Then
                    lineItem.SalesTaxCodeRef.FullName.SetValue("NON")
                End If
                'End If
            End If
        End If


        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            If responseList.StatusSeverity = "Error" Then
                Logging.WriteLine("ERROR: " & responseList.StatusMessage)
            End If

            If responseList.StatusCode <> 0 Then
                Trace.Write(o.OrderNumber & " " & responseList.StatusMessage)
                result = False
            Else
                Dim ret As QBFC7Lib.ISalesReceiptRet = CType(responseList.Detail, QBFC7Lib.ISalesReceiptRet)

                If Not ret Is Nothing Then
                    If ret.RefNumber.GetValue = req.RefNumber.GetValue Then
                        result = True
                    End If
                Else
                    If responseList.StatusCode <> 0 Then
                        If responseList.StatusSeverity = "Error" Then
                            'Me.errors += "(" & o.ID & " Error) " & responseList.StatusMessage
                            result = False
                        End If
                    End If
                End If
            End If
        End If

        Return result
    End Function

#End Region

#Region " Sales Orders "

    Public Function SalesOrderExists(ByVal orderID As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As Boolean
        Dim result As Boolean = False

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.ISalesOrderQuery = rs.AppendSalesOrderQueryRq

        If My.Settings.UseOrderPrefix = True Then
            req.ORTxnNoAccountQuery.RefNumberList.Add(TrimToLength(My.Settings.OrderPrefix & orderID, 11))
        Else
            req.ORTxnNoAccountQuery.RefNumberList.Add(TrimToLength(orderID, 11))
        End If

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.ISalesOrderRetList = CType(responseList.Detail, QBFC7Lib.ISalesOrderRetList)

            If Not info Is Nothing Then
                If info.Count >= 1 Then
                    result = True
                End If
            End If
        End If

        Return result
    End Function

    Public Function SalesOrderCreate5(ByRef o As BVC5WebServices.Order, ByVal listID As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As Boolean
        Dim result As Boolean = False

        ' Session Setup
        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        ' Create Request
        Dim req As QBFC7Lib.ISalesOrderAdd = rs.AppendSalesOrderAddRq

        req.CustomerRef.ListID.SetValue(listID)

        ' Order Number
        Dim newOrderID As String = GetFinalOrderID(o.OrderNumber)
        req.RefNumber.SetValue(TrimToLength(newOrderID, 11))

        ' Transaction Info
        If My.Settings.UseExportDateInsteadOfOrderDate = True Then
            req.TxnDate.SetValue(System.DateTime.Now.Date)
        Else
            req.TxnDate.SetValue(o.TimeOfOrder)
        End If
        If My.Settings.UseExportDateForShipping = True Then
            req.ShipDate.SetValue(System.DateTime.Now.Date)
        Else
            req.ShipDate.SetValue(o.TimeOfOrder.Date)
        End If

        req.Memo.SetValue("Auto created by " & Application.ProductName)
        If My.Settings.MarkOrdersToBePrinted = True Then
            If My.Settings.MarkPaidOrdersToBePrinted = True Then
                If o.PaymentStatus = BVC5WebServices.OrderPaymentStatus.Paid Then
                    req.IsToBePrinted.SetValue(True)
                Else
                    req.IsToBePrinted.SetValue(False)
                End If
            Else
                req.IsToBePrinted.SetValue(True)
            End If
        Else
            req.IsToBePrinted.SetValue(False)
        End If

        ' Mark Unpaid Orders
        'If o.PaymentStatus <> BVC5WebServices.OrderPaymentStatus.Paid Then
        '    req.IsPending.SetValue(My.Settings.MarkUnpaidOrdersAsPending)
        'Else
        '    req.IsPending.SetValue(False)
        '    If My.Settings.DepositSalesReceipts = True Then
        '        If My.Settings.SalesReceiptDepositAccount.Trim.Length > 0 Then
        '            If AccountExists(My.Settings.SalesReceiptDepositAccount, ses, ver).ListId <> "-1" Then
        '                req.DepositToAccountRef.FullName.SetValue(TrimToLength(My.Settings.SalesReceiptDepositAccount, 31))
        '            End If
        '        End If
        '    End If
        'End If


        Dim billA As New QBAddress(o.BillingAddress, CInt(ver.MajorVersion))
        req.BillAddress.Addr1.SetValue(billA.Addr1)
        req.BillAddress.Addr2.SetValue(billA.Addr2)
        req.BillAddress.City.SetValue(billA.City)
        req.BillAddress.State.SetValue(billA.State)
        req.BillAddress.PostalCode.SetValue(billA.PostalCode)
        req.BillAddress.Country.SetValue(billA.Country)
        'req.BillAddress.Addr3.SetValue(customer.BillingAddress.Addr3)
        'req.BillAddress.Addr4.SetValue(customer.BillingAddress.Addr4)
        'req.BillAddress.Addr5.SetValue(customer.BillingAddress.Addr5)

        Dim shipA As New QBAddress(o.ShippingAddress, CInt(ver.MajorVersion))
        req.ShipAddress.Addr1.SetValue(shipA.Addr1)
        req.ShipAddress.Addr2.SetValue(shipA.Addr2)
        req.ShipAddress.City.SetValue(shipA.City)
        req.ShipAddress.State.SetValue(shipA.State)
        req.ShipAddress.PostalCode.SetValue(shipA.PostalCode)
        req.ShipAddress.Country.SetValue(shipA.Country)
        'req.ShipAddress.Addr3.SetValue(shipA.Addr3)
        'req.ShipAddress.Addr4.SetValue(shipA.Addr4)
        'req.ShipAddress.Addr5.SetValue(shipA.Addr5)

        ' Payment Info
        'Dim PayInfo As New QBItemInfo
        'PayInfo = GetPaymentMethodInformationForOrder5(o, ses, ver, True)
        'If PayInfo.ListId <> "-1" Then
        '    req.PaymentMethodRef.ListID.SetValue(PayInfo.ListId)
        'End If

        ' Items
        If Not o.Items Is Nothing Then
            For i As Integer = 0 To o.Items.Length - 1
                With o.Items(i)

                    ' Get ListID for item
                    Dim itemInfo As New QBItemInfo
                    If My.Settings.ExportProductsAsSKU = True Then
                        itemInfo = Me.ProductExists(.ProductSku, ses, ver)
                    Else
                        itemInfo = Me.ProductExists(.ProductName, ses, ver)
                    End If

                    If itemInfo.ListId = "-1" Then
                        Dim newItemPrice As Decimal = Math.Round((.LineTotal / .Quantity), 2)
                        If My.Settings.ExportProductsAsSKU = True Then
                            itemInfo = ProductCreate(.ProductSku, .ProductName, newItemPrice, ses, ver)
                        Else
                            itemInfo = ProductCreate(.ProductName, .ProductSku & " " & .ProductName, newItemPrice, ses, ver)
                        End If
                    End If

                    ' Create the first line item for the invoice
                    Dim lineItem As QBFC7Lib.ISalesOrderLineAdd
                    lineItem = req.ORSalesOrderLineAddList.Append.SalesOrderLineAdd

                    lineItem.ItemRef.ListID.SetValue(itemInfo.ListId)

                    lineItem.Amount.SetValue(SafeDouble(.LineTotal))

                    Dim desc As String = ""
                    If My.Settings.ExportProductsAsSKU = True Then
                        desc = .ProductName
                        If Not My.Settings.ProductUseTitleOnly Then
                            If .ProductShortDescription.Trim.Length > 0 Then
                                desc += " - " & .ProductShortDescription
                            End If
                        End If
                    Else
                        desc = .ProductSku
                        If Not My.Settings.ProductUseTitleOnly Then
                            If .ProductShortDescription.Trim.Length > 0 Then
                                desc += " - " & .ProductShortDescription
                            End If
                        End If
                    End If

                    desc = desc.Replace("<br>", ", ")
                    desc = desc.Replace("<b>", "")
                    desc = desc.Replace("</b>", "")

                    If My.Settings.UseQuickBooksItemDescription = False Then
                        lineItem.Desc.SetValue(TrimToLength(desc, 4095))
                    End If

                    desc = Nothing

                    lineItem.Quantity.SetValue(CInt(.Quantity))

                    ' Tag items as taxable or not if applicable
                    'If ver.CountryCode <> "CA" Then
                    If Not My.Settings.DisableTaxes Then


                        If My.Settings.UseInlineTax = False Then
                            If .AssociatedProduct.TaxExempt = True Then
                                lineItem.SalesTaxCodeRef.FullName.SetValue("NON")
                            Else
                                lineItem.SalesTaxCodeRef.FullName.SetValue("TAX")
                            End If
                        Else
                            ' Using Inline Tax so Set everyting to "NON"
                            lineItem.SalesTaxCodeRef.FullName.SetValue("NON")
                        End If
                        'End If
                    End If

                End With
            Next
        End If

        ' Shipping
        Dim shipMethodID As String = "-1"
        shipMethodID = Me.ShipMethodExists(o.ShippingMethodDisplayName, ses, ver)
        If shipMethodID = "-1" Then
            shipMethodID = Me.ShipMethodCreate(o.ShippingMethodDisplayName, ses, ver)
        End If
        If shipMethodID <> "-1" Then
            req.ShipMethodRef.ListID.SetValue(shipMethodID)
        End If

        ' Create or get shipping charge item
        Dim ShipItemInfo As New QBItemInfo
        ShipItemInfo = Me.ShipItemExists(My.Settings.ShippingItemName, ses, ver)
        If ShipItemInfo.ListId = "-1" Then
            ShipItemInfo = Me.ShippingItemCreate(My.Settings.ShippingItemName, ses, ver)
        End If
        If (ShipItemInfo.ListId <> "-1") Then
            ' Version 3 adds explicit shipping line support
            'If CDbl(ver.MajorVersion) >= 4 Then
            '    req.ShippingLineAdd.Amount.SetValue(o.Packages(0).ShippingCost)
            '    req.ShippingLineAdd.AccountRef.ListID.SetValue(ShipItemInfo.ListId)
            'Else
            ' Add Shipping Charge Line Item
            ' Create the first line item for the invoice
            Dim shiplineItem As QBFC7Lib.ISalesOrderLineAdd
            shiplineItem = req.ORSalesOrderLineAddList.Append.SalesOrderLineAdd
            shiplineItem.ItemRef.ListID.SetValue(ShipItemInfo.ListId)
            shiplineItem.Amount.SetValue(SafeDouble(o.ShippingTotal))
            shiplineItem.Desc.SetValue(TrimToLength(o.ShippingMethodDisplayName, 4095))
            shiplineItem.Quantity.SetValue(1)
            'End If
        End If

        'Handling
        If o.HandlingTotal > 0 Then
            Dim handlingID As String = "-1"
            handlingID = Me.HandlingItemExists(My.Settings.HandlingItemName, ses, ver).ListId
            If handlingID = "-1" Then
                handlingID = Me.HandlingItemCreate(My.Settings.HandlingItemName, ses, ver).ListId
            End If
            If handlingID <> "-1" Then
                Dim hlineItem As QBFC7Lib.ISalesOrderLineAdd
                hlineItem = req.ORSalesOrderLineAddList.Append.SalesOrderLineAdd
                hlineItem.ItemRef.ListID.SetValue(handlingID)
                hlineItem.Amount.SetValue(SafeDouble(o.HandlingTotal))
                hlineItem.Desc.SetValue(TrimToLength("Handling Charges", 4095))
                hlineItem.Quantity.SetValue(1)
            End If
        End If


        'Special Instructions
        req.Memo.SetValue(TrimToLength(o.Instructions, 4095))
        'If o.Instructions.Trim.Length > 0 Then
        '    Dim memoID As String = "-1"
        '    memoID = Me.MemoItemExists(ses, ver).ListId
        '    If memoID = "-1" Then
        '        memoID = Me.MemoItemCreate(ses, ver).ListId
        '    End If
        '    If memoID <> "-1" Then
        '        Dim hlineItem As QBFC7Lib.ISalesReceiptLineAdd
        '        hlineItem = req.ORSalesReceiptLineAddList.Append.SalesReceiptLineAdd
        '        hlineItem.ItemRef.ListID.SetValue(memoID)
        '        hlineItem.Amount.SetValue(SafeDouble(0))
        '        hlineItem.Desc.SetValue(TrimToLength(o.Instructions, 4095))
        '        hlineItem.Quantity.SetValue(1)
        '    End If
        'End If

        'If ver.CountryCode = "CA" Then
        '    req.Tax1Total.SetValue(o.TaxTotal)
        'Else
        If Not My.Settings.DisableTaxes Then
            If o.TaxTotal > 0 Then
                If My.Settings.UseInlineTax = True Then

                    Dim taxID As String = "-1"
                    taxID = Me.TaxItemExists(My.Settings.InlineTaxItemName, ses, ver).ListId
                    If taxID = "-1" Then
                        taxID = Me.TaxItemCreate(My.Settings.InlineTaxItemName, ses, ver).ListId
                    End If
                    If taxID <> "-1" Then
                        Dim taxlineItem As QBFC7Lib.ISalesOrderLineAdd
                        taxlineItem = req.ORSalesOrderLineAddList.Append.SalesOrderLineAdd
                        taxlineItem.ItemRef.ListID.SetValue(taxID)
                        taxlineItem.Amount.SetValue(SafeDouble(o.TaxTotal))
                        taxlineItem.Desc.SetValue(TrimToLength("BV Calculated Taxes", 4095))
                        'taxlineItem.SalesTaxCodeRef.FullName.SetValue("NON")
                        'taxlineItem.Quantity.SetValue(1)
                    End If
                End If
            End If
            'End If
        End If

        'Total discount applied to order.
        Dim OrderDiscountAmount As Decimal = 0
        OrderDiscountAmount += o.OrderDiscounts
        OrderDiscountAmount += o.ShippingDiscounts
        If o.CustomProperties IsNot Nothing Then
            For q As Integer = 0 To o.CustomProperties.Length - 1
                If o.CustomProperties(q).DeveloperId = "bvsoftware" Then
                    If o.CustomProperties(q).Key = "postorderadjustment" Then
                        Dim postOrderAdjustment As Decimal = 0
                        Decimal.TryParse(o.CustomProperties(q).Value, postOrderAdjustment)
                        OrderDiscountAmount += postOrderAdjustment
                    End If
                End If
            Next
        End If

        If OrderDiscountAmount > 0 Then
            Dim orderDiscountTotalID As String = "-1"
            orderDiscountTotalID = Me.OrderDiscountItemExists(ses, ver).ListId
            If orderDiscountTotalID = "-1" Then
                orderDiscountTotalID = Me.OrderDiscountItemCreate(ses, ver).ListId
            End If
            If orderDiscountTotalID <> "-1" Then

                Dim lineItem As QBFC7Lib.ISalesOrderLineAdd
                lineItem = req.ORSalesOrderLineAddList.Append.SalesOrderLineAdd
                lineItem.ItemRef.ListID.SetValue(orderDiscountTotalID)
                lineItem.Amount.SetValue(-1 * SafeDouble(OrderDiscountAmount))
                lineItem.Quantity.SetValue(SafeDouble(1))
                lineItem.Desc.SetValue(TrimToLength("BV Commerce Discounts", 4095))
                'If ver.CountryCode <> "CA" Then
                If Not My.Settings.DisableTaxes Then
                    lineItem.SalesTaxCodeRef.FullName.SetValue("NON")
                End If
                'End If
            End If
        End If


        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            If responseList.StatusSeverity = "Error" Then
                Logging.WriteLine("ERROR: " & responseList.StatusMessage)
            End If

            If responseList.StatusCode <> 0 Then
                Trace.Write(o.OrderNumber & " " & responseList.StatusMessage)
                result = False
            Else
                Dim ret As QBFC7Lib.ISalesOrderRet = CType(responseList.Detail, QBFC7Lib.ISalesOrderRet)

                If Not ret Is Nothing Then
                    If ret.RefNumber.GetValue = req.RefNumber.GetValue Then
                        result = True
                    End If
                Else
                    If responseList.StatusCode <> 0 Then
                        If responseList.StatusSeverity = "Error" Then
                            'Me.errors += "(" & o.ID & " Error) " & responseList.StatusMessage
                            result = False
                        End If
                    End If
                End If
            End If
        End If

        Return result
    End Function

#End Region

#Region " Payments "


    Public Function GetPaymentMethodInformationForOrder5(ByRef o As BVC5WebServices.Order, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo, ByVal allowInfoOnly As Boolean) As QBItemInfo
        Dim result As New QBItemInfo
        result.ItemType = QBItemType.PaymentMethod
        result.ListId = "-1"

        If Not o.Payments Is Nothing Then


            Dim PaymentIdCreditCard As String = "4A807645-4B9D-43f1-BC07-9F233B4E713C"
            Dim PaymentIdTelephone As String = "9FD35C50-CDCB-42ac-9549-14119BECBD0C"
            Dim PaymentIdCheck As String = "494A61C8-D7E7-457f-B293-4838EF010C32"
            Dim PaymentIdCash As String = "7FCC4B3F-6E67-4f58-86B0-25BCCC035A0E"
            Dim PaymentIdPurchaseOrder As String = "26C948F3-22EF-4bcb-9AE9-DEB9839BF4A7"
            Dim PaymentIdCashOnDelivery As String = "EE171EFD-9E4A-4eda-AD70-4CB99F28E06C"
            Dim PaymentIdGiftCertificate As String = "91a205f1-8c1c-4267-bed0-c8e410e7e680"
            Dim PaymentIdPaypalExpress As String = "33eeba60-e5b7-4864-9b57-3f8d614f8301"

            Dim foundPayment As Boolean = False

            For i As Integer = 0 To o.Payments.Length - 1

                If o.Payments(i).AmountCharged > 0 Then

                    foundPayment = True

                    Select Case o.Payments(i).PaymentMethodId
                        Case PaymentIdCreditCard
                            Select Case o.Payments(i).CreditCardType
                                Case "A"
                                    result = Me.PaymentMethodExists(My.Settings.PaymentAmex, ses, ver)
                                    If result.ListId = "-1" Then
                                        result = Me.PaymentMethodCreate(My.Settings.PaymentAmex, ses, ver)
                                    End If
                                Case "C"
                                    result = Me.PaymentMethodExists(My.Settings.PaymentDiners, ses, ver)
                                    If result.ListId = "-1" Then
                                        result = Me.PaymentMethodCreate(My.Settings.PaymentDiners, ses, ver)
                                    End If
                                Case "D"
                                    result = Me.PaymentMethodExists(My.Settings.PaymentDiscover, ses, ver)
                                    If result.ListId = "-1" Then
                                        result = Me.PaymentMethodCreate(My.Settings.PaymentDiscover, ses, ver)
                                    End If
                                Case "J"
                                    result = Me.PaymentMethodExists(My.Settings.PaymentJCB, ses, ver)
                                    If result.ListId = "-1" Then
                                        result = Me.PaymentMethodCreate(My.Settings.PaymentJCB, ses, ver)
                                    End If
                                Case "M"
                                    result = Me.PaymentMethodExists(My.Settings.PaymentMasterCard, ses, ver)
                                    If result.ListId = "-1" Then
                                        result = Me.PaymentMethodCreate(My.Settings.PaymentMasterCard, ses, ver)
                                    End If
                                Case "V"
                                    result = Me.PaymentMethodExists(My.Settings.PaymentVisa, ses, ver)
                                    If result.ListId = "-1" Then
                                        result = Me.PaymentMethodCreate(My.Settings.PaymentVisa, ses, ver)
                                    End If
                            End Select
                        Case PaymentIdCheck
                            result = Me.PaymentMethodExists("Check", ses, ver)
                            If result.ListId = "-1" Then
                                result = Me.PaymentMethodCreate("Check", ses, ver)
                            End If
                        Case PaymentIdCash
                            result = Me.PaymentMethodExists("Cash", ses, ver)
                            If result.ListId = "-1" Then
                                result = Me.PaymentMethodCreate("Cash", ses, ver)
                            End If
                        Case PaymentIdPaypalExpress
                            result = Me.PaymentMethodExists(My.Settings.PaymentPayPal, ses, ver)
                            If result.ListId = "-1" Then
                                result = Me.PaymentMethodCreate(My.Settings.PaymentPayPal, ses, ver)
                            End If
                        Case Else
                            result = Me.PaymentMethodExists(My.Settings.PaymentOther, ses, ver)
                            If result.ListId = "-1" Then
                                result = Me.PaymentMethodCreate(My.Settings.PaymentOther, ses, ver)
                            End If
                    End Select
                End If

                If foundPayment = True Then
                    Exit For
                End If
            Next

        End If

        Return result
    End Function


    Public Function PaymentMethodExists(ByVal methodName As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBItemInfo
        Dim result As New QBItemInfo

        result.ListId = "-1"
        result.ItemType = QBItemType.PaymentMethod

        If Not methodName Is Nothing Then
            If methodName.Trim.Length > 0 Then

                Dim rs As QBFC7Lib.IMsgSetRequest
                rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
                rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

                Dim req As QBFC7Lib.IPaymentMethodQuery = rs.AppendPaymentMethodQueryRq

                req.ORPaymentMethodListQuery.FullNameList.Add(TrimToLength(methodName, 31))                

                Dim res As QBFC7Lib.IMsgSetResponse
                res = ses.DoRequests(rs)

                If (res.ResponseList.Count >= 1) Then
                    Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
                    Dim info As QBFC7Lib.IPaymentMethodRetList = CType(responseList.Detail, QBFC7Lib.IPaymentMethodRetList)

                    If Not info Is Nothing Then
                        If info.Count >= 1 Then
                            Dim theItem As QBFC7Lib.IPaymentMethodRet
                            theItem = info.GetAt(0)
                            If Not theItem Is Nothing Then
                                result.ListId = theItem.ListID.GetValue
                            End If
                        End If
                    End If
                End If

            End If
        End If


        Return result
    End Function

    Public Function PaymentMethodCreate(ByVal methodName As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBItemInfo
        Dim result As New QBItemInfo
        result.ListId = "-1"
        result.ItemType = QBItemType.PaymentMethod

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IPaymentMethodAdd = rs.AppendPaymentMethodAddRq

        req.Name.SetValue(TrimToLength(methodName, 31))

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.IPaymentMethodRet = CType(responseList.Detail, QBFC7Lib.IPaymentMethodRet)
            If Not info Is Nothing Then
                result.ListId = info.ListID.GetValue
            End If
        End If

        Return result
    End Function

#End Region

#Region " Products "

    Public Function ProductExists(ByVal productName As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBItemInfo
        Dim result As New QBItemInfo
        result.ListId = "-1"
        result.ItemType = QBItemType.Unknown


        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IItemQuery = rs.AppendItemQueryRq

        Dim fullname As String = GetFinalProductName(productName)

        If My.Settings.UseSubProductNameMatching = True Then
            ' Search By Partial Name to account for sub items
            req.ORListQuery.ListFilter.ORNameFilter.NameFilter.MatchCriterion.SetValue(QBFC7Lib.ENMatchCriterion.mcEndsWith)
            req.ORListQuery.ListFilter.ORNameFilter.NameFilter.Name.SetValue(fullname)
        Else
            ' Search By Full Name
            req.ORListQuery.FullNameList.Add(fullname)
        End If

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            If responseList.StatusSeverity = "Error" Then
                Logging.WriteLine("ERROR: " & responseList.StatusMessage)
            End If

            Dim info As QBFC7Lib.IORItemRetList = CType(responseList.Detail, QBFC7Lib.IORItemRetList)

            If Not info Is Nothing Then
                If info.Count >= 1 Then
                    Dim theItem As QBFC7Lib.IORItemRet
                    theItem = info.GetAt(0)
                    If Not theItem Is Nothing Then
                        Select Case theItem.ortype
                            Case QBFC7Lib.ENORItemRet.orirItemInventoryRet
                                result.ItemType = QBItemType.InventoryItem
                                result.ListId = theItem.ItemInventoryRet.ListID.GetValue
                            Case QBFC7Lib.ENORItemRet.orirItemNonInventoryRet
                                result.ItemType = QBItemType.NonInventoryItem
                                result.ListId = theItem.ItemNonInventoryRet.ListID.GetValue
                            Case QBFC7Lib.ENORItemRet.orirItemServiceRet
                                result.ItemType = QBItemType.Service
                                result.ListId = theItem.ItemServiceRet.ListID.GetValue
                            Case QBFC7Lib.ENORItemRet.orirItemGroupRet
                                result.ItemType = QBItemType.ItemGroup
                                result.ListId = theItem.ItemGroupRet.ListID.GetValue
                            Case QBFC7Lib.ENORItemRet.orirItemOtherChargeRet
                                result.ItemType = QBItemType.OtherCharge
                                result.ListId = theItem.ItemOtherChargeRet.ListID.GetValue
                            Case Else
                                result.ItemType = QBItemType.Unknown
                        End Select
                    End If

                End If
            End If
        End If

        Return result
    End Function

    Public Function ProductCreate(ByVal productName As String, ByVal description As String, ByVal price As Double, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBItemInfo
        If My.Settings.CreateInventoryItems Then
            Return ProductCreateInventory(productName, description, price, ses, ver)
        Else
            Return ProductCreateNonInventory(productName, description, price, ses, ver)
        End If
    End Function

    Public Function ProductCreateNonInventory(ByVal productName As String, ByVal description As String, ByVal price As Double, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBItemInfo
        Dim result As New QBItemInfo
        result.ListId = "-1"
        result.ItemType = QBItemType.Unknown

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IItemNonInventoryAdd = rs.AppendItemNonInventoryAddRq

        Dim fullname As String

        If My.Settings.UseProductPrefix = True Then
            fullname = My.Settings.ProductPrefix & " " & productName
        Else
            fullname = productName
        End If
        req.Name.SetValue(TrimToLength(fullname, 31))

        req.IsActive.SetValue(True)
        'If Not My.Settings.ProductUseTitleOnly Then
        req.ORSalesPurchase.SalesOrPurchase.Desc.SetValue(description)
        'End If
        req.ORSalesPurchase.SalesOrPurchase.ORPrice.Price.SetValue(price)
        req.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName.SetValue(My.Settings.NewProductIncomeAccount)

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)


        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            If responseList.StatusSeverity = "Error" Then
                Logging.WriteLine("ERROR: " & responseList.StatusMessage)
            End If

            Dim info As QBFC7Lib.IItemNonInventoryRet = CType(responseList.Detail, QBFC7Lib.IItemNonInventoryRet)

            If Not info Is Nothing Then
                result.ListId = info.ListID.GetValue
                result.ItemType = QBItemType.NonInventoryItem
            End If
        End If

        Return result
    End Function

    Public Function ProductCreateInventory(ByVal productName As String, ByVal description As String, ByVal price As Double, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBItemInfo
        Dim result As New QBItemInfo
        result.ListId = "-1"
        result.ItemType = QBItemType.Unknown

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IItemInventoryAdd = rs.AppendItemInventoryAddRq

        Dim fullname As String

        If My.Settings.UseProductPrefix = True Then
            fullname = My.Settings.ProductPrefix & " " & productName
        Else
            fullname = productName
        End If
        req.Name.SetValue(TrimToLength(fullname, 31))

        req.IsActive.SetValue(True)
        'If Not My.Settings.ProductUseTitleOnly Then
        req.SalesDesc.SetValue(description)
        '        End If
        req.SalesPrice.SetValue(price)
        req.IncomeAccountRef.FullName.SetValue(My.Settings.NewProductIncomeAccount)
        req.COGSAccountRef.FullName.SetValue(My.Settings.COGAccount)
        req.AssetAccountRef.FullName.SetValue(My.Settings.InventoryAssetAccount)
        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)


        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            If responseList.StatusSeverity = "Error" Then
                Logging.WriteLine("ERROR: " & responseList.StatusMessage)
            End If

            Dim info As QBFC7Lib.IItemInventoryRet = CType(responseList.Detail, QBFC7Lib.IItemInventoryRet)

            If Not info Is Nothing Then
                result.ListId = info.ListID.GetValue
                result.ItemType = QBItemType.NonInventoryItem
            End If
        End If

        Return result
    End Function

    Public Shared Function ListInventoryItems(ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As Collection(Of QBInventoryItem)
        Dim result As New Collection(Of QBInventoryItem)

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IItemInventoryQuery = rs.AppendItemInventoryQueryRq
        req.metaData.SetValue(QBFC7Lib.ENmetaData.mdNoMetaData)
        req.ORListQuery.ListFilter.ActiveStatus.SetValue(QBFC7Lib.ENActiveStatus.asActiveOnly)


        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.IItemInventoryRetList = CType(responseList.Detail, QBFC7Lib.IItemInventoryRetList)

            If Not info Is Nothing Then
                For i As Integer = 0 To info.Count - 1
                    Dim retItem As QBFC7Lib.IItemInventoryRet = info.GetAt(i)
                    If retItem IsNot Nothing Then
                        Dim item As New QBInventoryItem(retItem)
                        result.Add(item)
                    End If
                Next
            End If
        End If

        Return result
    End Function

#End Region

    Public Function AccountList(ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBAccountInfo()

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IAccountQuery = rs.AppendAccountQueryRq

        req.ORAccountListQuery.AccountListFilter.ActiveStatus.SetValue(QBFC7Lib.ENActiveStatus.asActiveOnly)

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.IAccountRetList = CType(responseList.Detail, QBFC7Lib.IAccountRetList)

            If Not info Is Nothing Then
                Dim result(info.Count) As QBAccountInfo
                For i As Integer = 0 To info.Count - 1
                    Dim a As QBFC7Lib.IAccountRet = info.GetAt(i)
                    If Not a Is Nothing Then

                        Dim ai As New QBAccountInfo
                        ai.ListId = a.ListID.GetValue
                        ai.Name = a.Name.GetValue
                        Select Case a.AccountType.GetValue
                            Case QBFC7Lib.ENAccountType.atEquity
                                ai.AccountType = QBAccountType.Equity
                            Case QBFC7Lib.ENAccountType.atExpense
                                ai.AccountType = QBAccountType.Equity
                            Case QBFC7Lib.ENAccountType.atIncome
                                ai.AccountType = QBAccountType.Income
                            Case QBFC7Lib.ENAccountType.atOtherExpense
                                ai.AccountType = QBAccountType.OtherExpense
                            Case QBFC7Lib.ENAccountType.atOtherIncome
                                ai.AccountType = QBAccountType.OtherIncome
                            Case QBFC7Lib.ENAccountType.atBank
                                ai.AccountType = QBAccountType.Bank
                            Case QBFC7Lib.ENAccountType.atCostOfGoodsSold
                                ai.AccountType = QBAccountType.CostOfGoodSold
                            Case QBFC7Lib.ENAccountType.atCreditCard
                                ai.AccountType = QBAccountType.CreditCard
                            Case QBFC7Lib.ENAccountType.atAccountsPayable
                                ai.AccountType = QBAccountType.AccountsPayable
                            Case QBFC7Lib.ENAccountType.atAccountsReceivable
                                ai.AccountType = QBAccountType.AccountsReceivable
                            Case QBFC7Lib.ENAccountType.atLongTermLiability
                                ai.AccountType = QBAccountType.LongTermLiability
                            Case Else
                                ai.AccountType = QBAccountType.Unknown
                        End Select
                        result(i) = ai
                        ai = Nothing
                    End If
                    a = Nothing
                Next
                Return result
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If

    End Function

    Public Function AccountCreate(ByVal info As QBAccountInfo, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBAccountInfo
        Dim result As New QBAccountInfo
        result = info
        result.ListId = "-1"

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IAccountAdd = rs.AppendAccountAddRq

        req.Name.SetValue(TrimToLength(info.Name, 31))

        Select Case info.AccountType
            Case QBAccountType.Bank
                req.AccountType.SetValue(QBFC7Lib.ENAccountType.atBank)
            Case QBAccountType.CostOfGoodSold
                req.AccountType.SetValue(QBFC7Lib.ENAccountType.atCostOfGoodsSold)
            Case QBAccountType.CreditCard
                req.AccountType.SetValue(QBFC7Lib.ENAccountType.atCreditCard)
            Case QBAccountType.Equity
                req.AccountType.SetValue(QBFC7Lib.ENAccountType.atEquity)
            Case QBAccountType.Expense
                req.AccountType.SetValue(QBFC7Lib.ENAccountType.atExpense)
            Case QBAccountType.Income
                req.AccountType.SetValue(QBFC7Lib.ENAccountType.atIncome)
            Case QBAccountType.OtherExpense
                req.AccountType.SetValue(QBFC7Lib.ENAccountType.atOtherExpense)
            Case QBAccountType.OtherIncome
                req.AccountType.SetValue(QBFC7Lib.ENAccountType.atOtherIncome)
            Case QBAccountType.Unknown
                Return result
        End Select

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim newinfo As QBFC7Lib.IAccountRet = CType(responseList.Detail, QBFC7Lib.IAccountRet)

            If Not newinfo Is Nothing Then
                result.ListId = newinfo.ListID.GetValue
            End If

        End If

        Return result

    End Function

    Public Function AccountExists(ByVal accountName As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBAccountInfo
        Dim result As New QBAccountInfo

        result.ListId = "-1"
        result.Name = accountName
        result.AccountType = QBAccountType.Unknown

        If Not accountName Is Nothing Then
            If accountName.Trim.Length > 0 Then

                Dim rs As QBFC7Lib.IMsgSetRequest
                rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
                rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

                Dim req As QBFC7Lib.IAccountQuery = rs.AppendAccountQueryRq

                req.ORAccountListQuery.FullNameList.Add(TrimToLength(accountName, 31))



                Dim res As QBFC7Lib.IMsgSetResponse
                res = ses.DoRequests(rs)

                If (res.ResponseList.Count >= 1) Then
                    Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
                    Dim info As QBFC7Lib.IAccountRetList = CType(responseList.Detail, QBFC7Lib.IAccountRetList)

                    If Not info Is Nothing Then
                        If info.Count >= 1 Then
                            Dim theItem As QBFC7Lib.IAccountRet
                            theItem = info.GetAt(0)
                            If Not theItem Is Nothing Then
                                result.ListId = theItem.ListID.GetValue
                            End If
                        End If
                    End If
                End If

            End If
        End If


        Return result
    End Function

    Public Function ShipMethodExists(ByVal shipMethodName As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As String
        Dim result As String = "-1"

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IShipMethodQuery = rs.AppendShipMethodQueryRq


        Dim fullname As String
        If My.Settings.UseShipMethodPrefix = True Then
            fullname = My.Settings.ShipMethodPrefix & " " & shipMethodName
        Else
            fullname = shipMethodName
        End If

        req.ORListQuery.FullNameList.Add(TrimToLength(fullname, 15))

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.IShipMethodRetList = CType(responseList.Detail, QBFC7Lib.IShipMethodRetList)

            If Not info Is Nothing Then
                If info.Count >= 1 Then
                    result = info.GetAt(0).ListID.GetValue()
                End If
            End If
        End If

        Return result
    End Function

    Public Function ShipMethodCreate(ByVal shipMethodName As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As String
        Dim result As String = "-1"

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IShipMethodAdd = rs.AppendShipMethodAddRq

        If My.Settings.UseShipMethodPrefix = True Then
            req.Name.SetValue(TrimToLength(My.Settings.ShipMethodPrefix & " " & shipMethodName, 15))
        Else
            req.Name.SetValue(TrimToLength(shipMethodName, 15))
        End If
        req.IsActive.SetValue(True)

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.IShipMethodRet = CType(responseList.Detail, QBFC7Lib.IShipMethodRet)

            If Not info Is Nothing Then
                result = info.ListID.GetValue
            End If
        End If

        Return result
    End Function

    Public Function ShipItemExists(ByVal shipItemName As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBItemInfo
        Dim result As New QBItemInfo
        result.ListId = "-1"
        result.ItemType = QBItemType.OtherCharge


        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IItemOtherChargeQuery = rs.AppendItemOtherChargeQueryRq

        req.ORListQuery.FullNameList.Add(TrimToLength(shipItemName, 31))


        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.IItemOtherChargeRetList = CType(responseList.Detail, QBFC7Lib.IItemOtherChargeRetList)

            If Not info Is Nothing Then
                If info.Count >= 1 Then
                    result.ListId = info.GetAt(0).ListID.GetValue
                End If
            End If
        End If

        Return result
    End Function

    Public Function ShippingItemCreate(ByVal shippingItemName As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBItemInfo
        Dim result As New QBItemInfo

        result.ListId = "-1"
        result.ItemType = QBItemType.OtherCharge

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IItemOtherChargeAdd = rs.AppendItemOtherChargeAddRq

        If My.Settings.UseShipMethodPrefix = True Then
            req.Name.SetValue(TrimToLength(My.Settings.ShipMethodPrefix & " " & My.Settings.ShippingItemName, 31))
        Else
            req.Name.SetValue(TrimToLength(My.Settings.ShippingItemName, 31))
        End If

        'If ver.CountryCode = "CA" Then
        '    ' Skip
        '    'req.TaxCodeForPurchaseRef.FullName.SetValue("E")
        '    'req.TaxCodeForSaleRef.FullName.SetValue("E")
        'Else
        req.SalesTaxCodeRef.FullName.SetValue("NON")
        'End If

        req.IsActive.SetValue(True)

        Dim ai As New QBAccountInfo
        ai = Me.AccountExists(My.Settings.ShippingItemIncomeAccount, ses, ver)
        If ai.ListId = "-1" Then
            ai.Name = My.Settings.ShippingItemIncomeAccount
            ai.AccountType = QBAccountType.Income
            ai = Me.AccountCreate(ai, ses, ver)
        End If
        If ai.ListId <> "-1" Then
            req.ORSalesPurchase.SalesOrPurchase.AccountRef.ListID.SetValue(ai.ListId)
        Else
            req.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName.SetValue(My.Settings.ShippingItemIncomeAccount)
        End If

        req.ORSalesPurchase.SalesOrPurchase.ORPrice.Price.SetValue(0.0)

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)


        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.IItemOtherChargeRet = CType(responseList.Detail, QBFC7Lib.IItemOtherChargeRet)

            If Not info Is Nothing Then
                result.ListId = info.ListID.GetValue
                result.ItemType = QBItemType.OtherCharge
            End If
        End If

        Return result
    End Function

    Public Function TaxItemExists(ByVal itemName As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBItemInfo


        Dim result As New QBItemInfo
        result.ListId = "-1"
        result.ItemType = QBItemType.Unknown

        If My.Settings.DisableTaxes Then
            Return result
        End If

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IItemSalesTaxQuery = rs.AppendItemSalesTaxQueryRq

        req.ORListQuery.FullNameList.Add(TrimToLength(itemName, 31))

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.IItemSalesTaxRetList = CType(responseList.Detail, QBFC7Lib.IItemSalesTaxRetList)

            If Not info Is Nothing Then
                If info.Count >= 1 Then
                    result.ListId = info.GetAt(0).ListID.GetValue
                End If
            End If
        End If

        Return result
    End Function

    Public Function TaxItemCreate(ByVal itemName As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBItemInfo
        Dim result As New QBItemInfo

        result.ListId = "-1"
        result.ItemType = QBItemType.Unknown

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IItemSalesTaxAdd = rs.AppendItemSalesTaxAddRq

        req.Name.SetValue(TrimToLength(itemName, 31))

        req.IsActive.SetValue(True)

        ' Set Vendor
        Dim vendorID As String = Me.VendorExists(My.Settings.InlineTaxVendorName, ses, ver)
        If vendorID = "-1" Then
            vendorID = Me.VendorCreate(My.Settings.InlineTaxVendorName, ses, ver)
        End If
        If vendorID <> "-1" Then
            req.TaxVendorRef.ListID.SetValue(vendorID)
        End If

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.IItemSalesTaxRet = CType(responseList.Detail, QBFC7Lib.IItemSalesTaxRet)

            If Not info Is Nothing Then
                result.ListId = info.ListID.GetValue
                result.ItemType = QBItemType.Unknown
            End If
        End If

        Return result
    End Function

    Public Function VendorExists(ByVal vendorName As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As String
        Dim result As String = "-1"

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IVendorQuery = rs.AppendVendorQueryRq

        req.ORVendorListQuery.FullNameList.Add(TrimToLength(vendorName, 41))
        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.IVendorRetList = CType(responseList.Detail, QBFC7Lib.IVendorRetList)

            If Not info Is Nothing Then
                If info.Count >= 1 Then
                    result = info.GetAt(0).ListID.GetValue
                End If
            End If
        End If

        Return result
    End Function

    Public Function VendorCreate(ByVal vendorName As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As String
        Dim result As String = "-1"

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IVendorAdd = rs.AppendVendorAddRq

        req.Name.SetValue(TrimToLength(vendorName, 41))

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.IVendorRet = CType(responseList.Detail, QBFC7Lib.IVendorRet)

            If Not info Is Nothing Then
                result = info.ListID.GetValue
            End If
        End If

        Return result
    End Function

    Public Function HandlingItemExists(ByVal handlingName As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBItemInfo
        Dim result As New QBItemInfo
        result.ListId = "-1"
        result.ItemType = QBItemType.OtherCharge


        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IItemOtherChargeQuery = rs.AppendItemOtherChargeQueryRq

        req.ORListQuery.FullNameList.Add(TrimToLength(handlingName, 31))


        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.IItemOtherChargeRetList = CType(responseList.Detail, QBFC7Lib.IItemOtherChargeRetList)

            If Not info Is Nothing Then
                If info.Count >= 1 Then
                    result.ListId = info.GetAt(0).ListID.GetValue
                End If
            End If
        End If

        Return result
    End Function

    Public Function HandlingItemCreate(ByVal handlingName As String, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBItemInfo
        Dim result As New QBItemInfo

        result.ListId = "-1"
        result.ItemType = QBItemType.OtherCharge

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IItemOtherChargeAdd = rs.AppendItemOtherChargeAddRq

        req.Name.SetValue(TrimToLength(My.Settings.HandlingItemName, 31))

        'If ver.CountryCode = "CA" Then
        '    ' Skip Tax
        'Else
        req.SalesTaxCodeRef.FullName.SetValue("NON")
        'End If

        req.IsActive.SetValue(True)

        ' Create New Account if needed
        Dim ai As New QBAccountInfo
        ai = Me.AccountExists(My.Settings.HandlineIncomeAccount, ses, ver)
        If ai.ListId = "-1" Then
            ai.Name = My.Settings.HandlineIncomeAccount
            ai.AccountType = QBAccountType.Income
            ai = Me.AccountCreate(ai, ses, ver)
        End If
        If ai.ListId <> "-1" Then
            req.ORSalesPurchase.SalesOrPurchase.AccountRef.ListID.SetValue(ai.ListId)
        Else
            req.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName.SetValue(My.Settings.HandlineIncomeAccount)
        End If

        req.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName.SetValue(My.Settings.HandlineIncomeAccount)
        req.ORSalesPurchase.SalesOrPurchase.ORPrice.Price.SetValue(0.0)

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.IItemOtherChargeRet = CType(responseList.Detail, QBFC7Lib.IItemOtherChargeRet)

            If Not info Is Nothing Then
                result.ListId = info.ListID.GetValue
                result.ItemType = QBItemType.OtherCharge
            End If
        End If

        Return result
    End Function

    Public Function MemoItemExists(ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBItemInfo
        Dim result As New QBItemInfo
        result.ListId = "-1"
        result.ItemType = QBItemType.OtherCharge

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IItemOtherChargeQuery = rs.AppendItemOtherChargeQueryRq

        req.ORListQuery.FullNameList.Add(TrimToLength("MEMO", 31))

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.IItemOtherChargeRetList = CType(responseList.Detail, QBFC7Lib.IItemOtherChargeRetList)

            If Not info Is Nothing Then
                If info.Count >= 1 Then
                    result.ListId = info.GetAt(0).ListID.GetValue
                End If
            End If
        End If

        Return result
    End Function

    Public Function MemoItemCreate(ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBItemInfo
        Dim result As New QBItemInfo

        result.ListId = "-1"
        result.ItemType = QBItemType.OtherCharge

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IItemOtherChargeAdd = rs.AppendItemOtherChargeAddRq

        req.Name.SetValue(TrimToLength("MEMO", 31))

        If ver.CountryCode = "CA" Then
            ' skip
        Else
            req.SalesTaxCodeRef.FullName.SetValue("NON")
        End If
        req.IsActive.SetValue(True)

        ' Create New Account if needed
        Dim ai As QBAccountInfo = Me.AccountExists(My.Settings.HandlineIncomeAccount, ses, ver)
        If ai.ListId = "-1" Then
            ai.Name = My.Settings.HandlineIncomeAccount
            ai.AccountType = QBAccountType.Income
            ai = Me.AccountCreate(ai, ses, ver)
        End If
        If ai.ListId <> "-1" Then
            req.ORSalesPurchase.SalesOrPurchase.AccountRef.ListID.SetValue(ai.ListId)
        Else
            req.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName.SetValue(My.Settings.HandlineIncomeAccount)
        End If

        req.ORSalesPurchase.SalesOrPurchase.AccountRef.FullName.SetValue(My.Settings.HandlineIncomeAccount)
        req.ORSalesPurchase.SalesOrPurchase.ORPrice.Price.SetValue(0.0)

        Dim res As QBFC7Lib.IMsgSetResponse
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
            Dim info As QBFC7Lib.IItemOtherChargeRet = CType(responseList.Detail, QBFC7Lib.IItemOtherChargeRet)

            If Not info Is Nothing Then
                result.ListId = info.ListID.GetValue
                result.ItemType = QBItemType.OtherCharge
            End If
        End If

        Return result
    End Function

    Public Function OrderDiscountItemExists(ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBItemInfo
        Return ProductExists(My.Settings.OrderDiscount, ses, ver)
        'Dim result As New QBItemInfo
        'result.ListId = "-1"
        'result.ItemType = QBItemType.Discount
        'Dim rs As QBFC7Lib.IMsgSetRequest
        'rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        'rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        'Dim req As QBFC7Lib.IItemDiscountQuery = rs.AppendItemDiscountQueryRq

        'req.ORListQuery.FullNameList.Add(TrimToLength(orderDiscountName, 31))

        'Dim res As QBFC7Lib.IMsgSetResponse
        'res = ses.DoRequests(rs)

        'If (res.ResponseList.Count >= 1) Then
        '    Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
        '    Dim info As QBFC7Lib.IItemDiscountRetList = CType(responseList.Detail, IItemDiscountRetList)

        '    If Not info Is Nothing Then
        '        If info.Count >= 1 Then
        '            result.ListId = info.GetAt(0).ListID.GetValue
        '        End If
        '    End If
        'End If

        'Return result
    End Function

    Public Function OrderDiscountItemCreate(ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBItemInfo
        Return ProductCreate(My.Settings.OrderDiscount, "BV Commerce Discounts", SafeDouble(0), ses, ver)
        'Dim result As New QBItemInfo

        'result.ListId = "-1"
        'result.ItemType = QBItemType.Discount

        'Dim rs As QBFC7Lib.IMsgSetRequest
        'rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        'rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        'Dim req As QBFC7Lib.IItemDiscountAdd = rs.AppendItemDiscountAddRq

        'req.Name.SetValue(TrimToLength(My.Settings.OrderDiscount, 31))
        'req.ItemDesc.SetValue("BV Commerce Discounts")

        'If ver.CountryCode = "CA" Then
        '    ' Skip Tax
        'Else
        '    req.SalesTaxCodeRef.FullName.SetValue("NON")
        'End If

        'req.IsActive.SetValue(True)

        '' Create New Account if needed
        'Dim ai As New QBAccountInfo
        'ai = Me.AccountExists(My.Settings.NewProductIncomeAccount, ses, ver)
        'If ai.ListId = "-1" Then
        '    ai.Name = My.Settings.HandlineIncomeAccount
        '    ai.AccountType = QBAccountType.Income
        '    ai = Me.AccountCreate(ai, ses, ver)
        'End If
        'If ai.ListId <> "-1" Then
        '    req.AccountRef.ListID.SetValue(ai.ListId)
        'Else
        '    req.AccountRef.FullName.SetValue(My.Settings.NewProductIncomeAccount)
        'End If

        'Dim res As QBFC7Lib.IMsgSetResponse
        'res = ses.DoRequests(rs)

        'If (res.ResponseList.Count >= 1) Then
        '    Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)
        '    Dim info As QBFC7Lib.IItemDiscountRet = CType(responseList.Detail, IItemDiscountRet)

        '    If Not info Is Nothing Then
        '        result.ListId = info.ListID.GetValue
        '        result.ItemType = QBItemType.OtherCharge
        '    End If
        'End If

        'Return result
    End Function

    Public Function RecordPayment(ByVal customerID As String, ByVal amount As Double, ByVal paymentMethodID As String, ByVal memo As String, ByVal depositAccountName As String, ByVal txnDate As Date, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As Boolean
        Dim result As Boolean = False

        Dim rs As QBFC7Lib.IMsgSetRequest
        rs = ses.CreateMsgSetRequest(ver.CountryCode, CShort(ver.MajorVersion), CShort(ver.MinorVersion))
        rs.Attributes.OnError = QBFC7Lib.ENRqOnError.roeContinue

        Dim req As QBFC7Lib.IReceivePaymentAdd = rs.AppendReceivePaymentAddRq

        req.CustomerRef.ListID.SetValue(customerID)

        If depositAccountName <> "" Then
            req.DepositToAccountRef.FullName.SetValue(depositAccountName)
        End If

        If memo.Trim.Length > 0 Then
            req.Memo.SetValue(memo)
        End If

        req.ORApplyPayment.IsAutoApply.SetValue(True)

        If paymentMethodID <> "-1" Then
            req.PaymentMethodRef.ListID.SetValue(paymentMethodID)
        End If

        req.TotalAmount.SetValue(SafeDouble(amount))
        req.TxnDate.SetValue(txnDate)

        Dim res As QBFC7Lib.IMsgSetResponse
        log("Starting Payment Record Transaction")
        res = ses.DoRequests(rs)

        If (res.ResponseList.Count >= 1) Then
            Dim responseList As QBFC7Lib.IResponse = res.ResponseList.GetAt(0)

            If responseList.StatusCode <> 0 Then
                log("Error: " & responseList.StatusMessage)
                result = False
            Else
                Dim info As QBFC7Lib.IReceivePaymentRet = CType(responseList.Detail, QBFC7Lib.IReceivePaymentRet)

                If Not info Is Nothing Then
                    log("Recording True for Payment Record")
                    result = True
                    'result = info.ListID.GetValue
                End If
            End If

        Else
            log("Transaction Response List Count was < 1")
        End If

        Return result
    End Function

    Private Function MinutesSince(ByVal startDate As DateTime) As Integer
        Dim result As Integer = 0
        Dim current As DateTime = DateTime.Now
        Dim ts As TimeSpan = current.Subtract(startDate)
        result = (ts.Days * 1440) + ts.Minutes
        Return result
    End Function

End Class

