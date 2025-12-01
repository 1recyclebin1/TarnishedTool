using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SilkyRing.Core;
using SilkyRing.Enums;
using SilkyRing.Interfaces;
using SilkyRing.Models;
using SilkyRing.Utilities;

namespace SilkyRing.ViewModels;

public class EnemyViewModel : BaseViewModel
{
    private readonly IEnemyService _enemyService;

    private const int EbNpcThinkParamId = 22000000;
    
    public EnemyViewModel(IEnemyService enemyService, IStateService stateService)
    {
        _enemyService = enemyService;

        stateService.Subscribe(State.Loaded, OnGameLoaded);
        stateService.Subscribe(State.NotLoaded, OnGameNotLoaded);

        EbForceActSequenceCommand = new DelegateCommand(ForceEbActSequence);

        _acts = new ObservableCollection<Act>(DataLoader.GetEbActs());
        SelectedAct = Acts.FirstOrDefault();
    }

    #region Commands

    public ICommand EbForceActSequenceCommand { get; set; }

    #endregion

    #region Properties

    private bool _areOptionsEnabled = true;

    public bool AreOptionsEnabled
    {
        get => _areOptionsEnabled;
        set => SetProperty(ref _areOptionsEnabled, value);
    }
    
    private ObservableCollection<Act> _acts;
    
    public ObservableCollection<Act> Acts
    {
        get => _acts;
        set => SetProperty(ref _acts, value);
    }
    
    private Act _selectedAct;

    public Act SelectedAct
    {
        get => _selectedAct;
        set => SetProperty(ref _selectedAct, value);
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

    private void ForceEbActSequence()
    {
        int[] acts = [22, SelectedAct.ActIdx];
        _enemyService.ForceActSequence(acts, EbNpcThinkParamId);
    }

    #endregion
}