using BlazorJwtAuth.Client.Service.Helpers;
using BlazorJwtAuth.Common.Models;

namespace BlazorJwtAuth.Client.Service.Services.Interfaces;

public interface IHomeHttpClientService
{
    Task<ResponseBase> GetIndexAsync(AppSettings appSettings, HttpClient httpClient);
}
