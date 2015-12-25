Imports CodeFluent.Runtime

Namespace RxApi
    Partial Public Class Team
        Private Sub OnBeforeSave()
            If Me.pName = String.Empty Then
                Throw New CodeFluentValidationException("Primeiro nome não pode estar em branco")
            End If

        End Sub

        Private Sub OnBeforeDelete()
            If Me.oPlayers.Count > 0 Then
                Throw New CodeFluentValidationException("Não é possível exclusão. Existem jogadores vinculados")
            End If
        End Sub
    End Class
End Namespace
