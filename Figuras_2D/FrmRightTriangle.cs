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
    public partial class Right_Triangle : Form
    {
        // Vertices del triangulo rectangulo para repintado
        private PointF[] rtVertices = null;

        public Right_Triangle()
        {
            InitializeComponent();
            // Asociar eventos de los botones y del panel
            this.button1.Click += btnCalcular_Click; // Calcular
            this.button2.Click += btnReset_Click;    // Resetear
            this.button3.Click += btnCerrar_Click;   // Cerrar
            this.panel1.Paint += panel1_Paint;       // Dibujar al repintar
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }


        /// Boton Calcular: valida que los lados formen un triangulo rectangulo,
        /// calcula área y perimetro, prepara vertices escalados y solicita repintado.
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            // Parseo de entradas
            if (!double.TryParse(textBox1.Text, out double a) ||
                !double.TryParse(textBox2.Text, out double b) ||
                !double.TryParse(textBox3.Text, out double c))
            {
                MessageBox.Show("Ingresa valores numéricos válidos para los tres lados.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar positivos
            if (a <= 0 || b <= 0 || c <= 0)
            {
                MessageBox.Show("Los lados deben ser mayores que 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Determinar el hipotetico cateto mayor (hipotenusa) como el lado mas largo
            double[] sides = new[] { a, b, c };
            Array.Sort(sides); // sides[2] es el mayor
            double leg1 = sides[0];
            double leg2 = sides[1];
            double hyp = sides[2];

            // Verificar la relación de Pitagoras con tolerancia
            double lhs = Math.Round(leg1 * leg1 + leg2 * leg2, 6);
            double rhs = Math.Round(hyp * hyp, 6);
            if (Math.Abs(lhs - rhs) > 1e-3)
            {
                MessageBox.Show("Los lados ingresados no forman un triángulo rectángulo (no cumplen Pitágoras).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Cálculo de area y perimetro
            double area = (leg1 * leg2) / 2.0;
            double perimetro = a + b + c;

            // Mostrar salidas
            textBox4.Text = area.ToString("N2");
            textBox5.Text = perimetro.ToString("N2");

            // Preparar dibujo: mapear catetos al panel
            const float padding = 10f;
            float availW = Math.Max(20, panel1.Width - (int)(2 * padding));
            float availH = Math.Max(20, panel1.Height - (int)(2 * padding));

            float reqW = (float)leg2; // usar leg2 como ancho aproximado
            float reqH = (float)leg1; // usar leg1 como alto aproximado
            float fx = reqW > 0 ? availW / reqW : 1f;
            float fy = reqH > 0 ? availH / reqH : 1f;
            float scale = Math.Max(0.3f, Math.Min(Math.Min(fx, fy), 25f));

            float wS = reqW * scale;
            float hS = reqH * scale;

            // Posicionar triangulo con ángulo recto en la esquina inferior izquierda centrado
            float cx = panel1.Width / 2f;
            float cy = panel1.Height / 2f;

            // Coordenadas: punto inferior-izquierda, inferior-derecha, superior-izquierda
            PointF p1 = new PointF(cx - wS / 2f, cy + hS / 2f); // esquina inferior izquierda
            PointF p2 = new PointF(cx + wS / 2f, cy + hS / 2f); // esquina inferior derecha
            PointF p3 = new PointF(cx - wS / 2f, cy - hS / 2f); // esquina superior izquierda (debe formar el ángulo recto en p1)

            // Ajuste para que el angulo recto quede en p1: hacemos p2 a la derecha y p3 arriba
            // Ensamblar vértices en sentido horario
            rtVertices = new[] { p1, p2, p3 };

            // Solicitar repintado
            panel1.Invalidate();
        }

        /// Boton Reset: limpia entradas, salidas y borra dibujo
        private void btnReset_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            rtVertices = null;
            panel1.Invalidate();
        }
        /// Botón Cerrar: cierra el formulario.

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// Paint del panel: dibuja triangulo rectangulo con relleno verde
        /// sombra y bordes oscuros (uso de Pen, Brush y PointF).

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            if (rtVertices == null || rtVertices.Length != 3)
                return;

            // Dibujar sombra
            using (SolidBrush shadow = new SolidBrush(Color.FromArgb(80, 40, 40, 60)))
            {
                PointF[] sh = new PointF[3];
                for (int i = 0; i < 3; i++) sh[i] = new PointF(rtVertices[i].X + 6f, rtVertices[i].Y + 6f);
                g.FillPolygon(shadow, sh);
            }

            // Relleno verde
            Color fill = Color.FromArgb(220, 182, 190, 66);
            using (SolidBrush brush = new SolidBrush(fill))
            {
                g.FillPolygon(brush, rtVertices);
            }

            // Borde grueso oscuro
            using (Pen thick = new Pen(Color.FromArgb(80, 40, 30), 2f))
            {
                thick.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                g.DrawPolygon(thick, rtVertices);
            }

            // Borde fino definido
            using (Pen thin = new Pen(Color.FromArgb(45, 52, 71), 2f))
            {
                thin.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                g.DrawPolygon(thin, rtVertices);
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
