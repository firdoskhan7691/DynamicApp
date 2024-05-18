using DynamicApplicationCP.Interfaces;
using DynamicApplicationCP.Services;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add controllers
        services.AddControllers();

        // Add Swagger for API documentation
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Register custom services
        services.AddSingleton<CosmosDBService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IProgramService, ProgramService>();
        services.AddScoped<ICandidateService, CandidateService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            // Enable Swagger in development mode
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Enforce HTTPS
        app.UseHttpsRedirection();

        // Authorization middleware
        app.UseAuthorization();

        // Routing middleware
        app.UseRouting();

        // Map controller endpoints
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
