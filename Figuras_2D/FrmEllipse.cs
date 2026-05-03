using System;
using System.Drawing;
using System.Windows.Forms;

namespace Figuras_2D
{
    public partial class FrmEllipse : Form
    {
        //  Variables globales
        int ancho = 0;
        int alto = 0;
        bool dibujar = false;

        public FrmEllipse()
        {
            InitializeComponent();

            // 🔹 Asociar eventos (por si no están conectados)
            btnGraficar.Click += btnGraficar_Click;
            btnResetear.Click += btnResetear_Click;
            btnSalir.Click += btnSalir_Click;
            panel1.Paint += panel1_Paint;
        }

        // 🔵 BOTÓN GRAFICAR
        private void btnGraficar_Click(object sender, EventArgs e)
        {
            try
            {
                ancho = int.Parse(textBox1.Text);
                alto = int.Parse(textBox2.Text);

                if (ancho <= 0 || alto <= 0)
                {
                    MessageBox.Show("Ingrese valores mayores a 0");
                    return;
                }

                double a = ancho / 2.0;
                double b = alto / 2.0;

                //  Área
                double area = Math.PI * a * b;

                // Perímetro (aproximación)
                double perimetro = Math.PI * (3 * (a + b) - Math.Sqrt((3 * a + b) * (a + 3 * b)));

                txtArea.Text = area.ToString("0.00");
                txtPerimetro.Text = perimetro.ToString("0.00");

                dibujar = true;
                panel1.Invalidate(); // Redibuja
            }
            catch
            {
                MessageBox.Show("Ingrese valores numéricos válidos");
            }
        }

        //  DIBUJAR ELIPSE
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (!dibujar) return;

            Graphics g = e.Graphics;

            //  Mejora visual (suavizado)
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            //  Centrar figura
            int x = panel1.Width / 2 - ancho / 2;
            int y = panel1.Height / 2 - alto / 2;

            //  Colores
            Pen lapiz = new Pen(Color.Blue, 3);
            Brush relleno = new SolidBrush(Color.LightSkyBlue);

            //  Dibujar
            g.FillEllipse(relleno, x, y, ancho, alto);
            g.DrawEllipse(lapiz, x, y, ancho, alto);
        }

        //  BOTÓN RESETEAR
        private void btnResetear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            txtArea.Clear();
            txtPerimetro.Clear();

            dibujar = false;
            panel1.Invalidate();
        }

        //  BOTÓN SALIR
        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void FrmEllipse_Load(object sender, EventArgs e)
        {

        }
    }
}