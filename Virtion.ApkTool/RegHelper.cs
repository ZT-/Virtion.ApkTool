using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Virtion.Util;

namespace Virtion.ApkTool
{
    public class RegHelper
    {
        public static bool RegisterApkSigner()
        {
            Register register = new Register();
            register.Domain = RegDomain.ClassesRoot;
            String regPath = @".apk\shell\Signed APK\Command";
            register.CreateSubKey(regPath);

            register.SubKey = regPath;
            if (register.WriteRegeditKey("", App.CurrentExe + " %1", RegValueKind.String) == true)
            {
                return false;
            }

            return true;
        }




    }
}
