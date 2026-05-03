using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.MonthCalendar;

namespace Figuras_2D
{
    public partial class Kite : Form
    {
        private PointF[] lastVertices = null;

        public Kite()
        {
            InitializeComponent();
        }

        // Inicializa/limpia los campos de entrada y salida
        private void Inicializar()
        {
            txtD.Clear();
            txtdMin.Clear();
            txtArea.Clear();
            txtPerimetro.Clear();
            lastVertices = null;
            panelDibujo.Invalidate();
        }

        // Evento: calcular area, perimetro y dibujar la cometa geometrica
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            // Validacion de entradas (diagonales)
            if (!double.TryParse(txtD.Text, out double D) || !double.TryParse(txtdMin.Text, out double d))
            {
                MessageBox.Show("Por favor ingresa valores numéricos válidos para las diagonales.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Calculos: area y perimetro de la cometa (kite)
            // area = (D * d) / 2
            double area = (D * d) / 2.0;

            // Para el perimetro necesitamos los cuatro lados.
            // En una cometa con diagonales D (mayor, vertical) y d (menor, horizontal),
            // los semilados son sqrt((D/2)^2 + (d/2)^2) para los dos lados iguales de cada pareja.
            double a = Math.Sqrt(Math.Pow(D / 2.0, 2) + Math.Pow(d / 2.0, 2));
            // En un kite  hay dos pares de lados iguales; si eetrico ambos pares pueden ser iguales
            // Aquí asumimos la forma clasica donde los cuatro lados tienen longitud 'a' para simplificar el dibujo
            double perimetro = 4.0 * a;

            // Mostrar resultados formateados
            txtArea.Text = area.ToString("N2");
            txtPerimetro.Text = perimetro.ToString("N2");

            // DIBUJO
            // Si el usuario ingresa 0 o valores no positivos, usamos valores minimos solo para visualizacion
            double minDisplay = 1.0;
            bool usedMinForDisplay = false;
            double displayD = D;
            double displayd = d;
            if (D <= 0 || d <= 0)
            {
                displayD = Math.Max(D, minDisplay);
                displayd = Math.Max(d, minDisplay);
                usedMinForDisplay = true;
            }

            // Factor adaptativo para ajustar la cometa al panel
            float padding = 10f;
            float maxW = Math.Max(50, panelDibujo.Width - (int)(2 * padding));
            float maxH = Math.Max(50, panelDibujo.Height - (int)(2 * padding));
            float reqW = (float)displayd;
            float reqH = (float)displayD;
            float fx = reqW > 0 ? maxW / reqW : 1f;
            float fy = reqH > 0 ? maxH / reqH : 1f;
            float scale = Math.Max(0.5f, Math.Min(Math.Min(fx, fy), 20.0f));

            float Dp = (float)displayD * scale; // diagonal vertical en pixeles
            float dp = (float)displayd * scale; // diagonal horizontal en pixeles

            // Centro del panel
            float cx = panelDibujo.Width / 2f;
            float cy = panelDibujo.Height / 2f;

            // Vértices de la cometa (orientación vertical para D mayor)
            PointF top = new PointF(cx, cy - Dp / 2f);
            PointF right = new PointF(cx + dp / 2f, cy - Dp * 0.15f);
            PointF bottom = new PointF(cx, cy + Dp / 2f);
            PointF left = new PointF(cx - dp / 2f, cy - Dp * 0.15f);
            lastVertices = new PointF[] { top, right, bottom, left };

            // Solicitar repintado del panel (se dibujara en Paint)
            panelDibujo.Invalidate();

            if (usedMinForDisplay)
            {
                MessageBox.Show("Se requieren valores mayores que 0 para dibujar a escala real. Se usaron valores mínimos visuales para mostrar la figura.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Inicializar();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Paint handler para dibujar la cometa con relleno violeta y borde oscuro
        private void panelDibujo_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            g.Clear(Color.White);

            if (lastVertices == null || lastVertices.Length != 4)
                return;

            // sombra suave
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(90, 40, 30, 80)))
            {
                PointF[] sh = new PointF[lastVertices.Length];
                for (int i = 0; i < lastVertices.Length; i++)
                    sh[i] = new PointF(lastVertices[i].X + 6f, lastVertices[i].Y + 8f);
                g.FillPolygon(shadowBrush, sh);
            }

            // relleno violeta claro
            Color fillColor = Color.FromArgb(200, 183, 121, 255); // violeta claro
            using (SolidBrush brush = new SolidBrush(fillColor))
            {
                g.FillPolygon(brush, lastVertices);
            }

            // contorno oscuro grueso
            using (Pen pen = new Pen(Color.FromArgb(80, 40, 20), 2f))
            {
                pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                g.DrawPolygon(pen, lastVertices);
            }

            // contorno fino mas oscuro para dar borde definido
            using (Pen pen2 = new Pen(Color.FromArgb(45, 52, 71), 2f))
            {
                pen2.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                g.DrawPolygon(pen2, lastVertices);
            }

            // diagonales punteadas suaves 
            using (Pen diagPen = new Pen(Color.FromArgb(120, Color.White), 1f))
            {
                diagPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                g.DrawLine(diagPen, lastVertices[0], lastVertices[2]);
                g.DrawLine(diagPen, lastVertices[1], lastVertices[3]);
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
