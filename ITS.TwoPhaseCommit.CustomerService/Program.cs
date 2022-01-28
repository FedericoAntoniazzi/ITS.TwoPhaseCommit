using MassTransit;
using ITS.TwoPhaseCommit.CustomerService;
using ITS.TwoPhaseCommit.CustomerService.Consumers;
using IHost = Microsoft.Extensions.Hosting.IHost;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        services =>
        {
            services.AddHostedService<Worker>();
            services.AddMassTransit(mt =>
            {
                mt.AddConsumer<ProposeCustomerUpdateConsumer>();
                mt.AddConsumer<CommitConsumer>();
                
                mt.UsingRabbitMq((context, config) =>
                {
                    config.Host(
                        "localhost",
                        "/",
                        credentials =>
                        {
                            credentials.Username("admin");
                            credentials.Password("password");
                        }
                    );
                    config.ReceiveEndpoint("propose-customer-update", e =>
                    {
                        e.ConfigureConsumer<ProposeCustomerUpdateConsumer>(context);
                    });
                    config.ReceiveEndpoint("commit", e =>
                    {
                        e.ConfigureConsumer<CommitConsumer>(context);
                    });
                    config.ConfigureEndpoints(context);
                });
            });
            services.AddMassTransitHostedService();
        })
    .Build();

await host.RunAsync();