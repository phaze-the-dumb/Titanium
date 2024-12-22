using MongoDB.Bson.Serialization.Attributes;

namespace Titanium.Database.Structs;

public class Server
{
  [BsonElement("_id")] public string id;
  [BsonElement("internal_port")] public int port;
  [BsonElement("connection_id")] public string connId;
}