using SSISTeam9.Services;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace SSISTeam9
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            container.RegisterType<IEmailService, EmailService>();
            container.RegisterType<IRestService, RestService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}