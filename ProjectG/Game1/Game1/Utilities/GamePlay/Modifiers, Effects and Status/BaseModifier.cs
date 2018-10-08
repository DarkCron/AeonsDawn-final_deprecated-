using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    [XmlRoot("Modifier")]
    public class BaseModifier
    {

        [XmlElement("Modifier name")]
        public String modifierName = "Modifier name";
        [XmlElement("Stat modifier")]
        public STATChart statModifier = new STATChart(true);
        [XmlElement("Stat modifierLength")]
        public int abilityModifierLength = 1;
        [XmlElement("Threat modifier")]
        public int abilityThreatModifier = 0;

        [XmlIgnore]
        public int turnsPassedSinceModifier = 0;
        internal MindControlInfo mci = null;

        public BaseModifier()
        {

        }

        public void ModifierTick()
        {
            turnsPassedSinceModifier++;
        }

        public bool ModifierIsOver()
        {
            if (turnsPassedSinceModifier >= abilityModifierLength)
            {
                EndModifier();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void EndModifier()
        {
            if (mci != null)
            {
                LUA.LuaTurnSetInfo ogFrom = mci.ogFrom.toLuaTurnSetInfo();
                LUA.LuaTurnSetInfo currentlyIn = mci.currentlyIn.toLuaTurnSetInfo();
                LUA.LuaCharacterTurnInfo lcti = mci.ct.toLuaCharacterTurnInfo();

                ogFrom.HandleChangeSide(lcti, currentlyIn, ogFrom, LUA.LuaTurnSetInfo.SideTurnType.Normal);
                mci = null;
            }
        }

        public BaseModifier Clone()
        {
            var temp = (BaseModifier)this.MemberwiseClone();
            temp.statModifier = this.statModifier.Clone();
            return temp;
        }

        internal int damageByModifier()
        {
            return statModifier.currentActiveStats[(int)STATChart.ACTIVESTATS.HP];
        }

    }

    internal class MindControlInfo
    {
        internal TurnSet ogFrom;
        internal TurnSet currentlyIn;
        internal CharacterTurn ct;

        internal MindControlInfo(CharacterTurn ct, TurnSet ogFrom, TurnSet currentlyIn)
        {
            this.ct = ct;
            this.ogFrom = ogFrom;
            this.currentlyIn = currentlyIn;
        }
    }
}
