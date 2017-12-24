namespace DrawingsManager
{
	partial class DrawingManagerForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DrawingManagerForm));
			this.panel1 = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
			this.dataGridView2 = new System.Windows.Forms.DataGridView();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.buttonRefresh = new System.Windows.Forms.Button();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonOpenDrawing = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			this.panel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.tableLayoutPanel1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1323, 692);
			this.panel1.TabIndex = 2;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::DrawingsManager.Properties.Resources.Logo;
			this.pictureBox1.Location = new System.Drawing.Point(10, 10);
			this.pictureBox1.Margin = new System.Windows.Forms.Padding(10);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(163, 40);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToOrderColumns = true;
			this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.GridColor = System.Drawing.SystemColors.Control;
			this.dataGridView1.Location = new System.Drawing.Point(3, 103);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowTemplate.Height = 24;
			this.dataGridView1.Size = new System.Drawing.Size(1317, 325);
			this.dataGridView1.TabIndex = 0;
			this.dataGridView1.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowEnter);
			this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 4);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 5;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(1323, 692);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.buttonOpenDrawing);
			this.panel2.Controls.Add(this.buttonRefresh);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(3, 63);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(1317, 34);
			this.panel2.TabIndex = 4;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.label3);
			this.panel3.Controls.Add(this.label1);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(3, 434);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(1317, 34);
			this.panel3.TabIndex = 5;
			// 
			// dataGridView2
			// 
			this.dataGridView2.AllowUserToAddRows = false;
			this.dataGridView2.AllowUserToDeleteRows = false;
			this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
			this.dataGridView2.BackgroundColor = System.Drawing.SystemColors.Window;
			this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView2.GridColor = System.Drawing.SystemColors.Control;
			this.dataGridView2.Location = new System.Drawing.Point(3, 3);
			this.dataGridView2.Name = "dataGridView2";
			this.dataGridView2.ReadOnly = true;
			this.dataGridView2.RowTemplate.Height = 24;
			this.dataGridView2.Size = new System.Drawing.Size(850, 209);
			this.dataGridView2.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(4, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(103, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Bill of Materials";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(4, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(85, 17);
			this.label2.TabIndex = 0;
			this.label2.Text = "Drawing List";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
			this.tableLayoutPanel2.Controls.Add(this.dataGridView2, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.panel4, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 474);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(1317, 215);
			this.tableLayoutPanel2.TabIndex = 6;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.richTextBox1);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(859, 3);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(455, 209);
			this.panel4.TabIndex = 1;
			// 
			// buttonRefresh
			// 
			this.buttonRefresh.Location = new System.Drawing.Point(1181, -1);
			this.buttonRefresh.Name = "buttonRefresh";
			this.buttonRefresh.Size = new System.Drawing.Size(127, 35);
			this.buttonRefresh.TabIndex = 0;
			this.buttonRefresh.Text = "Refresh";
			this.buttonRefresh.UseVisualStyleBackColor = true;
			this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
			// 
			// richTextBox1
			// 
			this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox1.Location = new System.Drawing.Point(0, 0);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(455, 209);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = "";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(856, 20);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(102, 17);
			this.label3.TabIndex = 1;
			this.label3.Text = "Json Part Data";
			// 
			// buttonOpenDrawing
			// 
			this.buttonOpenDrawing.Location = new System.Drawing.Point(1048, -1);
			this.buttonOpenDrawing.Name = "buttonOpenDrawing";
			this.buttonOpenDrawing.Size = new System.Drawing.Size(127, 35);
			this.buttonOpenDrawing.TabIndex = 1;
			this.buttonOpenDrawing.Text = "Open";
			this.buttonOpenDrawing.UseVisualStyleBackColor = true;
			this.buttonOpenDrawing.Click += new System.EventHandler(this.buttonOpenDrawing_Click);
			// 
			// DrawingManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(1323, 692);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "DrawingManagerForm";
			this.Text = "Drawing Manager";
			this.Load += new System.EventHandler(this.DrawingManager_Load);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.DataGridView dataGridView2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Button buttonRefresh;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button buttonOpenDrawing;
	}
}

