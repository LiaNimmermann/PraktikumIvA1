
using CommunityToolkit.Mvvm.ComponentModel;

namespace MicroVerseMaui.ViewModels;

public partial class BaseViewModel : ObservableObject
{

    public BaseViewModel()
    {

    }

    [ObservableProperty]
    string title;

    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    [ObservableProperty]
    bool isBusy;

    public bool IsNotBusy => !IsBusy;



}