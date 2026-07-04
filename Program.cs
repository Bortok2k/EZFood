using EZFood;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<AuthorizedHttpMessageHandler>();
builder.Services.AddScoped(sp =>
{
    var handler = sp.GetRequiredService<AuthorizedHttpMessageHandler>();
    handler.InnerHandler = new HttpClientHandler();
    return new HttpClient(handler)
    {
        BaseAddress = new Uri("http://localhost:8080")
    };
});
builder.Services.AddScoped<SidebarService>();

await builder.Build().RunAsync();

public class SidebarService
{
    public RenderFragment? SidebarButtons { get; private set; }
    public event Action? OnChange;

    public void SetButtons(RenderFragment buttons)
    {
        SidebarButtons = buttons;
        OnChange?.Invoke();
    }

    public void Clear()
    {
        SidebarButtons = null;
        OnChange?.Invoke();
    }
}