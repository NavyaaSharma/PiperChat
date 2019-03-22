Class MainPage
    Private Sub Page_Loaded(sender As Object, e As RoutedEventArgs)
        Dim item As New ListViewItem With {.Content = "Nick Fury: Sending Message to Captain Marvel"}
        item.Style = CType(My.Application.Resources("ListViewItemWinterStyle.Input"), Style)
        listView.Items.Add(item)
    End Sub
End Class
