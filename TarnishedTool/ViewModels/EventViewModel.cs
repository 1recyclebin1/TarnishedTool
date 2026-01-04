using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using TarnishedTool.Core;
using TarnishedTool.Enums;
using TarnishedTool.GameIds;
using TarnishedTool.Interfaces;
using TarnishedTool.Models;
using TarnishedTool.Utilities;

namespace TarnishedTool.ViewModels
{
    public class EventViewModel : BaseViewModel
    {
        private readonly IEventService _eventService;
        private readonly IItemService _itemService;
        private readonly IDlcService _dlcService;
        private readonly IEzStateService _ezStateService;
        private readonly IEmevdService _emevdService;
        private readonly HotkeyManager _hotkeyManager;
        private readonly IUtilityService _utilityService;
        public const int WhetstoneBladeId = 0x4000218E;
        
        private readonly List<int> _baseGameGestureIds; 
        private readonly List<int> _dlcGestureIds; 

        

        public EventViewModel(IEventService eventService, IStateService stateService, IItemService itemService,
            IDlcService dlcService, IEzStateService ezStateService, IEmevdService emevdService,
            HotkeyManager hotkeyManager, IUtilityService utilityService)
        {
            _eventService = eventService;
            _itemService = itemService;
            _dlcService = dlcService;
            _ezStateService = ezStateService;
            _emevdService = emevdService;
            _hotkeyManager = hotkeyManager;
            _utilityService = utilityService;


            stateService.Subscribe(State.Loaded, OnGameLoaded);
            stateService.Subscribe(State.NotLoaded, OnGameNotLoaded);
            stateService.Subscribe(State.FirstLoaded, OnGameFirstLoaded);

            SetEventCommand = new DelegateCommand(SetEvent);
            GetEventCommand = new DelegateCommand(GetEvent);
            UnlockWhetbladesCommand = new DelegateCommand(UnlockWhetblades);
            ClearDlcCommand = new DelegateCommand(ClearDlc);
            UnlockMetyrCommand = new DelegateCommand(UnlockMetyr);
            FightFortissaxCommand = new DelegateCommand(FightFortissax);
            UnlockGesturesCommand = new DelegateCommand(UnlockGestures);
            FightEldenBeastCommand = new DelegateCommand(FightEldenBeast);
            SetMorningCommand = new DelegateCommand(SetMorning);
            SetNoonCommand = new DelegateCommand(SetNoon);
            SetNightCommand = new DelegateCommand(SetNight);
            
            _baseGameGestureIds = DataLoader.GetSimpleList("BaseGestures", int.Parse);
            _dlcGestureIds = DataLoader.GetSimpleList("DlcGestures", int.Parse);

            RegisterHotkeys();
        }
        

        #region Commands
        
        public ICommand SetEventCommand { get; set; }
        public ICommand GetEventCommand { get; set; }
        public ICommand UnlockWhetbladesCommand { get; set; }
        public ICommand ClearDlcCommand { get; set; }
        public ICommand UnlockMetyrCommand { get; set; }
        public ICommand FightFortissaxCommand { get; set; }
        public ICommand UnlockGesturesCommand { get; set; }
        public ICommand FightEldenBeastCommand { get; set; }
        public ICommand SetMorningCommand { get; set; }
        public ICommand SetNoonCommand { get; set; }
        public ICommand SetNightCommand { get; set; }

        #endregion

        #region Properties
        
        private bool _areOptionsEnabled;
        
        public bool AreOptionsEnabled
        {
            get => _areOptionsEnabled;
            set => SetProperty(ref _areOptionsEnabled, value);
        }

        private bool _isDlcAvailable;
        
        public bool IsDlcAvailable
        {
            get => _isDlcAvailable;
            set => SetProperty(ref _isDlcAvailable, value);
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
        
        private bool _isDrawEventsEnabled;
        
        public bool IsDrawEventsEnabled
        {
            get => _isDrawEventsEnabled;
            set
            {
                if (!SetProperty(ref _isDrawEventsEnabled, value)) return;
                if (_isDrawEventsEnabled)
                {
                    _utilityService.PatchDebugFont();
                    _eventService.PatchEventEnable();
                }
                _eventService.ToggleDrawEvents(_isDrawEventsEnabled);
                
            }
        }
        
        private bool _isDisableEventsEnabled;
        
        public bool IsDisableEventsEnabled
        {
            get => _isDisableEventsEnabled;
            set
            {
                if (!SetProperty(ref _isDisableEventsEnabled, value)) return;
                _eventService.ToggleDisableEvents(_isDisableEventsEnabled);
                
            }
        }
        
        #endregion

        #region Private Methods

        private void OnGameLoaded()
        {
            AreOptionsEnabled = true;
            IsDlcAvailable = _dlcService.IsDlcAvailable;
        }

        private void OnGameNotLoaded()
        {
            AreOptionsEnabled = false;
        }
        
        private void OnGameFirstLoaded()
        {
            if (IsDrawEventsEnabled)
            {
                _utilityService.PatchDebugFont();
                _eventService.PatchEventEnable();
                _eventService.ToggleDrawEvents(true);
            }
            if (IsDisableEventsEnabled) _eventService.ToggleDisableEvents(false);
        }
        
        private void RegisterHotkeys()
        {
            _hotkeyManager.RegisterAction(HotkeyActions.DrawEvent, () => IsDrawEventsEnabled = !IsDrawEventsEnabled);
            _hotkeyManager.RegisterAction(HotkeyActions.SetMorning, () => SafeExecute(SetMorning));
            _hotkeyManager.RegisterAction(HotkeyActions.SetNoon, () => SafeExecute(SetNoon));
            _hotkeyManager.RegisterAction(HotkeyActions.SetNight, () => SafeExecute(SetNight));
        }
        
        private void SafeExecute(Action action)
        {
            if (!AreOptionsEnabled) return;
            action();
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
        
        private void UnlockWhetblades()
        {
            _itemService.SpawnItem(WhetstoneBladeId, 1, -1, false, 1);
            foreach (var whetBlade in Event.WhetBlades)
            {
                _eventService.SetEvent(whetBlade, true);
            }
        }

        private void ClearDlc() => _eventService.SetEvent(Event.ClearDlc, true);
        
        private void UnlockMetyr()
        {
            foreach (var eventId in Event.UnlockMetyr)
            {
                _eventService.SetEvent(eventId, true);
            }
        }
        
        private void FightFortissax() => _eventService.SetEvent(Event.FightFortissax, true);
        private void FightEldenBeast() => _eventService.SetEvent(Event.FightEldenBeast, true);
        
        private void UnlockGestures()
        {
            foreach (var baseGameGestureId in _baseGameGestureIds)
            {
                _ezStateService.ExecuteTalkCommand(EzState.TalkCommands.AcquireGesture(baseGameGestureId));
            }

            if (!IsDlcAvailable) return;
            
            foreach (var dlcGestureId in _dlcGestureIds)
            {
                _ezStateService.ExecuteTalkCommand(EzState.TalkCommands.AcquireGesture(dlcGestureId));
            }
        }
        
        private void SetMorning() => _emevdService.ExecuteEmevdCommand(Emevd.EmevdCommands.SetMorning);
        private void SetNoon() => _emevdService.ExecuteEmevdCommand(Emevd.EmevdCommands.SetNoon);
        private void SetNight() => _emevdService.ExecuteEmevdCommand(Emevd.EmevdCommands.SetNight);

        #endregion
    }
}