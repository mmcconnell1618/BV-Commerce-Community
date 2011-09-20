' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt

Public Class ExportSelectionForm

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

    Private Sub rbLatestOrders_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbLatestOrders.CheckedChanged
        SetControlVisibility()
    End Sub

    Private Sub SetControlVisibility()
        Me.LastOrderField.Enabled = False
        Me.SingleOrderField.Enabled = False
        Me.StartRangeField.Enabled = False
        Me.EndRangeField.Enabled = False

        If Me.rbLatestOrders.Checked = True Then
            Me.LastOrderField.Enabled = True
        ElseIf Me.rbSingleOrder.Checked = True Then
            Me.SingleOrderField.Enabled = True
        Else
            Me.StartRangeField.Enabled = True
            Me.EndRangeField.Enabled = True
        End If
    End Sub

    Private Sub rbSingleOrder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbSingleOrder.CheckedChanged
        SetControlVisibility()
    End Sub

    Private Sub rbOrderRange_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbOrderRange.CheckedChanged
        SetControlVisibility()
    End Sub

    Private Sub OkayButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkayButton.Click
        If Me.CheckForQuickBooks = True Then
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Function CheckForQuickBooks() As Boolean
        Dim result As Boolean = False

        Dim q As New QBFC7Lib.QBSessionManager

        Try

            q.OpenConnection2("", My.Settings.QBApplicationName, QBFC7Lib.ENConnectionType.ctLocalQBD)

            If Me.rbCurrentFile.Checked = True Then
                q.BeginSession("", QBFC7Lib.ENOpenMode.omDontCare)
            Else
                q.BeginSession(Me.CompanyFileField.Text.Trim, QBFC7Lib.ENOpenMode.omDontCare)
            End If

            result = True
        Catch ex As Exception
            Logging.LogException(ex)
            MsgBox(ex.Message)
        Finally
            q.EndSession()
            q.CloseConnection()
            q = Nothing
        End Try

        Return result
    End Function

    Private Sub ExportSelectionForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.LastOrderField.Text = My.Settings.LastOrderNumber
        Me.SingleOrderField.Text = My.Settings.LastOrderNumber
    End Sub
End Class