using Microsoft.ML;
using Sprint03.domain.model;
using Sprint03.infra.service.dto;

namespace Sprint03.infra.service
{
    public class AiService : IAiService
    {
        private readonly MLContext _mlContext;

        public AiService()
        {
            _mlContext = new MLContext();
        }

        public List<string> RecommendProducts(Customer customer)
        {
            var products = new List<string> { "Produto A", "Produto B", "Produto C" };


            return products.Take(3).ToList();
        }

        public string AnalyzeSentiment(string text)
        {
            var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text))
                           .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

            var data = new List<SentimentData> { new SentimentData { Text = text, Label = false } };
            var trainingData = _mlContext.Data.LoadFromEnumerable(data);

            var model = pipeline.Fit(trainingData);

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
            var prediction = predictionEngine.Predict(new SentimentData { Text = text });

            return prediction.Prediction ? "Positive" : "Negative";
        }
    }

    public class SentimentData
    {
        public bool Label { get; set; }
        public string Text { get; set; }
    }

    public class SentimentPrediction : SentimentData
    {
        public float Score { get; set; }
        public bool Prediction { get; set; }
    }
}
