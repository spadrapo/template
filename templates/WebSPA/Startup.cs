using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Sysphera.Middleware.Drapo;
using Sysphera.Middleware.Drapo.Pipe;
using System.IO;

namespace WebSPA
{
    public class Startup
    {
        private DrapoMiddlewareOptions _options = null;
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSignalR().AddJsonProtocol(options => options.PayloadSerializerOptions.PropertyNamingPolicy = null);
            services.AddDrapo();
            services.AddMvc()
                  .AddJsonOptions(options =>
                  {
                      options.JsonSerializerOptions.PropertyNamingPolicy = null;
                  });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Drapo Docs API",
                    Version = "v1",
                    Description = "API to be used in the drapo docs",
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDrapo(o => { ConfigureDrapo(env, o); });
            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = (context) =>
                {
                    var headers = context.Context.Response.GetTypedHeaders();
                    headers.CacheControl = new CacheControlHeaderValue() { NoCache = true };
                }
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<DrapoPlumberHub>(string.Format("/{0}", _options.Config.PipeHubName));
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebSPA API");
            });
        }

        private void ConfigureDrapo(IWebHostEnvironment env, DrapoMiddlewareOptions options)
        {
            if (env.IsDevelopment())
                options.Debug = true;
            options.Config.UsePipes = false;
            options.Config.CreateTheme("", "");
            options.Config.CreateTheme("Dark", "dark");
            options.Config.StorageErrors = "errors";
            options.Config.ValidatorUncheckedClass = "ppValidationUnchecked";
            options.Config.ValidatorValidClass = "ppValidatorValid";
            options.Config.ValidatorInvalidClass = "ppValidatorInvalid";
            options.Config.OnError = "UncheckItemField({{dkLayoutMenuState.menu}});ClearItemField({{taError.Container}});ClearSector(rainbow);ClearSector(footer);UpdateSector(content,/app/error/index.html,Error,true,true,{{tabError.Container}});UncheckDataField(dkTabs,Selected,false);AddDataItem(dkTabs,{{tabError}})";
            options.Config.LoadComponents(string.Format("{0}{1}components", env.WebRootPath, Path.AltDirectorySeparatorChar), "~/components");
            this._options = options;
        }
    }
}
