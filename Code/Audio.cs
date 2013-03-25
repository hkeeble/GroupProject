using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace VOID
{ 
    class Audio
    {
        private static AudioEngine audioEngine;
        private static SoundBank soundBank;
        private static WaveBank waveBank;

        public static void Initialize()
        {
            audioEngine = new AudioEngine("GroupProjectSounds.xsb");

            soundBank = new SoundBank(audioEngine, "Sound Bank.xsb");
            waveBank = new WaveBank(audioEngine, "Wave Bank.xwb");
        }

        public static void Play()
        {
            Cue wildBattle = soundBank.GetCue("CreatureFight");
            Cue bossBattle = soundBank.GetCue("BossFight");
            Cue labEnter = soundBank.GetCue("Lab");

            wildBattle.Play();
            bossBattle.Play();
            labEnter.Play();
        }

        public static void Stop()
        {
                
        }

    }
}
