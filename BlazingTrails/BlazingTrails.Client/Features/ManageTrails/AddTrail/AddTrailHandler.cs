﻿using System.Net.Http.Json;
using BlazingTrails.Shared.Features.ManageTrails.AddTrail;
using MediatR;

namespace BlazingTrails.Client.Features.ManageTrails.AddTrail;

public class AddTrailHandler : IRequestHandler<AddTrailRequest, AddTrailRequest.Response>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AddTrailHandler(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<AddTrailRequest.Response> Handle(AddTrailRequest request, CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient("SecureAPIClient");
        var response = await client.PostAsJsonAsync(AddTrailRequest.RouteTemplate, request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var trailId = await response.Content.ReadFromJsonAsync<int>(cancellationToken: cancellationToken);
            return new AddTrailRequest.Response(trailId);
        }

        return new AddTrailRequest.Response(-1);
    }
}
