using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MauiComparableRepro;

public partial class MainViewModel(IPopupService popupService) : ObservableObject
{
	const double finalUpdateProgressValue = 1;

	int updates;

	readonly IPopupService popupService = popupService;

	[ObservableProperty]
	public partial string Message { get; set; } = "";

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(FinishCommand))]
	public partial double UpdateProgress { get; set; }

	internal async void PerformUpdates(int numberOfUpdates)
	{
		double updateTotalForPercentage = numberOfUpdates + 1;
		updates = numberOfUpdates;

		for (var update = 1; update <= numberOfUpdates; update++)
		{
			Message = $"Updating {update} of {numberOfUpdates}";

			UpdateProgress = update / updateTotalForPercentage;

			await Task.Delay(TimeSpan.FromSeconds(1));
		}

		UpdateProgress = finalUpdateProgressValue;
		Message = $"Completed {numberOfUpdates} updates";
	}

	[RelayCommand(CanExecute = nameof(CanFinish))]
	void OnFinish()
	{
		popupService.ClosePopup();
	}

	[RelayCommand]
	void OnMore()
	{
		popupService.ShowPopup<MainViewModel>(onPresenting: viewModel => viewModel.PerformUpdates(updates + 2));
	}

	bool CanFinish() => UpdateProgress is finalUpdateProgressValue;
}