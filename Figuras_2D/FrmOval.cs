using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Figuras_2D
{
    public partial class FrmOval : Form
    {
        int ancho = 0;
        int alto = 0;
        bool dibujar = false;

        public FrmOval()
        {
            InitializeComponent();

            //  Evita errores en el diseñador
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;
        }

        //  BOTÓN GRAFICAR
        private void btnGraficar_Click(object sender, EventArgs e)
        {
            // Validación 
            if (!int.TryParse(textBox1.Text, out ancho) ||
                !int.TryParse(textBox2.Text, out alto))
            {
                MessageBox.Show("Ingrese valores numéricos válidos");
                return;
            }

            if (ancho <= 0 || alto <= 0)
            {
                MessageBox.Show("Ingrese valores mayores a 0");
                return;
            }

            //Cálculos

            double a = ancho / 2.0;
            double b = alto / 2.0;

            // Área
            double area = Math.PI * a * b;

            // Perímetro aproximado (Ramanujan)
            double perimetro = Math.PI * (3 * (a + b) - Math.Sqrt((3 * a + b) * (a + 3 * b)));

            textBox3.Text = area.ToString("0.00");
            textBox4.Text = perimetro.ToString("0.00");

            dibujar = true;
            panel1.Invalidate();
        }

        //  DIBUJAR ÓVALO
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (!dibujar) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int x = panel1.Width / 2 - ancho / 2;
            int y = panel1.Height / 2 - alto / 2;

            //  colores 
            using (Pen lapiz = new Pen(Color.DarkBlue, 2))
            using (Brush relleno = new SolidBrush(Color.LightBlue))
            {
                g.FillEllipse(relleno, x, y, ancho, alto);
                g.DrawEllipse(lapiz, x, y, ancho, alto);
            }
        }

        // RESETEAR CAMPOS Y PANEL
        private void btnResetear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();

            dibujar = false;
            panel1.Invalidate();
        }

        // SALIR
        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}