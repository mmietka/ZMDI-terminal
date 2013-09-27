using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Mariola
{
    public partial class OpenConnection : Form
    {
        public OpenConnection()
        {            
            InitializeComponent();
            RefreshListPorts();
        }

        void RefreshListPorts()
        {
            String[] portNames = SerialPort.GetPortNames();

            foreach (string port in portNames)
            {
                listBoxPorts.Items.Add(port);
            }
        }

        private void listBoxPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxPorts.Items.Count > 0)
            {
                ButtonConnect.Enabled = true;
            }
            else
            {
                ButtonConnect.Enabled = false;
            }
        }

        private void listBoxPorts_DoubleClick(object sender, EventArgs e)
        {
            if (listBoxPorts.SelectedIndex >= 0)
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
