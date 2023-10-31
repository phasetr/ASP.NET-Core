using System.Net;
using System.Net.Http.Json;
using Common.Models;
using Microsoft.AspNetCore.Components;

namespace Client.Shared;

public partial class DataForm<TModel>
    where TModel : IModel, new()
{
    private string _errorMessage = string.Empty;

    [Inject] private HttpClient Http { get; set; } = null!;

    [Inject] private NavigationManager Navigation { get; set; } = null!;

    [Parameter] [EditorRequired] public string ApiPath { get; set; } = string.Empty;

    [Parameter] [EditorRequired] public int Id { get; set; }

    [Parameter] public RenderFragment<TModel> ChildContent { get; set; } = null!;

    public TModel Model { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        await GetModel();
    }

    private async Task GetModel()
    {
        Model = await Http.GetFromJsonAsync<TModel>($"rest/{ApiPath}/{Id}") ?? new TModel();
    }

    private async Task SaveItem()
    {
        var response = Id <= 0
            ? await Http.PostAsJsonAsync($"rest/{ApiPath}", Model)
            : await Http.PutAsJsonAsync($"rest/{ApiPath}/{Id}", Model);

        if (response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Created)
                if (response.Headers.TryGetValues("location", out var urls))
                    Navigation.NavigateTo(urls.First(), replace: true);

            await GetModel();
        }
        else
        {
            _errorMessage = await response.Content.ReadAsStringAsync();
        }
    }
}
