using System;
using MongoDB.Bson;
using MongoDB.Driver;

class DBDriver {
    static void Main(string[] args) {
        Console.WriteLine("Hello World");

        var client = new MongoClient(
            "mongodb+srv://dbUser:friedegg@newworld.wu4zxtz.mongodb.net/?retryWrites=true&w=majority"
        );
        var database = client.GetDatabase("test");

        Console.WriteLine(database);
    }
}