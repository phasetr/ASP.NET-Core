using System.Net.Http.Json;
using MediaLibrary.Client.Shared.Model;
using MediaLibrary.Common.Models;
using Microsoft.AspNetCore.Components;

namespace MediaLibrary.Client.Shared;

public partial class DataView<TItem>
    where TItem : IModel, new()
{
    [Inject] public NavigationManager Navigation { get; set; } = null!;

    [Inject] public HttpClient Http { get; set; } = null!;

    [Parameter] [EditorRequired] public string ApiPath { get; set; } = string.Empty;

    public Table<TItem> Data { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        var type = typeof(TItem);
        Data.Columns = type.GetProperties().Select(x => new TableColumn {Name = x.Name, PropertyInfo = x});

        var model = await Http.GetFromJsonAsync<IEnumerable<TItem>>($"/rest/{ApiPath}/list") ?? new List<TItem>();

        foreach (var item in model)
        {
            var row = new TableRow<TItem>(item);

            foreach (var column in Data.Columns)
            {
                var value = column.PropertyInfo.GetValue(item);
                row.Values.Add(new TableCell {Value = value});
            }

            Data.Rows.Add(row);
        }
    }

    public string GetDetailUrl(int id)
    {
        return $"{Navigation.ToAbsoluteUri(Navigation.Uri).LocalPath}/{id}";
    }
}
