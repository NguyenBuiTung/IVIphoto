using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TakeimgIVI.Function
{
    public class LogError
    {
        // dùng lại
        #region GhiLog
        private static string ErrorFolder = Application.StartupPath + @"\ErrorLog\";
        private static string ErrorPath = ErrorFolder + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        static string s;
        public static void GhiLog(string log) //1 : INFO , 2:ERROR 
        {
            Directory.CreateDirectory(ErrorFolder);
            if (!File.Exists(ErrorPath))
            {
                File.Create(ErrorPath);
            }
            s = "[ERROR] :";
            StreamWriter stream = new StreamWriter(ErrorPath, true, Encoding.Default);
            stream.WriteLineAsync(DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff") + s + "\t" + log);
            stream.Close();
        }
        #endregion
    }

    public class Log
    {
        private static string Drieve = Directory.GetLogicalDrives()[1];
        private static string EventFolder = Drieve + "Log\\Event Log\\";
        private static string SystemFolder = Drieve + "Log\\System Log\\";
        private static string ErrorFolder = Drieve + "Log\\Error Log\\";
        private static string GmesFolder = Drieve + "Log\\Gmes Log\\";
        private static string link;
        private static string str;

        public static void Get_Current_Time()
        {
            //GMES_Data.CurrentTime = DateTime.Now.ToString("[yyyy:MM:dd HH:mm:ss:fff]");
        }
        public static void LogEvent(string log, int filepath, LogTitle title = LogTitle.None)
        {
            string EventPath = EventFolder + DateTime.Now.ToString("yyyyMMdd") + "-Event" + ".txt";
            string SystemPath = SystemFolder + DateTime.Now.ToString("yyyyMMdd") + "-System" + ".txt";
            string ErrorPath = ErrorFolder + DateTime.Now.ToString("yyyyMMdd") + "-Error" + ".txt";
            string GmesPath = GmesFolder + DateTime.Now.ToString("yyyyMMdd") + "-Gmes" + ".txt";
            if (!Directory.Exists(EventFolder))
            {
                Directory.CreateDirectory(EventFolder);
            }
            if (!Directory.Exists(SystemFolder))
            {
                Directory.CreateDirectory(SystemFolder);
            }
            if (!Directory.Exists(ErrorFolder))
            {
                Directory.CreateDirectory(ErrorFolder);
            }
            if (!Directory.Exists(GmesFolder))
            {
                Directory.CreateDirectory(GmesFolder);
            }
            try
            {
                if (filepath == 1) //LogEvent
                {
                    link = EventPath;
                }
                else if (filepath == 2) //LogProgram
                {
                    link = SystemPath;
                }
                else if (filepath == 3) //LogError
                {
                    link = ErrorPath;
                }
                else if (filepath == 4) //Gmeslog
                {
                    link = GmesPath;
                }
                else
                {
                    link = SystemPath;
                }
                if (!File.Exists(link))
                {
                    File.Create(link);
                }
                if(title != LogTitle.None)
                {
                    log = Status.FormatString($"[{Enum.GetName(typeof(LogTitle), title).ToUpper()}] : " + log);
                }
                using (StreamWriter stream = new StreamWriter(link, true, Encoding.Default)) { 
                    stream.WriteLine(log);
                }
            }
            catch
            {

            }
        }
    }

    public class DelegateToUI
    {
        public delegate void UpdateListBox(string s, LogTitle _tab);
        public static UpdateListBox update;
        public static void PushToListBox(string s, LogTitle _tab = LogTitle.Info)
        {
            update?.Invoke($"[{Enum.GetName(typeof(LogTitle), _tab).ToUpper()}] : " + s, _tab);
        }

        public delegate void UpdateLabel(string s);
        public static UpdateLabel _update_final_reusult_label;
        public static void UpdateFinalResultLabel(string s)
        {
            _update_final_reusult_label?.Invoke(s);
        }

        public delegate void UpdateProcessbar(int i);
        public static UpdateProcessbar _updateProcessBar;
        public static void UpdateProcessBar(int i)
        {
            _updateProcessBar?.Invoke(i);
        }

        public delegate void UpdateLabelJob(string s);
        public static UpdateLabelJob updateLabelJob;
        public static void UpdatelabelJob(string s)
        {
            updateLabelJob?.Invoke(s);
        }

        public delegate void UpdateProductCount();
        public static UpdateProductCount updateProductCount;
        public static void UIUpdateProductCount()
        {
            updateProductCount?.Invoke();
        }

        public delegate void UpdatePicture(Bitmap Truebmp);
        public static UpdatePicture updatePicture;
        public static void UIUpdatePicture(Bitmap Truebmp)
        {
            updatePicture?.Invoke(Truebmp);

        }

        public delegate void UpdateTaktTime();
        public static UpdateTaktTime updateTaktTime;
        public static void UIUpdateTaktTime()
        {
            updateTaktTime?.Invoke();

        }

        public delegate void UpdateProcessLabel(string str, Color background, string fore = "Lime");
        public static UpdateProcessLabel updateProcessLabel;
        public static void UIUpdateProcessLabel(string str, Color background, string fore = "Lime")
        {
            updateProcessLabel?.Invoke(str, background, fore);

        }

        public delegate void UpdateBarcodeValue(string s);
        public static UpdateBarcodeValue updateBarcodeValue;
        public static void UIUpdateBarcodeValue(string s)
        {
            updateBarcodeValue?.Invoke(s);
        }

        public delegate void _Reset_Display();
        public static _Reset_Display _reset_Display;
        public static void Reset_Display()
        {
            _reset_Display?.Invoke();
        }

        public delegate void _ClearLastResult();
        public static _ClearLastResult _clearLastResult;
        public static void ClearLastResult()
        {
            _clearLastResult?.Invoke();
        }

        public delegate void _Set_Value_And_Back_Color(int index, string state, string value);
        public static _Set_Value_And_Back_Color _set_Value_And_Back_Color;
        public static void Set_Value_And_Back_Color(int index, string state, string value)
        {
            _set_Value_And_Back_Color?.Invoke(index, state, value);
        }

        public delegate void UpdateProcessTime(double s);
        public static UpdateProcessTime updateprocesstime;
        public static void UIUpdateProcessTime(double s)
        {
            updateprocesstime?.Invoke(s);
        }

        public delegate void UpdateTextSimulation();
        public static UpdateTextSimulation updateTextSimulation;
        public static void UIUpdateTextSimulation()
        {
            updateTextSimulation?.Invoke();
        }

        public delegate void _TakeScreenShot(string path);
        public static _TakeScreenShot _takeScreenShot;
        public static void Take_Screen_Shot(string path)
        {
            _takeScreenShot?.Invoke(path);
        }

        public delegate void _SaveCameraView();
        public static _SaveCameraView _saveCameraView;
        public static void SaveCameraView()
        {
            _saveCameraView?.Invoke();
        }

        public delegate void _CloseForm();
        public static _CloseForm _closeForm;
        public static void CloseForm()
        {
            _closeForm?.Invoke();
        }

        public delegate void _PaintPanel(int index, string mode);
        public static _PaintPanel _paintPanel;
        public static void PaintPanel(int index, string mode)
        {
            _paintPanel?.Invoke(index, mode);
        }

        public delegate void _CloseManualForm();
        public static _CloseManualForm _closeManualForm;
        public static void CloseManualForm()
        {
            _closeManualForm?.Invoke();
        }

        public delegate void _AutoChangeJob(int index);
        public static _AutoChangeJob _autoChangeJob;
        public static void AutoChangeJob(int index)
        {
            _autoChangeJob?.Invoke(index);
        }

        public delegate void SaveImage();
        public static SaveImage saveImage;
        public static void UISaveImage()
        {
            saveImage?.Invoke();
        }

        public delegate void UpdateModelname(string modelname);
        public static UpdateModelname updateModelname;
        public static void Updatemodelname(string modelname)
        {
            updateModelname?.Invoke(modelname);
        }

        public delegate void UpdateColorlight(DO Do_id);
        public static UpdateColorlight updatecolorlight;
        public static void Updatecolorlight(DO Do_id)
        {
            updatecolorlight?.Invoke(Do_id);
        }
    }
}
