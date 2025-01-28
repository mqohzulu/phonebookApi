using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using phonebookApi.Middileware;
using ContactManagement.Infrastructure.Persistance;
using ContactManagement.Infrastructure.Extentions;
using ContactManagement.Application;
using ContactManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

{
    builder.Services.AddControllers().AddApiResultFilter();

    builder.Services.AddApplication()
        .AddInfrastructure(builder.Configuration);


    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigins",
            policy => policy
                .WithOrigins(
                    builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
                    ?? Array.Empty<string>())
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
    });
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Phonebook API",
            Version = "v1",
            Description = "simple phonebook API for managing contacts"
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

}

var app = builder.Build();

// Configure the HTTP request pipeline
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/error");
        app.UseHsts();
    }

    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseRequestLogging();

    app.UseHttpsRedirection();

    app.UseCors("AllowSpecificOrigins");

    // Authentication & Authorization
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHealthChecks("/health");

    // Apply migrations
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<PhonebookContext>();
        context.Database.Migrate();
    }
}

app.Run();
