using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

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
            int portInt = 0;
            if (port != null && ip != null)
            {
                portInt = Int32.Parse(port);
            }
            FlightManagerModel.Instance.connect(ip, portInt);
            saveSessionsLonLat();
            return View();
        }

        // GET: displayLines
        [HttpGet]
        public ActionResult displayLines(string ip, string port, string seconds)
        {
            int portInt = 0;
            if (port != null && ip != null)
            {
                portInt = Int32.Parse(port);
            }
            FlightManagerModel.Instance.connect(ip, portInt);
            saveSessionsLonLat();
            Session["time"] = seconds;
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
    }
}