using Game.Services;
using ObservableCollections;

namespace Game.Gameplay.View
{
    public class WorldGameplayRootViewModel
    {
        public readonly IObservableCollection<BuildingViewModel> AllBuildings;

        public WorldGameplayRootViewModel(BuildingsService buildingsService)
        {
            AllBuildings = buildingsService.AllBuildings;
        }
    }
}
