namespace all_rgb_gui
{
	partial class BetterTrackBar
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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


		private void InitializeComponent()
		{
			this.trbInner = new System.Windows.Forms.TrackBar();
			this.tbValue = new System.Windows.Forms.TextBox();
			this.lblCaption = new System.Windows.Forms.Label();
			this.tlpContainer = new System.Windows.Forms.TableLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.trbInner)).BeginInit();
			this.tlpContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// trbInner
			// 
			this.trbInner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.trbInner.AutoSize = false;
			this.trbInner.BackColor = System.Drawing.SystemColors.ControlLight;
			this.trbInner.Location = new System.Drawing.Point(56, 3);
			this.trbInner.Maximum = 100;
			this.trbInner.Name = "trbInner";
			this.trbInner.Size = new System.Drawing.Size(405, 24);
			this.trbInner.TabIndex = 0;
			this.trbInner.ValueChanged += new System.EventHandler(this.trbInner_ValueChanged);
			// 
			// tbValue
			// 
			this.tbValue.BackColor = System.Drawing.SystemColors.ControlLight;
			this.tbValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tbValue.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbValue.Location = new System.Drawing.Point(467, 3);
			this.tbValue.Name = "tbValue";
			this.tbValue.Size = new System.Drawing.Size(42, 16);
			this.tbValue.TabIndex = 1;
			this.tbValue.Text = "0";
			this.tbValue.TextChanged += new System.EventHandler(this.tbValue_TextChanged);
			// 
			// lblCaption
			// 
			this.lblCaption.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblCaption.AutoSize = true;
			this.lblCaption.BackColor = System.Drawing.SystemColors.ControlLight;
			this.lblCaption.Location = new System.Drawing.Point(3, 3);
			this.lblCaption.Margin = new System.Windows.Forms.Padding(3);
			this.lblCaption.Name = "lblCaption";
			this.lblCaption.Size = new System.Drawing.Size(47, 24);
			this.lblCaption.TabIndex = 2;
			this.lblCaption.Text = "<desc>";
			// 
			// tlpContainer
			// 
			this.tlpContainer.BackColor = System.Drawing.SystemColors.ControlLight;
			this.tlpContainer.ColumnCount = 3;
			this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48F));
			this.tlpContainer.Controls.Add(this.lblCaption, 0, 0);
			this.tlpContainer.Controls.Add(this.trbInner, 1, 0);
			this.tlpContainer.Controls.Add(this.tbValue, 2, 0);
			this.tlpContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpContainer.Location = new System.Drawing.Point(0, 0);
			this.tlpContainer.Name = "tlpContainer";
			this.tlpContainer.RowCount = 1;
			this.tlpContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpContainer.Size = new System.Drawing.Size(512, 30);
			this.tlpContainer.TabIndex = 3;
			// 
			// BetterTrackBar
			// 
			this.BackColor = System.Drawing.SystemColors.ControlLight;
			this.Controls.Add(this.tlpContainer);
			this.Name = "BetterTrackBar";
			this.Size = new System.Drawing.Size(512, 30);
			((System.ComponentModel.ISupportInitialize)(this.trbInner)).EndInit();
			this.tlpContainer.ResumeLayout(false);
			this.tlpContainer.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.TrackBar trbInner;
		private System.Windows.Forms.Label lblCaption;
		private System.Windows.Forms.TextBox tbValue;
		private System.Windows.Forms.TableLayoutPanel tlpContainer;
	}
}

