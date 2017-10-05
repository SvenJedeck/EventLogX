﻿Imports System.Windows.Forms

Public Class ApiEventlogXWinform

    Public Enum ControlType
        LISTBOX
        COMBOBOX
    End Enum

    Public WithEvents ParentForm As Form

    Public WithEvents MyEventLogs As New EventLogsX

    Public WithEvents    Logs As LogLists
    Public WithEvents Sources As SourceLists

    Public WithEvents        BtnDelLog As Button
    Public WithEvents     BtnDelSource As Button
    Public WithEvents    BtnLogToOther As Button
    Public WithEvents      BtnLogtoOwn As Button
    Public WithEvents     BtnNewSource As Button
    Public WithEvents BtnSourceToOther As Button
    Public WithEvents   BtnSourceToOwn As Button




    Public Property LogName As String
    Public Property SourceName As String

    Public Sub New( parentForm As Form, _
                    logsOwn As Object, logsOther As Object, sourcesOwn As Object, sourcesOther As Object, _
                    selectedLog As TextBox, selectedSource As TextBox, ownLogName As TextBox, ownSourceName As TextBox)

        Call InstanceLogsAndSources(logsOwn, logsOther, sourcesOwn, sourcesOther)

        Logs.TxtLogSelectedText = selectedLog
        Logs.TxtLogWrittenText = ownLogName
    
        Sources.TxtSourceSelected = selectedSource
        Sources.TxtSourceWritten = ownSourceName

    End Sub

    Public Sub New( parentForm As Form, _
                    logsOwn As Object, logsOther As Object, sourcesOwn As Object, sourcesOther As Object, _
                    selectedLog As TextBox, selectedSource As TextBox)

        Call InstanceLogsAndSources(logsOwn, logsOther, sourcesOwn, sourcesOther)

        Logs.TxtLogSelectedText = selectedLog
        Logs.TxtLogWrittenText = selectedLog
    
        Sources.TxtSourceSelected = selectedSource
        Sources.TxtSourceWritten = selectedSource

    End Sub
            Private Sub InstanceLogsAndSources(logsOwn As Object, logsOther As Object, sourcesOwn As Object, sourcesOther As Object)

                On Error Resume Next

                        Logs        = New LogLists       (CType(  logsOwn, ListBox),      CType(    logsOther, ListBox),      Me)
                        Logs        = New LogLists       (CType(  logsOwn, ComboBox),     CType(    logsOther, ListBox),      Me)
                        Logs        = New LogLists       (CType(  logsOwn, ListBox),      CType(    logsOther, ComboBox),     Me)
                        Logs        = New LogLists       (CType(  logsOwn, ComboBox),     CType(    logsOther, ComboBox),     Me)

                        Sources     = New SourceLists    (CType(  sourcesOwn, ListBox) ,  CType(    sourcesOther, ListBox),   Me)
                        Sources     = New SourceLists    (CType(  sourcesOwn, ComboBox) , CType(    sourcesOther, ListBox),   Me)
                        Sources     = New SourceLists    (CType(  sourcesOwn, ListBox) ,  CType(    sourcesOther, ComboBox),  Me)
                        Sources     = New SourceLists    (CType(  sourcesOwn, ComboBox) , CType(    sourcesOther, ComboBox),  Me)

                On Error GoTo 0

            End Sub

    Public Sub SetControls()

        If Logs Is Nothing Then Exit Sub
        If Sources Is Nothing Then Exit Sub

        Dim MyActionState = MyEventLogs.Validate(Logs.CurrentLogName, Sources.CurrentSourceName)

        If Not          BtnDelLog Is Nothing Then BtnDelLog.Enabled         = MyActionState.LOG_DELETE
        If Not      BtnLogToOther Is Nothing Then BtnLogToOther.Enabled     = MyActionState.LOG_TO_OTHER
        If Not        BtnLogtoOwn Is Nothing Then BtnLogtoOwn.Enabled       = MyActionState.LOG_TO_OWN
        If Not       BtnNewSource Is Nothing Then BtnNewSource.Enabled      = MyActionState.SOURCE_CREATE
        If Not       BtnDelSource Is Nothing Then BtnDelSource.Enabled      = MyActionState.SOURCE_DELETE
        If Not   BtnSourceToOther Is Nothing Then BtnSourceToOther.Enabled  = MyActionState.SOURCE_TO_OTHER
        If Not     BtnSourceToOwn Is Nothing Then BtnSourceToOwn.Enabled    = MyActionState.SOURCE_TO_OWN

    End Sub

    Private Sub BtnNewSource_Click(sender As Object, e As EventArgs) Handles BtnNewSource.Click
        MyEventLogs.CreateSource(Logs.CurrentLogName, Sources.CurrentSourceName)
        Logs.SetDataSources
    End Sub

    Private Sub BtnDelLog_Click(sender As Object, e As EventArgs) Handles BtnDelLog.Click
        MyEventLogs.DeleteLog(Logs.SelectedLog.Name)
        Logs.SetDataSources
    End Sub

    Private Sub BtnLogToOther_Click(sender As Object, e As EventArgs) Handles BtnLogToOther.Click
        Logs.SelectedLog.SetOwner(EventLogsX.OwnerEnum.OTHER, True)
        Logs.SetDataSources
    End Sub

    Private Sub BtnLogtoOwn_Click(sender As Object, e As EventArgs) Handles BtnLogtoOwn.Click
        Logs.SelectedLog.SetOwner(EventLogsX.OwnerEnum.OWN, True)
        Logs.SetDataSources
    End Sub

    Private Sub BtnSourceToOther_Click(sender As Object, e As EventArgs) Handles BtnSourceToOther.Click
        Sources.SelectedSource.SetOwner(EventLogsX.OwnerEnum.OTHER)
        Sources.SetDataSources(Logs.SelectedLog.Name)
    End Sub

    Private Sub BtnSourceToOwn_Click(sender As Object, e As EventArgs) Handles BtnSourceToOwn.Click
        Sources.SelectedSource.SetOwner(EventLogsX.OwnerEnum.OWN)
        Sources.SetDataSources(Logs.SelectedLog.Name)
    End Sub

    Private Sub BtnDelSource_Click(sender As Object, e As EventArgs) Handles BtnDelSource.Click
        MyEventLogs.DeleteSource(Sources.SelectedSource.Name)
        Sources.SetDataSources(Logs.SelectedLog.Name)
    End Sub

    Public Class LogLists

            Public Parent As ApiEventlogXWinform

            Public SelectedLog As EventLogsX.EventLogX
            Public CurrentLogName As String

            Public WithEvents LstLogsOwn As ListBox
            Public WithEvents LstLogsOther As ListBox
            Public WithEvents CmbLogsOwn As ComboBox
            Public WithEvents CmbLogsOther As ComboBox

            Public WithEvents TxtLogSelectedText As TextBox
            Public WithEvents TxtLogWrittenText As TextBox

            Public Sub New(listOwn As ListBox, listOther As ListBox, prnt As ApiEventlogXWinform)

                LstLogsOwn = listOwn
                LstLogsOther = listOther
                Parent = prnt
                
                Call SetDataSources

            End Sub

            Public Sub New(listOwn As ListBox, comboOther As ComboBox, prnt As ApiEventlogXWinform)
                LstLogsOwn = listOwn
                CmbLogsOwn = comboOther
                Parent = prnt

                Call SetDataSources

            End Sub

            Public Sub New(comboOwn As ComboBox, comboOther As ComboBox, prnt As ApiEventlogXWinform)
                CmbLogsOwn = comboOwn
                CmbLogsOther = comboOther
                Parent = prnt

                Call SetDataSources

            End Sub

            Public Sub New(comboOwn As ComboBox, listOther As ListBox, prnt As ApiEventlogXWinform)
                CmbLogsOwn = comboOwn
                LstLogsOther = listOther
                Parent = prnt

                Call SetDataSources

            End Sub

            Protected Friend Sub SetDataSources()

                If Not LstLogsOwn Is Nothing Then
                    LstLogsOwn.DataSource = Parent.MyEventLogs.GetLogs(EventLogsX.OwnerEnum.OWN)
                    LstLogsOwn.ValueMember = "Log"
                End If

                If Not LstLogsOther Is Nothing Then
                    LstLogsOther.DataSource = Parent.MyEventLogs.GetLogs(EventLogsX.OwnerEnum.OTHER)
                    LstLogsOther.ValueMember = "Log"
                End If

                If Not CmbLogsOwn Is Nothing Then
                    CmbLogsOwn.DataSource = Parent.MyEventLogs.GetLogs(EventLogsX.OwnerEnum.OTHER)
                    CmbLogsOwn.ValueMember = "Log"
                End If

                If Not CmbLogsOther Is Nothing Then
                    CmbLogsOther.DataSource = Parent.MyEventLogs.GetLogs(EventLogsX.OwnerEnum.OTHER)
                    CmbLogsOther.ValueMember = "Log"
                End If

            End Sub

            Private Sub Logs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _
                                                                                    LstLogsOther.SelectedIndexChanged, _
                                                                                    LstLogsOwn.SelectedIndexChanged, _
                                                                                    CmbLogsOther.SelectedIndexChanged, _
                                                                                    CmbLogsOwn.SelectedIndexChanged

                Select Case sender.GetType.GetProperty("Name").ReflectedType.Name
                    Case "ListBox"
                        If Not Parent.Sources Is Nothing Then Parent.Sources.SetDataSources(CType(CType(sender, ListBox).SelectedItem, EventLogsX.EventLogX).Name)
                        SelectedLog = CType(CType(sender, ListBox).SelectedItem, EventLogsX.EventLogX)
                    Case "ComboBox"
                        If Not Parent.Sources Is Nothing Then Parent.Sources.SetDataSources(CType(CType(sender, ComboBox).SelectedItem, EventLogsX.EventLogX).Name)
                        SelectedLog = CType(CType(sender, ComboBox).SelectedItem, EventLogsX.EventLogX)
                End Select

                 If Not TxtLogWrittenText Is Nothing Then
                     TxtLogWrittenText.Text = ""
                     TxtLogSelectedText.Text = SelectedLog.Name
                    
                    Parent.Sources.TxtSourceSelected.Text = ""
                     Parent.Sources.TxtSourceWritten.Text = ""
                 End If
                         CurrentLogName = SelectedLog.Name

            End Sub

            Private Sub LogText_TextChanged(sender As Object, e As EventArgs) Handles _
                                                                              TxtLogSelectedText.TextChanged, _
                                                                              TxtLogWrittenText.TextChanged

                If Not CurrentLogName = CType(sender, TextBox).Text Then
                    CurrentLogName = CType(sender, TextBox).Text
                    Parent.SetControls
                End If

            End Sub


    End Class ' LogLists

        Public Class SourceLists

            Public Parent As ApiEventlogXWinform

            Public CurrentSourceName As String

            Public SelectedSource As EventLogsX.EventLogX.LogSource

            Private WithEvents LstSourcesOwn As ListBox
            Private WithEvents LstSourcesOther As ListBox
            Private WithEvents CmbSourcesOwn As ComboBox
            Private WithEvents CmbSourcesOther As ComboBox

            Public WithEvents TxtSourceSelected As TextBox
            Public WithEvents TxtSourceWritten As TextBox

            Public Sub New(listOwn As ListBox, listOther As ListBox, prnt As ApiEventlogXWinform)
                LstSourcesOwn = listOwn
                LstSourcesOther = listOther
                Parent = prnt
            End Sub

            Public Sub New(listOwn As ListBox, comboOther As ComboBox, prnt As ApiEventlogXWinform)
                LstSourcesOwn = listOwn
                CmbSourcesOther = comboOther 
                Parent = prnt
            End Sub

            Public Sub New(comboOwn As ComboBox, comboOther As ComboBox, prnt As ApiEventlogXWinform)
                CmbSourcesOwn = comboOwn
                CmbSourcesOther = comboOther
                Parent = prnt
            End Sub

            Public Sub New(comboOwn As ComboBox, listOther As ListBox, prnt As ApiEventlogXWinform)
                CmbSourcesOwn = comboOwn
                LstSourcesOther = listOther
                Parent = prnt
            End Sub

            Private Sub Sources_SelectedIndexChanged(sender As Object, e As EventArgs) Handles _
                                                                                    LstSourcesOther.SelectedIndexChanged, _
                                                                                    LstSourcesOwn.SelectedIndexChanged, _
                                                                                    CmbSourcesOther.SelectedIndexChanged, _
                                                                                    CmbSourcesOwn.SelectedIndexChanged
                Select Case sender.GetType.GetProperty("Name").ReflectedType.Name
                    Case "ListBox"
                        SelectedSource = CType(CType(sender, ListBox).SelectedItem, EventLogsX.EventLogX.LogSource)   
                    Case "ComboBox"
                        SelectedSource = CType(CType(sender, ComboBox).SelectedItem, EventLogsX.EventLogX.LogSource)
                End Select

                If Not TxtSourceWritten Is Nothing Then
                       TxtSourceWritten.Text = ""
                      TxtSourceSelected.Text = SelectedSource.Name
                End If

                CurrentSourceName = SelectedSource.Name

            End Sub

            Private Sub SourceText_TextChanged(sender As Object, e As EventArgs) Handles _
                                                                                 TxtSourceSelected.TextChanged, _
                                                                                 TxtSourceWritten.TextChanged
                If Not CurrentSourceName = CType(sender, TextBox).Text Then
                    CurrentSourceName = CType(sender, TextBox).Text
                    Parent.SetControls
                End If

            End Sub

            Public Sub SetDataSources(logName As String)

                If Not LstSourcesOwn Is Nothing Then
                    LstSourcesOwn.DataSource = Parent.MyEventLogs.GetSources(logName, EventLogsX.OwnerEnum.OWN)
                    LstSourcesOwn.ValueMember = "Name"
                End If

                If Not LstSourcesOther Is Nothing Then
                    LstSourcesOther.DataSource = Parent.MyEventLogs.GetSources(logName, EventLogsX.OwnerEnum.OTHER)
                    LstSourcesOther.ValueMember = "Name"
                End If

                If Not CmbSourcesOwn Is Nothing Then
                    CmbSourcesOwn.DataSource = Parent.MyEventLogs.GetSources(logName, EventLogsX.OwnerEnum.OWN)
                    CmbSourcesOwn.ValueMember = "Name"
                End If

                If Not CmbSourcesOther Is Nothing Then
                    CmbSourcesOther.DataSource = Parent.MyEventLogs.GetSources(logName, EventLogsX.OwnerEnum.OTHER)
                    CmbSourcesOther.ValueMember = "Name"
                End If

            End Sub


        End Class ' SourceLists

End Class