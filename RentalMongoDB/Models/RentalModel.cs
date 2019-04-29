using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentalMongoDB.Models
{
    public class RentalModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonRequired]
        [BsonElement("Plate")]
        public int Plate { get; set; }

        [BsonRequired]
        [BsonElement("IdNumber")]
        public int IdNumber { get; set; }

        [BsonRequired]
        [BsonElement("RentalDays")]
        public int RentalDays { get; set; }

        [BsonRequired]
        [BsonElement("Cost")]
        public int Cost { get; set; }

                          
    }
}