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
    public partial class Triangle : Form
    {
        // Vertices calculados del triangulo para el repintado
        private PointF[] triangleVertices = null;

        public Triangle()
        {
            InitializeComponent();
            // Asociar eventos de interfaz
            this.button1.Click += btnCalcular_Click; // botón "Calcular"
            this.button2.Click += btnReset_Click;    // botón "Reset"
            this.button3.Click += btnCerrar_Click;   // botón "Cerrar"
            this.panel1.Paint += panel1_Paint;       // evento Paint para dibujar
        }


        // Maneador del boton Calcular: valida lados, calcula area y perimetro,
        // genera los vertices escalados del triángulo y solicita repintado.

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            // Parseo de entradas (lados A, B, C)
            if (!double.TryParse(textBox1.Text, out double a) ||
                !double.TryParse(textBox2.Text, out double b) ||
                !double.TryParse(textBox3.Text, out double c))
            {
                MessageBox.Show("Ingresa valores numéricos válidos para los tres lados.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar que los lados sean positivos
            if (a <= 0 || b <= 0 || c <= 0)
            {
                MessageBox.Show("Los tres lados deben ser mayores que 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar desigualdad triangular
            if (!(a + b > c && a + c > b && b + c > a))
            {
                MessageBox.Show("Los valores ingresados no forman un triángulo (violan la desigualdad triangular).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Calculo del peremetro y area formula de Heron
            double perimetro = a + b + c;
            double s = perimetro / 2.0;
            double area = Math.Sqrt(Math.Max(0.0, s * (s - a) * (s - b) * (s - c)));

            // Mostrar resultados en los campos de salida
            textBox4.Text = area.ToString("N2");      // area
            textBox5.Text = perimetro.ToString("N2"); // Perimetro

            // Preparar dibujo: calcular escala para que el triangulo quepa en el panel
            double baseLength = a; // tomamos 'a' como base para dibujar
            double height = baseLength > 0 ? (2.0 * area) / baseLength : 0.0;

            // Dimensiones del panel reducidas por padding
            const float padding = 8f;
            float availW = Math.Max(20, panel1.Width - (int)(2 * padding));
            float availH = Math.Max(20, panel1.Height - (int)(2 * padding));

            float reqW = (float)baseLength;
            float reqH = (float)height;
            float fx = reqW > 0 ? availW / reqW : 1f;
            float fy = reqH > 0 ? availH / reqH : 1f;
            float scale = Math.Max(0.3f, Math.Min(Math.Min(fx, fy), 30f));

            // Coordenadas escaladas en pixeles
            float aS = (float)baseLength * scale;
            float hS = (float)height * scale;

            // Centrar triangulo en el panel
            float cx = panel1.Width / 2f;
            float cy = panel1.Height / 2f;

            // Vertices: base horizontal (p1 -> p2) y pico superior (p3)
            PointF p1 = new PointF(cx - aS / 2f, cy + hS / 2f);
            PointF p2 = new PointF(cx + aS / 2f, cy + hS / 2f);
            PointF p3 = new PointF(cx, cy - hS / 2f);
            triangleVertices = new[] { p1, p2, p3 };

            // Solicitar repintado del panel para mostrar el triangulo
            panel1.Invalidate();
        }


        //Maneador del boton Reset: limpia entradas, salidas y el panel de dibujo.

        private void btnReset_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            triangleVertices = null;
            panel1.Invalidate();
        }


        // Maneador del boton Cerrar: cierra el formulario.

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //Evento Paint del panel: dibuja el triangulo usando Pen, Brush y PointF.


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Fondo blanco limpio
            g.Clear(Color.White);

            if (triangleVertices == null || triangleVertices.Length != 3)
                return;

            // Dibujar sombra suave detras del triángulo
            using (SolidBrush shadow = new SolidBrush(Color.FromArgb(90, 50, 50, 70)))
            {
                PointF[] sh = new PointF[3];
                for (int i = 0; i < 3; i++)
                    sh[i] = new PointF(triangleVertices[i].X + 6f, triangleVertices[i].Y + 8f);
                g.FillPolygon(shadow, sh);
            }

            // Relleno rosado (similar a la imagen)
            Color fillColor = Color.FromArgb(230, 255, 140, 160); // rosa claro
            using (SolidBrush brush = new SolidBrush(fillColor))
            {
                g.FillPolygon(brush, triangleVertices);
            }

            // Borde grueso oscuro
            using (Pen thickPen = new Pen(Color.FromArgb(70, 30, 30), 2f))
            {
                thickPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                g.DrawPolygon(thickPen, triangleVertices);
            }

            // Borde fino más definido
            using (Pen thinPen = new Pen(Color.FromArgb(45, 52, 71), 2f))
            {
                thinPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                g.DrawPolygon(thinPen, triangleVertices);
            }
        }
    }
}
