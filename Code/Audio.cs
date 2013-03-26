using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace VOiD
{ 
    class Audio
    {
        private static AudioEngine audioEngine;
        private static SoundBank soundBank;
        private static WaveBank waveBank;

        private static List<Cue> soundsPlaying;

        public static void Initialize()
        {
            audioEngine = new AudioEngine("Content/GroupProjectSounds.xgs");
            soundBank = new SoundBank(audioEngine, "Content/Sound Bank.xsb");
            waveBank = new WaveBank(audioEngine, "Content/Wave Bank.xwb");
            soundsPlaying = new List<Cue>();
        }

        public static void Play(string cueName)
        {
            Cue cue = soundBank.GetCue(cueName);
            cue.Play();
            soundsPlaying.Add(cue);
        }

        public static void Stop(string cueName)
        {
            for(int i = 0; i < soundsPlaying.Count; i++)
            {
                if (soundsPlaying[i].Name == cueName)
                {
                    soundsPlaying[i].Stop(AudioStopOptions.Immediate);
                    soundsPlaying.Remove(soundsPlaying[i]);
                }
            }
        }

        public static void StopAll()
        {
            for(int i = 0; i < soundsPlaying.Count; i++)
            {
                soundsPlaying[i].Stop(AudioStopOptions.Immediate);
                soundsPlaying.Remove(soundsPlaying[i]);
            }
        }

        public static bool IsPlaying(string cueName)
        {
            foreach(Cue c in soundsPlaying)
                if (c.Name == cueName)
                {
                    if (c.IsPlaying)
                        return true;
                    else
                        return false;
                }
            return false;
        }
    }
}
