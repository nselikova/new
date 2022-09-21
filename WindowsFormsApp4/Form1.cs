using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {        
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            xImageGallery1.AddImage(openFileDialog1.FileName);
            xImageGallery1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            xImageGallery1.SwapImage((int)numericUpDown1.Value, (int)numericUpDown2.Value);
            xImageGallery1.Focus();
        }
        /// <summary>
        /// чтобы показать что при выборе картинки происходит событие
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xImageGallery1_OnImageSelect(object sender, EventArgs e)
        {
            label1.Text = $"Выбрана {xImageGallery1.indShowImage} картинка";
        }
    }
}
