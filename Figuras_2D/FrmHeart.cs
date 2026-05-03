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
    public partial class HEART : Form
    {

        private PointF[] heartPoints = null;

        private const int Samples = 300;

        public HEART()
        {
            InitializeComponent();

            // Asociar manejadores de eventos
            this.button1.Click += Button1_Click; // Calcular
            this.button2.Click += Button2_Click; // Resetear
            this.button3.Click += Button3_Click; // Salir
            this.panel1.Paint += Panel1_Paint;

            // Opciones de dibujo
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        // Evento de pintura: dibuja el corazón si hay puntos
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (heartPoints == null || heartPoints.Length == 0)
                return;

            // Sombra ligera
            using (Brush sombra = new SolidBrush(Color.FromArgb(60, Color.Black)))
            {
                PointF[] shadow = heartPoints.Select(p => new PointF(p.X + 4f, p.Y + 4f)).ToArray();
                g.FillPolygon(sombra, shadow);
            }

            // Relleno color rosa claro
            using (Brush relleno = new SolidBrush(Color.FromArgb(0xF4, 0xA8, 0xA8)))
            {
                g.FillPolygon(relleno, heartPoints);
            }

            // Contorno oscuro grueso
            using (Pen contorno = new Pen(Color.FromArgb(0x2D, 0x2A, 0x3A), 6f))
            {
                contorno.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                g.DrawPolygon(contorno, heartPoints);
            }

            // Detalle interior fino
            using (Pen detalle = new Pen(Color.FromArgb(255, 255, 255, 80), 1f))
            {
                detalle.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                g.DrawPolygon(detalle, heartPoints);
            }
        }

        // Botón Calcular: valida entrada, genera la forma, calcula área y perímetro
        private void Button1_Click(object sender, EventArgs e)
        {
            // Validación de entrada: radio (float)
            if (!float.TryParse(textBox1.Text.Trim(), out float radio))
            {
                MessageBox.Show("Ingrese un número válido para el Radio (r).", "Entrada inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }

            if (radio <= 0)
            {
                MessageBox.Show("El radio debe ser mayor que cero.", "Entrada inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Centro del panel donde se ubicará el corazón
            PointF center = new PointF(this.panel1.ClientSize.Width / 2f, this.panel1.ClientSize.Height / 2f);

            // Generar puntos sin trasladar al centro aún
            PointF[] raw = CreateHeartRawPoints(Samples);

            // Calculamos una escala que haga que el tamaño sea proporcional al "radio"

            var rawBox = GetBoundingBox(raw);
            float rawWidth = rawBox.Width > 0 ? rawBox.Width : 30f;

            // pixelsPerRadio es cuántos píxeles queremos dar por cada unidad de "radio".

            float pixelsPerRadio = Math.Min(this.panel1.ClientSize.Width, this.panel1.ClientSize.Height) / 9f;

            // Escala final: convierte unidades del corazón crudo a píxeles según el radio
            float scale = radio * pixelsPerRadio / rawWidth;

            // Aplicar escala y centrar: primero calcular el bounding box del raw escalado
            PointF[] scaled = raw.Select(p => new PointF(p.X * scale, p.Y * scale)).ToArray();
            var bbox = GetBoundingBox(scaled);

            // Comprobar si cabe en el panel, ajustar escala si es necesario
            float margin = 10f;
            float maxWidth = this.panel1.ClientSize.Width - margin * 2;
            float maxHeight = this.panel1.ClientSize.Height - margin * 2;
            float neededWidth = bbox.Width;
            float neededHeight = bbox.Height;

            if (neededWidth > maxWidth || neededHeight > maxHeight)
            {
                // Calcular factor de ajuste para que quepa
                float factorW = maxWidth / neededWidth;
                float factorH = maxHeight / neededHeight;
                float adjust = Math.Min(factorW, factorH);

                var result = MessageBox.Show($"El corazón es demasiado grande para el área de dibujo. Se ajustará automáticamente para caber.\n¿Desea continuar?", "Ajuste de tamaño", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) return;

                scale *= adjust;
                scaled = raw.Select(p => new PointF(p.X * scale, p.Y * scale)).ToArray();
                bbox = GetBoundingBox(scaled);
            }

            // Trasladar para centrar en el panel
            float offsetX = center.X - (bbox.X + bbox.Width / 2f);
            float offsetY = center.Y - (bbox.Y + bbox.Height / 2f);
            heartPoints = scaled.Select(p => new PointF(p.X + offsetX, p.Y + offsetY)).ToArray();

            // Calcular área y perímetro
            double area = PolygonArea(heartPoints);
            double perimeter = PolygonPerimeter(heartPoints);

            // Mostrar resultados con formato
            textBox2.Text = area.ToString("0.00");
            textBox3.Text = perimeter.ToString("0.00");

            // Forzar repintado
            this.panel1.Invalidate();
        }

        // Botón Resetear: limpiar entradas y quitar dibujo
        private void Button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            heartPoints = null;
            this.panel1.Invalidate();
        }

        // Botón Salir: cerrar formulario
        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Manejador vacío declarado en el diseñador. Se mantiene para compatibilidad.
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        // Crea puntos paramétricos del corazón en coordenadas centradas en (0,0)

        private PointF[] CreateHeartRawPoints(int samples)
        {
            PointF[] pts = new PointF[samples];
            for (int i = 0; i < samples; i++)
            {
                double t = 2 * Math.PI * i / samples;
                // Ecuación paramétrica clásica del corazón
                double x = 16 * Math.Pow(Math.Sin(t), 3);
                double y = 13 * Math.Cos(t) - 5 * Math.Cos(2 * t) - 2 * Math.Cos(3 * t) - Math.Cos(4 * t);
                // Invertir Y para coordenadas gráficas (y crece hacia abajo en el control)
                pts[i] = new PointF((float)x, (float)(-y));
            }
            return pts;
        }

        // Obtiene el rectángulo que contiene los puntos
        private RectangleF GetBoundingBox(PointF[] pts)
        {
            if (pts == null || pts.Length == 0) return RectangleF.Empty;
            float minX = pts.Min(p => p.X);
            float maxX = pts.Max(p => p.X);
            float minY = pts.Min(p => p.Y);
            float maxY = pts.Max(p => p.Y);
            return new RectangleF(minX, minY, maxX - minX, maxY - minY);
        }

        // Calcula el área de un polígono usando la fórmula del zapatero
        private double PolygonArea(PointF[] pts)
        {
            if (pts == null || pts.Length < 3) return 0.0;
            double sum = 0.0;
            int n = pts.Length;
            for (int i = 0; i < n; i++)
            {
                int j = (i + 1) % n;
                sum += pts[i].X * pts[j].Y - pts[j].X * pts[i].Y;
            }
            return Math.Abs(sum) / 2.0;
        }

        // Calcula el perímetro sumando distancias entre vértices consecutivos
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
