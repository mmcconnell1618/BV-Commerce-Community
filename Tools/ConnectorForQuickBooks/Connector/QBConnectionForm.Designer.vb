<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class QBConnectionForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(QBConnectionForm))
        Me.CancelButton2 = New System.Windows.Forms.Button
        Me.OkayButton = New System.Windows.Forms.Button
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.BrowseButton = New System.Windows.Forms.Button
        Me.CompanyFileField = New System.Windows.Forms.TextBox
        Me.rbUserFile = New System.Windows.Forms.RadioButton
        Me.rbCurrentFile = New System.Windows.Forms.RadioButton
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'CancelButton2
        '
        Me.CancelButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelButton2.Location = New System.Drawing.Point(256, 89)
        Me.CancelButton2.Name = "CancelButton2"
        Me.CancelButton2.Size = New System.Drawing.Size(75, 23)
        Me.CancelButton2.TabIndex = 12
        Me.CancelButton2.Text = "Cancel"
        Me.CancelButton2.UseVisualStyleBackColor = True
        '
        'OkayButton
        '
        Me.OkayButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OkayButton.Location = New System.Drawing.Point(337, 89)
        Me.OkayButton.Name = "OkayButton"
        Me.OkayButton.Size = New System.Drawing.Size(75, 23)
        Me.OkayButton.TabIndex = 11
        Me.OkayButton.Text = "OK"
        Me.OkayButton.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.BrowseButton)
        Me.GroupBox2.Controls.Add(Me.CompanyFileField)
        Me.GroupBox2.Controls.Add(Me.rbUserFile)
        Me.GroupBox2.Controls.Add(Me.rbCurrentFile)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(400, 67)
        Me.GroupBox2.TabIndex = 10
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "QuickBooks File Selection"
        '
        'BrowseButton
        '
        Me.BrowseButton.Location = New System.Drawing.Point(308, 39)
        Me.BrowseButton.Name = "BrowseButton"
        Me.BrowseButton.Size = New System.Drawing.Size(75, 23)
        Me.BrowseButton.TabIndex = 3
        Me.BrowseButton.Text = "Browse"
        Me.BrowseButton.UseVisualStyleBackColor = True
        '
        'CompanyFileField
        '
        Me.CompanyFileField.Location = New System.Drawing.Point(146, 41)
        Me.CompanyFileField.Name = "CompanyFileField"
        Me.CompanyFileField.Size = New System.Drawing.Size(156, 20)
        Me.CompanyFileField.TabIndex = 2
        '
        'rbUserFile
        '
        Me.rbUserFile.AutoSize = True
        Me.rbUserFile.Location = New System.Drawing.Point(6, 42)
        Me.rbUserFile.Name = "rbUserFile"
        Me.rbUserFile.Size = New System.Drawing.Size(139, 17)
        Me.rbUserFile.TabIndex = 1
        Me.rbUserFile.Text = "Export to this file instead"
        Me.rbUserFile.UseVisualStyleBackColor = True
        '
        'rbCurrentFile
        '
        Me.rbCurrentFile.AutoSize = True
        Me.rbCurrentFile.Checked = True
        Me.rbCurrentFile.Location = New System.Drawing.Point(6, 19)
        Me.rbCurrentFile.Name = "rbCurrentFile"
        Me.rbCurrentFile.Size = New System.Drawing.Size(238, 17)
        Me.rbCurrentFile.TabIndex = 0
        Me.rbCurrentFile.TabStop = True
        Me.rbCurrentFile.Text = "Export to the Currently Open QuickBooks File"
        Me.rbCurrentFile.UseVisualStyleBackColor = True
        '
        'QBConnectionForm
        '
        Me.AcceptButton = Me.OkayButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CancelButton2
        Me.ClientSize = New System.Drawing.Size(424, 124)
        Me.ControlBox = False
        Me.Controls.Add(Me.CancelButton2)
        Me.Controls.Add(Me.OkayButton)
        Me.Controls.Add(Me.GroupBox2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "QBConnectionForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Connection Form"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CancelButton2 As System.Windows.Forms.Button
    Friend WithEvents OkayButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents BrowseButton As System.Windows.Forms.Button
    Friend WithEvents CompanyFileField As System.Windows.Forms.TextBox
    Friend WithEvents rbUserFile As System.Windows.Forms.RadioButton
    Friend WithEvents rbCurrentFile As System.Windows.Forms.RadioButton
End Class
