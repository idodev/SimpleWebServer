Imports System.IO
Imports System.Net
Imports System.Threading

Namespace SimpleWebServer

    Public Class WebServer
        Private ReadOnly _listener As HttpListener
        Private SourceFolder As IO.DirectoryInfo
        Private Log As Action(Of String, ConsoleColor)


        Public Sub New(ByVal Port As Integer, ByVal Source As String, ByVal l As Action(Of String, ConsoleColor))
            _listener = New HttpListener
            Try
                Log = l
                _listener.Prefixes.Add("http://localhost:" + Port.ToString + "/")
                SourceFolder = New IO.DirectoryInfo(Source)

            Catch

            End Try
        End Sub

        Public Sub StartServer()
            _listener.Start()
            ThreadPool.QueueUserWorkItem( _
                New WaitCallback( _
                    Sub()
                        Log("Webserver running...", ConsoleColor.Green)
                        Try
                            While _listener.IsListening
                                ThreadPool.QueueUserWorkItem( _
                                    New WaitCallback( _
                                        Sub(ctx As HttpListenerContext)
                                            ServeRequest(ctx.Request, ctx.Response)
                                        End Sub
                                    ), _listener.GetContext()
                                )
                            End While
                        Catch

                        End Try
                    End Sub
                )
            )
        End Sub

        Public Function IsRunning() As Boolean
            Return _listener.IsListening
        End Function

        Public Sub StopServer()
            Log("Webserver stopped...", ConsoleColor.Red)
            _listener.Stop()
        End Sub


        Private Sub ServeRequest(ByRef Request As HttpListenerRequest, ByRef Response As HttpListenerResponse)
            Dim ThisFile As IO.FileInfo
            Try
                Dim RawUrl = Request.RawUrl
                If RawUrl.EndsWith("/") Then
                    RawUrl = RawUrl + "index.html"
                End If
                Dim FilePath As String = SourceFolder.FullName + RawUrl
                ThisFile = New IO.FileInfo(FilePath)
                Dim data() As Byte = New Byte() {}

                If ThisFile.Exists Then
                    Response.StatusCode = HttpStatusCode.OK
                    data = File.ReadAllBytes(ThisFile.FullName)
                    Log("200 - " + RawUrl, ConsoleColor.White)
                Else
                    Response.StatusCode = HttpStatusCode.NotFound
                    Log("404 - " + RawUrl, ConsoleColor.Gray)
                End If

                Response.ContentLength64 = data.Length
                Response.OutputStream.Write(data, 0, data.Length)

            Catch ex As Exception
                Response.StatusCode = HttpStatusCode.InternalServerError
                Log("500 - " + Request.RawUrl, ConsoleColor.Red)
            Finally
                Response.OutputStream.Close()
            End Try

        End Sub

        Private Function MapPath(ByVal Path As String) As String
            Return SourceFolder.FullName + Path

        End Function


    End Class

End Namespace