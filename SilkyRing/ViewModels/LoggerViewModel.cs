using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SilkyRing.Memory.DLLShared;

namespace SilkyRing.ViewModels
{
    public class LoggerViewModel : BaseViewModel
    {
        
        private readonly DllManager _dllManager;
        private bool _isInitializing;
        
        private string _logText = string.Empty;
        
        private bool _isSetEventLogging;
        private bool _isApplySpEffectLogging; 
        private bool _isAiGoalsLogging;
        private bool _isApplySpEffectUniqueLogging;
        private bool _isSetEventUniqueLogging;
        
        public bool IsInitializing 
        { 
            get => _isInitializing; 
            set { _isInitializing = value; OnPropertyChanged(); }
        }
        
        public LoggerViewModel(DllManager dllManager)
        {
            _dllManager = dllManager;
            _dllManager.LogReceived += OnLogReceived;
        }

        public async Task InitializeAsync()
        {
            IsInitializing = true;
            
            await Task.Run(() =>
            {
                _dllManager.EnsureInjectedDll();
                _dllManager.StartLogReading();
            });
        
            IsInitializing = false;
        }
        
        public bool IsSetEventLogging
        {
            get => _isSetEventLogging;
            set 
            { 
                if (SetProperty(ref _isSetEventLogging, value))
                    _dllManager.SetLogCommand(LogCommand.LogSetEvent, value);
            }
        }
    
        public bool IsApplySpEffectLogging
        {
            get => _isApplySpEffectLogging;
            set 
            { 
                if (SetProperty(ref _isApplySpEffectLogging, value))
                    _dllManager.SetLogCommand(LogCommand.LogApplySpeffect, value);
            }
        }
        
        public bool IsAiGoalsLogging
        {
            get => _isAiGoalsLogging;
            set 
            { 
                if (SetProperty(ref _isAiGoalsLogging, value))
                    _dllManager.SetLogCommand(LogCommand.LogAiGoals, value);
            }
        }
    
        public bool IsApplySpEffectUniqueLogging
        {
            get => _isApplySpEffectUniqueLogging;
            set 
            { 
                if (SetProperty(ref _isApplySpEffectUniqueLogging, value))
                    _dllManager.SetLogCommand(LogCommand.LogUniqueSpeffect, value);
            }
        }
    
        public bool IsSetEventUniqueLogging
        {
            get => _isSetEventUniqueLogging;
            set 
            { 
                if (SetProperty(ref _isSetEventUniqueLogging, value))
                    _dllManager.SetLogCommand(LogCommand.LogUniqueSetEvent, value);
            }
        }
        
        public string LogText 
        { 
            get => _logText; 
            set => SetProperty(ref _logText, value);
        }
        
        public void ClearUniqueSetEvents()
        {
            _dllManager.SetLogCommand(LogCommand.ClearUniqueSetEvent, true);
        }
    
        public void ClearUniqueSpEffects()
        {
            _dllManager.SetLogCommand(LogCommand.ClearUniqueSpeffect, true);
        }
    
        public void ClearConsole()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _logBuilder.Clear();
                LogText = string.Empty;
            });
        }
        
        public void PauseAllLogging()
        {
            IsSetEventLogging = false;
            IsApplySpEffectLogging = false;
            IsAiGoalsLogging = false;
            IsApplySpEffectUniqueLogging = false;
            IsSetEventUniqueLogging = false;
        }
    
        private readonly StringBuilder _logBuilder = new StringBuilder();
        private const int MaxLength = 100000;
        
        private DateTime _lastUiUpdate = DateTime.MinValue;
        private readonly StringBuilder _pendingLogs = new StringBuilder();
        private readonly object _logLock = new object();

        private void OnLogReceived(object sender, string logs)
        {
            lock (_logLock)
            {
                _pendingLogs.Append(logs);
            }
            
            if ((DateTime.Now - _lastUiUpdate).TotalMilliseconds >= 250)
            {
                string logsToAdd;
                lock (_logLock)
                {
                    logsToAdd = _pendingLogs.ToString();
                    _pendingLogs.Clear();
                }
        
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _logBuilder.Append(logsToAdd);
            
                    if (_logBuilder.Length > MaxLength)
                    {
                        _logBuilder.Remove(0, _logBuilder.Length - 75000);
                    }
            
                    LogText = _logBuilder.ToString();
                });
        
                _lastUiUpdate = DateTime.Now;
            }
        }
    }
}