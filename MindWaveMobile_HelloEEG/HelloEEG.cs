using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Threading;


using NeuroSky.ThinkGear;
using NeuroSky.ThinkGear.Algorithms;


namespace testprogram {
    class Program {
        static Connector connector;
        static byte poorSig;

        public static void Main(string[] args) {

            Console.WriteLine("HelloEEG!");



            // Initialize a new Connector and add event handlers

            connector = new Connector();
            connector.DeviceConnected += new EventHandler(OnDeviceConnected);
            connector.DeviceConnectFail += new EventHandler(OnDeviceFail);
            connector.DeviceValidating += new EventHandler(OnDeviceValidating);

            // Scan for devices across COM ports
            // The COM port named will be the first COM port that is checked.
            connector.ConnectScan("COM4");

            // Blink detection needs to be manually turned on
            connector.setBlinkDetectionEnabled(true);
            Thread.Sleep(450000);




            System.Console.WriteLine("Goodbye.");
            connector.Close();
            Environment.Exit(0);
        }


        // Called when a device is connected 

        static void OnDeviceConnected(object sender, EventArgs e) {

            Connector.DeviceEventArgs de = (Connector.DeviceEventArgs)e;

            Console.WriteLine("Device found on: " + de.Device.PortName);
            de.Device.DataReceived += new EventHandler(OnDataReceived);

        }




        // Called when scanning fails

        static void OnDeviceFail(object sender, EventArgs e) {

            Console.WriteLine("No devices found! :(");

        }



        // Called when each port is being validated

        static void OnDeviceValidating(object sender, EventArgs e) {

            Console.WriteLine("Validating: ");

        }

        // Called when data is received from a device

        static void OnDataReceived(object sender, EventArgs e) {

            //Device d = (Device)sender;

            Device.DataEventArgs de = (Device.DataEventArgs)e;
            DataRow[] tempDataRowArray = de.DataRowArray;

            TGParser tgParser = new TGParser();
            tgParser.Read(de.DataRowArray);
            if (tgParser.ParsedData.Length == 0)
                return;

            /* Loops through the newly parsed data of the connected headset*/
            // The comments below indicate and can be used to print out the different data outputs. 

            List<string> list = new List<string>();
            for (int i = 0; i < tgParser.ParsedData.Length; i++)
                {
          
                    var parsedData = tgParser.ParsedData[i];
                    if (parsedData.ContainsKey("Raw"))
                    {
                    //Console.WriteLine("Raw Value:" + tgParser.ParsedData[i]["Raw"]);
                    //list.Add("Raw:" + parsedData["Raw"].ToString());
                    }

                    if (tgParser.ParsedData[i].ContainsKey("PoorSignal"))
                    {
                        //The following line prints the Time associated with the parsed data
                        //Console.WriteLine("Time:" + tgParser.ParsedData[i]["Time"]);

                        //A Poor Signal value of 0 indicates that your headset is fitting properly
                        //Console.WriteLine("Poor Signal:" + tgParser.ParsedData[i]["PoorSignal"]);
                        poorSig = (byte)tgParser.ParsedData[i]["PoorSignal"];
                    }


                    if (tgParser.ParsedData[i].ContainsKey("Attention"))
                    {

                        //Console.WriteLine("Att Value:" + tgParser.ParsedData[i]["Attention"]);
                        list.Add("Att:" + parsedData["Attention"].ToString());
                        //buff = Encoding.UTF8.GetBytes("Att:" + tgParser.ParsedData[i]["Attention"]);

                        // 서버에 데이타 전송
                        //sock.Send(buff, SocketFlags.None);
                    }


                    if (tgParser.ParsedData[i].ContainsKey("Meditation"))
                    {

                        //Console.WriteLine("Med Value:" + tgParser.ParsedData[i]["Meditation"]);
                        //list.Add("Med:" + parsedData["Meditation"].ToString());
                        //buff = Encoding.UTF8.GetBytes("Med:" + tgParser.ParsedData[i]["Meditation"]);

                        // 서버에 데이타 전송
                        //sock.Send(buff, SocketFlags.None);

                    }


                    if (tgParser.ParsedData[i].ContainsKey("EegPowerDelta"))
                    {

                        //Console.WriteLine("Delta: " + tgParser.ParsedData[i]["EegPowerDelta"]);
                        //buff = Encoding.UTF8.GetBytes("Delta:" + tgParser.ParsedData[i]["EegPowerDelta"]);

                        // 서버에 데이타 전송
                        // sock.Send(buff, SocketFlags.None);
                    }

                    if (tgParser.ParsedData[i].ContainsKey("BlinkStrength"))
                    {
                        //list.Add("Blink:" + parsedData["BlinkStrength"].ToString());
                        //Console.WriteLine("Eyeblink: " + tgParser.ParsedData[i]["BlinkStrength"]);
                        //buff = Encoding.UTF8.GetBytes("Blink:" + tgParser.ParsedData[i]["BlinkStrength"]);

                        // 서버에 데이타 전송
                        //sock.Send(buff, SocketFlags.None);
                    }

                
                }

            if (list.Count == 0)
                return;

            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            var ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
            sock.Connect(ep);

            foreach (string x in list) {
                
                sock.Send(Encoding.UTF8.GetBytes(x+System.Environment.NewLine));
            }

            sock.Disconnect(true);
            sock.Close();

        }

    }

}
