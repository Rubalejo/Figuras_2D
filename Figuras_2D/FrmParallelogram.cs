using System;
using System.Drawing;
using System.Windows.Forms;

namespace Figuras_2D
{
    public partial class FrmParallelogram : Form
    {
        public FrmParallelogram()
        {
            InitializeComponent();
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            // Verificamos que los datos sean correctos
            if (double.TryParse(txtAncho.Text, out double baseP) &&
                double.TryParse(txtLargo.Text, out double ladoP))
            {
                // CÁLCULOS (SIN CLASE PARA EVITAR ERROR)
                double area = baseP * ladoP;
                double perimetro = 2 * (baseP + ladoP);

                txtArea.Text = area.ToString("N2");
                txtPerimetro.Text = perimetro.ToString("N2");

                // DIBUJO
                using (Graphics canvas = panelDibujo.CreateGraphics())
                {
                    canvas.Clear(Color.White);

                    double minDisplay = 1.0;
                    bool usedMinForDisplay = false;
                    double displayBase = baseP;
                    double displaySide = ladoP;

                    if (baseP <= 0 || ladoP <= 0)
                    {
                        displayBase = Math.Max(baseP, minDisplay);
                        displaySide = Math.Max(ladoP, minDisplay);
                        usedMinForDisplay = true;
                    }

                    float padding = 10f;
                    float maxWidth = Math.Max(50, panelDibujo.Width - (int)(2 * padding));
                    float maxHeight = Math.Max(50, panelDibujo.Height - (int)(2 * padding));

                    float approxInclinationRatio = 0.5f;

                    float requiredWidth = (float)(displayBase + displaySide * approxInclinationRatio);
                    float requiredHeight = (float)(displaySide * 0.9f);

                    float factorX = requiredWidth > 0 ? maxWidth / requiredWidth : 1f;
                    float factorY = requiredHeight > 0 ? maxHeight / requiredHeight : 1f;

                    float factor = Math.Max(0.5f, Math.Min(Math.Min(factorX, factorY), 24.0f));

                    float anchoS = (float)displayBase * factor;
                    float largoS = (float)displaySide * factor;
                    float inclinacion = largoS * approxInclinationRatio;

                    if (usedMinForDisplay)
                    {
                        MessageBox.Show("Valores ajustados para visualización.");
                    }

                    //  POSICIÓN
                    float xBase = (panelDibujo.Width - (anchoS + inclinacion)) / 2;
                    float yBase = (panelDibujo.Height + (largoS * 0.8f)) / 2;

                    //  PUNTOS
                    PointF p1 = new PointF(xBase + inclinacion, yBase - (largoS * 0.8f));
                    PointF p2 = new PointF(p1.X + anchoS, p1.Y);
                    PointF p4 = new PointF(xBase, yBase);
                    PointF p3 = new PointF(p4.X + (p2.X - p1.X), p4.Y);

                    PointF[] vertices = { p1, p2, p3, p4 };

                    //  DIBUJO FINAL
                    using (SolidBrush brocha = new SolidBrush(Color.LightBlue))
                    using (Pen pluma = new Pen(Color.DarkBlue, 2))
                    {
                        canvas.FillPolygon(brocha, vertices);
                        canvas.DrawPolygon(pluma, vertices);
                    }
                }
            }
            else
            {
                MessageBox.Show("Ingresa valores válidos.");
            }
        }

        private void btnResetear_Click(object sender, EventArgs e)
        {
            txtAncho.Clear();
            txtLargo.Clear();
            txtArea.Clear();
            txtPerimetro.Clear();
            panelDibujo.Refresh();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}