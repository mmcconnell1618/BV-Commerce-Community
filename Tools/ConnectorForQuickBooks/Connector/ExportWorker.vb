' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt

Imports System.Text

Public Class ExportWorker

    Private _OrderList As New Collection(Of String)
    Private _SaveLastOrder As Boolean = True
    Private _ProblemOrders As New Collection(Of String)
    Private _SkippedOrders As New Collection(Of String)
    Private _Errors As New System.Collections.Specialized.StringCollection
    Private _ExportStartTime As DateTime
    Private _AverageExportTime As Long
    Private _TimeRemaining As Long
    Private _OrdersRemaining As Integer
    Private _SkipAllOrderDuplicateChecks As Boolean = False
    Private _Utils As New QuickBooksUtils

    Public Property OrderList() As Collection(Of String)
        Get
            Return _OrderList
        End Get
        Set(ByVal value As Collection(Of String))
            _OrderList = value
        End Set
    End Property
    Public Property SaveLastOrder() As Boolean
        Get
            Return _SaveLastOrder
        End Get
        Set(ByVal value As Boolean)
            _SaveLastOrder = value
        End Set
    End Property
    Public Property ProblemOrders() As Collection(Of String)
        Get
            Return _ProblemOrders
        End Get
        Set(ByVal value As Collection(Of String))
            _ProblemOrders = value
        End Set
    End Property
    Public Property SkippedOrders() As Collection(Of String)
        Get
            Return _SkippedOrders
        End Get
        Set(ByVal value As Collection(Of String))
            _SkippedOrders = value
        End Set
    End Property

    Private Sub w(ByVal message As String)
        Logging.Write(message)
    End Sub

    Private Sub wln(ByVal message As String)
        Logging.WriteLine(message)
    End Sub

    Public Sub StartExporting()
        Me.ProblemOrders.Clear()
        Me.SkippedOrders.Clear()

        _Errors.Clear()
        _ExportStartTime = Now()
        _AverageExportTime = 0

        ProgressBar1.Minimum = 0
        ProgressBar1.Maximum = 100

        Dim q As New QBFC7Lib.QBSessionManager

        Dim i As Integer = 0

        Try
            wln("Attempting to Open QBFC5 Connection")
            q.OpenConnection2("", My.Settings.QBApplicationName, QBFC7Lib.ENConnectionType.ctLocalQBD)
            wln("QBFC Connection Opened")

            If My.Settings.UseCompanyFile = True Then
                wln("Using Company File Name: " & My.Settings.CompanyFileName)
                q.BeginSession(My.Settings.CompanyFileName, QBFC7Lib.ENOpenMode.omDontCare)
            Else
                wln("Using currently open QB File")
                q.BeginSession("", QBFC7Lib.ENOpenMode.omDontCare)
            End If

            Dim ver As QBVersionInfo = _Utils.GetQuickBooksVersion(q)
            wln("QuickBooks Version = " & ver.MajorVersion & "." & ver.MinorVersion & " - " & ver.CountryCode)

            wln("_orders Count = " & _OrderList.Count)
            wln("Service Path = " & My.Settings.WebStoreUrl)


            Dim RecordStartTime As DateTime = Now()

            For i = 0 To _OrderList.Count - 1
                RecordStartTime = Now()

                Dim o5 As BVC5WebServices.Order
                o5 = Me.GetBVC5Order(_OrderList(i))
                If Not o5 Is Nothing Then
                    Me.lblCurrentOrder.Text = "Exporting BVC5 Order: " & o5.OrderNumber
                    wln("------------------------------------------------------------")
                    wln(lblCurrentOrder.Text)
                    Me.lblExportCount.Text = "Exporting order " & i + 1 & " of " & _OrderList.Count
                    Me.Focus()
                    Application.DoEvents()
                    ExportSingleOrderBVC5(o5, q, ver)
                    wln("Finished Export of BVC5 Order " & o5.OrderNumber)
                Else
                    wln("Order was nothing, finished processing")
                End If
                o5 = Nothing



                Dim ts As TimeSpan
                ts = Now.Subtract(_ExportStartTime)

                Dim recordTimeSpan As TimeSpan
                recordTimeSpan = Now.Subtract(RecordStartTime)
                wln("Order Export Time: " & recordTimeSpan.ToString)

                _AverageExportTime = CLng(ts.Ticks / (i + 1))
                _OrdersRemaining = _OrderList.Count - i - 1
                _TimeRemaining = CLng(_OrdersRemaining) * _AverageExportTime
                Dim eta As String = String.Format("{0:dd:HH:mm}", New TimeSpan(_TimeRemaining))
                If eta.IndexOf(".") > 0 Then
                    eta = eta.Substring(0, eta.IndexOf("."))
                End If
                Me.lblETA.Text = eta

            Next

            Dim totalTimeSpan As New TimeSpan
            totalTimeSpan = Now.Subtract(_ExportStartTime)
            wln("Total Export Time: " & totalTimeSpan.ToString)

        Catch ex As Exception
            Logging.LogException(ex)
            _Errors.Add(ex.Message)
            Me.ProblemOrders.Add(i.ToString)
        Finally
            q.EndSession()
            q.CloseConnection()
            q = Nothing
        End Try



        If Me.ProblemOrders.Count > 0 Then
            wln("Problem Orders:")
            wln("---------------------------")
            For j As Integer = 0 To Me.ProblemOrders.Count - 1
                wln(_ProblemOrders(j).ToString)
            Next
            wln("---------------------------")
        End If

        If Me.SkippedOrders.Count > 0 Then
            Dim skippedMessage As New StringBuilder
            wln("Skipped Orders:")
            wln("---------------------------")
            For k As Integer = 0 To Me.SkippedOrders.Count - 1
                wln(Me.SkippedOrders(k).ToString)
                skippedMessage.Append(Me.SkippedOrders(k).ToString)
                skippedMessage.Append(", ")
            Next
            wln("---------------------------")
            MsgBox("These orders were skipped: " & skippedMessage.ToString)
        End If

        If _Errors.Count > 0 Then
            wln(Now() & " - Errors in Last Export")
            For Each e As String In _Errors
                wln("Error: " & e)
            Next
            MsgBox("Errors: " & _Errors.ToString)
        End If

        Logging.CloseLogFile()
    End Sub


    Private Function GetBVC5Order(ByVal orderNumber As String) As BVC5WebServices.Order
        Dim result As New BVC5WebServices.Order

        Dim criteria As New BVC5WebServices.OrderSearchCriteria
        criteria.IsPlaced = True
        criteria.OrderNumber = orderNumber
        wln("Starting BVC 5 Order Download at: " & Now.ToString)
        Dim results() As BVC5WebServices.Order = MainForm.WebStore5.Orders_Order_FindByCriteria(MainForm.WebToken5, criteria)
        If results IsNot Nothing Then
            If results.Length > 0 Then
                result = MainForm.WebStore5.Orders_Order_FindByBvin(MainForm.WebToken5, results(0).Bvin)
            End If
        End If
        wln("Finished BVC 5 Order Download at: " & Now.ToString)

        If result Is Nothing Then
            wln("ERROR: Order from BVC5 was nothing")
            result = New BVC5WebServices.Order
        End If

        If result.Bvin = String.Empty Then
            wln("ERROR: Order Object was not Created!")
            Throw New Exception("Failed to load Order Ref# " & orderNumber & " from store.")
        End If

        Return result
    End Function

    Private Sub ExportSingleOrderBVC5(ByRef o As BVC5WebServices.Order, ByRef ses As QBFC7Lib.QBSessionManager, ByVal versionInfo As QBVersionInfo)
        Dim result As Boolean = False

        Try
            If Not o Is Nothing Then

                AbbreviateAddresses(o)

                Dim createOrder As Boolean = True

                wln("Checking for Duplicates")
                Dim orderExists As Boolean = False

                wln("Checking for existing order at: " & Now.ToString)
                Select Case My.Settings.ImportToMode
                    Case 0
                        orderExists = _Utils.InvoiceExists(GetFinalOrderID(o.OrderNumber), ses, versionInfo)
                    Case 1
                        orderExists = _Utils.SalesReceiptExists(GetFinalOrderID(o.OrderNumber), ses, versionInfo)
                    Case 2
                        orderExists = _Utils.SalesOrderExists(GetFinalOrderID(o.OrderNumber), ses, versionInfo)
                    Case Else
                        orderExists = _Utils.InvoiceExists(GetFinalOrderID(o.OrderNumber), ses, versionInfo)
                End Select
                wln("Finished Checking for existing order at: " & Now.ToString)

                If orderExists = True Then
                    If Me._SkipAllOrderDuplicateChecks = True Then
                        wln("Skipping All Checks and Skipping Duplicate Orders")
                        createOrder = False
                    Else
                        wln("Order Already Exists, Prompt User")
                        Dim DU As New DuplicateForm
                        DU.Message = "Order " & GetFinalOrderID(o.OrderNumber) & " already exists. Create a duplicate order?"
                        Select Case DU.ShowDialog
                            Case Windows.Forms.DialogResult.Cancel
                                createOrder = True
                            Case Windows.Forms.DialogResult.OK
                                createOrder = False
                            Case Windows.Forms.DialogResult.Ignore
                                createOrder = False
                                Me._SkipAllOrderDuplicateChecks = True
                            Case Else
                                createOrder = False
                        End Select
                        DU.Dispose()
                    End If
                End If


                If createOrder = True Then


                    If o.PaymentStatus = BVC5WebServices.OrderPaymentStatus.Paid Then
                        wln("Importing PAID Order")
                        result = SaveOrderToQuickBooksBVC5(o, ses, versionInfo)
                    Else
                        If My.Settings.ImportOrderMode = 1 Then
                            wln("Using ALL ORDERS Import")
                            result = SaveOrderToQuickBooksBVC5(o, ses, versionInfo)
                        Else
                            If My.Settings.ImportOrderMode = 2 Then ' AUTH
                                wln("Using AUTH ORDERS Import")
                                result = SaveOrderToQuickBooksBVC5(o, ses, versionInfo)
                            Else
                                Me.SkippedOrders.Add(o.OrderNumber)
                                wln("Order Skipped as UNPAID")
                            End If
                        End If
                    End If
                Else
                    wln("Skipping Order Because of Duplicate Settings.")
                End If
            Else
                wln("ERROR: Order Object was not Created!")
                Throw New Exception("Failed to load Order " & o.OrderNumber & " from store.")
            End If
        Catch sp As System.Web.Services.Protocols.SoapException
            wln("SOAP ERROR: " & sp.Message)
            wln(" - Target: " & sp.TargetSite.Name)
            wln(" - " & sp.StackTrace)
            Logging.LogException(sp)
            result = False
            _Errors.Add(sp.Message)
            Me.ProblemOrders.Add(o.OrderNumber)
        Catch ex As Exception
            Logging.LogException(ex)
            result = False
            _Errors.Add(ex.Message)
            Me.ProblemOrders.Add(o.OrderNumber)
        End Try

        wln("Result: " & result)
        wln("SaveLastOrder: " & _SaveLastOrder)
        If (result = True) And (_SaveLastOrder = True) Then
            wln("Setting Last Order Number")
            My.Settings.LastOrderNumber = o.OrderNumber
        Else
            wln("Not Setting Last Order Number")
        End If
    End Sub

    Private Function GetFinalOrderID(ByVal inputID As String) As String
        Dim result As String = inputID

        If My.Settings.UseOrderPrefix = True Then
            result = My.Settings.OrderPrefix & inputID
            result = QuickBooksUtils.TrimToLength(result, 11)
        End If

        Return result
    End Function

    Private Function SaveOrderToQuickBooksBVC5(ByRef thisOrder As BVC5WebServices.Order, ByRef ses As QBFC7Lib.QBSessionManager, ByVal versionInfo As QBVersionInfo) As Boolean
        Dim result As Boolean = False
        wln("Starting Save to QuickBooks at: " & Now.ToString)

        Try

            ' Check for Customer and Create as needed
            Dim customerInfo As New QBCustomerInfo
            customerInfo.ListID = "-1"
            customerInfo.Email = thisOrder.UserEmail
            customerInfo.FirstName = thisOrder.BillingAddress.FirstName
            customerInfo.Lastname = thisOrder.BillingAddress.LastName
            customerInfo.PhoneNumber = thisOrder.BillingAddress.Phone
            customerInfo.BillingAddress.LoadBvc5Address(thisOrder.BillingAddress, CInt(versionInfo.MajorVersion))
            customerInfo.ShippingAddress.LoadBvc5Address(thisOrder.ShippingAddress, CInt(versionInfo.MajorVersion))

            wln("Starting to Get Customer Info at: " & Now.ToString)
            customerInfo = GetUserListID(customerInfo, ses, versionInfo)
            wln("Finished getting Customer Info at: " & Now.ToString)

            If customerInfo.ListID = "-1" Then
                Throw New ArgumentException("No valid userID was returned!")
            Else

                wln("Starting To Update Customer at: " & Now.ToString)

                ' Update new info
                customerInfo.BillingAddress.LoadBvc5Address(thisOrder.BillingAddress, CInt(versionInfo.MajorVersion))
                customerInfo.ShippingAddress.LoadBvc5Address(thisOrder.ShippingAddress, CInt(versionInfo.MajorVersion))
                CopyCreditCardInformationToCustomer(customerInfo, thisOrder)

                ' Update customer records with new address info.
                _Utils.CustomerUpdate(customerInfo.ListID, customerInfo, ses, versionInfo)
                wln("Finished Updating Customer at: " & Now.ToString)

                Select Case My.Settings.ImportToMode
                    Case 0
                        ' Create Invoice
                        wln("Creating Invoice at: " & Now.ToString)
                        result = _Utils.InvoiceCreate5(thisOrder, customerInfo.ListID, ses, versionInfo)
                        wln("Finished Creating Invoice at: " & Now.ToString)
                    Case 1
                        ' Create Sales Receipt
                        wln("Creating Sales Receipt at: " & Now.ToString)
                        result = _Utils.SalesReceiptCreate5(thisOrder, customerInfo.ListID, ses, versionInfo)
                        wln("Finished Creating Sales Receipt at: " & Now.ToString)
                    Case 2
                        ' Create Sales Order
                        wln("Creating Sales Order at: " & Now.ToString)
                        result = _Utils.SalesOrderCreate5(thisOrder, customerInfo.ListID, ses, versionInfo)
                        wln("Finished Creating Sales Order at: " & Now.ToString)
                End Select
            End If

        Catch ex As Exception
            Logging.LogException(ex)
            _Errors.Add("Error while saving order " & thisOrder.OrderNumber & " " & ex.Message)
            _ProblemOrders.Add(thisOrder.OrderNumber)
            result = False
        End Try

        wln("Finished Save to QuickBooks at: " & Now.ToString)
        Return result
    End Function

    Private Sub CopyCreditCardInformationToCustomer(ByRef c As QBCustomerInfo, ByRef o As BVC5WebServices.Order)
        If Not o.Payments Is Nothing Then
            If o.Payments.Length > 0 Then
                For i As Integer = 0 To o.Payments.Length - 1
                    With o.Payments(i)
                        If .CreditCardNumber.Trim.Length > 0 Then
                            c.CreditCardInfo.CreditCardNumber = .CreditCardNumber
                            c.CreditCardInfo.ExpirationMonth = .CreditCardExpMonth
                            c.CreditCardInfo.ExpirationYear = .CreditCardExpYear
                            c.CreditCardInfo.NameOnCard = .CreditCardHolder
                            If Not o.BillingAddress Is Nothing Then
                                c.CreditCardInfo.CreditCardAddress = o.BillingAddress.Line1
                                c.CreditCardInfo.CreditCardPostalCode = o.BillingAddress.PostalCode
                            End If
                            Exit For
                        End If
                    End With
                Next
            End If
        End If
    End Sub

    Private Function GetUserListID(ByRef customer As QBCustomerInfo, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBCustomerInfo

        If My.Settings.ExportUsersAsName = True Or My.Settings.ExportReverseUserName = True Then
            wln("Creating Unique Customer by Name")
            Return CreateUniqueUser(customer, False, 0, ses, ver)
        Else
            wln("Creating Unique Customer by Email")
            Return CreateUniqueUser(customer, True, 0, ses, ver)
        End If

    End Function

    Private Function CreateUniqueUser(ByRef customer As QBCustomerInfo, ByVal byEmail As Boolean, ByVal copycount As Integer, ByRef ses As QBFC7Lib.QBSessionManager, ByVal ver As QBVersionInfo) As QBCustomerInfo
        Dim result As New QBCustomerInfo
        result.ListID = "-1"

        Dim currentInfo As New QBCustomerInfo
        ' If this is a second request, append copy suffix

        ' Do check for both name formats since QB will baulk even though it won't find the user.
        If byEmail = True Then
            If copycount > 0 Then
                currentInfo = _Utils.CustomerExists(customer.Email & " (" & copycount & ")", ses, ver)
            Else
                currentInfo = _Utils.CustomerExists(customer.Email, ses, ver)
            End If
        Else
            If copycount > 0 Then
                currentInfo = _Utils.CustomerExists(customer.Lastname & ", " & customer.FirstName & " (" & copycount & ")", ses, ver)
                If currentInfo.ListID = "-1" Then
                    currentInfo = _Utils.CustomerExists(customer.FirstName & " " & customer.Lastname & " (" & copycount & ")", ses, ver)
                End If
                If (currentInfo.ListID = "-1") AndAlso (My.Settings.CreateCustomersAsCompany = True) Then
                    If customer.BillingAddress.Company.Trim.Length > 0 Then
                        currentInfo = _Utils.CustomerExists(customer.BillingAddress.Company & " (" & copycount & ")", ses, ver)
                    End If
                End If
            Else
                currentInfo = _Utils.CustomerExists(customer.Lastname & ", " & customer.FirstName, ses, ver)
                If currentInfo.ListID = "-1" Then
                    currentInfo = _Utils.CustomerExists(customer.FirstName & " " & customer.Lastname, ses, ver)
                End If
                If (currentInfo.ListID = "-1") AndAlso (My.Settings.CreateCustomersAsCompany = True) Then
                    If customer.BillingAddress.Company.Trim.Length > 0 Then
                        currentInfo = _Utils.CustomerExists(customer.BillingAddress.Company, ses, ver)
                    End If
                End If
            End If
        End If


        wln("CustomerInfo after Exists Check = " & currentInfo.ListID)

        If currentInfo.ListID = "-1" Then
            wln("Creating New Customer")
            ' Create Customer since one doesn't exist yet

            Dim username As String = ""


            If (My.Settings.CreateCustomersAsCompany = True) AndAlso (customer.BillingAddress.Company.Trim.Length > 0) Then
                username = customer.BillingAddress.Company
            End If

            If username = String.Empty Then
                If My.Settings.ExportReverseUserName = True Then
                    username = customer.FirstName & " " & customer.Lastname
                ElseIf My.Settings.ExportUsersAsName = True Then
                    username = customer.Lastname & ", " & customer.FirstName
                Else
                    username = customer.Email
                End If
            End If

            If copycount > 0 Then
                result = _Utils.CustomerCreate(username & " (" & copycount & ")", customer.Email, customer, ses, ver)
            Else
                result = _Utils.CustomerCreate(username, customer.Email, customer, ses, ver)
            End If
        Else
            wln("Checking For Match")
            ' Customer with same name already exists
            If currentInfo.Email = customer.Email Then
                ' Customers are the same so return
                wln("Match Found!")
                result = currentInfo
            Else
                wln("No Match - Create Copy " & copycount + 1)
                result = CreateUniqueUser(customer, byEmail, copycount + 1, ses, ver)
            End If
        End If

        wln("Customer Result = " & result.ListID)

        Return result
    End Function

    Private Sub AbbreviateAddresses(ByRef o As BVC5WebServices.Order)
        Dim regions() As BVC5WebServices.Region = MainForm.WebStore5.Content_Region_FindByCountry(MainForm.WebToken5, o.BillingAddress.CountryBvin)
        If regions IsNot Nothing Then
            For Each r As BVC5WebServices.Region In regions
                If r.Bvin = o.BillingAddress.RegionBvin Then
                    o.BillingAddress.RegionName = r.Abbreviation
                End If

                If r.Bvin = o.ShippingAddress.RegionBvin Then
                    o.ShippingAddress.RegionName = r.Abbreviation
                End If
            Next
        End If
    End Sub

    Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click

    End Sub

End Class