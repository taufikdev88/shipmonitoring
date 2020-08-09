using shipmonitoring.Properties;
using System;
using System.IO;
using System.Windows.Forms;
using System.Data.OleDb;
using Newtonsoft.Json;

namespace shipmonitoring.Data
{
    class Payload
    {
        /*
         * deklarasi variabel variabel sesuai Key json yg kita olah
         * 
         * jika jsonnya {"AbC": "tahu"} maka variabelnya juga AbC
         * 
         * {get; set}; artinya bahwa nilai dari variabel ini bisa diambil dan diubah dari luar
         */
        private String Date { get; set;  }
        private String Time { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Current { get; set; }
        public float Voltage { get; set; }
        public float WaveHeight { get; set; }
        public float WavePeriod { get; set; }
        public float WavePower { get; set; }
        public float WaterTemp { get; set; }
        public float AirTemp { get; set; }


        /*
         * path nya seperti path yg ada di dashboard, sama sama menuju ke MyDocument/shipmonitoring
         */
        readonly string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.ProductName);
        /*
         * object yg menjembatani antara aplikasi dengan database microsoftAccess
         */
        OleDbConnection conn;

        /*
         * Handle membuat directory di MyDocument
         * 
         */
        public void CreateDir()
        {
            if (!Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
        }


        /*
         * Handle membaca data dari database
         * 
         * fitur lanjut
         */
        public bool ReadFromDB()
        {
            if (ConnectDB())
            {
                try
                {
                    OleDbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "Select * from Data where Date(Tanggal) >= Date(now())";
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string pld = "";
                            JsonConvert.SerializeObject(pld);
                        }
                        reader.Close();
                    }
                    conn.Close();
                    return true;
                }
                catch (Exception)
                {
                    Console.WriteLine(Resources.warning_database_baca_gagal);
                    return false;
                }
            }
            return false;
        }

        /*
         * handle menyimpan data ke database
         * 
         * mengembalikan nilai boolean
         */
        public bool SaveToDB()
        {
            this.Date = DateTime.Now.ToString("dd-MM-yyyy");
            this.Time = DateTime.Now.ToString("hh:mm:ss tt");

            /*
             * jalankan dulu perintah untuk koneksi ke database, jika berhasil, lanjut buat menyimpan datanya
             */
            if (ConnectDB())
            {
                /*
                 * penggunaan try agar jika terjadi error penyimpanan ke database , aplikasi tidak keluar, tp ada warning yg keluar
                 */
                try
                {
                    /*
                     * cmd adalah object yg disediakan oleh library untuk menjalankan perintah sql
                     */
                    OleDbCommand cmd = conn.CreateCommand();
                    /*
                     * ini adalah perintah sqlnya "insert into ......"
                     * 
                     * string.Format adalah fungsi dari library bawaan c# untuk mempermudah pengisian text  agar tidak ribet
                     */
                    cmd.CommandText = String.Format("Insert into tbl_data(Tanggal, Waktu, Latitude, Longitude, [Current], Voltage, WaveHeight, WavePeriod, WavePower, WaterTemp, AirTemp) values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')",
                        this.Date,
                        this.Time,
                        this.Latitude,
                        this.Longitude,
                        this.Current,
                        this.Voltage,
                        this.WaveHeight,
                        this.WavePeriod,
                        this.WavePower,
                        this.WaterTemp,
                        this.AirTemp);
                    /*
                     * ekseskusi perintah sqlnya
                     */
                    cmd.ExecuteNonQuery();
                    /*
                     * tutup koneksinya setelah perintah dijalankan
                     */
                    conn.Close();
                    /*
                     * beri tanda untuk keperluan debugging bahwa data sudah berhasil disimpan
                     * 
                     * ini hanya akan muncul saat aplikasi dijalankan dari visualStudio
                     */
                    Console.WriteLine(Resources.warning_database_simpan_berhasil);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(Resources.warning_database_simpan_gagal + " ,Message: " + ex.Message);
                    return false;
                }
            }
            return false;
        }

        /*
         * handle koneksi ke database
         * 
         * mengembalikan nilai boolean
         */
        private bool ConnectDB()
        {
            /*
             * koneksi ke databasenya
             */
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + "\\Database.accdb";
            conn = new OleDbConnection(connectionString);

            /*
             * coba buka koneksi nya menggunakan try catch, seperti tadi jika terjadi kegagalan, aplikasi tidak langsung error dan hang
             */
            try
            {
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.warning_database_koneksi_gagal);
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
