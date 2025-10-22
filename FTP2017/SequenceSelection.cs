using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FTP2017
{
	public class SequenceSelection : Form
	{
		public int SeqIndex = 0;

		private List<SequenceSetting> _SeqSettingList;

		private IContainer components = null;

		private Label label1;

		private Button button_OK;

		private ListView listView_SequenceList;

		private ColumnHeader columnHeader1;

		private ColumnHeader columnHeader2;

		private ColumnHeader columnHeader3;

		private ColumnHeader columnHeader4;

		public SequenceSelection()
		{
			InitializeComponent();
		}

		public SequenceSelection(List<SequenceSetting> SeqSettingList)
		{
			InitializeComponent();
			_SeqSettingList = SeqSettingList;
		}

		private void button_OK_Click(object sender, EventArgs e)
		{
			if (listView_SequenceList.SelectedIndices.Count > 0)
			{
				SeqIndex = listView_SequenceList.SelectedIndices[0];
			}
			else
			{
				SeqIndex = 0;
			}
			base.DialogResult = DialogResult.OK;
		}

		private void SequenceSelection_Load(object sender, EventArgs e)
		{
			for (int i = 0; i < _SeqSettingList.Count; i++)
			{
				ListViewItem lvi = new ListViewItem(_SeqSettingList[i].ProjectName);
				lvi.SubItems.Add(_SeqSettingList[i].StationName);
				lvi.SubItems.Add(_SeqSettingList[i].SequenceFile.Substring(_SeqSettingList[i].SequenceFile.LastIndexOf("\\") + 1));
				lvi.SubItems.Add(_SeqSettingList[i].Version);
				listView_SequenceList.Items.Add(lvi);
			}
		}

		private void SequenceSelection_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				base.DialogResult = DialogResult.Cancel;
			}
		}

		private void listView_SequenceList_DoubleClick(object sender, EventArgs e)
		{
			if (listView_SequenceList.SelectedIndices.Count > 0)
			{
				SeqIndex = listView_SequenceList.SelectedIndices[0];
			}
			else
			{
				SeqIndex = 0;
			}
			base.DialogResult = DialogResult.OK;
		}

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
			this.label1 = new System.Windows.Forms.Label();
			this.button_OK = new System.Windows.Forms.Button();
			this.listView_SequenceList = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("SimSun", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
			this.label1.Location = new System.Drawing.Point(12, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(178, 21);
			this.label1.TabIndex = 1;
			this.label1.Text = "Sequence List:";
			this.button_OK.Location = new System.Drawing.Point(261, 404);
			this.button_OK.Name = "button_OK";
			this.button_OK.Size = new System.Drawing.Size(91, 29);
			this.button_OK.TabIndex = 2;
			this.button_OK.Text = "OK";
			this.button_OK.UseVisualStyleBackColor = true;
			this.button_OK.Click += new System.EventHandler(button_OK_Click);
			this.listView_SequenceList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[4] { this.columnHeader1, this.columnHeader2, this.columnHeader3, this.columnHeader4 });
			this.listView_SequenceList.Font = new System.Drawing.Font("SimSun", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.listView_SequenceList.FullRowSelect = true;
			this.listView_SequenceList.GridLines = true;
			this.listView_SequenceList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView_SequenceList.HideSelection = false;
			this.listView_SequenceList.Location = new System.Drawing.Point(12, 51);
			this.listView_SequenceList.MultiSelect = false;
			this.listView_SequenceList.Name = "listView_SequenceList";
			this.listView_SequenceList.Size = new System.Drawing.Size(610, 337);
			this.listView_SequenceList.TabIndex = 3;
			this.listView_SequenceList.UseCompatibleStateImageBehavior = false;
			this.listView_SequenceList.View = System.Windows.Forms.View.Details;
			this.listView_SequenceList.DoubleClick += new System.EventHandler(listView_SequenceList_DoubleClick);
			this.columnHeader1.Text = "Project Name";
			this.columnHeader1.Width = 150;
			this.columnHeader2.Text = "Station Name";
			this.columnHeader2.Width = 150;
			this.columnHeader3.Text = "Sequence File";
			this.columnHeader3.Width = 190;
			this.columnHeader4.Text = "Version";
			this.columnHeader4.Width = 100;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(636, 445);
			base.Controls.Add(this.listView_SequenceList);
			base.Controls.Add(this.button_OK);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SequenceSelection";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Selecte Sequence";
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(SequenceSelection_FormClosed);
			base.Load += new System.EventHandler(SequenceSelection_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
