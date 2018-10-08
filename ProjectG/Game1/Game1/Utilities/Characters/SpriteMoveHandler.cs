using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.AI;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Control.Player;
using TBAGW.Utilities.Sprite;

namespace TBAGW
{
    public class SpriteMoveHandler
    {
        public List<Node> movePath = new List<Node>();
        public BaseSprite movingSprite = null;
        public float speed = 1.2f;
        public int stepTimer = 25;
        public int timeSinceLastStep = 0;
        public bool bIsBusy = false;
        public int currentNodeIndex = 0;
        public Vector2 finalPos = new Vector2();
        int rotationAtEnd = -1;

        bool bStartIgnorer = false;
        int timeSinceLastIgnore = 0;
        int ignoreTimer = 3000;
        bool bShouldIgnoreCollisionPlayer = false;

        public void Start(BaseSprite bs, List<Node> mp, int rotationAtEndOfMovement = -1)
        {
            if (bs != null && mp.Count != 0)
            {
                Node n = new Node();
                n.coord = (bs.position / 64).ToPoint();
                movingSprite = bs;
                movePath = mp;
                movePath.Reverse();
                if (movePath.Find(node => node.coord == n.coord) == null)
                {
                    movePath.Insert(0, n);
                }
                rotationAtEnd = rotationAtEndOfMovement;
                bIsBusy = true;
                currentNodeIndex = 0;
                timeSinceLastStep = 0;
                movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Movement;
                finalPos = movePath[movePath.Count - 1].coord.ToVector2() * 64;
            }

        }

        public void Update(GameTime gt)
        {
            // movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Movement;
            //timeSinceLastStep += gt.ElapsedGameTime.Milliseconds;
            stepTimer = 0;

            if (timeSinceLastStep >= stepTimer && !CombatProcessor.bIsRunning)
            {
                movingSprite.bRecalculateTrueMapSize = true;
                movingSprite.bMustUpdateHitBoxes = true;
                if (CanPerformStep())
                {
                    PerformStep();
                }

                // timeSinceLastStep = 0;
            }
            else if (CombatProcessor.bIsRunning && movingSprite.GetType() == typeof(BaseCharacter))
            {
                movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
            }

            if (bStartIgnorer)
            {
                timeSinceLastIgnore += gt.ElapsedGameTime.Milliseconds;
                if (timeSinceLastIgnore > ignoreTimer)
                {
                    timeSinceLastIgnore = 0;
                    bShouldIgnoreCollisionPlayer = true;
                    bStartIgnorer = false;
                }
            }


        }

        private bool CanPerformStep()
        {
            Point loc = movingSprite.position.ToPoint();
            Point finish = new Point();
            try
            {
                finish = (movePath[currentNodeIndex + 1].coord.ToVector2() * 64).ToPoint();
            }
            catch
            {
                finish = (movePath[currentNodeIndex].coord.ToVector2() * 64).ToPoint();
            }

            int nextDirection = directionNextNode();
            bool bMoveOn = false;
            switch (nextDirection)
            {
                case 0:
                    if (loc.Y <= finish.Y)
                    {
                        bMoveOn = true;
                    }
                    break;
                case 1:
                    if (loc.Y >= finish.Y)
                    {
                        bMoveOn = true;
                    }
                    break;
                case 2:
                    if (loc.X >= finish.X)
                    {
                        bMoveOn = true;
                    }
                    break;
                case 3:
                    if (loc.X <= finish.X)
                    {
                        bMoveOn = true;
                    }
                    break;
            }

            // if (loc != finish)
            if (!bMoveOn)
            {
                if (bShouldIgnoreCollisionPlayer)
                {
                    movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Movement;
                    return true;
                }

                if (PlayerController.selectedSprite != null)
                {
                    if (movingSprite.Contains(PlayerController.selectedSprite.trueMapSize()))
                    {
                        movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
                        bStartIgnorer = true;
                        return false;
                    }
                }


                //foreach (var hBox in movingSprite.closeProximityHitboxes())
                //{
                //    Rectangle temp = hBox;
                //    if (temp.Intersects(PlayerController.selectedSprite.spriteGameSize) || temp.Contains(PlayerController.selectedSprite.spriteGameSize))
                //    {
                //        movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
                //        bStartIgnorer = true;
                //        return false;
                //    }
                //}
                movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Movement;
                bStartIgnorer = false;
                return true;
            }
            else
            {

                if (currentNodeIndex == movePath.Count - 2 || movePath.Count == 1)
                {
                    //End reached
                    return StopPathMovement();
                }
                else
                {
                    currentNodeIndex++;

                    if (bShouldIgnoreCollisionPlayer)
                    {
                        movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Movement;
                        return true;
                    }


                    if (PlayerController.selectedSprite != null)
                    {
                        if (movingSprite.Contains(PlayerController.selectedSprite.trueMapSize()))
                        {
                            movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
                            bStartIgnorer = true;
                            return false;
                        }
                    }

                    //foreach (var hBox in movingSprite.closeProximityHitboxes())
                    //{
                    //    Rectangle temp = hBox;

                    //    if (temp.Intersects(PlayerController.selectedSprite.spriteGameSize) || temp.Contains(PlayerController.selectedSprite.spriteGameSize))
                    //    {
                    //        movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
                    //        bStartIgnorer = true;
                    //        return false;
                    //    }

                    //}

                    movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Movement;
                    bStartIgnorer = false;
                    return true;
                }
            }
        }

        private bool StopPathMovement()
        {
            bIsBusy = false;
            movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
            if (rotationAtEnd != -1)
            {
                movingSprite.rotationIndex = rotationAtEnd;
            }
            movePath.Clear();

            return false;
        }

        public void SkipPathMovement()
        {
            bIsBusy = false;
            movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
            movingSprite.position = finalPos;
        }

        private void PerformStep()
        {
            //0 = Down, 1 is UP, 2 Left, 3 RIGHT

            movingSprite.MoverMustUpdateHitboxes = true;
            switch (directionNextNode())
            {
                case 0:
                    Point tileBefore = (movingSprite.position / 64).ToPoint();
                    Point tileAfter = ((movingSprite.position - new Vector2((speed / 5f))) / 64).ToPoint();
                    //tileBefore.Y -= 1;
                    tileBefore.X -= 1;

                    float tempSpeed = (speed / 5f);
                    if (tileBefore != tileAfter)
                    {
                        tempSpeed = 64 % (speed / 5f);
                    }

                    movingSprite.position.Y -= tempSpeed;
                    movingSprite.rotationIndex = (int)BaseSprite.Rotation.Up;

                    break;
                case 1:
                    tileBefore = (movingSprite.position / 64).ToPoint();
                    tileAfter = ((movingSprite.position + new Vector2((speed / 5f))) / 64).ToPoint();
                    tempSpeed = (speed / 5f);
                    if (tileBefore != tileAfter)
                    {
                        //  tempSpeed = 64 % speed;
                    }
                    //Console.WriteLine(movingSprite.position);
                    movingSprite.position.Y += tempSpeed;
                    movingSprite.rotationIndex = (int)BaseSprite.Rotation.Down;
                    break;
                case 2:
                    tileBefore = (movingSprite.position / 64).ToPoint();
                    tileAfter = ((movingSprite.position + new Vector2((speed / 5f))) / 64).ToPoint();
                    tempSpeed = (speed / 5f);
                    if (tileBefore != tileAfter)
                    {
                        //tempSpeed = 64 % speed;
                    }

                    movingSprite.position.X += tempSpeed;
                    movingSprite.rotationIndex = (int)BaseSprite.Rotation.Right;
                    break;
                case 3:
                    tileBefore = (movingSprite.position / 64).ToPoint();
                    tileAfter = ((movingSprite.position - new Vector2((speed / 5f))) / 64).ToPoint();
                    //tileBefore.X -= 1;
                    tileBefore.Y -= 1;

                    tempSpeed = (speed / 5f);
                    if (tileBefore != tileAfter)
                    {
                        tempSpeed = 64 % (speed / 5f);
                    }

                    movingSprite.position.X -= tempSpeed;
                    movingSprite.rotationIndex = (int)BaseSprite.Rotation.Left;
                    break;
            }

            movingSprite.UpdatePosition();
        }

        private int directionNextNode()
        {
            Node currentNode = movePath[currentNodeIndex];
            Node nextNode = movePath[currentNodeIndex + 1];

            //next node is node above, move up
            if (currentNode.coord == new Point(nextNode.coord.X, nextNode.coord.Y - 1))
            {
                return 1;
            }
            else             //next node is node below, move down
            if (currentNode.coord == new Point(nextNode.coord.X, nextNode.coord.Y + 1))
            {
                return 0;
            }
            else             //next node is node Left, move left
            if (currentNode.coord == new Point(nextNode.coord.X - 1, nextNode.coord.Y))
            {
                return 2;
            }
            else             //next node is node Right, move right
            if (currentNode.coord == new Point(nextNode.coord.X + 1, nextNode.coord.Y))
            {
                return 3;
            }

            return 0;
        }
    }
}
