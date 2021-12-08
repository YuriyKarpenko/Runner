using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using ReactiveUI;
using Runner.Models;
#if AVALONIA
using Avalonia.Controls;
using Avalonia;
#else
using System.Windows.Controls;
using System.Windows.Media;
#endif

namespace Runner.ViewModels
{
#if AVALONIA
#else
#endif

	interface IDataContext
	{
#if AVALONIA
		void OnDataContext_Set(StyledElement e);
#else
		void OnDataContext_Set(FrameworkElement e);
#endif
	}

	public class BaseViewModel : ReactiveObject, IDataContext
	{
		#region static

		//static BaseViewModel()
		//{
		//	DependencyPropertyDescriptor pd = DependencyPropertyDescriptor.FromProperty(StyledElement.DataContextProperty, typeof(ContentControl));
		//	if (pd != null)
		//	{
		//		var oldSealed = UtilsReflection.GetPropertyValue(pd.Metadata, "Sealed");
		//		UtilsReflection.SetPropertyValue(pd.Metadata, "Sealed", false);

		//		pd.Metadata.PropertyChangedCallback += CallbackDataContext;

		//		UtilsReflection.SetPropertyValue(pd.Metadata, "Sealed", oldSealed);
		//	}

		//	//var pm = new FrameworkPropertyMetadata(CallbackDataContext);
		//	//FrameworkElement.DataContextProperty.AddOwner(typeof(ContentControl), pm);
		//}
		//static void CallbackDataContext(DependencyObject d, DependencyPropertyChangedEventArgs e)
		//{
		//	if (e.NewValue != null && e.NewValue is IDataContext vm)
		//	{
		//		vm.OnDataContext_Set(d as FrameworkElement);
		//	}
		//}

		#endregion

		/// <summary> Окно, в котором находится привязанный View </summary>
		protected Window? CurrentWindow = null;
		/// <summary> привязанный View </summary>
		protected UserControl? CurrentUC = null;

		#region Init

		/// <summary> Запускается в момент назначения данного ViewModel в качестве View.DataContext </summary>
		/// <param name="w">привязанный View</param>
		protected virtual void Init_Command_Core(Window w) { }

		/// <summary> Запускается в момент назначения данного ViewModel в качестве View.DataContext </summary>
		/// <param name="uc">привязанный View</param>
		protected virtual void Init_Command_Core(UserControl uc) { }

		/// <summary> Запускается в момент назначения данного ViewModel в качестве View.DataContext </summary>
		/// <param name="fe">привязанный View</param>
#if AVALONIA
		protected virtual void Init_Command_Core(StyledElement fe) { }
#else
		protected virtual void Init_Command_Core(FrameworkElement fe) { }
#endif


#if AVALONIA
		void IDataContext.OnDataContext_Set(StyledElement element)
#else
		void IDataContext.OnDataContext_Set(FrameworkElement element)
#endif
		{
			try
			{
				if (element.DataContext == this)
				{
					Init_Command_Core(element);

					if (element is UserControl uc && uc != this.CurrentUC)
					{
						this.CurrentUC = uc;
						this.Init_Command_Core(uc);
					}

					if (element is Window w && w != this.CurrentWindow)
					{
						this.CurrentWindow = w;
						this.Init_Command_Core(w);
					}
				}
			}
			catch (Exception ex)
			{
				Log(ex.ToString());
			}
		}

		#endregion

		protected void Log(string message, bool isError = true)
		{
			if (message != null)
			{
				//LogItems.Add(new M.LogItem(message, isError ? Brushes.Red : Brushes.Gray));
				//OnPropertyChanged("Logs");
			}
		}
	}
}