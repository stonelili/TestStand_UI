using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FTP2017
{
	public class InstrumentConfig : Form
	{
		private IContainer components = null;

		private DataGridView dataGridView_InstrumentList;

		private Label label1;

		private Button button_OK;

		private DataGridViewTextBoxColumn Column1;

		private DataGridViewTextBoxColumn Column2;

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.dataGridView_InstrumentList = new System.Windows.Forms.DataGridView();
			this.label1 = new System.Windows.Forms.Label();
			this.button_OK = new System.Windows.Forms.Button();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)this.dataGridView_InstrumentList).BeginInit();
			base.SuspendLayout();
			this.dataGridView_InstrumentList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView_InstrumentList.Columns.AddRange(this.Column1, this.Column2);
			this.dataGridView_InstrumentList.Location = new System.Drawing.Point(12, 36);
			this.dataGridView_InstrumentList.Name = "dataGridView_InstrumentList";
			this.dataGridView_InstrumentList.RowHeadersWidth = 20;
			this.dataGridView_InstrumentList.RowTemplate.Height = 23;
			this.dataGridView_InstrumentList.Size = new System.Drawing.Size(543, 382);
			this.dataGridView_InstrumentList.TabIndex = 0;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(161, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "Instrument Resource Config";
			this.button_OK.Location = new System.Drawing.Point(254, 432);
			this.button_OK.Name = "button_OK";
			this.button_OK.Size = new System.Drawing.Size(75, 23);
			this.button_OK.TabIndex = 2;
			this.button_OK.Text = "OK";
			this.button_OK.UseVisualStyleBackColor = true;
			this.Column1.HeaderText = "Type Name";
			this.Column1.Name = "Column1";
			this.Column1.Width = 180;
			this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Column2.HeaderText = "Resource";
			this.Column2.Name = "Column2";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(567, 467);
			base.Controls.Add(this.button_OK);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.dataGridView_InstrumentList);
			base.Name = "InstrumentConfig";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "InstrumentConfig";
			((System.ComponentModel.ISupportInitialize)this.dataGridView_InstrumentList).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		public InstrumentConfig()
		{
			InitializeComponent();
		}
	}
}
