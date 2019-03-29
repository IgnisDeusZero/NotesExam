namespace Notes.API
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Models.Notes.Repositories;
    using Models.Users.Repositories;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton<INoteRepository, MemoryNoteRepository>();
            services.AddSingleton<IUserRepository, MemoryUserRepository>();
            services.AddSingleton<Auth.IAuthenticator, Auth.Authenticator>();
            services.AddSingleton<Register.IRegistrator, Register.Registrator>();
        }

        public void Configure(IApplicationBuilder appBuilder)
        {
            appBuilder.UseMvc();
        }
    }
}
