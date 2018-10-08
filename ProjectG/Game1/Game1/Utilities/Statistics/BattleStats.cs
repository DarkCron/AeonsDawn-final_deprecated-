using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Characters;

namespace TBAGW
{

    public static class BattleStats
    {
        static public List<BaseCharacter> partyMembers = new List<BaseCharacter>();
        static public List<int> damageDoneThisFight = new List<int>();
        static public List<int> killingBlowsThisFight = new List<int>();
        static public List<int> healingDoneThisFight = new List<int>();
        static public List<int> critsThisFight = new List<int>();
        static public List<int> missesThisFight = new List<int>();
        static public List<int> damageReceivedThisFight = new List<int>();
        static public List<int> DebuffsAppliedThisFight = new List<int>();

        static public void Start(List<BaseCharacter> heroes)
        {
            partyMembers = heroes;
            damageDoneThisFight.Clear();
            killingBlowsThisFight.Clear();
            healingDoneThisFight.Clear();
            critsThisFight.Clear();
            missesThisFight.Clear();
            damageReceivedThisFight.Clear();
            DebuffsAppliedThisFight.Clear();
            foreach (var item in partyMembers)
            {
                damageDoneThisFight.Add(0);
                killingBlowsThisFight.Add(0);
                healingDoneThisFight.Add(0);
                critsThisFight.Add(0);
                missesThisFight.Add(0);
                damageReceivedThisFight.Add(0);
                DebuffsAppliedThisFight.Add(0);
            }
        }

        static public void AddRange(List<BaseCharacter> heroes)
        {
            partyMembers.AddRange(heroes);

            while (damageDoneThisFight.Count != partyMembers.Count)
            {
                damageDoneThisFight.Add(0);
                killingBlowsThisFight.Add(0);
                healingDoneThisFight.Add(0);
                critsThisFight.Add(0);
                missesThisFight.Add(0);
                damageReceivedThisFight.Add(0);
                DebuffsAppliedThisFight.Add(0);
            }

        }

        static public void AddDamageDoneByHero(BaseCharacter hero, int dmg)
        {
            if (!hero.bIsAI)
            {
                damageDoneThisFight[partyMembers.IndexOf(hero)] += dmg;
            }
          
        }

        static public void AddDamageReceivedByHero(BaseCharacter hero, int dmg)
        {
            if (!hero.bIsAI)
            {
                damageReceivedThisFight[partyMembers.IndexOf(hero)] += dmg;
            }
        }

        static public void ProcessAbilityCastByHero(BaseCharacter hero, BasicAbility ba)
        {
            if (ba.bAbilityHasDebuff&&!hero.bIsAI)
            {
                DebuffsAppliedThisFight[partyMembers.IndexOf(hero)] += 1;
            }
        }

        static public void AddKBByHero(BaseCharacter hero)
        {
            if (!hero.bIsAI)
            {
                killingBlowsThisFight[partyMembers.IndexOf(hero)]++;
            }
        }

        static public void AddMissesByHero(BaseCharacter hero)
        {
            if (!hero.bIsAI)
            {
                missesThisFight[partyMembers.IndexOf(hero)]++;
            }
        }

        static public void AddCritByHero(BaseCharacter hero)
        {
            if (!hero.bIsAI)
            {
                critsThisFight[partyMembers.IndexOf(hero)]++;
            }
        }

        static public void AddHealingDoneByHero(BaseCharacter hero, int healing)
        {
            if (!hero.bIsAI)
            {
                healingDoneThisFight[partyMembers.IndexOf(hero)] += healing;
            }
        }

        static public void Report(BaseCharacter bc)
        {
            Console.WriteLine("REPORT:");
            Console.WriteLine("DMG done: " + damageDoneThisFight[partyMembers.IndexOf(bc)]);
            Console.WriteLine("Killing Blows done: " + killingBlowsThisFight[partyMembers.IndexOf(bc)]);
            Console.WriteLine("Healing done: " + healingDoneThisFight[partyMembers.IndexOf(bc)]);
            Console.WriteLine("Crits done: " + critsThisFight[partyMembers.IndexOf(bc)]);
            Console.WriteLine("Misses done: " + missesThisFight[partyMembers.IndexOf(bc)]);
            Console.WriteLine("DMG received: " + damageReceivedThisFight[partyMembers.IndexOf(bc)]);
            Console.WriteLine("END REPORT");
        }

        static public int CalculateThreatFromBattle(BaseCharacter bs)
        {
            int extraThreat = 0;
            extraThreat += damageDoneThisFight[partyMembers.IndexOf(bs)];
            extraThreat += killingBlowsThisFight[partyMembers.IndexOf(bs)] * 5;
            extraThreat += (int)(healingDoneThisFight[partyMembers.IndexOf(bs)] * 1.5f);
            extraThreat += critsThisFight[partyMembers.IndexOf(bs)] * 3;
            extraThreat -= missesThisFight[partyMembers.IndexOf(bs)] * 2;
            extraThreat += DebuffsAppliedThisFight[partyMembers.IndexOf(bs)] * 3;

            return extraThreat;
        }

        static public int getKDFromBattle(BaseCharacter bs)
        {
            int extraThreat = 0;
            extraThreat += killingBlowsThisFight[partyMembers.IndexOf(bs)];
            return extraThreat;
        }
    }
}
