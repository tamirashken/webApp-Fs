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

        // GET: displayLines
        [HttpGet]
        public ActionResult saveFlightDetails(string ip, string port, string freq, string totalSec, string fileName)
        {
            int portInt = 0;
            if (port != null && ip != null)
            {
                portInt = Int32.Parse(port);
                FlightManagerModel.Instance.connect(ip, portInt);
                saveSessionsLonLat();
            }
            FlightDetailsModel.Instance.fileName = fileName;
            FlightManagerModel.Instance.createFile(fileName);
            Session["time"] = freq;
            Session["totalSec"] = totalSec;
            //Session["fileName"] = fileName.ToString();
            return View();
        }

        private void saveSessionsLonLat()
        {
            Session["lon"] = FlightManagerModel.Instance.Lon;
            Session["lat"] = FlightManagerModel.Instance.Lat;
        }

        [HttpPost]
        public void GetLonLat()
        {
            saveSessionsLonLat();
        }


        private string ToXml(FlightDetailsModel flightDet)
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("details");
            flightDet.ToXml(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }


        [HttpPost]
        public void saveDetailsPost(string fileName)
        {
            FlightDetailsModel.Instance.Lon = FlightManagerModel.Instance.Lon.ToString();
            FlightDetailsModel.Instance.Lat = FlightManagerModel.Instance.Lat.ToString();
            FlightDetailsModel.Instance.Throttle = FlightManagerModel.Instance.Throttle.ToString();
            FlightDetailsModel.Instance.Rudder = FlightManagerModel.Instance.Rudder.ToString();
            string s = ToXml(FlightDetailsModel.Instance);
            if (FlightDetailsModel.Instance.Details != null)
            {
                s = s.Remove(0, s.IndexOf('>') + 1);
            }
            FlightDetailsModel.Instance.Details += s;
            //Console.WriteLine("\n\n\n in func \n\n\n");
            //saveSessionsLonLat();
            //FlightManagerModel.Instance.writeFlightDetails();
            //FlightManagerModel.Instance.newFlightDetails();
            //return s;
        }

        [HttpPost]
        public string saveAll()
        {
            FlightDetailsModel.Instance.appendToXml(FlightDetailsModel.Instance.Details);
            return "flight details saved";
        }
    }
}