using TBAGW;
using TBAGW.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Storage;
using System;
using System.Collections.Generic;

namespace TBAGW.Utilities.Actions
{
    public class Actions
    {
        public ActionKey[] whatKeysIsActionAssignedTo = new ActionKey[3];

        public string actionIndentifierString;

        public Keys defaultKey;
        public Buttons defaultButton;
        public bool bUsed = false;

        public virtual void Initialize(String actionIndentifierString, List<ActionKey> actionKeysList)
        {
            this.actionIndentifierString = actionIndentifierString;
            for (int i = 0; i < 3; i++)
            {
                if(i!=2){
                    whatKeysIsActionAssignedTo[i] = new ActionKey();
                    whatKeysIsActionAssignedTo[i].Initialize(actionIndentifierString,i);
                    actionKeysList.Add(whatKeysIsActionAssignedTo[i]);
                }else if(i==2){
                    whatKeysIsActionAssignedTo[i] = new ActionKey();
                    whatKeysIsActionAssignedTo[i].Initialize(actionIndentifierString, i);
                    whatKeysIsActionAssignedTo[i].bKeyIsGamePadKey = true;
                    actionKeysList.Add(whatKeysIsActionAssignedTo[i]);
                }
            }
        }

        public void Update(ActionKey key, int column, String actionIndentifierString)
        {
            if (this.actionIndentifierString.Equals(actionIndentifierString))
            {
                whatKeysIsActionAssignedTo[column] = key;
            }
            
        }
    }
}
