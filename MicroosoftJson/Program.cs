using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MicrosoftJson {
    class Program {

        void Run() {
            DeserializeAppConfig();
            SerializeDeserializeExample();
            JsonReaderExample();
            JsonDocumentExample();
            Console.ReadKey();
        }

        private void DeserializeAppConfig() {
            var json = System.IO.File.ReadAllText(@"C:\repos\MicrosoftJson\MicroosoftJson\AppConfig.json");
            var appConfig = JsonSerializer.Deserialize<AppConfig>(json);
        }

        /// <summary>
        /// Serialize turns a class instance into JSON
        /// Deserialize turns JSON into a class instance
        /// 
        /// This is done automatically with WebAPI controller methods
        /// </summary>
        void SerializeDeserializeExample() {
            var cust = new Customer { Id = 1, Name = "Amazon", Sales = 1000000, Active = true };
            Console.WriteLine(cust.Serialize());

            var cust2json = "{ \"Id\" : 2, \"Name\" : \"WS\", \"Sales\" : 1000000 }";
            var cust2 = Customer.Deserialize(cust2json.ToString());
            Console.WriteLine(cust2.Serialize());
        }
        /// <summary>
        /// The JsonReader is a forward-only, sequential reader. It just reads everything
        /// inside the JSON file.
        /// </summary>
        void JsonReaderExample() {

            var appJson = "{ \"AppProperties\" : { \"ConnectionString\": \"server=localhost:12345;database=AppDb;trusted_connection=true;\" }}";
            byte[] data = Encoding.UTF8.GetBytes(appJson.ToArray());
            Utf8JsonReader reader = new Utf8JsonReader(data, isFinalBlock: true, state: default);
            while(reader.Read()) {
                Console.WriteLine(reader.TokenType);
                switch(reader.TokenType) {
                    case JsonTokenType.PropertyName:
                    case JsonTokenType.String:
                        var text = reader.GetString();
                        Console.Write($" {text}");
                        break;
                    case JsonTokenType.Number:
                        var nbr = reader.GetInt32();
                        Console.Write($" {nbr}");
                        break;
                }
                Console.WriteLine();
            }
        }
        /// <summary>
        /// The JsonDocument allows processing of all or part of a JSON file.
        /// This example shows getting a connection string from a config file
        /// in JSON format that looks like this:
        /// 
        /// {
        ///     AppProperties: {
        ///         ConnectionString: "server=localhost:12345;database=AppDb;trusted_connection=true"
        ///     }
        /// }
        /// </summary>
        void JsonDocumentExample() {

            var appJson = "{ \"AppProperties\" : { \"ConnectionString\": \"server=localhost:12345;database=AppDb;trusted_connection=true;\" }}";
            using(JsonDocument doc = JsonDocument.Parse(appJson)) {
                var root = doc.RootElement;
                var appProp = root.GetProperty("AppProperties");
                var connStr = appProp.GetProperty("ConnectionString").GetString();
                Console.WriteLine($"connStr is [{connStr}]");
            }
            
        }
        static void Main(string[] args) {
            (new Program()).Run();
        }
    }
}
