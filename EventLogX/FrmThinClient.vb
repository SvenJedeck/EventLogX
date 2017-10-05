Public Class FrmThinClient
' ftp://www.repository.thats.im/public_ftp/projects/logwizard
    Private ThisEv As ApiEventlogXWinform

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

        ThisEv = New ApiEventlogXWinform(Me, _
                                         LstLogsOwn, LstLogsOther, LstSourcesOwn, LstSourcesOther, _
                                         TxtLogName, TxtSourceName) With { _
                                                                                    .BtnDelLog = BtnDelLog, _
                                                                                 .BtnDelSource = BtnDelSource, _
                                                                                .BtnLogToOther = BtnLogToOther, _
                                                                                  .BtnLogtoOwn = BtnLogtoOwn, _
                                                                                 .BtnNewSource = BtnNewSource, _
                                                                             .BtnSourceToOther = BtnSourceToOther, _
                                                                               .BtnSourceToOwn = BtnSourceToOwn _
                                                                            }

   End Sub

End Class
