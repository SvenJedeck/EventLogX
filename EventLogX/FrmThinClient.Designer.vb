Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmThinClient
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.LstLogsOther = New System.Windows.Forms.ListBox()
        Me.TxtLogName = New System.Windows.Forms.TextBox()
        Me.TxtSourceName = New System.Windows.Forms.TextBox()
        Me.BtnNewSource = New System.Windows.Forms.Button()
        Me.TipLogName = New System.Windows.Forms.ToolTip(Me.components)
        Me.TipSourceName = New System.Windows.Forms.ToolTip(Me.components)
        Me.LstSourcesOther = New System.Windows.Forms.ListBox()
        Me.LstLogsOwn = New System.Windows.Forms.ListBox()
        Me.BtnDelSource = New System.Windows.Forms.Button()
        Me.LstSourcesOwn = New System.Windows.Forms.ListBox()
        Me.BtnDelLog = New System.Windows.Forms.Button()
        Me.BtnLogToOther = New System.Windows.Forms.Button()
        Me.BtnLogtoOwn = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.BtnSourceToOwn = New System.Windows.Forms.Button()
        Me.BtnSourceToOther = New System.Windows.Forms.Button()
        Me.SuspendLayout
        '
        'LstLogsOther
        '
        Me.LstLogsOther.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.LstLogsOther.FormattingEnabled = true
        Me.LstLogsOther.ItemHeight = 16
        Me.LstLogsOther.Location = New System.Drawing.Point(16, 64)
        Me.LstLogsOther.Margin = New System.Windows.Forms.Padding(4)
        Me.LstLogsOther.Name = "LstLogsOther"
        Me.LstLogsOther.Size = New System.Drawing.Size(408, 196)
        Me.LstLogsOther.TabIndex = 0
        '
        'TxtLogName
        '
        Me.TxtLogName.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.TxtLogName.Location = New System.Drawing.Point(904, 64)
        Me.TxtLogName.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtLogName.Name = "TxtLogName"
        Me.TxtLogName.Size = New System.Drawing.Size(532, 23)
        Me.TxtLogName.TabIndex = 1
        '
        'TxtSourceName
        '
        Me.TxtSourceName.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.TxtSourceName.Location = New System.Drawing.Point(904, 160)
        Me.TxtSourceName.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtSourceName.Name = "TxtSourceName"
        Me.TxtSourceName.Size = New System.Drawing.Size(404, 23)
        Me.TxtSourceName.TabIndex = 2
        '
        'BtnNewSource
        '
        Me.BtnNewSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.BtnNewSource.Location = New System.Drawing.Point(904, 184)
        Me.BtnNewSource.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnNewSource.Name = "BtnNewSource"
        Me.BtnNewSource.Size = New System.Drawing.Size(117, 40)
        Me.BtnNewSource.TabIndex = 3
        Me.BtnNewSource.Text = "NEW SCOURCE AND LOG"
        Me.BtnNewSource.UseVisualStyleBackColor = true
        '
        'TipLogName
        '
        Me.TipLogName.AutoPopDelay = 10000
        Me.TipLogName.InitialDelay = 150
        Me.TipLogName.ReshowDelay = 50
        '
        'LstSourcesOther
        '
        Me.LstSourcesOther.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.LstSourcesOther.FormattingEnabled = true
        Me.LstSourcesOther.ItemHeight = 16
        Me.LstSourcesOther.Location = New System.Drawing.Point(432, 64)
        Me.LstSourcesOther.Margin = New System.Windows.Forms.Padding(4)
        Me.LstSourcesOther.Name = "LstSourcesOther"
        Me.LstSourcesOther.Size = New System.Drawing.Size(464, 532)
        Me.LstSourcesOther.TabIndex = 4
        '
        'LstLogsOwn
        '
        Me.LstLogsOwn.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.LstLogsOwn.FormattingEnabled = true
        Me.LstLogsOwn.ItemHeight = 16
        Me.LstLogsOwn.Location = New System.Drawing.Point(16, 320)
        Me.LstLogsOwn.Margin = New System.Windows.Forms.Padding(4)
        Me.LstLogsOwn.Name = "LstLogsOwn"
        Me.LstLogsOwn.Size = New System.Drawing.Size(408, 100)
        Me.LstLogsOwn.TabIndex = 5
        '
        'BtnDelSource
        '
        Me.BtnDelSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.BtnDelSource.Location = New System.Drawing.Point(1024, 184)
        Me.BtnDelSource.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnDelSource.Name = "BtnDelSource"
        Me.BtnDelSource.Size = New System.Drawing.Size(117, 40)
        Me.BtnDelSource.TabIndex = 6
        Me.BtnDelSource.Text = "DELETE SOURCE"
        Me.BtnDelSource.UseVisualStyleBackColor = true
        '
        'LstSourcesOwn
        '
        Me.LstSourcesOwn.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.LstSourcesOwn.FormattingEnabled = true
        Me.LstSourcesOwn.ItemHeight = 16
        Me.LstSourcesOwn.Location = New System.Drawing.Point(16, 456)
        Me.LstSourcesOwn.Margin = New System.Windows.Forms.Padding(4)
        Me.LstSourcesOwn.Name = "LstSourcesOwn"
        Me.LstSourcesOwn.Size = New System.Drawing.Size(328, 132)
        Me.LstSourcesOwn.TabIndex = 7
        '
        'BtnDelLog
        '
        Me.BtnDelLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.BtnDelLog.Location = New System.Drawing.Point(904, 88)
        Me.BtnDelLog.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnDelLog.Name = "BtnDelLog"
        Me.BtnDelLog.Size = New System.Drawing.Size(117, 40)
        Me.BtnDelLog.TabIndex = 8
        Me.BtnDelLog.Text = "DELETE LOG AND SOURCES"
        Me.BtnDelLog.UseVisualStyleBackColor = true
        '
        'BtnLogToOther
        '
        Me.BtnLogToOther.Image = Global.EventLogX.My.Resources.Resources.arrow_full_up_128
        Me.BtnLogToOther.Location = New System.Drawing.Point(304, 264)
        Me.BtnLogToOther.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnLogToOther.Name = "BtnLogToOther"
        Me.BtnLogToOther.Size = New System.Drawing.Size(117, 40)
        Me.BtnLogToOther.TabIndex = 10
        Me.BtnLogToOther.UseVisualStyleBackColor = true
        '
        'BtnLogtoOwn
        '
        Me.BtnLogtoOwn.Image = Global.EventLogX.My.Resources.Resources.arrow_full_down_128
        Me.BtnLogtoOwn.Location = New System.Drawing.Point(184, 264)
        Me.BtnLogtoOwn.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnLogtoOwn.Name = "BtnLogtoOwn"
        Me.BtnLogtoOwn.Size = New System.Drawing.Size(117, 40)
        Me.BtnLogtoOwn.TabIndex = 9
        Me.BtnLogtoOwn.UseVisualStyleBackColor = true
        '
        'Label1
        '
        Me.Label1.AutoSize = true
        Me.Label1.Location = New System.Drawing.Point(16, 40)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 16)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Other Logs"
        '
        'Label2
        '
        Me.Label2.AutoSize = true
        Me.Label2.Location = New System.Drawing.Point(432, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(103, 16)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Other Sources"
        '
        'Label3
        '
        Me.Label3.AutoSize = true
        Me.Label3.Location = New System.Drawing.Point(16, 296)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 16)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Own Logs"
        '
        'Label4
        '
        Me.Label4.AutoSize = true
        Me.Label4.Location = New System.Drawing.Point(16, 432)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(95, 16)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Own Sources"
        '
        'Label5
        '
        Me.Label5.AutoSize = true
        Me.Label5.Location = New System.Drawing.Point(904, 40)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(524, 16)
        Me.Label5.TabIndex = 17
        Me.Label5.Text = "Logname // Typ name for new log, select existing log to delete log && sources."
        '
        'Label6
        '
        Me.Label6.AutoSize = true
        Me.Label6.Location = New System.Drawing.Point(904, 136)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(552, 16)
        Me.Label6.TabIndex = 18
        Me.Label6.Text = "Sourcename // Typ name for new source, select existing source to delete source."
        '
        'Label7
        '
        Me.Label7.AutoSize = true
        Me.Label7.Location = New System.Drawing.Point(1024, 96)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(341, 16)
        Me.Label7.TabIndex = 19
        Me.Label7.Text = "Note: Only OWN logs and sources can be deleted."
        '
        'Label8
        '
        Me.Label8.AutoSize = true
        Me.Label8.Location = New System.Drawing.Point(1024, 112)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(432, 16)
        Me.Label8.TabIndex = 20
        Me.Label8.Text = "         That's why, all soucres of a log must be in OWN sources."
        '
        'BtnSourceToOwn
        '
        Me.BtnSourceToOwn.Image = Global.EventLogX.My.Resources.Resources.arrow_full_left_128
        Me.BtnSourceToOwn.Location = New System.Drawing.Point(352, 504)
        Me.BtnSourceToOwn.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnSourceToOwn.Name = "BtnSourceToOwn"
        Me.BtnSourceToOwn.Size = New System.Drawing.Size(61, 40)
        Me.BtnSourceToOwn.TabIndex = 23
        Me.BtnSourceToOwn.UseVisualStyleBackColor = true
        '
        'BtnSourceToOther
        '
        Me.BtnSourceToOther.Image = Global.EventLogX.My.Resources.Resources.arrow_full_right_128
        Me.BtnSourceToOther.Location = New System.Drawing.Point(352, 456)
        Me.BtnSourceToOther.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnSourceToOther.Name = "BtnSourceToOther"
        Me.BtnSourceToOther.Size = New System.Drawing.Size(61, 40)
        Me.BtnSourceToOther.TabIndex = 22
        Me.BtnSourceToOther.UseVisualStyleBackColor = true
        '
        'FrmThinClient
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8!, 16!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = true
        Me.ClientSize = New System.Drawing.Size(1467, 617)
        Me.Controls.Add(Me.BtnSourceToOwn)
        Me.Controls.Add(Me.BtnSourceToOther)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BtnLogToOther)
        Me.Controls.Add(Me.BtnLogtoOwn)
        Me.Controls.Add(Me.BtnDelLog)
        Me.Controls.Add(Me.LstSourcesOwn)
        Me.Controls.Add(Me.BtnDelSource)
        Me.Controls.Add(Me.LstLogsOwn)
        Me.Controls.Add(Me.LstSourcesOther)
        Me.Controls.Add(Me.BtnNewSource)
        Me.Controls.Add(Me.TxtSourceName)
        Me.Controls.Add(Me.TxtLogName)
        Me.Controls.Add(Me.LstLogsOther)
        Me.Font = New System.Drawing.Font("Verdana", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "FrmThinClient"
        Me.Text = "EventLog Wizard"
        Me.ResumeLayout(false)
        Me.PerformLayout

End Sub

    Friend WithEvents LstLogsOther As ListBox
    Friend WithEvents TxtLogName As TextBox
    Friend WithEvents TxtSourceName As TextBox
    Friend WithEvents BtnNewSource As Button
    Friend WithEvents ListBox2 As ListBox
    Friend WithEvents TipLogName As ToolTip
    Friend WithEvents TipSourceName As ToolTip
    Friend WithEvents LstSourcesOther As ListBox
    Friend WithEvents LstLogsOwn As ListBox
    Friend WithEvents BtnDelSource As Button
    Friend WithEvents LstSourcesOwn As ListBox
    Friend WithEvents BtnDelLog As Button
    Friend WithEvents BtnLogtoOwn As Button
    Friend WithEvents BtnLogToOther As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents BtnSourceToOwn As Button
    Friend WithEvents BtnSourceToOther As Button
End Class
