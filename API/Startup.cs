namespace Notes.API
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Models.Notes.Repositories;
    using Models.Users.Repositories;
    using MongoDB.Driver;
    using System.Configuration;
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            MongoClient client = new MongoClient(ConfigurationManager.ConnectionStrings["NotesDb"].ConnectionString);
            services.AddSingleton<IMongoClient>(client);
            services.AddSingleton<INoteRepository, DbNoteRepository>();
            services.AddSingleton<IUserRepository, DbUserRepository>();
            services.AddSingleton<Auth.IAuthenticator, Auth.Authenticator>();
            services.AddSingleton<Register.IRegistrator, Register.Registrator>();
        }

        public void Configure(IApplicationBuilder appBuilder)
        {
            appBuilder.UseMvc();
        }
    }
}
