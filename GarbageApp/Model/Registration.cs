using System;
using System.Collections.Generic;
using System.Text;

namespace GarbageApp.Model
{
    public class Registration
    {
        public Guid GarbageId { get; set; }
        public string Description { get; set; }
        public string Street { get; set; }
        public Guid CategoryId { get; set; }
        public Guid CityId { get; set; }
        public int Amount { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public Guid PersonId { get; set; }
        public DateTime Date { get; set; }
    }
}
