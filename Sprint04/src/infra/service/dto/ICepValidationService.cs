namespace Sprint03.infra.service.dto
{
    public interface ICepValidationService
    {
        Task<bool> IsValidCepAsync(string cep);
    }
}