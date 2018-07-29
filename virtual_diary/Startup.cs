using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using virtual_diary.Providers;
using Unity;
using System.Data.Entity;
using virtual_diary.Infrastructure;
using Unity.Lifetime;
using virtual_diary.Repositories;
using virtual_diary.Models;
using Unity.WebApi;
using virtual_diary.Services;
using Newtonsoft.Json.Serialization;

[assembly: OwinStartup(typeof(virtual_diary.Startup))]
namespace virtual_diary
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            var container = SetupUnity();
            ConfigureOAuth(app, container);

            HttpConfiguration config = new HttpConfiguration();

            config.Formatters.JsonFormatter
                        .SerializerSettings
                        .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.DependencyResolver = new UnityDependencyResolver(container);

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            WebApiConfig.Register(config);
            app.UseWebApi(config);

        }

        public void ConfigureOAuth(IAppBuilder app, UnityContainer container)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider(container)
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }

        private UnityContainer SetupUnity()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<DbContext, AuthContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IGenericRepository<UserModel>, GenericRepository<UserModel>>();
            container.RegisterType<IGenericRepository<Admin>, GenericRepository<Admin>>();
            container.RegisterType<IGenericRepository<Teacher>, GenericRepository<Teacher>>();
            container.RegisterType<IGenericRepository<Parent>, GenericRepository<Parent>>();
            container.RegisterType<IGenericRepository<Student>, GenericRepository<Student>>();
            container.RegisterType<IGenericRepository<SubjectModel>, GenericRepository<SubjectModel>>();
            container.RegisterType<IGenericRepository<Class>, GenericRepository<Class>>();
            container.RegisterType<IGenericRepository<TeacherSubject>, GenericRepository<TeacherSubject>>();
            container.RegisterType<IGenericRepository<StudentTeacherSubject>, GenericRepository<StudentTeacherSubject>>();
            container.RegisterType<IGenericRepository<Grades>, GenericRepository<Grades>>();
            container.RegisterType<IAuthRepository, AuthRepository>();
            container.RegisterType<IAdminService, AdminService>();
            container.RegisterType<IParentService, ParentService>();
            container.RegisterType<ITeacherService, TeacherService>();
            container.RegisterType<IStudentService, StudentService>();
            container.RegisterType<ISubjectService, SubjectService>();
            container.RegisterType<IClassService, ClassService>();
            container.RegisterType<ITeacherSubjectService, TeacherSubjectService>();
            container.RegisterType<ISTSService, STSService>();
            container.RegisterType<IEmailService, EmailService>();
            return container;
        }

    }

}