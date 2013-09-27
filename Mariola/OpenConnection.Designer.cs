namespace Mariola
{
    partial class OpenConnection
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ButtonConnect = new System.Windows.Forms.Button();
            this.listBoxPorts = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ButtonConnect
            // 
            this.ButtonConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonConnect.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ButtonConnect.Enabled = false;
            this.ButtonConnect.Location = new System.Drawing.Point(99, 166);
            this.ButtonConnect.Name = "ButtonConnect";
            this.ButtonConnect.Size = new System.Drawing.Size(75, 23);
            this.ButtonConnect.TabIndex = 0;
            this.ButtonConnect.Text = "Connect";
            this.ButtonConnect.UseVisualStyleBackColor = true;
            // 
            // listBoxPorts
            // 
            this.listBoxPorts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxPorts.FormattingEnabled = true;
            this.listBoxPorts.Location = new System.Drawing.Point(13, 26);
            this.listBoxPorts.Name = "listBoxPorts";
            this.listBoxPorts.Size = new System.Drawing.Size(159, 134);
            this.listBoxPorts.TabIndex = 1;
            this.listBoxPorts.SelectedIndexChanged += new System.EventHandler(this.listBoxPorts_SelectedIndexChanged);
            this.listBoxPorts.DoubleClick += new System.EventHandler(this.listBoxPorts_DoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Serial Port";
            // 
            // OpenConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(186, 197);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxPorts);
            this.Controls.Add(this.ButtonConnect);
            this.Name = "OpenConnection";
            this.Text = "OpenConnection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonConnect;
        public System.Windows.Forms.ListBox listBoxPorts;
        private System.Windows.Forms.Label label1;
    }
}