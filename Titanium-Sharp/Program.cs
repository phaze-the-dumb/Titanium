using System.Net;
using Titanium.Logic;
using Titanium.Net.Structs;

using MongoDB.Driver;
using MongoDB.Bson;
using Titanium.Database.Structs;

internal class Program
{
    private static void Main()
    {
        Titanium.Main titanium = new(Titanium.Vars.Port);
    
        // MongoClient client = new("mongodb://127.0.0.1:27017");
        // var coll = client.GetDatabase("Hostdustry").GetCollection<Server>("Servers");

        titanium.OnPlayerJoin += ( Player player ) =>
        {
            // string serverConnId = player.Name.Split(":")[1];
            // var server = coll.Find(Builders<Server>.Filter.Eq("connection_id", serverConnId)).First();
      
            player.JoinTo(new Address(IPAddress.Parse("127.0.0.1"), 57018));
        };

        titanium.OnPlayerLeave += ( Player player ) => {

        };
    }
}