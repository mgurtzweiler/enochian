using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Buddy.Coroutines;
using ff14bot.Helpers;
using ff14bot.Managers;
using ff14bot.Objects;

namespace Enochian
{
    public enum CastType
    {
        Target,
        Self,
        TargetLocation,
        SelfLocation
    }
    public enum GCDType
    {
        On,
        Off
    }
    public enum SpellType
    {
        Damage,
        AoE,
        DoT,
        Movement,
        Buff,
        Debuff,
        Interrupt,
        Knockback,
        Execute,
        Aura,
        Cooldown,
        Defensive,
        Heal,
        Summon,
        Pet,
        Behind,
        Flank,
        Ninjutsu,
        Mudra,
        Card
    }

    public class Spell
    {
        public string Name { get; set; }
        public uint ID { get; set; }
        public byte Level { get; set; }
        public GCDType GCDType { private get; set; }
        public SpellType SpellType { private get; set; }
        public CastType CastType { private get; set; }

        public async Task<bool> Cast(GameObject target = null)
        {
            Logging.Write(Colors.Red, "[Enochian] Casting {0}", Name);
            Enochian.LastSpell = this;
            await Coroutine.Wait(1000, () => Actionmanager.DoAction(ID, target));
            return true;
        }
    }

    public class Spells
    {
        private Spell _blizzard;
        public Spell Blizzard
        {
            get
            {
                return _blizzard ??
                       (_blizzard =
                           new Spell
                           {
                               Name = "Blizzard",
                               ID = 142,
                               Level = 1,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Damage,
                               CastType = CastType.Target
                           });
            }
        }
        private Spell _fire;
        public Spell Fire
        {
            get
            {
                return _fire ??
                       (_fire =
                           new Spell
                           {
                               Name = "Fire",
                               ID = 141,
                               Level = 2,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Damage,
                               CastType = CastType.Target
                           });
            }
        }
        private Spell _transpose;
        public Spell Transpose
        {
            get
            {
                return _transpose ??
                       (_transpose =
                           new Spell
                           {
                               Name = "Transpose",
                               ID = 149,
                               Level = 4,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Buff,
                               CastType = CastType.Self
                           });
            }
        }
        private Spell _thunder;
        public Spell Thunder
        {
            get
            {
                return _thunder ??
                       (_thunder =
                           new Spell
                           {
                               Name = "Thunder",
                               ID = 144,
                               Level = 6,
                               GCDType = GCDType.On,
                               SpellType = SpellType.DoT,
                               CastType = CastType.Target
                           });
            }
        }
        private Spell _surecast;
        public Spell Surecast
        {
            get
            {
                return _surecast ??
                       (_surecast =
                           new Spell
                           {
                               Name = "Surecast",
                               ID = 143,
                               Level = 8,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Defensive,
                               CastType = CastType.Self
                           });
            }
        }
        private Spell _sleep;
        public Spell Sleep
        {
            get
            {
                return _sleep ??
                       (_sleep =
                           new Spell
                           {
                               Name = "Sleep",
                               ID = 145,
                               Level = 10,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Debuff,
                               CastType = CastType.Target
                           });
            }
        }
        private Spell _blizzardII;
        public Spell BlizzardII
        {
            get
            {
                return _blizzardII ??
                       (_blizzardII =
                           new Spell
                           {
                               Name = "Blizzard II",
                               ID = 146,
                               Level = 12,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Debuff,
                               CastType = CastType.Self
                           });
            }
        }
        private Spell _scathe;
        public Spell Scathe
        {
            get
            {
                return _scathe ??
                       (_scathe =
                           new Spell
                           {
                               Name = "Scathe",
                               ID = 156,
                               Level = 15,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Damage,
                               CastType = CastType.Target
                           });
            }
        }
        private Spell _fireII;
        public Spell FireII
        {
            get
            {
                return _fireII ??
                       (_fireII =
                           new Spell
                           {
                               Name = "Fire II",
                               ID = 147,
                               Level = 18,
                               GCDType = GCDType.On,
                               SpellType = SpellType.AoE,
                               CastType = CastType.Target
                           });
            }
        }
        private Spell _thunderII;
        public Spell ThunderII
        {
            get
            {
                return _thunderII ??
                       (_thunderII =
                           new Spell
                           {
                               Name = "Thunder II",
                               ID = 148,
                               Level = 22,
                               GCDType = GCDType.On,
                               SpellType = SpellType.DoT,
                               CastType = CastType.Target
                           });
            }
        }
        private Spell _swiftcast;
        public Spell Swiftcast
        {
            get
            {
                return _swiftcast ??
                       (_swiftcast =
                           new Spell
                           {
                               Name = "Swiftcast",
                               ID = 150,
                               Level = 26,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Buff,
                               CastType = CastType.Self
                           });
            }
        }
        private Spell _manaward;
        public Spell Manaward
        {
            get
            {
                return _manaward ??
                       (_manaward =
                           new Spell
                           {
                               Name = "Manaward",
                               ID = 157,
                               Level = 30,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Defensive,
                               CastType = CastType.Self
                           });
            }
        }
        private Spell _fireIII;
        public Spell FireIII
        {
            get
            {
                return _fireIII ??
                       (_fireIII =
                           new Spell
                           {
                               Name = "Fire III",
                               ID = 152,
                               Level = 34,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Damage,
                               CastType = CastType.Target
                           });
            }
        }
        private Spell _blizzardIII;
        public Spell BlizzardIII
        {
            get
            {
                return _blizzardIII ??
                       (_blizzardIII =
                           new Spell
                           {
                               Name = "Blizzard III",
                               ID = 154,
                               Level = 38,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Damage,
                               CastType = CastType.Target
                           });
            }
        }
        private Spell _lethargy;
        public Spell Lethargy
        {
            get
            {
                return _lethargy ??
                       (_lethargy =
                           new Spell
                           {
                               Name = "Lethargy",
                               ID = 151,
                               Level = 42,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Debuff,
                               CastType = CastType.Target
                           });
            }
        }
        private Spell _thunderIII;
        public Spell ThunderIII
        {
            get
            {
                return _thunderIII ??
                       (_thunderIII =
                           new Spell
                           {
                               Name = "Thunder III",
                               ID = 153,
                               Level = 46,
                               GCDType = GCDType.On,
                               SpellType = SpellType.DoT,
                               CastType = CastType.Target
                           });
            }
        }
        private Spell _aetherialmanipulation;
        public Spell AetherialManipulation
        {
            get
            {
                return _aetherialmanipulation ??
                       (_aetherialmanipulation =
                           new Spell
                           {
                               Name = "Aetherial Manipulation",
                               ID = 155,
                               Level = 50,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Defensive,
                               CastType = CastType.Self
                           });
            }
        }

        private Spell _convert;
        public Spell Convert
        {
            get
            {
                return _convert ??
                       (_convert =
                           new Spell
                           {
                               Name = "Convert",
                               ID = 158,
                               Level = 30,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Buff,
                               CastType = CastType.Self
                           });
            }
        }
        private Spell _freeze;
        public Spell Freeze
        {
            get
            {
                return _freeze ??
                       (_freeze =
                           new Spell
                           {
                               Name = "Freeze",
                               ID = 159,
                               Level = 35,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Debuff,
                               CastType = CastType.TargetLocation
                           });
            }
        }
        private Spell _apocatastasis;
        public Spell Apocatastasis
        {
            get
            {
                return _apocatastasis ??
                       (_apocatastasis =
                           new Spell
                           {
                               Name = "Apocatastasis",
                               ID = 160,
                               Level = 40,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Defensive,
                               CastType = CastType.Target
                           });
            }
        }
        private Spell _manawall;
        public Spell Manawall
        {
            get
            {
                return _manawall ??
                       (_manawall =
                           new Spell
                           {
                               Name = "Manawall",
                               ID = 161,
                               Level = 45,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Defensive,
                               CastType = CastType.Self
                           });
            }
        }
        private Spell _flare;
        public Spell Flare
        {
            get
            {
                return _flare ??
                       (_flare =
                           new Spell
                           {
                               Name = "Flare",
                               ID = 162,
                               Level = 50,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Cooldown,
                               CastType = CastType.Target
                           });
            }
        }
        private Spell _leylines;
        public Spell LeyLines
        {
            get
            {
                return _leylines ??
                       (_leylines =
                           new Spell
                           {
                               Name = "Ley Lines",
                               ID = 3573,
                               Level = 52,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Buff,
                               CastType = CastType.SelfLocation
                           });
            }
        }
        private Spell _sharpcast;
        public Spell Sharpcast
        {
            get
            {
                return _sharpcast ??
                       (_sharpcast =
                           new Spell
                           {
                               Name = "Sharpcast",
                               ID = 3574,
                               Level = 54,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Buff,
                               CastType = CastType.Self
                           });
            }
        }
        private Spell _enochian;
        public Spell Enochian
        {
            get
            {
                return _enochian ??
                       (_enochian =
                           new Spell
                           {
                               Name = "Enochian",
                               ID = 3575,
                               Level = 56,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Buff,
                               CastType = CastType.Self
                           });
            }
        }
        private Spell _blizzardIV;
        public Spell BlizzardIV
        {
            get
            {
                return _blizzardIV ??
                       (_blizzardIV =
                           new Spell
                           {
                               Name = "Blizzard IV",
                               ID = 3576,
                               Level = 58,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Damage,
                               CastType = CastType.Target
                           });
            }
        }
        private Spell _fireIV;
        public Spell FireIV
        {
            get
            {
                return _fireIV ??
                       (_fireIV =
                           new Spell
                           {
                               Name = "Fire IV",
                               ID = 3577,
                               Level = 60,
                               GCDType = GCDType.On,
                               SpellType = SpellType.Damage,
                               CastType = CastType.Target
                           });
            }
        }
    }
}
