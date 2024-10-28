using RobotAssignment.Services;

namespace RobotAssignment.Configuration
{
    public static class InjectServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IRobotService, RobotService>();

            return services;
        }
    }
}
