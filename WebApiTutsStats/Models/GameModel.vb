Public Class GameModel
    Public Property GameId As Integer
    Public Property AwayTeam As TeamModel
    Public Property HomeTeam As TeamModel
    Public Property StartTime As DateTime

    Public Property Events As List(Of GameEventModel)
End Class
