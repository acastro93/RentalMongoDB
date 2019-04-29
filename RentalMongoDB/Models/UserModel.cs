using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RentalMongoDB.Models
{
    public class UserModel
    {

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonRequired]
        [BsonElement("IdNumber")]
        public int IdNumber { get; set; }

        [BsonRequired]
        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonRequired]
        [BsonElement("LastName")]
        public string LastName { get; set; }

        [BsonRequired]
        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonRequired]
        [BsonElement("PhoneNumber")]
        public string PhoneNumber { get; set; }



    }
}