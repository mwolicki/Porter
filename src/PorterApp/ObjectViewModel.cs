namespace PorterApp
{
	internal sealed class ObjectViewModel : NotifyPropertyChange
	{
		private string _name;
		private ulong _size;

		public string Name
		{
			get { return _name; }
			set
			{
				if (value == _name) return;
				_name = value;
				OnPropertyChanged();
			}
		}

		public ulong Size
		{
			get { return _size; }
			set
			{
				if (value == _size) return;
				_size = value;
				OnPropertyChanged();
			}
		}
	}
}