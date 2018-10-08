using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBAGW.Utilities.Sound.BG
{
    static class MusicBGPlayer
    {
        static internal Song currentBGSong = default(Song);

        static public void Initialize(Game1 game)
        {
            //MenuSongAssestLoader.Initialize(game);

        }

        static public void Start(Song newSong)
        {
            MediaPlayer.Stop();

            if(default(Song)==(currentBGSong)||!currentBGSong.Equals(newSong)){
                currentBGSong = newSong;
                MediaPlayer.Play(currentBGSong);
            }
            else
            {
                MediaPlayer.Play(currentBGSong);
            }
        }

        static public void Repeat(bool bTemp = true)
        {

            if(bTemp){
                MediaPlayer.IsRepeating = true;
            }
            else
            {
                MediaPlayer.IsRepeating = false;
            }
        }

        static public void Update(GameTime gameTime)
        {
            if (Game1.bIsActive)
            {
                MediaPlayer.Resume();
            }else{
                MediaPlayer.Pause();
            }
        }

    }
}
