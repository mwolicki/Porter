using PorterApp.ViewModel;

namespace PorterApp.Command
{
	internal sealed class OpenObjectViewCommand : BaseCommand<MainViewModel>
	{
		public override void Execute(MainViewModel parameter)
		{
			var objectDetails = new ObjectDetails
			{
				DataContext = new ObjectDetailsViewModel((parameter).SelectedObject.ReferenceObject)
			};
			objectDetails.Show();
		}
	}
}