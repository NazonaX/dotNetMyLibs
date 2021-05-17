using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace wpfSimulation
{
   public  class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // 创建 Web API 的配置
            var config = new HttpConfiguration();
            // 启用标记路由
            config.MapHttpAttributeRoutes();
            //设置已json方式传递
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            // 默认的 Web API 路由
            config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{action}",
                    defaults: new { id = RouteParameter.Optional }
                );
            // 将路由配置附加到 appBuilder
            appBuilder.UseWebApi(config);
        }
    }
}
