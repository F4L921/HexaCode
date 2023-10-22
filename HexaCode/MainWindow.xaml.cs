// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Runtime.InteropServices;
using WinRT;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace HexaCode
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();
			ExtendsContentIntoTitleBar = true;
			SetTitleBar(AppTitleBar);
			nvSample.SelectedItem = acc;

			SubClassing();
		}

		private delegate IntPtr WinProc(IntPtr hWnd, PInvoke.User32.WindowMessage Msg, IntPtr wParam, IntPtr lParam);
		private WinProc newWndProc = null;
		private IntPtr oldWndProc = IntPtr.Zero;
		[DllImport("user32")]
		private static extern IntPtr SetWindowLong(IntPtr hWnd, PInvoke.User32.WindowLongIndexFlags nIndex, WinProc newProc);
		[DllImport("user32.dll")]
		static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, PInvoke.User32.WindowMessage Msg, IntPtr wParam, IntPtr lParam);

		private void SubClassing()
		{
			//Get the Window's HWND
			var hwnd = this.As<IWindowNative>().WindowHandle;

			newWndProc = new WinProc(NewWindowProc);
			oldWndProc = SetWindowLong(hwnd, PInvoke.User32.WindowLongIndexFlags.GWL_WNDPROC, newWndProc);
		}

		int MinWidth = 800;
		int MinHeight = 600;

		[StructLayout(LayoutKind.Sequential)]
		struct MINMAXINFO
		{
			public PInvoke.POINT ptReserved;
			public PInvoke.POINT ptMaxSize;
			public PInvoke.POINT ptMaxPosition;
			public PInvoke.POINT ptMinTrackSize;
			public PInvoke.POINT ptMaxTrackSize;
		}

		private IntPtr NewWindowProc(IntPtr hWnd, PInvoke.User32.WindowMessage Msg, IntPtr wParam, IntPtr lParam)
		{
			switch (Msg)
			{
				case PInvoke.User32.WindowMessage.WM_GETMINMAXINFO:
					var dpi = PInvoke.User32.GetDpiForWindow(hWnd);
					float scalingFactor = (float)dpi / 96;

					MINMAXINFO minMaxInfo = Marshal.PtrToStructure<MINMAXINFO>(lParam);
					minMaxInfo.ptMinTrackSize.x = (int)(MinWidth * scalingFactor);
					minMaxInfo.ptMinTrackSize.y = (int)(MinHeight * scalingFactor);
					Marshal.StructureToPtr(minMaxInfo, lParam, true);
					break;

			}
			return CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);
		}


		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
		internal interface IWindowNative
		{
			IntPtr WindowHandle { get; }
		}


		private void nvSample_Loaded(object sender, RoutedEventArgs e)
		{
			contentFrame.Navigate(typeof(Views.Accueil));
		}


		private void nvSample_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
		{
			if (args.IsSettingsInvoked)
			{
				contentFrame.Navigate(typeof(Views.Parametres));
			}
			else if (args.InvokedItemContainer != null && (args.InvokedItemContainer.Tag != null))
			{
				Type selectedpage = Type.GetType(args.InvokedItemContainer.Tag.ToString());
				contentFrame.Navigate(selectedpage, null, args.RecommendedNavigationTransitionInfo);
			}

		}
	}
}

