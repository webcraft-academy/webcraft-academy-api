namespace webcnAPI.Domain;
using MongoDB.Bson;
﻿using MongoDB.Bson.Serialization.Attributes;

public class Ninja {


    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
     public string Id { get; set; }

 
    public string UserName { get; set; }
    public string Email { get; set; } 
    public string PasswordHash { get; set; } 
  
    public string Github { get; set; }
    public string Stack { get; set; } 
    public string Slack { get; set; } 
    public string Dev { get; set; } 
    public string Youtube { get; set; } 
    public int Grade { get; set; } 
   
}

