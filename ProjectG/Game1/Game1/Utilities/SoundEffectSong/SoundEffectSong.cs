using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBAGW.Utilities;

namespace TBAGW
{
    internal class SoundEffectSong
    {
        static internal List<SoundEffectSong> loadedSoundEffectSongs = new List<SoundEffectSong>();
        static internal List<SoundEffectSong> soundEffectSongs = new List<SoundEffectSong>();

        float startVolume = 0f;
        int deltaVolume = 0;
        int targetVolume = 100;
        int timeToVolume = (0);//ms
        int timePassed = 0;
        internal int timeSpendPlaying = 0;

        internal SoundEffectInstance parent;
        internal SoundEffect parentSE;


        internal SoundEffectSong(SoundEffect se = null, bool bLoop = true, bool bRemoveRemainingSoundEffectSongs = false, SoundEffectSong disposable = null)
        {
            if (bRemoveRemainingSoundEffectSongs) { ClearSongs(); }
            parent = se.CreateInstance();
            parent.Volume *= SceneUtility.masterVolume * SceneUtility.musicVolume / 100f / 100f;
            parent.IsLooped = bLoop;
            parentSE = se;
            if (disposable != null)
            {
                disposable.parent.Stop();
                disposable.parent.Dispose();

            }

        }

        internal static void ClearSongs()
        {
            for (int i = 0; i < soundEffectSongs.Count; i++)
            {
                soundEffectSongs[i].SetVolume(0);
                soundEffectSongs[i].Dispose();
            }

            soundEffectSongs.Clear();
        }

        internal static void End()
        {
            for (int i = 0; i < soundEffectSongs.Count; i++)
            {
                soundEffectSongs[i].parent.Stop();
                soundEffectSongs[i].parent.Dispose();
                soundEffectSongs[i].parentSE.Dispose();
            }
        }

        internal static void Update(GameTime gt)
        {
            for (int i = 0; i < soundEffectSongs.Count; i++)
            {

                //soundEffectSongs[i].timeSpendPlaying += gt.ElapsedGameTime.Milliseconds;
                //soundEffectSongs[i].timePassed += gt.ElapsedGameTime.Milliseconds;
                if (soundEffectSongs[i].timePassed < soundEffectSongs[i].timeToVolume)
                {
                    soundEffectSongs[i].timePassed += gt.ElapsedGameTime.Milliseconds;

                    SoundEffectSong temp = soundEffectSongs[i];
                    if (temp.timePassed > temp.timeToVolume)
                    {
                        temp.timePassed = temp.timeToVolume;
                    }
                    if (temp.timeToVolume != 0)
                    {
                        var tempf = temp.startVolume + ((float)temp.deltaVolume * ((float)temp.timePassed / (float)temp.timeToVolume) / 100f);
                        if (tempf<0) { tempf = 0; }else if (tempf > 1) { tempf = 1f; }
                        temp.parent.Volume = tempf;
                        temp.parent.Volume *= SceneUtility.masterVolume * SceneUtility.musicVolume / 100f / 100f;
                    }
                    else
                    {
                        temp.SetVolume(temp.targetVolume);
                    }
                    if (temp.parent.State != SoundState.Playing)
                    {
                        temp.parent.Play();
                    }

                }
            }
        }

        internal void SetFade(int v, int vt)
        {
            
            parent.Volume = ((float)targetVolume / 100f) * SceneUtility.masterVolume * SceneUtility.musicVolume / 100f / 100f;
            startVolume = parent.Volume;
            deltaVolume = v - (int)(parent.Volume * 100);
            targetVolume = v;
            timeToVolume = vt;
            timePassed = 0;
        }

        internal void SetVolume(int v)
        {
            parent.Volume = (float)v / 100f;
            parent.Volume *= (float)SceneUtility.masterVolume * (float)SceneUtility.musicVolume / 100f / 100f;
            timePassed = 0;
            timeToVolume = 0;
            targetVolume = v;
        }

        internal void Dispose()
        {
            parent.Stop(true);
            //parent.Dispose();
        }

        internal void Start()
        {
            parent.Play();
        }

        static internal void Start(SoundEffectSong SES)
        {
            soundEffectSongs.Add(SES);
            soundEffectSongs.Last().SetFade(100, 2000);
            soundEffectSongs.Last().Start();
        }

        internal static bool IsPlaying(SoundEffectSong currentLayer)
        {
            return soundEffectSongs.Contains(currentLayer);
        }

        ~SoundEffectSong()
        {
            if (parent!=null&&!parent.IsDisposed&&!parentSE.IsDisposed)
            {
                if (parent.State == SoundState.Playing)
                {
                    parent.Stop();
                }
            }
        }
    }

    internal class LayeredSong
    {
        SoundEffectSong[] sList;

        internal LayeredSong(params SoundEffectSong[] songs)
        {
            sList = songs;
        }

        internal SoundEffectSong SwitchLayer(int index, int v, int vt)
        {
            if (sList.Length > index && index >= 0)
            {
                sList[index].SetFade(v, vt);
                return sList[index];
            }
            return null;
        }

        internal void Restart()
        {
            for (int i = 0; i < sList.Length; i++)
            {
                sList[i] = new SoundEffectSong(sList[i].parentSE, sList[i].parent.IsLooped, false, sList[i]);
            }
        }

        internal SoundEffectSong StartPlay()
        {
            Restart();
            if (sList.Length != 0)
            {
                sList[0].SetVolume(100);
            }
            for (int i = 1; i < sList.Length; i++)
            {
                sList[i].SetVolume(0);
            }

            for (int i = 0; i < sList.Length; i++)
            {
                sList[i].parent.Play();
                SoundEffectSong.soundEffectSongs.Add(sList[i]);
            }

            if (sList.Length != 0)
            {
                return sList[0];
            }
            return null;
        }
    }
}
