<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExportSelectionForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ExportSelectionForm))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.LastOrderField = New System.Windows.Forms.TextBox
        Me.SingleOrderField = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.EndRangeField = New System.Windows.Forms.TextBox
        Me.StartRangeField = New System.Windows.Forms.TextBox
        Me.rbOrderRange = New System.Windows.Forms.RadioButton
        Me.rbSingleOrder = New System.Windows.Forms.RadioButton
        Me.rbLatestOrders = New System.Windows.Forms.RadioButton
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.BrowseButton = New System.Windows.Forms.Button
        Me.CompanyFileField = New System.Windows.Forms.TextBox
        Me.rbUserFile = New System.Windows.Forms.RadioButton
        Me.rbCurrentFile = New System.Windows.Forms.RadioButton
        Me.CancelButton2 = New System.Windows.Forms.Button
        Me.OkayButton = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.LastOrderField)
        Me.GroupBox1.Controls.Add(Me.SingleOrderField)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.EndRangeField)
        Me.GroupBox1.Controls.Add(Me.StartRangeField)
        Me.GroupBox1.Controls.Add(Me.rbOrderRange)
        Me.GroupBox1.Controls.Add(Me.rbSingleOrder)
        Me.GroupBox1.Controls.Add(Me.rbLatestOrders)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(400, 100)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Select Orders to Export"
        '
        'LastOrderField
        '
        Me.LastOrderField.Location = New System.Drawing.Point(146, 18)
        Me.LastOrderField.Name = "LastOrderField"
        Me.LastOrderField.Size = New System.Drawing.Size(100, 20)
        Me.LastOrderField.TabIndex = 7
        Me.LastOrderField.Text = "0"
        '
        'SingleOrderField
        '
        Me.SingleOrderField.Location = New System.Drawing.Point(146, 41)
        Me.SingleOrderField.Name = "SingleOrderField"
        Me.SingleOrderField.Size = New System.Drawing.Size(100, 20)
        Me.SingleOrderField.TabIndex = 6
        Me.SingleOrderField.Text = "0"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(252, 67)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(25, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "and"
        '
        'EndRangeField
        '
        Me.EndRangeField.Location = New System.Drawing.Point(283, 64)
        Me.EndRangeField.Name = "EndRangeField"
        Me.EndRangeField.Size = New System.Drawing.Size(100, 20)
        Me.EndRangeField.TabIndex = 4
        Me.EndRangeField.Text = "0"
        '
        'StartRangeField
        '
        Me.StartRangeField.Location = New System.Drawing.Point(146, 64)
        Me.StartRangeField.Name = "StartRangeField"
        Me.StartRangeField.Size = New System.Drawing.Size(100, 20)
        Me.StartRangeField.TabIndex = 3
        Me.StartRangeField.Text = "0"
        '
        'rbOrderRange
        '
        Me.rbOrderRange.AutoSize = True
        Me.rbOrderRange.Location = New System.Drawing.Point(6, 65)
        Me.rbOrderRange.Name = "rbOrderRange"
        Me.rbOrderRange.Size = New System.Drawing.Size(134, 17)
        Me.rbOrderRange.TabIndex = 2
        Me.rbOrderRange.Text = "Export Orders Between"
        Me.rbOrderRange.UseVisualStyleBackColor = True
        '
        'rbSingleOrder
        '
        Me.rbSingleOrder.AutoSize = True
        Me.rbSingleOrder.Location = New System.Drawing.Point(6, 42)
        Me.rbSingleOrder.Name = "rbSingleOrder"
        Me.rbSingleOrder.Size = New System.Drawing.Size(125, 17)
        Me.rbSingleOrder.TabIndex = 1
        Me.rbSingleOrder.Text = "Export a Single Order"
        Me.rbSingleOrder.UseVisualStyleBackColor = True
        '
        'rbLatestOrders
        '
        Me.rbLatestOrders.AutoSize = True
        Me.rbLatestOrders.Checked = True
        Me.rbLatestOrders.Location = New System.Drawing.Point(6, 19)
        Me.rbLatestOrders.Name = "rbLatestOrders"
        Me.rbLatestOrders.Size = New System.Drawing.Size(128, 17)
        Me.rbLatestOrders.TabIndex = 0
        Me.rbLatestOrders.TabStop = True
        Me.rbLatestOrders.Text = "Export All Orders After"
        Me.rbLatestOrders.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.BrowseButton)
        Me.GroupBox2.Controls.Add(Me.CompanyFileField)
        Me.GroupBox2.Controls.Add(Me.rbUserFile)
        Me.GroupBox2.Controls.Add(Me.rbCurrentFile)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 138)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(400, 67)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Export Destination"
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
        'CancelButton2
        '
        Me.CancelButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelButton2.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelButton2.Location = New System.Drawing.Point(257, 232)
        Me.CancelButton2.Name = "CancelButton2"
        Me.CancelButton2.Size = New System.Drawing.Size(75, 23)
        Me.CancelButton2.TabIndex = 9
        Me.CancelButton2.Text = "Cancel"
        Me.CancelButton2.UseVisualStyleBackColor = True
        '
        'OkayButton
        '
        Me.OkayButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OkayButton.Location = New System.Drawing.Point(338, 232)
        Me.OkayButton.Name = "OkayButton"
        Me.OkayButton.Size = New System.Drawing.Size(75, 23)
        Me.OkayButton.TabIndex = 8
        Me.OkayButton.Text = "OK"
        Me.OkayButton.UseVisualStyleBackColor = True
        '
        'ExportSelectionForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CancelButton2
        Me.ClientSize = New System.Drawing.Size(425, 267)
        Me.ControlBox = False
        Me.Controls.Add(Me.CancelButton2)
        Me.Controls.Add(Me.OkayButton)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ExportSelectionForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select Orders To Export"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents LastOrderField As System.Windows.Forms.TextBox
    Friend WithEvents SingleOrderField As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents EndRangeField As System.Windows.Forms.TextBox
    Friend WithEvents StartRangeField As System.Windows.Forms.TextBox
    Friend WithEvents rbOrderRange As System.Windows.Forms.RadioButton
    Friend WithEvents rbSingleOrder As System.Windows.Forms.RadioButton
    Friend WithEvents rbLatestOrders As System.Windows.Forms.RadioButton
    Friend WithEvents rbUserFile As System.Windows.Forms.RadioButton
    Friend WithEvents rbCurrentFile As System.Windows.Forms.RadioButton
    Friend WithEvents CancelButton2 As System.Windows.Forms.Button
    Friend WithEvents OkayButton As System.Windows.Forms.Button
    Friend WithEvents BrowseButton As System.Windows.Forms.Button
    Friend WithEvents CompanyFileField As System.Windows.Forms.TextBox
End Class
