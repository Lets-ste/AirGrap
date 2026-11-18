password = InputBox("Enter PostgreSQL password:", "Password")
If password = "" Then WScript.Quit

Set WshShell = CreateObject("WScript.Shell")
Set fso = CreateObject("Scripting.FileSystemObject")

scriptDir = fso.GetParentFolderName(WScript.ScriptFullName)
logFile = scriptDir & "\pg_timetable.log"

cmd = "cmd /c cd /d """ & scriptDir & """ && .\pg_timetable.exe " & _
      "-c=airgrap-planner ""postgresql://postgres:" & password & "@localhost:5432/airgrap"" " & _
      "> """ & logFile & """ 2>&1"

WshShell.Run cmd, 0, False

' Ждём 5 секунд и проверяем лог
WScript.Sleep 5000

If fso.FileExists(logFile) Then
    Set f = fso.OpenTextFile(logFile, 1)
    content = f.ReadAll()
    f.Close

    ' Ищем в логе признаки ошибки
    If InStr(LCase(content), "error") > 0 Or _
       InStr(LCase(content), "fatal") > 0 Or _
       InStr(LCase(content), "failed") > 0 Then

        ' === Убиваем процесс pg_timetable.exe ===
        WshShell.Run "cmd /c taskkill /F /IM pg_timetable.exe /T", 0, True

        MsgBox "Errors found in log. pg_timetable.exe was terminated." & vbCrLf & vbCrLf & content, vbCritical, "Error"

    Else
        MsgBox "pg_timetable started successfully." & vbCrLf & vbCrLf & content, vbInformation, "Success"
    End If
Else
    MsgBox "Log file was not created. pg_timetable.exe may be missing.", vbExclamation, "Warning"
End If