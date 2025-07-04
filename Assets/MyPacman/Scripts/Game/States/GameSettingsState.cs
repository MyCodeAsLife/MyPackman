using R3;

namespace MyPacman
{
    public class GameSettingsState
    {
        public readonly ReactiveProperty<int> MusicVolume;
        public readonly ReactiveProperty<int> SFXVolume;
        public readonly ReactiveProperty<int> AutoSaveTimer;
        public readonly ReactiveProperty<int> HigthScore;

        public GameSettingsState(GameSettingsStateData gameSettingsStateData)
        {
            MusicVolume = new ReactiveProperty<int>(gameSettingsStateData.MusicVolume);
            SFXVolume = new ReactiveProperty<int>(gameSettingsStateData.SFXVolume);
            AutoSaveTimer = new ReactiveProperty<int>(gameSettingsStateData.AutoSaveTimer);
            HigthScore = new ReactiveProperty<int>(gameSettingsStateData.HigthScore);

            MusicVolume.Skip(1).Subscribe(value => gameSettingsStateData.MusicVolume = value);
            SFXVolume.Skip(1).Subscribe(value => gameSettingsStateData.SFXVolume = value);
            AutoSaveTimer.Skip(1).Subscribe(value => gameSettingsStateData.AutoSaveTimer = value);
            HigthScore.Skip(1).Subscribe(value => gameSettingsStateData.HigthScore = value);
        }
    }
}