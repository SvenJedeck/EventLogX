Imports  Microsoft.Win32
Imports System.Text.RegularExpressions

''' <summary>
''' 
''' </summary>
Public Class EventLogsX

#Region "Declaration"

    Public Event Dummy()

    ''' <summary>
    ''' Declare if it is selfcreated log/source
    ''' </summary>
    Public Enum OwnerEnum
        OWN 
        OTHER
    End Enum


    Public Structure ActionTypes
        Public LOG_DELETE As Boolean
        Public LOG_TO_OTHER As Boolean
        Public LOG_TO_OWN As Boolean
        Public SOURCE_CREATE As Boolean
        Public SOURCE_DELETE As Boolean
        Public SOURCE_TO_OTHER As Boolean
        Public SOURCE_TO_OWN As Boolean
    End Structure
        Public ReadOnly Property ActionState As ActionTypes

    Public Structure StateTypeLog
        Public       CHARSET_FITS As Boolean
        Public     CHILDS_ARE_OWN As Boolean
        Public             EXISTS As Boolean
        Public  SHORT_NAME_EXISTS As Boolean
        Public             IS_OWN As Boolean
        Public       IS_PROTECTED As Boolean
        Public IS_PROTECTED_FULLY As Boolean
    End Structure 
        Public ReadOnly Property LogState As StateTypeLog

    Public Structure StateTypeSource
        Public       CHARSET_FITS As Boolean
        Public             EXISTS As Boolean
        Public IS_STANDARD_SOURCE As Boolean
        Public             IS_OWN As Boolean
        Public       IS_PROTECTED As Boolean
        Public REGKEY_LENGTH_FITS As Boolean
    End Structure 
        Public ReadOnly Property SourceState As StateTypeSource

    Public Structure GeneralState
        Public LogState As StateTypeLog
        Public SourceState As StateTypeSource
    End Structure

    Public Enum Actions
        LOG_DELETE
        LOG_ONLY_TO_OWN
        LOG_ONLY_TO_OTHER
        LOG_ALL_TO_OWN
        LOG_ALL_TO_OTHER
        SOURCE_CREATE
        SOURCE_DELETE
        SOUCRE_TO_OWN
        SOURCE_TO_OTHER
    End Enum

    Private Const KEYLENGT_RESERVE As Integer = 25

    ''' <summary>
    ''' Represents logs which are completly protected. No log or source can be deleted or added.
    ''' </summary>
    Private ThisProtectedLogsFull As New List(Of String)({"Security", "System"})

    ''' <summary>
    ''' Represents logs which logs are protected. None of these logs can be deleted, but sources can be deleted and added.
    ''' </summary>
    Private ThisProtectedLogs As New List(Of String)({"Security", "System", "Application", "HardwareEvents", "Internet Explorer", "Key Management Service", "OAlerts", "Windows Azure", "Windows PowerShell"})

    ''' <summary>
    ''' Represents the EventLog Root Key as <see cref="RegistryKey"/>
    ''' </summary>
    ''' <returns></returns>
    Protected ReadOnly Property RegRootKey As RegistryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\CurrentControlSet\Services\Eventlog\", True)

    ''' <summary>
    ''' List of all EventLogX at the local machine. Note: Logs have unique name per machine. But in MSDN it is said, that a log cant have same name as source. As tested (Win10) this is wrong. They can.
    ''' </summary>
    ''' <returns><see cref=" List(Of EventLogX)"/> </returns>
    Protected Property LogList As New List(Of EventLogX)

    ''' <summary>
    ''' List of all EventLogX.LogSource at the local machine. Note: Sources have unique name per machine. But in MSDN it is said, that a log cant have same name as source. As tested (Win10) this is wrong. They can.
    ''' </summary>
    ''' <returns><see cref="List(Of EventLogX.LogSource)"/></returns>
    Protected Property SourceList As New List(Of EventLogX.LogSource)

#End Region ' Declaration

    Public Sub New()

        Call InitReg()

        LogList = EventLogX.GetEventLogs(Me)

    End Sub ' Sub New

#Region "Public Code"

    ''' <summary>
    ''' Get all EventLogs of local machine
    ''' </summary>
    ''' <returns></returns>
    Public Function GetLogs() As List(Of EventLogX)
        Return LogList
    End Function ' GetLogs

    ''' <summary>
    ''' Get EventLogs filtered by owner of local machine 
    ''' </summary>
    ''' <param name="owner"></param>
    ''' <returns></returns>
    Public Function GetLogs(owner As OwnerEnum) As List(Of EventLogX)
        Return LogList.FindAll(Function(L) L.Owner = owner)
    End Function ' GetLogs

    ''' <summary>
    ''' Get single EventLog filtered by name. 
    ''' </summary>
    ''' <param name="logName"></param>
    ''' <returns></returns>
    Public Function GetLogs(logName As String) As EventLogX
        Return LogList.Find(Function(L) L.Name.ToLower = logName.ToLower)
    End Function ' GetLogs

    Public Function LogExists(logName As String) As Boolean
        Return LogList.Exists(Function (L) L.Name.ToLower = logName.ToLower)
    End Function ' GetLogs

    Public Function LogExists(evtLog As EventLog) As Boolean
        Return LogList.Exists(Function (L) L.Log = evtLog.Log)
    End Function ' GetLogs

    Public Function LogExists(evtLogX As EventLogX) As Boolean
        Return LogList.Exists(Function (L) L Is evtLogX)
    End Function ' GetLogs


    ''' <summary>
    '''  Get all EventLog Sources of local machine 
    ''' </summary>
    ''' <returns></returns>
    Public Function GetSources() As List(Of EventLogX.LogSource)
        Return SourceList
    End Function ' GetSources

    ''' <summary>
    ''' Get EventLog filtered by EventLogName of local machine 
    ''' </summary>
    ''' <param name="logName"></param>
    ''' <returns><see cref="List(Of EventLogX.LogSource)"/></returns>
    Public Function GetSources(logName As String) As List(Of EventLogX.LogSource)
        Return SourceList.FindAll(Function(S) S.Parent.Name.ToLower = logName.ToLower)
    End Function ' GetSources

    ''' <summary>
    ''' Get EventLog filtered by EventLogName and OWNER of local machine 
    ''' </summary>
    ''' <param name="logName"></param>
    ''' <param name="owner"></param>
    ''' <returns></returns>
    Public Function GetSources(logName As String, owner As OwnerEnum) As List(Of EventLogX.LogSource)
        Return SourceList.FindAll(Function(S) S.Parent.Name.ToLower = logName.ToLower).FindAll(Function(S) S.Owner = owner)
    End Function ' GetSources

    ''' <summary>
    ''' Each new log in LogList, adds his child sources in this way. Perhaps better in LOG class
    ''' </summary>
    ''' <param name="AddingSourceList"></param>
    Protected Sub AddSource(AddingSourceList As List(Of EventLogX.LogSource))
        sourceList.AddRange(AddingSourceList)
    End Sub ' AddSource

    ''' <summary>
    ''' ReRead Logs and Sources from machine.
    ''' </summary>
    Public Sub Refresh()
        SourceList.Clear : LogList = EventLogX.GetEventLogs(Me)
    End Sub

    ''' <summary>
    ''' If EventLog doesn't exists, it is created. Thats why, here is no function "CreateLog". Returns: "NOTHING" on OS error and FALSE if other error. On FALSE error, EventLogsX should be new instanced.
    ''' </summary>
    ''' <param name="logName"></param>
    ''' <param name="sourceName"></param>
    ''' <remarks>Detailed own event and error event should be raised in future.</remarks>
    ''' <returns>Returns: "NOTHING" on OS error and FALSE if other error. On FALSE error, EventLogsX should be new instanced. <see cref="Boolean"/></returns>
    Public Function CreateSource(logName As String, sourceName As String) As Boolean

        Dim MyErrorCount As Short   = -1
        Dim     MyReturn As Boolean = False

        Call Validate(logName, sourceName)

        If Not ActionState.SOURCE_CREATE Then Return Nothing

        Try

            EventLog.CreateEventSource(sourceName, logName)

        ReTryWriteEntry: 

            MyErrorCount =+ 1

        ' OS needs a while until log is created and can be used. 
            Threading.Thread.Sleep(1000)

            Dim MyEvLog As New EventLog(logName)

             ' Try to use and init Log, if error then it goes back to jump point
            MyEvLog.WriteEntry(sourceName, "Log init entry user defined source.", EventLogEntryType.Information, 666, 42)
            MyEvLog.WriteEntry(logName, "Log init entry user standard source.", EventLogEntryType.Information, 666, 42)

        Catch ex As Exception

            Debug.Print ("CreateSource: " & ex.Message)

            ' EventLog.CreateEventSource(logName, standardSource) throw error
            If MyErrorCount = -1 Then Return Nothing 

            ' EventLog.CreateEventSource(logName, standardSource) was OK, but couldn't access at the moment. Try at least 30 sec.
            If MyErrorCount >= 30 Then Return Nothing Else GoTo ReTryWriteEntry 

        End Try

        Try

            Dim MyRegListOfOwnSources = CType(RegRootKey.GetValue("OwnSources"), Array).Cast(Of String)().ToList()

            ' If successfully written in EventLog, then add (only new) Log to "OwnLogs" in Registry
            If Not LogState.EXISTS Then

                Dim MyRegListOfOwnLogs = CType(RegRootKey.GetValue("OwnLogs"), Array).Cast(Of String)().ToList()
                MyRegListOfOwnLogs.Add(logName)
                RegRootKey.SetValue("OwnLogs", MyRegListOfOwnLogs.ToArray)

                ' Add standard source to "OwnSources" in Registry
                MyRegListOfOwnSources.Add(logName)

            End If

                ' Add new Source "OwnSources" in Registry
                MyRegListOfOwnSources.Add(sourceName)
                RegRootKey.SetValue("OwnSources", MyRegListOfOwnSources.ToArray)

            ' Re-Read Logs and Sources
            Call Refresh

            MyReturn = True

        Catch ex As Exception

            MyReturn = False

        End Try

        Return MyReturn

    End Function  ' Function CreateSource


    ''' <summary>
    ''' Delete logs and sources. Checks before, if all sources are OWN.
    ''' </summary>
    ''' <param name="logName"></param>
    ''' <returns></returns>
    Public Function DeleteLog(logName As String) As Boolean

        ' If there is any child source not in OWN, then Validate is FALSE
        If Not Validate(logName, "").LOG_DELETE Then Return False

        Try

            For Each S In LogList.Find(Function (L) L.Name.ToLower = logName.ToLower).ChildSources
                DeleteSource(S.Name, True)
            Next

            EventLog.Delete(logName)

          ' OS needs a little while until log is deleted. 
            Threading.Thread.Sleep(200)

            ' Remove log from Reg >> must be an own log, cus else Validate(logName, "").LOG_DELETE would be FALSE
            Dim MyRegListOfOwnLogs = CType(RegRootKey.GetValue("OwnLogs"), Array).Cast(Of String)().ToList()
            MyRegListOfOwnLogs.Remove(logName)
            RegRootKey.SetValue("OwnLogs", MyRegListOfOwnLogs.ToArray)

            ' Remove last/standarad source from Reg >> can't be deleted before log is deleted. It is automatic deleted, if log is deleted. 
            Dim MyRegListOfOwnSources = CType(RegRootKey.GetValue("OwnSources"), Array).Cast(Of String)().ToList()
                Try
                    MyRegListOfOwnSources.Remove(logName)
                Catch ex As Exception
                End Try
            RegRootKey.SetValue("OwnSources", MyRegListOfOwnSources.ToArray)

            ' Re-Read Logs and Sources
            Call Refresh

        Catch ex As Exception

            ' Re-Read Logs and Sources
            Call Refresh
            Return False

        End Try

        Return True

    End Function


    ''' <summary>
    ''' Delete 1 single source, remove from registry and re-read logs/sources.
    ''' </summary>
    ''' <param name="sourceName"></param>
    ''' <returns></returns>
    Public Function DeleteSource(sourceName As String) As Boolean

        If Not Validate("", sourceName).SOURCE_DELETE Then Return False

        Try
            EventLog.DeleteEventSource(sourceName)

            ' Remove source from Reg >> must be an own source, cus else Validate("", sourceName).SOURCE_DELETE would be FALSE
            Dim MyRegListOfOwnSources = CType(RegRootKey.GetValue("OwnSources"), Array).Cast(Of String)().ToList()
            MyRegListOfOwnSources.Remove(sourceName)
            RegRootKey.SetValue("OwnSources", MyRegListOfOwnSources.ToArray)
        
            ' Re-Read Logs and Sources
            Call Refresh

        Catch ex As Exception
            Return False
        End Try

        Return True

    End Function


    ''' <summary>
    ''' Used for deleting sources if parent log is deleted. Implemented because of performance reasons >> No re-read logs/sources is done here. The 'withChilds' is only for signature.
    ''' </summary>
    ''' <param name="sourceName"></param>
    ''' <param name="withChilds"></param>
    ''' <returns></returns>
    Private Function DeleteSource(sourceName As String, withChilds As Boolean) As Boolean

        If Not Validate("", sourceName).SOURCE_DELETE Then Return False

        Try
            EventLog.DeleteEventSource(sourceName)

            ' Remove source from Reg >> must be an own source, cus else Validate("", sourceName).SOURCE_DELETE would be FALSE
            Dim MyRegListOfOwnSources = CType(RegRootKey.GetValue("OwnSources"), Array).Cast(Of String)().ToList()
            MyRegListOfOwnSources.Remove(sourceName)
            RegRootKey.SetValue("OwnSources", MyRegListOfOwnSources.ToArray)

        Catch ex As Exception
            Return False
        End Try

        Return True

    End Function


    ''' <summary>
    ''' Vaildate assumption for every action type. Validation uses <see cref="GeneralState"/> to interpret information. 
    ''' </summary>
    ''' <param name="logName"></param>
    ''' <param name="sourceName"></param>
    ''' <returns></returns>
    Public Function Validate(logName As String, sourceName As String) As ActionTypes

        Call SetStates(logName, sourceName)

                Validate.LOG_DELETE =     ValidateLogDelete(logName, sourceName)
              Validate.LOG_TO_OTHER =    ValidateLogToOther(logName, sourceName) 
                Validate.LOG_TO_OWN =      ValidateLogToOwn(logName, sourceName) 
             Validate.SOURCE_CREATE =  ValidateSourceCreate(logName, sourceName) 
             Validate.SOURCE_DELETE =  ValidateSourceDelete(logName, sourceName) 
           Validate.SOURCE_TO_OTHER = ValidateSourceToOther(logName, sourceName) 
             Validate.SOURCE_TO_OWN =   ValidateSourceToOwn(logName, sourceName) 

        _ActionState = Validate

    End Function
#Region "Validate help functions"
            Private Function ValidateLogDelete(logName As String, sourceName As String) As Boolean

                If Not          LogState.EXISTS Then Return False
                If Not  LogState.CHILDS_ARE_OWN Then Return False
                If Not          LogState.IS_OWN Then Return False
                If        LogState.IS_PROTECTED Then Return False

                Return True        

            End Function

            Private Function ValidateLogToOther(logName As String, sourceName As String) As Boolean

                If Not LogState.EXISTS Then Return False
                If Not LogState.IS_OWN Then Return False

                Return True        

            End Function

            Private Function ValidateLogToOwn(logName As String, sourceName As String) As Boolean

                If Not       LogState.EXISTS Then Return False
                If     LogState.IS_PROTECTED Then Return False
                If           LogState.IS_OWN Then Return False
                
                Return True 

            End Function

            Private Function ValidateSourceCreate(logName As String, sourceName As String) As Boolean

                If       SourceState.EXISTS             Then Return False
                If Not   SourceState.CHARSET_FITS       Then Return False
                If Not   SourceState.REGKEY_LENGTH_FITS Then Return False

                Select Case LogState.EXISTS
                    Case True
                                If  LogState.IS_PROTECTED_FULLY Then Return False
                    Case False
                                If Not LogState.CHARSET_FITS Then Return False
                             If   LogState.SHORT_NAME_EXISTS Then Return False
                End Select

                Return True

            End Function

            Private Function ValidateSourceDelete(logName As String, sourceName As String) As Boolean

                If Not  SourceState.EXISTS              Then Return False
                If Not  SourceState.IS_OWN              Then Return False
                If      SourceState.IS_STANDARD_SOURCE  Then Return False
                If      SourceState.IS_PROTECTED        Then Return False

                Return True

            End Function

            Private Function ValidateSourceToOther(logName As String, sourceName As String) As Boolean

                If Not SourceState.EXISTS Then Return False
                If Not SourceState.IS_OWN Then Return False

                Return True

            End Function

            Private Function ValidateSourceToOwn(logName As String, sourceName As String) As Boolean

                If Not  SourceState.EXISTS          Then Return False
                If      SourceState.IS_OWN          Then Return False
                If      SourceState.IS_PROTECTED    Then Return False
                
                Return True

            End Function
#End Region ' "Validate help functions"


    ''' <summary>
    ''' Sets the information bits about every log and source state. Eg. log/source EXISTS, CHARSET_FITS, etc. Look at <see cref="StateTypeLog"/> and <seealso cref="StateTypeSource"/>.
    ''' </summary>
    ''' <param name="logName"></param>
    ''' <param name="sourceName"></param>
    ''' <returns></returns>
    Protected Friend Function SetStates(logName As String, sourceName As String) As GeneralState

        SetStates.LogState = SetLogState(logName)
        SetStates.SourceState = SetSourceState(logName, sourceName)

        Return SetStates

    End Function
#Region "Set States help functions"

    ''' <summary>
    ''' Sets info bits for given LOG.
    ''' </summary>
    Private Function SetLogState(logName As String) As StateTypeLog

                    If LogList.Exists(Function(L) L.Name.ToLower = logName.ToLower) AndAlso _
                            EventLog.Exists(logName) Then _
        _LogState.EXISTS = True Else _LogState.EXISTS = False

                    If LogList.Exists(Function(L) L.ShortName.ToLower = Left(logName, 8).ToLower) Then
        _LogState.SHORT_NAME_EXISTS = True 
                    Else 
        _LogState.SHORT_NAME_EXISTS = False
                    End If

        _LogState.CHARSET_FITS = New Regex("^[a-zA-Z]{1,1}[a-zA-Z_0-9_\.\s]{4,}$").IsMatch(logName)

                     If LogList.Exists(Function(L) L.Name.ToLower = logName.ToLower And L.Owner = OwnerEnum.OWN) Then _
        _LogState.IS_OWN = True Else _LogState.IS_OWN = False

                    If ThisProtectedLogs.Exists(Function(Str) Str.ToLower = logName.ToLower) Then _
        _Logstate.IS_PROTECTED = True Else _LogState.IS_PROTECTED = False

                    If ThisProtectedLogsFull.Exists(Function(Str) Str.ToLower = logName.ToLower) Then _
        _Logstate.IS_PROTECTED_FULLY = True Else _LogState.IS_PROTECTED_FULLY = False

                    If SourceList.Exists(Function (S) S.Parent.Name.ToLower = logName.ToLower And S.Owner = OwnerEnum.OTHER) AndAlso LogState.EXISTS Then _
        _Logstate.CHILDS_ARE_OWN = False Else _Logstate.CHILDS_ARE_OWN = True

        Return LogState

    End Function

    ''' <summary>
    ''' Set info bits for given SOURCE.
    ''' </summary>
    Private Function SetSourceState(logName As String, sourceName As String) As StateTypeSource

            If sourceName Is Nothing Then sourceName = ""
            If    logName Is Nothing Then    logName = ""

                        If SourceList.Exists(Function(S) S.Name.ToLower = sourceName.ToLower) AndAlso _
                                EventLog.SourceExists(sourceName) Then _
            _SourceState.EXISTS = True Else _SourceState.EXISTS = False

            _SourceState.CHARSET_FITS = New Regex("^[a-zA-Z]{1,1}[a-zA-Z_0-9_\.\s]{4,}$").IsMatch(sourceName)

                         If SourceList.Exists(Function(S) S.Name.ToLower = sourceName.ToLower And S.Owner = OwnerEnum.OWN) Then _
            _SourceState.IS_OWN = True Else _SourceState.IS_OWN = False

        Try

            If ThisProtectedLogsFull.Exists(Function(Str)   Str.ToLower = LogList.Find( _
                                            Function (L) L.Name.ToLower = SourceList.Find( _
                                            Function (S) S.Name.ToLower = sourceName.ToLower).Parent.Name.ToLower).Name.ToLower) Then 
                _SourceState.IS_PROTECTED = True 
            Else 
                _SourceState.IS_PROTECTED = False
            End If

        Catch ex As Exception
                _SourceState.IS_PROTECTED = False 
        End Try

                        If  Len(RegRootKey.Name) + _
                            KEYLENGT_RESERVE + _
                            Len(logName) + _
                            Len(sourceName) > 255 Then _
            _SourceState.REGKEY_LENGTH_FITS = False Else _SourceState.REGKEY_LENGTH_FITS = True

                        If logName = sourceName And Not sourceName = "" Then _
            _SourceState.IS_STANDARD_SOURCE = True Else _SourceState.IS_STANDARD_SOURCE = False

        Return SourceState

    End Function

#End Region ' "Set States help functions"

#End Region ' Public Code

#Region "Private Code"


    ''' <summary>
    ''' Check Registry, if Keys exists. If not, they are created.
    ''' </summary>
    Private Sub InitReg()

        If Not RegRootKey.GetValueNames.ToList.Exists(Function(V) V.ToLower = "OwnLogs".ToLower) Then _
               RegRootKey.SetValue("OwnLogs", New String() {}, RegistryValueKind.MultiString)

        If Not RegRootKey.GetValueNames.ToList.Exists(Function(V) V.ToLower = "OwnSources".ToLower) Then _
               RegRootKey.SetValue("OwnSources", New String() {}, RegistryValueKind.MultiString)

    End Sub

#End Region ' Private Code


    Public Class EventLogX
        Inherits EventLog


        Public ReadOnly Property Parent As EventLogsX
        Public ReadOnly Property Name As String
        Public ReadOnly Property ShortName As String

        Private _Owner As OwnerEnum

        Public ReadOnly Property ChildSources As List(Of LogSource)

        ''' <summary>
        ''' Sets owner state. If <paramref name="withChilds"/> = True, then ownership of refered sources will be changed as well.
        ''' </summary>
        ''' <param name="newOwner"></param>
        ''' <param name="withChilds"></param>
        Public Sub SetOwner(newOwner As OwnerEnum, withChilds As Boolean)

            If newOwner = _Owner Then Exit Sub

            ' Transform MultiSZ RegValue to List(Of String)
            Dim MyOwnList = CType(Parent.RegRootKey.GetValue("OwnLogs"), Array).Cast(Of String)().ToList()

            Select Case newOwner

                Case OwnerEnum.OTHER

                    If Parent.ActionState.LOG_TO_OTHER Then MyOwnList.Remove(Me.Name) Else Exit Sub

                Case OwnerEnum.OWN

                    If Parent.ActionState.LOG_TO_OWN Then MyOwnList.Add(Me.Name) Else Exit Sub

            End Select

            ' Write the MultiSZ RegValue to registry
            Parent.RegRootKey.SetValue("OwnLogs", MyOwnList.ToArray)

            _Owner = newOwner

            If withChilds Then
                For Each ChildSource In ChildSources
                    ChildSource.SetOwner(newOwner, True)
                Next
            End If

        Parent.Refresh

        End Sub

        Public ReadOnly Property Owner As OwnerEnum

            Get
                Return _Owner
            End Get

        End Property ' Owner As OwnerEnum

        Public Sub New(logName As String, parent As EventLogsX)
            MyBase.New(logName)

            _Name = Log
            _ShortName = Left(Name, 8)
            _Parent = parent
            _Owner = GetOwnerState()

            ChildSources = GetSourcesFromReg()

        End Sub

        ''' <summary>
        ''' Searches all EventLogs and returns List (Of EventLogX)
        ''' </summary>
        ''' <param name="parent"></param>
        ''' <returns></returns>
        Protected Friend Overloads Shared Function GetEventLogs(parent As EventLogsX) As List(Of EventLogX)

            GetEventLogs = New List(Of EventLogX)

            For Each Ev In EventLog.GetEventLogs
                GetEventLogs.Add(New EventLogX(Ev.Log, parent))
            Next

        End Function

        Private Function GetOwnerState() As OwnerEnum

            If CType(Parent.RegRootKey.GetValue("OwnLogs"), Array).Cast(Of String)().ToList().Find(Function(L) L.ToLower = Me.Name.ToLower) Is Nothing Then Return OwnerEnum.OTHER

            Return OwnerEnum.OWN

        End Function

        Private Function GetSourcesFromReg() As List(Of LogSource)

            Dim MyReturn As New List(Of LogSource)

            For Each SubKeyName In Parent.RegRootKey.OpenSubKey(Me.Name).GetSubKeyNames
                MyReturn.Add(New LogSource(Me, SubKeyName))
            Next

            Parent.AddSource(MyReturn)

            Return MyReturn

        End Function

        Public Class LogSource

            Public ReadOnly Property Root As EventLogsX
            Public ReadOnly Property Parent As EventLogX
            Public ReadOnly Property Name As String

            Public ReadOnly Property Owner As OwnerEnum

            Public Sub SetOwner(newOwner As OwnerEnum, Optional IsChildMove As Boolean = False)

                If newOwner = _Owner Then Exit Sub

                    Call Root.Validate(Parent.Name, Name)

                Dim MyOwnList = CType(Root.RegRootKey.GetValue("OwnSources"), Array).Cast(Of String)().ToList()

                If newOwner = OwnerEnum.OWN Then
                        
                    If Root.ActionState.SOURCE_TO_OWN Then MyOwnList.Add(Me.Name) Else Exit Sub

                ElseIf newOwner = OwnerEnum.OTHER Then

                    If Root.ActionState.SOURCE_TO_OTHER Then MyOwnList.Remove(Me.Name) Else Exit Sub
                        
                End If

                Root.RegRootKey.SetValue("OwnSources", MyOwnList.ToArray)
                _Owner = newOwner

                If Not IsChildMove Then Root.Refresh

            End Sub

            Public Sub New(parent As EventLogX, name As String)

                _Parent = parent
                _root = parent.Parent
                _Name = name
                _Owner = GetOwnerState()

            End Sub
            Private Function GetOwnerState() As OwnerEnum

                ' Lookup own soures RegValue. if not inside then owner is NOT "OWN" >> OTHER
                If CType(Root.RegRootKey.GetValue("OwnSources"), Array).Cast(Of String)().ToList().Find(Function(L) L.ToLower = Me.Name.ToLower) Is Nothing Then Return OwnerEnum.OTHER

                Return OwnerEnum.OWN

            End Function

        End Class ' LogSource

    End Class ' EventlogX

End Class ' EventlogsX

