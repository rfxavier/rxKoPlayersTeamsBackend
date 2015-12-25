Imports System.Net
Imports System.Web.Http
Imports System.Net.Http
Imports WebApiTutsStats.Models
Imports CodeFluent.Runtime

Namespace Controllers
    Public Class TeamController
        Inherits BaseApiController

        ' GET: api/Team
        Public Sub New()
            MyBase.New(New ModelFactory)
        End Sub

        ' GET: api/team
        Public Function [Get]() As IHttpActionResult
            Dim teams = RxApi.TeamCollection.LoadAll
            Dim models = teams.Select(Function(x) ModelFactory.Create(x)).OrderBy(Function(x) x.TeamId)

            Return Ok(models)
        End Function

        ' GET: api/team/5
        Public Function [Get](id As Long) As IHttpActionResult
            Try
                Dim team As RxApi.Team = RxApi.Team.Load(id)

                If team Is Nothing Then
                    Return NotFound()
                End If

                Dim model = ModelFactory.Create(team)

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

        ' POST: api/Team
        Public Function [Post](<FromBody()> teamModel As TeamModel) As IHttpActionResult
            Try
                Dim teamEntity = ModelFactory.Create(teamModel)

                teamEntity.Save()

                ''TO BE EXPERIMENTED - RX
                'If teamModel.Players.Count > 0 Then
                '    'ADD PLAYERS TO TEAM, USING PAYLOAD FROM BODY IN A SINGLE CALL (ROUND TRIP) TO WEB API
                '    '{"teamName":"Nome Time", "players": [{"firstName": "John", "lastName": "Doe"}, {"firstName" :"Jane","lastName": "Doe"}]}
                '    For Each player In teamModel.Players
                '        Dim playerEntity = ModelFactory.Create(player)

                '        'BEFORE SAVING, ASSIGN TEAM RECENTLY INSERTED OR UPDATED (teamEntity.Save())
                '        playerEntity.oTeam = teamEntity
                '        playerEntity.Save()
                '    Next
                'End If

                Dim model = ModelFactory.Create(teamEntity)
                Return Created(String.Format("{0}/{1}", Request.RequestUri.ToString, model.TeamId.ToString()), model)
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

        ' PUT: api/Team/5
        Public Function [Put](<FromBody()> teamModel As TeamModel) As IHttpActionResult

            Try
                Dim teamEntity = ModelFactory.Create(teamModel)

                If teamEntity Is Nothing Then
                    Return NotFound()
                End If

                teamEntity.Save()

                Dim model = ModelFactory.Create(teamEntity)
                Return Ok(model)
            Catch ex As Exception
#If DEBUG Then
                Return BadRequest(ex.Message)
#Else
                Return BadRequest()
#End If
            End Try
        End Function

        ' DELETE: api/Team/5
        Public Function Delete(id As Long) As IHttpActionResult

            Try
                Dim teamEntity = RxApi.Team.Load(id)

                If teamEntity Is Nothing Then
                    Return NotFound()
                ElseIf teamEntity.Delete Then
                    Return Ok()
                Else
                    Return InternalServerError()
                End If
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
    End Class
End Namespace