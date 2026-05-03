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
    public partial class FrmArrow : Form
    {
        // Variable global para almacenar el tamaño ingresado
        private float tamanoFlecha = 0;

        public FrmArrow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validar formato de número
            if (float.TryParse(textBox1.Text, out float tamaño))
            {
                // Validar rango de 0 a 12 cm
                if (tamaño >= 0 && tamaño <= 12)
                {
                    tamanoFlecha = tamaño;

                    // Cálculos matemáticos (Área = 5L^2, Perímetro = 12L)
                    float area = 5 * (tamaño * tamaño);
                    float perimetro = 12 * tamaño;

                    // Mostrar resultados
                    textBox2.Text = area.ToString("0.##");
                    textBox3.Text = perimetro.ToString("0.##");

                    // Dibujar
                    panel1.Invalidate();
                }
                else
                {
                    MessageBox.Show("Por favor, ingrese un tamaño válido entre 0 y 12 cm.", "Rango inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox1.Clear();
                    textBox1.Focus();
                }
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un número válido.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Clear();
                textBox1.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Limpiar cajas
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

            // Reiniciar dibujo
            tamanoFlecha = 0;
            panel1.Invalidate();

            textBox1.Focus();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // No dibujar si no hay tamaño
            if (tamanoFlecha <= 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Escala y tamaño base
            float escala = 15f;
            float L = tamanoFlecha * escala;

            // Centro del panel
            float cx = panel1.Width / 2f;
            float cy = panel1.Height / 2f;

            // Los 7 vértices de la flecha
            PointF[] puntosFlecha = new PointF[]
            {
                new PointF(cx - 2*L, cy - L/2f),    //  Atrás arriba
                new PointF(cx, cy - L/2f),          //  Cuello arriba
                new PointF(cx, cy - 1.5f*L),        //  Aleta superior
                new PointF(cx + 2*L, cy),           //  Punta derecha
                new PointF(cx, cy + 1.5f*L),        //  Aleta inferior
                new PointF(cx, cy + L/2f),          //  Cuello abajo
                new PointF(cx - 2*L, cy + L/2f)     //  Atrás abajo
            };

            // Relleno celeste
            using (SolidBrush brocha = new SolidBrush(Color.FromArgb(52, 152, 219)))
            {
                g.FillPolygon(brocha, puntosFlecha);
            }

            // Borde oscuro
            using (Pen lapiz = new Pen(Color.FromArgb(44, 62, 80), 2))
            {
                g.DrawPolygon(lapiz, puntosFlecha);
            }
        }
    }
}