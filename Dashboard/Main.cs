using shipmonitoring.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft;
using shipmonitoring.Data;
using Newtonsoft.Json;
using ZedGraph;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET;

namespace shipmonitoring.Dashboard
{
    public partial class Main : Form
    {
        Payload payload;
        GraphPane powerPane, wavePane, tempPane;
        RollingPointPairList listVoltage, listCurrent, listWHeight, listWPeriod, listWPower, listWater, listAir;
        GMapMarker center;
        DateTime lastmapposchange;
        PointLatLng newPos;

        int TickStart;
        readonly string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.ProductName);

        /*
         * Fungsi utama yg pertama kali dijalankan
         * 
         */
        public Main()
        {
            InitializeComponent();;
        }

        /*
         * handle saat form baru dibuka
         * 
         */
        private void Main_Load(object sender, EventArgs e)
        {
            UpdatePort();
            SetupPane();
            SetupMap();
            TickStart = Environment.TickCount;
            txtStatusKoneksi.Text = Resources.koneksi_siap;
            newPos = new PointLatLng();
            Tes();
        }

        /*
         * setup map awal
         * 
         */
        private void SetupMap()
        {
            center = new GMarkerGoogle(new GMap.NET.PointLatLng(-8.240101, 111.469429), GMarkerGoogleType.none);
            
            gMapControl1.CacheLocation = path + "//gmapcache//";
            gMapControl1.MinZoom = 0;
            gMapControl1.MaxZoom = 24;
            gMapControl1.Zoom = 3;

            gMapControl1.OnMapZoomChanged += GMapControl1_OnMapZoomChanged;
            gMapControl1.DisableFocusOnMouseEnter = true;

            gMapControl1.RoutesEnabled = false;
            gMapControl1.PolygonsEnabled = false;

            gMapControl1.EmptyTileColor = Color.Gray;
            gMapControl1.HoldInvalidation = true;

            gMapControl1.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = AccessMode.ServerAndCache;
        }

        private void UpdateMapZoom(int zoom)
        {
            BeginInvoke((Action)delegate
            {
                try
                {
                    gMapControl1.Zoom = zoom;
                }
                catch
                {
                }
            });
        }

        private void UpdateMapPosition(PointLatLng currentLoc)
        {
            BeginInvoke((Action)delegate
            {
                try
                {
                    if(lastmapposchange.Second != DateTime.Now.Second)
                    {
                        if (Math.Abs(currentLoc.Lat - gMapControl1.Position.Lat) > 0.0001 || Math.Abs(currentLoc.Lng - gMapControl1.Position.Lng) > 0.0001){
                            gMapControl1.Position = currentLoc;
                        }
                        lastmapposchange = DateTime.Now;
                    }
                }
                catch
                {

                }
            });
        }

        private void GMapControl1_OnPositionChanged(PointLatLng point)
        {
            center.Position = point;
        }

        private void GMapControl1_OnMapZoomChanged()
        {

        }

        /*
         * setup graphphane agar siap buat diberi data baru nanti
         * 
         */
        private void SetupPane()
        {
            powerPane = zedGraphPower.GraphPane;
            powerPane.Title.Text = "Power Consumtion";
            powerPane.XAxis.Title.Text = "Time";
            powerPane.YAxis.Title.Text = "Ampere/Volt";

            listVoltage = new RollingPointPairList(1200);
            listCurrent = new RollingPointPairList(1200);
            powerPane.AddCurve("Voltage", listVoltage, Color.Red, SymbolType.None);
            powerPane.AddCurve("Current", listCurrent, Color.Blue, SymbolType.None);

            zedGraphPower.AxisChange();

            wavePane = zedGraphWave.GraphPane;
            wavePane.Title.Text = "Wave Monitoring";
            wavePane.XAxis.Title.Text = "Time";
            wavePane.YAxis.Title.Text = "Wave";

            listWHeight = new RollingPointPairList(1200);
            listWPeriod = new RollingPointPairList(1200);
            listWPower = new RollingPointPairList(1200);
            wavePane.AddCurve("Wave Height", listWHeight, Color.Red, SymbolType.None);
            wavePane.AddCurve("Wave Period", listWPeriod, Color.Blue, SymbolType.None);
            wavePane.AddCurve("Wave Power", listWPower, Color.Yellow, SymbolType.None);

            zedGraphWave.AxisChange();

            tempPane = zedGraphTemp.GraphPane;
            tempPane.Title.Text = "Temp Monitoring";
            tempPane.XAxis.Title.Text = "Time";
            tempPane.YAxis.Title.Text = "Celcius";

            listWater = new RollingPointPairList(1200);
            listAir = new RollingPointPairList(1200);
            tempPane.AddCurve("Water Temp", listWater, Color.Red, SymbolType.None);
            tempPane.AddCurve("Air Temp", listAir, Color.Blue, SymbolType.None);

            zedGraphTemp.AxisChange();
        }


        private void Tes()
        {
            payload = new Payload();

            string jsonstring = "{" +
                "'Date': 12312323," +
                "'Time': 321312312," +
                "'Latitude': -8.240101," +
                "'Longitude': 111.469429," +
                "'Current': 4.1," +
                "'Voltage': 12.2," +
                "'WaveHeight': 4.0," +
                "'WavePeriod': 3.3," +
                "'WavePower': 5.4," +
                "'WaterTemp': 76.0," +
                "'AirTemp': 55.3" +
                "}";
            payload = JsonConvert.DeserializeObject<Payload>(jsonstring);
            payload.SaveToDB();

            double time = (Environment.TickCount - TickStart) / 1000.0;
            listVoltage.Add(time, payload.Voltage);
            listCurrent.Add(time, payload.Current);

            listWHeight.Add(time, payload.WaveHeight);
            listWPeriod.Add(time, payload.WavePeriod);
            listWPower.Add(time, payload.WavePower);

            listWater.Add(time, payload.WaterTemp);
            listAir.Add(time, payload.AirTemp);

            zedGraphPower.AxisChange();
            zedGraphPower.Invalidate();

            zedGraphWave.AxisChange();
            zedGraphWave.Invalidate();

            zedGraphTemp.AxisChange();
            zedGraphTemp.Invalidate();

            newPos.Lat = payload.Latitude;
            newPos.Lng = payload.Longitude;
            UpdateMapPosition(newPos);
        }

        /*
         * handle combo box untuk port available saat di klik
         */
        private void CmbPort_Click(object sender, EventArgs e)
        {
            Console.WriteLine("di klik");
            UpdatePort();
        }

        /*
         * update comport yang tersedia ke dalam comboBox
         *
         */
        void UpdatePort()
        {
            cmbPort.Items.Clear(); // hapus item yg ada di combo agar bisa diperbarui dengan yang baru
            cmbPort.Text = String.Empty;
            if (serialPort.IsOpen) serialPort.Close();
            foreach (var port in SerialPort.GetPortNames())
            {
                cmbPort.Items.Add(port);
            }
        }

        /*
         * handle btnKoneksi saat di klik
         * 
         */
        private void BtnKoneksi_Click(object sender, EventArgs e)
        {
            if(btnKoneksi.Text.Equals(Resources.button_koneksi_terputus))
            {
                serialPort.PortName = cmbPort.SelectedItem.ToString();
            }
            try
            {
                serialPort.Open();
                btnKoneksi.Text = Resources.button_koneksi_tersambung;
                txtStatusKoneksi.Text = Resources.koneksi_tersambung;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.warning_koneksi_gagal);
                Console.WriteLine(ex.Message);
                txtStatusKoneksi.Text = ex.Message;
            }

            if (btnKoneksi.Text.Equals(Resources.button_koneksi_tersambung))
            {
                if (serialPort.IsOpen) serialPort.Close();
                btnKoneksi.Text = Resources.button_koneksi_terputus;
                txtStatusKoneksi.Text = Resources.koneksi_terputus;
            }
        }

        /*
         * handle data yg masuk ke serial
         * 
         */
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            String data = serialPort.ReadLine();
            try
            {
                payload = JsonConvert.DeserializeObject<Payload>(data);
                payload.SaveToDB();

                double time = (Environment.TickCount - TickStart) / 1000.0;
                listVoltage.Add(time, payload.Voltage);
                listCurrent.Add(time, payload.Current);

                listWHeight.Add(time, payload.WaveHeight);
                listWPeriod.Add(time, payload.WavePeriod);
                listWPower.Add(time, payload.WavePower);

                listWater.Add(time, payload.WaterTemp);
                listAir.Add(time, payload.AirTemp);

                zedGraphPower.AxisChange();
                zedGraphPower.Invalidate();

                zedGraphWave.AxisChange();
                zedGraphWave.Invalidate();

                zedGraphTemp.AxisChange();
                zedGraphTemp.Invalidate();

                newPos.Lat = payload.Latitude;
                newPos.Lng = payload.Longitude;
                UpdateMapPosition(newPos);
            }
            catch(JsonException)
            {
                Console.WriteLine(data);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Tes();
        }
    }
}
