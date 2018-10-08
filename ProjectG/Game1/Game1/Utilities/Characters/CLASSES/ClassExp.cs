using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TBAGW
{
    [XmlRoot("Class experience holder")]
    public class ClassExp
    {
        [XmlElement("Total Experience")]
        public int totalExp = 0;
        [XmlElement("Class level")]
        public int classLevel = 0;
        [XmlElement("Exp lua loc")]
        public String loc = "";
        [XmlElement("Exp check function")]
        public String funcName = "";

        int levelDiff = 0;
        float expMod = 1.3f;
        bool bHasProperLua = false;
        int baseExpRea = 200;
        NLua.Lua expScript;
        [XmlIgnore]
        public int leftOverEXP = 0;
        [XmlIgnore]
        public int expTillNextLevel = 0;
        [XmlIgnore]
        public bool bLevelledUp = false;
        [XmlIgnore]
        BaseClass baseClass;

        public ClassExp() { }

        public void Reload(BaseClass bc)
        {
            baseClass = bc;
        }

        public void AddExp(int amount)
        {
            
            totalExp += amount;

            int tempLevel = classLevel;
            CalculateResetExpAndLevels();

            if (classLevel != tempLevel)
            {
                bLevelledUp = true;
                levelDiff = classLevel - tempLevel;
               // baseClass.classLevel++;
            }

        }

        public void CalculateResetExpAndLevels()
        {
            CheckLuaScript();

            int requiredExp = 0;
            int previouslyRequired = 0;


            if (bHasProperLua)
            {
                for (int i = 1; i < classLevel + 2; i++)
                {
                    try
                    {
                        int tempCount = (int)((double)(((expScript[funcName] as NLua.LuaFunction).Call(i))[0]));
                        requiredExp += tempCount;

                        if (i <= classLevel)
                        {
                            previouslyRequired += tempCount;
                        }
                    }
                    catch (Exception)
                    {
                        bHasProperLua = false;
                    }
                }
            }

            if (!bHasProperLua)
            {
                requiredExp += 200;
                for (int i = 1; i < classLevel + 1; i++)
                {
                    int tempCount = (int)(baseExpRea * expMod * (i )+1);
                    requiredExp += tempCount;
                    if (i <= classLevel)
                    {
                        previouslyRequired += tempCount;
                    }
                }
            }

            if (totalExp > requiredExp)
            {
                classLevel++;

                //CalculateResetExpAndLevels();
            }
            else
            {
                expTillNextLevel = requiredExp - totalExp;
                leftOverEXP = totalExp - previouslyRequired;
            }

            //int temp = 0;
            //int counter = 0;
            //if (requiredExp.Count == 0)
            //{
            //    requiredExp.Add(0);
            //}

            //while (requiredExp[0] != 0 && temp < totalExp && counter < requiredExp.Count)
            //{
            //    temp += requiredExp[counter];
            //    counter++;
            //    try
            //    {
            //        if (temp >= totalExp)
            //        {
            //            expTillNextLevel = temp - totalExp;
            //            leftOverEXP = totalExp - (temp - requiredExp[counter - 1]);
            //            classLevel = counter - 1;
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        leftOverEXP = totalExp;
            //        expTillNextLevel = requiredExp[0] - totalExp;
            //        classLevel = counter;
            //    }

            //}
        }

        private void CheckLuaScript()
        {
            if (expScript == null && !loc.Equals("") && !funcName.Equals(""))
            {
                try
                {
                    expScript = new NLua.Lua();
                    expScript.LoadCLRPackage();
                    expScript.DoFile(Game1.rootContent + loc);

                    bHasProperLua = expScript.GetFunction(funcName) != null;
                }
                catch (Exception e)
                {
                    bHasProperLua = false;
                    expScript = null;
                    if (Game1.bIsDebug)
                    {
                        Console.WriteLine(e);
                        //throw e;
                    }
                }
            }
            else if (loc.Equals(""))
            {
                bHasProperLua = false;
                expScript = null;
            }
            else if(!bHasProperLua)
            {
                expScript = null;
                bHasProperLua = false;
            }
        }

        public ClassExp Clone()
        {
            ClassExp temp = new ClassExp();
            temp = (ClassExp)this.MemberwiseClone();
            //temp.requiredExp = new List<int>(requiredExp);
            return temp;
        }

        public int ExpRequirementCurrentLevel()
        {
            if (bHasProperLua)
            {
                try
                {
                    return (int)((double)(((expScript[funcName] as NLua.LuaFunction).Call(classLevel+1))[0]));
                }
                catch (Exception)
                {
                    bHasProperLua = false;
                }

            }

            if (classLevel == 0)
            {
                return baseExpRea;
            }

            return (int)(baseExpRea * expMod * (classLevel )+1);
        }

        public int ExpRequirementLevel(int level)
        {
            if (bHasProperLua)
            {
                try
                {
                    return (int)((double)(((expScript[funcName] as NLua.LuaFunction).Call(level))[0]));
                }
                catch (Exception e)
                {
                    bHasProperLua = false;
                }

            }

            if (level == 1)
            {
                return baseExpRea;
            }
            else if (level == 0)
            {
                return 0;
            }

            int i = (int)((float)baseExpRea * expMod * (float)(level-1))+1;
            return i;
        }

        public NLua.Lua getLevelScript()
        {
            CheckLuaScript();
            if (!bHasProperLua)
            {
                return null;
            }else
            {
                return expScript;
            }
        }
    }
}
