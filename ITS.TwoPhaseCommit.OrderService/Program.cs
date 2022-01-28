using MassTransit;
using ITS.TwoPhaseCommit.OrderService;
using ITS.TwoPhaseCommit.OrderService.Consumers;
using IHost = Microsoft.Extensions.Hosting.IHost;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(
        services =>
        {
            services.AddHostedService<Worker>();
            services.AddMassTransit(mt =>
            {
                mt.AddConsumer<CommitConsumer>();
                mt.AddConsumer<ProposeOrderCreateConsumer>();

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
                    config.ReceiveEndpoint(
                        "propose-order-create",
                        e => { e.ConfigureConsumer<ProposeOrderCreateConsumer>(context); });
                    config.ReceiveEndpoint(
                        "commit",
                        e => { e.ConfigureConsumer<CommitConsumer>(context); });
                    config.ConfigureEndpoints(context);
                });
            });
            services.AddMassTransitHostedService();
        })
    .Build();

await host.RunAsync();