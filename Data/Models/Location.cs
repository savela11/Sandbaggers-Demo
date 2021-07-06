namespace Data.Models
{
    public class Location
    {
        public string Name { get; set; } = "";
        public string StreetNumbers { get; set; } = "";
        public string StreetName { get; set; } = "";
        public string State { get; set; } = "";
        public string City { get; set; } = "";
        public string PostalCode { get; set; } = "";
    }



    // public class Location : ValueObject
    // {
    //     public string Name { get; set; }
    //     public string StreetNumbers { get; set; }
    //     public string StreetName { get; set; }
    //     public string State { get; set; }
    //     public string City { get; set; }
    //     public string PostalCode { get; set; }
    //
    //     private Location()
    //     {
    //     }
    //
    //     public Location(string name, string streetNumbers, string streetName, string state, string city, string postalCode)
    //     {
    //         Name = name;
    //         StreetNumbers = streetNumbers;
    //         StreetName = streetName;
    //         State = state;
    //         City = city;
    //         PostalCode = postalCode;
    //     }
    //
    //
    //     public override string ToString()
    //     {
    //         return $"{StreetNumbers} {StreetName}, {City}, {State} {PostalCode}";
    //     }
    //
    //     protected override IEnumerable<object> GetAtomicValues()
    //     {
    //         // Using a yield return statement to return each element one at a time
    //         yield return Name;
    //         yield return StreetNumbers;
    //         yield return StreetName;
    //         yield return State;
    //         yield return City;
    //         yield return PostalCode;
    //     }
    // }
}
