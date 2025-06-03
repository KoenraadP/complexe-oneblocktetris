using OneBlockTetris.Entities;
using System;
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
        public Timer GameTimer { get; set; }
        // controleren of de speler een blokje naar links of rechts wil bewegen
        public bool LeftPressed { get; set; }
        public bool RightPressed { get; set; }
        // hoe snel vallen de blokjes
        public int Speed { get; set; }
        // hoe veel sneller moeten de blokjes vallen per level
        public int SpeedUp { get; set; }
        // de huidige blok die aan het vallen is
        public Block ActiveBlock { get; set; }
        // grid waarin we bijhouden waar de blokjes al staan
        // standaard zal een bool altijd false zijn als je deze niet initialiseert
        public bool[,] Playgrid { get; set; }
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
            Speed = 50; // milliseconden, 1 seconde is 1000 ms
            SpeedUp = 10; // hoeveelheid milliseconden die er van de snelheid afgetrokken wordt per level

            // grid initialiseren op 20 rijen en 10 kolommen
            Playgrid = new bool[PlayfieldRows, PlayfieldColumns];

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

            // eerste blokje aanmaken en plaatsen
            NewBlock();

            // timer aanmaken
            GameTimer = new Timer
            {
                Interval = Speed // interval in milliseconden
            };
            // event handler toevoegen voor de timer
            GameTimer.Tick += GameTimer_Tick;
            GameTimer.Start();
        }

        // wat moet er gebeuren iedere keer de timer 'tickt'
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            // controleren of blokje onderaan staat of er een ander blokje onder staat
            // als dat zo is, dan moet er een nieuw blokje komen bovenaan
            if (CheckDown() == true)
            {
                NewBlock();
            }
            MoveBlock();
        }

        // code om blokje te verplaatsen
        private void MoveBlock()
        {
            // controleren of de speler een blokje naar links of rechts wil bewegen
            // ook controleren dat we niet buiten de grenzen van het speelveld gaan
            if (LeftPressed == true && ActiveBlock.CurrentColumn != 0)
            {
                ActiveBlock.Left -= BlockSize; // blokje 1 kolom naar links verplaatsen
                ActiveBlock.CurrentColumn--; // huidige kolom verlagen
            }

            if (RightPressed == true && ActiveBlock.CurrentColumn != 9)
            {
                ActiveBlock.Left += BlockSize; // blokje 1 kolom naar rechts verplaatsen
                ActiveBlock.CurrentColumn++; // huidige kolom verhogen                
            }

            ActiveBlock.Top += BlockSize; // blokje 1 rij naar beneden verplaatsen
            ActiveBlock.CurrentRow++; // huidige rij verhogen

            // reset de toetsen zodat het blokje niet blijft bewegen
            LeftPressed = false; // reset de linkse toets zodat het blokje niet blijft bewegen
            RightPressed = false; // reset de rechtse toets zodat het blokje niet blijft bewegen
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

            // locatie van blokje instellen
            ActiveBlock.Location = new Point(
                ActiveBlock.CurrentColumn * BlockSize,
                ActiveBlock.CurrentRow * BlockSize
            );

            // voeg het blokje toe aan het speelveld
            Playfield.Controls.Add(ActiveBlock);
        }

        // methode die controleert of het blokje beneden staat
        // of dat er een ander blokje onder staat
        private bool CheckDown()
        {
            // twee voorwaarden: eerst controle of we de onderste rij bereikt hebben van het speelveld
            // daarna ook controleren of er een blokje onder het huidige blokje staat in de grid
            if (ActiveBlock.CurrentRow == PlayfieldRows - 1
                || Playgrid[ActiveBlock.CurrentRow + 1, ActiveBlock.CurrentColumn] == true)
            {
                // juiste locatie in grid op true zetten
                Playgrid[ActiveBlock.CurrentRow, ActiveBlock.CurrentColumn] = true;
                // controleren of er een lijn gemaakt werd
                if (CheckLine() == true)
                {
                    
                }
                return true;
            }
            return false;
        }

        // methode die controleert of er een lijn gemaakt werd
        // dus 10 true states naast elkaar binnen één rij
        private bool CheckLine()
        {
            for (int col = 0; col < PlayfieldColumns; col++)
            {
                // als er een false is in de rij, dan is er geen lijn gemaakt
                if (Playgrid[ActiveBlock.CurrentRow, col] == false)
                {
                    return false;
                }
            }
            // als we hier komen, dan is er een lijn gemaakt 
            // want dan stonden alle vakjes binnen de rij op true
            return true;
        }

        private void DropField()
        {
            // alle blokjes (panels) die nu op het speelveld staan, verwijderen
            Playfield.Controls.Clear();
            // gemaakte lijn (dus tien x true) wissen uit de grid array
            // alle overige rijen in de grid één rij naar beneden verplaatsen
            // huidige array kopiëren zonder de rij waarin de lijn gemaakt werd
            // in dit spel zal dit altijd de onderste rij zijn
            // we gaan hiervoor een tweede 'temp' array maken
            // en de playgrid dan vervangen door de nieuwe 'temp' array waarden
            bool[,] tempGrid = new bool[PlayfieldRows, PlayfieldColumns];
            // kopie starten vanaf het begin van de playgrid
            // plakken in temparray vanaf rij 1 (index 10 = eerste kolom van de tweede rij)
            // aantal items om te kopiëren uit playgrid is totaal aantal - de laatste rij dus - 10
            Array.Copy(Playgrid, 0, tempGrid, PlayfieldColumns, Playgrid.Length - PlayfieldColumns);
            // nu moeten we de playgrid vervangen door de nieuwe tempGrid
            Playgrid = tempGrid;
        }
    }
}
