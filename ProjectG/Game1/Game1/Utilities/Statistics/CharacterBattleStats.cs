using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TBAGW.Utilities.Characters;

namespace TBAGW
{
    [XmlRoot("Character Battle Stats")]
    public class CharacterBattleStats
    {
        [XmlElement("Total Damage Done")]
        int damageDoneThisFight = 0;
        [XmlElement("Total Killing Blows Done")]
        int killingBlowsThisFight = 0;
        [XmlElement("Total Healing Done")]
        int healingDoneThisFight = 0;
        int critsThisFight = 0;
        [XmlElement("Total Misses")]
        int missesThisFight = 0;
        [XmlElement("Total Damage Received")]
        int damageReceivedThisFight = 0;

        public CharacterBattleStats()
        {

        }

        public void FinalizeBattleStats(BaseCharacter bc)
        {
            damageDoneThisFight += BattleStats.damageDoneThisFight[BattleStats.partyMembers.IndexOf(bc)];
            killingBlowsThisFight += BattleStats.killingBlowsThisFight[BattleStats.partyMembers.IndexOf(bc)];
            healingDoneThisFight += BattleStats.healingDoneThisFight[BattleStats.partyMembers.IndexOf(bc)];
            critsThisFight += BattleStats.critsThisFight[BattleStats.partyMembers.IndexOf(bc)];
            missesThisFight += BattleStats.missesThisFight[BattleStats.partyMembers.IndexOf(bc)];
            damageReceivedThisFight += BattleStats.damageReceivedThisFight[BattleStats.partyMembers.IndexOf(bc)];
        }
    }
}
