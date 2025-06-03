using System;
using System.Windows.Forms;
using OneBlockTetris.Bll;

namespace OneBlockTetris
{
    public partial class frmOneBlockTetris : Form
    {
        // Game uit Bll als property van onze form
        public Game Game { get; set; }

        public frmOneBlockTetris()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            // nieuw Game maken en de huidige form koppelen
            Game = new Game(this);
            // form aanpassen
            Width = Game.Playfield.Width + 100; 
            Height = Game.Playfield.Height + 40;
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void FrmOneBlockTetris_Load(object sender, EventArgs e)
        {
            StartGame();
        }
    }
}
