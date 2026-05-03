using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Figuras_2D
{
    public partial class FrmHexagon : Form
    {
        float radioVal = 0;
        int numLados = 6;

        OperacionesHexagono op = new OperacionesHexagono();

        public FrmHexagon()
        {
            InitializeComponent();

            // Activar botón reset
            button2.Click += button2_Click;
        }

        // BOTÓN CALCULAR
        private void button1_Click(object sender, EventArgs e)
        {
            if (float.TryParse(textBox1.Text, out float radioIngresado))
            {
                if (radioIngresado > 0 && radioIngresado <= 10)
                {
                    radioVal = radioIngresado;

                    textBox2.Text = op.CalcularArea(numLados, radioIngresado).ToString("F2");
                    textBox3.Text = op.CalcularPerimetro(numLados, radioIngresado).ToString("F2");

                    Pentagono.Refresh(); // Redibuja
                }
                else
                {
                    MessageBox.Show("Ingrese un valor entre 1 y 10");
                }
            }
            else
            {
                MessageBox.Show("Ingrese un número válido");
            }
        }

        // RESET
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            radioVal = 0;

            Pentagono.Refresh();
        }

        // DIBUJO
        private void Pentagono_Paint(object sender, PaintEventArgs e)
        {
            if (radioVal <= 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int w = Pentagono.Width;
            int h = Pentagono.Height;

            float centroX = w / 2f;
            float centroY = h / 2f;

            float radioBase = Math.Min(w, h) / 3;
            float radioFinal = radioBase * (radioVal / 5f);

            PointF[] vertices = op.CalcularPuntos(numLados, radioFinal, (int)centroX, (int)centroY);

            using (Pen lapiz = new Pen(Color.Blue, 3))
            using (SolidBrush relleno = new SolidBrush(Color.BlueViolet))
            {
                g.FillPolygon(relleno, vertices);
                g.DrawPolygon(lapiz, vertices);
            }
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }

    public class OperacionesHexagono
    {
        public PointF[] CalcularPuntos(int lados, float radio, int centroX, int centroY)
        {
            PointF[] puntos = new PointF[lados];

            for (int i = 0; i < lados; i++)
            {
                double angulo = 2 * Math.PI * i / lados;

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