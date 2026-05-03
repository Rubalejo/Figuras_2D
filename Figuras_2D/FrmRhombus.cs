using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.MonthCalendar;

namespace Figuras_2D
{
    public partial class FrmRhombus : Form
    {
        // Singleton para evitar multiples instancias
        private static FrmRhombus instancia = null;

        public FrmRhombus()
        {
            InitializeComponent();
        }

        public static FrmRhombus GetInstance()
        {
            if (instancia == null || instancia.IsDisposed)
            {
                instancia = new FrmRhombus();
            }
            return instancia;
        }

        // Inicializa los campos de entrada/salida
        public void InicializarData()
        {
            txtArea.Text = "";
            txtPerimetro.Text = "";
            txtD.Text = "";
            txtdmin.Text = "";
            panelDibujo.Refresh();
        }

        // Evento calcular: valida, calcula aea/perimetro y dibuja el rombo
        private void btnCalcular_Click(object sender, EventArgs e)
        {
            // Validacion segura de entradas
            if (!double.TryParse(txtD.Text, out double dMayor) ||
                !double.TryParse(txtdmin.Text, out double dMenor))
            {
                MessageBox.Show("Por favor ingresa valores numéricos válidos para las diagonales.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Calculos matematicos reales usando las diagonales ingresadas
            double area = (dMayor * dMenor) / 2.0;
            double semiD2 = Math.Pow(dMayor / 2.0, 2);
            double semid2 = Math.Pow(dMenor / 2.0, 2);
            double perimetro = 4.0 * Math.Sqrt(semiD2 + semid2);

            // Mostrar resultados
            txtArea.Text = area.ToString("N2");
            txtPerimetro.Text = perimetro.ToString("N2");

            // DIBUJO 
            // Usamos valores minimos para visualización si el usuario ingresa 0 o negativos,
            // pero los calculos numericos anteriores se realizaron con los valores reales.
            double minDisplay = 1.0;
            bool usedMinForDisplay = false;
            double displayD = dMayor;
            double displayd = dMenor;
            if (dMayor <= 0 || dMenor <= 0)
            {
                displayD = Math.Max(dMayor, minDisplay);
                displayd = Math.Max(dMenor, minDisplay);
                usedMinForDisplay = true;
            }

            // Factor adaptativo para que el rombo quepa en el panel
            float padding = 10f;
            float maxWidth = Math.Max(50, panelDibujo.Width - (int)(2 * padding));
            float maxHeight = Math.Max(50, panelDibujo.Height - (int)(2 * padding));

            // Requerido en pixeles aproximadamente
            float requiredWidth = (float)displayd; // ancho aproximado sera la diagonal menor
            float requiredHeight = (float)displayD; // alto aproximado sera la diagonal mayor

            float factorX = requiredWidth > 0 ? maxWidth / requiredWidth : 1f;
            float factorY = requiredHeight > 0 ? maxHeight / requiredHeight : 1f;
            float factor = Math.Max(0.5f, Math.Min(Math.Min(factorX, factorY), 20.0f));

            float dYS = (float)displayD * factor; // diagonal mayor en pixeles (vertical)
            float dXS = (float)displayd * factor; // diagonal menor en pixeles (horizontal)

            // Centro del panel
            float cx = panelDibujo.Width / 2.0f;
            float cy = panelDibujo.Height / 2.0f;

            // Vertices del rombo centrado
            PointF pTop = new PointF(cx, cy - dYS / 2f);
            PointF pRight = new PointF(cx + dXS / 2f, cy);
            PointF pBottom = new PointF(cx, cy + dYS / 2f);
            PointF pLeft = new PointF(cx - dXS / 2f, cy);
            PointF[] vertices = { pTop, pRight, pBottom, pLeft };

            // Dibujamos usando using para liberar recursos
            using (Graphics g = panelDibujo.CreateGraphics())
            {
                g.Clear(Color.White);
                Color fill = Color.FromArgb(255, 211, 105);
                using (SolidBrush brush = new SolidBrush(fill))
                using (Pen pen = new Pen(Color.FromArgb(45, 52, 71), 2))
                {
                    g.FillPolygon(brush, vertices);
                    g.DrawPolygon(pen, vertices);
                }
            }

            if (usedMinForDisplay)
            {
                MessageBox.Show("Se requieren valores mayores que 0 para dibujar a escala real. Se usaron valores mínimos visuales para mostrar la figura.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            InicializarData();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}