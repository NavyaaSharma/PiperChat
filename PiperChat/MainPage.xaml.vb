Imports System.ComponentModel
Imports System.Timers

Class MainPage

    Dim pyFace As New PythonInterface()
    Public listener_timer As New Timers.Timer

    Public Delegate Sub AppendToListView_(ByVal text As String)
    Public AppendToListView_Delegate As New AppendToListView_(AddressOf AppendToListView_Normal)

    Private Sub listener_timer_Tick(sender As Object, e As ElapsedEventArgs)
        pyFace.stdinStreamWriter.WriteLine("")
    End Sub

    Private Sub AppendToListView(text As String)
        Dim item As New ListViewItem With {.Content = text}
        item.Style = CType(My.Application.Resources("ListViewItemWinterStyle.Input"), Style)
        listView.Items.Add(item)
    End Sub

    Private Sub AppendToListView_Normal(text As String)

        listView.Dispatcher.Invoke(New Action(Sub()
                                                  Dim item As New ListViewItem With {.Content = text}
                                                  item.Style = CType(My.Application.Resources("ListViewItemWinterStyle"), Style)

                                                  listView.Items.Add(item)

                                              End Sub))



    End Sub




    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        Dim item As New ListViewItem With {.Content = "Nick Fury: Sending Message to Captain Marvel"}
        item.Style = CType(My.Application.Resources("ListViewItemWinterStyle.Input"), Style)
        'listView.Items.Add(item)
    End Sub

    Private Sub Output_Received(sender As Object, e As DataReceivedEventArgs)
        If e.Data = Nothing Then Exit Sub
        AppendToListView_Normal(e.Data)
    End Sub



    Private Sub BtnStart_HexagonMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles btnStart.HexagonMouseLeftButtonDown
        pyFace.FilePath = $"""{AppDomain.CurrentDomain.BaseDirectory & "scripts\client.py"}"""

        pyFace.RunScript()

        pyFace.stdinStreamWriter.WriteLine("Vishaal")
        listener_timer.Enabled = True
        listener_timer.Interval = 10
        AddHandler pyFace.OutputReceived, AddressOf Output_Received
        AddHandler listener_timer.Elapsed, AddressOf listener_timer_Tick
    End Sub


    Private Sub TextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles textBox.KeyDown
        If e.KeyboardDevice.IsKeyDown(Key.Enter) And textBox.Text IsNot String.Empty Then
            pyFace.stdinStreamWriter.WriteLine("send_all")
            pyFace.stdinStreamWriter.WriteLine(Environment.MachineName & ": " & textBox.Text)
            AppendToListView(Environment.MachineName & ": " & textBox.Text)
            textBox.Text = ""
        End If
    End Sub
End Class
