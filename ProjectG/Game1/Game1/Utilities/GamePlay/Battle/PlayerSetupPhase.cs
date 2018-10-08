using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.AI;
using TBAGW.Utilities;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Control.Player;

using TBAGW.Utilities.Sprite;

namespace TBAGW
{
    static public class PlayerSetupPhase
    {
        public static List<BasicTile> availableTiles = new List<BasicTile>();
        public static BaseCharacter selectedChar = null;
        public static BaseCharacter secondaryChar = null;


        public static void Start(List<BasicTile> ats)
        {
            availableTiles = ats;
            selectedChar = null;
            secondaryChar = null;
            foreach (var item in CombatProcessor.encounterEnemies)
            {
                ats.RemoveAll(t => t.mapPosition.Intersects(item.spriteGameSize) || t.mapPosition.Contains(item.spriteGameSize) || t.mapPosition == item.spriteGameSize);
            }
            BasicTile temp = new BasicTile();
            temp.mapPosition = PlayerController.selectedSprite.spriteGameSize;
            ats.Add(temp);
        }

        public static void Update()
        {
            CombatCtrl.selectedChar = selectedChar;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
            {
                selectedChar = CombatProcessor.heroCharacters.Find(c => c.spriteGameSize.Contains(GameProcessor.EditorCursorPos));

                secondaryChar = null;
            }

            if (selectedChar != null)
            {
                if (Mouse.GetState().RightButton == ButtonState.Pressed && !KeyboardMouseUtility.AnyButtonsPressed())
                {
                    secondaryChar = CombatProcessor.heroCharacters.Find(c => c.spriteGameSize.Contains(GameProcessor.EditorCursorPos));

                    Vector2 temp = new Vector2(-1);
                    if (availableTiles.Find(t => t.mapPosition.Contains(GameProcessor.EditorCursorPos)) != null)
                    {
                        temp = (GameProcessor.EditorCursorPos / 64).ToPoint().ToVector2() * 64;
                    }

                    if (secondaryChar != null)
                    {
                        temp = secondaryChar.position;
                    }

                    if (temp != new Vector2(-1))
                    {
                        /*
                            if (secondaryChar != null)
                            {
                                secondaryChar.position = selectedChar.position;
                            }

                            selectedChar.position = temp;*/

                        if (CombatProcessor.zone.Contains(GameProcessor.EditorCursorPos) && !PathMoveHandler.bIsBusy)
                        {
                            //   allPossibleNodes = PathFinder.NewPathSearch(PlayerController.selectedSprite.position, GameProcessor.EditorCursorPos, zoneTiles);
                            CombatProcessor.allPossibleNodes = PathFinder.NewPathSearch(selectedChar.position, GameProcessor.EditorCursorPos, CombatProcessor.radiusTiles);
                            PathMoveHandler.Start(selectedChar, CombatProcessor.allPossibleNodes);
                        }
                        else if (CombatProcessor.zone.Contains(GameProcessor.EditorCursorPos) && PathMoveHandler.bIsBusy)
                        {
                            PathMoveHandler.SkipPathMovement();
                            CombatProcessor.allPossibleNodes = PathFinder.NewPathSearch(selectedChar.position, GameProcessor.EditorCursorPos, CombatProcessor.radiusTiles);
                            PathMoveHandler.Start(selectedChar, CombatProcessor.allPossibleNodes);
                        }
                    }
                }
            }
        }

        public static void Draw(SpriteBatch sb)
        {

            BattleGUI.DrawBlueTiles(sb, availableTiles);
            /*
            foreach (var item in availableTiles)
            {
                sb.Draw(Game1.hitboxHelp, item.mapPosition, Color.LightBlue);
            }*/

            if (availableTiles.Find(t => t.mapPosition.Contains(GameProcessor.EditorCursorPos)) != null)
            {
                Vector2 temp = (GameProcessor.EditorCursorPos / 64).ToPoint().ToVector2() * 64;
                if (selectedChar != null)
                {
                    selectedChar.Draw(sb, temp, (int)BaseSprite.Rotation.Down, Color.White * .4f);
                }
            }

        }

        internal static void End()
        {
            PathMoveHandler.bIsBusy = false;
            PathMoveHandler.movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
        }
    }
}
