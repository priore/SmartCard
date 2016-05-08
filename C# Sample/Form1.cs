using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using Priore.Controls;

namespace Sample
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		internal System.Windows.Forms.Label lblMsg;
		internal System.Windows.Forms.GroupBox gbRead;
		internal System.Windows.Forms.Button cmdRead;
		internal System.Windows.Forms.TextBox txtStrR;
		internal System.Windows.Forms.Label Label9;
		internal System.Windows.Forms.ComboBox cbAddrR;
		internal System.Windows.Forms.Label Label10;
		internal System.Windows.Forms.GroupBox cbWrite;
		internal System.Windows.Forms.Button cmdWrite;
		internal System.Windows.Forms.TextBox txtStrW;
		internal System.Windows.Forms.Label Label6;
		internal System.Windows.Forms.ComboBox cbAddrW;
		internal System.Windows.Forms.Label Label5;
		internal System.Windows.Forms.ComboBox cbReaders;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.Button cmdReadAPDU;
		internal System.Windows.Forms.Button cmdWriteAPDU;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		
		// smartcard object
		private Priore.Controls.SmartCard scard;

		// CSC values (for GemClubMemo)
		string sAdmin = System.String.Concat((char)0xAA,(char)0xAA,(char)0xAA,(char)0xAA);
		string sUser1 = System.String.Concat((char)0x11,(char)0x11,(char)0x11,(char)0x11);
		string sUser2 = System.String.Concat((char)0x22,(char)0x22,(char)0x22,(char)0x22);

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			// inizialize smartcard object
			scard = new SmartCard();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
					
					// release smartcard object
					scard.Dispose();

				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.lblMsg = new System.Windows.Forms.Label();
            this.gbRead = new System.Windows.Forms.GroupBox();
            this.cmdReadAPDU = new System.Windows.Forms.Button();
            this.cmdRead = new System.Windows.Forms.Button();
            this.txtStrR = new System.Windows.Forms.TextBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.cbAddrR = new System.Windows.Forms.ComboBox();
            this.Label10 = new System.Windows.Forms.Label();
            this.cbWrite = new System.Windows.Forms.GroupBox();
            this.cmdWriteAPDU = new System.Windows.Forms.Button();
            this.cmdWrite = new System.Windows.Forms.Button();
            this.txtStrW = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.cbAddrW = new System.Windows.Forms.ComboBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.cbReaders = new System.Windows.Forms.ComboBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.gbRead.SuspendLayout();
            this.cbWrite.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMsg
            // 
            this.lblMsg.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMsg.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblMsg.Location = new System.Drawing.Point(12, 421);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(768, 37);
            this.lblMsg.TabIndex = 11;
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gbRead
            // 
            this.gbRead.Controls.Add(this.cmdReadAPDU);
            this.gbRead.Controls.Add(this.cmdRead);
            this.gbRead.Controls.Add(this.txtStrR);
            this.gbRead.Controls.Add(this.Label9);
            this.gbRead.Controls.Add(this.cbAddrR);
            this.gbRead.Controls.Add(this.Label10);
            this.gbRead.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.gbRead.Location = new System.Drawing.Point(12, 244);
            this.gbRead.Name = "gbRead";
            this.gbRead.Size = new System.Drawing.Size(772, 158);
            this.gbRead.TabIndex = 10;
            this.gbRead.TabStop = false;
            this.gbRead.Text = "Read Data (Address = hex, Data  =  chars)";
            // 
            // cmdReadAPDU
            // 
            this.cmdReadAPDU.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdReadAPDU.Location = new System.Drawing.Point(600, 100);
            this.cmdReadAPDU.Name = "cmdReadAPDU";
            this.cmdReadAPDU.Size = new System.Drawing.Size(150, 42);
            this.cmdReadAPDU.TabIndex = 7;
            this.cmdReadAPDU.Text = "with APDU";
            this.cmdReadAPDU.Click += new System.EventHandler(this.cmdReadAPDU_Click);
            // 
            // cmdRead
            // 
            this.cmdRead.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdRead.Location = new System.Drawing.Point(600, 44);
            this.cmdRead.Name = "cmdRead";
            this.cmdRead.Size = new System.Drawing.Size(150, 43);
            this.cmdRead.TabIndex = 6;
            this.cmdRead.Text = "Read";
            this.cmdRead.Click += new System.EventHandler(this.cmdRead_Click);
            // 
            // txtStrR
            // 
            this.txtStrR.BackColor = System.Drawing.SystemColors.Window;
            this.txtStrR.Location = new System.Drawing.Point(464, 44);
            this.txtStrR.MaxLength = 4;
            this.txtStrR.Name = "txtStrR";
            this.txtStrR.ReadOnly = true;
            this.txtStrR.Size = new System.Drawing.Size(100, 31);
            this.txtStrR.TabIndex = 3;
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(288, 52);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(159, 25);
            this.Label9.TabIndex = 2;
            this.Label9.Text = "String (4 chars)";
            // 
            // cbAddrR
            // 
            this.cbAddrR.Location = new System.Drawing.Point(128, 44);
            this.cbAddrR.Name = "cbAddrR";
            this.cbAddrR.Size = new System.Drawing.Size(144, 33);
            this.cbAddrR.TabIndex = 1;
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(24, 52);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(91, 25);
            this.Label10.TabIndex = 0;
            this.Label10.Text = "Address";
            // 
            // cbWrite
            // 
            this.cbWrite.Controls.Add(this.cmdWriteAPDU);
            this.cbWrite.Controls.Add(this.cmdWrite);
            this.cbWrite.Controls.Add(this.txtStrW);
            this.cbWrite.Controls.Add(this.Label6);
            this.cbWrite.Controls.Add(this.cbAddrW);
            this.cbWrite.Controls.Add(this.Label5);
            this.cbWrite.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbWrite.Location = new System.Drawing.Point(12, 66);
            this.cbWrite.Name = "cbWrite";
            this.cbWrite.Size = new System.Drawing.Size(772, 170);
            this.cbWrite.TabIndex = 9;
            this.cbWrite.TabStop = false;
            this.cbWrite.Text = "Write Data (Address = hex, Data  =  chars)";
            // 
            // cmdWriteAPDU
            // 
            this.cmdWriteAPDU.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdWriteAPDU.Location = new System.Drawing.Point(600, 100);
            this.cmdWriteAPDU.Name = "cmdWriteAPDU";
            this.cmdWriteAPDU.Size = new System.Drawing.Size(150, 42);
            this.cmdWriteAPDU.TabIndex = 7;
            this.cmdWriteAPDU.Text = "with APDU";
            this.cmdWriteAPDU.Click += new System.EventHandler(this.cmdWriteAPDU_Click);
            // 
            // cmdWrite
            // 
            this.cmdWrite.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdWrite.Location = new System.Drawing.Point(600, 44);
            this.cmdWrite.Name = "cmdWrite";
            this.cmdWrite.Size = new System.Drawing.Size(150, 43);
            this.cmdWrite.TabIndex = 6;
            this.cmdWrite.Text = "Write";
            this.cmdWrite.Click += new System.EventHandler(this.cmdWrite_Click);
            // 
            // txtStrW
            // 
            this.txtStrW.Location = new System.Drawing.Point(464, 44);
            this.txtStrW.MaxLength = 4;
            this.txtStrW.Name = "txtStrW";
            this.txtStrW.Size = new System.Drawing.Size(100, 31);
            this.txtStrW.TabIndex = 3;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(288, 55);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(159, 25);
            this.Label6.TabIndex = 2;
            this.Label6.Text = "String (4 chars)";
            // 
            // cbAddrW
            // 
            this.cbAddrW.Location = new System.Drawing.Point(128, 44);
            this.cbAddrW.Name = "cbAddrW";
            this.cbAddrW.Size = new System.Drawing.Size(144, 33);
            this.cbAddrW.TabIndex = 1;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(32, 55);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(91, 25);
            this.Label5.TabIndex = 0;
            this.Label5.Text = "Address";
            // 
            // cbReaders
            // 
            this.cbReaders.Location = new System.Drawing.Point(168, 15);
            this.cbReaders.Name = "cbReaders";
            this.cbReaders.Size = new System.Drawing.Size(612, 33);
            this.cbReaders.TabIndex = 7;
            this.cbReaders.SelectedIndexChanged += new System.EventHandler(this.cbReaders_SelectedIndexChanged);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(12, 22);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(144, 25);
            this.Label1.TabIndex = 6;
            this.Label1.Text = "Reader Name";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(10, 24);
            this.ClientSize = new System.Drawing.Size(800, 489);
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.gbRead);
            this.Controls.Add(this.cbWrite);
            this.Controls.Add(this.cbReaders);
            this.Controls.Add(this.Label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gbRead.ResumeLayout(false);
            this.gbRead.PerformLayout();
            this.cbWrite.ResumeLayout(false);
            this.cbWrite.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			string hex;

			// fill combobox with valid address range
			for(int i=8; i<56; i++)		
			{
				hex = String.Format("{0:x2}", i);
				cbAddrW.Items.Add("0x" + hex);
				cbAddrR.Items.Add("0x" + hex);
			}

			// fill combobox with list of the readers
			string[] rdrlst = scard.Readers();
			for (int i = 0; i < rdrlst.Length; i++)
			{
				cbReaders.Items.Add(rdrlst[i]);
			}

			// select first values
			cbAddrW.SelectedIndex = 0;
			cbAddrR.SelectedIndex = 0;
			cbReaders.SelectedIndex = 0;

			// start
			scard.ReaderName = rdrlst[0];
			scard.Interval = 1000;
			scard.Enabled = true;
		}

		private void cbReaders_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			scard.ReaderName = cbReaders.Text;
		}

		private void cmdWrite_Click(object sender, System.EventArgs e)
		{
			int addr = System.Convert.ToInt32(cbAddrW.Text, 16);
			scard.Connect();

			// send CSC administrator (full permissions)
			scard.SendCSC(sAdmin,SmartCard.CSCTypeConstants.CSC_ADMIN);
			if (scard.StatusWord != SmartCard.StatusWordConstants.SWORD_COMMAND_SUCCESS)
			{
				MessageBox.Show("CSC Administrator Error.", "Write Card", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				scard.Disconnect();
				return;
			}

			// send CSC User Area 1
			scard.SendCSC(sUser1, SmartCard.CSCTypeConstants.CSC_USER1);
			if (scard.StatusWord!=SmartCard.StatusWordConstants.SWORD_COMMAND_SUCCESS)
			{
				MessageBox.Show("CSC User Area 1 Error.", "Write Card", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				scard.Disconnect();
				return;
			}

			// send CSC User Area 2
			scard.SendCSC(sUser2, SmartCard.CSCTypeConstants.CSC_USER2);
			if (scard.StatusWord!=SmartCard.StatusWordConstants.SWORD_COMMAND_SUCCESS)
			{
				MessageBox.Show("CSC User Area 2 Error.", "Write Card", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				scard.Disconnect();
				return;
			}

			// write string
			scard.WriteCard(addr, txtStrW.Text);

			// check errors
			SmartCard.StatusWordConstants ret = scard.StatusWord;
			if (ret!=SmartCard.StatusWordConstants.SWORD_COMMAND_SUCCESS)
			{
				MessageBox.Show("Write data error (" + ret.ToString() + ")", "Write Card", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			} 
			else 
			{
				MessageBox.Show("Write data correctly.", "Write Card", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			// disconnect
			scard.Disconnect();
		}

		private void cmdRead_Click(object sender, System.EventArgs e)
		{
			int addr = System.Convert.ToInt32(cbAddrR.Text, 16);
			scard.Connect();

			// send CSC administrator (full permissions)
			scard.SendCSC(sAdmin,SmartCard.CSCTypeConstants.CSC_ADMIN);
			if (scard.StatusWord != SmartCard.StatusWordConstants.SWORD_COMMAND_SUCCESS)
			{
				MessageBox.Show("CSC Administrator Error", "Read Card", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				scard.Disconnect();
				return;
			}

			// send CSC User Area 1
			scard.SendCSC(sUser1, SmartCard.CSCTypeConstants.CSC_USER1);
			if (scard.StatusWord!=SmartCard.StatusWordConstants.SWORD_COMMAND_SUCCESS)
			{
				MessageBox.Show("CSC User Area 1 Error", "Read Card", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				scard.Disconnect();
				return;
			}

			// send CSC User Area 2
			scard.SendCSC(sUser2, SmartCard.CSCTypeConstants.CSC_USER2);
			if (scard.StatusWord!=SmartCard.StatusWordConstants.SWORD_COMMAND_SUCCESS)
			{
				MessageBox.Show("CSC User Area 2 Error.", "Read Card", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				scard.Disconnect();
				return;
			}

			// show data
			txtStrR.Text = scard.ReadCard(addr);

			// check errors
			SmartCard.StatusWordConstants ret = scard.StatusWord;
			if (ret!=SmartCard.StatusWordConstants.SWORD_COMMAND_SUCCESS)
			{
				MessageBox.Show("Read data error (" + ret.ToString() + ")", "Read Card", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			} 

			// disconnect
			scard.Disconnect();
		}

		private void cmdReadAPDU_Click(object sender, System.EventArgs e)
		{
			byte addr = System.Convert.ToByte(cbAddrR.Text, 16);
			scard.Connect();

			// buffers
			byte[] asend = null;		// no send more data
			byte[] arecv = new byte[4];	// only receive data
			
			// APDU Command (GemClubMemo)
			scard.APDUCommand(0x80, 0xBE, 0x00, addr, 0x04, ref asend, ref arecv);
			if (scard.StatusWord!=SmartCard.StatusWordConstants.SWORD_COMMAND_SUCCESS)
			{
				MessageBox.Show("Read data error.", "Write Card", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				scard.Disconnect();
				return;
			} 

			// show data
			txtStrR.Text = Encoding.ASCII.GetString(arecv,0,4);

			// disconnect
			scard.Disconnect();
		}

		private void cmdWriteAPDU_Click(object sender, System.EventArgs e)
		{
			byte[] asend = null;
			byte[] arecv = null;

			byte addr = System.Convert.ToByte(cbAddrR.Text, 16);
			scard.Connect();

			// buffers for CSC Admin
			asend = new byte[4] {0xAA, 0xAA, 0xAA, 0xAA};	// send data
			arecv = null;									// no recevie data

			// send CSC Administrator
			scard.APDUCommand(0x00, 0x20, 0x00, 0x07, 0x04, ref asend, ref arecv);
			if (scard.StatusWord!=SmartCard.StatusWordConstants.SWORD_COMMAND_SUCCESS)
			{
				MessageBox.Show("CSC Administrator Error", "Read Card", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				scard.Disconnect();
				return;
			} 

			// buffers for CSC User Area 1
			asend = new byte[4] {0x11, 0x11, 0x11, 0x11};	// send data
			arecv = null;									// no recevie data

			// send CSC User Area 1
			scard.APDUCommand(0x00, 0x20, 0x00, 0x39, 0x04, ref asend, ref arecv);
			if (scard.StatusWord!=SmartCard.StatusWordConstants.SWORD_COMMAND_SUCCESS)
			{
				MessageBox.Show("CSC User Area 1 Error", "Read Card", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				scard.Disconnect();
				return;
			} 

			// buffers for CSC User Area 2
			asend = new byte[4] {0x22, 0x22, 0x22, 0x22};	// send data
			arecv = null;									// no recevie data

			// send CSC User Area 2
			scard.APDUCommand(0x00, 0x20, 0x00, 0x3B, 0x04, ref asend, ref arecv);
			if (scard.StatusWord!=SmartCard.StatusWordConstants.SWORD_COMMAND_SUCCESS)
			{
				MessageBox.Show("CSC User Area 2 Error", "Read Card", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				scard.Disconnect();
				return;
			} 

			// buffers
			asend = Encoding.ASCII.GetBytes(txtStrW.Text);	// send data
			arecv = null;									// no recevie data
			
			// APDU Command (GemClubMemo)
			scard.APDUCommand(0x80, 0xDE, 0x00, addr, 0x04, ref asend, ref arecv);
			if (scard.StatusWord!=SmartCard.StatusWordConstants.SWORD_COMMAND_SUCCESS)
			{
				MessageBox.Show("Write data error.", "Write Card", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			} 
			else 
			{
				MessageBox.Show("Write data correctly.", "Write Card", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			// disconnect
			scard.Disconnect();
		}

	}
}
