using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;

namespace DarkSoulsIIIRandomWeaponMod
{
    class Whoevenuseswpflul
    {
        //not really sure
        public const int ProcessVmRead = 0x10;
        public const int Th32CsSnapprocess = 0x2;
        public const int MemCommit = 4096;
        public const int PageReadwrite = 4;
        public const int ProcessCreateThread = 0x2;
        public const int ProcessVmOperation = 0x8;
        public const int ProcessVmWrite = 0x20;
        public const int ProcessAllAccess = 0x1f0fff;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, long dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess,
     long lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess,
     long lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);


        public static List<string> Enableweapons = new List<string>();
        private static Thread _fgt;
        private static bool _shouldrun;
        private static double _waittime = 1000;

        private static int Weaponsfcs(IReadOnlyList<string> weaponlist)
        {
            if (weaponlist.Count == 0)
            {
                // amazing weapon incoming
                return 2030000;
            }

            bool isbossweapon = false;
            char[] delimiterChars = { ' ' };

            Random rnd = new Random();
            var r = rnd.Next(weaponlist.Count);
            var cur = weaponlist[r].Split(delimiterChars)[0];


            foreach (var t in Weaponlists.BossWeaponlist)
            {
                // Check all boss weapons, we should really just do 2 seperat lists, but meh, this is easier
                var curwpn = t.Split(delimiterChars)[0];
                if (cur == curwpn) { isbossweapon = true; }
            }

            // Check for errors, used when debugging
            /*# ifdef _DEBUG
                        for (int i = 0; i < cur.length(); ++i)
                        {
                            const char* current = &cur[i];
                            if (!isNumber(current))
                            {
                                std::cout << "Invalid character in (" << cur << ")(" << cur[i] << ")" << std::endl;
                                cur.erase(i, 1);
                            }
                        }
            #endif*/

            int weapon;
            if (cur.Any()) //if not empty
            {
                weapon = Convert.ToInt32(cur);

                var upgradeLvl = rnd.Next(0, isbossweapon ? 5 : 10);
                weapon = weapon + upgradeLvl;
            }
            else
            {
                //std::cout << "Invalid position in string (" << RandomWeaponIndex << ")" << std::endl;
                weapon = 2030000;
            }
            return weapon;
        }

        private static void Threadingman()
        {
            // THREADING ON THE RUN
            var rWeapon = new byte[sizeof(long)];
            const long primaryrightwebOffset1 = 0x10;
            const long primaryrightwebOffset2 = 0x330;


            var id = -1;
            var processes = Process.GetProcesses();
            var bg = new Process();
            foreach (var pp in processes)
            {
                //Check if Dark Souls is Open
                if (!pp.MainWindowTitle.Equals("DARK SOULS III")) continue;
                id = pp.Id;
                bg = pp;
            }
            if (id == -1)
            {
                MainWindow.Fails();
                MessageBox.Show("Failed to find process. Please run with administrative privileges.");
                return;
            }


            IntPtr hprocess = OpenProcess(ProcessAllAccess, false, id);
            if (hprocess.ToInt64() == 0)
            {
                MainWindow.Fails();
                MessageBox.Show("Failed to attach to process. Please run with administrative privileges.");
                return;
            }

            var bytesRead = 0;
            var ret = IntPtr.Add(bg.MainModule.BaseAddress, 0x4740178);
            //var f = ret;
            ReadProcessMemory(hprocess, ret.ToInt64(), rWeapon, rWeapon.Length, ref bytesRead);
            var rf = (IntPtr)BitConverter.ToInt64(rWeapon, 0);
            var sec = IntPtr.Add(rf, (int)primaryrightwebOffset1);
            ReadProcessMemory(hprocess, sec.ToInt64(), rWeapon, rWeapon.Length, ref bytesRead);
            rf = (IntPtr)BitConverter.ToInt64(rWeapon, 0);

            sec = IntPtr.Add(rf, (int)primaryrightwebOffset2);
            ReadProcessMemory(hprocess, sec.ToInt64(), rWeapon, rWeapon.Length, ref bytesRead);
            BitConverter.ToInt32(rWeapon, 0);


            while (_shouldrun)
            {
                var weapon = Weaponsfcs(Enableweapons);

                WriteProcessMemory(hprocess, sec.ToInt64(), BitConverter.GetBytes(weapon), 4, ref bytesRead);

                Thread.Sleep(Convert.ToInt32(_waittime * 1000));
            }
        }

        private static void Merge(ICollection<string> toinsert, IEnumerable<string> tobeinserted)
        {
            foreach (var t in tobeinserted)
            {
                toinsert.Add(t);
            }
        }

        public static void Modlist(
            string wait,
            bool? standard,
            bool? bows,
            bool? whips,
            bool? shields,
            // Infusions
            bool? heavy,
            bool? sharp,
            bool? refined,
            bool? simple,
            bool? crystal,
            bool? fire,
            bool? chaos,
            bool? lightning,
            bool? deep,
            bool? dark,
            bool? poison,
            bool? blood,
            bool? raw,
            bool? blessed,
            bool? hollow,
            // SECRET
            bool? secret
            )
        {
            var l = double.TryParse(wait, out _waittime);
            if (l)
            {
                Debug.Print("ok");
                Debug.Print(Convert.ToString(_waittime, CultureInfo.CurrentCulture));
            }
            else
            {
                Debug.Print("no");
            }
            Enableweapons.Clear();

            if (fire == true)
            {
                Debug.Print("lol");
            }

            if (standard == true)
            {

                Merge(Enableweapons, Weaponlists.Standardweapons);
                if (heavy == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWeaponHeavy);
                }
                if (sharp == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWeaponSharp);
                }
                if (refined == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWeaponRefined);
                }
                if (simple == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWeaponSimple);
                }
                if (crystal == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWeaponCrystal);
                }
                if (fire == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWeaponFire);
                }
                if (chaos == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWeaponChaos);
                }
                if (lightning == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWeaponLightning);
                }
                if (deep == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWeaponDeep);
                }
                if (dark == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWeaponDark);
                }
                if (poison == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWeaponPoison);
                }
                if (blood == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWeaponBlood);
                }
                if (raw == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWeaponRaw);
                }
                if (blessed == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWeaponBlessed);
                }
                if (hollow == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWeaponHollow);
                }
            }
            if (bows == true)
            {

                Merge(Enableweapons, Weaponlists.StandardBows);
            }
            if (whips == true)
            {

                Merge(Enableweapons, Weaponlists.StandardWhip);
                if (heavy == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWhipHeavy);
                }
                if (sharp == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWhipSharp);
                }
                if (refined == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWhipRefined);
                }
                if (simple == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWhipSimple);
                }
                if (crystal == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWhipCrystal);
                }
                if (fire == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWhipFire);
                }
                if (chaos == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWhipChaos);
                }
                if (lightning == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWhipLightning);
                }
                if (deep == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWhipDeep);
                }
                if (dark == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWhipDark);
                }
                if (poison == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWhipPoison);
                }
                if (blood == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWhipBlood);
                }
                if (raw == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWhipRaw);
                }
                if (blessed == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWhipBlessed);
                }
                if (hollow == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardWhipHollow);
                }
            }
            if (shields == true)
            {

                Merge(Enableweapons, Weaponlists.StandardShields);
                if (heavy == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardShieldsHeavy);
                }
                if (sharp == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardShieldsSharp);
                }
                if (refined == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardShieldsRefined);
                }
                if (simple == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardShieldsSimple);
                }
                if (crystal == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardShieldsCrystal);
                }
                if (fire == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardShieldsFire);
                }
                if (chaos == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardShieldsChaos);
                }
                if (lightning == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardShieldsLightning);
                }
                if (deep == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardShieldsDeep);
                }
                if (dark == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardShieldsDark);
                }
                if (poison == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardShieldsPoison);
                }
                if (blood == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardShieldsBlood);
                }
                if (raw == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardShieldsRaw);
                }
                if (blessed == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardShieldsBlessed);
                }
                if (hollow == true)
                {

                    Merge(Enableweapons, Weaponlists.StandardShieldsHollow);
                }
            }

            if (secret != true) return;
            for (var i = 0; i < 4; i++)
            {

                Merge(Enableweapons, Weaponlists.StandardShields);

                Merge(Enableweapons, Weaponlists.StandardShieldsHeavy);

                Merge(Enableweapons, Weaponlists.StandardShieldsSharp);

                Merge(Enableweapons, Weaponlists.StandardShieldsRefined);

                Merge(Enableweapons, Weaponlists.StandardShieldsSimple);

                Merge(Enableweapons, Weaponlists.StandardShieldsCrystal);

                Merge(Enableweapons, Weaponlists.StandardShieldsFire);

                Merge(Enableweapons, Weaponlists.StandardShieldsChaos);

                Merge(Enableweapons, Weaponlists.StandardShieldsLightning);

                Merge(Enableweapons, Weaponlists.StandardShieldsDeep);

                Merge(Enableweapons, Weaponlists.StandardShieldsDark);

                Merge(Enableweapons, Weaponlists.StandardShieldsPoison);

                Merge(Enableweapons, Weaponlists.StandardShieldsBlood);

                Merge(Enableweapons, Weaponlists.StandardShieldsRaw);

                Merge(Enableweapons, Weaponlists.StandardShieldsBlessed);

                Merge(Enableweapons, Weaponlists.StandardShieldsHollow);
            }
        }

        public static bool Run()
        {
            if (_shouldrun)
            {
                // Stop
                _shouldrun = false;
                Thread.Sleep(10);
                _fgt.Abort();
            }
            else
            {
                // Start
                _shouldrun = true;
                _fgt = new Thread(Threadingman);
                _fgt.Start();
            }
            return _shouldrun;
        }

        public static void Stop()
        {
            _shouldrun = false;
        }
    }
}
