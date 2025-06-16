using R3;

namespace MyPacman
{
    public class GameSettingsState
    {
        public GameSettingsState(GameSettingsStateData gameSettingsStateData)
        {
            MusicVolume = new ReactiveProperty<int>(gameSettingsStateData.MusicVolume);
            SFXVolume = new ReactiveProperty<int>(gameSettingsStateData.SFXVolume);

            // Skip - Для того чтобы при подписке не "получить" значение.
            MusicVolume.Skip(1).Subscribe(value => gameSettingsStateData.MusicVolume = value);  // Подписка на изменение оригинала, при изменении Proxy
            SFXVolume.Skip(1).Subscribe(value => gameSettingsStateData.SFXVolume = value);      // Подписка на изменение оригинала, при изменении Proxy
        }

        public ReactiveProperty<int> MusicVolume { get; }
        public ReactiveProperty<int> SFXVolume { get; }
    }
}