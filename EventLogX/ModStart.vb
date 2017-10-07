Imports System.Diagnostics
Imports System.Reflection
Imports System.Security.Principal
Imports System.Windows.Forms

Module ModStart

    Private ThisBackColorOrg As ConsoleColor = Console.BackgroundColor
    Private    ThisForeColorOrg As ConsoleColor = Console.ForegroundColor

    Private ThisFrmThinClient As FrmThinClient = Nothing
    Private ThisFormThread As Threading.Thread = Nothing'(AddressOf ShowFrmThinClient) = 

    Private     ThisSelectedLog As EventLogsX.EventLogX
    Private  ThisSelectedSource As EventLogsX.EventLogX.LogSource
    
    Private ThisEventLogs As EventLogsX


    Public Sub Main()

        ThisEventLogs = New EventLogsX
        Call ShowRoot

    End Sub

    Private Sub CloseForm()

        If Not ThisFrmThinClient Is Nothing Then

            Try
                ThisFrmThinClient.BeginInvoke(New Action(Sub() ThisFrmThinClient.Close()))
            Catch ex As Exception
            End Try

            ThisFrmThinClient = Nothing

        End If
        
    End Sub

    Private Sub CloseThread()

        If Not ThisFrmThinClient Is Nothing Then CloseForm
    
        If Not ThisFormThread Is Nothing Then 
            On Error Resume Next    
                ThisFormThread.Abort 
                ThisFormThread = Nothing
            On Error GoTo 0
        End If

    End Sub

    Private Sub InitForm()

        If Not ThisFrmThinClient Is Nothing Then CloseForm
        If Not ThisFormThread Is Nothing Then CloseThread

        ThisFormThread = New Threading.Thread(AddressOf ShowFrmThinClient) 

        ThisFormThread.SetApartmentState(Threading.ApartmentState.STA)
        ThisFormThread.Start()

    End Sub

    Private Sub ShowRoot()

        ThisEventLogs.Refresh

           ThisSelectedLog = Nothing
        ThisSelectedSource = Nothing

        Console.BackgroundColor = ConsoleColor.DarkGreen

        Console.WriteLine("{0,-80}", " Enter Number ")
        Console.WriteLine("{0,-80}", " 1 = Start Thin Client")
        Console.WriteLine("{0,-80}", " 2 = Show Logs")
        Console.WriteLine("{0,-80}", " 3 = Create Log")
        Console.WriteLine("{0,-80}", " 4 = Exit Programm")

        Console.BackgroundColor = ConsoleColor.Black

        Dim MyCommand = Console.ReadLine
        Console.WriteLine("")

        Try

            Select Case CInt(MyCommand)

                Case 1 ' Start WinForm

                    Call InitForm
                    Call ShowRoot : Exit Sub

                Case 2 ' Show Dialog

                    Call ShowLogs : Exit Sub

                Case 3

                    Call ShowCreate : Exit Sub

                Case 4 ' End

                    CloseThread()
                    End

                Case Else
                    Console.BackgroundColor = ConsoleColor.DarkRed
                    Console.WriteLine("No valid number typed.")
                    Console.BackgroundColor = ConsoleColor.DarkGreen
                    Console.WriteLine("Try again.")
                    Call ShowRoot : Exit Sub
            End Select

        Catch ex As Exception
                    Console.BackgroundColor = ConsoleColor.DarkRed
                    Console.WriteLine("No valid number typed.")
                    Console.BackgroundColor = ConsoleColor.DarkGreen
                    Console.WriteLine("Try again.")
                    Call ShowRoot : Exit Sub
        End Try

        Call ShowRoot

    End Sub

    Private Sub ShowFrmThinClient()
        ThisFrmThinClient = New FrmThinClient()
         Application.Run(ThisFrmThinClient)
    End Sub

    Private Sub ShowLogs()

           ThisSelectedLog = Nothing
        ThisSelectedSource = Nothing

        ThisEventLogs.Refresh

        Console.BackgroundColor = ConsoleColor.DarkGreen
        Console.WriteLine("{0,-80}", "Select Log")
        Console.BackgroundColor = ConsoleColor.Green

        Console.ForegroundColor = ConsoleColor.DarkBlue

            For Each L In ThisEventLogs.GetLogs()
                Console.WriteLine("{0,-70}{1,-10}", L.Name, L.Owner)
            Next            

        Console.ForegroundColor = ThisForeColorOrg
        Console.BackgroundColor = ConsoleColor.DarkGreen
        Console.WriteLine("{0,-80}", "Type Log Name to select log, type ""ESC"" to go back to start menu.")

        Console.BackgroundColor = ConsoleColor.Black

        Dim MyLogName = Console.ReadLine.ToLower
        Console.WriteLine("") 

        If MyLogName.ToLower = "ESC".ToLower Then Call ShowRoot : Exit Sub

        ThisSelectedLog = ThisEventLogs.GetLogs().Find(Function (L) L.Name.ToLower = MyLogName)

        If ThisSelectedLog Is Nothing Then
            Console.BackgroundColor = ConsoleColor.DarkRed
            Console.WriteLine("No valid log typed in. Press Enter to go on.")
            Console.ReadLine
            Call ShowLogs : Exit Sub
        Else
            Call ShowSources : Exit Sub
        End If

        Call ShowRoot

    End Sub

    Private Sub ShowSources()

        ThisSelectedSource = Nothing

        Console.BackgroundColor = ConsoleColor.DarkGreen
        Console.WriteLine("")
        Console.WriteLine("{0,-80}", "Select Source")
        Console.BackgroundColor = ConsoleColor.Green

        Console.ForegroundColor = ConsoleColor.DarkBlue

            For Each S In ThisSelectedLog.ChildSources
                Console.WriteLine("{0,-70}{1,-10}", S.Name, S.Owner.ToString)
            Next

        Console.ForegroundColor = ThisForeColorOrg
        Console.BackgroundColor = ConsoleColor.DarkGreen
        Console.WriteLine("{0,-80}", "Type Source Name to select source, type ""ESC"" to go back, ""HOME"" for start menu.")
        Console.BackgroundColor = ConsoleColor.Black

        Dim MySourceName = Console.ReadLine.ToLower
        Console.WriteLine("") 

        If MySourceName.ToLower = "ESC".ToLower Then
            Call ShowLogs : Exit Sub
        ElseIf MySourceName.ToLower = "HOME".ToLower 
            Call ShowRoot : Exit Sub
        End If

        ThisSelectedSource = ThisEventLogs.GetSources().Find(Function (L) L.Name.ToLower = MySourceName)

        If ThisSelectedSource Is Nothing Then
            Console.BackgroundColor = ConsoleColor.DarkRed
            Console.WriteLine("{0,-80}", "No valid source typed in. Press Enter to go on.")
            Console.ReadLine
            Call ShowSources : Exit Sub
        End If

        Console.WriteLine("")
        
        Call ShowActions

    End Sub

    Private Sub ShowActions()

        Console.BackgroundColor = ConsoleColor.DarkBlue
        Console.WriteLine("{0,-20}{1,-60}"," Selected Log: ", ThisSelectedLog.Name)
        Console.WriteLine("{0,-20}{1,-60}"," Selected Source: ", ThisSelectedSource.Name)
        Console.WriteLine("")
        Console.BackgroundColor = ConsoleColor.DarkGreen
        Console.WriteLine("{0,-80}"," Select action to do.")
        Console.WriteLine("{0,-80}"," Green back color indicates positiv action validation.")

        Dim MyActionState = ThisEventLogs.Validate(ThisSelectedLog.Name, ThisSelectedSource.Name)

        Console.BackgroundColor =  CType(IIf(MyActionState.LOG_DELETE, ConsoleColor.DarkGreen, ConsoleColor.DarkRed), ConsoleColor)
        Console.WriteLine("{0,-80}", " 1 = Delete Log")
        Console.BackgroundColor =  CType(IIf(MyActionState.SOURCE_DELETE, ConsoleColor.DarkGreen, ConsoleColor.DarkRed), ConsoleColor)
        Console.WriteLine("{0,-80}", " 2 = Delete Source")
        Console.BackgroundColor =  CType(IIf(MyActionState.LOG_TO_OTHER, ConsoleColor.DarkGreen, ConsoleColor.DarkRed), ConsoleColor)
        Console.WriteLine("{0,-80}", " 3 = Switch Log to OTHER")
        Console.BackgroundColor =  CType(IIf(MyActionState.LOG_TO_OWN, ConsoleColor.DarkGreen, ConsoleColor.DarkRed), ConsoleColor)
        Console.WriteLine("{0,-80}", " 4 = Switch Log to OWN")
        Console.BackgroundColor =  CType(IIf(MyActionState.SOURCE_TO_OTHER, ConsoleColor.DarkGreen, ConsoleColor.DarkRed), ConsoleColor)
        Console.WriteLine("{0,-80}", " 5 = Switch Source to OTHER")
        Console.BackgroundColor =  CType(IIf(MyActionState.SOURCE_TO_OWN, ConsoleColor.DarkGreen, ConsoleColor.DarkRed), ConsoleColor)
        Console.WriteLine("{0,-80}", " 6 = Switch Source to OWN")
        Console.BackgroundColor =  ConsoleColor.DarkGreen
        Console.WriteLine("")
        Console.WriteLine("{0,-80}", " 7 = Back to Source Selection")
        Console.WriteLine("{0,-80}", " 8 = Back to Log Selection")
        Console.WriteLine("{0,-80}", " 9 = Back to Root")

        Console.BackgroundColor = ConsoleColor.Black
        Dim MyAction = Console.ReadLine

            Try 

                Select Case CInt(MyAction)

                    Case 1 ' Delete Log

                        If MyActionState.LOG_DELETE Then

                            Dim MyLogName = ThisSelectedLog.Name

                            If ThisEventLogs.DeleteLog(ThisSelectedLog.Name) Then
                                Console.BackgroundColor = ConsoleColor.DarkGreen
                                Console.WriteLine("{0,-80}", "Log " & MyLogName & " deleted. Press Enter to continue")
                                Console.ReadLine
                                Console.WriteLine("")
                                Call ShowRoot : Exit Sub                                
                            Else
                                Console.BackgroundColor = ConsoleColor.DarkRed
                                Console.WriteLine("{0,-80}", "Error deleting " & MyLogName & ". Press Enter to continue")
                                Console.ReadLine
                                Console.WriteLine("")
                                Call ShowRoot : Exit Sub                                
                            End If
                            
                        Else ' Assumptions missing to delete log.
                            Console.BackgroundColor = ConsoleColor.DarkRed
                            Console.WriteLine("{0,-80}", "No valid action was choosen. Press Enter to go on.")
                            Console.ReadLine
                            Console.WriteLine("")
                            Call ShowActions : Exit Sub
                        End If

                    Case 2 ' Delete Source

                        If MyActionState.SOURCE_DELETE Then

                            Dim MySourceName = ThisSelectedSource.Name
                            ThisEventLogs.DeleteSource(ThisSelectedSource.Name)

                            ' Check if log is deleted.
                            If ThisEventLogs.GetSources.Find(Function (S) S.Name = MySourceName) Is Nothing Then
                                Console.BackgroundColor = ConsoleColor.DarkGreen
                                Console.WriteLine("{0,-80}", "Source """ & MySourceName & """ deleted. Press Enter to continue")
                                Console.ReadLine
                                Console.WriteLine("")
                                Call ShowRoot : Exit Sub                                
                            Else
                                Console.BackgroundColor = ConsoleColor.DarkRed
                                Console.WriteLine("{0,-80}", "Error deleting """ & MySourceName & """. Press Enter to continue")
                                Console.ReadLine
                                Console.WriteLine("")
                                Call ShowRoot : Exit Sub                                
                            End If

                        Else

                            Console.BackgroundColor = ConsoleColor.DarkRed
                            Console.WriteLine("{0,-80}", "No valid action was choosen. Press Enter to go on.")
                            Console.ReadLine
                            Console.WriteLine("")
                            Call ShowActions : Exit Sub

                        End If

                    Case 3 ' Switch Log to OTHER

                        If MyActionState.LOG_TO_OTHER Then
                            ThisSelectedLog.SetOwner(EventLogsX.OwnerEnum.OTHER, True)
                            Console.BackgroundColor = ConsoleColor.DarkGreen
                            Console.WriteLine("{0,-80}", "Log """ & ThisSelectedLog.Name & """ new owner = ""OWN"". Press Enter to continue")
                            Console.ReadLine
                            Console.WriteLine("")

                        Else
                            Console.BackgroundColor = ConsoleColor.DarkRed
                            Console.WriteLine("{0,-80}", "No valid action typed in. Press Enter to go on.")
                            Console.ReadLine
                            Console.WriteLine("")
                            Call ShowActions : Exit Sub
                        End If

                    Case 4 ' Switch Log to OWN

                        If MyActionState.LOG_TO_OWN Then
                            ThisSelectedLog.SetOwner(EventLogsX.OwnerEnum.OWN, True)
                            Console.BackgroundColor = ConsoleColor.DarkGreen
                            Console.WriteLine("{0,-80}", "Log """ & ThisSelectedLog.Name & """ new owner = ""OTHER"". Press Enter to continue")
                            Console.ReadLine
                            Console.WriteLine("")
                        Else
                            Console.BackgroundColor = ConsoleColor.DarkRed
                            Console.WriteLine("{0,-80}", "No valid action typed in. Press Enter to go on.")
                            Console.ReadLine
                            Console.WriteLine("")
                            Call ShowActions : Exit Sub
                        End If

                    Case 5 ' Switch Source to OTHER

                        If MyActionState.SOURCE_TO_OTHER Then
                            ThisSelectedSource.SetOwner(EventLogsX.OwnerEnum.OTHER)
                            Console.BackgroundColor = ConsoleColor.DarkGreen
                            Console.WriteLine("{0,-80}", "Source """ & ThisSelectedSource.Name & """ new owner = ""OTHER"". Press Enter to continue")
                            Console.ReadLine
                            Console.WriteLine("")
                        Else
                            Console.BackgroundColor = ConsoleColor.DarkRed
                            Console.WriteLine("{0,-80}", "No valid action typed in. Press Enter to go on.")
                            Console.ReadLine
                            Call ShowActions : Exit Sub
                        End If

                    Case 6 ' Switch Source to OWN

                        If MyActionState.SOURCE_TO_OWN Then
                            ThisSelectedSource.SetOwner(EventLogsX.OwnerEnum.OWN)
                            Console.BackgroundColor = ConsoleColor.DarkGreen
                            Console.WriteLine("{0,-80}", "Source """ & ThisSelectedSource.Name & """ new owner = ""OWN"". Press Enter to continue")
                            Console.ReadLine
                            Console.WriteLine("")
                        Else
                            Console.BackgroundColor = ConsoleColor.DarkRed
                            Console.WriteLine("{0,-80}", "No valid action typed in. Press Enter to go on.")
                            Console.ReadLine
                            Console.WriteLine("")
                            Call ShowActions : Exit Sub
                        End If

                    Case 7 ' Back to Source Selection
                             Call ShowSources : Exit Sub

                    Case 8 ' Back to Log Selection
                             Call ShowLogs : Exit Sub

                    Case 9 ' Back to Root
                             Call ShowRoot : Exit Sub

                    Case Else

                        Console.BackgroundColor = ConsoleColor.DarkRed
                        Console.WriteLine("{0,-80}", "No valid action typed in. Press Enter to go on.")
                        Console.ReadLine
                        Console.WriteLine("")
                        Call ShowActions : Exit Sub

                End Select

            Catch ex As Exception

                Console.BackgroundColor = ConsoleColor.DarkRed
                Console.WriteLine("{0,-80}", "No valid action typed in. Press Enter to go on.")
                Console.ReadLine
                Console.WriteLine("")
                Call ShowActions : Exit Sub

            End Try

        Console.ReadLine


    End Sub

    Private Sub ShowCreate()

        Console.BackgroundColor = ConsoleColor.DarkGreen
        Console.WriteLine("{0,-80}", "Type ""L"" to create new LOG and SOURCE, ""S"" to create SOURCE to existing LOG, ""ESC"" to go back.")

        Console.BackgroundColor = ConsoleColor.Black
        Dim MyAction = Console.ReadLine
        Console.WriteLine("")

        Select Case MyAction

            Case "L".ToLower

                Console.BackgroundColor = ConsoleColor.DarkGreen
                Console.WriteLine("")
                Console.WriteLine("{0,-80}", "Type name of new log.")

                Console.BackgroundColor = ConsoleColor.Black
                Dim MyLogName = Console.ReadLine

                Console.BackgroundColor = ConsoleColor.DarkGreen
                Console.WriteLine("")
                Console.WriteLine("{0,-80}", "Type name of new source.")

                Console.BackgroundColor = ConsoleColor.Black
                Dim MySourceName = Console.ReadLine
                Console.WriteLine("")
                                
                If ThisEventLogs.Validate(MyLogName, MySourceName).SOURCE_CREATE Then

                    Console.BackgroundColor = ConsoleColor.DarkGreen
                    Console.WriteLine("{0,-80}", "Please confirm with ""Y"", else just ""ENTER"" ")
                    Console.BackgroundColor = ConsoleColor.DarkBlue
                    Console.WriteLine("{0,-40}{1,-40}", MyLogName, MySourceName)

                       Console.BackgroundColor = ConsoleColor.Black
                    If Console.ReadLine = "Y".ToLower Then ' confirm creating

                        Console.WriteLine("")

                        If ThisEventLogs.CreateSource(MyLogName, MySourceName) Then

                            Console.BackgroundColor = ConsoleColor.DarkGreen
                            Console.WriteLine("{0,-80}", "Log and Source were created. Press ""Enter"" to continue.")
                            Console.ReadLine

                        Else

                            Console.BackgroundColor = ConsoleColor.DarkRed
                            Console.WriteLine("{0,-80}", "An error occured. ")
                            Console.ReadLine

                        End If

                        Console.WriteLine("")
                        Call ShowRoot : Exit Sub

                    Else ' not confirmed >> creating was aborted by user.
                   
                        Console.WriteLine("")
                        Call ShowCreate : Exit Sub

                    End If

                Else ' negative validation result, cant create

                    Console.BackgroundColor = ConsoleColor.DarkRed
                    Console.WriteLine("{0,-80}", "No valid names for new log/source.")
                    Console.BackgroundColor = ConsoleColor.DarkGreen
                    Console.WriteLine("{0,-80}", "Want to know details ? Type ""Y"", if not only ""Enter"".")

                    Console.BackgroundColor = ConsoleColor.Black

                    If Console.ReadLine = "Y".ToLower Then ' show some details to the nagative validation result.
                    
                        Call WriteCreateLogAndSourceDetails(MyLogName, MySourceName)

                    End If

                    Console.WriteLine("")
                    Call ShowCreate : Exit Sub

                End If

            Case "S".ToLower

                Console.BackgroundColor = ConsoleColor.DarkGreen
                Console.WriteLine("")
                Console.WriteLine("{0,-80}", "Type name of existing log.")

                Console.BackgroundColor = ConsoleColor.Black
                Dim MyLogName = Console.ReadLine

                If ThisEventLogs.GetLogs().Find(Function (L) L.Name.ToLower = MyLogName) Is Nothing Then

                            Console.BackgroundColor = ConsoleColor.DarkRed
                            Console.WriteLine("{0,-80}", "The log you specified doesn't exist. Press ""ENTER"".")
                            Console.ReadLine
                            Call ShowCreate : Exit Sub

                End If

                Console.BackgroundColor = ConsoleColor.DarkGreen
                Console.WriteLine("")
                Console.WriteLine("{0,-80}", "Type name of new source.")

                Console.BackgroundColor = ConsoleColor.Black
                Dim MySourceName = Console.ReadLine
                Console.WriteLine("")
                                
                If ThisEventLogs.Validate(MyLogName, MySourceName).SOURCE_CREATE Then

                    Console.BackgroundColor = ConsoleColor.DarkGreen
                    Console.WriteLine("{0,-80}", "Please confirm with ""Y"", else just ""ENTER"" ")
                    Console.BackgroundColor = ConsoleColor.DarkBlue
                    Console.WriteLine("{0,-40}{1,-40}", MyLogName, MySourceName)

                       Console.BackgroundColor = ConsoleColor.Black
                    If Console.ReadLine = "Y".ToLower Then ' confirm creating

                        Console.WriteLine("")

                        If ThisEventLogs.CreateSource(MyLogName, MySourceName) Then

                            Console.BackgroundColor = ConsoleColor.DarkGreen
                            Console.WriteLine("{0,-80}", "Log and Source were created. Press ""Enter"" to continue.")
                            Console.ReadLine

                        Else

                            Console.BackgroundColor = ConsoleColor.DarkRed
                            Console.WriteLine("{0,-80}", "An error occured. ")
                            Console.ReadLine

                        End If

                        Console.WriteLine("")
                        Call ShowRoot : Exit Sub

                    Else ' not confirmed >> creating was aborted by user.
                   
                        Console.WriteLine("")
                        Call ShowCreate : Exit Sub

                    End If

                Else ' negative validation result, cant create

                    Console.BackgroundColor = ConsoleColor.DarkRed
                    Console.WriteLine("{0,-80}", "No valid names for new log/source.")
                    Console.BackgroundColor = ConsoleColor.DarkGreen
                    Console.WriteLine("{0,-80}", "Want to know details ? Type ""Y"", if not only ""Enter"".")

                    Console.BackgroundColor = ConsoleColor.Black

                    If Console.ReadLine = "Y".ToLower Then ' show some details to the nagative validation result.
                    
                        Call WriteCreateLogAndSourceDetails(MyLogName, MySourceName)

                    End If

                    Console.WriteLine("")
                    Call ShowCreate : Exit Sub

                End If


            Case "ESC".ToLower

                Call ShowRoot : Exit Sub

            Case Else

                Console.BackgroundColor = ConsoleColor.DarkRed
                Console.WriteLine("{0,-80}", "No valid action typed in. Press ""ENTER"" to go on.")
                Console.ReadLine
                Console.WriteLine("")
                Call ShowCreate : Exit Sub

        End Select


        Console.BackgroundColor = ConsoleColor.Black
        Console.ReadLine

        Call ShowRoot

    End Sub

    Private Sub WriteCreateLogAndSourceDetails(logName As String, sourceName As String)


        ThisEventLogs.Validate(logName, sourceName)

        Console.WriteLine("")

        Console.BackgroundColor = ConsoleColor.DarkGreen
        Console.WriteLine("{0,-80}", "Log and Source States")

        Console.BackgroundColor = ConsoleColor.Green
        Console.ForegroundColor = ConsoleColor.DarkBlue    

        Console.WriteLine("{0,-60}{1,-20}",         "LogState.CHARSET_FITS:", ThisEventLogs.LogState.CHARSET_FITS)
        Console.WriteLine("{0,-60}{1,-20}",               "LogState.EXISTS:", ThisEventLogs.LogState.EXISTS)
        Console.WriteLine("{0,-60}{1,-20}",   "LogState.IS_PROTECTED_FULLY:", ThisEventLogs.LogState.IS_PROTECTED_FULLY)

        Console.WriteLine("{0,-80}", "--------")

        Console.WriteLine("{0,-60}{1,-20}", "SourceState.CHARSET_FITS:",        ThisEventLogs.SourceState.CHARSET_FITS)
        Console.WriteLine("{0,-60}{1,-20}", "SourceState.EXISTS:",              ThisEventLogs.SourceState.EXISTS)
        Console.WriteLine("{0,-60}{1,-20}", "SourceState.REGKEY_LENGTH_FITS:",  ThisEventLogs.SourceState.REGKEY_LENGTH_FITS)

        Console.WriteLine("{0,-80}", "--------")
        Console.ForegroundColor = ThisForeColorOrg

        Console.BackgroundColor = ConsoleColor.DarkGreen
        Console.WriteLine("{0,-80}", "Press ""ENTER"" to continue.")
        Console.ReadLine

    End Sub


End Module
