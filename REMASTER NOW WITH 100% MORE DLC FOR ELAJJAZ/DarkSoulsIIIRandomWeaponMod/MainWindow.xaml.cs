using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using static System.Windows.Application;


namespace DarkSoulsIIIRandomWeaponMod
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

    public MainWindow()
        {
            InitializeComponent();
        }

        // Not stolen from pro website https://stackoverflow.com/questions/11377977/global-hotkeys-in-wpf-working-from-every-window
        [DllImport("User32.dll")]
        private static extern bool RegisterHotKey(
            [In] IntPtr hWnd,
            [In] int id,
            [In] uint fsModifiers,
            [In] uint vk);

        [DllImport("User32.dll")]
        private static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);

        private HwndSource _source;
        private const int HotkeyId = 9000;

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var helper = new WindowInteropHelper(this);
            _source = HwndSource.FromHwnd(helper.Handle);
            _source?.AddHook(HwndHook);
            RegisterHotKey();
        }

        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            _source = null;
            UnregisterHotKey();
            base.OnClosed(e);
        }

        private void RegisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            const uint keyX = 0x58;
            const uint modAlt = 0x0001;
            if (!RegisterHotKey(helper.Handle, HotkeyId, modAlt, keyX))
            {
                // handle error
            }
        }

        private void UnregisterHotKey()
        {
            var helper = new WindowInteropHelper(this);
            UnregisterHotKey(helper.Handle, HotkeyId);
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int wmHotkey = 0x0312;
            switch (msg)
            {
                case wmHotkey:
                    switch (wParam.ToInt32())
                    {
                        case HotkeyId:
                            OnHotKeyPressed();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        private void Handlemicri()
        {
            Start.Content = Whoevenuseswpflul.Run() ? "Stop" : "Start";
        }

        private void OnHotKeyPressed()
        {
            Handlemicri();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Handlemicri();
        }

        public static void Fails()
        {
            // Not running
            if (Current == null) return;
            Debug.Assert(Current.MainWindow != null, "Current.MainWindow != null");
            ((MainWindow) Current.MainWindow).Start.Content = "Start";
        }

        private void Sendupdate()
        {
            // Best way to make program, put shit code elsewhere
            Whoevenuseswpflul.Modlist(Timer.Text, StandWeap.IsChecked, Bows.IsChecked, Whips.IsChecked, Shields.IsChecked, HeavyInfusions.IsChecked, SharpInfusions.IsChecked, RefinedInfusions.IsChecked, SimpleInfusions.IsChecked, CrystalInfusions.IsChecked, FireInfusions.IsChecked, ChaosInfusions.IsChecked, LightningInfusions.IsChecked, DeepInfusions.IsChecked, DarkInfusions.IsChecked, PoisonInfusions.IsChecked, BloodInfusions.IsChecked, RawInfusions.IsChecked, BlessedInfusions.IsChecked, HollowInfusions.IsChecked, Secret.IsChecked);
        }

        private void StandWeap_Checked(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void Bows_Checked(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void Shields_Checked(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void ListBoxItem_Selected_2(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void ListBoxItem_Selected_3(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void ListBoxItem_Selected_4(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void ListBoxItem_Selected_5(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void ListBoxItem_Selected_6(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void ListBoxItem_Selected_7(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void ListBoxItem_Selected_8(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void ListBoxItem_Selected_9(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void ListBoxItem_Selected_10(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void ListBoxItem_Selected_11(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void ListBoxItem_Selected_12(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void ListBoxItem_Selected_13(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void ListBoxItem_Selected_14(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void ListBoxItem_Selected_15(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void ListBoxItem_Selected_16(object sender, RoutedEventArgs e)
        {
            Sendupdate();
        }

        private void Closingg(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Whoevenuseswpflul.Stop();
        }

        private void timer_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sendupdate();
        }

        private void Secret_Checked(object sender, RoutedEventArgs e)
        {
            Sendupdate();
            Secret.Content = "Znorq: only thing i find about 'secret' option is 'shouto out to Elajjaz for trying the mod and testing the secret option' so no help there : )";
        }

        private int _k;
        private void Testt(object sender, MouseButtonEventArgs e)
        {
            _k += 1;
            if (_k >= 20)
            {
                Status.Content = "Usable: U bored?";
            }
        }
    }
}
