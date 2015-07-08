using System.Dynamic;
using ElasticSearch.Client.Query.Utils;
using ElasticSearch.Client.Utils;

namespace ElasticSearch.Client.Query.QueryGenerator.AggregationComponents.Aggregates
{
    public class AggregateComponentBase : IAggregateComponent
    {
        private readonly string _aggregateOperationName;
        protected readonly ComponentBag Components = new ComponentBag();

        public AggregateComponentBase(string aggregateOperationName)
        {
            _aggregateOperationName = aggregateOperationName;
        }

        public ExpandoObject BuildRequestComponent()
        {
            if (Components.Count == 0)
                return null;

            ExpandoObject requestComponent = new ExpandoObject();
            requestComponent.Add(_aggregateOperationName, Components);

            return requestComponent;
        }

        protected string Field
        {
            get { return Components.Get<string>("field"); }
            set { Components.Set("field", value); }
        }

        public string Script
        {
            get { return Components.Get<string>("script"); }
            set { Components.Set("script", value); }
        }

        public int? Size
        {
            get { return Components.Get<int?>("size"); }
            set { Components.Set("size", value); }
        }

    }
}