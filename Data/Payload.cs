using shipmonitoring.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using Newtonsoft.Json;

namespace shipmonitoring.Data
{
    class Payload
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Current { get; set; }
        public string Voltage { get; set; }
        public string WaveHeight { get; set; }
        public string WavePeriod { get; set; }
        public string WavePower { get; set; }
        public string WaterTemp { get; set; }
        public string AirTemp { get; set; }

        string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Application.ProductName);
        OleDbConnection conn;


        /*
         * Handle membuat directory di MyDocument
         * 
         */
        public void createDir()
        {
            if (!Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
        }


        /*
         * Handle membaca data dari database
         * 
         * fitur lanjut
         */
        public bool readFromDB()
        {
            if (connectDB())
            {
                try
                {
                    OleDbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "Select * from Data where Date(Tanggal) >= Date(now())";
                    OleDbDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string pld = "";
                        JsonConvert.SerializeObject(pld);
                    }
                    reader.Close();
                    conn.Close();
                    return true;
                }
                catch (Exception ex)
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
        public bool saveToDB()
        {
            if (connectDB())
            {
                try
                {
                    OleDbCommand cmd = conn.CreateCommand();
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
                    cmd.ExecuteNonQuery();
                    conn.Close();
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
        private bool connectDB()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + "\\Database.accdb";
            conn = new OleDbConnection(connectionString);

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
