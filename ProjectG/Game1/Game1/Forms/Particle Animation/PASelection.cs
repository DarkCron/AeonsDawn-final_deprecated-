using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TBAGW.Scenes.Editor;

namespace TBAGW.Forms.Particle_Animation
{
    public partial class PASelection : Form
    {
        public PASelection()
        {
            InitializeComponent();
        }

        private void PASelection_Load(object sender, EventArgs e)
        {

        }

        public delegate void CompleteFunction(ParticleAnimation pa);
        CompleteFunction function;

        public void Start()
        {
            listBox1.DataSource = null;
            listBox1.DataSource = MapBuilder.gcDB.gameParticleAnimations;
            Show();
        }

        public void Start(CompleteFunction function)
        {
            this.function = function;
            listBox1.DataSource = null;
            listBox1.DataSource = MapBuilder.gcDB.gameParticleAnimations;
            Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                listBox1.SelectedIndex = -1;
                listBox1.DataSource = null;
                listBox1.DataSource = (MapBuilder.gcDB.gameParticleAnimations);
            }
            else
            {
                listBox1.SelectedIndex = -1;
                listBox1.DataSource = null;
                listBox1.DataSource = (MapBuilder.gcDB.gameParticleAnimations.FindAll(o => o.particleAnimationName.IndexOf(textBox1.Text, StringComparison.OrdinalIgnoreCase) >= 0));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                function(listBox1.SelectedItem as ParticleAnimation);
                Close();
            }
        }
    }
}
