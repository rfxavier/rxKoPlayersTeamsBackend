Imports System.Web.Http
Imports WebApiTutsStats.Models
Imports System.Web.Http.Cors

<EnableCors("*", "*", "DELETE,GET,HEAD,POST,PUT,OPTIONS,TRACE")> _
Public MustInherit Class BaseApiController
    Inherits ApiController
    Private _modelFactory As IModelFactory

    Protected Sub New(modelFactory As IModelFactory)
        _modelFactory = modelFactory
    End Sub

    Protected ReadOnly Property ModelFactory As IModelFactory
        Get
            Return _modelFactory
        End Get
    End Property
End Class
