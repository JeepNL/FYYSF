using FYYSF;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton(services =>
{
	IConfiguration? configuration = services.GetRequiredService<IConfiguration>();
	string? backendUrl = $"https://{configuration["Settings:BackEndUrl"]}";

	// Create a channel with a GrpcWebHandler that is addressed to the backend server.
	// GrpcWebText is used because server streaming requires it. If server streaming is not used in your app
	// then GrpcWeb is recommended because it produces smaller messages.
	GrpcWebHandler httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
	return GrpcChannel.ForAddress(backendUrl, new GrpcChannelOptions
	{
		HttpHandler = httpHandler
		//MaxReceiveMessageSize = 1 * 1024 * 1024, // 1MB
		//MaxSendMessageSize = 1 * 1024 * 1024, // 1MB
		//MaxRetryAttempts = 3 // ?? #TODO
	});
});

await builder.Build().RunAsync();
