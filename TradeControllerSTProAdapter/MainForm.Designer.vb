<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainFrom
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainFrom))
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.EndpointLabel = New System.Windows.Forms.Label()
        Me.EndpointInput = New System.Windows.Forms.TextBox()
        Me.SocketConnectButton = New System.Windows.Forms.Button()
        Me.StatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip
        '
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 54)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(497, 22)
        Me.StatusStrip.SizingGrip = False
        Me.StatusStrip.TabIndex = 0
        Me.StatusStrip.Text = "StatusStrip"
        '
        'StatusLabel
        '
        Me.StatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusLabel.ForeColor = System.Drawing.Color.Red
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(83, 17)
        Me.StatusLabel.Text = "Disconnected"
        '
        'EndpointLabel
        '
        Me.EndpointLabel.AutoSize = True
        Me.EndpointLabel.Location = New System.Drawing.Point(12, 17)
        Me.EndpointLabel.Name = "EndpointLabel"
        Me.EndpointLabel.Size = New System.Drawing.Size(86, 13)
        Me.EndpointLabel.TabIndex = 1
        Me.EndpointLabel.Text = "Console Address"
        '
        'EndpointInput
        '
        Me.EndpointInput.Location = New System.Drawing.Point(104, 14)
        Me.EndpointInput.Name = "EndpointInput"
        Me.EndpointInput.Size = New System.Drawing.Size(300, 20)
        Me.EndpointInput.TabIndex = 2
        '
        'SocketConnectButton
        '
        Me.SocketConnectButton.Location = New System.Drawing.Point(410, 12)
        Me.SocketConnectButton.Name = "SocketConnectButton"
        Me.SocketConnectButton.Size = New System.Drawing.Size(75, 23)
        Me.SocketConnectButton.TabIndex = 3
        Me.SocketConnectButton.Text = "Connect"
        Me.SocketConnectButton.UseVisualStyleBackColor = True
        '
        'MainFrom
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(497, 76)
        Me.Controls.Add(Me.SocketConnectButton)
        Me.Controls.Add(Me.EndpointInput)
        Me.Controls.Add(Me.EndpointLabel)
        Me.Controls.Add(Me.StatusStrip)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainFrom"
        Me.Text = "Trade Console"
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents StatusStrip As StatusStrip
    Friend WithEvents StatusLabel As ToolStripStatusLabel
    Friend WithEvents EndpointLabel As Label
    Friend WithEvents EndpointInput As TextBox
    Friend WithEvents SocketConnectButton As Button
End Class
