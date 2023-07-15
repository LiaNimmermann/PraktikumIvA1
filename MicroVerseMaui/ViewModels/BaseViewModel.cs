
using CommunityToolkit.Mvvm.ComponentModel;

namespace MicroVerseMaui.ViewModels;


// The base view model for all viewmodels
public partial class BaseViewModel : ObservableObject
{
    //Initializes a new instance of the BaseViewModel class.
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