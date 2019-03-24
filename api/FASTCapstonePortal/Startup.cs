using FASTCapstonePortal.Interfaces;
using FASTCapstonePortal.Repositories;
using FASTCapstonePortal.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FASTCapstonePortal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                                              .AllowAnyMethod()
                                              .AllowAnyHeader()
                                              .AllowCredentials());
                            });
            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("CorsPolicy"));
             });
            services.AddSignalR();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "FAST Capstone Portal", Description = "FAST Capstone Portal API" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] {} }
                });
            });
            services.AddDbContext<CapstoneDBContext>(options => options
            .UseLazyLoadingProxies()
            .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<CapstoneUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<CapstoneDBContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IStudent, StudentRepositoryService>();
            services.AddScoped<IGroup, GroupRepositoryService>();
            services.AddScoped<INotification, NotificationRepositoryService>();
            services.AddScoped<IProject, ProjectRepositoryService>();
            services.AddScoped<IEntreaty, EntreatyRepositoryService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IImageWriterService, ImageWriterService>();
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "https://CapstonePortal.com",
                    ValidIssuer = "https://CapstonePortal.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("SecurityKey")))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/signalServer"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }

                    //In case error message needs to be returned in response body

                    //OnChallenge = context =>
                    //{
                    //    // Skip the default logic.
                    //    context.HandleResponse();

                    //    return context.Response.WriteAsync(context.ErrorDescription);
                    //}
            };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCors("CorsPolicy");
            app.UseSignalR(routes =>
            {
                routes.MapHub<SignalServer>("/signalServer");
            });
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(
            c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "FAST Capstone Portal API");
            });
        }
    }
}
