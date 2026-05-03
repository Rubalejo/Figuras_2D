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
    public partial class FrmPentagon : Form
    {
        // 1. Variables Globales
        float radioVal = 0;
        int numLados = 5; // Pentágono
        OperacionesGraficas op = new OperacionesGraficas();

        public FrmPentagon()
        {
            InitializeComponent();
        }

        // BOTÓN CALCULAR 
        private void button1_Click(object sender, EventArgs e)
        {
            if (float.TryParse(textBox1.Text, out float radioIngresado))
            {
                if (radioIngresado > 0 && radioIngresado <= 15)
                {
                    // Escala x10 para el dibujo
                    radioVal = radioIngresado * 10;

                    // Resultados matemáticos
                    textBox2.Text = op.CalcularArea(numLados, radioIngresado).ToString("F2");
                    textBox3.Text = op.CalcularPerimetro(numLados, radioIngresado).ToString("F2");

                    // Refrescar el panel para dibujar
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
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            radioVal = 0;
            panel1.Invalidate();
        }

        // EVENTO PAINT DEL PANEL
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (radioVal > 0)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Calculamos puntos centrados
                PointF[] vertices = op.CalcularPuntos(numLados, radioVal, panel1.Width, panel1.Height);

                // Estilo solicitado: Borde azul y relleno amarillo
                using (Pen lapizAzul = new Pen(Color.Blue, 3))
                using (SolidBrush rellenoAmarillo = new SolidBrush(Color.Yellow))
                {
                    g.FillPolygon(rellenoAmarillo, vertices);
                    g.DrawPolygon(lapizAzul, vertices);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Intentamos leer el radio del primer cuadro de texto
            if (float.TryParse(textBox1.Text, out float radioIngresado))
            {
                // Validación
                if (radioIngresado > 0 && radioIngresado <= 15)
                {
                    // Aplicamos escala 10
                    radioVal = radioIngresado * 10;


                    // textBox2 es Área, textBox3 es Perímetro
                    textBox2.Text = op.CalcularArea(numLados, radioIngresado).ToString("F2");
                    textBox3.Text = op.CalcularPerimetro(numLados, radioIngresado).ToString("F2");

                    // Refrescamos el panel para que ejecute el código de dibujo (Paint)
                    panel1.Invalidate();
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese un radio entre 1 y 15.", "Aviso");
                    textBox1.Focus();
                }
            }
            else
            {
                MessageBox.Show("Error: Ingrese un número válido en el campo de Radio.", "Error de entrada");
                textBox1.Clear();
                textBox1.Focus();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }

    // CLASE DE OPERACIONES 
    public class OperacionesGraficas
    {
        public PointF[] CalcularPuntos(int lados, float radio, int ancho, int alto)
        {
            PointF[] puntos = new PointF[lados];
            float centroX = ancho / 2;
            float centroY = alto / 2;

            for (int i = 0; i < lados; i++)
            {
                // -Math.PI / 2 para que la punta mire hacia arriba
                double angulo = (2 * Math.PI * i / lados) - (Math.PI / 2);
                float x = centroX + (float)(radio * Math.Cos(angulo));
                float y = centroY + (float)(radio * Math.Sin(angulo));
                puntos[i] = new PointF(x, y);
            }
            return puntos;
        }

        public double CalcularArea(int n, float r) => (n * Math.Pow(r, 2) * Math.Sin(2 * Math.PI / n)) / 2;
        public double CalcularPerimetro(int n, float r) => 2 * n * r * Math.Sin(Math.PI / n);
    }
}