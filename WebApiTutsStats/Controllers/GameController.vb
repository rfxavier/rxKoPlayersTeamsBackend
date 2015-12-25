Imports System.Net
Imports System.Web.Http
Imports WebApiTutsStats.Models

Namespace Controllers
    Public Class GameController
        Inherits BaseApiController

        Public Sub New()
            MyBase.New(New ModelFactory)
        End Sub

        ' GET: api/game
        Public Function [Get]() As IHttpActionResult
            Dim games = RxApi.gameCollection.LoadAll
            Dim models = games.Select(Function(x) ModelFactory.Create(x))

            Return Ok(models)
        End Function


        ' GET: api/game/5
        Public Function [Get](id As Long) As IHttpActionResult
            Try
                Dim game As RxApi.Game = RxApi.Game.Load(id)

                If game Is Nothing Then
                    Return NotFound()
                End If

                Dim model = ModelFactory.Create(game)

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


        Public Function CreateEvent(<FromBody()> gameEventModel As GameEventModel) As IHttpActionResult
            Try
                Dim gameEntity = RxApi.Game.Load(gameEventModel.GameId)
                Dim playerEntity = RxApi.Player.Load(gameEventModel.PlayerId)
                Dim pointValue = gameEventModel.PointValue

                Dim gameEventEntity = ModelFactory.Create(gameEntity, playerEntity, pointValue)

                gameEventEntity.Save()

                Return Created(String.Format("{0}/{1}", Request.RequestUri.ToString, gameEntity.pId.ToString), gameEventModel)
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