using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyKeyChangerForAppleWireless {
    // ref
    // http://qiita.com/exliko/items/3135e4413a6da067b35d
    // http://inputsimulator.codeplex.com/
    // https://stackoverflow.com/questions/12761169/send-keys-through-sendinput-in-user32-dll
    // http://nonsoft.la.coocan.jp/SoftSample/CS.NET/SampleSendInput.html
    // http://d.hatena.ne.jp/ken_2501jp/20130406/1365235955

    /// <summary>
    /// Keyboard Global Hook
    /// </summary>
    public static class KeyboardGlobalHook {

        #region Declaration - Dll
        private static class NativeMethods {
            // callback 
            public delegate IntPtr KeyboardGlobalHookCallback(int code, uint msg, ref KBDLLHOOKSTRUCT hookData);

            // https://msdn.microsoft.com/ja-jp/library/cc430103.aspx
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern IntPtr SetWindowsHookEx(int idHook, KeyboardGlobalHookCallback lpfn, IntPtr hMod, uint dwThreadId);

            // https://msdn.microsoft.com/ja-jp/library/cc429591.aspx
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, uint msg, ref KBDLLHOOKSTRUCT hookData);

            // https://msdn.microsoft.com/ja-jp/library/cc430120.aspx
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
            public static extern bool UnhookWindowsHookEx(IntPtr hhk);

            [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "MapVirtualKeyA")]
            public extern static int MapVirtualKey(int wCode, int wMapType);

            // https://msdn.microsoft.com/ja-jp/library/cc411004.aspx
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public extern static void SendInput(int nInputs, Input[] pInputs, int cbsize);

            // https://msdn.microsoft.com/ja-jp/library/cc364822.aspx
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern uint keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

            // https://msdn.microsoft.com/ja-jp/library/cc364746.aspx
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern IntPtr GetMessageExtraInfo();
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        private static IntPtr _keyEventHandle;
        private static event NativeMethods.KeyboardGlobalHookCallback _hookCallback;

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
        public struct Input {
            [System.Runtime.InteropServices.FieldOffset(0)]
            public int Type;

            [System.Runtime.InteropServices.FieldOffset(4)]
            public MouseInput Mouse;

            [System.Runtime.InteropServices.FieldOffset(4)]
            public KeyboardInput Keyboard;

            [System.Runtime.InteropServices.FieldOffset(4)]
            public HardwareInput Hardware;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct MouseInput {
            public int X;
            public int Y;
            public int Data;
            public int Flags;
            public int Time;
            public int ExtraInfo;
        }

        // https://msdn.microsoft.com/ja-jp/library/windows/desktop/ms646271(v=vs.85).aspx
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct KeyboardInput {
            public short VirtualKey;    // 1 to 254. if thd Flags menmber specifes KEYEVENTF_UNICODE, VirtualKey must be 0.
            public ushort ScanCode;      // A hardware scan code for the key. If Flags specifies KEYEVENTF_UNICODE, wScan specifies a Unicode character which is to be sent to the foreground application.
            public int Flags;           // 
            public int Time;            // timestamp
            public int ExtraInfo;
        }
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct HardwareInput {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }
        #endregion

        #region Declaration
        private static class Const {
            public const int Action = 0;            // 0のみフックするのがお約束らしい
            public const int HookTypeLL = 13;       // WH_KEYBOARD_LL
        }
        private static class InputType {
            public const int Mouse = 0;
            public const int Keyboard = 1;
            public const int Hardware = 2;
        }
        private static class KeyStroke {
            public const int KeyDown = 0x100;
            public const int KeyUp = 0x101;
            public const int SysKeyDown = 0x104;
            public const int SysKeyup = 0x105;
        }
        private static class Flags {
            public const int None = 0x00;
            public const int KeyDown = 0x00;
            public const int KeyUp = 0x02;
            public const int ExtendeKey = 0x01;     //  拡張コード(これを設定することで修飾キーも有効になる)
            public const int Unicode = 0x04;
            public const int ScanCode = 0x08;
        }
        private static class ExtraInfo {
            public const int SendKey = 1;
        }

        private class KeySet {
            public short VirtualKey;
            public ushort ScanCode;
            public int Flag;
            public KeySet(byte[] pair) : this(pair, Flags.None) {
            }
            public KeySet(byte[] pair, int flag) {
                this.VirtualKey = KeySetPair.VirtualKey(pair);
                this.ScanCode = KeySetPair.ScanCode(pair);
                this.Flag = flag;
            }
        }
        private static class ModifiedKey {
            public const int None = 0x00;
            public const int User1 = 0x01 << 1;
            public const int User2 = 0x01 << 2;
            public const int User3 = 0x01 << 3;
        }
        private static int _modified = ModifiedKey.None;

        // ScanCode Modified Key Pair
        private static Dictionary<ushort, byte> _modifiedKeys = new Dictionary<ushort, byte> {
            { ScanCode.Muhenkan, ModifiedKey.User1 },
            { ScanCode.F15, ModifiedKey.User2 },
            { ScanCode.AtMark, ModifiedKey.User3 },
        };


        // User1(Muhenkan)
        private static Dictionary<byte, KeySet> _convertMappingUser1 = new Dictionary<byte, KeySet> {
            { ScanCode.F14, new KeySet(KeySetPair.Kanji) },

            { ScanCode.Z, new KeySet(KeySetPair.Num3) },
            { ScanCode.X, new KeySet(KeySetPair.Minus) },
            { ScanCode.C, new KeySet(KeySetPair.Colon) },
            { ScanCode.N, new KeySet(KeySetPair.Delete) },
            { ScanCode.M, new KeySet(KeySetPair.BackSpace) },
            { ScanCode.LessThan, new KeySet(KeySetPair.Insert) },
            { ScanCode.GreaterThan, new KeySet(KeySetPair.Tab) },
            { ScanCode.Underscore, new KeySet(KeySetPair.PageDown, Flags.ExtendeKey) },
            
            { ScanCode.A, new KeySet(KeySetPair.Caret) },
            { ScanCode.S, new KeySet(KeySetPair.Underscore) },
            { ScanCode.D, new KeySet(KeySetPair.BracketsR) },
            { ScanCode.F, new KeySet(KeySetPair.Yen) },
            { ScanCode.J, new KeySet(KeySetPair.Home, Flags.ExtendeKey) },
            { ScanCode.K, new KeySet(KeySetPair.End, Flags.ExtendeKey) },
            { ScanCode.L, new KeySet(KeySetPair.Enter) },
            { ScanCode.SemiColon, new KeySet(KeySetPair.Escape) },
            { ScanCode.Colon, new KeySet(KeySetPair.PageUp, Flags.ExtendeKey) },

            { ScanCode.Q, new KeySet(KeySetPair.Num1) },
            { ScanCode.W, new KeySet(KeySetPair.Num2) },
            { ScanCode.E, new KeySet(KeySetPair.Num3) },
            { ScanCode.R, new KeySet(KeySetPair.Num4) },
            { ScanCode.T, new KeySet(KeySetPair.Num5) },
            { ScanCode.Y, new KeySet(KeySetPair.Num6) },
            { ScanCode.U, new KeySet(KeySetPair.Num7) },
            { ScanCode.I, new KeySet(KeySetPair.Num8) },
            { ScanCode.O, new KeySet(KeySetPair.Num9) },
            { ScanCode.P, new KeySet(KeySetPair.Num0) },
        };

        // User2(F15)
        private static Dictionary<byte, KeySet> _convertMappingUser2 = new Dictionary<byte, KeySet> {
            { ScanCode.X, new KeySet(KeySetPair.Minus) },
            { ScanCode.C, new KeySet(KeySetPair.Colon) },
            { ScanCode.M, new KeySet(KeySetPair.Left, Flags.ExtendeKey) },
            { ScanCode.LessThan, new KeySet(KeySetPair.Down, Flags.ExtendeKey) },
            { ScanCode.GreaterThan, new KeySet(KeySetPair.Up, Flags.ExtendeKey) },
            { ScanCode.Slash, new KeySet(KeySetPair.Right, Flags.ExtendeKey) },

            { ScanCode.A, new KeySet(KeySetPair.F11) },
            { ScanCode.S, new KeySet(KeySetPair.F12) },
            { ScanCode.K, new KeySet(KeySetPair.PrintScreen) },
            { ScanCode.L, new KeySet(KeySetPair.ScrollLock) },
            { ScanCode.SemiColon, new KeySet(KeySetPair.Pause) },
            { ScanCode.Colon, new KeySet(KeySetPair.CapsLock) },

            { ScanCode.Q, new KeySet(KeySetPair.F1) },
            { ScanCode.W, new KeySet(KeySetPair.F2) },
            { ScanCode.E, new KeySet(KeySetPair.F3) },
            { ScanCode.R, new KeySet(KeySetPair.F4) },
            { ScanCode.T, new KeySet(KeySetPair.F5) },
            { ScanCode.Y, new KeySet(KeySetPair.F6) },
            { ScanCode.U, new KeySet(KeySetPair.F7) },
            { ScanCode.I, new KeySet(KeySetPair.F8) },
            { ScanCode.O, new KeySet(KeySetPair.F9) },
            { ScanCode.P, new KeySet(KeySetPair.F10) },
        };

        // User3(@)
        private static Dictionary<byte, KeySet> _convertMappingUser3 = new Dictionary<byte, KeySet> {
            { ScanCode.Z, new KeySet(KeySetPair.BracketsL) },
            { ScanCode.X, new KeySet(KeySetPair.BracketsR) },
            { ScanCode.C, new KeySet(KeySetPair.Yen) },
            { ScanCode.V, new KeySet(KeySetPair.AtMark) },

            { ScanCode.A, new KeySet(KeySetPair.SemiColon) },
            { ScanCode.S, new KeySet(KeySetPair.Minus) },
            { ScanCode.D, new KeySet(KeySetPair.Colon) },
            { ScanCode.F, new KeySet(KeySetPair.Caret) },
            { ScanCode.G, new KeySet(KeySetPair.Underscore) },
        };

        // single
        private static Dictionary<ushort, KeySet> _normalConvert = new Dictionary<ushort, KeySet> {
            { ScanCode.F14, new KeySet(KeySetPair.Kana) },
            { ScanCode.Underscore, new KeySet(KeySetPair.Enter) },
            { ScanCode.Colon, new KeySet(KeySetPair.Minus) },
            { ScanCode.F13, new KeySet(KeySetPair.Tab) },
            { ScanCode.Tab, new KeySet(KeySetPair.Escape) },
        };

        private static Dictionary<int, Dictionary<byte, KeySet>> _convertMappingList = new Dictionary<int, Dictionary<byte, KeySet>> {
            { ModifiedKey.User1, _convertMappingUser1},
            { ModifiedKey.User2, _convertMappingUser2},
            { ModifiedKey.User3, _convertMappingUser3},
        };
        #endregion

        #region Public Property
        public static bool IsHooking {
            get;
            private set;
        }
        #endregion

        #region Public Method
        /// <summary>
        ///  start global hook.
        /// </summary>
        public static void Start() {
            if (IsHooking) {
                return;
            }
            IsHooking = true;

            _hookCallback = HookProcedure;
            IntPtr hinst = System.Runtime.InteropServices.Marshal.GetHINSTANCE(typeof(KeyboardGlobalHook).Assembly.GetModules()[0]);

            _keyEventHandle = NativeMethods.SetWindowsHookEx(Const.HookTypeLL, _hookCallback, hinst, 0);
            if (_keyEventHandle == IntPtr.Zero) {
                IsHooking = false;
                throw new System.ComponentModel.Win32Exception();
            }
        }

        /// <summary>
        /// stop global hook.
        /// </summary>
        public static void Stop() {
            if (!IsHooking) {
                return;
            }

            if (_keyEventHandle != IntPtr.Zero) {
                IsHooking = false;
                NativeMethods.UnhookWindowsHookEx(_keyEventHandle);
                _keyEventHandle = IntPtr.Zero;
                _hookCallback -= HookProcedure;
                _modified = ModifiedKey.None;
            }
        }

        /// <summary>
        /// reset key. but I'm not sure this method is effective or not.
        /// </summary>
        public static void Reset() {
            _modified = ModifiedKey.None;
        }
        #endregion

        #region Privater Method
        /// <summary>
        /// hook
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <param name="hookData"></param>
        /// <returns></returns>
        private static IntPtr HookProcedure(int code, uint msg, ref KBDLLHOOKSTRUCT hookData) {
            byte scanCode = (byte)hookData.scanCode;

            System.Diagnostics.Debug.Print("### scanCode:" + scanCode.ToString("x4"));
            System.Diagnostics.Debug.Print("### hookData.dwExtraInfo:" + hookData.dwExtraInfo.ToString("x4"));
            System.Diagnostics.Debug.Print("### msg:" + msg.ToString("x4"));
            System.Diagnostics.Debug.Print("### code:" + code.ToString("x4"));
            System.Diagnostics.Debug.Print("### _modified:" + _modified);

            if (Const.Action != code || (IntPtr)ExtraInfo.SendKey == hookData.dwExtraInfo) {
                goto ExitProc;
            }

            var keyStroke = (KeyStroke.KeyDown == msg) ? Flags.KeyDown : Flags.KeyUp;
            if (_modifiedKeys.ContainsKey(scanCode)) {
                if (KeyStroke.KeyDown == msg) {
                    _modified = _modifiedKeys[scanCode];
                } else {
                    _modified = ModifiedKey.None;
                }
                return (IntPtr)1;
            }
            if (ModifiedKey.None == _modified) {
                if (_normalConvert.ContainsKey(scanCode)) {
                    SendKey(keyStroke, _normalConvert[scanCode]);
                    return (IntPtr)1;
                }
                goto ExitProc;
            }

            var mappingData = _convertMappingList[_modified];
            
            if (mappingData.ContainsKey(scanCode)) {
                SendKey(keyStroke, mappingData[scanCode]);
                return (IntPtr)1;
            }

        ExitProc:
            return NativeMethods.CallNextHookEx(_keyEventHandle, code, msg, ref hookData);
        }

        
        /// <summary>
        /// send key
        /// </summary>
        /// <param name="flag">flag. usually set additional flags</param>
        /// <param name="keyset">key set</param>
        private static void SendKey(int flag, KeySet keyset) {
            Input input = new Input();
            input.Type = InputType.Keyboard;
            input.Keyboard.Flags = flag | keyset.Flag;
            input.Keyboard.VirtualKey = keyset.VirtualKey;
            input.Keyboard.ScanCode = keyset.ScanCode;
            input.Keyboard.Time = 0;
            input.Keyboard.ExtraInfo = ExtraInfo.SendKey;
            Input[] inputs = { input };
            NativeMethods.SendInput(inputs.Length, inputs, System.Runtime.InteropServices.Marshal.SizeOf(inputs[0]));
        }
        #endregion
    }
}
