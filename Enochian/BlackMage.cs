using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using ff14bot;
using ff14bot.AClasses;
using ff14bot.Helpers;
using ff14bot.Enums;
using ff14bot.Managers;
using ff14bot.Navigation;
using ff14bot.Objects;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Buddy.Coroutines;
using ff14bot.RemoteWindows;

namespace Enochian
{
    public class BlackMage
    {
        private Node SingleTarget30Rotation { get; set; }
        public BlackMage()
        {
            var spells = new Spells();
            // The casts
            var castThunderOne = new CastSpell(spells.Thunder);
            var castFireThree = new CastSpell(spells.FireIII);
            var castFire = new CastSpell(spells.Fire);
            var castBlizzardThree = new CastSpell(spells.BlizzardIII);
            var castBlizzardFour = new CastSpell(spells.BlizzardIV);
            var castFireFour = new CastSpell(spells.FireIV);
            var castEnochian = new CastBuff(spells.Enochian);
            // The tree
            var deadEnd = new DeadEndNode();
            var umbralIceLastTick = new CurrentMana(castFireThree, castThunderOne, 10000.0f);
            var lastSpellBlizzardThree = new LastSpellCast(castBlizzardFour, umbralIceLastTick, spells.BlizzardIII);
            var umbralIceManaGt2000 = new CurrentMana(lastSpellBlizzardThree, castBlizzardFour, 2000);
            var lastCastFireThree = new LastSpellCast(castFireFour, castFireThree, spells.FireIII);
            var umbralIceThreeAura = new HasAura(umbralIceManaGt2000, lastCastFireThree, "Umbral Ice III");
            var astralFireTimerLong = new CurrentMana(castFireFour, castBlizzardThree, 2000.0f);
            var astralFireTimerShort = new CurrentMana(castFire, castBlizzardThree, 2000.0f);
            var astralFireTimer = new AuraTimer(astralFireTimerLong, astralFireTimerShort, "Astral Fire III", 6f);
            var lastCastFireThreeWaitForBuff = new LastSpellCast(deadEnd, umbralIceThreeAura, spells.FireIII);
            var hasAstralFireThree = new HasAura(astralFireTimer, lastCastFireThreeWaitForBuff, "Astral Fire III");
            var canCastEnochian = new CanCastBuff(castEnochian, castBlizzardThree, spells.Enochian.ID);
            var fullManaAfterB4 = new CurrentMana(castFireThree, castThunderOne, 10000f);
            var lastSpellBliz4 = new LastSpellCast(fullManaAfterB4, castBlizzardFour, spells.BlizzardIV);
            var hasAstralFireThreeShortEnochian = new HasAura(castBlizzardThree, lastSpellBliz4, "Astral Fire III");
            
            var enochianTimeLeft = new AuraTimer(hasAstralFireThree, hasAstralFireThreeShortEnochian, "Enochian", 10f);
            SingleTarget30Rotation = new HasAura(enochianTimeLeft, canCastEnochian, "Enochian");
            
        }


        public async Task<bool> Combat(LocalPlayer player)
        {
            var spells = new Spells();
            if (MovementManager.IsMoving)
            {
                return await new Spells().Scathe.Cast(player);
            }
            if (player.IsCasting) return false;
            await SingleTarget30Rotation.DoAction(player);
            return true;
        }
    }

    public abstract class Node: Action
    {
        public Node No { get; set; }
        public Node Yes { get; set; }

        protected Node(Node yes, Node no)
        {
            Yes = yes;
            No = no;
        }
    }

    public abstract class EndNode : Node
    {
        protected EndNode() : base(null, null) { }
    }


    public class HasAura : Node
    {
        private string AuraName { get; set; }
        public HasAura(Node yes, Node no, string auraNme) : base(yes, no)
        {
            AuraName = auraNme;
        }

        public override async Task<bool> DoAction(LocalPlayer player)
        {
            var hasAura = player.HasAura(AuraName);
            if (hasAura)
            {
                Logging.Write(Colors.CornflowerBlue, "[Enochian] HasAura {0} - Yes", AuraName);
                return await Yes.DoAction(player);
            }
            await Coroutine.Wait(500, () => player.HasAura(AuraName));
            Logging.Write(Colors.CornflowerBlue, "[Enochian] HasAura {0} - No", AuraName);
            return await No.DoAction(player);
        }
    }

    public class LastSpellCast : Node
    {
        private Spell Spell { get; set; }
        public LastSpellCast(Node yes, Node no, Spell spell) : base(yes, no)
        {
            Spell = spell;
        }

        public override async Task<bool> DoAction(LocalPlayer player)
        {
            if (Enochian.LastSpell == Spell)
            {
                Logging.Write(Colors.CornflowerBlue, "[Enochian] LastSpellCast {0} - Yes", Spell.Name);
                return await Yes.DoAction(player);
            }
            Logging.Write(Colors.CornflowerBlue, "[Enochian] LastSpellCast {0} - No", Spell.Name);
            return await No.DoAction(player);
        }
    }

    public class AuraTimer : Node
    {
        private float Timer { get; set; }
        private string AuraName { get; set; }
        public AuraTimer(Node yes, Node no, string auraName, float timeRemaining) : base(yes, no)
        {
            Timer = timeRemaining;
            AuraName = auraName;
        }

        public override async Task<bool> DoAction(LocalPlayer player)
        {
            var aura = player.GetAuraByName(AuraName);
            if (aura != null && aura.TimeLeft > Timer)
            {
                Logging.Write(Colors.CornflowerBlue, "[Enochian] AuraTimer Yes ({0} => {1} > {2})", aura.Name, aura.TimeLeft, Timer);
                return await Yes.DoAction(player);
            }
            Logging.Write(Colors.CornflowerBlue, "[Enochian] AuraTimer No ({0} => {1} < {2})", aura.Name, aura.TimeLeft, Timer);
            return await No.DoAction(player);
        }
    }

    public class CurrentMana : Node
    {
        private float Amount { get; set; }
        public CurrentMana(Node yes, Node no, float amount) : base(yes, no)
        {
            Amount = amount;
        }

        public override async Task<bool> DoAction(LocalPlayer player)
        {
            if (player.CurrentMana > Amount)
            {
                Logging.Write(Colors.CornflowerBlue, "[Enochian] CurrentMana Yes ({0} > {1})", player.CurrentMana, Amount);
                return await Yes.DoAction(player);
            }
            Logging.Write(Colors.CornflowerBlue, "[Enochian] CurrentMana No ({0} < {1})", player.CurrentMana, Amount);
            return await No.DoAction(player);
        }
    }

    public class CanCastBuff : Node
    {
        public uint SpellId { get; set; }
        public CanCastBuff(Node yes, Node no, uint spellId) : base(yes, no)
        {
            SpellId = spellId;
        }
        public override async Task<bool> DoAction(LocalPlayer player)
        {
            if (Actionmanager.CanCast(SpellId, player))
            {
                Logging.Write(Colors.CornflowerBlue, "[Enochian] CanCastBuff Yes ({0})", SpellId);
                return await Yes.DoAction(player);
            }
            Logging.Write(Colors.CornflowerBlue, "[Enochian] CanCastBuff No ({0})", SpellId);
            return await No.DoAction(player);
        }
    }
    public class LastCastCheck : Node
    {
        public Spell Spell { get; set; }
        public LastCastCheck(Node yes, Node no, Spell spell) : base(yes, no)
        {
            Spell = spell;
        }
        public override async Task<bool> DoAction(LocalPlayer player)
        {
            if (Enochian.LastSpell == Spell)
            {
                Logging.Write(Colors.CornflowerBlue, "[Enochian] LastCastCheck Yes ({0})", Spell.Name);
                return await Yes.DoAction(player);
            }
            Logging.Write(Colors.CornflowerBlue, "[Enochian] LastCastCheck No ({0})", Spell.Name);
            return await No.DoAction(player);
        }
    }

    public class CastSpell : EndNode
    {
        private Spell Spell { get; set; }
        public CastSpell(Spell spell)
        {
            Spell = spell;
        }
        public override async Task<bool> DoAction(LocalPlayer player)
        {
            Logging.Write(Colors.Yellow, "[Enochian] CastSpell {0}", Spell.Name);
            await Spell.Cast(player.CurrentTarget);
            return true;
        }
    }

    public class CastBuff : EndNode
    {
        private Spell Spell { get; set; }
        public CastBuff(Spell spell)
        {
            Spell = spell;
        }
        public override async Task<bool> DoAction(LocalPlayer player)
        {
            Logging.Write(Colors.Green, "[Enochian] CastBuff {0}", Spell.Name);
            await Coroutine.Wait(500, () => Actionmanager.DoAction(Spell.ID, player)); 
            return true;
        }
    }

    public class DeadEndNode : EndNode
    {
        public override async Task<bool> DoAction(LocalPlayer player)
        {
            Logging.Write(Colors.Red, "[Enochian] Dead End");
            await Coroutine.Wait(500, () => Actionmanager.CanCast("Fire", player.CurrentTarget));
            return true;
        }
    }

    public class AuraState
    {
        public const int LeyLinesId = 737;
        public const int CircleOfPowerId = 738;
        public const int EnochianId = 868;
        public const int FireStarterId = 165;
        public const int ThunderCloudId = 164;
        public const int AstralFireIIIId = 175;
        public const int UmbralIceIIId = 178;

        public int AuraId { get; set; }
        public bool HasAura { get; set; }
        public float TimeLeft { get; set; }
    }
    public abstract class Action
    {
        public abstract Task<bool> DoAction(LocalPlayer player);

        internal AuraState GetAuraState(LocalPlayer player, int auraId)
        {
            if (player.Auras.Any(x => x.Id == auraId))
            {
                var aura = player.Auras.Single(x => x.Id == auraId);
                return new AuraState
                {
                    AuraId = auraId,
                    HasAura = true,
                    TimeLeft = aura.TimeLeft
                };
            }
            return new AuraState
            {
                AuraId = auraId,
                HasAura = false,
                TimeLeft = 0f
            };
        }
    }
/*
    public class EnochianBuff : Rule
    {
        public override async Task<bool> DoAction(LocalPlayer player)
        {
            var enochian = GetAuraState(player, AuraState.EnochianId);
            if (!enochian.HasAura && Actionmanager.CanCast(AuraState.EnochianId, player))
            {
                await new Spells().FireIV.Cast(player.CurrentTarget);
                return true;
            }
            return false;
        }
    }
    public class FireIV : Rule
    {
        public override async Task<bool> DoAction(LocalPlayer player)
        {
            
            var enochian = GetAuraState(player, AuraState.EnochianId);
            var af3 = GetAuraState(player, AuraState.AstralFireIIIId);
            var currentMana = player.CurrentMana;
            if (enochian.HasAura && af3.HasAura && enochian.TimeLeft > 6f && af3.TimeLeft > 6f && currentMana > 2000)
            {
                await new Spells().FireIV.Cast(player.CurrentTarget);
                return true;
            }
            return false;
        }
    }

    public class Fire : Rule
    {
        public override async Task<bool> DoAction(LocalPlayer player)
        {
            var enochian = GetAuraState(player, AuraState.EnochianId);
            var af3 = GetAuraState(player, AuraState.AstralFireIIIId);
            var currentMana = player.CurrentMana;
            if (af3.HasAura && af3.TimeLeft < 5f && currentMana > 2000)
            {
                await new Spells().Fire.Cast(player.CurrentTarget);
                return true;
            }
            return false;
        }
    }

    public class ThundercloudThunderIII : Rule
    {
        public override async Task<bool> DoAction(LocalPlayer player)
        {
            var aura = GetAuraState(player, AuraState.ThunderCloudId);
            //var af3 = GetAuraState(player, AuraState.AstralFireIIIId);
            var currentMana = player.CurrentMana;
            if (aura.HasAura)
            {
                await new Spells().ThunderIII.Cast(player.CurrentTarget);
                return true;
            }
            return false;
        }
    }

    public class FirestarterFireIII : Rule
    {
        public override async Task<bool> DoAction(LocalPlayer player)
        {
            var firestarter = GetAuraState(player, AuraState.FireStarterId);
            //var af3 = GetAuraState(player, AuraState.AstralFireIIIId);
            var currentMana = player.CurrentMana;
            if (firestarter.HasAura)
            {
                await new Spells().FireIII.Cast(player.CurrentTarget);
                return true;
            }
            return false;
        }
    }

    public class FireIII : Rule
    {
        public override async Task<bool> DoAction(LocalPlayer player)
        {
            var ui3 = GetAuraState(player, AuraState.UmbralIceIIId);
            var af3 = GetAuraState(player, AuraState.AstralFireIIIId);
            var currentMana = player.CurrentMana;
            if ((!af3.HasAura && currentMana > 2000) || (ui3.HasAura && currentMana > player.MaxMana*.95))
            {
                await new Spells().FireIII.Cast(player.CurrentTarget);
                return true;
            }
            return false;
        }
    }

    public class BlizardIII : Rule
    {
        public override async Task<bool> DoAction(LocalPlayer player)
        {
            var enochian = GetAuraState(player, AuraState.EnochianId);
            var af3 = GetAuraState(player, AuraState.AstralFireIIIId);
            var currentMana = player.CurrentMana;
            if (af3.HasAura && currentMana < 2000)
            {
                await new Spells().BlizzardIII.Cast(player.CurrentTarget);
                return true;
            }
            return false;
        }
    }
    public class BlizardIV : Rule
    {
        public override async Task<bool> DoAction(LocalPlayer player)
        {
            var enochian = GetAuraState(player, AuraState.EnochianId);
            var ui3 = GetAuraState(player, AuraState.UmbralIceIIId);
            var currentMana = player.CurrentMana;
            if (ui3.HasAura && currentMana > 2000 && currentMana < player.MaxMana && enochian.HasAura)
            {
                await new Spells().BlizzardIV.Cast(player.CurrentTarget);
                return true;
            }
            return false;
        }
    }*/
}
