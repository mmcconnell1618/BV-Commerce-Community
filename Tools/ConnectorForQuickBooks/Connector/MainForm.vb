' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt

Public Class MainForm

    Private _WebUsername As String = String.Empty
    Private _WebPassword As String = String.Empty

    Private _WebStore5 As New BVC5WebServices.WebServices3
    Private _WebToken5 As New BVC5WebServices.AuthenticationToken

    Public Property WebStore5() As BVC5WebServices.WebServices3
        Get
            Return _WebStore5
        End Get
        Set(ByVal value As BVC5WebServices.WebServices3)
            _WebStore5 = value
        End Set
    End Property
    Public Property WebToken5() As BVC5WebServices.AuthenticationToken
        Get
            Return _WebToken5
        End Get
        Set(ByVal value As BVC5WebServices.AuthenticationToken)
            _WebToken5 = value
        End Set
    End Property


    Private Sub ExitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitButton.Click
        Application.Exit()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Application.Exit()
    End Sub

    Private Sub AboutButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutButton.Click
        ShowAbout()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ShowAbout()
    End Sub

    Private Sub ShowAbout()
        Dim a As New AboutForm
        a.ShowDialog()
    End Sub

    Private Sub OpenSite()

        Dim lf As New LoginForm
        lf.WebStoreUrl = My.Settings.WebStoreUrl
        lf.WebUsername = My.Settings.WebUsename
        lf.WebPassword = My.Settings.WebPassword


        If lf.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.WebPassword = lf.WebPassword
            My.Settings.WebUsename = lf.WebUsername
            My.Settings.WebStoreUrl = lf.WebStoreUrl
            lf.Dispose()
            StartExport()
        Else
            lf.Dispose()
        End If

    End Sub

    Private Sub OpenStoreButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenStoreButton.Click
        OpenSite()
    End Sub

    Private Sub OpenStoreToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        OpenSite()
    End Sub

    Private Sub StartExport()

        Try
            Me.Enabled = False

            Dim es As New ExportSelectionForm


            es.LastOrderField.Text = My.Settings.LastOrderNumber
            es.SingleOrderField.Text = My.Settings.SingleOrderNumber
            es.StartRangeField.Text = My.Settings.StartOrderNumber
            es.EndRangeField.Text = My.Settings.EndOrderNumber
            If My.Settings.UseCompanyFile = True Then
                es.rbUserFile.Checked = True
                es.rbCurrentFile.Checked = False
            End If
            es.CompanyFileField.Text = My.Settings.CompanyFileName
            es.rbLatestOrders.Checked = True

            If es.ShowDialog = Windows.Forms.DialogResult.OK Then

                'Save QB Settings
                My.Settings.UseCompanyFile = es.rbUserFile.Checked
                My.Settings.CompanyFileName = es.CompanyFileField.Text.Trim
                My.Settings.EndOrderNumber = es.EndRangeField.Text.Trim
                My.Settings.SingleOrderNumber = es.SingleOrderField.Text.Trim
                My.Settings.StartOrderNumber = es.StartRangeField.Text.Trim

                'Get Orders to Process
                Dim orderIDs As New Collection(Of String)

                If es.rbLatestOrders.Checked = True Then
                    PopulateLatestOrders(orderIDs, CInt(es.LastOrderField.Text.Trim))
                ElseIf es.rbSingleOrder.Checked = True Then
                    PopulateOrderRange(orderIDs, CInt(es.SingleOrderField.Text.Trim), CInt(es.SingleOrderField.Text.Trim))
                Else
                    PopulateOrderRange(orderIDs, CInt(es.StartRangeField.Text.Trim), CInt(es.EndRangeField.Text.Trim))
                End If

                If orderIDs.Count > 0 Then

                    ProcessOrders(orderIDs, es.rbLatestOrders.Checked)

                    MsgBox("Export Finished")
                Else
                    MsgBox("No orders were found to export.")
                End If

            End If

            es.Dispose()

        Catch ex As Exception
            Logging.LogException(ex)
            MsgBox("Error Running Export: " & ex.Message)
        Finally
            Me.Enabled = True
        End Try

    End Sub

    Private Sub PopulateLatestOrders(ByRef o As Collection(Of String), ByVal lastOrder As Integer)
        Cursor = Cursors.WaitCursor
        Application.DoEvents()

        Try            
            Dim startOrder As Integer = lastOrder + 1
            Dim endOrder As Integer = 32000000
            PopulateOrderRange(o, startOrder, endOrder)
        Catch sp As System.Web.Services.Protocols.SoapException
            Logging.LogException(sp)
            Cursor = Cursors.Default
            MsgBox("Soap Exception Populating Orders: " & sp.Message)
        Catch ex As Exception
            Logging.LogException(ex)
            Cursor = Cursors.Default
            MsgBox("Error Populating Orders: " & ex.Message)
        End Try

        Cursor = Cursors.Default
    End Sub

    Private Sub PopulateOrderRange(ByRef o As Collection(Of String), ByVal startOrder As Integer, ByVal endOrder As Integer)
        Cursor = Cursors.WaitCursor
        Application.DoEvents()

        Try

            Dim store5Orders() As BVC5WebServices.Order = WebStore5.Orders_Order_FindByRange(WebToken5, startOrder.ToString, endOrder.ToString)
            If store5Orders IsNot Nothing Then
                For Each od As BVC5WebServices.Order In store5Orders
                    o.Add(od.OrderNumber)
                Next
            End If

        Catch sp As System.Web.Services.Protocols.SoapException
            Logging.LogException(sp)
            Cursor = Cursors.Default
            MsgBox("Soap Exception Populating Order Range: " & sp.Message)
        Catch ex As Exception
            Logging.LogException(ex)
            Cursor = Cursors.Default
            MsgBox("Error Populating Order Range: " & ex.Message)
        End Try

        Cursor = Cursors.Default
    End Sub

    Private Sub ProcessOrders(ByRef orders As Collection(Of String), ByVal savelastOrder As Boolean)
        Dim ew As New ExportWorker
        ew.OrderList = orders
        ew.SaveLastOrder = savelastOrder
        ew.Show()
        ew.Focus()
        Application.DoEvents()
        ew.StartExporting()
        ew.Hide()
        ew.Dispose()
    End Sub

    Private Sub OptionsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptionsButton.Click
        ShowOptions()
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ShowOptions()
    End Sub

    Private Sub ShowOptions()
        OptionsForm.ShowDialog()
        OptionsForm.LoadSettings()
    End Sub

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If My.Settings.FirstRun = True Then
            If OptionsForm.ShowDialog = Windows.Forms.DialogResult.OK Then
                My.Settings.FirstRun = False
            End If
        End If
    End Sub

    Private Sub SendItemsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SendItemsButton.Click
        Dim lf As New LoginForm
        lf.WebStoreUrl = My.Settings.WebStoreUrl
        lf.WebUsername = My.Settings.WebUsename
        lf.WebPassword = My.Settings.WebPassword

        If lf.ShowDialog = Windows.Forms.DialogResult.OK Then
            My.Settings.WebPassword = lf.WebPassword
            My.Settings.WebUsename = lf.WebUsername
            My.Settings.WebStoreUrl = lf.WebStoreUrl
            lf.Dispose()
            SendItems()
        Else
            lf.Dispose()
        End If
    End Sub

    Private Sub SendItems()
        Try
            Me.Enabled = False
            Dim connForm As New QBConnectionForm
            If connForm.ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim siw As New SendItemsWorker
                siw.ShowDialog()
                siw.Dispose()
            End If
            connForm.Dispose()
        Catch ex As Exception
            MsgBox("Error Running Export: " & ex.Message)
        Finally
            Me.Enabled = True
        End Try
    End Sub

End Class