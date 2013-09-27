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
    public partial class MainForm : Form
    {
        public bool toClose = false;
        public string commandToProcessingForRichTextBox = "";

        string messageFromRS232ToParse = "";
        List<string> listOfAllMessage = new List<string>();
        List<RsMessage> listOfMessage = new List<RsMessage>();
        List<MessagesFromModule> modules = new List<MessagesFromModule>();


        //public 
        public MainForm()
        {
            OpenConnection window = new OpenConnection();
            if (window.ShowDialog() == DialogResult.OK)
            {
                //todo
            }
            else
            {
                Close();
                toClose = true;
                return;
            }
            InitializeComponent();
            this.Text += " ver:" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();

            try
            {
                object port = window.listBoxPorts.SelectedItem;
                serialPort1.PortName = (string)port;
                serialPort1.BaudRate = 115200;
                serialPort1.Parity = Parity.None;
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Open();
                timer1.Start();
                textBoxRsOut.Text += "connected with " + port.ToString() + " \r\n";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                textBoxRsOut.Text += ex.ToString() + "\r\n";
            };
        }

        string GetStringAndCath(ref string strInOut, int from, int to)
        {
            string toReturn = strInOut.Substring(from, to - from);
            strInOut = strInOut.Substring(to, strInOut.Length - to);
            return (toReturn);
        }

        void AddNewValueForModule(string address, string rssi, int countSend, int countReceive, DateTime time, string hop)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Text == address)
                {
                    item.SubItems[1].Text = rssi;
                    item.SubItems[2].Text = hop;
                    item.SubItems[3].Text = countSend.ToString()+"/"+countReceive.ToString();
                    item.SubItems[4].Text = time.Hour+":"+time.Minute+":"+time.Second+":"+time.Millisecond;
                    return;
                }
            }

            string[] items = { address, rssi.ToString(), countSend.ToString(), countReceive.ToString(), "" };
            listView1.Items.Add(new ListViewItem(items));
        }

        void ParseCommand(ref string strIn)
        {
            int indexStart = strIn.IndexOf('<');

            if (indexStart < 0)
            {
                //If the command is not in XML format
                listOfAllMessage.Add(strIn);
                strIn = "";
            }
            else
            {
                while (indexStart >= 0)
                {
                    if (indexStart > 0)
                    {
                        //Cut first no xml chars
                        string xx = GetStringAndCath(ref strIn, 0, indexStart);
                        listOfAllMessage.Add(xx);
                    }
                    else
                    {
                        //If the command is in XML format
                        int indexStop = strIn.IndexOf('>', indexStart);
                        if (indexStop >= 0)
                        {
                            string strAfterCut = GetStringAndCath(ref strIn, 0, indexStop + 1);

                            //todo: jesli xx jest >100 wyzerowac
                            switch (strAfterCut)
                            {
                                case ("<SendUDP2>"):
                                    {
                                        int findEnd = strIn.IndexOf(@"<\SendUDP2>");
                                        string information = GetStringAndCath(ref strIn, 0, findEnd);

                                        RsMessage message = new RsMessage("SendUDP2", information);
                                        listOfMessage.Add(message);

                                        {//send
                                            bool found = false;
                                            foreach (MessagesFromModule foundModule in modules)
                                            {
                                                if (foundModule.address == message.addr)
                                                {
                                                    foundModule.AddSend(message);
                                                    AddNewValueForModule(foundModule.address, foundModule.rssi, foundModule.countSend, foundModule.countReceive, foundModule.lastPackage, foundModule.hop);
                                                    found = true;
                                                    break;
                                                }
                                            }
                                            if (found == false)
                                            {
                                                MessagesFromModule newModule = new MessagesFromModule();
                                                newModule.address = message.addr;
                                                newModule.lastPackage = DateTime.MinValue;
                                                modules.Add(newModule);
                                                newModule.AddSend(message);
                                                AddNewValueForModule(newModule.address, newModule.rssi, newModule.countSend, newModule.countReceive, newModule.lastPackage, newModule.hop);
                                            }
                                        }
                                        break;
                                    }
                                case ("<SendUDP>"):
                                    {
                                        int findEnd = strIn.IndexOf(@"<\SendUDP>");
                                        string information = GetStringAndCath(ref strIn, 0, findEnd);

                                        RsMessage message = new RsMessage("SendUDP", information);
                                        listOfMessage.Add(message);

                                        {//send
                                            bool found = false;
                                            foreach (MessagesFromModule foundModule in modules)
                                            {
                                                if (foundModule.address == message.addr)
                                                {
                                                    foundModule.AddSend(message);
                                                    AddNewValueForModule(foundModule.address, foundModule.rssi, foundModule.countSend, foundModule.countReceive, foundModule.lastPackage, foundModule.hop);
                                                    found = true;
                                                    break;
                                                }
                                            }
                                            if (found == false)
                                            {
                                                MessagesFromModule newModule = new MessagesFromModule();
                                                newModule.address = message.addr;
                                                newModule.lastPackage = DateTime.MinValue;
                                                modules.Add(newModule);
                                                newModule.AddSend(message);
                                                AddNewValueForModule(newModule.address, newModule.rssi, newModule.countSend, newModule.countReceive, newModule.lastPackage, newModule.hop);
                                            }
                                        }
                                        break;
                                    }
                                case ("<ReceiveUDP>"):
                                    {
                                        int findEnd = strIn.IndexOf(@"<\ReceiveUDP>");
                                        string information = GetStringAndCath(ref strIn, 0, findEnd);

                                        RsMessage message = new RsMessage("ReceiveUDP", information);
                                        listOfMessage.Add(message);

                                        {//receive
                                            bool found = false;
                                            foreach (MessagesFromModule foundModule in modules)
                                            {
                                                if (foundModule.address == message.addr)
                                                {
                                                    foundModule.AddReceive(message);
                                                    AddNewValueForModule(foundModule.address, foundModule.rssi, foundModule.countSend, foundModule.countReceive, foundModule.lastPackage, foundModule.hop);
                                                    found = true;
                                                    break;
                                                }
                                            }
                                            if (found == false)
                                            {
                                                MessagesFromModule newModule = new MessagesFromModule();
                                                newModule.address = message.addr;
                                                newModule.lastPackage = DateTime.MinValue;
                                                modules.Add(newModule);
                                                newModule.AddReceive(message);
                                                AddNewValueForModule(newModule.address, newModule.rssi, newModule.countSend, newModule.countReceive, newModule.lastPackage, newModule.hop);
                                            }
                                        }
                                        break;
                                    }
                                case ("<Connection Status>"):
                                    {
                                        int findEnd = strIn.IndexOf(@"<\Connection Status>");
                                        string information = GetStringAndCath(ref strIn, 0, findEnd);
                                        listOfMessage.Add(new RsMessage("Connection Status", information));

                                        int indexOfRssi=information.IndexOf("RSSI = ");
                                        if (indexOfRssi>=0)
                                        {
                                            string rssi = information.Substring(indexOfRssi + "RSSI = ".Length, 4);

                                            if (information.Length > "Addr = ".Length + 39)
                                            {
                                                string addr = information.Substring(information.IndexOf("Addr = ") + "Addr = ".Length, 39);

                                                foreach (MessagesFromModule foundModule in modules)
                                                {
                                                    if (foundModule.address == addr)
                                                    {
                                                        foundModule.rssi = rssi;
                                                        {
                                                            int indexOfhop = information.IndexOf("HopCount = ");
                                                            if (indexOfhop > 0)
                                                            {
                                                                foundModule.hop = information.Substring(information.IndexOf("HopCount = ") + "HopCount = ".Length, 2);
                                                            }
                                                        }
                                                        AddNewValueForModule(foundModule.address, foundModule.rssi, foundModule.countSend, foundModule.countReceive, foundModule.lastPackage, foundModule.hop);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    }
                                case ("<PING>"):
                                    {
                                        int findEnd = strIn.IndexOf(@"<\PING>");
                                        string information = GetStringAndCath(ref strIn, 0, findEnd);
                                        listOfMessage.Add(new RsMessage("PING", information));
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                            listOfAllMessage.Add(strAfterCut);
                        }
                        else
                        {
                            //Wait for complete information from RS232
                            return;
                        }
                    }
                    indexStart = strIn.IndexOf('<');
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.BytesToRead > 0)
                {
                    byte[] received = new byte[serialPort1.BytesToRead];

                    serialPort1.Read(received, 0, received.Length);
                    commandToProcessingForRichTextBox += System.Text.Encoding.ASCII.GetString(received);

                    messageFromRS232ToParse += System.Text.Encoding.ASCII.GetString(received);

                    if (checkBoxAutoScroll.Checked == true)
                    {
                        //todo: do poprawy autoscroll
                        textBoxRsOut.Text += System.Text.Encoding.ASCII.GetString(received);

                        if (textBoxRsOut.Text.Length > 60000)
                        {
                            textBoxRsOut.Text = textBoxRsOut.Text.Substring(textBoxRsOut.Text.Length - 60000, 60000 - 1);
                        }

                        this.textBoxRsOut.SelectionStart = textBoxRsOut.Text.Length;
                        this.textBoxRsOut.ScrollToCaret();

                        //do prasowania
                        {
                            ParseCommand(ref messageFromRS232ToParse);
                        }
                    }
                }
            }
            catch { };
        }

        private void ButtonSend_Click(object sender, EventArgs e)
        {
            serialPort1.Write(textBoxToSend.Text + "\n");
        }

        private void Send_Click(object sender, EventArgs e)
        {
        }

        private void numericUpDownNeighborCacheSize_ValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("test");            
        }

        private void buttonNeighborCacheSize_Click(object sender, EventArgs e)
        {
            serialPort1.Write("ZSNCS " + numericUpDownNeighborCacheSize.Value + "\n");
        }

        private void buttonMaxSocketCount_Click(object sender, EventArgs e)
        {
            serialPort1.Write("ZSMSC " + numericUpDownMaxSocketCount.Value + "\n");
        }

        private void buttonRouteTimeout_Click(object sender, EventArgs e)
        {
            serialPort1.Write("ZSRT  " + numericUpDownRouteTimeout.Value + "\n");
        }

        private void buttonNeighborReachableTime_Click(object sender, EventArgs e)
        {
            serialPort1.Write("ZSNRT " + numericUpDownNeighborReachableTime.Value + "\n");
        }

        private void buttonMaxHopCount_Click(object sender, EventArgs e)
        {
            serialPort1.Write("ZSMHC " + numericUpDownMaxHopCount.Value + "\n");
        }

        private void buttonRouteMaxFailCount_Click(object sender, EventArgs e)
        {
            serialPort1.Write("ZSRMFC  " + numericUpDownRouteMaxFailCount.Value + "\n");
        }

        private void buttonRouteRequestMinLinkRssi_Click(object sender, EventArgs e)
        {
            serialPort1.Write("ZSRRMLR " + numericUpDownRouteRequestMinLinkRssi.Value + "\n");
        }

        private void buttonRouteRequestMinLinkRssiReduction_Click(object sender, EventArgs e)
        {
            serialPort1.Write("ZSRRMLRR " + numericUpDownRouteRequestMinLinkRssiReduction.Value + "\n");
        }

        private void buttonDoduplicateAddressDetection_Click(object sender, EventArgs e)
        {
            serialPort1.Write("ZSDAD " + numericUpDownDoduplicateAddressDetection.Value + "\n");
        }

        private void buttonDoRouterSolicitation_Click(object sender, EventArgs e)
        {
            serialPort1.Write("ZSDRS " + numericUpDownDoRouterSolicitation.Value + "\n");
        }

        private void buttonRouteRequestAttempts_Click(object sender, EventArgs e)
        {
            serialPort1.Write("ZSRRA " + numericUpDownRouteRequestAttempts.Value + "\n");
        }

        private void buttonHheaderCompressionContext1_Click(object sender, EventArgs e)
        {
            serialPort1.Write("ZSHCC1  " + numericUpDownHheaderCompressionContext1.Value + "\n");
        }

        private void buttonHheaderCompressionContext2_Click(object sender, EventArgs e)
        {
            serialPort1.Write("ZSHCC2  " + numericUpDownHheaderCompressionContext2.Value + "\n");
        }

        private void buttonHheaderCompressionContext3_Click(object sender, EventArgs e)
        {
            serialPort1.Write("ZSHCC3  " + numericUpDownHheaderCompressionContext3.Value + "\n");
        }

        private void buttonTransmitPower_Click(object sender, EventArgs e)
        {
            serialPort1.Write("ZSTP  " + numericUpDownTransmitPower.Value + "\n");
        }

        private void textBoxToSend_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                serialPort1.Write(textBoxToSend.Text + "\n");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                string textFromList = listBox1.Items[listBox1.SelectedIndex].ToString();
                int startIndex = textFromList.IndexOf('"') + 1;
                if (startIndex >= 0)
                {
                    int stopIndex = textFromList.IndexOf('"', startIndex);
                    if (stopIndex >= 0)
                    {
                        textBoxToSend.Text = textFromList.Substring(startIndex, stopIndex - startIndex);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            foreach (RsMessage msg in listOfMessage)
            {
                listBox2.Items.Add(msg.type + " " + msg.message);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string address = listView1.SelectedItems[0].Text;
                foreach (MessagesFromModule foundModule in modules)
                {
                    if (foundModule.address == address)
                    {
                        listBox2.Items.Clear();
                        foreach (RsMessage msg in foundModule.messages)
                        {
                            listBox2.Items.Add(msg.type + " " + msg.message);
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// clear textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            textBoxRsOut.Text = "";
        }

        /// <summary>
        /// clear textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBoxRsOut.Text = "";
        }

        /// <summary>
        /// send ping
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string address = listView1.SelectedItems[0].Text;
                serialPort1.Write("ZSPING " + address + "\n");
            }
        }
    }
}
