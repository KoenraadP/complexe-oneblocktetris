using OneBlockTetris.Entities;
using System.Drawing;
using System.Windows.Forms;

namespace OneBlockTetris.Bll
{
    public class Game
    {
        #region properties
        // form waarop gespeeld zal worden
        public Form MainForm { get; set; }
        // grootte in pixels van één blokje
        public int BlockSize { get; set; }
        // de panel (speelveld) waarop gespeeld zal worden
        public Panel Playfield { get; set; }
        // aantal kolommen en rijen van het speelveld
        public int PlayfieldColumns { get; set; }
        public int PlayfieldRows { get; set; }
        // bijhouden op welk level de speler zit
        public int Level { get; set; }
        // bijhouden hoeveel lijnen al gemaakt werden in totaal
        public int LinesTotal { get; set; }
        // bijhouden hoeveel lijnen er al gemaakt zijn in het huidige level
        // als deze waarde dan bvb 10 wordt, dan gaat de speler naar het volgende level
        public int LinesThisLevel { get; set; }
        // instellen hoeveel lijnen er moeten gemaakt worden om naar het volgende level te gaan
        public int LinesToAdvance { get; set; }
        // timer die automatisch de blokjes laat vallen
        public Timer GameTime { get; set; }
        // controleren of de speler een blokje naar links of rechts wil bewegen
        public bool LeftPressed { get; set; }
        public bool RightPressed { get; set; }
        // hoe snel vallen de blokjes
        public int Speed { get; set; }
        // hoe veel sneller moeten de blokjes vallen per level
        public int SpeedUp { get; set; }
        // de huidige blok die aan het vallen is
        public Block ActiveBlock { get; set; }
        #endregion

        public Game(Form form)
        {
            MainForm = form;

            // standaard waarden
            BlockSize = 25;
            PlayfieldColumns = 10;
            PlayfieldRows = 20;
            Level = 1;
            LinesTotal = 0;
            LinesThisLevel = 0;
            LinesToAdvance = 10;
            Speed = 500; // milliseconden, 1 seconde is 1000 ms
            SpeedUp = 100; // hoeveelheid milliseconden die er van de snelheid afgetrokken wordt per level

            // maak een nieuw speelveld
            // hierin zullen de blokjes vallen en geplaatst worden
            Playfield = new Panel
            {
                // er moeten tien blokjes in de breedte passen
                // en twintig blokjes in de hoogte
                Width = PlayfieldColumns * BlockSize,
                Height = PlayfieldRows * BlockSize,
                BackColor = Color.LightGray                
            };
            // voeg het speelveld toe aan de form
            MainForm.Controls.Add(Playfield);

            // alternatieve manier van playfield maken
            //Playfield = new Panel();
            //Playfield.Width = ...
        }

        private void NewBlock()
        {
            // hier komt de code om een nieuw blokje te maken
            // en toe te voegen aan het speelveld
            // BlockSize werd ingesteld op 25 pixels in de constructor van Game
            ActiveBlock = new Block(BlockSize)
            {
                CurrentRow = 0,
                CurrentColumn = PlayfieldColumns / 2 - 1 // blokje start 'ongeveer' in het midden van het speelveld 
            };
        }
    }
}
