using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.Sound.BG
{
    static class MenuSongAssestLoader
    {

        static public Song MMSong;
        static public Song OptionsSong;
        static public Song BattleSongOminous;

        static public void Initialize(Game1 game)
        {
            MMSong = game.Content.Load<Song>(@"Sound\BG\Calmant");
            OptionsSong = game.Content.Load<Song>(@"Sound\BG\Universal");
            BattleSongOminous = game.Content.Load<Song>(@"Sound\BG\All This");
        }


    }
}
