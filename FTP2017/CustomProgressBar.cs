using System.Drawing;
using System.Windows.Forms;

namespace FTP2017
{
	public class CustomProgressBar : ProgressBar
    {
        private ProgressBar progressBar1;

        public CustomProgressBar()
		{
			SetStyle(ControlStyles.UserPaint, value: true);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			SolidBrush brush = null;
			Rectangle bounds = new Rectangle(0, 0, base.Width, base.Height);
			e.Graphics.FillRectangle(new SolidBrush(BackColor), 1, 1, bounds.Width - 2, bounds.Height - 2);
			bounds.Height -= 4;
			bounds.Width = (int)((double)bounds.Width * ((double)base.Value / (double)base.Maximum)) - 4;
			brush = new SolidBrush(ForeColor);
			e.Graphics.FillRectangle(brush, 2, 2, bounds.Width, bounds.Height);
		}

        private void InitializeComponent()
        {
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 23);
            this.progressBar1.TabIndex = 0;
            this.ResumeLayout(false);

        }
    }
}
