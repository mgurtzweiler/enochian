using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Enochian
{
    public class BlackMage
    {
        public async Task<bool> Combat(GameState state)
        {
            var spells = new Spells();
            Logging.Write(Colors.HotPink, "[Enochian] Current Mana {0}", state.CurrentMana);
            if (MovementManager.IsMoving)
                return false;
            if (state.CurrentMana < 2200)
            {
                await spells.BlizzardIII.Cast(Core.Player.CurrentTarget);
                return true;
            }
            await spells.Fire.Cast(Core.Player.CurrentTarget);
            return true;
        }
    }
}
