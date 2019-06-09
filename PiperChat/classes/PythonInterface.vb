Imports System.IO

Public Class PythonInterface

    Private pyPath As String = String.Empty

    Public FilePath As String = ""
    Public Event OutputReceived(sender As Object, e As DataReceivedEventArgs)
    Public stdinStreamWriter As StreamWriter
    Public stdoutStreamReader As StreamReader

    Public Sub New()
        If My.Computer.FileSystem.FileExists("cache\pythondir.txt") Then
            pyPath = My.Computer.FileSystem.ReadAllText("cache\pythondir.txt")
        Else
            My.Computer.FileSystem.CreateDirectory("cache")
            IO.File.Create("cache\pythondir.txt").Close()
            Dim data = InputBox("Enter the python path: ")
            pyPath = data
            My.Computer.FileSystem.WriteAllText("cache\pythondir.txt", pyPath, False)
        End If

    End Sub
    Public Sub RunScript()
        Dim proc As New Process
        With proc.StartInfo
            .Arguments = $" {FilePath}"
            .CreateNoWindow = True
            .UseShellExecute = False
            .RedirectStandardOutput = True
            .RedirectStandardInput = True
            .FileName = pyPath
        End With
        proc.Start()
        proc.BeginOutputReadLine()
        AddHandler proc.OutputDataReceived, AddressOf Python_OutputReceived
        stdinStreamWriter = proc.StandardInput
        'stdoutStreamReader = proc.StandardOutput
    End Sub

    Public Sub Python_OutputReceived(sender As Object, e As DataReceivedEventArgs)
        RaiseEvent OutputReceived(sender, e)
    End Sub



End Class
