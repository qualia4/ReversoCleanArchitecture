namespace Reverso.Infrastructure;
using MongoDB.Driver;
using Domain.Web;

public class MongoDbContext
{
    public IMongoDatabase Database { get; }

    public MongoDbContext(string connectionString, string dbName)
    {
        var client = new MongoClient(connectionString);
        Database = client.GetDatabase(dbName);
    }

    public IMongoCollection<User> Users => Database.GetCollection<User>("users");
}