using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeimgIVI
{
    public class Camera_Parameter
    {
        public static int MV_UNKNOW_DEVICE = Convert.ToInt32(0x00000000);

        public static int MV_GIGE_DEVICE = Convert.ToInt32(0x00000001);

        public static int MV_1394_DEVICE = Convert.ToInt32(0x00000002);

        public static int MV_USB_DEVICE = Convert.ToInt32(0x00000004); // usb 3.0

        public static int MV_CAMERALINK_DEVICE = Convert.ToInt32(0x00000008);

        public static int MV_OK = Convert.ToInt32(0x00000000);

        // 0x80000000-0x800000FF
        /// Error or invalid handle
        public static int MV_E_HANDLE = Convert.ToInt32(0x80000000);
        /// Not supported function
        public static int MV_E_SUPPORT = Convert.ToInt32(0x80000001);
        /// Buffer overflow
        public static int MV_E_BUFOVER = Convert.ToInt32(0x80000002);
        /// Function calling order error
        public static int MV_E_CALLORDER = Convert.ToInt32(0x80000003);
        /// Incorrect parameter
        public static int MV_E_PARAMETER = Convert.ToInt32(0x80000004);
        /// Applying resource failed
        public static int MV_E_RESOURCE = Convert.ToInt32(0x80000006);
        /// No data
        public static int MV_E_NODATA = Convert.ToInt32(0x80000007);
        /// Precondition error, or running environment changed
        public static int MV_E_PRECONDITION = Convert.ToInt32(0x80000008);
        /// Version mismatches
        public static int MV_E_VERSION = Convert.ToInt32(0x80000009);
        /// Insufficient memory
        public static int MV_E_NOENOUGH_BUF = Convert.ToInt32(0x8000000A);
        /// Abnormal image, maybe incomplete image because of lost packet
        public static int MV_E_ABNORMAL_IMAGE = Convert.ToInt32(0x8000000B);
        /// Load library failed
        public static int MV_E_LOAD_LIBRARY = Convert.ToInt32(0x8000000C);
        /// No Avaliable Buffer
        public static int MV_E_NOOUTBUF = Convert.ToInt32(0x8000000D);
        /// Encryption error
        public static int MV_E_ENCRYPT = Convert.ToInt32(0x8000000E);
        /// Unknown error
        public static int MV_E_UNKNOW = Convert.ToInt32(0x800000FF);

        // GenICam 0x80000100-0x800001FF
        /// General error
        public static int MV_E_GC_GENERIC = Convert.ToInt32(0x80000100);
        /// Illegal parameters
        public static int MV_E_GC_ARGUMENT = Convert.ToInt32(0x80000101);
        /// The value is out of range
        public static int MV_E_GC_RANGE = Convert.ToInt32(0x80000102);
        /// Property
        public static int MV_E_GC_PROPERTY = Convert.ToInt32(0x80000103);
        /// Running environment error
        public static int MV_E_GC_RUNTIME = Convert.ToInt32(0x80000104);
        /// Logical error
        public static int MV_E_GC_LOGICAL = Convert.ToInt32(0x80000105);
        /// Node accessing condition error
        public static int MV_E_GC_ACCESS = Convert.ToInt32(0x80000106);
        /// Timeout
        public static int MV_E_GC_TIMEOUT = Convert.ToInt32(0x80000107);
        /// Transformation exception
        public static int MV_E_GC_DYNAMICCAST = Convert.ToInt32(0x80000108);
        /// GenICam unknown error
        public static int MV_E_GC_UNKNOW = Convert.ToInt32(0x800001FF);

        // GigE_STATUS 0x80000200-0x800002FF
        /// The command is not supported by device
        public static int MV_E_NOT_IMPLEMENTED = Convert.ToInt32(0x80000200);
        /// The target address being accessed does not exist
        public static int MV_E_INVALID_ADDRESS = Convert.ToInt32(0x80000201);
        /// The target address is not writable
        public static int MV_E_WRITE_PROTECT = Convert.ToInt32(0x80000202);
        /// No permission
        public static int MV_E_ACCESS_DENIED = Convert.ToInt32(0x80000203);
        /// Device is busy, or network disconnected
        public static int MV_E_BUSY = Convert.ToInt32(0x80000204);
        /// Network data packet error
        public static int MV_E_PACKET = Convert.ToInt32(0x80000205);
        /// Network error
        public static int MV_E_NETER = Convert.ToInt32(0x80000206);
        /// Device IP conflict
        public static int MV_E_IP_CONFLICT = Convert.ToInt32(0x80000221);

        // USB_STATUS 0x80000300-0x800003FF
        /// Reading USB error
        public static int MV_E_USB_READ = Convert.ToInt32(0x80000300);
        /// Writing USB error
        public static int MV_E_USB_WRITE = Convert.ToInt32(0x80000301);
        /// Device exception
        public static int MV_E_USB_DEVICE = Convert.ToInt32(0x80000302);
        /// GenICam error
        public static int MV_E_USB_GENICAM = Convert.ToInt32(0x80000303);
        /// Insufficient bandwidth, this error code is newly added
        public static int MV_E_USB_BANDWIDTH = Convert.ToInt32(0x80000304);
        /// Driver mismatch or unmounted drive
        public static int MV_E_USB_DRIVER = Convert.ToInt32(0x80000305);
        /// USB unknown error
        public static int MV_E_USB_UNKNOW = Convert.ToInt32(0x800003FF);

        // 0x80000400-0x800004FF
        /// Firmware mismatches
        public static int MV_E_UPG_FILE_MISMATCH = Convert.ToInt32(0x80000400);
        /// Firmware language mismatches
        public static int MV_E_UPG_LANGUSGE_MISMATCH = Convert.ToInt32(0x80000401);
        /// Upgrading conflicted (repeated upgrading requests during device upgrade)
        public static int MV_E_UPG_CONFLICT = Convert.ToInt32(0x80000402);
        /// Camera internal error during upgrade
        public static int MV_E_UPG_INNER_ERR = Convert.ToInt32(0x80000403);
        /// Unknown error during upgrade
        public static int MV_E_UPG_UNKNOW = Convert.ToInt32(0x800004FF);
    }

   

}
