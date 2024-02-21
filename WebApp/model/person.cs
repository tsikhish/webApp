using System;
using System.Net.Sockets;

namespace webApp.model
{
    public class person
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobPosition { get; set; }
        public double Salary { get; set; }
        public double WorkExperience { get; set; }
        public address PersonAddress { get; set; }
    }
    public class address
    {
        public string Country { get; set; }
        public string City { get; set; }
        public int HomeNumber { get; set; }
    }
}
