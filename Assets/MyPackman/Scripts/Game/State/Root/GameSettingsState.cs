using System;

namespace Game.State.Root
{
    // Настройки игры (звук, графика, управление и т.д.)
    [Serializable]
    public class GameSettingsState
    {
        public int MusicValue;
        public int SFXVolume;
    }
}
