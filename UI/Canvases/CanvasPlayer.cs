using MamacitaOrbit.Main;
using MamacitaOrbit.UI.Buttons;
using System.Diagnostics;

namespace MamacitaOrbit.UI.Canvases
{
    internal sealed class CanvasPlayer : Canvas
    {
        private Text uridiumNumberText;
        private Text creditNumberText;
        private Text laserDamageText;

        private long uridiumNumber;
        private long creditNumber;

        private Button quitCanvasButton;

        private float incrementationTimer;
        private float incrementationCreditTimer;
        private readonly float incrementationCooldown;
        private readonly float incrementationCreditCooldown;
        public CanvasPlayer(Texture2D texture, Rectangle rect, float depthLayer, string name, 
            bool isDraggable = true, bool isScissorCanvas = false) : base(texture, rect, depthLayer, name, isDraggable, isScissorCanvas)
        {
            quitCanvasButton = AddButton(0, 0, 30, 17,
                Globals.Content.Load<Texture2D>("UI/canvasButtons/31"),
                Globals.Content.Load<Texture2D>("UI/canvasButtons/34"),
                (s, e) => UIManager.CallCanvas(s, e, this));
            ;

            /*incrementationTimer = 0.001f;
            incrementationCreditTimer = 0.01f;
            incrementationCooldown = incrementationTimer;
            incrementationCreditCooldown = incrementationCreditTimer;*/

            /*uridiumNumber = Globals.Player.Uridium;
            creditNumber = Globals.Player.Credit;*/

            uridiumNumberText = AddText(35, 40, $"{uridiumNumber}", Color.White);
            creditNumberText = AddText(35, 70, $"{creditNumber}", Color.White);
            laserDamageText = AddText(200, 70, $"{Globals.Player.ProjectileDamage}", Color.White); 

            var uridiumIcon = AddIcon(15, 32, 15, 15, Globals.Content.Load<Texture2D>("UI/iconsAndShii/9_shipInfoIcon_uridium"));
            var creditIcon = AddIcon(15, 62, 15, 15, Globals.Content.Load<Texture2D>("UI/iconsAndShii/146_shipInfoIcon_credits"));
            var laserIcon = AddIcon(165, 63, 15, 15, Globals.Content.Load<Texture2D>("UI/iconsAndShii/89_shipInfoIcon_laser"));
        }

        public override void Update()
        {
            base.Update();

            /*incrementationTimer -= Globals.DeltaTime;
            incrementationCreditTimer -= Globals.DeltaTime;
            SuperCoolIncrementation();*/
            laserDamageText.Message = $"{Globals.Player.ProjectileDamage}";
            uridiumNumberText.Message = Globals.Player.Uridium.ToString();
            creditNumberText.Message = Globals.Player.Credit.ToString();
        }

        /*private void SuperCoolIncrementation()
        {
            if (incrementationTimer > 0)
                return;

            if (uridiumNumber < Globals.Player.Uridium)
            {
                uridiumNumber += 1;
                incrementationTimer = incrementationCooldown;
                
            }
            else if (uridiumNumber > Globals.Player.Uridium || Globals.Player.Uridium < 0)
            {
                uridiumNumber = Globals.Player.Uridium;
                incrementationTimer = incrementationCooldown;
                
            }
            

            if (incrementationCreditTimer > 0)
                return;
            
            if (creditNumber < Globals.Player.Credit && Globals.Player.Credit > 0)
            {
                creditNumber += Globals.Player.Credit / 1000;
                incrementationCreditTimer = incrementationCooldown;
            }
            else if (creditNumber > Globals.Player.Credit || Globals.Player.Credit < 0)
            {
                creditNumber = Globals.Player.Credit;
                incrementationCreditTimer = incrementationCooldown;
            }
            
        }*/
    }
}
