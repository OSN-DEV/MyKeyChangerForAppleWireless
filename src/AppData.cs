using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLib.Data;

namespace MyKeyChangerForAppleWireless {
    public class AppData : AppDataBase<AppData> {

        #region Declration
        private static string _settingFile = GetAppPath() + @"\app.data";

        public bool IsStart { set; get; } = false;
        #endregion

        #region Constructor
        #endregion

        #region Public Method
        public static new AppData GetInstance() {
            return AppDataBase<AppData>.GetInstance(_settingFile);
        }

        /// <summary>
        /// save settings to xml file
        /// </summary>
        public void Save() {
            base.SaveToXml(_settingFile, this);
        }
        #endregion

        #region Private Method
        /// <summary>
        /// get app binary path
        /// </summary>
        /// <returns>app executable path</returns>
        public static string GetAppPath() {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (!path.EndsWith(@"\")) {
                path += @"\";
            }
            return path;
        }
        #endregion
    }
}
