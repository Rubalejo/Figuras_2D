using System;
using System.Drawing;
using System.Windows.Forms;

namespace Figuras_2D
{
    public partial class FrmCuadrado : Form
    {
        private float lado = 0;

        public FrmCuadrado()
        {
            InitializeComponent();

            panel1.Paint += Panel1_Paint;
            button1.Click += ButtonGraficar_Click;
            button2.Click += ButtonResetear_Click;
            button3.Click += ButtonSalir_Click;
            textBox1.KeyPress += TextBox1_KeyPress;
        }

        // VALIDACIÓN: SOLO NÚMEROS
        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        // BOTÓN GRAFICAR
        private void ButtonGraficar_Click(object sender, EventArgs e)
        {
            if (float.TryParse(textBox1.Text, out lado) && lado > 0)
            {
                float area = lado * lado;
                float perimetro = 4 * lado;

                textBox2.Text = area.ToString();
                textBox3.Text = perimetro.ToString();

                panel1.Invalidate(); // Redibuja
            }
            else
            {
                MessageBox.Show("Ingrese un valor válido");
            }
        }

        // DIBUJO DEL CUADRADO (ESCALA REAL)
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            if (lado <= 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            float margen = 10;
            float maxPanel = Math.Min(panel1.Width, panel1.Height) - 2 * margen;

            // ESCALA CONTROLADA (CLAVE)
            float factorEscala = 5f;

            float ladoEscalado = lado * factorEscala;

            // Limitar tamaño máximo
            if (ladoEscalado > maxPanel)
                ladoEscalado = maxPanel;

            // Tamaño mínimo visible
            if (ladoEscalado < 5)
                ladoEscalado = 5;

            // Centrar figura
            float x = (panel1.Width - ladoEscalado) / 2;
            float y = (panel1.Height - ladoEscalado) / 2;

            // Colores
            Pen lapiz = new Pen(Color.Blue, 3);
            Brush relleno = new SolidBrush(Color.LightSkyBlue);

            // Dibujar
            g.FillRectangle(relleno, x, y, ladoEscalado, ladoEscalado);
            g.DrawRectangle(lapiz, x, y, ladoEscalado, ladoEscalado);
        }

        // BOTÓN RESETEAR
        private void ButtonResetear_Click(object sender, EventArgs e)
        {
            lado = 0;
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            panel1.Invalidate();
        }

        // BOTÓN SALIR
        private void ButtonSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}