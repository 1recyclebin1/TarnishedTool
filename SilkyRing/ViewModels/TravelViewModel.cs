using System.Windows.Input;
using SilkyRing.Core;
using SilkyRing.Enums;
using SilkyRing.Interfaces;
using SilkyRing.Models;
using SilkyRing.Services;
using SilkyRing.Utilities;

namespace SilkyRing.ViewModels
{
    public class TravelViewModel : BaseViewModel
    {
        private readonly ITravelService _travelService;
        private readonly IEventService _eventService;

        public SearchableGroupedCollection<string, Grace> Graces { get; }
        public SearchableGroupedCollection<string, BossWarp> Bosses { get; }

        public TravelViewModel(ITravelService travelService, IEventService eventService, IStateService stateService)
        {
            _travelService = travelService;
            _eventService = eventService;

            stateService.Subscribe(State.Loaded, OnGameLoaded);
            stateService.Subscribe(State.NotLoaded, OnGameNotLoaded);
            
            Graces = new SearchableGroupedCollection<string, Grace>(
                DataLoader.GetGraces(),
                (grace, search) => grace.Name.ToLower().Contains(search) ||
                                   grace.MainArea.ToLower().Contains(search));
            Bosses = new SearchableGroupedCollection<string, BossWarp>(
                DataLoader.GetBossWarps(),
                (bossWarp, search) => bossWarp.Name.ToLower().Contains(search) ||
                                      bossWarp.MainArea.ToLower().Contains(search));

            WarpCommand = new DelegateCommand(Warp);
            UnlockMainGameGracesCommand = new DelegateCommand(UnlockMainGameGraces);
            UnlockDlcGracesCommand = new DelegateCommand(UnlockDlcGraces);
        }

        #region Commands

        public ICommand WarpCommand { get; set; }
        public ICommand UnlockMainGameGracesCommand { get; set; }
        public ICommand UnlockDlcGracesCommand { get; set; }
        
        #endregion

        #region Properties

        private bool _areOptionsEnabled;

        public bool AreOptionsEnabled
        {
            get => _areOptionsEnabled;
            set => SetProperty(ref _areOptionsEnabled, value);
        }

        #endregion


        #region Private Methods
        
        private void OnGameLoaded()
        {
            AreOptionsEnabled = true;
        }

        private void OnGameNotLoaded()
        {
            AreOptionsEnabled = false;
        }

        private void Warp() => _travelService.Warp(Graces.SelectedItem);

        private void UnlockMainGameGraces()
        {
            foreach (var grace in Graces.AllItems)
            {
                if (grace.IsDlc) continue;
                _eventService.SetEvent(grace.FlagId, true);
            }
        }

        private void UnlockDlcGraces()
        {
            foreach (var grace in Graces.AllItems)
            {
                if (!grace.IsDlc) continue;
                _eventService.SetEvent(grace.FlagId, true);
            }
        }

        #endregion

        
    }
}