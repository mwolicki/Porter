using Porter.Models;
using PorterApp.ViewModel;

namespace PorterApp.Command
{
	internal sealed class OpenObjectViewCommand : BaseCommand<IReferenceObject>
	{
		public override void Execute(IReferenceObject parameter)
		{
			var objectDetails = new ObjectDetails
			{
				DataContext = new ObjectDetailsViewModel(parameter)
			};
			objectDetails.Show();
		}
	}
}