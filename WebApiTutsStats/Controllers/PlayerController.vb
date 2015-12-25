Imports System.Net
Imports System.Web.Http
Imports System.Net.Http
Imports CodeFluent.Runtime
Imports WebApiTutsStats.Models

Namespace Controllers
    Public Class PlayerController
        Inherits BaseApiController

        Public Sub New()
            MyBase.New(New ModelFactory)
        End Sub

        ' GET: api/Player
        Public Function [Get]() As IHttpActionResult
            Dim players = RxApi.PlayerCollection.LoadAll
            Dim models = players.Select(Function(x) ModelFactory.Create(x)).OrderBy(Function(x) x.PlayerId)

            Return Ok(models)
        End Function

        ' GET: api/Player/5
        Public Function [Get](id As Long) As IHttpActionResult
            Try
                Dim player As RxApi.Player = RxApi.Player.Load(id)

                If player Is Nothing Then
                    Return NotFound()
                End If

                Dim model = ModelFactory.Create(player)

                Return Ok(model)
            Catch ex As Exception
                'logging
#If DEBUG Then
                Return InternalServerError()
#Else
                Return InternalServerError(ex)
#End If
            End Try
        End Function

        Public Function [Post](<FromBody()> playerModel As PlayerModel) As IHttpActionResult
            Try
                Dim playerEntity = ModelFactory.Create(playerModel)

                playerEntity.Save()

                Dim model = ModelFactory.Create(playerEntity)
                Return Created(String.Format("{0}/{1}", Request.RequestUri.ToString, model.PlayerId.ToString()), model)
            Catch ex As Exception
                If TypeOf ex Is CodeFluentValidationException Then
                    Return BadRequest(ex.Message)
                Else

#If DEBUG Then
                    Return BadRequest(ex.Message)
#Else
                    Return BadRequest()
#End If

                End If

            End Try
        End Function


        'Public Function [Post](<FromBody()> player As RxApi.Player) As HttpResponseMessage
        '    Try
        '        If Player Is Nothing OrElse Not Player.Save() Then
        '            [Post] = Request.CreateResponse(HttpStatusCode.BadRequest)
        '        End If

        '        Dim msg = New HttpResponseMessage(HttpStatusCode.Created)
        '        msg.Headers.Location = New Uri(String.Format("{0}/{1}", Request.RequestUri.ToString, Player.pId.ToString()))
        '        [Post] = msg

        '        '[Post] = Created(String.Format("http://localhost:6555/api/player/{0}", Player.pId), Player)
        '        'Return Request.CreateResponse(HttpStatusCode.Created, Player)
        '    Catch ex As Exception
        '        [Post] = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message, Configuration.Formatters.JsonFormatter)
        '    End Try
        'End Function

        Public Function [Put](<FromBody()> playerModel As PlayerModel) As IHttpActionResult

            Try
                Dim playerEntity = ModelFactory.Create(playerModel)

                If playerEntity Is Nothing Then
                    Return NotFound()
                End If

                playerEntity.Save()

                Dim model = ModelFactory.Create(playerEntity)
                Return Ok(model)
            Catch ex As Exception
#If DEBUG Then
                Return BadRequest(ex.Message)
#Else
                Return BadRequest()
#End If
            End Try

            'Try
            '    If player Is Nothing OrElse Not player.Save() Then
            '        [Put] = Request.CreateResponse(HttpStatusCode.BadRequest)
            '    End If

            '    Dim msg = New HttpResponseMessage(HttpStatusCode.OK)
            '    msg.Headers.Location = New Uri(String.Format("{0}/{1}", Request.RequestUri.ToString, player.pId.ToString()))
            '    [Put] = msg

            '    '[[Put]] = Created(String.Format("http://localhost:6555/api/player/{0}", Player.pId), Player)
            '    'Return Request.CreateResponse(HttpStatusCode.Created, Player)
            'Catch ex As Exception
            '    [Put] = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message, Configuration.Formatters.JsonFormatter)
            'End Try
        End Function

        Public Function Delete(id As Long) As IHttpActionResult

            Try
                Dim playerEntity = RxApi.Player.Load(id)

                If playerEntity Is Nothing Then
                    Return NotFound()
                ElseIf playerEntity.Delete Then
                    Return Ok()
                Else
                    Return InternalServerError()
                End If
            Catch ex As Exception
#If DEBUG Then
                Return BadRequest(ex.Message)
#Else
                Return BadRequest()
#End If
            End Try
        End Function
    End Class
End Namespace