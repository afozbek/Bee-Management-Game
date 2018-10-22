using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeeComplexForm
{
    public partial class Form1 : Form
    {
        World world;
        private Random random;
        private DateTime start = DateTime.Now;
        private DateTime end;
        private int framesRun = 0;

        public Form1()
        {
            InitializeComponent();
            world = new World(new BeeMessage(SendMessage));

            timer1.Interval = 50;

            //We will create our new event-->Form1.RunFrame(object sender , EventArgs e)
            timer1.Tick += new EventHandler(RunFrame);
            timer1.Enabled = false;
            UpdateStats(new TimeSpan());
        }

        //Update form's labels and stuff
        private void UpdateStats(TimeSpan frameDuration)
        {
            double nectar = 0;

            Bees.Text = world.Bees.Count.ToString();
            Flowers.Text = world.Flowers.Count.ToString();
            HoneyInHive.Text = String.Format("{0:F3}" , world.Hive.Honey);

            foreach (Flower flower in world.Flowers)
                nectar += flower.Nectar;

            NectarInFlowers.Text = String.Format("{0:F3}" , nectar);
            FramesRun.Text = framesRun.ToString();
            double milliSeconds = frameDuration.TotalMilliseconds;

            if (milliSeconds != 0.0)
                FrameRate.Text = string.Format("{0:f0} ({1:f1} ms)" ,
            1000 / milliSeconds , milliSeconds);

            else
                FrameRate.Text = "N / A";
        }

        private void SendMessage(int ID , string Message)
        {
            statusStrip1.Items[0].Text = "Bee #" + ID + ": " + Message;

            //Linq
            var beeGroups =
                from bee in world.Bees
                group bee by bee.CurrentState into beeGroup
                orderby beeGroup.Key
                select beeGroup;

            listBox1.Items.Clear();

            foreach (var group in beeGroups)
            {
                string s;
                //This bit of code makes sure it says, “1 bee” and “3 bees”, keeping the plural right.
                if (group.Count() == 1)
                    s = "";
                else
                    s = "s";

                listBox1.Items.Add(group.Key.ToString() + ": " 
                                                        + group.Count() + " bee" + s);

                if (group.Key == Bee.BeeState.Idle
                    && group.Count() == world.Bees.Count()
                    && framesRun > 0)
                {
                    listBox1.Items.Add("Simulation ended: all bees are idle");
                    toolStrip1.Items[0].Text = "Simulation ended";
                    statusStrip1.Items[0].Text = "Simulation ended";
                    timer1.Enabled = false;
                }
            }
        }

        //Events
        private void RunFrame(object sender , EventArgs e)
        {
            //Increase the frame count, and tell the world to Go()
            framesRun++;
            world.Go(random);

            end = DateTime.Now;

            //Next, we figure out the time elapsed since the last frame was run.
            TimeSpan frameDuration = end - start;
            start = end;

            //Finally, update the stats again, with the new time duration.
            UpdateStats(frameDuration);
        }

        private void Form1_Load(object sender , EventArgs e)
        {
            // code to start simulator
        }

        private void StartStripButton_Click(object sender , EventArgs e)
        {
            if (timer1.Enabled)
            {
                toolStrip1.Items[0].Text = "Resume simulation";
                timer1.Stop();
            }
            else
            {
                toolStrip1.Items[0].Text = "Pause simulation";
                timer1.Start();
            }
        }

        private void ResetStripButton1_Click(object sender , EventArgs e)
        {
            framesRun = 0;
            world = new World(new BeeMessage(SendMessage));
            if (!timer1.Enabled)
                toolStrip1.Items[0].Text = "Start simulation";

        }

        private void toolStrip1_ItemClicked(object sender , ToolStripItemClickedEventArgs e)
        {

        }
    }
}
