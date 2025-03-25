using System;

namespace Game.State.Root
{
    // Настройки игры (звук, графика, управление и т.д.)
    [Serializable]
    public class GameSettingsState
    {
        public int MusicVolume;
        public int SFXVolume;
    }
}
