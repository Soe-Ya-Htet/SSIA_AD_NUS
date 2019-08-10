using Microsoft.Extensions.DependencyInjection;

namespace AlbumViewerNetCore
{
    public class Startup
    {
        public Startup()
        {

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    }
}


