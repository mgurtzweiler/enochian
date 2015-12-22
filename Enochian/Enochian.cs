using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Clio.Utilities;
using Enochian;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Enums;
using ff14bot.Helpers;
using ff14bot.Interfaces;
using ff14bot.Managers;
using ff14bot.Objects;
using TreeSharp;

namespace Enochian
{
    public class SpellCast
    {
        public Spell Spell { get; set; }
        public DateTime CastTime { get; set;  }

        public SpellCast(Spell spell)
        {
            Spell = spell;
            CastTime = DateTime.Now;
        }
    }
    public class Enochian : CombatRoutine
    {
        public static Spell LastSpell { get; set; }
        public static List<SpellCast> LastSpells { get; private set; }

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
            // clean up after a min
            LastSpells.RemoveAll(x => x.CastTime < DateTime.Now.AddMinutes(-1));
        }

        public override void Initialize()
        {
            base.Initialize();
            LastSpells = new IndexedList<SpellCast>();
            Logging.Write(Colors.BlueViolet, "[Enochian] Init");
        }

        public override Composite CombatBehavior
        {
            get { return new ActionRunCoroutine(ctx => Routine.Combat(Core.Player)); } 
        }
    }


    
}
