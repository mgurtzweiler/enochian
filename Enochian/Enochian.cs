using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Enochian;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Objects;
using TreeSharp;

namespace Enochian
{
    public class Enochian : CombatRoutine
    {
        public override string Name
        {
            get { return "Enochian"; }
        }

        public override float PullRange
        {
            get { return 15.0f; }
        }

        public override ClassJobType[] Class
        {
            get {  return new[] { ClassJobType.BlackMage }; }
        }

        private BlackMage Routine = new BlackMage();

        public override void Pulse()
        {
            base.Pulse();
        }

        public override Composite CombatBehavior
        {
            get { return new ActionRunCoroutine(ctx => Routine.Combat(new GameState(Core.Player))); } 
        }
    }

    public class GameState
    {
        public uint CurrentMana;
        public GameState(LocalPlayer player)
        {
            CurrentMana = player.CurrentMana;
            foreach (var aura in player.CharacterAuras.AuraList)
            {
                Logging.Write(Colors.Red, "Aura: {0} {1} {2}", aura.LocalizedName, aura.Id, aura.TimeLeft);
            }
        }
    }
}
