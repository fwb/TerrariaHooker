using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RomTerraria
{
    public class Trainer : GameComponent
    {
        private Terraria.Main TerrariaGame; // For future reference

        public bool RestoreHealth { get; set; }
        public bool RestoreMana { get; set; }
        public int RestoreTimer { get; set; }

        private TimeSpan timeSinceLastBuff = TimeSpan.Zero;

        public Trainer(Game game) : base(game)
        {
            TerrariaGame = (Terraria.Main)game;
        }

        public override void Update(GameTime gameTime)
        {
            //if (Terraria.Main.netMode != 0) return; // No trainer in MP

            timeSinceLastBuff += gameTime.ElapsedGameTime;

            if( timeSinceLastBuff < TimeSpan.FromMilliseconds(RestoreTimer ) ) {
                return;
            }

            timeSinceLastBuff = TimeSpan.Zero;

            var p = Terraria.Main.player[Terraria.Main.myPlayer];

            if (p != null &&
                p.active)
            {
                if (p.statLife < p.statLifeMax && RestoreHealth)
                    p.statLife++;

                if (p.statMana < p.statManaMax && RestoreMana)
                    p.statMana++;
            }

            base.Update(gameTime);
        }
    }
}
