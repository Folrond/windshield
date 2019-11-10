using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SensorManager.Manager;

namespace Debug
{
    public partial class Form1 : Form
    {
        private SensorHubManager manager;
        


        public Form1()
        {
            InitializeComponent();
            manager = new SensorHubManager();
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            Task.Run(() =>
            {
                while (true)
                {
                    pictureBox1.Invoke((MethodInvoker)delegate
                   {
                       pictureBox1.Image?.Dispose();
                       pictureBox1.Image = manager.RecievedA?.Data?.Clone() as Bitmap;
                   });

                    pictureBox2.Invoke((MethodInvoker) delegate
                    {
                        pictureBox2.Image?.Dispose();
                        pictureBox2.Image = manager.RecievedB?.Data?.Clone() as Bitmap;
                    });
                   
                    Thread.Sleep(300);
                   
                }

            });
            manager.Start();

        }
    }
}
