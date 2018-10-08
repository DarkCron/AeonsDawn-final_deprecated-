using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBAGW
{
    public class EncounterObjective
    {
        /// <summary>
        /// Skirmish: normal battle
        /// SafeGuard: keep a (cpu controlled)-character alive
        /// Boss: KO a certain enemy
        /// Run: Reach a certain zone to win
        /// </summary>
        public enum objectiveType { Skirmish = 0, SafeGuard, Boss, Run }

        public int objective = (int)objectiveType.Skirmish;

        public bool ObjectiveReached(List<TurnSet> encounterGroups)
        {
            switch (objective)
            {
                case (int)objectiveType.Skirmish:
                    return SkirmishObjectiveReached(encounterGroups);
            }


            return false;
        }

        private bool SkirmishObjectiveReached(List<TurnSet> encounterGroups)
        {
            foreach (var eg in encounterGroups)
            {
                if (eg.bIsEnemyTurnSet)
                {
                    var bc = eg.charactersInGroup.Find(c => c.IsAlive());
                    if (bc != null || CombatProcessor.encounterEnemies.Count != 0)
                    {
                        return false;
                    }
                }

            }

            return true;
        }

        internal static bool Lost(EncounterObjective objective)
        {
            if (PlayerSaveData.heroParty.FindAll(h => h.IsAlive()).Count == 0)
            {
                return true;
            }
            return false;
        }
    }
}
