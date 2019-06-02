using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyKeyChangerForAppleWireless {
    public partial class KeyChangerMain : Component {

        #region Constructor
        public KeyChangerMain() {
            InitializeComponent();
            this.Initialize();
        }

        public KeyChangerMain(IContainer container) {
            container.Add(this);

            InitializeComponent();
            this.Initialize();
        }
        #endregion

        #region Event
        private void AppMenuStart_Click(object sender, EventArgs e) {
            this.StartHook();

            var appData = AppData.GetInstance();
            appData.IsStart = true;
            appData.Save();
        }

        private void AppMenuStop_Click(object sender, EventArgs e) {
            this.StopHook();

            var appData = AppData.GetInstance();
            appData.IsStart = false;
            appData.Save();
        }

        private void AppMenuReset_Click(object sender, EventArgs e) {
            KeyboardGlobalHook.Reset();
        }

        private void AppMenuExit_Click(object sender, EventArgs e) {
            this.StopHook();
            Application.Current.Shutdown();
        }
        #endregion

        #region Private Method
        /// <summary>
        /// initialize component
        /// </summary>
        private void Initialize() {
            this.cAppMenuStart.Click += AppMenuStart_Click;
            this.cAppMenuStop.Click += AppMenuStop_Click;
            this.cAppMenuReset.Click += AppMenuReset_Click;
            this.cAppMenuExit.Click += AppMenuExit_Click;

            this.SetCheck(AppData.GetInstance().IsStart);
            if (this.cAppMenuStart.Checked) {
                this.StartHook();
            }
        }

        /// <summary>
        /// start global hook
        /// </summary>
        private void StartHook() {
            if (KeyboardGlobalHook.IsHooking) {
                return;
            }
            this.SetCheck(true);
            KeyboardGlobalHook.Start();
        }

        /// <summary>
        /// stop global hook
        /// </summary>
        private void StopHook() {
            if (!KeyboardGlobalHook.IsHooking) {
                return;
            }
            this.SetCheck(false);
            KeyboardGlobalHook.Stop();
        }

        /// <summary>
        /// check context menu and sava settings
        /// </summary>
        /// <param name="isStart">true:global hook is start, false:otherwise</param>
        private void SetCheck(bool isStart) {
            this.cAppMenuStart.Checked = isStart;
            this.cAppMenuStop.Checked = !isStart;
        }
        #endregion

    }
}
