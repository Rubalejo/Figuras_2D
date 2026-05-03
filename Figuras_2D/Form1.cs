using Figuras2D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Figuras_2D
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void circleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCircle form = new frmCircle();
            form.Show();

        }

        private void elliipseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEllipse form = new FrmEllipse();
            form.Show();
        }

        private void rectangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRectangle form = new FrmRectangle();
            form.Show();

        }

        private void ovalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmOval form = new FrmOval();
            form.Show();
        }

        private void squareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCuadrado form = new FrmCuadrado();
            form.Show();
        }

        private void trapeziumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTrapezium form = new FrmTrapezium();
            form.Show();
        }

        private void parallelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmParallelogram form = new FrmParallelogram();
            form.Show();
        }

        private void rhombusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRhombus form = new FrmRhombus();
            form.Show();
        }

        private void kiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Kite form = new Kite();
            form.Show();
        }

        private void triangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Right_Triangle form = new Right_Triangle();
            form.Show();
        }

        private void triangleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Triangle form = new Triangle();
            form.Show();
        }

        private void scaleneTriangleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Scalene_Triangle form = new Scalene_Triangle();
            form.Show();
        }

        private void pentagonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPentagon form = new FrmPentagon();
            form.Show();
        }

        private void hexagonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmHexagon frmHexagon = new FrmHexagon();
            frmHexagon.Show();
        }

        private void heptagonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmHeptagon frmHeptagon = new FrmHeptagon();
            frmHeptagon.Show();
        }

        private void octagonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmOctagon frmOctagon = new FrmOctagon();
            frmOctagon.Show();
        }

        private void nonagonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmNonagon frmNonagon = new FrmNonagon();
            frmNonagon.Show();
        }

        private void decagonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDecagon frmDecagon = new FrmDecagon();
            frmDecagon.Show();
        }

        private void starToolStripMenuItem_Click(object sender, EventArgs e)
        {
            STAR frmStar = new STAR();
            frmStar.Show();
        }

        private void heartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HEART frmHeart = new HEART();
            frmHeart.Show();
        }

        private void crescentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CRESCENT frmCrescent = new CRESCENT();
            frmCrescent.Show();
        }

        private void crossToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCross frmCross = new FrmCross();
            frmCross.Show();

        }

        private void graphicsToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void PieStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmPie frmPie = new FrmPie();
            frmPie.Show();
        }

        private void arrowStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmArrow frmArrow = new FrmArrow();
            frmArrow.Show();
        }
    }
}
