using ITS.TwoPhaseCommit.Coordinator.Consumers;
using ITS.TwoPhaseCommit.Coordinator.Services;
using ITS.TwoPhaseCommit.Models;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddMassTransit(
    mt =>
    {
        mt.AddConsumer<ProposeResponseConsumer>();
        mt.AddConsumer<AckConsumer>();
        
        mt.UsingRabbitMq((context, config) =>
        {
            config.ReceiveEndpoint("propose-response-endpoint", e =>
            {
                e.ConfigureConsumer<ProposeResponseConsumer>(context);
            });
            config.ReceiveEndpoint("ack-endpoint", e =>
            {
                e.ConfigureConsumer<AckConsumer>(context);
            });
            
            config.Host(
                "localhost",
                "/",
                credentials =>
                {
                    credentials.Username("admin");
                    credentials.Password("password");
                }
            );

            config.ConfigureEndpoints(context);
        });
    }
);
builder.Services.AddMassTransitHostedService();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITransactionManager<CustomerOrder>, CustomerOrderTransactionManager>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();