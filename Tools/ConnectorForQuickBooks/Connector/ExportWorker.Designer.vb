<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExportWorker
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ExportWorker))
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar
        Me.lblCurrentOrder = New System.Windows.Forms.Label
        Me.lblExportCount = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblETA = New System.Windows.Forms.Label
        Me.ButtonCancel = New System.Windows.Forms.Button
        Me.BackgroundWorker = New System.ComponentModel.BackgroundWorker
        Me.SuspendLayout()
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(12, 75)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(552, 14)
        Me.ProgressBar1.TabIndex = 0
        '
        'lblCurrentOrder
        '
        Me.lblCurrentOrder.AutoSize = True
        Me.lblCurrentOrder.Location = New System.Drawing.Point(12, 9)
        Me.lblCurrentOrder.Name = "lblCurrentOrder"
        Me.lblCurrentOrder.Size = New System.Drawing.Size(60, 13)
        Me.lblCurrentOrder.TabIndex = 1
        Me.lblCurrentOrder.Text = "Exporting..."
        '
        'lblExportCount
        '
        Me.lblExportCount.AutoSize = True
        Me.lblExportCount.Location = New System.Drawing.Point(12, 40)
        Me.lblExportCount.Name = "lblExportCount"
        Me.lblExportCount.Size = New System.Drawing.Size(60, 13)
        Me.lblExportCount.TabIndex = 2
        Me.lblExportCount.Text = "Exporting..."
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 107)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(135, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Estimated Time Remaining:"
        '
        'lblETA
        '
        Me.lblETA.AutoSize = True
        Me.lblETA.Location = New System.Drawing.Point(153, 107)
        Me.lblETA.Name = "lblETA"
        Me.lblETA.Size = New System.Drawing.Size(28, 13)
        Me.lblETA.TabIndex = 4
        Me.lblETA.Text = "0:00"
        '
        'ButtonCancel
        '
        Me.ButtonCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonCancel.Location = New System.Drawing.Point(489, 102)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCancel.TabIndex = 5
        Me.ButtonCancel.Text = "Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'ExportWorker
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(576, 137)
        Me.ControlBox = False
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.lblETA)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblExportCount)
        Me.Controls.Add(Me.lblCurrentOrder)
        Me.Controls.Add(Me.ProgressBar1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ExportWorker"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Exporting..."
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ProgressBar1 As System.Windows.Forms.ProgressBar
    Friend WithEvents lblCurrentOrder As System.Windows.Forms.Label
    Friend WithEvents lblExportCount As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblETA As System.Windows.Forms.Label
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents BackgroundWorker As System.ComponentModel.BackgroundWorker
End Class
