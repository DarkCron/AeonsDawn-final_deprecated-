using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBAGW
{
    public class TurnInfo
    {
        int totalMoves = 0;
        public int movesTaken = 0;
        private int maxMoves;
        public bool bEnded = false;

        public TurnInfo(int maxMoves)
        {
            this.maxMoves = maxMoves;
        }
    }
}
