using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
using System.Xml;

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
        IList<string> lons;
        IList<String> lats;
        bool isClientConnected;
        #region Singleton
        private FlightManagerModel()
        {
            this.client = new Client();
            this.isClientConnected = false;
            lons = new List<string>();
            lats = new List<String>();
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
        //public FlightDetailsWriter fsWriter { get; set; }
    
      
        
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
                try
                {
                    string lonStr = client.write(Constants.LON_STRING);
                    string[] lonArr = lonStr.Split('\'');
                    return Double.Parse(lonArr[1]);
                }
                catch (Exception)
                {
                    return -1000;
                }
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
        public string[] dataFromFile { get; set; }

        #endregion


        public bool connect(string ip, int port)
        {
            if (!isClientConnected) {
                isClientConnected = client.connect(ip, port);
            }
            return isClientConnected;
        }

        public string getLine()
        {
            return dataFromFile[0];
        }



        public void disconnectClient()
        {
            client.disconnect();
            isClientConnected = false;
        }

  /*      public void writeFlightDetails(string fileName)
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
*/
        public void createFile(string fileName)
        {
            string path = HttpContext.Current.Server.MapPath(String.Format(Constants.SCENARIO_FILE, fileName));
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            System.IO.File.Create(path);
        }

        /*
 *The function reads all data of planes location to the array 
 */
        public void Read(string fileName)
        {
            
            string path = HttpContext.Current.Server.MapPath(String.Format(Constants.SCENARIO_FILE, fileName));

            string[] temp = System.IO.File.ReadAllLines(path);
            string[] data = temp[0].Split(',');
            for(int i = 1; i < data.Length - 1; i += 5)
            {
                lons.Add(data[i]);
                lats.Add(data[i+1]);
            }
        }

        public string getNextLon()
        {
            if (this.lons.Count > 0)
            {
                string temp = this.lons.ElementAt(0);
                this.lons.RemoveAt(0);
                return temp;
            }
            else
            {
                return "-1000";
            }
        }

        public string getNextLat()
        {
            if (this.lats.Count > 0)
            {
                string temp = this.lats.ElementAt(0);
                this.lats.RemoveAt(0);
                return temp;
            }
            else
            {
                return "-1000";
            }
        }

        //writing to FS through the client class.
        ~FlightManagerModel()
        {
            client.disconnect();
        }
    }
}