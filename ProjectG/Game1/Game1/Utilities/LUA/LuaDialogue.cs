using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities.Characters;

namespace LUA
{
    public class LuaDialogue:LuaCommand
    {
        public List<LuaText> text = new List<LuaText>();
        public String leftChar = "";
        public String rightChar = "";
        public int speaker = 0; // 0 is Left, 1 is right

        public LuaCharacterInfo lCharInfo = new LuaCharacterInfo();
        public LuaCharacterInfo rCharInfo = new LuaCharacterInfo();

        internal BaseCharacter lbc = null;
        internal BaseCharacter rbc = null;
        internal bool bInitialize = true;
        internal BaseCharacter speakerbc = null;

        public void Initialize(String lbc, String rbc, int s)
        {
            bInitialize = true;
            this.leftChar = lbc;
            this.rightChar = rbc;
            speaker = s;
            Initialize();
        }

        public void Initialize()
        {
            if (bInitialize)
            {
                bInitialize = false;
                lbc = TBAGW.GameProcessor.gcDB.gameCharacters.Find(b=>b.IsName(leftChar));
                rbc = TBAGW.GameProcessor.gcDB.gameCharacters.Find(b => b.IsName(rightChar));
                speakerbc = speaker == 0 ? lbc : rbc;
                lCharInfo = lbc == null ? new LuaCharacterInfo() : lbc.toCharInfo();
                rCharInfo = rbc == null ? new LuaCharacterInfo() : rbc.toCharInfo();
            }
        }

        public LuaDialogue(String Text)
        {
            text.Add(new LuaText());
            this.cType = CommandType.Dialogue;
            text.Last().text = Text;
        }

        public LuaText getDialogue(TBAGW.GameText.Language l)
        {
            if (TBAGW.Utilities.SriptProcessing.ScriptProcessor.currentDisplayMode != (int)TBAGW.Utilities.SriptProcessing.ScriptProcessor.ActiveScriptDisplayMode.None)
            {
                Console.WriteLine("Overriding ScriptProcessor displayMode");
            }
            TBAGW.Utilities.SriptProcessing.ScriptProcessor.currentDisplayMode = (int)TBAGW.Utilities.SriptProcessing.ScriptProcessor.ActiveScriptDisplayMode.None;
            if (TBAGW.GameScreenEffect.currentEffect!=TBAGW.GameScreenEffect.Effects.Conversation)
            {
                TBAGW.GameScreenEffect.InitializeConversationEffect();
            }
           
            var temp = text.Find(t=>t.language == l);
            if (temp == null)
            {
                return text.First();
            }else
            {
                return temp;
            }
        }
    }

    public class LuaText
    {
        public String text = "";
        public TBAGW.GameText.Language language = TBAGW.GameText.Language.English;
    }

    public class LuaNameChange : LuaCommand
    {
        public String nameChange = "";
        public String nameBefore = "";
        public LuaCharacterInfo lci = new LuaCharacterInfo();

        public LuaNameChange(LuaCharacterInfo lci, String name)
        {
            this.lci = lci;
            nameChange = name;
            nameBefore = lci.dialogueName;
        }

        public void ChangeName()
        {
            lci.dialogueName = nameChange;
            bIsDone = true;
        }

    }
}
