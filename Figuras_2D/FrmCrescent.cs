using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Figuras2D
{
    public partial class CRESCENT : Form
    {
        // Datos para el dibujo escalado 
        private PointF outerCenterPx;
        private PointF innerCenterPx;
        private float outerRadiusPx;
        private float innerRadiusPx;
        private bool hasCrescent = false;

        // Parámetros en unidades de entrada 
        private float outerRadiusUnits;
        private float innerRadiusUnits;
        private float centerDistanceUnits;

        public CRESCENT()
        {
            InitializeComponent();

            // Asociar manejadores de eventos
            this.button1.Click += Button1_Click; // Calcular
            this.button2.Click += Button2_Click; // Resetear
            this.button3.Click += Button3_Click; // Salir
            this.panel1.Paint += Panel1_Paint;

            // Dibujos más suaves
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        // Pintado del panel: dibuja la media luna usando Region (outer - inner)
        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (!hasCrescent)
                return;

            // Crear paths para las dos circunferencias 
            RectangleF outerRect = new RectangleF(outerCenterPx.X - outerRadiusPx, outerCenterPx.Y - outerRadiusPx, outerRadiusPx * 2f, outerRadiusPx * 2f);
            RectangleF innerRect = new RectangleF(innerCenterPx.X - innerRadiusPx, innerCenterPx.Y - innerRadiusPx, innerRadiusPx * 2f, innerRadiusPx * 2f);

            using (GraphicsPath outerPath = new GraphicsPath())
            using (GraphicsPath innerPath = new GraphicsPath())
            using (Region crescentRegion = new Region())
            {
                outerPath.AddEllipse(outerRect);
                innerPath.AddEllipse(innerRect);

                crescentRegion.MakeEmpty();
                crescentRegion.Union(outerPath);
                crescentRegion.Exclude(innerPath);

                // Relleno verde claro
                using (Brush fill = new SolidBrush(Color.FromArgb(0xC8, 0xE0, 0x7A)))
                {
                    g.FillRegion(fill, crescentRegion);
                }

                // --- CAMBIO: Contorno más fino (de 6f a 3f) ---
                using (Pen outline = new Pen(Color.FromArgb(0x2A, 0x2F, 0x32), 3f))
                {
                    outline.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

                    float d = centerDistanceUnits;
                    if (d >= outerRadiusUnits + innerRadiusUnits)
                    {
                        g.DrawEllipse(outline, outerRect);
                    }
                    else if (d <= Math.Abs(outerRadiusUnits - innerRadiusUnits))
                    {
                        g.DrawEllipse(outline, outerRect);
                        g.DrawEllipse(outline, innerRect);
                    }
                    else
                    {
                        DrawCrescentArcs(g, outline, outerCenterPx, outerRadiusPx, innerCenterPx, innerRadiusPx, centerDistanceUnits);
                    }
                }
            }
        }

        // Manejo del botón Calcular
        private void Button1_Click(object sender, EventArgs e)
        {
            // Validar entradas
            if (!float.TryParse(textBox1.Text.Trim(), out float R))
            {
                MessageBox.Show("Ingrese un número válido para Radio Mayor (R).", "Entrada inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }
            if (!float.TryParse(textBox2.Text.Trim(), out float r))
            {
                MessageBox.Show("Ingrese un número válido para Radio Menor (r).", "Entrada inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }
            if (R <= 0 || r <= 0 || r >= R)
            {
                MessageBox.Show("Radios inválidos (R debe ser mayor que r y ambos mayores a 0).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            outerRadiusUnits = R;
            innerRadiusUnits = r;
            centerDistanceUnits = outerRadiusUnits - innerRadiusUnits / 2f;

            // Cálculos
            double intersectionArea = CircleIntersectionArea(outerRadiusUnits, innerRadiusUnits, centerDistanceUnits);
            double outerArea = Math.PI * outerRadiusUnits * outerRadiusUnits;
            double crescentArea = outerArea - intersectionArea;
            double crescentPerimeter = CalculateCrescentPerimeter(outerRadiusUnits, innerRadiusUnits, centerDistanceUnits);

            textBox3.Text = crescentArea.ToString("0.00");
            textBox4.Text = crescentPerimeter.ToString("0.00");

            // Escalado controlado: definir un valor inicial de píxeles por unidad
            // para que radios pequeños produzcan figuras pequeñas y al aumentar

            float initialPixelsPerUnit = 12f;

            // Conversión inicial
            float pixelsPerUnit = initialPixelsPerUnit;
            outerRadiusPx = outerRadiusUnits * pixelsPerUnit;
            innerRadiusPx = innerRadiusUnits * pixelsPerUnit;
            float dPx = centerDistanceUnits * pixelsPerUnit;

            // Centros (outer en el centro del panel)
            outerCenterPx = new PointF(this.panel1.ClientSize.Width / 2f, this.panel1.ClientSize.Height / 2f);
            innerCenterPx = new PointF(outerCenterPx.X + dPx, outerCenterPx.Y);

            // Ajuste de escala para que la luna no sea demasiado grande ni demasiado pequeña
            float margin = 20f;
            var bbox = new RectangleF(outerCenterPx.X - outerRadiusPx, outerCenterPx.Y - outerRadiusPx, outerRadiusPx * 2f, outerRadiusPx * 2f);
            bbox = RectangleF.Union(bbox, new RectangleF(innerCenterPx.X - innerRadiusPx, innerCenterPx.Y - innerRadiusPx, innerRadiusPx * 2f, innerRadiusPx * 2f));

            float maxW = this.panel1.ClientSize.Width - margin * 2f;
            float maxH = this.panel1.ClientSize.Height - margin * 2f;
            float maxAllowed = Math.Min(maxW, maxH);
            float minOuterPx = 30f;

            // Si es demasiado grande, reducir
            if (bbox.Width > maxW || bbox.Height > maxH)
            {
                float adjust = Math.Min(maxW / bbox.Width, maxH / bbox.Height);
                pixelsPerUnit *= adjust;
                outerRadiusPx = outerRadiusUnits * pixelsPerUnit;
                innerRadiusPx = innerRadiusUnits * pixelsPerUnit;
                dPx = centerDistanceUnits * pixelsPerUnit;
                innerCenterPx = new PointF(outerCenterPx.X + dPx, outerCenterPx.Y);
            }

            // Si resultó demasiado pequeño, aumentar para mantener legibilidad
            if (outerRadiusPx < minOuterPx)
            {
                float adjust = minOuterPx / outerRadiusPx;
                pixelsPerUnit *= adjust;
                outerRadiusPx = outerRadiusUnits * pixelsPerUnit;
                innerRadiusPx = innerRadiusUnits * pixelsPerUnit;
                dPx = centerDistanceUnits * pixelsPerUnit;
                innerCenterPx = new PointF(outerCenterPx.X + dPx, outerCenterPx.Y);
            }

            hasCrescent = true;
            this.panel1.Invalidate();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            hasCrescent = false;
            this.panel1.Invalidate();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private double CircleIntersectionArea(double R, double r, double d)
        {
            if (d >= R + r) return 0.0;
            if (d <= Math.Abs(R - r)) return Math.PI * Math.Min(R, r) * Math.Min(R, r);

            double R2 = R * R;
            double r2 = r * r;
            double alpha = Math.Acos((d * d + R2 - r2) / (2 * d * R)) * 2.0;
            double beta = Math.Acos((d * d + r2 - R2) / (2 * d * r)) * 2.0;

            return 0.5 * R2 * (alpha - Math.Sin(alpha)) + 0.5 * r2 * (beta - Math.Sin(beta));
        }

        private double CalculateCrescentPerimeter(double R, double r, double d)
        {
            if (d >= R + r) return 2.0 * Math.PI * R;
            if (d <= Math.Abs(R - r)) return 2.0 * Math.PI * R + 2.0 * Math.PI * r;

            double phi = 2.0 * Math.Acos((d * d + R * R - r * r) / (2.0 * d * R));
            double theta = 2.0 * Math.Acos((d * d + r * r - R * R) / (2.0 * d * r));

            return (2.0 * Math.PI - phi) * R + (theta * r);
        }

        private void DrawCrescentArcs(Graphics g, Pen pen, PointF cOuter, float Rpx, PointF cInner, float rpx, float dUnits)
        {
            double alpha = Math.Acos((dUnits * dUnits + outerRadiusUnits * outerRadiusUnits - innerRadiusUnits * innerRadiusUnits) / (2.0 * dUnits * outerRadiusUnits));
            double beta = Math.Acos((dUnits * dUnits + innerRadiusUnits * innerRadiusUnits - outerRadiusUnits * outerRadiusUnits) / (2.0 * dUnits * innerRadiusUnits));

            float alphaDeg = (float)(alpha * 180.0 / Math.PI);
            float betaDeg = (float)(beta * 180.0 / Math.PI);

            RectangleF outerRect = new RectangleF(cOuter.X - Rpx, cOuter.Y - Rpx, Rpx * 2f, Rpx * 2f);
            RectangleF innerRect = new RectangleF(cInner.X - rpx, cInner.Y - rpx, rpx * 2f, rpx * 2f);

            g.DrawArc(pen, outerRect, alphaDeg, 360f - 2f * alphaDeg);
            g.DrawArc(pen, innerRect, 180f - betaDeg, 2f * betaDeg);
        }
        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}


