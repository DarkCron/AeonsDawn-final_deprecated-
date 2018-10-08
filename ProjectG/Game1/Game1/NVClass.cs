using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TBAGW
{
    class NVClass
    {
        [DllImport("nvapi.dll")]
        public static extern int NvAPI_Initialize();
    }
}
