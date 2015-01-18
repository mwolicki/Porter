using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PorterApp.Annotations;
using PorterApp.ViewModel;

namespace PorterApp
{
	/// <summary>
	/// Interaction logic for TypesTreeWindow.xaml
	/// </summary>
	public partial class TypesTreeWindow
	{
		public TypesTreeWindow(ObservableCollection<TreeItem> items)
		{
			DataContext = items;
			InitializeComponent();
		}
	}

	internal class TreeView2 : TreeView
	{
		// HACK: Prevent docking library from setting item source to null value
		protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
		{
			if (newValue == null && oldValue != null)
			{
				ItemsSource = oldValue;
			}
			else
			{
				base.OnItemsSourceChanged(oldValue, newValue);
			}
		}
	}


	public static class TextBlockBehavior
	{
		public static readonly DependencyProperty OnDoubleClickProperty = DependencyProperty.RegisterAttached(
			"OnDoubleClick", typeof (ICommand), typeof (TextBlockBehavior), new PropertyMetadata(default(ICommand), OnDoubleClickChanged));

		public static readonly DependencyProperty OnDoubleClickParamProperty = DependencyProperty.RegisterAttached(
			"OnDoubleClickParam", typeof(object), typeof(TextBlockBehavior), new PropertyMetadata(default(object)));

		private static void OnDoubleClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var textBlock = d as UIElement;
			if (textBlock != null)
			{
				textBlock.MouseLeftButtonDown+=TextBlockOnMouseLeftButtonDown;
			}
		}

		private static void TextBlockOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
		{
			var d = sender as DependencyObject;
			if (d != null && mouseButtonEventArgs.ClickCount == 2)
			{
				var command = (ICommand)d.GetValue(OnDoubleClickProperty);
				if (command != null)
					command.Execute(d.GetValue(OnDoubleClickParamProperty));
			}
		}

		public static bool GetOnDoubleClick(DependencyObject obj)
		{
			return (bool)obj.GetValue(OnDoubleClickProperty);
		}

		public static void SetOnDoubleClick(DependencyObject obj, bool value)
		{
			obj.SetValue(OnDoubleClickProperty, value);
		}

		public static bool GetOnDoubleClickParam(DependencyObject obj)
		{
			return (bool)obj.GetValue(OnDoubleClickProperty);
		}

		public static void SetOnDoubleClickParam(DependencyObject obj, bool value)
		{
			obj.SetValue(OnDoubleClickProperty, value);
		}
	}
}
