' This software is copyrighted 2002-2009 by 
' BV Software, LLC and Marcus McConnell. 
' All Rights Reserved

' This software is licensed under the 
' Microsoft Reciprocal License. 
' By using this software and/or source code 
' you agree to this license. A copy of the license
' can be found online or included in this 
' distribution in License.txt

Imports System.IO

Public Class LoginForm

    Private Class LoginPacket
        Public Url As String = String.Empty
        Public UserName As String = String.Empty
        Public Password As String = String.Empty
        Public service5 As BVC5WebServices.WebServices3
        Public Token5 As BVC5WebServices.AuthenticationToken
    End Class

    Private Class LoginResult
        Public Completed As Boolean = False
        Public ErrorMessage As String = String.Empty
        Public Service5 As BVC5WebServices.WebServices3
        Public Token5 As BVC5WebServices.AuthenticationToken
    End Class

    Public Property WebStoreUrl() As String
        Get
            Return Me.WebStoreUrlField.Text.Trim
        End Get
        Set(ByVal value As String)
            Me.WebStoreUrlField.Text = value
        End Set
    End Property
    Public Property WebUsername() As String
        Get
            Return Me.UsernameField.Text.Trim
        End Get
        Set(ByVal value As String)
            Me.UsernameField.Text = value
        End Set
    End Property
    Public Property WebPassword() As String
        Get
            Return Me.PasswordField.Text.Trim
        End Get
        Set(ByVal value As String)
            Me.PasswordField.Text = value
        End Set
    End Property

    Private Sub OkayButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkayButton.Click
        Dim result As Boolean = False

        If Me.WebStoreUrl.EndsWith("/") = False Then
            Me.WebStoreUrl += "/"
        End If

        Dim lp As New LoginPacket
        lp.Password = Me.WebPassword
        lp.Url = Me.WebStoreUrl
        lp.UserName = Me.WebUsername

        lp.service5 = MainForm.WebStore5        

        Me.OkayButton.Enabled = False
        LoginWorker.RunWorkerAsync(lp)

    End Sub

    Private Function CheckAccount() As Boolean
        Dim result As Boolean = False

        Try
            MainForm.WebStore5.Url = Path.Combine(Me.WebStoreUrl, "WebServices3.asmx")
            MainForm.WebToken5 = MainForm.WebStore5.Login(Me.WebUsername, Me.WebPassword)
            If Not MainForm.WebToken5 Is Nothing Then
                If MainForm.WebToken5.TokenRejected = False Then
                    result = True
                Else
                    MsgBox("That username and password combination is not a valid order admin on this site. Please check your settings and try again.")
                End If
            Else
                MsgBox("An error occued while attempting to get an Authentication Token")
            End If

        Catch sp As System.Web.Services.Protocols.SoapException
            Cursor = Cursors.Default
            MsgBox("Soap Exception: " & sp.Message)
            Logging.LogException(sp)
        Catch ex As Exception
            Cursor = Cursors.Default
            Logging.LogException(ex)
            MsgBox("Error: " & ex.Message)
        End Try

        Return result
    End Function

    Private Sub LoginWorker_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles LoginWorker.DoWork

        'LoginWorker.ReportProgress(25)

        Dim info As LoginPacket = CType(e.Argument, LoginPacket)

        Dim result As New LoginResult

        Try
            info.service5.Url = Path.Combine(info.Url, "WebServices3.asmx")
            If LoginWorker.CancellationPending Then
                e.Cancel = True
                Return
            Else
                info.Token5 = info.service5.Login(info.UserName, info.Password)
                If Not info.Token5 Is Nothing Then
                    If info.Token5.TokenRejected = False Then
                        result.Token5 = info.Token5
                        result.Service5 = info.service5
                        result.Completed = True
                    Else
                        result.ErrorMessage = "That username and password combination is not a valid order admin on this site. Please check your settings and try again."
                    End If
                Else
                    result.ErrorMessage = "An error occued while attempting to get an Authentication Token"
                End If
            End If

        Catch sp As System.Web.Services.Protocols.SoapException
            result.ErrorMessage = "Soap Exception: " & sp.Message
        Catch ex As Exception
            result.ErrorMessage = "Error: " & ex.Message
        End Try

        'LoginWorker.ReportProgress(100)

        e.Result = result
    End Sub

    Private Sub LoginWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles LoginWorker.RunWorkerCompleted
        If CType(e.Result, LoginResult).Completed = True Then
            MainForm.WebStore5 = CType(e.Result, LoginResult).Service5
            MainForm.WebToken5 = CType(e.Result, LoginResult).Token5

            Me.DialogResult = Windows.Forms.DialogResult.OK
        Else
            If e.Cancelled = True Then
                MsgBox("Cancelled")
            Else
                MsgBox(CType(e.Result, LoginResult).ErrorMessage)
            End If
        End If

        Me.OkayButton.Enabled = True
    End Sub

    Private Sub CancelButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelButton2.Click
        If LoginWorker.IsBusy = True Then
            LoginWorker.CancelAsync()
        Else
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End If
    End Sub

End Class