<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.ExitButton = New System.Windows.Forms.Button
        Me.OpenStoreButton = New System.Windows.Forms.Button
        Me.AboutButton = New System.Windows.Forms.Button
        Me.OptionsButton = New System.Windows.Forms.Button
        Me.SendItemsButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'ExitButton
        '
        Me.ExitButton.Location = New System.Drawing.Point(11, 75)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(122, 23)
        Me.ExitButton.TabIndex = 0
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'OpenStoreButton
        '
        Me.OpenStoreButton.FlatAppearance.BorderSize = 0
        Me.OpenStoreButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.OpenStoreButton.Image = CType(resources.GetObject("OpenStoreButton.Image"), System.Drawing.Image)
        Me.OpenStoreButton.Location = New System.Drawing.Point(170, 11)
        Me.OpenStoreButton.Name = "OpenStoreButton"
        Me.OpenStoreButton.Size = New System.Drawing.Size(232, 36)
        Me.OpenStoreButton.TabIndex = 1
        Me.OpenStoreButton.UseVisualStyleBackColor = True
        '
        'AboutButton
        '
        Me.AboutButton.Location = New System.Drawing.Point(11, 42)
        Me.AboutButton.Name = "AboutButton"
        Me.AboutButton.Size = New System.Drawing.Size(122, 23)
        Me.AboutButton.TabIndex = 3
        Me.AboutButton.Text = "About"
        Me.AboutButton.UseVisualStyleBackColor = True
        '
        'OptionsButton
        '
        Me.OptionsButton.Location = New System.Drawing.Point(11, 11)
        Me.OptionsButton.Name = "OptionsButton"
        Me.OptionsButton.Size = New System.Drawing.Size(122, 23)
        Me.OptionsButton.TabIndex = 4
        Me.OptionsButton.Text = "Options"
        Me.OptionsButton.UseVisualStyleBackColor = True
        '
        'SendItemsButton
        '
        Me.SendItemsButton.FlatAppearance.BorderSize = 0
        Me.SendItemsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.SendItemsButton.Image = CType(resources.GetObject("SendItemsButton.Image"), System.Drawing.Image)
        Me.SendItemsButton.Location = New System.Drawing.Point(170, 62)
        Me.SendItemsButton.Name = "SendItemsButton"
        Me.SendItemsButton.Size = New System.Drawing.Size(232, 36)
        Me.SendItemsButton.TabIndex = 6
        Me.SendItemsButton.UseVisualStyleBackColor = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(433, 115)
        Me.Controls.Add(Me.SendItemsButton)
        Me.Controls.Add(Me.OptionsButton)
        Me.Controls.Add(Me.AboutButton)
        Me.Controls.Add(Me.OpenStoreButton)
        Me.Controls.Add(Me.ExitButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainForm"
        Me.Text = "BV Connector for QuickBooks"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ExitButton As System.Windows.Forms.Button
    Friend WithEvents OpenStoreButton As System.Windows.Forms.Button
    Friend WithEvents AboutButton As System.Windows.Forms.Button
    Friend WithEvents OptionsButton As System.Windows.Forms.Button
    Friend WithEvents SendItemsButton As System.Windows.Forms.Button
End Class
