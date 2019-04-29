using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using System.Configuration;


namespace RentalMongoDB.App_Start
{
    public class MongoDBContext
    {
        MongoClient client;
        MongoServer server;
        public MongoDatabase db;

        public MongoDBContext()
        {
            var MongoDatabaseName = ConfigurationManager.AppSettings["MongoDatabaseName"];
            var MongoPort = ConfigurationManager.AppSettings["MongoPort"];  //27017  
            var MongoHost = ConfigurationManager.AppSettings["MongoHost"];  //localhost  

            // Creating MongoClientSettings  
            var settings = new MongoClientSettings
            {
                Server = new MongoServerAddress(MongoHost, Convert.ToInt32(MongoPort))
            };
            client = new MongoClient(settings);
            server = client.GetServer();
            db = server.GetDatabase(MongoDatabaseName);

        }
    }
}