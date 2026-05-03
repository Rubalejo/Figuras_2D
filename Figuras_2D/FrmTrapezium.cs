using System;
using System.Drawing;
using System.Windows.Forms;

namespace Figuras_2D
{
    public partial class FrmTrapezium : Form
    {
        private float baseA = 0;
        private float baseB = 0;
        private float altura = 0;

        public FrmTrapezium()
        {
            InitializeComponent();

            panel1.Paint += Panel1_Paint;

            button1.Click += ButtonGraficar_Click;
            button2.Click += ButtonResetear_Click;
            button3.Click += ButtonSalir_Click;

            textBox1.KeyPress += SoloNumeros;
            textBox2.KeyPress += SoloNumeros;
            textBox3.KeyPress += SoloNumeros;
        }

   
        // VALIDACIÓN
       
        private void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        // BOTÓN GRAFICAR

        private void ButtonGraficar_Click(object sender, EventArgs e)
        {
            if (float.TryParse(textBox1.Text, out baseA) &&
                float.TryParse(textBox2.Text, out baseB) &&
                float.TryParse(textBox3.Text, out altura) &&
                baseA > 0 && baseB > 0 && altura > 0)
            {
                // Área del trapecio
                float area = ((baseA + baseB) * altura) / 2;
                textBox4.Text = area.ToString();

                panel1.Invalidate();
            }
            else
            {
                MessageBox.Show("Ingrese valores válidos");
            }
        }


        // DIBUJO DEL TRAPECIO

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            if (baseA <= 0 || baseB <= 0 || altura <= 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float margen = 10;

            float maxWidth = panel1.Width - 2 * margen;
            float maxHeight = panel1.Height - 2 * margen;

            float mayorBase = Math.Max(baseA, baseB);

            // ESCALA PROPORCIONAL
            float escalaX = maxWidth / mayorBase;
            float escalaY = maxHeight / altura;

            float escala = Math.Min(escalaX, escalaY);

            float baseA_s = baseA * escala;
            float baseB_s = baseB * escala;
            float altura_s = altura * escala;

            // CENTRADO
            float centroX = panel1.Width / 2;

            float yBaseInferior = panel1.Height - margen;
            float yBaseSuperior = yBaseInferior - altura_s;

            // PUNTOS DEL TRAPECIO (uso de Point requerido)
            PointF p1 = new PointF(centroX - baseB_s / 2, yBaseInferior); // izquierda abajo
            PointF p2 = new PointF(centroX + baseB_s / 2, yBaseInferior); // derecha abajo
            PointF p3 = new PointF(centroX + baseA_s / 2, yBaseSuperior); // derecha arriba
            PointF p4 = new PointF(centroX - baseA_s / 2, yBaseSuperior); // izquierda arriba

            PointF[] puntos = { p1, p2, p3, p4 };

            // COLORES
            Pen lapiz = new Pen(Color.DarkBlue, 3);
            Brush relleno = new SolidBrush(Color.LightGreen);

            // DIBUJO
            g.FillPolygon(relleno, puntos);
            g.DrawPolygon(lapiz, puntos);
        }


        // RESETEAR

        private void ButtonResetear_Click(object sender, EventArgs e)
        {
            baseA = baseB = altura = 0;

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();

            panel1.Invalidate();
        }

        // SALIR

        private void ButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}