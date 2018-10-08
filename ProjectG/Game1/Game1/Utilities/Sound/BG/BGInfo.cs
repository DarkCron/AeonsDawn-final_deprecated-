using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TBAGW
{
    [XmlRoot("BG Info")]
    public class BGInfo
    {
        [XmlElement("Song Engine Location")]
        public String songELoc = "";
        [XmlElement("Song SoundBank Location")]
        public String songSBLoc = "";
        [XmlElement("Song WaveBank Location")]
        public String songWBLoc = "";
        [XmlElement("Song identifier")]
        public int songCollectionID = 0;
        [XmlElement("Song name")]
        public String songCollectionName = "Default name";
        [XmlArrayItem("song names")]
        public List<String> songNames = new List<String>();
        [XmlArrayItem("song ID")]
        public List<int> songIDs = new List<int>();

        [XmlIgnore]
        public AudioEngine audioEngine;
        [XmlIgnore]
        public SoundBank soundBank;
        [XmlIgnore]
        public WaveBank waveBank;
        [XmlIgnore]
        public List<Cue> activeCues = new List<Cue>();

        public BGInfo() {

        }

        public void ReloadContent()
        {
            audioEngine = new AudioEngine(Path.Combine("Content/",songELoc));
            soundBank = new SoundBank(audioEngine, Path.Combine("Content/", songSBLoc));
            waveBank = new WaveBank(audioEngine, Path.Combine("Content/", songWBLoc));
        }

        public void Dispose() {
            audioEngine.Dispose();
            soundBank.Dispose();
            waveBank.Dispose();
        }

        public void SEPlay(int i) {
            soundBank.GetCue(songNames[i]).Play();
            activeCues.Add(soundBank.GetCue(songNames[i]));
        }

        public void StopAllActiveCues() {
            foreach (var item in activeCues)
            {
                item.Stop(AudioStopOptions.AsAuthored);
            }

            activeCues.Clear();
        }

        public override string ToString()
        {
            return songCollectionName + ", ID: " + songCollectionID;
        }
    }
}
