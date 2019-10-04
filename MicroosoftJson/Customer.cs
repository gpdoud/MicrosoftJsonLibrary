using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace MicrosoftJson {


    ///<summary>
    /// One of the differences between Newtonsoft.Json and System.Text.Json
    /// is that Microsoft's library does not operate on fields like Customer.Active.
    /// It only operates on properties like Id, Name, and Sales. When run the 
    /// serialize does not output the Active field value.
    ///</summary>
    class Customer {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Sales { get; set; }
        public bool Active = true; // Microsoft's JSON lib doesn't serialize fields like this

        public string Serialize() {
            return JsonSerializer.Serialize(this, typeof(Customer),
                                            new JsonSerializerOptions { WriteIndented = true });
        }
        public static Customer Deserialize(string json) {
            return JsonSerializer.Deserialize(json, typeof(Customer)) as Customer;
        }
        public override string ToString() {
            return $"Customer: id[{Id}],name[{Name}],sales[{Sales}]";
        }
    }
}
