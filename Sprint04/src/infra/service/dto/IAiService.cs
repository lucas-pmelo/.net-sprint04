using Microsoft.ML;
using Microsoft.ML.Data;
using Sprint03.domain.model;

namespace Sprint03.infra.service.dto
{
    public interface IAiService
    {
        List<string> RecommendProducts(Customer customer);
        string AnalyzeSentiment(string text);
    }
}