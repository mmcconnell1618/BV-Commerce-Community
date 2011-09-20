<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoginForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LoginForm))
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.WebStoreUrlField = New System.Windows.Forms.TextBox
        Me.UsernameField = New System.Windows.Forms.TextBox
        Me.PasswordField = New System.Windows.Forms.TextBox
        Me.OkayButton = New System.Windows.Forms.Button
        Me.CancelButton2 = New System.Windows.Forms.Button
        Me.LoginWorker = New System.ComponentModel.BackgroundWorker
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Web Store Url:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(31, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Username:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(33, 65)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Password:"
        '
        'WebStoreUrlField
        '
        Me.WebStoreUrlField.Location = New System.Drawing.Point(95, 12)
        Me.WebStoreUrlField.Name = "WebStoreUrlField"
        Me.WebStoreUrlField.Size = New System.Drawing.Size(337, 20)
        Me.WebStoreUrlField.TabIndex = 3
        '
        'UsernameField
        '
        Me.UsernameField.Location = New System.Drawing.Point(95, 38)
        Me.UsernameField.Name = "UsernameField"
        Me.UsernameField.Size = New System.Drawing.Size(337, 20)
        Me.UsernameField.TabIndex = 4
        '
        'PasswordField
        '
        Me.PasswordField.Location = New System.Drawing.Point(95, 62)
        Me.PasswordField.Name = "PasswordField"
        Me.PasswordField.Size = New System.Drawing.Size(337, 20)
        Me.PasswordField.TabIndex = 5
        Me.PasswordField.UseSystemPasswordChar = True
        '
        'OkayButton
        '
        Me.OkayButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OkayButton.Location = New System.Drawing.Point(357, 95)
        Me.OkayButton.Name = "OkayButton"
        Me.OkayButton.Size = New System.Drawing.Size(75, 23)
        Me.OkayButton.TabIndex = 6
        Me.OkayButton.Text = "OK"
        Me.OkayButton.UseVisualStyleBackColor = True
        '
        'CancelButton2
        '
        Me.CancelButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelButton2.Location = New System.Drawing.Point(276, 95)
        Me.CancelButton2.Name = "CancelButton2"
        Me.CancelButton2.Size = New System.Drawing.Size(75, 23)
        Me.CancelButton2.TabIndex = 7
        Me.CancelButton2.Text = "Cancel"
        Me.CancelButton2.UseVisualStyleBackColor = True
        '
        'LoginWorker
        '
        Me.LoginWorker.WorkerSupportsCancellation = True
        '
        'LoginForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(444, 130)
        Me.ControlBox = False
        Me.Controls.Add(Me.CancelButton2)
        Me.Controls.Add(Me.OkayButton)
        Me.Controls.Add(Me.PasswordField)
        Me.Controls.Add(Me.UsernameField)
        Me.Controls.Add(Me.WebStoreUrlField)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "LoginForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Login To Store"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents WebStoreUrlField As System.Windows.Forms.TextBox
    Friend WithEvents UsernameField As System.Windows.Forms.TextBox
    Friend WithEvents PasswordField As System.Windows.Forms.TextBox
    Friend WithEvents OkayButton As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents CancelButton2 As System.Windows.Forms.Button
    Friend WithEvents LoginWorker As System.ComponentModel.BackgroundWorker
End Class
