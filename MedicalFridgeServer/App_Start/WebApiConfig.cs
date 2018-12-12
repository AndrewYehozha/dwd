using System.Web.Http;

namespace MedicalFridgeServer
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Конфигурация и службы веб-API
            config.EnableCors();
            
            // Маршруты веб-API
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "MyApi1",
                routeTemplate: "api/{controller}/{value}/"
            );

            config.Routes.MapHttpRoute(
                name: "MyApi2",
                routeTemplate: "api/{controller}/{value1}/{value2}"
            );
        }
    }
}
