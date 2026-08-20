using MMLib.SwaggerForOcelot.DependencyInjection;
using MMLib.SwaggerForOcelot.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(
    "ocelot.json",
    optional: false, // application fails to start if ocelit.json is missing
    reloadOnChange: true); // Updates routes dynamically at runtime if the JSON file is modified, without restarting the service

builder.Services.AddOcelot(builder.Configuration); // Registers internal Ocelot services

builder.Services.AddSwaggerForOcelot(builder.Configuration);// Registers downstream Swagger fetching and transformation handlers.

var app = builder.Build();

app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";
});

await app.UseOcelot(); // Intercepts incoming HTTP requests

app.Run(); // Starts the Kestrel web server on the gateway's configured port