using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Xml;
using System.Text;
using System.Net;

namespace WebApplication1.Controllers
{
    public class FsController : Controller
    {
        // GET: Fs
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // GET: display
        [HttpGet]
        public ActionResult display(string ip, string port)
        {
            IPAddress address;
            //invalid ip adress
            if (!IPAddress.TryParse(ip, out address))
            {
                //go to another function of load
            } else
            {
                int portInt = 0;
                if (port != null && ip != null)
                {
                    portInt = Int32.Parse(port);
                }
                FlightManagerModel.Instance.connect(ip, portInt);
                saveSessionsLonLat();
                
            }
            return View();

        }

        // GET: displayLines
        [HttpGet]
        public ActionResult displayLines(string ip, string port, string freq)
        {
            int portInt = 0;
            if (port != null && ip != null)
            {
                portInt = Int32.Parse(port);
                FlightManagerModel.Instance.connect(ip, portInt);
                saveSessionsLonLat();
            }
            Session["time"] = freq;
            return View();
        }


        public void saveSessionsLonLat()
        {
            Session["lon"] = FlightManagerModel.Instance.Lon;
            Session["lat"] = FlightManagerModel.Instance.Lat;
        }

        [HttpPost]
        public void GetLonLat()
        {
            saveSessionsLonLat();
        }

        private string ToXml(FlightDetails flightDet)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("Your Flight Details:");
            flightDet.ToXml(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }


        [HttpPost]
        public string saveFlightDetails(string fileName)
        {
            FlightManagerModel.Instance.writeFlightDetails(fileName);
            FlightManagerModel.Instance.newFlightDetails();
            return ToXml(FlightManagerModel.Instance.FlightDetails);
        }
    }
}