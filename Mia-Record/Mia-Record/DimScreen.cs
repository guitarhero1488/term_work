using Mia_Record.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mia_Record
{
    public partial class DimScreen : Form
    {
        public DimScreen()
        {
            InitializeComponent();
        }

        private void DimScreen_Load(object sender, EventArgs e)
        {
            try
            {
                panel1.Location = new Point(ActiveForm.Width / 2 - panel1.Size.Width / 2, ActiveForm.Height / 2 - panel1.Size.Height / 2);
                panel1.Anchor = AnchorStyles.None;
            }
            catch (Exception)
            {
            }
        }
    }
}
