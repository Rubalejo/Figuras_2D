using System;
using System.Drawing;
using System.Windows.Forms;

namespace Figuras_2D
{
    public partial class frmCircle : Form
    {
        int radio;
        bool dibujar = false;

        public frmCircle()
        {
            InitializeComponent();
        }

        // BOTÓN GRAFICAR
        private void btnGraficar_Click(object sender, EventArgs e)
        {
            int x, y; // solo para validar, ya no se usan para dibujar

            if (!int.TryParse(txtPosicionX.Text, out x) ||
                !int.TryParse(txtPosicionY.Text, out y) ||
                !int.TryParse(txtRadio.Text, out radio))
            {
                MessageBox.Show("Ingrese valores numéricos válidos");
                return;
            }

            if (radio <= 0)
            {
                MessageBox.Show("El radio debe ser mayor a 0");
                return;
            }

            if (x < 0 || y < 0)
            {
                MessageBox.Show("Las posiciones no pueden ser negativas");
                return;
            }

            // Cálculos
            double area = Math.PI * Math.Pow(radio, 2);
            double perimetro = 2 * Math.PI * radio;

            MessageBox.Show("Área: " + area.ToString("0.00") +
                            "\nPerímetro: " + perimetro.ToString("0.00"));

            dibujar = true;
            panel1.Invalidate();
        }

        // DIBUJO CENTRADO
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.DrawRectangle(Pens.Black, 0, 0, panel1.Width - 1, panel1.Height - 1);

            if (!dibujar) return;

            int diametro = radio * 2;
                        
            int posX = (panel1.Width - diametro) / 2;
            int posY = (panel1.Height - diametro) / 2;

            // Color aleatorio
            Random rnd = new Random();
            Brush relleno = new SolidBrush(Color.FromArgb(
                rnd.Next(256),
                rnd.Next(256),
                rnd.Next(256)
            ));

            Brush borde = new SolidBrush(Color.FromArgb(176, 196, 114)); // verde claro de la imagen
            Pen lapiz = new Pen(Color.Black, 3); // borde oscuro

            // Dibujar
            g.FillEllipse(relleno, posX, posY, diametro, diametro);
            g.DrawEllipse(lapiz, posX, posY, diametro, diametro);
        }

        // RESETEAR

        private void btnResetear_Click(object sender, EventArgs e)
        {
            txtPosicionX.Clear();
            txtPosicionY.Clear();
            txtRadio.Clear();

            dibujar = false;

            panel1.Invalidate();
        }

        // SALIR
        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}