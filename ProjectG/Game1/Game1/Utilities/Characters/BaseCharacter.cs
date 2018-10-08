using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TBAGW;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.Control.Player;
using TBAGW.Utilities.Sprite;
using TBAGW.Utilities.SriptProcessing;
using static TBAGW.AIBehaviour;

namespace TBAGW.Utilities.Characters
{
    [XmlRoot("BaseCharacter")]
    public class BaseCharacter : BaseSprite
    {
        public enum CharacterAnimations { Idle = 0, Movement, Melee, Ranged, Magic, Hurt, Death, Disable }
        public enum CharacterBattleAnimations { Idle = 0, Attack, Hurt, Jump_Brace, Counter_Attack, Jump_MidAir, Jump_Landing, Death, Death_State, Shield_Block }
        public enum PortraitExpressions { Neutral = 0, Angry, Laughing, Suprised }

        [XmlElement("CHAR STATS")]
        public STATChart statChart = new STATChart(true);
        [XmlArray("Animations")]
        public List<ShapeAnimation> charAnimations = new List<ShapeAnimation>();
        [XmlElement("Character Name")]
        public String CharacterName = "TOM NOT NEO";
        [XmlElement("Display Name")]
        public String displayName = "TOM NOT NEO";
        //[XmlArrayItem("Abilities")]
        //public List<BasicAbility> abilityList = new List<BasicAbility>();
        [XmlArrayItem("Traits")]
        public List<BaseTrait> traitList = new List<BaseTrait>();
        [XmlArrayItem("Modifiers")]
        public List<BaseModifier> modifierList = new List<BaseModifier>();
        [XmlArrayItem("Effects")]
        public List<BaseEffect> effectList = new List<BaseEffect>();
        [XmlArrayItem("Status")]
        public List<BasicStatus> statusList = new List<BasicStatus>();
        //[XmlElement("Class")]
        //public BaseClass equippedClass = new BaseClass(true);
        [XmlElement("Weapon")]
        public int weaponID = -1;
        [XmlElement("Armour")]
        public int armourID = -1;
        [XmlArray("Battle Animations")]
        public List<ShapeAnimation> charBattleAnimations = new List<ShapeAnimation>();
        [XmlArrayItem("Battle Animation locations")]
        public List<Vector2> battleAnimLocs = new List<Vector2>();
        [XmlElement("Battle Animation Shadow locations")]
        public Vector2 battleAnimShadowAdjustLocs = new Vector2();

        [XmlElement("Battle Animation scale")]
        public float battleAnimScale = 1;
        [XmlElement("Battle Animation jump speed")]
        public int jumpSpeed = 5;
        /// <summary>
        /// This is the location for the enemy to end, not the current character's actual goal.
        /// </summary>
        [XmlElement("Battle Jump target")]
        public Vector2 jumpEnd = Vector2.Zero;
        /// <summary>
        /// When this character attacks, when to connect the "hurt" animation of the receiving character.
        /// </summary>
        [XmlElement("Battle Attack timer")]
        public int bAttackTimer = 500;
        /// <summary>
        /// How long should this particular character stay in the "hurt" animation.
        /// </summary>
        [XmlElement("Battle Stun timer")]
        public int bStunTimer = 500;
        [XmlElement("Battle Statistics")]
        public CharacterBattleStats charStatistics = new CharacterBattleStats();
        [XmlElement("Accumulated Level")]
        public int level = 1;
        [XmlElement("AI Type")]
        public AIBehaviourType Behaviour = AIBehaviourType.Neutral;
        [XmlElement("Active Stat Modifier Active")]
        public List<ActiveStatModifier> activeStatModifiers = new List<ActiveStatModifier>();
        [XmlElement("Highest Character Index")]
        public static int characterIDLatest = 0;
        [XmlElement("Portrait Expressions")]
        public List<ShapeAnimation> portraitAnimations = new List<ShapeAnimation>();

        [XmlElement("Enemy weapon selection")]
        public List<int> enemyWeaponArray = new List<int>();
        [XmlElement("Enemy armour selection")]
        public List<int> enemyArmourArray = new List<int>();
        [XmlElement("Enemy counter type")]
        public BaseClass.CLASSType counterableClassType = BaseClass.CLASSType.MELEE;
        [XmlElement("Enemy counter range")]
        public int counterRange = 1;
        [XmlElement("CCC identifier")]
        public int CCCidentifier = 0;

        [XmlElement("Expression info")]
        public List<CharacterExpressionInfo> dialogueExpressions = new List<CharacterExpressionInfo>();

        [XmlElement("Loot table")]
        public LootList lootList = new LootList();

        [XmlElement("Climax Attack frame")]
        public int climaxAttackFrame = 3;
        [XmlElement("Climax Hurt frame")]
        public int climaxHurtFrame = 3;

        [XmlElement("Lua info file")]
        public String luaLoc = "";
        internal NLua.Lua luaState = null;

        [XmlElement("Additional Combat info")]
        public CharacterCombatInfo combatInfo = new CharacterCombatInfo();

        [XmlIgnore]
        public EnemyAIInfo eai = null;
        [XmlIgnore]
        public bool bIsAI = false;
        [XmlIgnore]
        public CharacterClassCollection CCC = new CharacterClassCollection();
        [XmlIgnore]
        public bool bSaveAP = true;
        [XmlIgnore]
        public bool bAIExecuteDefend = false;

        //   [XmlIgnore]
        //public int animationIndex = (int)CharacterAnimations.Idle;
        [XmlIgnore]
        public BaseEquipment weapon = null;
        [XmlIgnore]
        public STATChart latestStatChart = null;
        [XmlIgnore]
        public BaseEquipment armour = null;
        [XmlIgnore]
        public int animationBattleIndex = (int)CharacterBattleAnimations.Idle;
        [XmlIgnore]
        public bool bInBattle = false;
        [XmlIgnore]
        public bool bJumpAttack = false;
        [XmlIgnore]
        public bool bJumpBack = true;
        [XmlIgnore]
        public List<int> battleAnimationTaskList = new List<int>();
        [XmlIgnore]
        public bool bIsAlive = true;
        [XmlIgnore]
        public List<BasicAbility> possibleAbilities = new List<BasicAbility>();
        [XmlIgnore]
        public List<BasicTile> maxRadius = new List<BasicTile>();
        [XmlIgnore]
        public SFXHelp lastPlayedExpressionSFX;
        [XmlIgnore]
        public bool attackedAsAI = false;

        public bool HasProperLua()
        {
            bool temp = false;

            if (!luaLoc.Equals(""))
            {
                try
                {
                    luaState = new NLua.Lua();
                    luaState.LoadCLRPackage();
                    luaState.DoFile(Game1.rootContent + luaLoc);
                    return true;
                }
                catch (Exception)
                {

                }
            }

            return temp;
        }

        public override void ReloadTextures()
        {
            foreach (var anim in charAnimations)
            {
                anim.ReloadTexture();
                anim.bc = this;
            }

            foreach (var anim in charBattleAnimations)
            {
                anim.ReloadTexture();
                anim.bc = this;
            }

            foreach (var item in portraitAnimations)
            {
                try
                {
                    item.ReloadTexture();
                    item.bc = this;
                }
                catch
                {
                }
            }

        }

        public void ReloadChangesInDataBase(GameContentDataBase gcdb)
        {
            charAnimations = new List<ShapeAnimation>(gcdb.gameCharacters.Find(c => c.shapeID == shapeID).charAnimations);
            portraitAnimations = new List<ShapeAnimation>(gcdb.gameCharacters.Find(c => c.shapeID == shapeID).portraitAnimations);
        }

        public void ReloadFromDatabase(GameContentDataBase gcdb)
        {
            try
            {
                weapon = gcdb.gameItems.Find(w => w.itemID == weaponID) as BaseEquipment;
            }
            catch
            {
            }

            try
            {
                armour = gcdb.gameItems.Find(a => a.itemID == armourID) as BaseEquipment;
            }
            catch
            {
            }

            try
            {
                if (gcdb.gameCCCs.Find(gccc => gccc.identifier == CCCidentifier) == default(CharacterClassCollection))
                {
                    CCC = new CharacterClassCollection();

                }
                else
                {
                    CCC = gcdb.gameCCCs.Find(ccc => ccc.identifier == CCCidentifier);
                }
            }
            catch
            {
            }

            CCC.parent = this;

            while (dialogueExpressions.Count != Enum.GetNames(typeof(PortraitExpressions)).Length)
            {
                dialogueExpressions.Add(new CharacterExpressionInfo((PortraitExpressions)dialogueExpressions.Count));
            }

            foreach (var item in dialogueExpressions)
            {
                try
                {
                    item.Reload(gcdb);
                }
                catch (Exception)
                {

                }
            }

            try
            {
                HasProperLua();
            }
            catch (Exception e)
            {
                if (Game1.bIsDebug)
                {
                    throw;
                }
                else
                {
                    Console.WriteLine("Error reloading lua for " + this.ToString() + "\n" + e.Message);
                }
            }

            lootList.ReloadGCDB(gcdb);
            combatInfo.parent = this;
        }

        public SoundEffectInstance PlayRandomConvExpression(PortraitExpressions expression)
        {
            SoundEffectInstance temp = null;
            var tempSFXInfo = dialogueExpressions.Find(de => de.expression == expression).returnCorrespondingFX();
            var tempSFX = tempSFXInfo == null ? null : tempSFXInfo.sfx;
            if (tempSFX != null)
            {
                temp = tempSFX.CreateInstance();
                temp.Volume = 0.06f;
                temp.Play();
            }

            return temp;
        }

        public void ToActivateDialogueSFX()
        {
            if (lastPlayedExpressionSFX != null)
            {
                if (SFXProcessor.PlayedSFXBefore(lastPlayedExpressionSFX))
                {
                    lastPlayedExpressionSFX.sfxI.Stop();
                    lastPlayedExpressionSFX.sfxI.Play();
                }
            }
            else
            {
                lastPlayedExpressionSFX = SFXProcessor.GenerateAndIDSFX(GameProcessor.gcDB.gameSFXs[0].sfx);
            }
        }

        public BaseCharacter() : base()
        {
        }

        public BaseCharacter(Texture2D shapeTexture, Texture2D hitboxTexture, Rectangle spriteGameSize, Rectangle hitBoxTexBox, Rectangle rectangleToDraw, int scale, Vector2 position, Vector2 center = default(Vector2), String shapeName = "")
            : base(shapeTexture, hitboxTexture, spriteGameSize, hitBoxTexBox, rectangleToDraw, scale, position, center = default(Vector2), shapeName = "")
        {
            this.shapeTexture = shapeTexture;
            textureLoc = shapeTexture.Name.Replace(rootContent, "");
            this.hitBoxTexture = hitboxTexture;
            hitboxTextureLoc = hitboxTexture.Name.Replace(rootContent, "");
            this.hitBoxTexBox = hitBoxTexBox;
            this.spriteGameSize = spriteGameSize;

            //  this.scale = scale;
            this.position = position;
            this.bHasCollision = true;
            this.shapeName = shapeName;

            this.shapeTextureBounds = shapeTexture.Bounds;
            this.rectangleToDraw = rectangleToDraw;

            foreach (var item in Enum.GetNames(typeof(Rotation)))
            {
                shapeHitBox.Add(new List<Rectangle>());
            }

            spriteGameSize.X = (int)position.X;
            spriteGameSize.Y = (int)position.Y;
            if (charAnimations[(int)CharacterAnimations.Idle].bDirectional)
            {
                shapeHitBox[(int)BaseSprite.Rotation.Down] = GenerateHitBoxesFromAnyTex(charAnimations[(int)CharacterAnimations.Idle].animationFramesDirectional[(int)BaseSprite.Rotation.Down][0], charAnimations[(int)CharacterAnimations.Idle].animationTexture);
                shapeHitBox[(int)BaseSprite.Rotation.Up] = GenerateHitBoxesFromAnyTex(charAnimations[(int)CharacterAnimations.Idle].animationFramesDirectional[(int)BaseSprite.Rotation.Up][0], charAnimations[(int)CharacterAnimations.Idle].animationTexture);
                shapeHitBox[(int)BaseSprite.Rotation.Left] = GenerateHitBoxesFromAnyTex(charAnimations[(int)CharacterAnimations.Idle].animationFramesDirectional[(int)BaseSprite.Rotation.Left][0], charAnimations[(int)CharacterAnimations.Idle].animationTexture);
                shapeHitBox[(int)BaseSprite.Rotation.Right] = GenerateHitBoxesFromAnyTex(charAnimations[(int)CharacterAnimations.Idle].animationFramesDirectional[(int)BaseSprite.Rotation.Right][0], charAnimations[(int)CharacterAnimations.Idle].animationTexture);

            }

        }

        public BaseCharacter(String charName = "TOM NOT NEO")
        {
            CharacterName = charName;

            foreach (var item in Enum.GetNames(typeof(CharacterAnimations)))
            {
                charAnimations.Add(new ShapeAnimation());
            }

            foreach (var item in Enum.GetNames(typeof(CharacterBattleAnimations)))
            {
                charBattleAnimations.Add(new ShapeAnimation());
            }

            charAnimations[(int)CharacterAnimations.Idle].ToggleDirectionalAnim();


            if (charAnimations[(int)CharacterAnimations.Idle].bDirectional)
            {
                shapeHitBox[(int)BaseSprite.Rotation.Down] = GenerateHitBoxesFromAnyTex(charAnimations[(int)CharacterAnimations.Idle].animationFramesDirectional[(int)BaseSprite.Rotation.Down][0], charAnimations[(int)CharacterAnimations.Idle].animationTexture);
                shapeHitBox[(int)BaseSprite.Rotation.Up] = GenerateHitBoxesFromAnyTex(charAnimations[(int)CharacterAnimations.Idle].animationFramesDirectional[(int)BaseSprite.Rotation.Up][0], charAnimations[(int)CharacterAnimations.Idle].animationTexture);
                shapeHitBox[(int)BaseSprite.Rotation.Left] = GenerateHitBoxesFromAnyTex(charAnimations[(int)CharacterAnimations.Idle].animationFramesDirectional[(int)BaseSprite.Rotation.Left][0], charAnimations[(int)CharacterAnimations.Idle].animationTexture);
                shapeHitBox[(int)BaseSprite.Rotation.Right] = GenerateHitBoxesFromAnyTex(charAnimations[(int)CharacterAnimations.Idle].animationFramesDirectional[(int)BaseSprite.Rotation.Right][0], charAnimations[(int)CharacterAnimations.Idle].animationTexture);
            }
        }

        public virtual void SaveCharacter()
        {
            if (CheckIfAnimationsComplete())
            {
                if (charAnimations[(int)CharacterAnimations.Idle].bDirectional)
                {
                    shapeHitBox.Clear();
                    foreach (var rotation in Enum.GetNames(typeof(BaseSprite.Rotation)))
                    {
                        shapeHitBox.Add(new List<Rectangle>());
                    }
                    shapeHitBox[(int)BaseSprite.Rotation.Down] = GenerateHitBoxesFromAnyTex(charAnimations[(int)CharacterAnimations.Idle].animationFramesDirectional[(int)BaseSprite.Rotation.Down][0], charAnimations[(int)CharacterAnimations.Idle].animationTexture);
                    shapeHitBox[(int)BaseSprite.Rotation.Up] = GenerateHitBoxesFromAnyTex(charAnimations[(int)CharacterAnimations.Idle].animationFramesDirectional[(int)BaseSprite.Rotation.Up][0], charAnimations[(int)CharacterAnimations.Idle].animationTexture);
                    shapeHitBox[(int)BaseSprite.Rotation.Left] = GenerateHitBoxesFromAnyTex(charAnimations[(int)CharacterAnimations.Idle].animationFramesDirectional[(int)BaseSprite.Rotation.Left][0], charAnimations[(int)CharacterAnimations.Idle].animationTexture);
                    shapeHitBox[(int)BaseSprite.Rotation.Right] = GenerateHitBoxesFromAnyTex(charAnimations[(int)CharacterAnimations.Idle].animationFramesDirectional[(int)BaseSprite.Rotation.Right][0], charAnimations[(int)CharacterAnimations.Idle].animationTexture);
                }

                System.Windows.Forms.MessageBox.Show("Successfully saved character: " + CharacterName);
            }
        }

        // [XmlIgnore]
        // public KeyValuePair<Vector2, int> lastFailedTest = new KeyValuePair<Vector2, int>(new Vector2(-3.14f, -3.14f), (int)Rotation.Up);

        public override void Move(Vector2 movement)
        {
            //GameProcessor.bUpdateShadows = true;
            //position += movement;
            // position = new Vector2(128,128);
            // && ((position == lastFailedTest.Key && rotationIndex == lastFailedTest.Value) == false)
            if (GameProcessor.bIsInGame && movement != Vector2.Zero)
            {

                int futurXTile = (int)(position.X + 32 + movement.X) / 64;
                int futurYTile = (int)(position.Y + 48 + movement.Y) / 64;
                Vector2 futurePositionGrid = new Vector2(futurXTile, futurYTile);

                bool bCollides = false;
                Rectangle collisionProximiter = spriteGameSize;
                collisionProximiter.Location += movement.ToPoint();



                //Vector2 facingVector = collisionProximiter.Location.ToVector2() / 64;
                //switch (rotationIndex)
                //{
                //    case (int)Rotation.Up:
                //        break;
                //    case (int)Rotation.Down:
                //        facingVector.Y = facingVector.Y + 1;
                //        break;
                //    case (int)Rotation.Left:
                //        facingVector.X = facingVector.X;
                //        break;
                //    case (int)Rotation.Right:
                //        facingVector.X = facingVector.X + 1;
                //        break;
                //}


                Vector2 movementProximity = Vector2.Zero;
                if (movement.X > 0)
                {
                    movementProximity.X = (int)Math.Ceiling((double)movement.X);
                }
                else if (movement.X < 0)
                {
                    movementProximity.X = (int)Math.Floor((double)movement.X);
                }

                if (movement.Y > 0)
                {
                    movementProximity.Y = (int)Math.Ceiling((double)movement.Y);
                }
                else if (movement.Y < 0)
                {
                    movementProximity.Y = (int)Math.Floor((double)movement.Y);
                }


                var tileList = (GameProcessor.loadedMap.canMoveToThisTile(movementProximity.ToPoint(), this));
                foreach (var item in tileList)
                {
                    if (!item.mapPosition.Contains(item.hitboxes[0]) || !item.mapPosition.Contains(item.hitboxes[0]))
                    {
                        item.reloadMapPosition();
                    }
                }

                if (!(GameProcessor.loadedMap.isWithinMap(this, movementProximity)))
                {
                    bCollides = true;
                    goto collisionFound;
                }

                //var spritesInCamera = GameProcessor.loadedMap.returnAllObjectsInCamera(new Rectangle((int)-GameProcessor.sceneCamera.X, (int)-GameProcessor.sceneCamera.Y, 1366, 768), GameProcessor.zoom).FindAll(sprite => sprite.bHasCollision && (sprite.spriteGameSize.Intersects(this.spriteGameSize) || sprite.spriteGameSize.Contains(this.spriteGameSize)) && sprite != this);

                Rectangle r = new Rectangle(trueMapSize().Location, trueMapSize().Size);
                //r.Inflate(speed * 3, speed * 3);
                r = new Rectangle((int)(r.X - speed * 5), (int)(r.Y - speed * 5), (int)(r.Width + 5 * speed * 2), (int)(r.Height + 5 * speed * 2));
                var spritesNearby = Map.MapChunk.consideredSprites.FindAll(oi => oi.bHasCollision && oi.obj != this && (oi.mapSize.Contains(r) || oi.mapSize.Intersects(r)));
                List<Rectangle> boxesToConsider = new List<Rectangle>();
                foreach (var item in spritesNearby)
                {
                    boxesToConsider.AddRange(item.hitBoxes());
                }
                //(spritesNearby[9].obj as BaseSprite).shapeHitBox.Clear();
                //(spritesNearby[9].obj as BaseSprite).shapeHitBox.Add(new List<Rectangle>{ new Rectangle(0,0,64,64) });
                foreach (var item in tileList)
                {
                    foreach (var hitbox in item.hitboxes)
                    {
                        Rectangle hb = new Rectangle(hitbox.Location + item.offset, hitbox.Size);
                        boxesToConsider.Add(hb);
                    }
                    // boxesToConsider.AddRange(item.hitboxes);
                }

                //boxesToConsider.RemoveAll(b => (!r.Contains(b) && !r.Intersects(b)));


                if (boxesToConsider.Count == 0)
                {
                    goto collisionFound;
                }

                List<Rectangle> boxesToTestAgain = new List<Rectangle>();
                //   goto collisionFound;
                foreach (var hBox in closeProximityHitboxes())
                {
                    Rectangle temp = new Rectangle(hBox.Location, hBox.Size);
                    temp.Location += (movementProximity * 1.5f).ToPoint();
                    foreach (var hb in boxesToConsider)
                    {
                        if ((hb.Contains(temp) || hb.Intersects(temp)))
                        {

                            boxesToTestAgain.Add(hb);
                            bCollides = true;
                            goto collisionFound;
                            //goto collisionFound;
                        }
                    }
                }


             


            //foreach (var hBox in closeProximityHitboxes())
            //{
            //    Rectangle temp = new Rectangle(hBox.Location, hBox.Size);
            //    temp.Location += (movementProximity * 3 / 3).ToPoint();
            //    foreach (var hb in boxesToTestAgain)
            //    {
            //        if (( temp.Contains(hb) || hb.Contains(temp)))
            //        {
            //            bCollides = true;
            //            goto collisionFound;
            //        }
            //    }
            //}

            #region OLD
            //foreach (var hBox in closeProximityHitboxes())
            //{
            //    for (int i = 1; i < 4; i++)
            //    {
            //        Rectangle temp = new Rectangle(hBox.Location, hBox.Size);

            //        temp.Location += (movementProximity * i / 3).ToPoint();

            //        foreach (var item in spritesNearby)
            //        {
            //            foreach (var hb in item.hitBoxes())
            //            {
            //                if ((temp.Intersects(hb) || temp.Contains(hb) || hb.Contains(temp)))
            //                {
            //                    if (bFirstCheckWasTrue)
            //                    {

            //                        bCollides = true;


            //                        goto collisionFound;
            //                    }
            //                    else
            //                    {
            //                        bFirstCheckWasTrue = true;
            //                    }

            //                }

            //            }
            //            bFirstCheckWasTrue = false;
            //        }

            //        foreach (var item in tileList)
            //        {
            //            foreach (var hb in item.hitboxes)
            //            {
            //                if (temp.Intersects(hb) || temp.Contains(hb) || hb.Contains(temp))
            //                {
            //                    // Console.WriteLine("Intersects");
            //                    bCollides = true;
            //                    goto collisionFound;
            //                }
            //            }
            //        }
            //        if (bCollides)
            //        {
            //            break;
            //        }
            //    }
            //    bFirstCheckWasTrue = false;

            //}
            #endregion

            collisionFound: { }
                var bTest = closeProximityHitboxes();
                // bCollides = false;
                if (!bCollides)
                {
                    // Console.WriteLine(movement+"   "+PlayerController.lastPressedKey.actionIndentifierString);
                    if (movement.Y < 0)
                    {
                        animationIndex = (int)CharacterAnimations.Movement;
                        rotationIndex = (int)BaseSprite.Rotation.Up;
                        if (position.Y < 0)
                        {
                            //  movement += new Vector2(0, -1.0f);
                        }
                    }
                    else if (movement.Y > 0)
                    {
                        animationIndex = (int)CharacterAnimations.Movement;
                        rotationIndex = (int)BaseSprite.Rotation.Down;
                        if (position.Y > 0)
                        {
                            //  movement += new Vector2(0, 1.0f);
                        }

                    }
                    else if (movement.X < 0)
                    {
                        animationIndex = (int)CharacterAnimations.Movement;
                        rotationIndex = (int)BaseSprite.Rotation.Left;
                        if (position.X < 0)
                        {
                            //   movement += new Vector2(-1.0f, 0);
                        }
                    }
                    else if (movement.X > 0)
                    {
                        animationIndex = (int)CharacterAnimations.Movement;
                        rotationIndex = (int)BaseSprite.Rotation.Right;
                        if (position.X > 0)
                        {
                            // movement += new Vector2(1.0f, 0);
                        }
                    }

                    position += movement;
                    MoverMustUpdateHitboxes = true;
                    UpdatePosition();
                    //  MovementLogic();
                }
                else if (false)
                {
                    Vector2 newMovement = new Vector2();
                    if (movement.X != 0)
                    {
                        if (movement.X > 0)
                        {
                            newMovement.X = movement.X - 1;
                        }
                        else if (movement.X < 0)
                        {
                            newMovement.X = movement.X + 1;
                        }
                    }

                    if (movement.Y != 0)
                    {
                        if (movement.Y > 0)
                        {
                            newMovement.Y = movement.Y - 1;
                        }
                        else if (movement.Y < 0)
                        {
                            newMovement.Y = movement.Y + 1;
                        }
                    }

                    //lastFailedTest = new KeyValuePair<Vector2, int>(position, rotationIndex);
                    newMovement = new Vector2((float)Math.Round(newMovement.X), (float)Math.Round(newMovement.Y));
                    if (Vector2.Zero != movement)
                    {
                        Move(newMovement);
                    }

                }
            }
        ignore: { }
        }

        public void Heal()
        {
            var sc = trueSTATChart();
            statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.HP] = sc.currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXHP];
            statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.MANA] = sc.currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXMANA];
            MakeSureActiveStatsAreInOrder();
        }

        public bool CollidesWith(BaseCharacter Mover, Vector2 movement)
        {
            Rectangle collisionProximiter = Mover.spriteGameSize;
            collisionProximiter.Location += movement.ToPoint();

            if ((collisionProximiter.Intersects(spriteGameSize) || collisionProximiter.Contains(spriteGameSize)) && bHasCollision)
            {
                foreach (var hBox in Mover.closeProximityHitboxes())
                {
                    Rectangle temp = hBox;
                    temp.Location += movement.ToPoint();
                    foreach (var hb in closeProximityHitboxes())
                    {
                        if (temp.Intersects(hb) || temp.Contains(hb))
                        {
                            // Console.WriteLine("Intersects");
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void DoCompleteMeleeAttack(BaseCharacter target)
        {
            bool bHasMeleeCounter = target.CCC.equippedClass.bHasMCounter;
            bJumpAttack = true;
            bJumpBack = !bHasMeleeCounter;
            battleAnimationTaskList.Clear();
            battleAnimationTaskList.Add((int)CharacterBattleAnimations.Jump_Brace);
            battleAnimationTaskList.Add((int)CharacterBattleAnimations.Jump_MidAir);
            battleAnimationTaskList.Add((int)CharacterBattleAnimations.Jump_Landing);
            battleAnimationTaskList.Add((int)CharacterBattleAnimations.Attack);
            if (bJumpBack)
            {
                battleAnimationTaskList.Add((int)CharacterBattleAnimations.Jump_Brace);
                battleAnimationTaskList.Add((int)CharacterBattleAnimations.Jump_MidAir);
                battleAnimationTaskList.Add((int)CharacterBattleAnimations.Jump_Landing);
            }
            else
            {
                battleAnimationTaskList.Add((int)CharacterBattleAnimations.Hurt);
            }
            bJumpAttack = false;
            bJumpBack = true;
        }

        public void DoCompleteRangedAttack(BaseCharacter target)
        {
            bool bHasRangedCounter = target.CCC.equippedClass.bHasRCounter;
            battleAnimationTaskList.Add((int)CharacterBattleAnimations.Attack);
        }

        public void DoCompleteMagicAttack(BaseCharacter target)
        {
            battleAnimationTaskList.Add((int)CharacterBattleAnimations.Attack);
        }

        public void ChangeBattleAnimation(int i)
        {
            //this.animationBattleIndex = this.animationBattleIndex;
            animationBattleIndex = i;
            if (animationBattleIndex != i)
            {
                charBattleAnimations[i].elapsedFrameTime = 0;
                charBattleAnimations[i].frameIndex = 0;
                charBattleAnimations[i].bPause = false;
            }
            else if (i == 0)
            {
                charBattleAnimations[i].elapsedFrameTime = 0;
                charBattleAnimations[i].frameIndex = 0;
                charBattleAnimations[i].bPause = false;
            }

            charBattleAnimations[i].bAnimationFinished = false;
            charBattleAnimations[i].bSimplePlayOnce = true;

            if (i == 0)
            {
                charBattleAnimations[i].bSimplePlayOnce = false;
            }
        }

        public void ChangeBattleAnimation(CharacterBattleAnimations ca)
        {
            ChangeBattleAnimation((int)ca);
        }

        /// <summary>
        /// Only for normal animations
        /// </summary>
        /// <param name="ai">Animation Index</param>
        /// <param name="lp">Loop play</param>
        public void PlayAnimation(int ai, bool lp = true)
        {
            if (animationIndex != ai)
            {
                charAnimations[animationIndex].frameIndex = 0;
                charAnimations[animationIndex].bPlayAnimation = false;
                animationIndex = ai;
                charAnimations[animationIndex].bPlayAnimation = true;
                if (!lp)
                {
                    charAnimations[animationIndex].bMustEndAnimation = true;
                }
            }

        }

        /// <summary>
        /// For saving purposes
        /// </summary>
        /// <returns></returns>
        private bool CheckIfAnimationsComplete()
        {
            List<int> changeAnims = new List<int>();
            foreach (var anim in charAnimations)
            {
                if (anim.animationTexture == null)
                {
                    if (charAnimations.IndexOf(anim) == (int)CharacterAnimations.Idle)
                    {
                        System.Windows.Forms.MessageBox.Show("Warning! No default idle animation found, your character will be saved but may cause errors when you try run the game on launch. The idle animation should always be directional and with at least 1 frame.");
                        return false;
                    }
                }
                if (charAnimations.IndexOf(anim) == (int)CharacterAnimations.Idle)
                {
                    if (anim.bDirectional)
                    {
                        foreach (var dirAnim in anim.animationFramesDirectional)
                        {
                            if (dirAnim.Count == 0)
                            {
                                System.Windows.Forms.MessageBox.Show("Warning! Default Idle animation is directional but doesn't contain at least 1 frame in every direction.");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Warning! Default Idle animation is not directional.");
                        return false;
                    }
                }
                if (anim.animationTexture == null && anim != charAnimations[(int)CharacterAnimations.Idle])
                {
                    changeAnims.Add(charAnimations.IndexOf(anim));
                }
                if (anim == charAnimations[(int)CharacterAnimations.Idle] && anim.animationFrames.Count == 0)
                {
                    changeAnims.Add(charAnimations.IndexOf(anim));
                }
                if (!anim.bDirectional && anim != charAnimations[(int)CharacterAnimations.Idle] && anim.animationFrames.Count == 0)
                {
                    changeAnims.Add(charAnimations.IndexOf(anim));
                }

                foreach (var dirAnim in anim.animationFramesDirectional)
                {
                    if (anim.bDirectional && anim != charAnimations[(int)CharacterAnimations.Idle] && dirAnim.Count == 0)
                    {
                        anim.bDirectional = false;
                        changeAnims.Add(charAnimations.IndexOf(anim));
                        break;
                    }
                }
            }
            String report = "Missing animations found, replaced with -Idle- animation:\n";
            foreach (var index in changeAnims)
            {
                charAnimations[index].animationFrames = charAnimations[(int)CharacterAnimations.Idle].animationFramesDirectional[(int)BaseSprite.Rotation.Down];
                report += "Changed animation: " + Enum.GetNames(typeof(CharacterAnimations))[index] + " to default Idle animation\n";
            }

            if (changeAnims.Count != 0)
            {
                System.Windows.Forms.MessageBox.Show("Initial report before saving:\n" + report);
            }

            return true;
        }

        public void CreateTempChar()
        {
            foreach (var item in Enum.GetNames(typeof(CharacterAnimations)))
            {
                charAnimations.Add(new ShapeAnimation());
            }

            charAnimations[(int)CharacterAnimations.Idle].ToggleDirectionalAnim();
        }

        public override void UpdateMinimalUpdateOutsideMap(GameTime gt)
        {
            base.UpdateMinimalUpdateOutsideMap(gt);
        }

        public override void Update(GameTime gameTime)
        {


            if (!bInBattle)
            {
                base.Update(gameTime);
                charAnimations[animationIndex].Update(gameTime, this);
                // CharacterName
            }
            else if (bInBattle)
            {
                try
                {
                    if (statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.HP] <= 0)
                    {
                        bIsAlive = false;
                    }
                    FailSafeAfterUpdate();
                }
                catch
                {
                    statChart.DefaultStatChart();
                }


                charBattleAnimations[animationBattleIndex].BAnimUpdate(gameTime, this);
            }
        }

        public override void MinimalUpdate(GameTime gt)
        {
            charAnimations[animationIndex].Update(gt, this);
        }

        private void FailSafeAfterUpdate()
        {
            while (battleAnimLocs.Count != charBattleAnimations.Count)
            {
                if (battleAnimLocs.Count > charBattleAnimations.Count)
                {
                    battleAnimLocs.RemoveAt(battleAnimLocs.Count - 1);
                }
                else
                {
                    battleAnimLocs.Add(battleAnimLocs[0]);
                }
                Console.WriteLine("Failsafe activated! Have a nice day -The Doctor");

            }


        }

        public override void Draw(SpriteBatch spriteBatch, Color sColor = new Color(), int index = -1, bool waterDraw = false)
        {
            Color drawColor = Color.White;
            if (sColor != default(Color))
            {
                drawColor = sColor;
            }
            if (!bInBattle)
            {
                try
                {
                    if (!waterDraw)
                    {
                        charAnimations[animationIndex].Draw(spriteBatch, this, drawColor);
                    }
                    else
                    {
                        charAnimations[animationIndex].DrawForWaterReflecton(spriteBatch, this, drawColor);
                    }

                }
                catch
                {
                    Console.WriteLine(":S  @" + animationIndex);
                }

            }
            else
            {
                charBattleAnimations[animationBattleIndex].BattleAnimationDraw(spriteBatch, battleAnimLocs[animationBattleIndex], battleAnimScale);
            }


        }

        public void Draw(SpriteBatch spriteBatch, Vector2 drawLoc, int ri, Color sColor = new Color(), int index = -1)
        {
            Color drawColor = Color.White;
            if (sColor != default(Color))
            {
                drawColor = sColor;
            }
            if (!bInBattle)
            {
                try
                {
                    charAnimations[animationIndex].Draw(spriteBatch, this, drawLoc, ri, drawColor);
                }
                catch
                {
                    Console.WriteLine(":S  @" + animationIndex);
                }
            }
        }

        public override void DrawFX(SpriteBatch spriteBatch, Color sColor = new Color(), int index = -1, bool waterDraw = false)
        {
            Color drawColor = Color.White;
            if (sColor != default(Color))
            {
                drawColor = sColor;
            }
            if (!bInBattle)
            {
                try
                {
                    if (!waterDraw)
                    {
                        charAnimations[animationIndex].DrawFX(spriteBatch, this, drawColor);
                    }
                    else
                    {
                        charAnimations[animationIndex].DrawForWaterReflecton(spriteBatch, this, drawColor);
                    }

                }
                catch
                {
                    Console.WriteLine(":S  @" + animationIndex);
                }

            }
            else
            {
                charBattleAnimations[animationBattleIndex].BattleAnimationDraw(spriteBatch, battleAnimLocs[animationBattleIndex], battleAnimScale);
            }


        }


        public override string ToString()
        {
            String toString = "Shape name: " + CharacterName + "; Position Map: " + position / 64 + "; ScriptID: " + shapeID;
            if (script != null)
            {
                toString += script.identifier;
            }
            return toString;
        }

        public STATChart trueSTATChart()
        {
            CCC.charClassList.ForEach(c => c.GenerateStatUp());
            STATChart newSC = new STATChart();
            newSC.DefaultStatChart();

            newSC.currentPassiveStats = statChart.AddPassiveStatChart(newSC).currentPassiveStats;
            newSC.currentSpecialStats = statChart.AddSpecialStatChart(newSC).currentSpecialStats;
            newSC.currentActiveStats = statChart.AddActiveStatChart(newSC).currentActiveStats;

            foreach (var trait in traitList)
            {
                newSC.currentPassiveStats = trait.statModifier.AddPassiveStatChart(newSC).currentPassiveStats;
                newSC.currentSpecialStats = trait.statModifier.AddSpecialStatChart(newSC).currentSpecialStats;
                //  newSC.currentActiveStats = trait.statModifier.AddActiveStatChart(newSC).currentActiveStats;
            }

            foreach (var modifier in modifierList)
            {
                newSC.currentPassiveStats = modifier.statModifier.AddPassiveStatChart(newSC).currentPassiveStats;
                newSC.currentSpecialStats = modifier.statModifier.AddSpecialStatChart(newSC).currentSpecialStats;
                //newSC.currentActiveStats = modifier.statModifier.AddActiveStatChart(newSC).currentActiveStats;
            }

            foreach (var effect in effectList)
            {
                newSC.currentPassiveStats = effect.statModifier.AddPassiveStatChart(newSC).currentPassiveStats;
                newSC.currentSpecialStats = effect.statModifier.AddSpecialStatChart(newSC).currentSpecialStats;
                //  newSC.currentActiveStats = effect.statModifier.AddActiveStatChart(newSC).currentActiveStats;
            }

            foreach (var status in statusList)
            {
                newSC.currentPassiveStats = status.statModifier.AddPassiveStatChart(newSC).currentPassiveStats;
                newSC.currentSpecialStats = status.statModifier.AddSpecialStatChart(newSC).currentSpecialStats;
                //   newSC.currentActiveStats = status.statModifier.AddActiveStatChart(newSC).currentActiveStats;
            }

            if (weapon != null)
            {
                newSC.currentPassiveStats = weapon.statModifier.AddPassiveStatChart(newSC).currentPassiveStats;
                newSC.currentSpecialStats = weapon.statModifier.AddSpecialStatChart(newSC).currentSpecialStats;
                //   newSC.currentActiveStats = weapon.statModifier.AddActiveStatChart(newSC).currentActiveStats;

            }

            if (armour != null)
            {
                newSC.currentPassiveStats = armour.statModifier.AddPassiveStatChart(newSC).currentPassiveStats;
                newSC.currentSpecialStats = armour.statModifier.AddSpecialStatChart(newSC).currentSpecialStats;
                // newSC.currentActiveStats = armour.statModifier.AddActiveStatChart(newSC).currentActiveStats;

            }

            if (CCC != null && CCC.equippedClass != null)
            {
                newSC.currentPassiveStats = CCC.equippedClass.classStats.AddPassiveStatChart(newSC).currentPassiveStats;
                newSC.currentSpecialStats = CCC.equippedClass.classStats.AddSpecialStatChart(newSC).currentSpecialStats;
            }

            foreach (var item in CCC.allClassesExceptEquipped())
            {
                newSC.AddStatChartWithoutActive(item.statUp);
            }


            newSC.currentPassiveStats.ForEach(v => { if (v < 0) { v = 0; } });

            //  Console.WriteLine(newSC.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD]);
            return newSC;
        }

        public STATChart trueSTATChartOutsideCombat()
        {
            STATChart newSC = new STATChart();
            newSC.DefaultStatChart();

            newSC.currentPassiveStats = statChart.AddPassiveStatChart(newSC).currentPassiveStats;
            newSC.currentSpecialStats = statChart.AddSpecialStatChart(newSC).currentSpecialStats;
            newSC.currentActiveStats = statChart.AddActiveStatChart(newSC).currentActiveStats;

            //foreach (var trait in traitList)
            //{
            //    newSC.currentPassiveStats = trait.statModifier.AddPassiveStatChart(newSC).currentPassiveStats;
            //    newSC.currentSpecialStats = trait.statModifier.AddSpecialStatChart(newSC).currentSpecialStats;
            //    //  newSC.currentActiveStats = trait.statModifier.AddActiveStatChart(newSC).currentActiveStats;
            //}

            //foreach (var modifier in modifierList)
            //{
            //    newSC.currentPassiveStats = modifier.statModifier.AddPassiveStatChart(newSC).currentPassiveStats;
            //    newSC.currentSpecialStats = modifier.statModifier.AddSpecialStatChart(newSC).currentSpecialStats;
            //    //newSC.currentActiveStats = modifier.statModifier.AddActiveStatChart(newSC).currentActiveStats;
            //}

            //foreach (var effect in effectList)
            //{
            //    newSC.currentPassiveStats = effect.statModifier.AddPassiveStatChart(newSC).currentPassiveStats;
            //    newSC.currentSpecialStats = effect.statModifier.AddSpecialStatChart(newSC).currentSpecialStats;
            //    //  newSC.currentActiveStats = effect.statModifier.AddActiveStatChart(newSC).currentActiveStats;
            //}

            foreach (var status in statusList)
            {
                newSC.currentPassiveStats = status.statModifier.AddPassiveStatChart(newSC).currentPassiveStats;
                newSC.currentSpecialStats = status.statModifier.AddSpecialStatChart(newSC).currentSpecialStats;
                //   newSC.currentActiveStats = status.statModifier.AddActiveStatChart(newSC).currentActiveStats;
            }

            if (weapon != null)
            {
                newSC.currentPassiveStats = weapon.statModifier.AddPassiveStatChart(newSC).currentPassiveStats;
                newSC.currentSpecialStats = weapon.statModifier.AddSpecialStatChart(newSC).currentSpecialStats;
                //   newSC.currentActiveStats = weapon.statModifier.AddActiveStatChart(newSC).currentActiveStats;

            }

            if (armour != null)
            {
                newSC.currentPassiveStats = armour.statModifier.AddPassiveStatChart(newSC).currentPassiveStats;
                newSC.currentSpecialStats = armour.statModifier.AddSpecialStatChart(newSC).currentSpecialStats;
                // newSC.currentActiveStats = armour.statModifier.AddActiveStatChart(newSC).currentActiveStats;

            }

            if (CCC != null && CCC.equippedClass != null)
            {
                newSC.currentPassiveStats = CCC.equippedClass.classStats.AddPassiveStatChart(newSC).currentPassiveStats;
                newSC.currentSpecialStats = CCC.equippedClass.classStats.AddSpecialStatChart(newSC).currentSpecialStats;
            }


            foreach (var item in CCC.allClassesExceptEquipped())
            {
                newSC.AddStatChartWithoutActive(item.statUp);
            }

            newSC.currentPassiveStats.ForEach(v => { if (v < 0) { v = 0; } });

            //  Console.WriteLine(newSC.currentActiveStats[(int)STATChart.ACTIVESTATS.SHIELD]);
            return newSC;
        }

        public bool Equals(BaseCharacter obj)
        {
            return (obj.shapeID == shapeID);
        }

        public BaseCharacter Clone()
        {
            BaseCharacter temp = (BaseCharacter)this.MemberwiseClone();
            temp.statChart = temp.statChart.Clone();
            temp.charAnimations = new List<ShapeAnimation>();
            foreach (var item in charAnimations)
            {
                temp.charAnimations.Add(item.Clone());
            }
            //temp.abilityList = new List<BasicAbility>(abilityList);
            temp.traitList = new List<BaseTrait>(traitList);
            temp.modifierList = new List<BaseModifier>(modifierList);
            temp.effectList = new List<BaseEffect>(effectList);
            temp.statusList = new List<BasicStatus>(statusList);
            temp.charBattleAnimations = new List<ShapeAnimation>();
            foreach (var item in charBattleAnimations)
            {
                temp.charBattleAnimations.Add(item.Clone());
            }
            temp.CCC = CCC.Clone();
            CCC.parent = temp;

            return temp;
        }

        public List<BasicTile> returnOffenseAbilityRange(List<BasicTile> bt)
        {
            List<List<BasicTile>> abilityRanges = new List<List<BasicTile>>();
            if (CCC == null)
            {
                CCC = new CharacterClassCollection();
                CCC.parent = this;
            }

            //if(CCC.equippedClass.classAbilities.Count==0)
            //{
            //    CCC.equippedClass.classAbilities.Add(GameProcessor.gcDB.gameAbilities[0]);
            //}
            foreach (var ability in CCC.possibleAbilities())
            {
                if (ability.abilityType == (int)BasicAbility.ABILITY_TYPE.ATTACK && ability.IsAbilityAvailable(latestStatChart))
                {
                    abilityRanges.Add(MapListUtility.returnValidMapRadius(ability.abilityMinRange, ability.abilityMaxRange, bt, (position / 64).ToPoint().ToVector2() * 64));
                }
            }
            //foreach (var ability in CCC.charSeparateAbilities)
            //{
            //    if (ability.abilityType == (int)BasicAbility.ABILITY_TYPE.ATTACK && ability.IsAbilityAvailable(latestStatChart))
            //    {
            //        abilityRanges.Add(MapListUtility.returnValidMapRadius(ability.abilityMinRange, ability.abilityMaxRange, bt, (position / 64).ToPoint().ToVector2() * 64));
            //    }
            //}
            List<BasicTile> temp = new List<BasicTile>();
            foreach (var range in abilityRanges)
            {
                temp.AddRange(range.Except(temp));
            }
            return temp;
        }

        public List<BasicTile> returnSupportAbilityRange(List<BasicTile> bt)
        {
            List<List<BasicTile>> abilityRanges = new List<List<BasicTile>>();
            foreach (var ability in CCC.possibleAbilities())
            {
                if (ability.abilityType == (int)BasicAbility.ABILITY_TYPE.SUPPORT)
                {
                    abilityRanges.Add(MapListUtility.returnValidMapRadius(ability.abilityMinRange, ability.abilityMaxRange, bt, (position / 64).ToPoint().ToVector2() * 64));
                }
            }
            //foreach (var ability in CCC.equippedClass.classAbilities)
            //{
            //    if (ability.abilityType == (int)BasicAbility.ABILITY_TYPE.SUPPORT)
            //    {
            //        abilityRanges.Add(MapListUtility.returnValidMapRadius(ability.abilityMinRange, ability.abilityMaxRange, bt, (position / 64).ToPoint().ToVector2() * 64));
            //    }
            //}
            List<BasicTile> temp = new List<BasicTile>();
            foreach (var range in abilityRanges)
            {
                temp.AddRange(range.Except(temp));
            }
            return temp;
        }

        public void ResetAbilities()
        {
            foreach (var ability in CCC.charSeparateAbilities)
            {
                ability.Reset();
            }
            foreach (var ability in CCC.equippedClass.classAbilities)
            {
                ability.Reset();
            }
        }

        public void ClearCombatModifiers()
        {
            modifierList.Clear();
        }

        public void ProcessConsumable(BaseConsumable bc)
        {
            statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.HP] += bc.ConsumableActiveStatModifier.activeStatModifier[(int)STATChart.ACTIVESTATS.HP];
            statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.MANA] += bc.ConsumableActiveStatModifier.activeStatModifier[(int)STATChart.ACTIVESTATS.MANA];
            statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.STORED_AP] += bc.ConsumableActiveStatModifier.activeStatModifier[(int)STATChart.ACTIVESTATS.STORED_AP];

            if (CombatProcessor.bMainCombat)
            {
                modifierList.Add(bc.ConsumableModifier.Clone());
            }
            bc.ConsumeItem();
            MakeSureThatStatsAreCorrectInBattle();
            EncounterInfo.ClearDeathChars();
            List<ActiveStatModifier> temp = new List<ActiveStatModifier>();
            temp.Add(bc.ConsumableActiveStatModifier);
            temp[0].displayName = bc.itemName;
            BattleGUI.CharacterSoloStart(this, bc);
        }

        public void ProcessTurn()
        {


            foreach (var ability in CCC.equippedClass.classAbilities)
            {
                ability.TickAbilityCoolDown();
            }

            foreach (var ability in CCC.charSeparateAbilities)
            {
                ability.TickAbilityCoolDown();
            }

            var activeStatModsExtractedFromModifiers = modifierList.FindAll(m => ActiveStatModifier.Generate(m.statModifier) != null);
            foreach (var item in activeStatModsExtractedFromModifiers)
            {
                activeStatModifiers.Add(ActiveStatModifier.Generate(item.statModifier));
            }


            foreach (var modifier in modifierList)
            {
                modifier.ModifierTick();
            }
            modifierList.RemoveAll(m => m.ModifierIsOver());

            foreach (var modifier in activeStatModifiers)
            {
                modifier.ProcessStats(statChart, trueSTATChart());
                modifier.tick();
            }
            activeStatModifiers.RemoveAll(m => m.isOver());


            MakeSureThatStatsAreCorrectInBattle();
        }

        public void MakeSureActiveStatsAreInOrder()
        {
            MakeSureThatStatsAreCorrectInBattle();
        }

        private void MakeSureThatStatsAreCorrectInBattle()
        {
            int maxTrueHP = trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXHP];
            int maxTrueMP = trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXMANA];

            int currentTrueHP = trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.HP];
            int currentTrueMP = trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.MANA];

            if (currentTrueHP > maxTrueHP)
            {
                statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.HP] = maxTrueHP;
            }
            if (currentTrueMP > maxTrueMP)
            {
                statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.MANA] = maxTrueMP;
            }

            //EncounterInfo.ClearDeathChars();
        }

        public bool HasAvailableSupportAbilities()
        {
            foreach (var item in CCC.charSeparateAbilities)
            {
                if (item.IsAbilityAvailable(trueSTATChart()) && item.abilityType == (int)BasicAbility.ABILITY_TYPE.SUPPORT && item.IsAbilityAvailable(trueSTATChart()))
                {
                    return true;
                }
            }
            foreach (var item in CCC.equippedClass.classAbilities)
            {
                if (item.IsAbilityAvailable(trueSTATChart()) && item.abilityType == (int)BasicAbility.ABILITY_TYPE.SUPPORT && item.IsAbilityAvailable(trueSTATChart()))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasAvailableAOEAbilities()
        {
            foreach (var item in CCC.possibleAbilities())
            {
                if (item.bIsAOE && item.IsAbilityAvailable(trueSTATChart()))
                {
                    return true;
                }
            }
            return false;
        }

        public ShapeAnimation currentBattleAnimation()
        {
            return charBattleAnimations[animationBattleIndex];
        }

        public int RemainingMana()
        {
            return trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.MANA];
        }

        public int RemainingHP()
        {
            return trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.HP];
        }

        public int RemainingAP()
        {
            return trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.STORED_AP];
        }

        public bool IsAlive()
        {
            if (statChart.currentActiveStats[(int)STATChart.ACTIVESTATS.HP] <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int returnTotalThreat()
        {
            int threat = 0;
            foreach (var modifier in modifierList)
            {
                threat += modifier.abilityThreatModifier;
            }
            threat += CCC.equippedClass.baseThreatClass;
            threat += BattleStats.CalculateThreatFromBattle(this);

            return threat;
        }

        public void preCombatLogic()
        {
            if (weapon != null)
            {
                //weapon.statModifier.currentPassiveStats[(int)STATChart.PASSIVESTATS.STR] = 999;

            }
        }

        public void postCombatLogic()
        {
            ClearCombatModifiers();
            ResetAllAbilities();
        }

        private void ResetAllAbilities()
        {
            foreach (var c in CCC.charClassList)
            {
                foreach (var abi in c.classAbilities)
                {
                    abi.ResetCoolDown();
                }
            }

            foreach (var abi in CCC.charSeparateAbilities)
            {
                abi.ResetCoolDown();
            }
        }

        public int distanceFrom(BaseCharacter target)
        {
            Point p1 = (this.position / 64).ToPoint();
            Point p2 = (target.position / 64).ToPoint();
            int distance = Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
            return distance;
        }

        public Point pointDistanceFrom(BaseCharacter target)
        {
            Point p1 = (this.position / 64).ToPoint();
            Point p2 = (target.position / 64).ToPoint();
            int distance = Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
            return new Point(Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y));
        }

        public void changePosition(Vector2 pos)
        {
            position = pos;
            spriteGameSize.Location = pos.ToPoint();
            UpdatePosition();
        }

        public void changePositionRelative(Vector2 v)
        {
            position += v;
            spriteGameSize.Location = position.ToPoint();
            UpdatePosition();
        }

        public void UpdateStats(STATChart sc)
        {
            latestStatChart = sc;
            int radius = latestStatChart.currentPassiveStats[(int)STATChart.PASSIVESTATS.AP] * latestStatChart.currentPassiveStats[(int)STATChart.PASSIVESTATS.MOB];
            //maxRadius = MapListUtility.returnValidMapRadius2(radius, CombatProcessor.zoneTiles, position);
            //maxRadius = GameProcessor.loadedMap.possibleTilesGameZone(maxRadius);
            //EncounterInfo.encounterGroups.Find(eg => eg.bIsEnemyTurnSet).groupTurnSet.Find(gts => gts.character == this).ReGenerateTurn2(maxRadius);
            //maxRadius = EncounterInfo.encounterGroups.Find(eg => eg.bIsEnemyTurnSet).groupTurnSet.Find(gts => gts.character == this).returnCompleteArea();
        }

        public void UpdateRadius()
        {
            int radius = latestStatChart.currentPassiveStats[(int)STATChart.PASSIVESTATS.AP] * latestStatChart.currentPassiveStats[(int)STATChart.PASSIVESTATS.MOB];
            maxRadius = MapListUtility.returnValidMapRadius2(radius, CombatProcessor.zoneTiles, position);
            maxRadius = GameProcessor.loadedMap.possibleTilesGameZoneForEnemy(maxRadius, this);
            var t = EncounterInfo.encounterGroups.FindAll(eg => eg.charactersInGroup.Contains(this));
            CharacterTurn ct = null;
            t.ForEach(ts => { if (ts.groupTurnSet.Find(gts => gts.character == this) != null) { ct = ts.groupTurnSet.Find(gts => gts.character == this); } });
            ct.ReGenerateTurn2(maxRadius);

            maxRadius = ct.returnCompleteArea();
        }

        public Rectangle SpriteGameSizeClone()
        {
            return new Rectangle(spriteGameSize.X, spriteGameSize.Y, spriteGameSize.Width, spriteGameSize.Height);
        }

        public List<BasicAbility> AbilityList()
        {
            var temp = CCC.possibleAbilities();
            temp = temp.Distinct().ToList();
            return temp;
        }

        public List<BasicAbility> UsableAbilityList(BaseCharacter target)
        {
            if (target == null)
            {
                return new List<BasicAbility>();
            }
            var temp = CCC.possibleAbilities();
            temp = temp.Distinct().ToList();
            temp.RemoveAll(abi => !abi.abilityCanHitTargetInRange(this, target));
            if (temp.Count == 0) { return temp; }
            temp.RemoveAll(abi => !abi.IsAbilityAvailable(trueSTATChart()));
            return temp;
        }

        public bool HasPreEncounterScript()
        {
            if (script != null && script.scriptContent.Count != 0)
            {
                if (script.scriptContent.Find(l => l.StartsWith("@TBB")) != default(String))
                {
                    if (script.scriptContent.Find(l => l.StartsWith("@TBE")) != default(String))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public BaseScript GenerateCombatScript()
        {
            BaseScript tempScript = new BaseScript();
            try
            {
                tempScript.scriptContent = new List<string>(script.scriptContent.FindAll(l => script.scriptContent.IndexOf(l) > script.scriptContent.IndexOf(script.scriptContent.Find(li => li.StartsWith("@TBB"))) && script.scriptContent.IndexOf(l) < script.scriptContent.IndexOf(script.scriptContent.Find(li => li.StartsWith("@TBE")))
                ));
            }
            catch (Exception)
            {
                tempScript = null;
                Console.WriteLine("ERROR: error generating combat script between @TB tags");
            }


            return tempScript;
        }

        public override Rectangle returnProperTexBounds()
        {
            try
            {
                return charAnimations[animationIndex].animationTexture.Bounds;
            }
            catch (Exception)
            {

                return default(Rectangle);
            }

        }

        public override Rectangle returnTexSource()
        {
            try
            {
                if (charAnimations[animationIndex].bDirectional)
                {
                    return charAnimations[animationIndex].animationFramesDirectional[rotationIndex][charAnimations[animationIndex].frameIndex];
                }
                else
                {
                    return charAnimations[animationIndex].animationFrames[charAnimations[animationIndex].frameIndex];
                }

            }
            catch (Exception)
            {

                return default(Rectangle);
            }


        }

        internal void ReloadFromSaveFile(CharacterSaveData csd)
        {
            CCC.ReloadFromSave(csd);

            try
            {
                CCC.equippedClass = CCC.charClassList.Find(charClass => charClass.classIdentifier == CCC.equippedClassIdentifier);
            }
            catch (Exception)
            {


            }
        }

        public int HealingRequired = 0;
        internal bool NeedsHealing(int healAmount = 0)
        {
            int maxHP = trueSTATChart().currentPassiveStats[(int)STATChart.PASSIVESTATS.MAXHP];
            int HP = trueSTATChart().currentActiveStats[(int)STATChart.ACTIVESTATS.HP];
            HealingRequired = maxHP - HP;
            if (HealingRequired >= healAmount)
            {
                return true;
            }
            return false;
        }

        internal void ResetAllBattleAnims()
        {
            battleAnimationTaskList.Clear();
            foreach (var item in charBattleAnimations)
            {
                item.frameIndex = 0;
                item.elapsedFrameTime = 0;
                item.bSimplePlayOnce = true;
            }

            animationBattleIndex = (int)BaseCharacter.CharacterBattleAnimations.Idle;
            charBattleAnimations[(int)BaseCharacter.CharacterBattleAnimations.Idle].bSimplePlayOnce = false;
            charBattleAnimations[(int)BaseCharacter.CharacterBattleAnimations.Idle].bMustEndAnimation = false;
        }

        public void LuaUpdateStats(LUA.LuaStatEdit lse)
        {
            var temp = new STATChart(true);
            try
            {
                temp = lse.ExtractStatChart();
            }
            catch (Exception)
            {

            }

            LuaUpdateStats(temp);
        }

        public void LuaUpdateStats(STATChart sc)
        {
            statChart = statChart.StatChartAddition(sc);
        }

        public LUA.LuaCharacterInfo toCharInfo()
        {
            return LUA.LuaCharacterInfo.toCharInfo(this);
        }

        public bool IsName(String n)
        {
            if (CharacterName.Equals(n, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else if (displayName.Equals(n, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    [XmlRoot("Character Expression Util")]
    public class CharacterExpressionInfo
    {
        [XmlElement("Expression")]
        public BaseCharacter.PortraitExpressions expression = BaseCharacter.PortraitExpressions.Neutral;
        [XmlElement("SFX dialogue list")]
        public List<int> dialogueList = new List<int>();
        [XmlIgnore]
        public List<SFXInfo> sfxList = new List<SFXInfo>();

        public CharacterExpressionInfo() { }

        public CharacterExpressionInfo(BaseCharacter.PortraitExpressions expression)
        {
            this.expression = expression;
        }

        public void Reload(GameContentDataBase gcdb)
        {
            dialogueList.ForEach(dl => sfxList.Add(gcdb.gameSFXs.Find(sfx => sfx.sfxID == dl)));
        }

        public bool isCurrentExpression(BaseCharacter.PortraitExpressions pe)
        {
            if (pe == expression)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public SFXInfo returnCorrespondingFX()
        {
            if (dialogueList.Count == 0)
            {
                return null;
            }
            else
            {
                int random = dialogueList[GamePlayUtility.Randomize(0, dialogueList.Count - 1)];
                return GameProcessor.gcDB.gameSFXs.Find(sfx => sfx.sfxID == random);
            }
        }

        public SFXInfo returnCorrespondingFX(int ID)
        {
            if (dialogueList.Count == 0)
            {
                return null;
            }
            else
            {
                return GameProcessor.gcDB.gameSFXs.Find(sfx => sfx.sfxID == ID);
            }
        }
    }
}

namespace LUA
{
    public class LuaCharacterInfo
    {
        public String name = "";
        internal TBAGW.Utilities.Characters.BaseCharacter parent = new TBAGW.Utilities.Characters.BaseCharacter();
        public bool bIsAlive = false;
        public String dialogueName = "";


        public LuaCharacterInfo() { }

        internal static LuaCharacterInfo toCharInfo(TBAGW.Utilities.Characters.BaseCharacter bc)
        {
            LuaCharacterInfo lci = new LuaCharacterInfo();

            if (bc == null)
            {
                bc = new TBAGW.Utilities.Characters.BaseCharacter();
            }

            lci.parent = bc;

            lci.bIsAlive = bc.IsAlive();
            lci.name = bc.displayName;
            lci.dialogueName = bc.displayName;


            return lci;
        }

        public void Rotate(int rot)
        {
            parent.rotationIndex = rot % 4;
        }

        public int getStat(String statName)
        {
            var temp = LUA.LuaStatEdit.getStat(statName);
            if (temp.Key == null)
            {
                return 0;
            }
            else
            {
                if (temp.Key == typeof(STATChart.ACTIVESTATS))
                {
                    return parent.trueSTATChart().currentActiveStats[temp.Value];
                }
                else if (temp.Key == typeof(STATChart.PASSIVESTATS))
                {
                    return parent.trueSTATChart().currentPassiveStats[temp.Value];
                }
                else if (temp.Key == typeof(STATChart.SPECIALSTATS))
                {
                    return parent.trueSTATChart().currentSpecialStats[temp.Value];
                }
            }
            return 0;
        }

        public bool IsWeakTo(String n)
        {
            var affinity = LuaHelp.getAffinity(n);
            return parent.combatInfo.IsWeakTo(affinity);
        }

        public bool IsStrongTo(String n)
        {
            var affinity = LuaHelp.getAffinity(n);
            return parent.combatInfo.IsWeakTo(affinity);
        }

        public void LuaUpdateStats(LUA.LuaStatEdit lse)
        {
            parent.LuaUpdateStats(lse);
        }

        public void ChangeName(String n)
        {
            parent.displayName = n;
        }
    }
}