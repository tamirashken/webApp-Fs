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
                return RedirectToAction("DisplayFromFile", new { fileName = ip, rate = port });
            } else
            {
                int portInt = 0;
                if (port != null && ip != null)
                {
                    portInt = Int32.Parse(port);
                }
               if(!FlightManagerModel.Instance.connect(ip, portInt))
                {
                    return RedirectToAction("Error");
                }
                saveSessionsLonLat();
                FlightManagerModel.Instance.disconnectClient();

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
                //FlightManagerModel.Instance.disconnectClient();
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
            //FlightManagerModel.Instance.createFile(fileName);
            Session["time"] = freq;
            Session["totalSec"] = totalSec;
            return View();
        }

        /*
       * The ActionResults directs to "FlightDetailsFile" view, which displays the plane route on the map
       * given his coordinates from given file name, each given number of seconds
       */
        [HttpGet]
        public ActionResult DisplayFromFile(string fileName, string rate)
        {
            FlightManagerModel.Instance.disconnectClient();
            var infoFileModel = FlightManagerModel.Instance;
            infoFileModel.Read(fileName);
            ViewBag.FirstLon = FlightManagerModel.Instance.getNextLon();
            ViewBag.FirstLat = FlightManagerModel.Instance.getNextLat();
            ViewBag.rate = rate;
            
            return View("DisplayFromFile");
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
            writer.WriteStartElement("line");
            flightDet.ToXml(writer);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            return sb.ToString();
        }

        public string stringToXml(string coordinates)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("coordinates");
            // split the string by spaces.
            string[] temp = coordinates.Split(' ');
            // fill values to fields in XML.
            writer.WriteElementString("Lon", temp[0]);
            writer.WriteElementString("Lat", temp[1]);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            // stringify the XML.
            return sb.ToString();
        }

        [HttpPost]
        public string loadDetailsPost()
        {
            string Lon = FlightManagerModel.Instance.getNextLon();
            string Lat = FlightManagerModel.Instance.getNextLat();
            return stringToXml(Lon + " " + Lat);
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
        }

        [HttpPost]
        public string saveAll()
        {
            FlightDetailsModel.Instance.appendToXml(FlightDetailsModel.Instance.Details);
            return "flight details saved";
        }
    }
}