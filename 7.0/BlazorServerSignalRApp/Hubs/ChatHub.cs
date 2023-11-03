using BlazorServerSignalRApp.Data;
using Microsoft.AspNetCore.SignalR;

namespace BlazorServerSignalRApp.Server.Hubs;

public class ChatHub : Hub
{
    private static string? lastConnected;
    public override Task OnConnectedAsync()
    {
        lastConnected = Context.ConnectionId;
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        if (lastConnected != Context.ConnectionId)
        {
            lastConnected = null;
        }
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task WaitMessage()
    {
        var clientId = lastConnected ?? Context.ConnectionId;
        var message = await Clients.Client(clientId).InvokeAsync<string>("GetMessage", CancellationToken.None);
        Console.WriteLine($"Got message {message}");
    }

    public async Task<string> WaitMessageWithPrompt(string prompt)
    {
        var clientId = lastConnected ?? Context.ConnectionId;
        var message = await Clients.Client(clientId).InvokeAsync<string>("GetMessageWithPrompt", $"Input {prompt}", CancellationToken.None);
        Console.WriteLine($"Got echo {message}");
        return message;
    }

    public async Task ComplexMessage()
    {
        var clientId = lastConnected ?? Context.ConnectionId;
        var request = new ComplexSampleRequest("Test title", "Test description",
            new List<TestItem> { new(777, "Tri topora"), new(44, "Sorok cetverty") });
        var response = await Clients.Client(clientId).InvokeAsync<ComplexSampleResponse>("ComplexPrompt", request, CancellationToken.None);
        Console.WriteLine($"Got echo {response}");
    }
}
