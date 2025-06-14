namespace Infrastructure.Model {
    public class QueryResult<T> {
        public IList<T> resultAsObject { get; set; }
        public string resultAsString { get; set; }
    }
}
