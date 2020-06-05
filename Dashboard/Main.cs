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
using System.IO;

namespace shipmonitoring.Dashboard
{
    public partial class Main : Form
    {
        Payload payload;

        public Main()
        {
            InitializeComponent();
            updatePort();

            txtStatusKoneksi.Text = Resources.koneksi_siap;

        }

        private void tes()
        {
            payload = new Payload();

            string jsonstring = "{" +
                "'Date': '22 MEI 2020'," +
                "'Time': '13:00'," +
                "'Latitude': '123'," +
                "'Longitude': '65'," +
                "'Current': '4'," +
                "'Voltage': '12'," +
                "'WaveHeight': '4'," +
                "'WavePeriod': '3'," +
                "'WavePower': '5'," +
                "'WaterTemp': '76'," +
                "'AirTemp': '54'" +
                "}";
            payload = JsonConvert.DeserializeObject<Payload>(jsonstring);
            payload.saveToDB();
        }

        /*
         * handle combo box untuk port available saat di klik
         */
        private void cmbPort_Click(object sender, EventArgs e)
        {
            updatePort();
        }

        /*
         * update comport yang tersedia ke dalam comboBox
         *
         */
        void updatePort()
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
        private void btnKoneksi_Click(object sender, EventArgs e)
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
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            String data = serialPort.ReadLine();
            try
            {
                payload = JsonConvert.DeserializeObject<Payload>(data);
                payload.saveToDB();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
