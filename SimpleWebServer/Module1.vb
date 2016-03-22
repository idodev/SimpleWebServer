Module Module1

    Sub Main(ByVal args As String())
        If args.Length <> 2 Then
            Console.WriteLine("Please provide a path and port")
            LogEvent("Press any key to close...")
            Console.ReadKey()
            Exit Sub
        End If


        Dim RootPath As String = args(0)
        Dim Port As Integer = args(1)

        RootPath = IO.Path.Combine(Environment.CurrentDirectory, RootPath)
        
        Dim ws As New SimpleWebServer.WebServer(Port, RootPath, AddressOf LogEvent)
        ws.StartServer()

        LogEvent("Server Running at " + RootPath + "...", ConsoleColor.Green)
        LogEvent("Type 'stop' to terminate the server.")
        While Console.ReadLine.ToLower <> "stop"
            LogEvent("Input not recognized. Type 'stop' to terminate the server.")
        End While

        ws.StopServer()
        ws = Nothing
        LogEvent("Press any key to close...")
        Console.ReadKey()

    End Sub


    Private Sub LogEvent(ByVal Text As String, Optional Color As ConsoleColor = Nothing)
        If Color = Nothing Then Color = ConsoleColor.White
        Console.ForegroundColor = Color
        Console.WriteLine(Text)

    End Sub

End Module
