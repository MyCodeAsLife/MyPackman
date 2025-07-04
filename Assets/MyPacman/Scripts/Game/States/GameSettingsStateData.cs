using System;

namespace MyPacman
{
    [Serializable]
    public class GameSettingsStateData
    {
        public int MusicVolume { get; set; }
        public int SFXVolume { get; set; }
        public int AutoSaveTimer { get; set; }

        public int HigthScore { get; set; }
    }
}