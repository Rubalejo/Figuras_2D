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

namespace Figuras2D
{
    public partial class STAR : Form
    {
        private PointF[] starPoints = null;
        
        private const int NumTips = 5;

        public STAR()
        {
            InitializeComponent();

            this.button1.Click += Button1_Click; // Calcular
            this.button2.Click += Button2_Click; // Resetear
            this.button3.Click += Button3_Click; // Cerrar
            this.panel1.Paint += Panel1_Paint;

            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        // Evento: pintar la estrella en el panel
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (starPoints == null || starPoints.Length == 0)
            {
                return;
            }

            // Dibujar sombra ligera (offset)
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(60, Color.Black)))
            {
                PointF[] shadowPts = starPoints.Select(p => new PointF(p.X + 2f, p.Y + 2f)).ToArray();
                g.FillPolygon(shadowBrush, shadowPts);
            }

            // Relleno principal
            using (Brush fill = new SolidBrush(Color.FromArgb(0x62, 0xD4, 0xD1)))
            {
                g.FillPolygon(fill, starPoints);
            }

            // Contorno grueso
            using (Pen outline = new Pen(Color.DarkSlateBlue, 2f))
            {
                outline.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                g.DrawPolygon(outline, starPoints);
            }
        }

        // Botón Calcular: genera la estrella, calcula área y perímetro
        private void Button1_Click(object sender, EventArgs e)
        {
            // Validar entradas
            if (!float.TryParse(textBox1.Text.Trim(), out float outerRadius))
            {
                MessageBox.Show("Ingrese un número válido para Radio Mayor (R).", "Entrada inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!float.TryParse(textBox2.Text.Trim(), out float innerRadius))
            {
                MessageBox.Show("Ingrese un número válido para Radio Menor (r).", "Entrada inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (outerRadius <= 0 || innerRadius <= 0 || innerRadius >= outerRadius)
            {
                MessageBox.Show("Radios inválidos. Asegúrese que R > r y ambos sean > 0.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Calcular el centro del panel como punto de referencia
            PointF center = new PointF(this.panel1.ClientSize.Width / 2f, this.panel1.ClientSize.Height / 2f);


            // Generar los puntos usando exactamente los valores que el usuario ingresó
            //starPoints = CreateStarPoints(center, outerRadius, innerRadius, NumTips);
            float zoom = 2f;
            starPoints = CreateStarPoints(center, outerRadius * zoom, innerRadius * zoom, NumTips);
            // Calcular área y perímetro con los mismos puntos
            double area = PolygonArea(starPoints);
            double perimeter = PolygonPerimeter(starPoints);

            // Mostrar resultados
            textBox3.Text = area.ToString("0.00");
            textBox4.Text = perimeter.ToString("0.00");

            // Forzar repintado del panel
            this.panel1.Invalidate();
        }

        // Botón Resetear: limpia valores y quita la estrella del panel
        private void Button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            starPoints = null;
            this.panel1.Invalidate();
        }

        // Botón Cerrar: cierra el formulario
        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Genera los puntos de una estrella de N puntas alternando radios
        private PointF[] CreateStarPoints(PointF center, float outerRadius, float innerRadius, int numTips)
        {
            int totalPoints = numTips * 2;
            PointF[] pts = new PointF[totalPoints];
            double startAngle = -Math.PI / 2.0;
            double step = Math.PI / numTips;

            for (int i = 0; i < totalPoints; i++)
            {
                double angle = startAngle + i * step;
                float radius = (i % 2 == 0) ? outerRadius : innerRadius;
                float x = center.X + (float)(Math.Cos(angle) * radius);
                float y = center.Y + (float)(Math.Sin(angle) * radius);
                pts[i] = new PointF(x, y);
            }
            return pts;
        }

        // Calcula el área de un polígono usando la fórmula de la lazada
        private double PolygonArea(PointF[] pts)
        {
            if (pts == null || pts.Length < 3) return 0.0;
            double sum = 0.0;
            int n = pts.Length;
            for (int i = 0; i < n; i++)
            {
                int j = (i + 1) % n;
                sum += (double)pts[i].X * pts[j].Y - (double)pts[j].X * pts[i].Y;
            }
            return Math.Abs(sum) / 2.0;
        }

        // Calcula el perímetro sumando distancias
        private double PolygonPerimeter(PointF[] pts)
        {
            if (pts == null || pts.Length < 2) return 0.0;
            double sum = 0.0;
            int n = pts.Length;
            for (int i = 0; i < n; i++)
            {
                int j = (i + 1) % n;
                double dx = pts[i].X - pts[j].X;
                double dy = pts[i].Y - pts[j].Y;
                sum += Math.Sqrt(dx * dx + dy * dy);
            }
            return sum;
        }
    }
}