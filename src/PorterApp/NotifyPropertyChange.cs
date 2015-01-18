using System.ComponentModel;
using System.Runtime.CompilerServices;
using PorterApp.Annotations;

namespace PorterApp
{
    public abstract class NotifyPropertyChange : INotifyPropertyChanged
	{
		public virtual event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}