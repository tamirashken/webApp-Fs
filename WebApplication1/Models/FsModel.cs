using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;



static class Constants
{
    public const string LON_STRING = "get /position/longitude-deg \r\n";
    public const string LAT_STRING = "get /position/latitude-deg \r\n";
    public const string THROTTLE_STRING = "get /controls/engines/current-engine/throttle \r\n";
    public const string RUDDER_STRING = "get /controls/flight/rudder \r\n";
    public const string SCENARIO_FILE = "~/FlightDetails/{0}.xml";           // The Path of the Secnario
}
namespace WebApplication1.Models
{
    //should be singletone
    public class FlightManagerModel
    {
        Client client;
        bool isClientConnected;
        #region Singleton
        private FlightManagerModel()
        {
            this.client = new Client();
            this.isClientConnected = false;
        }

        private static FlightManagerModel m_Instance = null;

        public static FlightManagerModel Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new FlightManagerModel();

                }
                return m_Instance;
            }
        }

        
        #endregion
        #region Properties
        /**
         *  
         */
        public FlightDetailsWriter fsWriter { get; set; }
    
      
        
        private double lat;
        public double Lat
        {
            get
            {
                string latStr = client.write(Constants.LAT_STRING);
                string[] latArr = latStr.Split('\'');
                return Double.Parse(latArr[1]);

            }
            set { lat = value; }
        }


        private double lon;
        public double Lon
        {
            get
            {
                string lonStr = client.write(Constants.LON_STRING);
                string[] lonArr = lonStr.Split('\'');
                return Double.Parse(lonArr[1]);
            }

            set { lon = value; }
        }
        public double Throttle {
            get
            {
                string throttleStr = client.write(Constants.THROTTLE_STRING);
                string[] throttleArr = throttleStr.Split('\'');
                return Double.Parse(throttleArr[1]);
            }
        }

       
        public double Rudder
        {
            get
            {
                string rudderStr = client.write(Constants.RUDDER_STRING);
                string[] rudderArr = rudderStr.Split('\'');
                return Double.Parse(rudderArr[1]);
            }
        }


        #endregion


        public void connect(string ip, int port)
        {
            if (!isClientConnected) {
                isClientConnected = client.connect(ip, port);
            }
        }



        public void disconnectClient()
        {
            client.disconnect();
        }

        public void writeFlightDetails(string fileName)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(Constants.SCENARIO_FILE, fileName));
            
            if (!File.Exists(path))
            {
                FlightDetailsModel.Instance.Lat = (Instance.Lat).ToString();
                FlightDetailsModel.Instance.Lon = (this.Lon).ToString();
                FlightDetailsModel.Instance.Throttle = (this.Throttle).ToString();
                FlightDetailsModel.Instance.Rudder = (this.Rudder).ToString();

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                {
                    file.WriteLine(FlightDetailsModel.Instance.Lat);
                    file.WriteLine(FlightDetailsModel.Instance.Lon);
                    file.WriteLine(FlightDetailsModel.Instance.Throttle);
                    file.WriteLine(FlightDetailsModel.Instance.Rudder);
                }
            }
        }

        public void createFile(string fileName)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(Constants.SCENARIO_FILE, fileName));
            if (!System.IO.File.Exists(path))
            {
                System.IO.File.Create(path);  
            }
        }

        //writing to FS through the client class.
        ~FlightManagerModel()
        {
            client.disconnect();
        }
    }
}