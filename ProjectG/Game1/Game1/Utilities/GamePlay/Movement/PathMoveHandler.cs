using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.AI;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Sprite;

namespace TBAGW
{
    public static class PathMoveHandler
    {
        static public List<Node> movePath = new List<Node>();
        static public BaseSprite movingSprite = new BaseSprite();
        static public float speed = 3;
        static public int stepTimer = 10;
        static public int timeSinceLastStep = 0;
        static public bool bIsBusy = false;
        static public int currentNodeIndex = 0;
        static public Vector2 finalPos = new Vector2();
        static int rotationAtEnd = -1;
       

        static public void Start(BaseSprite bs, List<Node> mp, int rotationAtEndOfMovement = -1)
        {
            if (bs != null && mp.Count != 0)
            {
                Node n = new Node();
                n.coord = (bs.position/64).ToPoint();
                movingSprite = bs;
                movePath = mp;
               // movePath.Reverse();
                if(movePath.Find(node=>node.coord==n.coord)==null) {
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

        static public void Update(GameTime gt)
        {
            stepTimer = 0;
            speed = 8*SettingsFile.speedMod;
            if (timeSinceLastStep >= stepTimer)
            {
                movingSprite.bRecalculateTrueMapSize = true;
               // movingSprite.bMustUpdateHitBoxes = true;
                if (CanPerformStep())
                {
                    PerformStep();
                }

                // timeSinceLastStep = 0;
            }
        }

        private static bool CanPerformStep()
        {        
            if(movePath.Count==0)
            {
                return false;
            }
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




            if (!bMoveOn)
            {
                movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Movement;
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

                    movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Movement;
                    return true;
                }
            }



            //if (loc != finish)
            //{
            //    return true;
            //}
            //else {

            //    if (currentNodeIndex == movePath.Count - 2||movePath.Count==1)
            //    {
            //        //End reached
            //        return StopPathMovement();
            //    }
            //    else {
            //        currentNodeIndex++;
            //        return true;
            //    }
            //}
        }

        private static bool StopPathMovement()
        {
            bIsBusy = false;
            movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
            if(rotationAtEnd!=-1)
            {
                movingSprite.rotationIndex = rotationAtEnd;

            }
            movingSprite.position = finalPos;
            movePath.Clear();
            movingSprite.UpdatePosition();
            return false;
        }

        public static void SkipPathMovement()
        {
            bIsBusy = false;
            movingSprite.animationIndex = (int)BaseCharacter.CharacterAnimations.Idle;
            movingSprite.position = finalPos;
            movingSprite.UpdatePosition();
        }

        private static void PerformStep()
        {
            //0 = Down, 1 is UP, 2 Left, 3 RIGHT

            //movingSprite.MoverMustUpdateHitboxes = true;
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
                        //tempSpeed = 64 % (speed / 5f);
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
                      //  tempSpeed = 64 % (speed / 5f);
                    }

                    movingSprite.position.X -= tempSpeed;
                    movingSprite.rotationIndex = (int)BaseSprite.Rotation.Left;
                    break;
            }

            movingSprite.UpdatePosition();
        }

        private static int directionNextNode()
        {
            if(movePath.Count<=1)
            {
                StopPathMovement();
                return -1;
            }
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
