<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AboutForm))
        Me.OkayButton = New System.Windows.Forms.Button
        Me.NameField = New System.Windows.Forms.Label
        Me.VersionField = New System.Windows.Forms.Label
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
        Me.SuspendLayout()
        '
        'OkayButton
        '
        Me.OkayButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OkayButton.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OkayButton.Location = New System.Drawing.Point(175, 67)
        Me.OkayButton.Name = "OkayButton"
        Me.OkayButton.Size = New System.Drawing.Size(75, 23)
        Me.OkayButton.TabIndex = 0
        Me.OkayButton.Text = "OK"
        Me.OkayButton.UseVisualStyleBackColor = True
        '
        'NameField
        '
        Me.NameField.AutoSize = True
        Me.NameField.Location = New System.Drawing.Point(12, 18)
        Me.NameField.Name = "NameField"
        Me.NameField.Size = New System.Drawing.Size(75, 13)
        Me.NameField.TabIndex = 1
        Me.NameField.Text = "Product Name"
        '
        'VersionField
        '
        Me.VersionField.AutoSize = True
        Me.VersionField.Location = New System.Drawing.Point(12, 41)
        Me.VersionField.Name = "VersionField"
        Me.VersionField.Size = New System.Drawing.Size(40, 13)
        Me.VersionField.TabIndex = 2
        Me.VersionField.Text = "0.0.0.0"
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Location = New System.Drawing.Point(12, 80)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(44, 13)
        Me.LinkLabel1.TabIndex = 3
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "Log File"
        '
        'AboutForm
        '
        Me.AcceptButton = Me.OkayButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.OkayButton
        Me.ClientSize = New System.Drawing.Size(262, 102)
        Me.ControlBox = False
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.VersionField)
        Me.Controls.Add(Me.NameField)
        Me.Controls.Add(Me.OkayButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "AboutForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "About"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents OkayButton As System.Windows.Forms.Button
    Friend WithEvents NameField As System.Windows.Forms.Label
    Friend WithEvents VersionField As System.Windows.Forms.Label
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
End Class
