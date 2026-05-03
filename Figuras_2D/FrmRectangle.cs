using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Figuras_2D
{
    public partial class FrmRectangle : Form
    {
        private double ancho = 0;
        private double largo = 0;
        private bool hasRectangle = false;

        public FrmRectangle()
        {
            InitializeComponent();
            this.panel1.Paint += Panel1_Paint;
            this.button3.Click += Button3_Click;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Calcular
            if (!TryParseInput(textBox1.Text, out double a) || !TryParseInput(textBox2.Text, out double b))
            {
                MessageBox.Show("Por favor ingrese valores numéricos válidos para ancho y largo.", "Entrada inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (a <= 0 || b <= 0)
            {
                MessageBox.Show("Ancho y largo deben ser mayores que cero.", "Entrada inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ancho = a;
            largo = b;
            hasRectangle = true;

            double perimetro = 2 * (ancho + largo);
            double area = ancho * largo;

            textBox3.Text = perimetro.ToString();
            textBox4.Text = area.ToString();

            panel1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Reset
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            ancho = 0;
            largo = 0;
            hasRectangle = false;
            panel1.Invalidate();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(panel1.BackColor);

            g.FillRectangle(Brushes.LightBlue, 100, 100, 80, 80);

            if (!hasRectangle)
                return;

            // compute scale to fit rectangle inside panel with padding
            float padding = 20f;
            float pw = panel1.ClientSize.Width - padding * 2f;
            float ph = panel1.ClientSize.Height - padding * 2f;

            if (pw <= 0 || ph <= 0)
                return;

            float rw = (float)ancho;
            float rh = (float)largo;

            // scale to fit, preserve aspect
            float scale = Math.Min(pw / rw, ph / rh);

            float drawW = rw * scale;
            float drawH = rh * scale;

            // center
            float x = (panel1.ClientSize.Width - drawW) / 2f;
            float y = (panel1.ClientSize.Height - drawH) / 2f;

            using (var pen = new Pen(Color.DarkBlue, 2f))
            using (var brush = new SolidBrush(Color.FromArgb(50, Color.LightBlue)))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var rect = new RectangleF(x, y, drawW, drawH);
                g.FillRectangle(brush, rect);
                g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
            }
        }

        private bool TryParseInput(string text, out double value)
        {
            if (double.TryParse(text, out value))
                return true;
            // try replace comma with dot for flexible decimal input
            var alt = text.Replace(',', '.');
            return double.TryParse(alt, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out value);
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}