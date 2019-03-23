Imports System.Uri



Class MainWindow
    Private Sub NavigationWindow_Loaded(sender As Object, e As RoutedEventArgs)
        Dim mainPage As New MainPage
        Me.NavigationService.Navigate(New Uri("MainPage.xaml", UriKind.RelativeOrAbsolute))
        Me.Height = mainPage.Height + 40
        Me.Width = mainPage.Width + 10


    End Sub

    Private Sub NavigationWindow_SizeChanged(sender As Object, e As SizeChangedEventArgs)

    End Sub
End Class
