using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RentalMongoDB.Models
{
    public class VehicleModel
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonRequired]
        [BsonElement("Plate")]
        public int Plate { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("TankCapacity")]
        public int? TankCapacity { get; set; }

        [BsonRequired]
        [BsonElement("Brand")]
        public string Brand { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("Style")]
        public string Style { get; set; }

        [BsonRequired]
        [BsonElement("Model")]
        public string Model { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("Color")]
        public string Color { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("HorsePower")]
        public int? HorsePower { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("Fuel")]
        public string Fuel { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("Transmition")]
        public string Transmition { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("Year")]
        public int? Year { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("Extras")]
        public string Extras { get; set; }

        [BsonIgnoreIfNull]
        [BsonElement("PassengerCap")]
        public int? PassengerCap { get; set; }

        [BsonRequired]
        [BsonElement("RentalPrice")]
        public int? RentalPrice { get; set; }

        [BsonRequired]
        [BsonElement("State")]
        public string State { get; set; }





    }
}