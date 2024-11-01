using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Sprint03.infra.service.dto;

public class CepValidationService : ICepValidationService
{
    private readonly HttpClient _httpClient;

    public CepValidationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> IsValidCepAsync(string cep)
    {
        var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
        if (!response.IsSuccessStatusCode) return false;

        var content = await response.Content.ReadAsStringAsync();
        var json = JObject.Parse(content);
        return json["erro"] == null;
    }
}