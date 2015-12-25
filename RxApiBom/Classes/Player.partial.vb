Imports CodeFluent.Runtime

Namespace RxApi
    Partial Public Class Player
        Private Sub OnBeforeSave()
            If Me.pFirstName = String.Empty Then
                Throw New CodeFluentValidationException("Primeiro nome não pode estar em branco")
            End If
        End Sub
    End Class
End Namespace
