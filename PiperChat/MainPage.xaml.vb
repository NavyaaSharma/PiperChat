Imports System.ComponentModel
Imports System.Timers

Class MainPage
    Dim password As String = ""
    Dim pyFace As New PythonInterface()
    Public listener_timer As New Timers.Timer
    Dim des As New Simple3des("piperchat")
    Public Delegate Sub AppendToListView_(ByVal text As String)
    Public AppendToListView_Delegate As New AppendToListView_(AddressOf AppendToListView_Normal)

    Private Sub listener_timer_Tick(sender As Object, e As ElapsedEventArgs)
        pyFace.stdinStreamWriter.WriteLine("")
    End Sub

    Private Sub AppendToListView(text As String)
        Dim item As New ListViewItem With {.Content = text}
        item.Style = CType(My.Application.Resources("ListViewItemWinterStyle.Input"), Style)
        listView.Items.Add(item)
        listView.SelectedIndex = listView.Items.Count - 1
        listView.ScrollIntoView(listView.SelectedItem)
    End Sub

    Private Sub AppendToListView_Normal(text As String)

        listView.Dispatcher.Invoke(New Action(Sub()
                                                  Dim item As New ListViewItem With {.Content = des.DecryptData(text)}
                                                  item.Style = CType(My.Application.Resources("ListViewItemWinterStyle"), Style)
                                                  If item.Content = Nothing Then
                                                      Exit Sub
                                                  End If
                                                  listView.Items.Add(item)
                                                  listView.SelectedIndex = listView.Items.Count - 1
                                                  listView.ScrollIntoView(listView.SelectedItem)
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
            pyFace.stdinStreamWriter.WriteLine(des.EncryptData(Environment.MachineName & ": " & textBox.Text))
            AppendToListView(Environment.MachineName & ": " & textBox.Text)
            textBox.Text = ""
        End If
    End Sub

    Private Sub password_update(sender As Object, e As EventArgs)
        des = New Simple3des(sender)
    End Sub

    Private Sub BtnSetupEncryption_HexagonMouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles btnSetupEncryption.HexagonMouseLeftButtonDown
        Dim passwordWindow As New PasswordWindow
        passwordWindow.Show()
        AddHandler passwordWindow.passwordUpdated, AddressOf password_update


    End Sub
End Class
