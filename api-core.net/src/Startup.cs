using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;

namespace VotesAPI

{	//allows to setup or include the configuration values
    public class Startup
    {
		//allows to add services to the dependency injection container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }
		//allows to add middleware and services to the HTTP pipeline
        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}

