using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.GamePlay.Characters.Friendly.Team
{
    static class BasicTeamUtility
    {
        public enum FriendlyParties{None =-1, Party = 0, Neutral1,Neutral2,Neutral3};

        static public List<BaseCharacter>[] parties = new List<BaseCharacter>[Enum.GetNames(typeof(FriendlyParties)).Length];

        static public void Initialize()
        {
            parties[(int)FriendlyParties.Party] = new List<BaseCharacter>();
            parties[(int)FriendlyParties.Neutral1] = new List<BaseCharacter>();
            parties[(int)FriendlyParties.Neutral2] = new List<BaseCharacter>();
            parties[(int)FriendlyParties.Neutral3] = new List<BaseCharacter>();
        }

        static public void Reset()
        {
            foreach (var party in parties)
            {
                party.Clear();
            }
        }
    }
}
