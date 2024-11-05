using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TakeimgIVI
{
    public class Lens2
    {
        LC_Controller2.Form1 frLens;
        public int INDEX;
        public bool Status = false;
        Thread check;
        public Lens2(int index)
        {
            INDEX = index;
            frLens?.Dispose();

            frLens = new LC_Controller2.Form1(index);

            Thread.Sleep(100);
            frLens.LOADFIRST(index);

            check = new Thread(() =>
            {
                while (true)
                {
                    CheckConnect();
                    Thread.Sleep(1000);
                }
            })
            { IsBackground = true };
            check.Start();
        }

        bool shown = false;
        public void ShowController()
        {
            frLens.Visible = true;
            frLens.BringToFront();
        }

        public bool Reconnect()
        {
            Status = frLens.LOADFIRST(INDEX);
            return Status;
        }

        public bool CheckConnect()
        {
            Status = frLens.checkcn();
            if(INDEX == 0) { Function.Status.Lens1 = Status; }
            else if(INDEX == 1) {  Function.Status.Lens2 = Status; }
            return Status;
        }

        public bool UpdateLens(ushort focus, ushort iris)
        {
            bool fc = UpdateFocus(focus);
            Thread.Sleep(100);
            bool Ir = UpdateIris(iris);
            return fc && Ir;
        }

        public bool UpdateFocus(ushort focus)
        {
            if (!CheckConnect())
            {
                if (!Reconnect())
                    return false;
            }
            return frLens.UpdateFocus(focus);
        }

        public bool UpdateIris(ushort iris)
        {
            if (!CheckConnect())
            {
                if (!Reconnect())
                    return false;
            }
            return frLens.UpdateIris(iris);
        }
    }
}
