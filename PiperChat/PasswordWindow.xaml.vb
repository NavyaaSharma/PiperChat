Public Class PasswordWindow
    Public password As String = ""
    Public Event passwordUpdated(sender As Object, e As EventArgs)

    Private Sub TextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles textBox.KeyDown
        If e.KeyboardDevice.IsKeyDown(Key.Enter) Then
            password = textBox.Text
            RaiseEvent passwordUpdated(password, e)
            Me.Close()
        End If
    End Sub


End Class
