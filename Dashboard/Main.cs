using shipmonitoring.Properties;
using System;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
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
        /*
         * menyiapkan semua variabel dan object yang dibutuhkan untuk berjalannya aplikasi
         * 
         * 1. payload -> object dari kelas payload untuk menampung data yang diterima dari arduino
         * 2. powerPane dll -> object untuk mengolah chart chart yg ada di dashboard
         * 3. listVoltage dll -> ini adalah listData untuk mengisi chart, list ini yg nanti datanya ditambah dengan data yg baru
         * 4. markers -> object untuk menampung marker/penanda di maps
         * 5. center -> ini adalah marker untuk lokasi kapalnya
         * 6. lastmapposchange -> ini adalah untuk menandai kapan terakhir kali posisinya berpindah
         * 8. newPos -> object untuk menampung latitude dan longitude yg diterima dari arduino dan di masukkan ke maps
         * 9. dataCount -> untuk menghitung jumlah data yg diterima dari arduino
         * 10. path -> lokasi untuk menyimpan cache maps dan merujuk database dari aplikasi, menunjuk ke MyDocument/shipmonitoring/
         */
        Payload payload;
        GraphPane powerPane, wavePane, tempPane;
        RollingPointPairList listVoltage, listCurrent, listWHeight, listWPower, listWater, listAir;
        GMapOverlay markers;
        GMapMarker center;
        DateTime lastmapposchange;
        PointLatLng newPos;

        int dataCount = 0;
        float period = 0;
        readonly string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.ProductName);

        /*
         * Fungsi utama yg pertama kali dijalankan
         * 
         */
        public Main()
        {
            InitializeComponent();
            Console.WriteLine(DateTime.Now.ToString("dd-MM-yyyy"));
        }

        /*
         * handle saat form baru dibuka
         */
        private void Main_Load(object sender, EventArgs e)
        {
            /*
             * c# mempunyai keamanan, dengan tidak boleh merubah variable dari thread yg baru, contohnya: data yg didapat dari serial jika langsung 
             * digunakan untuk merubah chart dll akar error, maka dari itu diberi coding ini
             */
            CheckForIllegalCrossThreadCalls = false;

            /*
             * inisialisasi object payload dan buat folder di MyDocument jika masih belum ada folder bernama shipmonitoring
             */
            payload = new Payload();
            payload.CreateDir();

            /*
             * jalankan perintah untuk menyiapkan Pane awal dan Map sesuai keinginan kita
             * update txtStatusKoneksi untuk memberi tahu bahwa aplikasi siap digunakan, textnya ngambil dari resources
             */
            SetupPane();
            SetupMap();
            txtStatusKoneksi.Text = Resources.koneksi_siap;
        }

        /*
         * setup map awal
         */
        private void SetupMap()
        {
            /*
             * inisialisasi awal markers dan newPos , lat dan long itu random aja
             * centernya di update posisinya sesuai dengan posisi, center diberi tanda yellow_dot sebagai tanda itu adalah kapalnya
             * lalu updateposisi mapsnya sesuai posisi newPos nya
             */
            markers = new GMapOverlay("markers");
            newPos = new PointLatLng(-8.240101, 111.469429);
            center = new GMarkerGoogle(newPos, GMarkerGoogleType.yellow_dot);
            UpdateMapPosition(newPos);

            markers.Markers.Add(center);
            gMapControl1.Overlays.Add(markers);

            /*
             * setup awal buat properti properti gmapnya
             */
            gMapControl1.CacheLocation = path + "//gmapcache//";
            gMapControl1.MinZoom = 0;
            gMapControl1.MaxZoom = 24;
            gMapControl1.Zoom = 3;

            /*
             * beri listener saat mapnya di zoom melalui scroll
             * saat ini, fitur zoom dengan scrollbar masih belum ditambahkan
             */
            gMapControl1.OnMapZoomChanged += GMapControl1_OnMapZoomChanged;
            gMapControl1.DisableFocusOnMouseEnter = true;

            gMapControl1.RoutesEnabled = false;
            gMapControl1.PolygonsEnabled = false;

            gMapControl1.EmptyTileColor = Color.Gray;
            gMapControl1.HoldInvalidation = true;

            /*
             * map providernya pilih dari Bing dan tipe akses ke maps nya berupa cache dan online, cache agar tidak selalu menggunakan internet
             */
            gMapControl1.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
        }

        /* 
         * fungsi ini belum digunakan karena fiturnya belum ditambah, kalau mau dihapus boleh
         * 
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
        */

        /*
         * handle fungsi untuk update posisi map
         */
        private void UpdateMapPosition(PointLatLng currentLoc)
        {
            /*
             * invoke adalah fungsi untuk request ganti data dari thread yg baru saja dibuat seperti data dari serial, digunakan untuk mengubah posisi maps
             * 
             * sebenernya ini tidak diperlukan karena diawal tadi , sudah di kasih fungsi checkillegarthread = false
             */
            BeginInvoke((Action)delegate
            {
                try
                {
                    /*
                     * cek terlebih dahulu agar pergantian posisinya tidak terlalu sering, 1 detik sekali saja agar tidak memberatkan
                     */
                    if(lastmapposchange.Second != DateTime.Now.Second)
                    {
                        /*
                         * cek juga perpindahan lokasinya cukup jauh untuk melakukan update map, jika masih sama, gak perlu di update
                         */
                        if (Math.Abs(currentLoc.Lat - gMapControl1.Position.Lat) > 0.0001 || Math.Abs(currentLoc.Lng - gMapControl1.Position.Lng) > 0.0001){
                            gMapControl1.Position = currentLoc;
                        }
                        /*
                         * reset waktu terakhir kita update posisi mapsnya
                         */
                        lastmapposchange = DateTime.Now;
                    }
                }
                catch
                {
                }
            });
        }

        /*
         * saat posisi dari maps nya berubah, jangan lupa ubah juga posisi dari markernya
         */
        private void GMapControl1_OnPositionChanged(PointLatLng point)
        {
            center.Position = point;
        }

        /*
         * ini masih belum dipakai
         */
        private void GMapControl1_OnMapZoomChanged()
        {

        }

        /*
         * setup graphphane agar siap buat diberi data baru nanti
         */
        private void SetupPane()
        {
            /*
             * kaitkan object powerPane dengan chart sesuai yg kita atur di dashboard
             * ubah ubah textnya untuk judul, axis x dan axis y
             */
            powerPane = zedGraphPower.GraphPane;
            powerPane.Title.Text = "Power Consumtion";
            powerPane.XAxis.Title.Text = "Time";
            powerPane.YAxis.Title.Text = "Ampere/Volt";

            /*
             * inisialisasi list data tegangan dan arus dengan data yg isinya 1200 data
             */
            listVoltage = new RollingPointPairList(1200);
            listCurrent = new RollingPointPairList(1200);
            /*
             * beri nama dan warna pada list datanya
             */
            powerPane.AddCurve("Voltage", listVoltage, Color.Red, SymbolType.None);
            powerPane.AddCurve("Current", listCurrent, Color.Blue, SymbolType.None);

            /*
             * beri tanda pada chart nya bahwa ada data yg diubah
             */
            zedGraphPower.AxisChange();

            wavePane = zedGraphWave.GraphPane;
            wavePane.Title.Text = "Wave Monitoring";
            wavePane.XAxis.Title.Text = "Time";
            wavePane.YAxis.Title.Text = "Wave";

            listWHeight = new RollingPointPairList(1200);
            listWPower = new RollingPointPairList(1200);
            wavePane.AddCurve("Wave Height", listWHeight, Color.Red, SymbolType.None);
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

        /*
         * handle combo box untuk port available saat di klik
         */
        private void CmbPort_Click(object sender, EventArgs e)
        {
            /*
             * setiap kali comboBox/list nya di klik, jalankan dulu perintah untuk update portnya / cek comport yang tersedia
             */
            UpdatePort();
        }

        /*
         * update comport yang tersedia ke dalam comboBox
         */
        void UpdatePort()
        {
            /*
             * hapus item yg ada di combo agar bisa diperbarui dengan yang baru
             * reset juga item yg terpilih di combo box nya
             */
            cmbPort.Items.Clear();
            cmbPort.Text = String.Empty;
            /*
             * cek dulu, jika serialnya masih tersambung dengan perangkat arduino, tutup dulu
             * gunakan perintah try catch agar aplikasi tidak tertutup / memunculkan error jika terjadi error
             */
            try
            {
                if (serialPort.IsOpen) serialPort.Close();
            }
            catch { }
            /*
             * ambil setiap port yang tersedia, lalu tambahkan ke comboBox
             */
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
                if (cmbPort.Text.Equals(string.Empty))
                {
                    MessageBox.Show(Resources.warning_comport_kosong);
                    return;
                }

                serialPort.PortName = cmbPort.SelectedItem.ToString();

                try
                {
                    serialPort.Open();
                    btnKoneksi.Text = Resources.button_koneksi_tersambung;
                    txtStatusKoneksi.Text = Resources.koneksi_tersambung;
                    MessageBox.Show(Resources.koneksi_tersambung);
                    cmbPort.Enabled = false;
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Resources.warning_koneksi_gagal + " (" + ex.Message + ")");
                    Console.WriteLine(ex.Message);
                }
            }

            if (btnKoneksi.Text.Equals(Resources.button_koneksi_tersambung))
            {
                try
                {
                    if (serialPort.IsOpen) serialPort.Close();
                    btnKoneksi.Text = Resources.button_koneksi_terputus;
                    txtStatusKoneksi.Text = Resources.koneksi_terputus;
                    cmbPort.Enabled = true;
                }
                catch
                { }
            }
        }

        /*
         * handle data yg masuk ke serial
         */
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            /*
             * baca data yang diterima sampai menemukan line baru/ \r\n yg dijelaskan di file ino nya
             */
            String data = serialPort.ReadLine();
            /*
             * olah datanya, jgn lupa pakai try catch , jika sewaktu waktu ada error, aplikasi tidak hang
             */
            try
            {
                /*
                 * pengiriman data menggunakan format json keuntungannya adalah banyak library yg tersedia 
                 * di deserealize dan dimasukkan ke object payload, key nya sesuai dengan variabel variabel yg ada didalam object payload
                 * contoh: jsonnya {"AbC": "tahu"} , nah variabel di objectnya ada private string AbC;
                 * otomatis saat di akses payload.AbC maka isinya adalah "tahu"
                 */
                payload = JsonConvert.DeserializeObject<Payload>(data);
                /*
                 * didalam kelas Payload saya kasih fungsi untuk menyimpan ke database, tinggal akses saja/jalankan perintah SaveToDB();
                 */
                payload.SaveToDB();

                /*
                 * update jumlah datanya dengan perintah dataCount++;
                 */
                dataCount++;
                /*
                 * lalu tambahkan data yg diterima ke list yang sudah kita siapkan diatas tadi
                 */
                listVoltage.Add(dataCount, payload.Voltage);
                listCurrent.Add(dataCount, payload.Current);

                listWHeight.Add(period, payload.WaveHeight);
                listWPower.Add(period, payload.WavePower);

                listWater.Add(dataCount, payload.WaterTemp);
                listAir.Add(dataCount, payload.AirTemp);

                period += payload.WavePeriod;

                /*
                 * beritahu semua pane/chart bahwa datanya berubah, lalu kita suruh buat menampilkan yang terbaru
                 */
                zedGraphPower.AxisChange();
                zedGraphPower.Invalidate();

                zedGraphWave.AxisChange();
                zedGraphWave.Invalidate();

                zedGraphTemp.AxisChange();
                zedGraphTemp.Invalidate();

                /*
                 * newPos tadi update datanya dengan data yang diterima dari arduino
                 */
                newPos.Lat = payload.Latitude;
                newPos.Lng = payload.Longitude;
                /*
                 * lalu jalankan perintah update posisi mapsnya
                 */
                UpdateMapPosition(newPos);

                /*
                 * ini untuk informasi saja ke user, kalau datanya diterima benar, maka nyalanya hijau
                 */
                progressBar1.ForeColor = Color.Green;
            }
            /*
             * Kalau datanya salah, atau terjadi error maka nyalanya merah, garis di pojok kanan atas
             */
            catch(JsonException)
            {
                Console.WriteLine(data);
                progressBar1.ForeColor = Color.Red;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                progressBar1.ForeColor = Color.Red;
            }
        }

        /*
         * ini keperluan buat debugging saja, untuk tes data agar tidak selalu menggunakan arduino untuk mencoba
         * ini tidak akan berjalan selama Timer1 yg ada di dashboard enabled nya bernilai false
         * 
         * fungsi tes pun sama persis dengan perintah perintah yg ada di serialport datareceived
         */
        private void Timer1_Tick(object sender, EventArgs e)
        {
            Tes();
        }

        private void Tes()
        {
            string jsonstring = "{" +
                "'Date': 12312323," +
                "'Time': 321312312," +
                "'Latitude': -8.240101," +
                "'Longitude': 70.469429," +
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

            dataCount++;
            listVoltage.Add(dataCount, payload.Voltage);
            listCurrent.Add(dataCount, payload.Current);

            listWHeight.Add(dataCount, payload.WaveHeight);
            listWPower.Add(dataCount, payload.WavePower);

            listWater.Add(dataCount, payload.WaterTemp);
            listAir.Add(dataCount, payload.AirTemp);

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
    }
}
