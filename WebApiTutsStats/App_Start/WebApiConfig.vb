Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Http
Imports System.Net.Http.Formatting
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Converters
Imports WebApiContrib.Formatting.Jsonp
Imports Newtonsoft.Json.Serialization
Imports System.Web.Http.Cors

Public Module WebApiConfig
    Public Sub Register(ByVal config As HttpConfiguration)
        'DEFAULTS TO JSON
        GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear()

        'GlobalConfiguration.Configuration.Formatters.Insert(0, New JsonpMediaTypeFormatter(New JsonMediaTypeFormatter()))
        GlobalConfiguration.Configuration.AddJsonpFormatter()

        ' Web API configuration and services
        Dim cors = New EnableCorsAttribute("*", "*", "DELETE,GET,HEAD,POST,PUT,OPTIONS,TRACE")
        config.EnableCors(cors)

        config.MessageHandlers.Add(New BrowserOptionsHandler)

        ' Web API routes
        'CONFIGURATION BASED
        'config.MapHttpAttributeRoutes()

        'CONVENTION BASED
        config.Routes.MapHttpRoute(
            name:="DefaultApi",
            routeTemplate:="api/{controller}/{id}",
            defaults:=New With {.id = RouteParameter.Optional}
        )

        config.Routes.MapHttpRoute(
            name:="GameEvent",
            routeTemplate:="api/game/{id}/events",
            defaults:=New With {.controller = "game", .action = "CreateEvent"}
        )

        Dim jsonFormatter = config.Formatters.OfType(Of JsonMediaTypeFormatter)().First()
        jsonFormatter.SerializerSettings.ContractResolver = New CamelCasePropertyNamesContractResolver()

        'ENABLE JSON DE-SERIALIZATION FOR CODEFLUENT GENERATED WCF PROXY - SUB-PRODUCER SERVICES
        'Dim jsonFormatter As JsonMediaTypeFormatter = DirectCast(config.Formatters.FirstOrDefault(Function(f) TypeOf f Is JsonMediaTypeFormatter), JsonMediaTypeFormatter)
        'If jsonFormatter IsNot Nothing Then
        '    ' use WCF serializer
        '    jsonFormatter.UseDataContractJsonSerializer = True
        'End If
    End Sub
End Module
