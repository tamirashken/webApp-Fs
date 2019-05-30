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
    public const string THROTTLE_STRING = "23";
    public const string RUDDER_STRING = "21";
}
namespace WebApplication1.Models
{
    //should be singletone
    public class FlightManagerModel
    {
        Client client;
        TcpClient tcpClient;
        bool isClientConnected;

        #region Singleton
        private FlightManagerModel()
        {
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
         *  all notifies will be sent to the ViewModel beacuase the view model is the observer so it can check which property changed
         *  and find 
         */

        private double lat;
        public double Lat
        {
            get
            {
                return client.write(Constants.LAT_STRING);
            }
            set { lat = value; }
        }


        private double lon;
        public double Lon
        {
            get
            {
                lon = client.write(Constants.LON_STRING);
                return lon;
            }

            set { lon = value; }
        }
        public double Throttle { get; set; }
        public double Rudder { get; set; }

        #endregion

        public void connect(string ip, int port)
        {
            isClientConnected = client.connect(ip, port);
        }

        public void disconnectClient()
        {
            client.disconnect();
        }

        //writing to FS through the client class.
        public void getInfo(string command)
        {
            if (isClientConnected)
            {

            }
        }
        ~FlightManagerModel()
        {
            client.disconnect();
        }
    }
}