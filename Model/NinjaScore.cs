namespace webcnAPI.Domain;
using MongoDB.Bson;
﻿using MongoDB.Bson.Serialization.Attributes;

public class NinjaScore
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    public ObjectId NinjaId { get; set; }
    public int Score { get; set; }
    // You can include additional properties, like the date the score was earned.
}