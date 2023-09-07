using Microsoft.JSInterop;

namespace BlazorJwtAuth.Client.Service.Helpers;

public class CookieStorageAccessor
{
    private readonly IJSRuntime _jsRuntime;
    private Lazy<IJSObjectReference> _accessorJsRef = new();

    public CookieStorageAccessor(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    private async Task WaitForReference()
    {
        if (_accessorJsRef.IsValueCreated is false)
            _accessorJsRef =
                new Lazy<IJSObjectReference>(
                    await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/CookieStorageAccessor.js"));
    }

    public async ValueTask DisposeAsync()
    {
        if (_accessorJsRef.IsValueCreated) await _accessorJsRef.Value.DisposeAsync();
    }

    public async Task<T> GetAsync<T>(string key)
    {
        await WaitForReference();
        var result = await _accessorJsRef.Value.InvokeAsync<T>("get", key);
        return result;
    }

    public async Task<T> GetAllAsync<T>(string key)
    {
        await WaitForReference();
        var result = await _accessorJsRef.Value.InvokeAsync<T>("getAll", key);
        return result;
    }

    public async Task SetValueAsync<T>(string key, T value)
    {
        await WaitForReference();
        await _accessorJsRef.Value.InvokeVoidAsync("set", key, value);
    }
}
