using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyKeyChangerForAppleWireless {

    public partial class App : Application {
        #region Declaration
        private KeyChangerMain _keyChanger;
        #endregion

        #region Event
        /// <summary>
        /// System.Windows.Application.Startup
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            this._keyChanger = new KeyChangerMain();
        }

        /// <summary>
        /// System.Windows.Application.Exit
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e) {
            base.OnExit(e);
            this._keyChanger.Dispose();
        }
        #endregion

    }
}
