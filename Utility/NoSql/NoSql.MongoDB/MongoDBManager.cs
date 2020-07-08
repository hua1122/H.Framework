using System.Configuration;

namespace NoSql.MongoDB
{
    public class MongoDBManager
    {
        private readonly string _mongoConnectionString;
        private readonly string _mongoDatabase;
        private readonly MongoDBContext _mongoDBContext;

        public MongoDBManager()
        {
            _mongoConnectionString = ConfigurationManager.AppSettings["MongoDBConnectionString"];//mongodb://119.3.45.71:27311,119.3.48.193:27321
            _mongoDatabase = ConfigurationManager.AppSettings["MongoDBName"];//databaseName
            _mongoDBContext = new MongoDBContext(_mongoConnectionString, _mongoDatabase);
        }

    }
}
