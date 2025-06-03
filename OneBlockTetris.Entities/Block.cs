using System.Windows.Forms;
using System.Drawing;

namespace OneBlockTetris.Entities
{
    public class Block : Panel
    {
        public int CurrentRow { get; set; }
        public int CurrentColumn { get; set; }

        public Block(int blockSize)
        {
            // grootte van het blokje
            Width = blockSize;
            Height = blockSize;
            // kleur mag je zelf kiezen
            BackColor = Color.Red;
            // rand instellen
            BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
