using System;
using System.Drawing;
using System.Windows.Forms;

namespace Figuras_2D
{
    public partial class FrmHeptagon : Form
    {
        // VARIABLES
        float radioVal = 0;
        int numLados = 7;
                
        HeptagonHelper op = new HeptagonHelper();

        public FrmHeptagon()
        {
            InitializeComponent();
        }

        // BOTÓN CALCULAR
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (float.TryParse(textBox1.Text, out float radioIngresado))
            {
                if (radioIngresado > 0 && radioIngresado <= 15)
                {
                    // Escala
                    radioVal = radioIngresado * 10;

                    // Resultados
                    textBox2.Text = op.CalcularArea(numLados, radioIngresado).ToString("F2");
                    textBox3.Text = op.CalcularPerimetro(numLados, radioIngresado).ToString("F2");

                    // SOLO aquí se dibuja
                    panel1.Invalidate();
                }
                else
                {
                    MessageBox.Show("Ingrese un radio entre 1 y 15");
                }
            }
            else
            {
                MessageBox.Show("Ingrese un valor numérico válido");
            }
        }

        // BOTÓN RESETEAR
        private void button2_Click_1(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

            radioVal = 0;

            panel1.Invalidate();
            textBox1.Focus();
        }

        // DIBUJO
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //  NO dibuja si no has ingresado datos
            if (radioVal <= 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            PointF[] vertices = op.CalcularPuntos(numLados, radioVal, panel1.Width, panel1.Height);

            using (Pen lapiz = new Pen(Color.Blue, 3))
            using (SolidBrush relleno = new SolidBrush(Color.Red))
            {
                g.FillPolygon(relleno, vertices);
                g.DrawPolygon(lapiz, vertices);
            }
        }
    }

    public class HeptagonHelper
    {
        public PointF[] CalcularPuntos(int lados, float radio, int ancho, int alto)
        {
            PointF[] puntos = new PointF[lados];

            float centroX = ancho / 2;
            float centroY = alto / 2;

            for (int i = 0; i < lados; i++)
            {
                double angulo = (2 * Math.PI * i / lados) - (Math.PI / 2);

                float x = centroX + (float)(radio * Math.Cos(angulo));
                float y = centroY + (float)(radio * Math.Sin(angulo));

                puntos[i] = new PointF(x, y);
            }

            return puntos;
        }

        public double CalcularArea(int n, float r)
        {
            return (n * Math.Pow(r, 2) * Math.Sin(2 * Math.PI / n)) / 2;
        }

        public double CalcularPerimetro(int n, float r)
        {
            return 2 * n * r * Math.Sin(Math.PI / n);
        }
    }
}