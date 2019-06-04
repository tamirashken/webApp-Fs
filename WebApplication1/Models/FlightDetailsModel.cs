using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace WebApplication1.Models
{
    public class FlightDetailsModel
    {
        private FlightDetailsModel()
        {
            
        }

        private static FlightDetailsModel m_Instance = null;

        public static FlightDetailsModel Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new FlightDetailsModel();

                }
                return m_Instance;
            }
        }
        public string Lon { get; set; }
        public string Lat { get; set; }
        public string Throttle { get; set; }
        public string Rudder { get; set; }
        public string fileName { get; set; }
        public string Details { get; set; }
        



        public void ToXml(XmlWriter writer)
        {
            writer.WriteString(",");
            writer.WriteValue(this.Lon);
            writer.WriteString(",");
            writer.WriteValue(this.Lat);
            writer.WriteString(",");
            writer.WriteValue(this.Throttle);
            writer.WriteString(",");
            writer.WriteValue(this.Rudder);
            writer.WriteString(",");
        }

 
        public void appendToXml(string strToAppend)
        {
            string createText = strToAppend + Environment.NewLine;
            string path = HttpContext.Current.Server.MapPath(String.Format(Constants.SCENARIO_FILE, fileName));
            File.WriteAllText(path, createText);
            Details = "";
        }       
    }
}
