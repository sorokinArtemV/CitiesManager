using CitiesManager.Infrastucture.DataBaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));
}).AddXmlSerializerFormatters();

builder.Services.AddApiVersioning(config =>
{
    config.ApiVersionReader = new UrlSegmentApiVersionReader();
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "WebAPI.xml"));
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CitiesManager.WebAPI",
        Version = "1.0"
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "CitiesManager.WebAPI",
        Version = "2.0"
    });
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policyBuilder =>
    {
        policyBuilder.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!);
        policyBuilder.WithHeaders("Authorization", "origin", "content-type", "accept");
        policyBuilder.WithMethods("GET", "POST", "PUT", "DELETE");
        // builder.WithOrigins("*");
    });
    
    options.AddPolicy("4100Client", policyBuilder =>
    {
        policyBuilder.WithOrigins(builder.Configuration.GetSection("AllowedOrigins2").Get<string[]>()!);
        policyBuilder.WithHeaders("Authorization", "origin", "accept");
        policyBuilder.WithMethods("GET");
    });
    
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");
    });
}

app.UseRouting();
app.UseCors();

app.UseHsts();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();