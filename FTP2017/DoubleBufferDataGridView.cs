using System.Windows.Forms;

namespace FTP2017
{
	public class DoubleBufferDataGridView : DataGridView
    {
        private DataGridView dataGridView_ResultList;

        public DoubleBufferDataGridView()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, value: true);
			UpdateStyles();
		}

        private void InitializeComponent()
        {
            this.dataGridView_ResultList = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ResultList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_ResultList
            // 
            this.dataGridView_ResultList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_ResultList.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView_ResultList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_ResultList.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_ResultList.Name = "dataGridView_ResultList";
            this.dataGridView_ResultList.RowTemplate.Height = 27;
            this.dataGridView_ResultList.Size = new System.Drawing.Size(240, 150);
            this.dataGridView_ResultList.TabIndex = 0;
            // 
            // DoubleBufferDataGridView
            // 
            this.RowTemplate.Height = 27;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_ResultList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
