using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Figuras_2D   
{
    public partial class FrmPie : Form
    {
        private float radioPie = 0;

        public FrmPie()
        {
            InitializeComponent();
        }

        // BOTÓN CALCULAR
        private void button1_Click(object sender, EventArgs e)
        {
            if (float.TryParse(textBox1.Text, out float radio))
            {
                if (radio >= 0 && radio <= 9)
                {
                    radioPie = radio;

                    float area = 0.75f * (float)Math.PI * (radio * radio);
                    float perimetro = (2 * radio) + (1.5f * (float)Math.PI * radio);

                    textBox2.Text = area.ToString("0.##");
                    textBox3.Text = perimetro.ToString("0.##");

                    panel1.Invalidate();
                }
                else
                {
                    MessageBox.Show("Ingrese un valor entre 0 y 9");
                    textBox1.Clear();
                    textBox1.Focus();
                }
            }
            else
            {
                MessageBox.Show("Ingrese un número válido");
                textBox1.Clear();
                textBox1.Focus();
            }
        }

        // BOTÓN RESETEAR
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

            radioPie = 0;
            panel1.Invalidate();

            textBox1.Focus();
        }

        // DIBUJO
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (radioPie <= 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float escala = 20f;
            float w = radioPie * escala;

            float cx = panel1.Width / 2f;
            float cy = panel1.Height / 2f;

            float x = cx - w;
            float y = cy - w;
            float ancho = 2 * w;
            float alto = 2 * w;

            using (SolidBrush brocha = new SolidBrush(Color.BurlyWood))
            {
                g.FillPie(brocha, x, y, ancho, alto, 0, 270);
            }

            using (Pen lapiz = new Pen(Color.Black, 2))
            {
                g.DrawPie(lapiz, x, y, ancho, alto, 0, 270);
            }
        }
    }
}