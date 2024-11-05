using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TakeimgIVI
{
    public partial class FormTestImage : Form
    {
        Bitmap Bitmap;
        public FormTestImage(Bitmap bitmap)
        {
            InitializeComponent();
            Bitmap = bitmap;
        }

        private void FormTestImage_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Bitmap;
        }
    }
}
