using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TBAGW.Scenes.Editor;
using TBAGW.Utilities.Characters;
using TBAGW.Utilities.Control.Player;
using TBAGW.Utilities.Map;
using TBAGW.Utilities.SriptProcessing;

namespace TBAGW.Utilities.Sprite
{
    [XmlRoot("BaseSprite")]
    public class BaseSprite
    {
        public enum Rotation { Up = 0, Right, Down, Left };
        #region FIELDS
        [XmlElement("Scale Vector")]
        public Vector2 scaleVector = new Vector2(1);
        [XmlElement("HitboxBounds")]
        public Rectangle shapeTextureBounds = new Rectangle(); //SpriteSheetBounds
        [XmlElement("Position")]
        public Vector2 position = new Vector2(0);
        [XmlElement("HasCollision")]
        public bool bHasCollision = false;
        [XmlElement("NameOfTheShape")]
        public String shapeName = "";
        [XmlElement("NameOfTheShapeMap")]
        public String shapeMapName = "";
        [XmlElement("RectangleToDraw")]
        public Rectangle rectangleToDraw = new Rectangle(); //Box on spritesheet that's drawn on screen
        [XmlElement("TextureLoc")]
        public String textureLoc = "";
        [XmlElement("HitboxTextureLoc")]
        public String hitboxTextureLoc = "";
        [XmlElement("HitboxTextureBox")]
        public Rectangle hitBoxTexBox = new Rectangle();
        [XmlElement("SpriteGameSize")]
        public Rectangle spriteGameSize = new Rectangle(0, 0, 64, 64);
        [XmlArrayItem("ShapeHitBoxCollection")]
        public List<List<Rectangle>> shapeHitBox = new List<List<Rectangle>>();
        [XmlElement("Rotation")]
        public int rotationIndex = (int)BaseSprite.Rotation.Up;
        [XmlElement("Sprite Script")]
        public int scriptID = 0;
        [XmlElement("SpriteLoc")]
        public String spriteLocation = "";
        [XmlElement("Shape ID")]
        public int shapeID = 0;
        [XmlElement("Is active by default")]
        public bool bIsActive = true;
        [XmlElement("Is visible by default")]
        public bool bIsVisible = true;
        /// <summary>
        /// DO NOT USE FOR CHARACTER SPECIFIC ANIMATIONS!
        /// </summary>
        [XmlArrayItem("BaseSprite Animations")]
        public List<ShapeAnimation> baseAnimations = new List<ShapeAnimation>();
        [XmlElement("base animation index")]
        public int animationIndex = 0;
        [XmlElement("Sprite opacity")]
        public int spriteOpacity = 100;
        [XmlElement("Execute script on player touch")]
        public bool bActivateOnTouch = false;
        [XmlElement("Activate from all sides")]
        public bool bActivateFromAllSides = false;
        [XmlElement("objectGroup")]
        public String objectGroup = "";
        [XmlElement("Selected Sprite Effect")]
        public int spriteEffect = (int)SpriteEffects.None;

        [XmlElement("Object map ID")]
        public int objectIDAddedOnMap = 0;

        [XmlElement("Has shadow")]
        public bool bHasShadow = true;
        [XmlElement("Is affected by wind")]
        public bool bIsAffectedByWind = false;

        [XmlElement("Water reflection")]
        public bool bNeedsWaterReflection = false;

        [XmlIgnore]
        public BaseScript script = new BaseScript();
        [XmlIgnore]
        public Texture2D shapeTexture; //Complete texture file were shape is located
        [XmlIgnore]
        public Texture2D hitBoxTexture; //Complete texture file were hitbox is located
        [XmlIgnore]
        public bool bStopAtTarget = true;
        [XmlIgnore]
        public bool bRender = true;
        [XmlIgnore]
        public float speed = 1;
        [XmlIgnore]
        protected Vector2 targetPos;
        [XmlIgnore]
        protected String rootContent = Environment.CurrentDirectory + @"\Content\";
        //public List<Rectangle> currentHitBox = new List<Rectangle>();
        // public enum Rotation { Up = 0, Right, Down, Left };
        [XmlIgnore]
        public List<Rectangle> closeEventDetector = new List<Rectangle> { new Rectangle(0, 0, 32, 16), new Rectangle(0, 0, 16, 32), new Rectangle(0, 0, 32, 16), new Rectangle(0, 0, 16, 32) };
        [XmlIgnore]
        public List<Point> closeEventDetectorSetOff = new List<Point> { new Point(16, -16), new Point(16 + 64, 16), new Point(16, 16 + 64), new Point(-16, 16) };
        [XmlIgnore]
        public Rectangle shapeSurroundings = new Rectangle(0, 0, 96, 96);
        [XmlIgnore]
        public Point shapeSurroundingsOffset = new Point(-16);

        [XmlIgnore]
        public int opacitySteps = 0;
        [XmlIgnore]
        public int opacityStepsTaken = 0;
        [XmlIgnore]
        public float opacityStep = 0.01f;
        [XmlIgnore]
        public float opacityDetailed = 1f;
        #endregion
        internal TileSource.TileType groundTileType = TileSource.TileType.Ground;
        internal bool bActivateAgainOncePlayerGone = false;
        [XmlIgnore]
        private int grassModifierTimePassed = 0;
        [XmlIgnore]
        private int windTimer = 0;
        [XmlIgnore]
        public float horizontalGrassModifier = 0;
        float rot = (float)(0);
        public SpriteMoveHandler smh = new SpriteMoveHandler();
        internal bool bMustUpdateHitBoxes = true;
        [XmlIgnore]
        public bool MoverMustUpdateHitboxes = false;
        [XmlIgnore]
        public List<Rectangle> collisionBoxes = new List<Rectangle>();
        RenderTarget2D fxRender;
        RenderTarget2D shadowRenderRef;
        Rectangle fxDrawLocation;
        internal BasicTile groundTile = new BasicTile();
        internal bool bRecalculateTrueMapSize = true;
        Rectangle MapSize = new Rectangle();
        internal int degreesRot = 0;
        internal BasicMap fromMap = null;
        internal BasicMap currentMapToDisplayOn = null;
        internal Object parentObject = null;
        internal bool bMustGenerateFX = true;
        internal ShapeAnimationInfo animInfo = default(ShapeAnimationInfo);

        public BaseSprite()
        {

        }

        internal void AssignShadowRender(RenderTarget2D r2d)
        {
            if (shadowRenderRef != null && !shadowRenderRef.IsDisposed && shadowRenderRef != r2d)
            {
                shadowRenderRef.Dispose();
            }
            shadowRenderRef = r2d;
        }

        internal RenderTarget2D getShadowFX()
        {
            return shadowRenderRef;
        }

        internal float toRadiansRotation()
        {
            return (((float)degreesRot * 3.14f) / 180f);
        }

        public virtual void ReloadTextures()
        {
            try
            {
                if (!textureLoc.Equals("", StringComparison.OrdinalIgnoreCase))
                {
                    shapeTexture = Game1.contentManager.Load<Texture2D>(textureLoc);
                }
            }
            catch (Exception)
            {
                textureLoc = "TempTexture";
            }

            try
            {
                if (!hitboxTextureLoc.Equals("", StringComparison.OrdinalIgnoreCase))
                {
                    hitBoxTexture = Game1.contentManager.Load<Texture2D>(hitboxTextureLoc);
                }

            }
            catch (Exception)
            {
                hitboxTextureLoc = "TempTexture";
            }

            foreach (var item in baseAnimations)
            {
                item.ReloadTexture();
            }
        }

        public BaseSprite(Texture2D shapeTexture, Texture2D hitboxTexture, Rectangle spriteGameSize, Rectangle hitBoxTexBox, Rectangle rectangleToDraw, int scale, Vector2 position, Vector2 center = default(Vector2), String shapeName = "")
        {
            this.shapeTexture = shapeTexture;
            textureLoc = shapeTexture.Name.Replace(rootContent, "");
            this.hitBoxTexture = hitboxTexture;
            hitboxTextureLoc = hitboxTexture.Name.Replace(rootContent, "");
            this.hitBoxTexBox = hitBoxTexBox;
            this.spriteGameSize = spriteGameSize;

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


            GenerateHitBoxesFromOtherTex();
            GenerateRotationHitboxes();
        }

        public void GenerateHitBoxesFromOtherTex()
        {
            Color[] rawTextureData = new Color[hitBoxTexture.Width * hitBoxTexture.Height];
            hitBoxTexture.GetData<Color>(0, hitBoxTexBox, rawTextureData, 0, rawTextureData.Length);
            int width = hitBoxTexBox.Width;
            int height = hitBoxTexBox.Height;
            Color[,] rawTextureDataGrid = new Color[width, height];

            foreach (var item in Enum.GetNames(typeof(Rotation)))
            {
                shapeHitBox.Add(new List<Rectangle>());
            }

            List<Rectangle> discHitBoxTemp = new List<Rectangle>();

            for (int column = 0; column < width; column++)
            {
                for (int row = 0; row < height; row++)
                {
                    // Assumes row major ordering of the array.
                    rawTextureDataGrid[column, row] = rawTextureData[row * width + column];

                    if (rawTextureDataGrid[column, row] != default(Color))
                    {
                        discHitBoxTemp.Add(new Rectangle((int)position.X + column, (int)position.Y + row, 1, 1));
                    }
                }
            }
            int currentRow = discHitBoxTemp[0].X;
            Vector2 startPos = new Vector2(discHitBoxTemp[0].X, discHitBoxTemp[0].Y);
            int newWidth = 0;
            int pos = 0;
            int prevYPos = discHitBoxTemp[0].Y - 1;
            foreach (Rectangle hitboxTemp in discHitBoxTemp)
            {

                if (currentRow == hitboxTemp.X && (prevYPos + 1) == discHitBoxTemp[pos].Y)
                {
                    newWidth++;
                    prevYPos = discHitBoxTemp[pos].Y;

                }
                else
                {

                    //   shapeHitBox[rotationIndex].Add(new Rectangle((int)(startPos.X - position.X) * scale + (int)position.X, (int)(startPos.Y - position.Y) * scale + (int)position.Y, 1 * scale, (newWidth) * scale));
                    startPos = new Vector2(discHitBoxTemp[pos].X, discHitBoxTemp[pos].Y);
                    currentRow = discHitBoxTemp[pos].X;
                    newWidth = 1;
                    prevYPos = discHitBoxTemp[pos].Y;
                }

                if (pos + 1 == discHitBoxTemp.Count)
                {
                    //  shapeHitBox[rotationIndex].Add(new Rectangle((int)(startPos.X - position.X) * scale + (int)position.X, (int)(startPos.Y - position.Y) * scale + (int)position.Y, 1 * scale, (newWidth) * scale));
                    startPos = new Vector2(discHitBoxTemp[pos].X, discHitBoxTemp[pos].Y);
                    currentRow = discHitBoxTemp[pos].X;
                    newWidth = 0;
                }
                pos++;
            }
        }

        public List<Rectangle> GenerateHitBoxesFromAnyTex(Rectangle texBox, Texture2D tex)
        {
            Color[] rawTextureData = new Color[tex.Width * tex.Height];
            tex.GetData<Color>(0, texBox, rawTextureData, 0, rawTextureData.Length);
            int width = texBox.Width;
            int height = texBox.Height;
            Color[,] rawTextureDataGrid = new Color[width, height];

            List<Rectangle> discHitBoxTemp = new List<Rectangle>();
            List<Rectangle> HitBoxTemp = new List<Rectangle>();

            for (int column = 0; column < width; column++)
            {
                for (int row = 0; row < height; row++)
                {
                    // Assumes row major ordering of the array.
                    rawTextureDataGrid[column, row] = rawTextureData[row * width + column];

                    if (rawTextureDataGrid[column, row] != default(Color))
                    {
                        discHitBoxTemp.Add(new Rectangle((int)position.X + column, (int)position.Y + row, 1, 1));
                    }
                }
            }
            int currentRow = discHitBoxTemp[0].X;
            Vector2 startPos = new Vector2(discHitBoxTemp[0].X, discHitBoxTemp[0].Y);
            int newWidth = 0;
            int pos = 0;
            int prevYPos = discHitBoxTemp[0].Y - 1;
            foreach (Rectangle hitboxTemp in discHitBoxTemp)
            {

                if (currentRow == hitboxTemp.X && (prevYPos + 1) == discHitBoxTemp[pos].Y)
                {
                    newWidth++;
                    prevYPos = discHitBoxTemp[pos].Y;

                }
                else
                {

                    // HitBoxTemp.Add(new Rectangle((int)(startPos.X - position.X) * scale + (int)position.X, (int)(startPos.Y - position.Y) * scale + (int)position.Y, 1 * scale, (newWidth) * scale));
                    startPos = new Vector2(discHitBoxTemp[pos].X, discHitBoxTemp[pos].Y);
                    currentRow = discHitBoxTemp[pos].X;
                    newWidth = 1;
                    prevYPos = discHitBoxTemp[pos].Y;
                }

                if (pos + 1 == discHitBoxTemp.Count)
                {
                    //   HitBoxTemp.Add(new Rectangle((int)(startPos.X - position.X) * scale + (int)position.X, (int)(startPos.Y - position.Y) * scale + (int)position.Y, 1 * scale, (newWidth) * scale));
                    startPos = new Vector2(discHitBoxTemp[pos].X, discHitBoxTemp[pos].Y);
                    currentRow = discHitBoxTemp[pos].X;
                    newWidth = 0;
                }
                pos++;
            }

            return HitBoxTemp;
        }

        public void GenerateOpacitySteps(int opacityEnd, float time)
        {
            if (time != 0)
            {
                opacitySteps = (int)(time * 60);
                opacityStep = (float)(((float)(opacityEnd - spriteOpacity)) / ((float)opacitySteps)) / 100;
                opacityDetailed = ((float)spriteOpacity) / 100;
            }
            else
            {
                spriteOpacity = opacityEnd;
            }

            opacityStepsTaken = 0;

        }

        public void StartSpriteMoveHandler(Vector2 end, int mapIndex, NPCMoveToCommand tc)
        {

            BasicMap mapToCheckIn;
            if (mapIndex == -1)
            {
                mapToCheckIn = GameProcessor.parentMap;
            }
            else
            {
                mapToCheckIn = BasicMap.allMapsGame().Find(m => m.identifier == mapIndex);
            }


            if (tc.nodePath == null)
            {

                var chunks = MapChunk.returnChunkRadius(mapToCheckIn, position, 2);

                if (chunks.Count != 0 && position != end)
                {
                    List<BasicTile> tiles = new List<BasicTile>();
                    foreach (var chunk in chunks)
                    {
                        //  chunk.Checkhunk();
                        // tiles.AddRange(mapToCheckIn.possibleTilesWithController(chunk.region, mapToCheckIn));
                        tiles.AddRange(mapToCheckIn.possibleTilesWithController(chunk.region, chunk, mapToCheckIn));
                    }
                    var tempTileStart = tiles.Find(tile => tile.positionGrid.X == 62);
                    tiles = tiles.Distinct().ToList();
                    try
                    {

                        var allPossibleNodes = AI.PathFinder.NewPathSearch(position, end, tiles);
                        //  allPossibleNodes.Reverse();

                        var lastVector = position;
                        if (allPossibleNodes.Count != 0)
                        {
                            lastVector = new Vector2(allPossibleNodes[allPossibleNodes.Count - 1].coord.X * 64, allPossibleNodes[allPossibleNodes.Count - 1].coord.Y * 64);
                        }

                        if (lastVector == end)
                        {
                            // (PlayerController.selectedSprite as BaseCharacter).changePosition(new Vector2(allPossibleNodes[1].coord.X * 64, allPossibleNodes[1].coord.Y * 64));
                            allPossibleNodes.Reverse();
                            var NodesWithinRange = MapListUtility.findEqualNodesToTileList(allPossibleNodes, tiles);
                            //PathMoveHandler.Start(gts.character, NodesWithinRange);
                            smh = new SpriteMoveHandler();
                            smh.Start(this, NodesWithinRange);
                            tc.nodePath = new List<AI.Node>( NodesWithinRange);
                            smh.speed = speed;
                        }
                        else if (allPossibleNodes.Count == 0 && lastVector != end)
                        {
                            position = end;
                            throw new Exception("No path possible exception.");
                        }
                    }
                    catch (Exception e)
                    {
                        tc.nodePath = new List<AI.Node>();
                        Console.WriteLine("ERROR generating path for " + this + ", from: " + position + ", to:" + end);
                        Console.WriteLine("On map ID: " + mapIndex + ", " + mapToCheckIn);
                        Console.WriteLine("Teleporting to end instead. " + end);
                        Console.WriteLine(e.Message);
                        this.position = end;
                        this.UpdatePosition();
                        this.bMustUpdateHitBoxes = true;
                        this.bRecalculateTrueMapSize = true;
                        smh.bIsBusy = false;
                    }
                }
                else if (chunks.Count == 0)
                {
                    tc.nodePath = new List<AI.Node>();
                    Console.WriteLine("Soft error, no chunks found for " + this + ", from: " + position + ", to:" + end);
                    Console.WriteLine("On map ID: " + mapIndex + ", " + mapToCheckIn);
                    Console.WriteLine("Teleporting to end instead. " + end);
                    this.position = end;
                    this.UpdatePosition();
                    this.bMustUpdateHitBoxes = true;
                    this.bRecalculateTrueMapSize = true;
                    smh.bIsBusy = false;
                }
            }
            else
            {
                if (tc.nodePath.Count != 0)
                {
                    smh = new SpriteMoveHandler();
                    smh.Start(this, tc.nodePath);
                    smh.speed = speed;
                }
                else //if(tc.bCheckJustOneMoreTime)
                {
                    tc.bCheckJustOneMoreTime = false;
                    tc.nodePath = null;

                    this.position = end;
                    this.UpdatePosition();
                    this.bMustUpdateHitBoxes = true;
                    this.bRecalculateTrueMapSize = true;
                    smh.bIsBusy = false;
                }

            }

        }

        internal void UpdateHitBoxes()
        {
            List<Rectangle> temp = new List<Rectangle>();
            if (shapeHitBox.Count == 0)
            {
                shapeHitBox.Add(new List<Rectangle>());
            }
            foreach (var hitbox in shapeHitBox[(int)Rotation.Up])
            {
                temp.Add(new Rectangle((int)(hitbox.X * scaleVector.X) + (int)position.X, (int)(hitbox.Y * scaleVector.Y) + (int)position.Y, (int)(hitbox.Width * scaleVector.X), (int)(hitbox.Height * scaleVector.Y)));

                if (temp.Last().Width <= 0)
                {
                    temp[temp.Count - 1] = new Rectangle(temp[temp.Count - 1].X, temp[temp.Count - 1].Y, 1, temp[temp.Count - 1].Height);
                }
                if (temp.Last().Height <= 0)
                {
                    temp[temp.Count - 1] = new Rectangle(temp[temp.Count - 1].X, temp[temp.Count - 1].Y, temp[temp.Count - 1].Width, 1);
                }
            }
            collisionBoxes = (temp);
            bMustUpdateHitBoxes = false;
            MoverMustUpdateHitboxes = false;
        }

        internal void UpdateHitBoxes(ObjectGroup og)
        {
            List<Rectangle> temp = new List<Rectangle>();
            if (shapeHitBox.Count == 0)
            {
                shapeHitBox.Add(new List<Rectangle>());
            }

            foreach (var hitbox in shapeHitBox[(int)Rotation.Up])
            {
                Vector2 diff = new Vector2(1.0f) - og.scaleVector;
                if (diff != new Vector2(0))
                {
                    int index = og.groupItems.IndexOf(this);

                    temp.Add(new Rectangle((int)(hitbox.X * og.scaleVector.X) + (int)(position.X) - (int)((position.X - og.trueMapSize.Location.X) * diff.X), (int)(hitbox.Y * og.scaleVector.Y) + (int)position.Y - (int)((position.Y - og.trueMapSize.Location.Y) * diff.Y), (int)(hitbox.Width * og.scaleVector.X), (int)(hitbox.Height * og.scaleVector.Y)));

                }
                else
                {
                    temp.Add(new Rectangle((int)(hitbox.X * og.scaleVector.X) + (int)position.X, (int)(hitbox.Y * og.scaleVector.Y) + (int)position.Y, (int)(hitbox.Width * og.scaleVector.X), (int)(hitbox.Height * og.scaleVector.Y)));

                }


                if (temp.Last().Width <= 0)
                {
                    temp[temp.Count - 1] = new Rectangle(temp[temp.Count - 1].X, temp[temp.Count - 1].Y, 1, temp[temp.Count - 1].Height);
                }
                if (temp.Last().Height <= 0)
                {
                    temp[temp.Count - 1] = new Rectangle(temp[temp.Count - 1].X, temp[temp.Count - 1].Y, temp[temp.Count - 1].Width, 1);
                }


            }
            collisionBoxes = (temp);
            bMustUpdateHitBoxes = false;
            MoverMustUpdateHitboxes = false;
        }

        public virtual void UpdateMinimalUpdateOutsideMap(GameTime gt)
        {
            if (smh.bIsBusy)
            {
                smh.Update(gt);
            }

            if (position == Vector2.Zero && spriteGameSize.Location != Point.Zero)
            {
                position = spriteGameSize.Location.ToVector2();
            }

            spriteGameSize.X = (int)position.X;
            spriteGameSize.Y = (int)position.Y;
        }

        public virtual void MinimalUpdate(GameTime gt)
        {
            if (baseAnimations.Count != 0 && baseAnimations.Count > animationIndex)
            {
                baseAnimations[animationIndex].Update(gt, this);
            }
        }



        public virtual void Update(GameTime gameTime)
        {
            if (PlayerController.selectedSprite != null && bMustUpdateHitBoxes && bHasCollision)
            {
                if (ContainsForCollision(PlayerController.selectedSprite))
                {
                    UpdateHitBoxes();
                }
            }

            if (PlayerController.selectedSprite != null && MoverMustUpdateHitboxes && bHasCollision)
            {
                if (ContainsForCollision(PlayerController.selectedSprite))
                {
                    UpdateHitBoxes();
                }
            }

            if (smh.bIsBusy)
            {
                // position
                smh.Update(gameTime);
            }

            if (bIsAffectedByWind)
            {
                HandleWindGeneration(gameTime);
            }

            if (opacityStepsTaken < opacitySteps)
            {
                opacityStepsTaken++;
                opacityDetailed += opacityStep;
                spriteOpacity = (int)(opacityDetailed * 100);

            }

            rot = (float)((Math.PI / 180) * (360));

            if (position == Vector2.Zero && spriteGameSize.Location != Point.Zero)
            {
                position = spriteGameSize.Location.ToVector2();
            }

            spriteGameSize.X = (int)position.X;
            spriteGameSize.Y = (int)position.Y;
            // bRender = SceneUtility.isWithinMap(position);
            // MoveHitBoxes();

            closeEventDetector[0] = new Rectangle((int)position.X + closeEventDetectorSetOff[0].X, (int)position.Y + closeEventDetectorSetOff[0].Y, 32, 16);
            closeEventDetector[1] = new Rectangle((int)position.X + closeEventDetectorSetOff[1].X, (int)position.Y + closeEventDetectorSetOff[1].Y, 16, 32);
            closeEventDetector[2] = new Rectangle((int)position.X + closeEventDetectorSetOff[2].X, (int)position.Y + closeEventDetectorSetOff[2].Y, 32, 16);
            closeEventDetector[3] = new Rectangle((int)position.X + closeEventDetectorSetOff[3].X, (int)position.Y + closeEventDetectorSetOff[3].Y, 16, 32);

            shapeSurroundings = new Rectangle((int)position.X + shapeSurroundingsOffset.X, (int)position.Y + shapeSurroundingsOffset.Y, 96, 96);



            if (baseAnimations.Count != 0 && baseAnimations.Count > animationIndex)
            {
                baseAnimations[animationIndex].Update(gameTime, this);
            }
            if (bIsActive && (script != null || script != default(BaseScript) || scriptID != -1))
            {
                if (parentObject != null && parentObject.GetType() == typeof(NPC))
                {
                    goto skipScript;
                }
                if (PlayerController.selectedSprite != null)
                {
                    if (bActivateOnTouch)
                    {

                        if (PlayerController.selectedSprite.trueMapSize().Contains(trueMapSize()) || PlayerController.selectedSprite.trueMapSize().Intersects(trueMapSize()))
                        {
                            ScriptProcessor.ChangeActiveScript(script, this, PlayerController.selectedSprite, true);
                            ScriptProcessor.UpdateExecuteScript(gameTime, this);
                            bActivateAgainOncePlayerGone = true;
                            bActivateOnTouch = false;
                        }
                        else if ((!PlayerController.selectedSprite.spriteGameSize.Contains(spriteGameSize) || !PlayerController.selectedSprite.spriteGameSize.Intersects(spriteGameSize)) && ScriptProcessor.backgroundScripts.Contains(script))
                        {
                            ScriptProcessor.FinalizeScript(this);
                            //  spriteOpacity = 100;
                        }
                    }
                }
            }
            if (bActivateAgainOncePlayerGone)
            {
                if ((!PlayerController.selectedSprite.spriteGameSize.Contains(spriteGameSize) && !PlayerController.selectedSprite.spriteGameSize.Intersects(spriteGameSize)))
                {
                    bActivateOnTouch = true;
                    bActivateAgainOncePlayerGone = false;
                }
            }
        skipScript: { }
        }

        private void HandleWindGeneration(GameTime gt)
        {
            grassModifierTimePassed += gt.ElapsedGameTime.Milliseconds;

            if (grassModifierTimePassed > windTimer)
            {
                windTimer = GamePlayUtility.Randomize(200, 1000);
                float maxWind = 0.4f;
                float minWind = -0.4f;
                float windMargin = 0.05f;
                float minWindMargin = 0f;
                float maxWindMargin = 0f;
                //horizontalGrassModifier = 0;
                minWindMargin = horizontalGrassModifier - windMargin;
                if (minWindMargin < minWind)
                {
                    minWindMargin = minWind;
                }
                maxWindMargin = horizontalGrassModifier + windMargin;
                if (maxWindMargin > maxWind)
                {
                    maxWindMargin = maxWind;
                }
                horizontalGrassModifier = GamePlayUtility.ExpertRandomizeSmallNumbers(minWindMargin, maxWindMargin);

                grassModifierTimePassed = 0;
            }
        }

        public virtual void UpdatePosition()
        {
            spriteGameSize.X = (int)position.X;
            spriteGameSize.Y = (int)position.Y;
            bMustUpdateHitBoxes = true;
            bRecalculateTrueMapSize = true;
            if (this.GetType() == typeof(SpriteLight))
            {
                (this as SpriteLight).RecalculateLight();
            }
            MovementLogic();

            //   var ObjectAsMapSprite = MapChunk.consideredSprites.FindAll(s => s.obj.GetType() == typeof(NPC)).Find(s => (s.obj as NPC).baseCharacter == this);
            //   var ObjectAsMapSpriteList = MapChunk.consideredSprites.FindAll(s => s.obj.GetType() == typeof(NPC));
            objectInfo ObjectAsMapSprite = null;
            if (MapObjectHelpClass.objectsToUpdateOutsideOfMap.Find(s => (s as NPC).baseCharacter == this) != null)
            {
                ObjectAsMapSprite = (MapObjectHelpClass.objectsToUpdateOutsideOfMap.Find(s => (s as NPC).baseCharacter == this) as NPC).objInfo; ;
            }

            if (ObjectAsMapSprite != null)
            {
                ObjectAsMapSprite.heightIndicator = this.getHeightIndicator();
                ObjectAsMapSprite.mapSize = this.trueMapSize();
                InsertObjectInFrontOfNeededObjects(ObjectAsMapSprite);
            }
            else if (this == PlayerController.selectedSprite && !CombatProcessor.bIsRunning)
            {
                var test = MapChunk.consideredSprites.FindAll(s => s.objectType == objectInfo.type.Character).Find(s => s.obj == this);
                if (test != null)
                {
                    MapChunk.consideredSprites.FindAll(s => s.objectType == objectInfo.type.Character).Find(s => s.obj == this).heightIndicator = this.getHeightIndicator();
                }

            }

        }

        private void InsertObjectInFrontOfNeededObjects(objectInfo objectAsMapSprite)
        {
            int index = MapChunk.consideredSprites.IndexOf(objectAsMapSprite);
            var testList = MapChunk.consideredSprites.FindAll(s => s != objectAsMapSprite && (s.mapSize.Contains(objectAsMapSprite.mapSize) || s.mapSize.Intersects(objectAsMapSprite.mapSize)) && (s.heightIndicator > objectAsMapSprite.heightIndicator) && (true || MapChunk.consideredSprites.IndexOf(s) > index));
            if (testList.Count != 0)
            {
                int min = testList.Max(i => MapChunk.consideredSprites.IndexOf(i));
                if (index > min)
                {
                    var temp = new KeyValuePair<int, int>(index, min);
                    if (!MapChunk.changesToOrder.Contains(temp))
                    {
                        MapChunk.changesToOrder.Add(temp);
                    }
                }
            }
        }

        public void UpdatePositioNonMovement()
        {
            spriteGameSize.X = (int)position.X;
            spriteGameSize.Y = (int)position.Y;
            // bRender = SceneUtility.isWithinMap(position);
        }

        public virtual void UpdateAnim(GameTime gameTime, int index)
        {

            if (baseAnimations.Count != 0 && index != -1)
            {
                baseAnimations[index].Update(gameTime, this);
            }
            if (bIsActive)
            {

            }
        }

        public void postSerializationReload(GameContentDataBase gcdb)
        {
            try
            {
                script = gcdb.gameScripts.Find(s => s.identifier == scriptID);
            }
            catch (Exception)
            {
                script = new BaseScript();
            }

        }

        public List<Rectangle> closeProximityHitboxes()
        {
            // List<Rectangle> currentHitbox = new List<Rectangle>(shapeHitBox[0]);
            //foreach (var hitbox in shapeHitBox[(int)Rotation.Up])
            //{
            //    currentHitbox.Add(new Rectangle(hitbox.X + (int)position.X, hitbox.Y + (int)position.Y, hitbox.Width, hitbox.Height));
            //}
            UpdateHitBoxes();

            return collisionBoxes;
        }

        public void Clear()
        {
            //  shapeHitBox.Clear();
        }

        public virtual void Move(Vector2 movement)
        {
            if (GameProcessor.bIsInGame)
            {
                int futurXTile = (int)(position.X + movement.X) / 64;
                int futurYTile = (int)(position.Y + movement.Y) / 64;
                Vector2 futurePositionGrid = new Vector2(futurXTile, futurYTile);

                bool bCollides = false;
                Rectangle collisionProximiter = spriteGameSize;
                collisionProximiter.Location += movement.ToPoint();

                //if (GameProcessor.loadedMap.canMoveToThisTile((collisionProximiter.Location.ToVector2() / 64).ToPoint(),this))
                //{
                //    bCollides = true;
                //}


                if (!bCollides)
                {
                    position += movement;
                }
            }

        }

        public bool Contains(Vector2 target)
        {
            if (spriteGameSize.Contains(target) && bHasCollision)
            {
                foreach (var hitbox in shapeHitBox[rotationIndex])
                {
                    if (hitbox.Contains(target))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool ContainsForEditorSelection(Vector2 target)
        {
            if (trueMapSize().Contains(target))
            {

                return true;

            }

            return false;
        }

        public bool ContainsForCollision(BaseSprite bs)
        {
            if (bHasCollision)
            {
                if (this.trueMapSize().Contains(bs.trueMapSize()) || this.trueMapSize().Intersects(bs.trueMapSize()))
                {
                    return true;
                }
            }

            return false;
        }

        public void GenerateRotationHitboxes()
        {
            List<Rectangle> tempHitbox = new List<Rectangle>();
            Rectangle tempRectangle = new Rectangle();

            if (shapeHitBox.Count == 0)
            {
                foreach (var item in Enum.GetNames(typeof(Rotation)))
                {
                    shapeHitBox.Add(new List<Rectangle>());
                }
            }

            //For 270 degrees Right
            foreach (var hitbox in shapeHitBox[(int)Rotation.Up])
            {
                int tempX = (int)(hitbox.X);
                int tempY = (int)(hitbox.Y);
                int tempWidth = (int)(hitbox.Width);
                int tempHeight = (int)(hitbox.Height);
                Vector2 tempPosition = new Vector2(tempX - (position.X), tempY - (position.Y));

                tempRectangle.X = tempY;
                tempRectangle.Y = tempX;
                tempRectangle.Width = (int)(tempHeight);
                tempRectangle.Height = (int)(tempWidth);


                tempHitbox.Add(tempRectangle);
            }
            shapeHitBox[(int)Rotation.Right].Clear();
            shapeHitBox[(int)Rotation.Right].AddRange(tempHitbox);
            tempHitbox.Clear();

            //Rotation 180 DOWN
            foreach (var hitbox in shapeHitBox[(int)Rotation.Up])
            {
                int tempX = (int)(hitbox.X);
                int tempY = (int)(hitbox.Y);
                int tempWidth = (int)(hitbox.Width);
                int tempHeight = (int)(hitbox.Height);
                Vector2 tempPosition = new Vector2((tempX - (position.X)), (tempY - (position.Y)));

                tempRectangle.X = hitBoxTexBox.Width - tempWidth - tempX;
                tempRectangle.Y = hitBoxTexBox.Height - tempHeight - tempY;
                tempRectangle.Width = (int)(tempWidth);
                tempRectangle.Height = (int)(tempHeight);

                tempHitbox.Add(tempRectangle);
            }
            shapeHitBox[(int)Rotation.Down].Clear();
            shapeHitBox[(int)Rotation.Down].AddRange(tempHitbox);
            tempHitbox.Clear();

            //For 90 degrees RIGHT
            foreach (var hitbox in shapeHitBox[(int)Rotation.Down])
            {
                int tempX = (int)(hitbox.X);
                int tempY = (int)(hitbox.Y);
                int tempWidth = (int)(hitbox.Width);
                int tempHeight = (int)(hitbox.Height);
                Vector2 tempPosition = new Vector2(tempX - (position.X), tempY - (position.Y));


                tempRectangle.Y = (int)((tempPosition.X));

                //                tempRectangle.Y = (int)((-tempPosition.X + (hitBoxTexture.Width - scale) - (tempWidth - scale) + position.Y));
                tempRectangle.X = (int)((tempPosition.Y) + (position.X));
                tempRectangle.Width = (int)(tempHeight);
                tempRectangle.Height = (int)(tempWidth);

                tempRectangle.X = tempY;
                tempRectangle.Y = tempX;
                tempRectangle.Width = (int)(tempHeight);
                tempRectangle.Height = (int)(tempWidth);


                tempHitbox.Add(tempRectangle);
            }
            shapeHitBox[(int)Rotation.Left].Clear();
            shapeHitBox[(int)Rotation.Left].AddRange(tempHitbox);
            tempHitbox.Clear();
        }

        public virtual Rectangle returnProperTexBounds()
        {
            if (baseAnimations.Count == 0)
            {
                if (shapeTexture != null)
                {
                    return shapeTexture.Bounds;
                }
                else
                {
                    return default(Rectangle);
                }


            }
            else
            {

                try
                {
                    if (baseAnimations[animationIndex].animationTexture != null)
                    {
                        return baseAnimations[animationIndex].animationTexture.Bounds;
                    }
                    else
                    {
                        return default(Rectangle);
                    }
                }
                catch
                {
                    return default(Rectangle);
                }
            }

        }

        public virtual Rectangle returnTexSource()
        {
            if (baseAnimations.Count == 0)
            {
                if (shapeTexture != null)
                {
                    return rectangleToDraw;
                }
                else
                {
                    return default(Rectangle);
                }


            }
            else
            {

                try
                {
                    if (baseAnimations[animationIndex].animationTexture != null)
                    {
                        return baseAnimations[animationIndex].animationFrames[baseAnimations[animationIndex].frameIndex];
                    }
                    else
                    {
                        return default(Rectangle);
                    }
                }
                catch
                {
                    return default(Rectangle);
                }
            }

        }

        public virtual void DrawFX(SpriteBatch spriteBatch, Color sColor = new Color(), int index = -1, bool waterDraw = false)
        {
            Color drawColor = Color.White;
            if (sColor != default(Color))
            {
                drawColor = sColor;
            }
            drawColor *= (spriteOpacity / 100f);
            if ((shapeTexture == null || textureLoc.Equals("")) && baseAnimations.Count == 0)
            {
                baseAnimations.Add(new ShapeAnimation());
            }

            if (bIsVisible)
            {

                if (baseAnimations.Count == 0)
                {
                    #region OLD
                    //90
                    if (rotationIndex == (int)Rotation.Left)
                    {
                        spriteBatch.Draw(shapeTexture, spriteGameSize, rectangleToDraw, drawColor, (float)Math.PI / 2, new Vector2(0, rectangleToDraw.Width), (SpriteEffects)spriteEffect, 0);
                        // Console.WriteLine("Right");
                    }//180
                    else if (rotationIndex == (int)Rotation.Down)
                    {
                        spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor, (float)Math.PI, new Vector2(rectangleToDraw.Width, rectangleToDraw.Height), (SpriteEffects)spriteEffect, 0);
                        //Console.WriteLine("Down");

                    }//270
                    else if (rotationIndex == (int)Rotation.Right)
                    {
                        spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor, (float)(3 * Math.PI / 2), new Vector2(rectangleToDraw.Height, 0), (SpriteEffects)spriteEffect, 0);
                        //  Console.WriteLine("Left");
                    }
                    else if (rotationIndex == (int)Rotation.Up)
                    {//0

                        switch (groundTileType)
                        {
                            case TileSource.TileType.Ground:
                                this.rot = 0f;

                                spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor, rot, Vector2.Zero, (SpriteEffects)spriteEffect, 0);
                                break;
                            case TileSource.TileType.Building:
                                spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor, rot, Vector2.Zero, (SpriteEffects)spriteEffect, 0);
                                break;
                            case TileSource.TileType.Fluid:
                                // spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor, rot, Vector2.Zero, (SpriteEffects)spriteEffect, 0);

                                var tempRect = new Rectangle(spriteGameSize.X, spriteGameSize.Y + 15, spriteGameSize.Width, spriteGameSize.Height - 15);
                                float divider = (64f - 15f) / 64f; ;
                                float divider2 = (15f) / 64f; ;
                                var refR = rectangleToDraw;
                                var tempRectSource = new Rectangle(refR.X, refR.Y + (int)(refR.Height * divider2 * 0), refR.Width, (int)(refR.Height * divider));
                                spriteBatch.Draw(shapeTexture, tempRect, tempRectSource, drawColor, 0f, Vector2.Zero, (SpriteEffects)spriteEffect, 0);
                                break;
                            default:
                                break;
                        }

                        //spriteBatch.Draw(Game1.hitboxHelp,new Rectangle(0,0,64,64),Color.Green);
                        // Console.WriteLine("Up");
                        #endregion
                    }
                    else
                    {
                        Console.WriteLine("Rotation be no good");
                    }
                }
                else if (baseAnimations.Count != 0)
                {
                    try
                    {
                        if (!waterDraw)
                        {
                            if (index == -1)
                            {
                                baseAnimations[animationIndex].DrawFX(spriteBatch, this, sColor);
                            }
                            else
                            {
                                baseAnimations[index].DrawFX(spriteBatch, this, sColor);
                            }
                        }
                        else
                        {
                            if (index == -1)
                            {
                                baseAnimations[animationIndex].DrawForWaterReflecton(spriteBatch, this);
                            }
                            else
                            {
                                baseAnimations[index].DrawForWaterReflecton(spriteBatch, this);
                            }
                        }

                    }
                    catch
                    {
                        Console.WriteLine(":S  @" + animationIndex);
                    }
                }

            }
            else if (!bIsVisible && MapBuilder.bIsRunning)
            {
                if (baseAnimations.Count == 0)
                {
                    //90
                    if (rotationIndex == (int)Rotation.Left)
                    {
                        spriteBatch.Draw(shapeTexture, spriteGameSize, rectangleToDraw, drawColor * .3f, (float)Math.PI / 2, new Vector2(0, rectangleToDraw.Width), (SpriteEffects)spriteEffect, 0);
                        // Console.WriteLine("Right");
                    }//180
                    else if (rotationIndex == (int)Rotation.Down)
                    {
                        spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor * .3f, (float)Math.PI, new Vector2(rectangleToDraw.Width, rectangleToDraw.Height), (SpriteEffects)spriteEffect, 0);
                        //Console.WriteLine("Down");

                    }//270
                    else if (rotationIndex == (int)Rotation.Right)
                    {
                        spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor * .3f, (float)(3 * Math.PI / 2), new Vector2(rectangleToDraw.Height, 0), (SpriteEffects)spriteEffect, 0);
                        //  Console.WriteLine("Left");
                    }
                    else if (rotationIndex == (int)Rotation.Up)
                    {//0

                        spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor * .3f, 0, Vector2.Zero, (SpriteEffects)spriteEffect, 0);
                        // Console.WriteLine("Up");
                    }
                    else
                    {
                        Console.WriteLine("Rotation be no good");
                    }
                }
                else if (baseAnimations.Count != 0)
                {
                    try
                    {
                        if (index == -1)
                        {
                            baseAnimations[animationIndex].Draw(spriteBatch, this, drawColor * .3f);
                        }
                        else
                        {
                            baseAnimations[index].Draw(spriteBatch, this, drawColor * .3f);
                        }
                    }
                    catch
                    {
                        Console.WriteLine(":S  @" + animationIndex);
                    }
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Color sColor = new Color(), int index = -1, bool waterDraw = false)
        {
            Color drawColor = Color.White;
            if (sColor != default(Color))
            {
                drawColor = sColor;
            }
            drawColor *= (spriteOpacity / 100f);
            if ((shapeTexture == null || textureLoc.Equals("")) && baseAnimations.Count == 0)
            {
                baseAnimations.Add(new ShapeAnimation());
            }

            if (bIsVisible)
            {

                if (baseAnimations.Count == 0)
                {

                    //90
                    if (rotationIndex == (int)Rotation.Left)
                    {
                        spriteBatch.Draw(shapeTexture, spriteGameSize, rectangleToDraw, drawColor, (float)Math.PI / 2, new Vector2(0, rectangleToDraw.Width), (SpriteEffects)spriteEffect, 0);
                        // Console.WriteLine("Right");
                    }//180
                    else if (rotationIndex == (int)Rotation.Down)
                    {
                        spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor, (float)Math.PI, new Vector2(rectangleToDraw.Width, rectangleToDraw.Height), (SpriteEffects)spriteEffect, 0);
                        //Console.WriteLine("Down");

                    }//270
                    else if (rotationIndex == (int)Rotation.Right)
                    {
                        spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor, (float)(3 * Math.PI / 2), new Vector2(rectangleToDraw.Height, 0), (SpriteEffects)spriteEffect, 0);
                        //  Console.WriteLine("Left");
                    }
                    else if (rotationIndex == (int)Rotation.Up)
                    {//0

                        switch (groundTileType)
                        {
                            case TileSource.TileType.Ground:
                                this.rot = 0f;

                                spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor, rot, Vector2.Zero, (SpriteEffects)spriteEffect, 0);
                                break;
                            case TileSource.TileType.Building:
                                spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor, rot, Vector2.Zero, (SpriteEffects)spriteEffect, 0);
                                break;
                            case TileSource.TileType.Fluid:
                                // spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor, rot, Vector2.Zero, (SpriteEffects)spriteEffect, 0);

                                var tempRect = new Rectangle(spriteGameSize.X, spriteGameSize.Y + 15, spriteGameSize.Width, spriteGameSize.Height - 15);
                                float divider = (64f - 15f) / 64f; ;
                                float divider2 = (15f) / 64f; ;
                                var refR = rectangleToDraw;
                                var tempRectSource = new Rectangle(refR.X, refR.Y + (int)(refR.Height * divider2 * 0), refR.Width, (int)(refR.Height * divider));
                                spriteBatch.Draw(shapeTexture, tempRect, tempRectSource, drawColor, 0f, Vector2.Zero, (SpriteEffects)spriteEffect, 0);
                                break;
                            default:
                                break;
                        }

                        //spriteBatch.Draw(Game1.hitboxHelp,new Rectangle(0,0,64,64),Color.Green);
                        // Console.WriteLine("Up");
                    }
                    else
                    {
                        Console.WriteLine("Rotation be no good");
                    }
                }
                else if (baseAnimations.Count != 0)
                {
                    try
                    {
                        if (!waterDraw)
                        {
                            if (index == -1)
                            {
                                baseAnimations[animationIndex].Draw(spriteBatch, this, sColor);
                            }
                            else
                            {
                                baseAnimations[index].Draw(spriteBatch, this, sColor);
                            }
                        }
                        else
                        {
                            if (index == -1)
                            {
                                baseAnimations[animationIndex].DrawForWaterReflecton(spriteBatch, this);
                            }
                            else
                            {
                                baseAnimations[index].DrawForWaterReflecton(spriteBatch, this);
                            }
                        }

                    }
                    catch
                    {
                        Console.WriteLine(":S  @" + animationIndex);
                    }
                }

            }
            else if (!bIsVisible && MapBuilder.bIsRunning)
            {
                if (baseAnimations.Count == 0)
                {
                    //90
                    if (rotationIndex == (int)Rotation.Left)
                    {
                        spriteBatch.Draw(shapeTexture, spriteGameSize, rectangleToDraw, drawColor * .3f, (float)Math.PI / 2, new Vector2(0, rectangleToDraw.Width), (SpriteEffects)spriteEffect, 0);
                        // Console.WriteLine("Right");
                    }//180
                    else if (rotationIndex == (int)Rotation.Down)
                    {
                        spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor * .3f, (float)Math.PI, new Vector2(rectangleToDraw.Width, rectangleToDraw.Height), (SpriteEffects)spriteEffect, 0);
                        //Console.WriteLine("Down");

                    }//270
                    else if (rotationIndex == (int)Rotation.Right)
                    {
                        spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor * .3f, (float)(3 * Math.PI / 2), new Vector2(rectangleToDraw.Height, 0), (SpriteEffects)spriteEffect, 0);
                        //  Console.WriteLine("Left");
                    }
                    else if (rotationIndex == (int)Rotation.Up)
                    {//0

                        spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor * .3f, 0, Vector2.Zero, (SpriteEffects)spriteEffect, 0);
                        // Console.WriteLine("Up");
                    }
                    else
                    {
                        Console.WriteLine("Rotation be no good");
                    }
                }
                else if (baseAnimations.Count != 0)
                {
                    try
                    {
                        if (index == -1)
                        {
                            baseAnimations[animationIndex].Draw(spriteBatch, this, drawColor * .3f);
                        }
                        else
                        {
                            baseAnimations[index].Draw(spriteBatch, this, drawColor * .3f);
                        }
                    }
                    catch
                    {
                        Console.WriteLine(":S  @" + animationIndex);
                    }
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Rectangle destination, Color sColor = new Color(), int index = -1, bool waterDraw = false)
        {
            Color drawColor = Color.White;
            if (sColor != default(Color))
            {
                drawColor = sColor;
            }
            drawColor *= (spriteOpacity / 100f);
            if (bIsVisible)
            {
                if (baseAnimations.Count == 0)
                {
                    //90
                    if (rotationIndex == (int)Rotation.Left)
                    {
                        spriteBatch.Draw(shapeTexture, destination, rectangleToDraw, drawColor, (float)Math.PI / 2, new Vector2(0, rectangleToDraw.Width), (SpriteEffects)spriteEffect, 0);
                        // Console.WriteLine("Right");
                    }//180
                    else if (rotationIndex == (int)Rotation.Down)
                    {
                        spriteBatch.Draw(shapeTexture, (destination), rectangleToDraw, drawColor, (float)Math.PI, new Vector2(rectangleToDraw.Width, rectangleToDraw.Height), (SpriteEffects)spriteEffect, 0);
                        //Console.WriteLine("Down");

                    }//270
                    else if (rotationIndex == (int)Rotation.Right)
                    {
                        spriteBatch.Draw(shapeTexture, (destination), rectangleToDraw, drawColor, (float)(3 * Math.PI / 2), new Vector2(rectangleToDraw.Height, 0), (SpriteEffects)spriteEffect, 0);
                        //  Console.WriteLine("Left");
                    }
                    else if (rotationIndex == (int)Rotation.Up)
                    {//0
                        MovementLogic();
                        switch (groundTileType)
                        {
                            case TileSource.TileType.Ground:
                                spriteBatch.Draw(shapeTexture, (destination), rectangleToDraw, drawColor, rot, Vector2.Zero, (SpriteEffects)spriteEffect, 0);
                                break;
                            case TileSource.TileType.Building:
                                spriteBatch.Draw(shapeTexture, (destination), rectangleToDraw, drawColor, rot, Vector2.Zero, (SpriteEffects)spriteEffect, 0);
                                break;
                            case TileSource.TileType.Fluid:
                                // spriteBatch.Draw(shapeTexture, (spriteGameSize), rectangleToDraw, drawColor, rot, Vector2.Zero, (SpriteEffects)spriteEffect, 0);

                                var tempRect = new Rectangle(destination.X, destination.Y + 15, destination.Width, destination.Height - 15);
                                float divider = (64f - 15f) / 64f; ;
                                float divider2 = (15f) / 64f; ;
                                var refR = rectangleToDraw;
                                var tempRectSource = new Rectangle(refR.X, refR.Y + (int)(refR.Height * divider2 * 0), refR.Width, (int)(refR.Height * divider));
                                spriteBatch.Draw(shapeTexture, tempRect, tempRectSource, drawColor, 0f, Vector2.Zero, (SpriteEffects)spriteEffect, 0);
                                break;
                            default:
                                break;
                        }

                        //spriteBatch.Draw(Game1.hitboxHelp,new Rectangle(0,0,64,64),Color.Green);
                        // Console.WriteLine("Up");
                    }
                    else
                    {
                        Console.WriteLine("Rotation be no good");
                    }
                }
                else if (baseAnimations.Count != 0)
                {
                    try
                    {
                        if (!waterDraw)
                        {
                            if (index == -1)
                            {
                                baseAnimations[animationIndex].Draw(spriteBatch, this, destination, sColor);
                            }
                            else
                            {
                                baseAnimations[index].Draw(spriteBatch, this, destination, sColor);
                            }
                        }
                        else
                        {
                            if (index == -1)
                            {
                                baseAnimations[animationIndex].DrawForWaterReflecton(spriteBatch, this, destination);
                            }
                            else
                            {
                                baseAnimations[index].DrawForWaterReflecton(spriteBatch, this, destination);
                            }
                        }

                    }
                    catch
                    {
                        Console.WriteLine(":S  @" + animationIndex);
                    }
                }

            }
            else if (!bIsVisible && MapBuilder.bIsRunning)
            {
                if (baseAnimations.Count == 0)
                {
                    //90
                    if (rotationIndex == (int)Rotation.Left)
                    {
                        spriteBatch.Draw(shapeTexture, destination, rectangleToDraw, drawColor, (float)Math.PI / 2, new Vector2(0, rectangleToDraw.Width), (SpriteEffects)spriteEffect, 0);
                        // Console.WriteLine("Right");
                    }//180
                    else if (rotationIndex == (int)Rotation.Down)
                    {
                        spriteBatch.Draw(shapeTexture, (destination), rectangleToDraw, drawColor, (float)Math.PI, new Vector2(rectangleToDraw.Width, rectangleToDraw.Height), (SpriteEffects)spriteEffect, 0);
                        //Console.WriteLine("Down");

                    }//270
                    else if (rotationIndex == (int)Rotation.Right)
                    {
                        spriteBatch.Draw(shapeTexture, (destination), rectangleToDraw, drawColor, (float)(3 * Math.PI / 2), new Vector2(rectangleToDraw.Height, 0), (SpriteEffects)spriteEffect, 0);
                        //  Console.WriteLine("Left");
                    }
                    else if (rotationIndex == (int)Rotation.Up)
                    {//0
                        spriteBatch.Draw(shapeTexture, (destination), rectangleToDraw, drawColor, 0, Vector2.Zero, (SpriteEffects)spriteEffect, 0);
                        // Console.WriteLine("Up");
                    }
                    else
                    {
                        Console.WriteLine("Rotation be no good");
                    }
                }
                else if (baseAnimations.Count != 0)
                {
                    try
                    {
                        if (index == -1)
                        {
                            baseAnimations[animationIndex].Draw(spriteBatch, this, destination, drawColor * .3f);
                        }
                        else
                        {
                            baseAnimations[index].Draw(spriteBatch, this, destination, drawColor * .3f);
                        }
                    }
                    catch
                    {
                        Console.WriteLine(":S  @" + animationIndex);
                    }
                }
            }
        }


        public virtual RenderTarget2D getFXRender(SpriteBatch spriteBatch, Color sColor = new Color(), int index = -1)
        {

            spriteBatch.End();

            if (bMustGenerateFX)
            {
                if (fxRender == null)
                {
                    if (spriteGameSize.Size == new Point()) { spriteGameSize.Size = new Point(64); }
                    fxRender = new RenderTarget2D(Game1.graphics.GraphicsDevice, spriteGameSize.Height * 3, spriteGameSize.Height);
                    fxDrawLocation = new Rectangle(spriteGameSize.Height, 0, spriteGameSize.Width, spriteGameSize.Height);
                }
                if (this.GetType() == typeof(Characters.BaseCharacter))
                {

                }

                var temp = position;
                position = fxDrawLocation.Location.ToVector2();
                UpdatePositioNonMovement();

                spriteBatch.GraphicsDevice.SetRenderTarget(fxRender);
                spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp);
                DrawFX(spriteBatch, sColor, index);
                spriteBatch.End();
                spriteBatch.GraphicsDevice.SetRenderTarget(null);
                spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);
                position = temp;
                UpdatePositioNonMovement();
                bMustGenerateFX = false;
            }


            return fxRender;
        }

        public override string ToString()
        {
            String toString = "";
            if (!shapeMapName.Equals("", StringComparison.OrdinalIgnoreCase))
            {
                toString += "MapName: " + shapeMapName;
            }
            toString += " Shape name: " + shapeName + " ShapeID: " + shapeID + "; Position Map: " + position / 64 + "; ScriptID: ";
            if (script != null)
            {
                toString += script.identifier;
            }

            return toString;
        }

        public virtual void Interact()
        {
            List<objectInfo> nearestSprites = MapChunk.consideredSprites.FindAll(obj => obj.bHasScript && (obj.mapSize.Intersects(spriteGameSize) || obj.mapSize.Contains(spriteGameSize)));
            foreach (var item in nearestSprites)
            {
                BaseSprite nearestSprite = null;
                Object objectToPass = null;
                if (item.obj.GetType() == typeof(BaseSprite))
                {
                    nearestSprite = item.obj as BaseSprite;
                    objectToPass = nearestSprite;

                    if (nearestSprite != null && nearestSprite.bIsActive)
                    {
                        if (!ScriptProcessor.bIsRunning)
                        {
                            try
                            {
                                if (nearestSprite.script.scriptContent[0] == "@AST")
                                {
                                    GameProcessor.bGameUpdateIsPaused = true;
                                    nearestSprite.script.scriptLineIndex = 0;
                                    ScriptProcessor.ChangeActiveScript(nearestSprite.script, objectToPass, PlayerController.selectedSprite, true);
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Error in Interact() BaseSprite");
                                GameProcessor.bGameUpdateIsPaused = false;
                            }
                        }
                    }
                }
                else if (item.obj.GetType() == typeof(Characters.BaseCharacter))
                {
                    nearestSprite = item.obj as BaseSprite;
                    objectToPass = nearestSprite;

                    if (nearestSprite != null && nearestSprite.bIsActive)
                    {
                        if (!ScriptProcessor.bIsRunning)
                        {
                            try
                            {
                                if (nearestSprite.script.scriptContent[0] == "@AST")
                                {
                                    GameProcessor.bGameUpdateIsPaused = true;
                                    nearestSprite.script.scriptLineIndex = 0;
                                    ScriptProcessor.ChangeActiveScript(nearestSprite.script, objectToPass, PlayerController.selectedSprite, true);
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Error in Interact() BaseSprite");
                                GameProcessor.bGameUpdateIsPaused = false;
                            }
                        }
                    }
                }
                else if (item.obj.GetType() == typeof(NPC))
                {
                    nearestSprite = (item.obj as NPC).baseCharacter;
                    objectToPass = item.obj as NPC;
                    var Script = (objectToPass as NPC).GetScript();

                    if (Script != null && !ScriptProcessor.bIsRunning)
                    {
                        try
                        {
                            if (Script.scriptContent[0] == "@AST")
                            {
                                GameProcessor.bGameUpdateIsPaused = true;
                                Script.scriptLineIndex = 0;
                                ScriptProcessor.ChangeActiveScript(Script, objectToPass, PlayerController.selectedSprite, true);
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Error in Interact() BaseSprite");
                            GameProcessor.bGameUpdateIsPaused = false;
                        }
                    }
                }
            }




        }

        public BaseSprite ShallowCopy()
        {
            BaseSprite temp = (BaseSprite)this.MemberwiseClone();
            temp.baseAnimations = new List<ShapeAnimation>();
            foreach (var item in baseAnimations)
            {
                temp.baseAnimations.Add(item.Clone());
            }
            //  baseAnimations.ForEach(a => temp.baseAnimations.Add(a.Clone()));
            temp.shapeHitBox = new List<List<Rectangle>>(shapeHitBox);
            return temp;
        }

        public bool IsOnTop(BaseSprite sprite)
        {
            if (sprite != null && sprite == this)
            {
                return false;
            }


            if (sprite != null && GameProcessor.bIsInGame && (trueMapSize().Contains(sprite.trueMapSize()) || trueMapSize().Intersects(sprite.trueMapSize())))
            {
                //if (spriteGameSize.Y > sprite.spriteGameSize.Y && spriteGameSize.Height + spriteGameSize.Y < sprite.spriteGameSize.Height + sprite.spriteGameSize.Y)
                if (spriteGameSize.Y * scaleVector.Y > sprite.spriteGameSize.Y * sprite.scaleVector.Y && getHeightIndicator() < sprite.getHeightIndicator())
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public Vector2 positionToMapCoords()
        {
            return (position / 64).ToPoint().ToVector2();
        }

        public void MovementLogic()
        {
            CheckGroundTile();
        }

        public void CheckGroundTile()
        {
            var map = GameProcessor.loadedMap;
            if (!GameProcessor.bIsInGame)
            {
                map = MapBuilder.loadedMap;
            }
            groundTileType = TileSource.TileType.Ground;
            var tempListChunks = map.tilesOnPositionSpriteForWater(trueMapSize());
            foreach (var item in tempListChunks)
            {
                if (item.tileSource.tileType == TileSource.TileType.Fluid)
                {
                    groundTileType = TileSource.TileType.Fluid;
                    groundTile = item;
                    break;
                }
                else
                {
                    groundTile = item;
                }
            }

            if (tempListChunks.Count == 0)
            {
                groundTileType = TileSource.TileType.Ground;
                groundTile = new BasicTile();
            }

            //tempListChunks = map.tilesOnPositionSpriteForGround(trueMapSize());
            //foreach (var item in tempListChunks)
            //{
            //    if (item.tileSource.tileType == TileSource.TileType.Stairs)
            //    {
            //        groundTileType = TileSource.TileType.Stairs;
            //        groundTile = item;
            //        //    Console.WriteLine("Stairs");
            //        break;
            //    }
            //}

        }

        public virtual bool Contains(Rectangle r)
        {
            if (trueMapSize().Contains(r) || trueMapSize().Intersects(r))
            {
                return true;
            }
            return false;
        }

        internal virtual Rectangle trueMapSize()
        {

            if ((spriteGameSize.Size.ToVector2() * scaleVector) != (MapSize.Size.ToVector2()))
            {
                bRecalculateTrueMapSize = true;
            }
            if (bRecalculateTrueMapSize)
            {
                var temp = (spriteGameSize.Size.ToVector2() * scaleVector).ToPoint();
                //Console.WriteLine(spriteGameSize.Location);
                MapSize = new Rectangle(spriteGameSize.Location, (spriteGameSize.Size.ToVector2() * scaleVector).ToPoint());
                bRecalculateTrueMapSize = false;
            }

            return MapSize;
        }

        internal int getHeightIndicator()
        {
            return (int)(spriteGameSize.Y + (spriteGameSize.Height) * scaleVector.Y);
        }

        internal void AssignScaleVector(Vector2 vectorScale)
        {
            this.scaleVector = vectorScale;
            UpdateHitBoxes();
            bRecalculateTrueMapSize = true;
            trueMapSize();
        }

        internal void AssignScaleVectorGroupItems(ObjectGroup og)
        {
            UpdateHitBoxes(og);
            bRecalculateTrueMapSize = true;
            trueMapSize();
            //scaleVector = og.scaleVector;
        }

        public LUA.LuaPoint LUAPointPos()
        {
            var temp = new LUA.LuaPoint(positionToMapCoords().X, positionToMapCoords().Y);
            return temp;
        }

        internal virtual bool UpdateAnimInfo()
        {
            bool t = animInfo.MustUpdate(this);
            if (t) { animInfo = new ShapeAnimationInfo(this); }
            bMustGenerateFX = t;
            return t;
        }
    }

    internal struct ShapeAnimationInfo
    {
        int ID;
        int frame;
        int rotation;

        internal ShapeAnimationInfo(BaseSprite bs)
        {
            this.ID = bs.animationIndex;

            if (bs.GetType() == typeof(BaseSprite))
            {
                frame = bs.baseAnimations[ID].frameIndex;
            }
            else if (bs.GetType() == typeof(BaseCharacter))
            {
                frame = (bs as BaseCharacter).charAnimations[ID].frameIndex;
            }
            else
            {
                frame = -1;
            }


            rotation = bs.rotationIndex;
        }

        internal ShapeAnimationInfo(int ID, int frame, int rotation)
        {
            this.ID = ID;
            this.frame = frame;
            this.rotation = rotation;
        }

        internal bool MustUpdate(BaseSprite bs)
        {

            if (bs.animationIndex != ID)
            {
                return true;
            }

            if (bs.rotationIndex != rotation)
            {
                return true;
            }

            if (bs.GetType() == typeof(BaseSprite))
            {
                if (bs.baseAnimations[ID].frameIndex != frame)
                {
                    return true;
                }
            }
            else if (bs.GetType() == typeof(BaseCharacter))
            {
                if ((bs as BaseCharacter).charAnimations[ID].frameIndex != frame)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
