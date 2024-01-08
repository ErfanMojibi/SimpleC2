namespace TeamServer.Models.Listeners
{
    public class HttpListener : Listener
    {
        public override string Name { get; }
        public int BindPort { get; }

        private CancellationTokenSource _tokenSource;

        public HttpListener(string name, int bindPort)
        {
            Name = name;
            BindPort = bindPort;
        }
        public override Task Start()
        {
            var hostBuilder = new HostBuilder().ConfigureWebHostDefaults(host =>
            {
                host.UseUrls($"http://0.0.0.0:{BindPort}");
                host.Configure(ConfigureApp);
                host.ConfigureServices(ConfigureServices);

            });

            var host = hostBuilder.Build();

            _tokenSource = new CancellationTokenSource();
            var task = host.RunAsync(_tokenSource.Token); // run as long as token is not canceled

            return task;

        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddControllers(); //????
            serviceCollection.AddSingleton(AgentService);
        }

        private void ConfigureApp(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("/", "/", new { controller = "HttpListener", action = "HandleImplant" });
            });

        }

        public override void Stop()
        {
            _tokenSource.Cancel();
        }
    }
}
