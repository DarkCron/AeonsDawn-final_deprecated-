using TBAGW.Utilities.Input;
using TBAGW.Utilities.OnScreen.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.GamePlay.Characters.Friendly.Team
{
    static class SelectionUtility
    {
        static public int currentSelectedPartyMember = -1;
        static public BaseCharacter secondSelectedCharacter;
        static public BaseCharacter primarySelectedCharacter;
        static private bool bHoverPrimary = false;
        static private bool bHasClickedPrimary = false;
        static private bool bHoverSecondary = false;
        static private bool bHasClickedSecondary = false;

        static public void Update(GameTime gameTime, List<BaseCharacter> activeObjects)
        {
            bool bTemp = false;
            BaseCharacter tempChar = default(BaseCharacter);
            foreach (BaseCharacter character in activeObjects)
            {
                if (character.Contains(CursorUtility.trueCursorPos))
                {
                    bTemp = true;
                    bHoverPrimary = true;
                    tempChar = character;
                    break;
                }
            }

            if (bTemp == true)
            {
                SecondMemberSelected(activeObjects, tempChar);
                PartyMemberSelected(activeObjects, tempChar);
            }
            else if(!bHasClickedSecondary)
            {
                bHoverPrimary = false;
                bHoverSecondary = false;
                secondSelectedCharacter = default(BaseCharacter);
            }else if (bHasClickedSecondary)
            {
                bHoverPrimary = false;
                bHoverSecondary = false;
            }

            //   Console.Out.WriteLine(secondSelectedCharacter == default(BaseCharacter));
            if (!bHoverPrimary && !bHasClickedPrimary)
            {
                bHasClickedPrimary = false;
                SelectionUtility.currentSelectedPartyMember = -1;
                currentSelectedPartyMember = -1;
                secondSelectedCharacter = default(BaseCharacter);
                primarySelectedCharacter = default(BaseCharacter);
            }

            if (buttonPressUtility.isPressed(Game1.cancelString))
            {
                if (currentSelectedPartyMember != -1)
                {
                    bHasClickedPrimary = false;
                    bHasClickedSecondary = false;
                    SelectionUtility.currentSelectedPartyMember = -1;
                    currentSelectedPartyMember = -1;
                    secondSelectedCharacter = default(BaseCharacter);
                    primarySelectedCharacter = default(BaseCharacter);
                }
            }else if (buttonPressUtility.isMousePressed(Mouse.GetState().RightButton))
            {
                if (bHasClickedPrimary && bHasClickedSecondary)
                {
                    bHasClickedSecondary = false;
                } else if (bHasClickedPrimary&&!bHasClickedSecondary)
                {
                    bHasClickedPrimary = false;
                    bHasClickedSecondary = false;
                    SelectionUtility.currentSelectedPartyMember = -1;
                    currentSelectedPartyMember = -1;
                    secondSelectedCharacter = default(BaseCharacter);
                    primarySelectedCharacter = default(BaseCharacter);
                }
                 
            }
        }

        private static void PartyMemberSelected(List<BaseCharacter> activeObjects, BaseCharacter selection)
        {
            if (buttonPressUtility.isMousePressed(Mouse.GetState().LeftButton)&&!bHasClickedPrimary)
            {
                //Can only select heroes
                if (typeof(BasicHero) == selection.GetType())
                {
                    if ((selection as BasicHero).bInitializedAssets)
                    {
                        int temp = 0;
                        foreach (var item in BasicTeamUtility.parties[(int)BasicTeamUtility.FriendlyParties.Party])
                        {
                            if (item.Equals(selection))
                            {
                                bHasClickedPrimary = true;
                                currentSelectedPartyMember = temp;
                                primarySelectedCharacter = selection;
                                break;
                            }

                            temp++;
                        }
                    }
                }
            }
            else //Handles hover while not clicking down below
            {
                if (selection.bInitializedAssets)
                {
                    int temp = 0;
                    foreach (var item in activeObjects)
                    {
                        if (item.Equals(selection) && currentSelectedPartyMember == -1)
                        {
                            currentSelectedPartyMember = temp;
                            break;
                        }
                        temp++;
                    }
                }
            }
        }

        private static void SecondMemberSelected(List<BaseCharacter> activeObjects, BaseCharacter selection)
        {
            if (bHasClickedPrimary)
            {
                if (selection.bInitializedAssets && buttonPressUtility.isMousePressed(Mouse.GetState().LeftButton))
                {
                    bHasClickedSecondary = true;
                    if (currentSelectedPartyMember != -1)
                    {

                    }
                }

                if (true&&!bHasClickedSecondary)
                {
                    secondSelectedCharacter = selection;
                }
            }
        }

        public static bool HasMemberSelected()
        {
            if (currentSelectedPartyMember!=-1&&bHasClickedPrimary)
            {
                return true;
            }
            return false;
        }
    }
}
