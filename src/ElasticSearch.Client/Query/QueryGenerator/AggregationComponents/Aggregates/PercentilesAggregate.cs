namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class PercentilesAggregate : AggregateComponentBase
    {
        public PercentilesAggregate(string field, double[] percents = null)
            : base("percentiles")
        {
            Field = field;
            Percents = percents;
        }

        public double[] Percents
        {
            get
            {
                return Components.Get<double[]>("percents");
            }

            set
            {
                Components.Set("percents", value);
            }
        }
    }
}