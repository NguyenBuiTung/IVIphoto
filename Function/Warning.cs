using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using static TakeimgIVI.Function.DelegateToUI;

namespace TakeimgIVI.Function
{
	public class Warning
	{
        // dùng lại
		public static bool IsOpening = false;
		public static void Show(ERRORCODE code)
		{
            Updatecolorlight(DO.LIGHTRED);
            Status.State = STATE.BYPASS;
            if (!IsOpening)
			{
                Thread t = new Thread(() =>
                {
                    if (!CheckOpened("FormNotification"))
                    {
                        FormNotification f = new FormNotification(code);
                        f.ShowDialog();
                        f.BringToFront();
                    }
                });
                t.IsBackground = true;
                t.Start();
            }
			else
			{   
				new Thread(()=>
				{
					Thread.Sleep(100);
					CheckOpened("FormNotification");
				})
				{ IsBackground= true }.Start();
            }
            IsOpening = true;
        }

        public static void Show(string code)
        {
            Updatecolorlight(DO.LIGHTRED);
            Status.State = STATE.BYPASS;
            if (!CheckOpened("FormNotification"))
            {
                FormNotification f = new FormNotification(code.ToUpper());
                f.ShowDialog();
                f.BringToFront();
            }
        }

        public static bool CheckOpened(string name)
		{
			FormCollection fc = Application.OpenForms;
			foreach (Form frm in fc)
			{
				if (frm.Name.Contains(name))
				{
					frm.BeginInvoke(new Action(() =>
					{
						frm.BringToFront();
					}));
					return true;
				}
			}
			return false;
		}

        public static bool CloseForm(string name = "FormNotification")
        {
            FormCollection fc = Application.OpenForms;
            foreach (Form frm in fc)
            {
                if (frm.Name.Contains(name))
                {
                    frm.BeginInvoke(new Action(() =>
                    {
                        frm.Close();
                    }));
                    return true;
                }
            }
            return false;
        }


    }
	public enum ERRORCODE
	{
		NO_BARCODE,
		NO_IMAGE,
		DIS_SCAN,
		DIS_CAMERA1,
        DIS_CAMERA2,
        TIME_OUT_TAKE_IMAGE,
        TAKE_IMAGE_ERR,
        DIS_ADAM,
        EARLY_PCS_REMOVAL,
        NONE
	}
}
