using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace TBAGW
{
    public static class SFXProcessor
    {
        static public List<SFXHelp> util = new List<SFXHelp>();

        static public SFXHelp GenerateAndIDSFX(SoundEffect SFX, bool isLooped = false)
        {
            int tempID = 0;
            while(util.Find(u=>u.ID == tempID)==default(SFXHelp))
            {
                tempID++;
            }

            util.Add(new SFXHelp(SFX.CreateInstance(),tempID, isLooped));


            return util[util.Count-1];
        }

        static public bool PlayedSFXBefore(SFXHelp sfxh)
        {
            if(util.Find(u=>u.ID==sfxh.ID)!=default(SFXHelp))
            {
                return true;
            }else
            {
                return false;
            }
        }


    }

    public class SFXHelp
    {
        public SoundEffectInstance sfxI;
        public int ID = 0;

        public SFXHelp(SoundEffectInstance sei, int ID, bool isLooped = false)
        {
            sfxI = sei;
            this.ID = ID;
            sfxI.IsLooped = isLooped;
        }
    }
}
