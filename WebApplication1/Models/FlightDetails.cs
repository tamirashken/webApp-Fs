using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace WebApplication1.Models
{
    public class FlightDetails
    {
        public string Lon { get; set; }
        public string Lat { get; set; }
        public string Throttle { get; set; }
        public string Rudder { get; set; }


        public void ToXml(XmlWriter writer)
        {
            writer.WriteElementString("Lon", this.Lon);
            writer.WriteElementString("Lat", this.Lat);
            writer.WriteElementString("Throttle", this.Throttle);
            writer.WriteElementString("Rudder", this.Rudder);
            writer.WriteEndElement();
        }
    }
}