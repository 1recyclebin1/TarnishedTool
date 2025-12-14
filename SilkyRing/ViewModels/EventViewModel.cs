using System.Windows.Input;
using System.Windows.Media;
using SilkyRing.Core;
using SilkyRing.Enums;
using SilkyRing.Interfaces;
using SilkyRing.Services;

namespace SilkyRing.ViewModels
{
    public class EventViewModel : BaseViewModel
    {
        private readonly IEventService _eventService;

        public EventViewModel(IEventService eventService, IStateService stateService)
        {
            _eventService = eventService;


            stateService.Subscribe(State.Loaded, OnGameLoaded);
            stateService.Subscribe(State.NotLoaded, OnGameNotLoaded);

            SetEventCommand = new DelegateCommand(SetEvent);
            GetEventCommand = new DelegateCommand(GetEvent);
        }
        
        #region Commands
        
        public ICommand SetEventCommand { get; set; }
        public ICommand GetEventCommand { get; set; }

        #endregion

        #region Properties
        
        private bool _areOptionsEnabled;
        
        public bool AreOptionsEnabled
        {
            get => _areOptionsEnabled;
            set => SetProperty(ref _areOptionsEnabled, value);
        }

        private string _setFlagId;

        public string SetFlagId
        {
            get => _setFlagId;
            set => SetProperty(ref _setFlagId, value);
        }

        private int _flagStateIndex;

        public int FlagStateIndex
        {
            get => _flagStateIndex;
            set => SetProperty(ref _flagStateIndex, value);
        }
        

        private string _getFlagId;

        public string GetFlagId
        {
            get => _getFlagId;
            set => SetProperty(ref _getFlagId, value);
        }

        private string _eventStatusText;

        public string EventStatusText
        {
            get => _eventStatusText;
            set => SetProperty(ref _eventStatusText, value);
        }

        private Brush _eventStatusColor;

        public Brush EventStatusColor
        {
            get => _eventStatusColor;
            set => SetProperty(ref _eventStatusColor, value);
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
        
        private void SetEvent()
        {
            if (string.IsNullOrWhiteSpace(SetFlagId))
                return;

            string trimmedFlagId = SetFlagId.Trim();

            if (!long.TryParse(trimmedFlagId, out long flagIdValue) || flagIdValue <= 0)
                return;

            _eventService.SetEvent(flagIdValue, FlagStateIndex == 0);
        }
        
        private void GetEvent()
        {
            if (string.IsNullOrWhiteSpace(GetFlagId))
                return;

            string trimmedFlagId = GetFlagId.Trim();

            if (!long.TryParse(trimmedFlagId, out long flagIdValue) || flagIdValue <= 0)
                return;

            if (_eventService.GetEvent(flagIdValue))
            {
                EventStatusText = "True";
                EventStatusColor = Brushes.Chartreuse;
            }
            else
            {
                EventStatusText = "False";
                EventStatusColor = Brushes.Red;
            }
        }

        #endregion
    }
}