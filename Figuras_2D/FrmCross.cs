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
    public partial class FrmCross : Form
    {
        // Variable global para almacenar el tamaño ingresado y usarlo al dibujar
        private float tamanoLado = 0;

        public FrmCross()
        {
            InitializeComponent();
        }

        // Evento del botón "Calcular" (button1)
        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Validar que el usuario ingrese un formato de número válido
            if (float.TryParse(textBox1.Text, out float tamaño))
            {
                // 2. Validar que el número no sea negativo y esté en el rango de 0 a 12 cm
                if (tamaño >= 0 && tamaño <= 10)
                {
                    tamanoLado = tamaño;

                    // Cálculos matemáticos
                    float area = 5 * (tamaño * tamaño);
                    float perimetro = 12 * tamaño;

                    // Mostrar resultados en los TextBoxes correspondientes
                    textBox2.Text = area.ToString("0.##");
                    textBox3.Text = perimetro.ToString("0.##");

                    // Invalidar el panel para forzar que se ejecute el evento Paint y dibuje la cruz
                    panel1.Invalidate();
                }
                else
                {
                    // Mensaje de error si está fuera del rango
                    MessageBox.Show("Por favor, ingrese un tamaño válido entre 0 y 9 cm.", "Rango inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Limpiar y enfocar para que el usuario intente de nuevo
                    textBox1.Clear();
                    textBox1.Focus();
                }
            }
            else
            {
                // Mensaje de error si ingresa letras o símbolos
                MessageBox.Show("Por favor, ingrese un número válido.", "Error de entrada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Clear();
                textBox1.Focus();
            }
        }

        // Evento del botón "Resetear" (button2)
        private void button2_Click(object sender, EventArgs e)
        {
            // Limpiar cajas de texto
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

            // Reiniciar tamaño y borrar el dibujo
            tamanoLado = 0;
            panel1.Invalidate();

            // Poner el cursor de vuelta en el primer input
            textBox1.Focus();
        }

        // Evento Paint del panel (panel1) para dibujar la figura
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Si el tamaño es 0 o negativo, no dibujamos nada
            if (tamanoLado <= 0) return;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // Mejorar bordes

            // Escalar el tamaño para que se vea proporcionado en pantalla (ej: 1cm = 15 pixeles)
            float escala = 15f;
            float w = tamanoLado * escala;

            // Encontrar el centro del panel
            float cx = panel1.Width / 2f;
            float cy = panel1.Height / 2f;

            // Coordenadas de los 12 vértices del polígono en forma de cruz
            PointF[] puntos = new PointF[]
            {
                new PointF(cx - w/2, cy - 3*w/2), // 1. Arriba Izquierda
                new PointF(cx + w/2, cy - 3*w/2), // 2. Arriba Derecha
                new PointF(cx + w/2, cy - w/2),   // 3. Esquina interior superior derecha
                new PointF(cx + 3*w/2, cy - w/2), // 4. Derecha Arriba
                new PointF(cx + 3*w/2, cy + w/2), // 5. Derecha Abajo
                new PointF(cx + w/2, cy + w/2),   // 6. Esquina interior inferior derecha
                new PointF(cx + w/2, cy + 3*w/2), // 7. Abajo Derecha
                new PointF(cx - w/2, cy + 3*w/2), // 8. Abajo Izquierda
                new PointF(cx - w/2, cy + w/2),   // 9. Esquina interior inferior izquierda
                new PointF(cx - 3*w/2, cy + w/2), // 10. Izquierda Abajo
                new PointF(cx - 3*w/2, cy - w/2), // 11. Izquierda Arriba
                new PointF(cx - w/2, cy - w/2)    // 12. Esquina interior superior izquierda
            };

            // 1. Dibujar el relleno de la cruz (Color celeste similar a la imagen)
            using (SolidBrush brocha = new SolidBrush(Color.FromArgb(52, 152, 219)))
            {
                g.FillPolygon(brocha, puntos);
            }

            // 2. Dibujar el contorno de la cruz (Grosor 2)
            using (Pen lapiz = new Pen(Color.FromArgb(44, 62, 80), 2))
            {
                g.DrawPolygon(lapiz, puntos);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}