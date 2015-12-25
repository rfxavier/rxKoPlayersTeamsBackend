Imports RxApiBom.RxApi.Web

Namespace Models
    Public Interface IModelFactory
        Function Create(player As RxApi.Player) As PlayerModel
        Function Create(playerModel As PlayerModel) As RxApi.Player
        Function Create(team As RxApi.Team) As TeamModel
        Function Create(teamModel As TeamModel) As RxApi.Team
        Function Create(game As RxApi.Game) As GameModel
        Function Create(gameEvent As RxApi.GameEvent) As GameEventModel
        Function Create(gameEntity As RxApi.Game, playerEntity As RxApi.Player, pointValue As Integer) As RxApi.GameEvent
    End Interface
    Public Class ModelFactory
        Implements IModelFactory

        Public Function Create(ByVal player As RxApi.Player) As PlayerModel Implements IModelFactory.Create
            Dim playerTeamName As String = String.Empty

            If player.oTeam IsNot Nothing Then
                playerTeamName = player.oTeam.pName
            End If

            Return New PlayerModel With {.FirstName = player.pFirstName, _
                                         .LastName = player.pLastName, _
                                         .PlayerId = player.pId, _
                                         .TeamId = player.oTeampId, _
                                         .TeamName = playerTeamName}
        End Function

        Public Function Create(ByVal playerModel As PlayerModel) As RxApi.Player Implements IModelFactory.Create
            If playerModel.PlayerId <= 0 Then
                Return New RxApi.Player With {.pFirstName = playerModel.FirstName, _
                                              .pLastName = playerModel.LastName, _
                                              .oTeampId = playerModel.TeamId, _
                                              .pCreatedDate = DateTime.Now, _
                                              .pUpdatedDate = DateTime.Now}
            Else
                Dim existingPlayer = RxApi.Player.Load(playerModel.PlayerId)

                If existingPlayer IsNot Nothing Then
                    existingPlayer.pFirstName = playerModel.FirstName
                    existingPlayer.pLastName = playerModel.LastName
                    existingPlayer.pUpdatedDate = DateTime.Now
                End If

                Return existingPlayer
            End If
        End Function

        Public Function Create(ByVal team As RxApi.Team) As TeamModel Implements IModelFactory.Create
            Return New TeamModel With {.TeamId = team.pId, _
                                       .TeamName = team.pName, _
                                       .Players = team.oPlayers.Select(Function(x) Create(x)).ToList}
        End Function

        Public Function Create(ByVal teamModel As TeamModel) As RxApi.Team Implements IModelFactory.Create
            If teamModel.TeamId <= 0 Then
                Return New RxApi.Team With {.pName = teamModel.TeamName, _
                                            .pCreatedDate = DateTime.Now, _
                                            .pUpdatedDate = DateTime.Now}
            Else
                Dim existingTeam = RxApi.Team.Load(teamModel.TeamId)

                If existingteam IsNot Nothing Then
                    existingTeam.pName = teamModel.TeamName
                    existingTeam.pUpdatedDate = DateTime.Now
                End If

                Return existingteam
            End If
        End Function

        Public Function Create(ByVal game As RxApi.Game) As GameModel Implements IModelFactory.Create
            Return New GameModel With {.AwayTeam = Create(game.oAwayTeam), _
                                       .HomeTeam = Create(game.oHomeTeam), _
                                       .Events = game.oGameEvents.Select(Function(x) Create(x)).ToList, _
                                       .GameId = game.pId, _
                                       .StartTime = game.pStartTime}
        End Function

        Public Function Create(ByVal gameEvent As RxApi.GameEvent) As GameEventModel Implements IModelFactory.Create
            Return New GameEventModel With {.GameId = gameEvent.oGamepId, _
                                            .PlayerId = gameEvent.oPlayerpId, _
                                            .PointValue = gameEvent.pPointValue}
        End Function

        Public Function Create(ByVal gameEntity As RxApi.Game, ByVal playerEntity As RxApi.Player, ByVal pointValue As Integer) As RxApi.GameEvent Implements IModelFactory.Create
            Return New RxApi.GameEvent With {.oGame = gameEntity, _
                                             .oPlayer = playerEntity, _
                                             .pPointValue = pointValue, _
                                             .pCreatedDate = DateTime.Now, _
                                             .pUpdatedDate = DateTime.Now}
        End Function
    End Class
End Namespace