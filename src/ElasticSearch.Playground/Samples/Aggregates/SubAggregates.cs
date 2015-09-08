using ElasticSearch.Client.ElasticSearch.Results;
using ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates;
using ElasticSearch.Client.Utils;
using NUnit.Framework;

namespace ElasticSearch.Playground.Samples.Aggregates
{
    [TestFixture]
    public class SubAggregates : TestBase
    {
        [Test]
        public void HeavySubAggregating()
        {
            var histogramAggregate = new DateHistogramAggregate("@timestamp","1d");
            var histogramSubAggregate = new SubAggregate(histogramAggregate);
            
            var totalSoldCarsAggregate = new ValueCountAggregate("TotalSoldCars");

            var countryTermsAggregate = new TermsAggregate("Country");
            var countrySubAggregate = new SubAggregate(countryTermsAggregate);
            countrySubAggregate.Aggregates.Add("TotalSoldCars", totalSoldCarsAggregate);

            var carTypeTermsAggregate = new TermsAggregate("Type");
            var carTypeSubAggregate = new SubAggregate(carTypeTermsAggregate);
            carTypeSubAggregate.Aggregates.Add("TotalSoldCarsOfType", totalSoldCarsAggregate);

            histogramSubAggregate.Aggregates.Add("CountrySubAggr", countrySubAggregate);

            countrySubAggregate.Aggregates.Add("CarTypeSubAggr",carTypeSubAggregate);


            QueryBuilder.Aggregates.Add("HistogramSubAggr", histogramSubAggregate);

            QueryBuilder.PrintQuery();

            AggregateResult result = Client.ExecuteAggregate(QueryBuilder);
            result.PrintResult();
        } 

    }
}