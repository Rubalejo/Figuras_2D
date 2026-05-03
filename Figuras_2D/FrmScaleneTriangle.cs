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
    public partial class Scalene_Triangle : Form
    {
        // Vertices calculados del triagulo escaleno para dibujar
        private PointF[] triangleVertices = null;

        public Scalene_Triangle()
        {
            InitializeComponent();
            // Asociar eventos de interfaz
            this.button1.Click += btnCalcular_Click; // Calcular
            this.button2.Click += btnReset_Click;    // Resetear
            this.button3.Click += btnCerrar_Click;   // Cerrar
            this.panel1.Paint += panel1_Paint;       // Evento Paint para dibujar
        }
        /// Boton Calcular: valida lados, calcula area y perimetro, calcula vertices
        /// usando ley de cosenos y solicita repintado del panel.
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            // Parseo seguro de entradas
            if (!double.TryParse(textBox1.Text, out double a) ||
                !double.TryParse(textBox2.Text, out double b) ||
                !double.TryParse(textBox3.Text, out double c))
            {
                MessageBox.Show("Ingresa números válidos para los tres lados.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar positivos
            if (a <= 0 || b <= 0 || c <= 0)
            {
                MessageBox.Show("Los lados deben ser mayores que cero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar desigualdad triangular
            if (!(a + b > c && a + c > b && b + c > a))
            {
                MessageBox.Show("Los valores no forman un triángulo (violación de la desigualdad triangular).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar que sea escaleno: los tres lados deben ser diferentes
            const double epsilon = 1e-6;
            if (Math.Abs(a - b) < epsilon || Math.Abs(a - c) < epsilon || Math.Abs(b - c) < epsilon)
            {
                MessageBox.Show("Para un triángulo escaleno los tres lados deben ser diferentes entre sí.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Calculo del perimetro y area
            double perimetro = a + b + c;
            double s = perimetro / 2.0;
            double area = Math.Sqrt(Math.Max(0.0, s * (s - a) * (s - b) * (s - c)));

            // Mostrar resultados
            textBox4.Text = area.ToString("N2");
            textBox5.Text = perimetro.ToString("N2");

            // Calcular posicion exacta del tercer vertice usando ley de cosenos
            // Colocamos el lado 'a' como base entre p1=(0,0) y p2=(a,0).
            // Entonces las distancias a p3 deben ser: |p3-p1| = b y |p3-p2| = c.
            double x = (b * b + a * a - c * c) / (2.0 * a);
            double y2 = b * b - x * x;
            if (y2 < 0) y2 = 0; // tolerancia numerica
            double y = Math.Sqrt(y2);

            // Puntos en espacio de modelo
            PointF p1m = new PointF(0f, 0f);
            PointF p2m = new PointF((float)a, 0f);
            PointF p3m = new PointF((float)x, (float)y);

            // Calcular bounding box del triangulo modelo
            float minX = Math.Min(Math.Min(p1m.X, p2m.X), p3m.X);
            float maxX = Math.Max(Math.Max(p1m.X, p2m.X), p3m.X);
            float minY = Math.Min(Math.Min(p1m.Y, p2m.Y), p3m.Y);
            float maxY = Math.Max(Math.Max(p1m.Y, p2m.Y), p3m.Y);

            float modelW = maxX - minX;
            float modelH = maxY - minY;

            // area disponible en el panel con padding
            const float padding = 10f;
            float availW = Math.Max(20, panel1.Width - (int)(2 * padding));
            float availH = Math.Max(20, panel1.Height - (int)(2 * padding));

            float fx = modelW > 0 ? availW / modelW : 1f;
            float fy = modelH > 0 ? availH / modelH : 1f;
            float scale = Math.Max(0.3f, Math.Min(Math.Min(fx, fy), 15f));

            // Transformar puntos del modelo a coordenadas de panel y centrar
            float cx = panel1.Width / 2f;
            float cy = panel1.Height / 2f;

            // Centro del modelo
            float modelCx = (minX + maxX) / 2f;
            float modelCy = (minY + maxY) / 2f;

            PointF tp1 = new PointF((p1m.X - modelCx) * scale + cx, (p1m.Y - modelCy) * scale + cy);
            PointF tp2 = new PointF((p2m.X - modelCx) * scale + cx, (p2m.Y - modelCy) * scale + cy);
            PointF tp3 = new PointF((p3m.X - modelCx) * scale + cx, (p3m.Y - modelCy) * scale + cy);

            triangleVertices = new[] { tp1, tp2, tp3 };

            // Solicitar repintado del panel para mostrar el triangulo
            panel1.Invalidate();
        }
        /// Boton Reset: limpia campos y borra el dibujo.
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

        /// Boton Cerrar: cierra el formulario.
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// Paint del panel: dibuja el triangulo escaleno con relleno rosado,
        /// sombra y bordes, usando Pen, Brush y PointF.
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            if (triangleVertices == null || triangleVertices.Length != 3)
                return;

            // Sombra
            using (SolidBrush shadow = new SolidBrush(Color.FromArgb(80, 50, 40, 70)))
            {
                PointF[] sh = new PointF[3];
                for (int i = 0; i < 3; i++) sh[i] = new PointF(triangleVertices[i].X + 6f, triangleVertices[i].Y + 6f);
                g.FillPolygon(shadow, sh);
            }

            // Relleno color rosado
            Color fill = Color.FromArgb(230, 255, 150, 170);
            using (SolidBrush brush = new SolidBrush(fill))
            {
                g.FillPolygon(brush, triangleVertices);
            }
            // Borde grueso
            using (Pen thick = new Pen(Color.FromArgb(80, 40, 30), 3f))
            {
                thick.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                g.DrawPolygon(thick, triangleVertices);
            }

            // Borde fino
            using (Pen thin = new Pen(Color.FromArgb(45, 52, 71), 2f))
            {
                thin.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                g.DrawPolygon(thin, triangleVertices);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
