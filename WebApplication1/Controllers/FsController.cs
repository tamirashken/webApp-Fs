﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class FsController : Controller
    {
        // GET: Fs
        public ActionResult Index()
        {
            return View();
        }

        // GET: display
        [HttpGet]
        public ActionResult display(string ip, string port)
        {
            //  double lon, lat;
            // FlightManagerModel.Instance.connect(ip, port);
            // lon =FlightManagerModel.Instance.getInfo("lon");
            // lat = FlightManagerModel.Instance.getInfo("lat");
            return View();
        }

        // GET: display
        [HttpGet]
        public ActionResult displayLines(string ip, string port, string time)
        {
            
            //  double lon, lat;
            // FlightManagerModel.Instance.connect(ip, port);
            // lon =FlightManagerModel.Instance.getInfo("lon");
            // lat = FlightManagerModel.Instance.getInfo("lat");
            return View();
        }
    }
}