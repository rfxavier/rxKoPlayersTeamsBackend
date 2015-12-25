Imports System.Net.Http
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Net

Public Class BrowserOptionsHandler
    Inherits DelegatingHandler
    Const Origin As String = "Origin"
    Const RequestMethod As String = "DELETE,GET,HEAD,POST,PUT,OPTIONS,TRACE"
    Const RequestHeaders As String = "Access-Control-Request-Headers"
    Const AllowOrigin As String = "Access-Control-Allow-Origin"
    Const AllowMethods As String = "Access-Control-Allow-Methods"
    Const AllowHeaders As String = "Access-Control-Allow-Headers"

    Protected Overrides Function SendAsync(request As HttpRequestMessage, cancellationToken As CancellationToken) As Task(Of HttpResponseMessage)
        Dim isOptionsRequest As Boolean = (request.Method = HttpMethod.Options)

        If isOptionsRequest Then
            Return Task.Factory.StartNew(Of HttpResponseMessage)(Function()
                                                                     Dim response As New HttpResponseMessage(HttpStatusCode.OK)
                                                                     'conditionally limit allowed origins if needed
                                                                     response.Headers.Add(AllowOrigin, request.Headers.GetValues(Origin).First())

                                                                     'Dim accessControlRequestMethod As String = request.Headers.GetValues(RequestMethod).FirstOrDefault()
                                                                     'If accessControlRequestMethod IsNot Nothing Then
                                                                     '    'conditionally limit allowed methods if needed
                                                                     '    response.Headers.Add(AllowMethods, RequestMethod)
                                                                     'End If
                                                                     response.Headers.Add(AllowMethods, RequestMethod)

                                                                     Dim requestedHeaders As String = String.Join(", ", request.Headers.GetValues(RequestHeaders))
                                                                     If Not String.IsNullOrEmpty(requestedHeaders) Then
                                                                         'conditionally limit allowed headers if needed
                                                                         response.Headers.Add(AllowHeaders, requestedHeaders)
                                                                     End If

                                                                     Return response

                                                                 End Function, cancellationToken)
        End If
        Return MyBase.SendAsync(request, cancellationToken)
    End Function

End Class
