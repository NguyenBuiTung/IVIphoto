using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using TakeimgIVI.Function;

namespace TakeimgIVI
{
    class GMES_Pakage
    {
        public static XmlDocument xmlDoc = new XmlDocument();
        private static string Barcodebuffer { set; get; }
        private static string ResultBuffer { set; get; }

        public static System.Timers.Timer WaitS6F5;
        public static System.Timers.Timer WaitTriggerCam;
        public static void DocLoad()
        {
            try
            {
                string getinfo = "S1F1";
                xmlDoc.Load(Application.StartupPath + @"\DataPackage\TuyenPacket.xml");
                XmlElement xml = xmlDoc.DocumentElement;
                IEnumerator enumerator;
                enumerator = xml.ChildNodes.GetEnumerator();
                Send_GMES_Server(CheckID(ref getinfo));
            }
            catch (Exception ex)
            {
                DelegateToUI.update("[ERROR]" + ex.Message, LogTitle.Gmes);
            }
        }
        public static XmlElement ChangeXmlElement(XElement el)
        {
            XmlDocument document = new XmlDocument();
            document.Load(el.CreateReader());
            return document.DocumentElement;
        }

        public static string CheckID(ref string TempStr)
        {
            if (Status.GMES)
            {
                IEnumerator enumerator = null;
                XmlElement documentElement = xmlDoc.DocumentElement;
                try
                {
                    enumerator = documentElement.ChildNodes.GetEnumerator();
                    while (true)
                    {
                        if (!enumerator.MoveNext())
                        {
                            break;
                        }
                        XmlElement current = (XmlElement)enumerator.Current;
                        if (current.Attributes.Item(1).Value == TempStr)
                        {
                            if (TempStr != "S2F4" && TempStr != "S2F32" && TempStr != "S6F6")
                            {
                                GMES_Socket.Start_TimeOut_Timer();
                            }
                            string str2 = current.Attributes.Item(2).Value;
                            return current.OuterXml;
                        }
                    }
                }
                catch (Exception ex)
                {
                    DelegateToUI.update("[ERROR]" + ex.Message, LogTitle.Gmes);
                }
                finally
                {
                    if (enumerator is IDisposable)
                    {
                        (enumerator as IDisposable).Dispose();
                    }
                }
                return null;
            }
            else
                return null;
        }
        public static bool WaitS5F5TooLongBool;
        private static void WaitS6F5TooLong(object obj, System.Timers.ElapsedEventArgs ev)
        {
            WaitS5F5TooLongBool = true;
            Thread.Sleep(1000);
            Warning.Show("Long response from GMES");
            DelegateToUI.update("S6F5 information is waiting too long...", LogTitle.Error);
            WaitS6F5.Enabled = false;
            WaitS6F5.Stop();
        }
        private static void WaitTriggerTooLong(object obj, System.Timers.ElapsedEventArgs ev)
        {
            if (!WaitTriggerCam.Enabled)
            {
                WaitTriggerCam.Stop();
                return;
            }
            DelegateToUI.update("Resend camera trigger", LogTitle.Info);
            //Programm.Line.UI.Excute_Data();
            try
            {
                if (WaitTriggerCam.Enabled)
                {
                    Warning.Show("Long response from Camera Trigger");
                    DelegateToUI.update("Camera Trigger is waiting too long...", LogTitle.Info);
                    WaitTriggerCam.Enabled = false;
                    WaitTriggerCam.Stop();
                }
                else
                {
                    WaitTriggerCam.Stop();
                }
            }
            catch { }
        }
        public static bool S6F5Ready;
        public static void MSG_Send(string TempMSG)
        {
            if (TempMSG != "")
            {
                try
                {
                    XmlElement element2 = ChangeXmlElement(XElement.Parse(TempMSG));
                    string s = element2.Attributes.Item(1).Value;
                    if (s == "S1F2") //RECV
                    {
                        GMES_Data.EQPID = element2.SelectSingleNode("ELEMENT").SelectSingleNode("EQPID").InnerText;
                        GMES_Data.EQPNAME = element2.SelectSingleNode("ELEMENT").SelectSingleNode("EQPNAME").InnerText;
                        GMES_Data.PROCID = element2.SelectSingleNode("ELEMENT").SelectSingleNode("PROCID").InnerText;
                        GMES_Data.ORGID = element2.SelectSingleNode("ELEMENT").SelectSingleNode("ORGID").InnerText;
                        GMES_Data.LINEID = element2.SelectSingleNode("ELEMENT").SelectSingleNode("LINEID").InnerText;
                        DelegateToUI.update("RECV S1F2 Pakage Data From GMES Successfully !", LogTitle.Info);
                        //Programm.Line.UI.UpdateInfoGMES();
                    }
                    else if (s == "S2F3") //RECV -> feedback 
                    {
                        string reply = "S2F4";
                        element2 = ChangeXmlElement(XElement.Parse(CheckID(ref reply)));
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("EQPID").InnerText = GMES_Data.EQPID;
                        Send_GMES_Server(element2.OuterXml);
                    }
                    else if (s == "S2F31")
                    {
                        string reply = "S2F32";
                        string innerText = element2.SelectSingleNode("ITEM").SelectSingleNode("TIME").InnerText;
                        element2 = ChangeXmlElement(XElement.Parse(CheckID(ref reply)));
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("EQPID").InnerText = GMES_Data.EQPID;
                        element2.SelectSingleNode("ITEM").SelectSingleNode("ACK").InnerText = TimeSet((long)Conversions.ToInteger(Strings.Left(innerText, 4)), (long)Conversions.ToInteger(Strings.Mid(innerText, 5, 2)), (long)Conversions.ToInteger(Strings.Mid(innerText, 7, 2)), (long)Conversions.ToInteger(Strings.Mid(innerText, 9, 2)), (long)Conversions.ToInteger(Strings.Mid(innerText, 11, 2)), (long)Conversions.ToInteger(Strings.Mid(innerText, 13, 2))) ? "0" : "1";
                        Send_GMES_Server(element2.OuterXml);
                    }
                    else if (s == "S6F11") //Send Barcode
                    {
                        //if (WaitTriggerCam != null)
                        //{
                        //    if (GMES_Pakage.WaitTriggerCam.Enabled)
                        //    {
                        //        GMES_Pakage.WaitTriggerCam.Enabled = false;
                        //        GMES_Pakage.WaitTriggerCam.Stop();
                        //    }
                        //}

                        //frmMain.IsExecuting = false;
                        element2 = ChangeXmlElement(XElement.Parse(CheckID(ref s)));
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("EQPID").InnerText = GMES_Data.EQPID;
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("CEID").InnerText = GMES_Data.CEID;
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("RPTID").InnerText = GMES_Data.RPTID;
                        element2.SelectSingleNode("ITEM").SelectSingleNode("SUBITEMLIST").Attributes.Item(0).Value = "2";
                        XmlNode newChild = element2.SelectSingleNode("ITEM").SelectSingleNode("SUBITEMLIST").SelectSingleNode("NAME").Clone();
                        XmlNode node4 = element2.SelectSingleNode("ITEM").SelectSingleNode("SUBITEMLIST").SelectSingleNode("VALUE").Clone();
                        int num6 = Conversions.ToInteger(element2.SelectSingleNode("ITEM").SelectSingleNode("SUBITEMLIST").Attributes.Item(0).Value) - 1;
                        int num7 = 0;
                        while (true)
                        {
                            if (num7 > num6)
                            {
                                element2.LastChild.FirstChild.SelectSingleNode("NAME").InnerText = "SETID";
                                element2.LastChild.FirstChild.SelectSingleNode("VALUE").InnerText = Barcodebuffer;
                                element2.LastChild.LastChild.ChildNodes.Item(2).InnerText = "SPEC_DOWNLOAD_YN";
                                element2.LastChild.LastChild.ChildNodes.Item(3).InnerText = "Y";
                                Send_GMES_Server(element2.OuterXml);
                                break;
                            }
                            element2.SelectSingleNode("ITEM").SelectSingleNode("SUBITEMLIST").AppendChild(newChild);
                            element2.SelectSingleNode("ITEM").SelectSingleNode("SUBITEMLIST").AppendChild(node4);
                            num7++;
                        }
                        //Programm.Line.UI.UpdateProcessBar(20);
                        DelegateToUI.update("Send Barcode to GMES", LogTitle.Send);
                        if (WaitS6F5 == null)
                        {
                            //WaitS6F5 = new System.Timers.Timer(3000);
                            //WaitS6F5.Elapsed += WaitS6F5TooLong;
                            //WaitS6F5.Enabled = true;
                            //WaitS6F5.Start();
                        }
                        else if (!WaitS6F5.Enabled)
                        {
                            //WaitS6F5.Enabled = true;
                            //WaitS6F5.Start();
                        }

                        if (WaitTriggerCam == null)
                        {
                            //WaitTriggerCam = new System.Timers.Timer(5000);
                            //WaitTriggerCam.Elapsed += WaitTriggerTooLong;
                            //WaitTriggerCam.Enabled = true;
                            //WaitTriggerCam.Start();
                        }
                        else if (!WaitTriggerCam.Enabled)
                        {
                            //WaitTriggerCam.Enabled = true;
                            //WaitTriggerCam.Start();
                        }

                    }
                    else if (s == "S6F12") //RECV Event Report Acknowledge
                    {
                        GMES_Data.Received_Data = true;

                        if (element2.SelectSingleNode("ITEM").SelectSingleNode("ACK").InnerText != "1") //check Barcode OK
                        {
                            DelegateToUI.update("GMES Check Barcode Successfully !", LogTitle.Info);
                            GMES_Data.ACK = "0";
                            //Programm.Line.UI.UpdateProcessBar(30);
                        }
                        else  //Báo lỗi đèn còi ra PLC
                        {
                            GMES_Data.ACK = "1";
                            GMES_Data.Reason = element2.SelectSingleNode("ITEM").SelectSingleNode("REASON").InnerText;
                            Warning.Show(GMES_Data.Reason);
                            DelegateToUI.update(GMES_Data.Reason, LogTitle.Error);
                            //Programm.Line.UI.UpdateProcessBar(15, "GMES Not Acceped, Skip Product...");
                            DelegateToUI.UIUpdateProcessLabel("NG",Color.Black,"Red");
                            //Programm.Line.UI.UpdateProcessBar(0);
                        }
                    }
                    else if (s == "S6F5") //RECV Product Data
                    {
                        try
                        {
                            //Thread.Sleep(4000);
                            //WaitS5F5TooLongBool = false;
                            //WaitS6F5.Enabled = false;
                            //WaitS6F5.Stop();
                        }
                        catch { }
                        XmlNode x = element2.SelectSingleNode("ITEM").SelectSingleNode("SUBITEMLIST");
                        GMES_Data.TID = element2.SelectSingleNode("ELEMENT").SelectSingleNode("TID").InnerText;
                        GMES_Data.WOID = element2.SelectSingleNode("ELEMENT").SelectSingleNode("WOID").InnerText;
                        GMES_Data.ORGID = element2.SelectSingleNode("ELEMENT").SelectSingleNode("ORGID").InnerText;
                        GMES_Data.LINEID = element2.SelectSingleNode("ELEMENT").SelectSingleNode("LINEID").InnerText;
                        GMES_Data.EQPID = element2.SelectSingleNode("ELEMENT").SelectSingleNode("EQPID").InnerText;
                        GMES_Data.PROCID = element2.SelectSingleNode("ELEMENT").SelectSingleNode("PROCID").InnerText;
                        GMES_Data.SUFFIX = element2.SelectSingleNode("ELEMENT").SelectSingleNode("MODEL").SelectSingleNode("SUFFIX").InnerText;
                        GMES_Data.ID = element2.SelectSingleNode("ELEMENT").SelectSingleNode("MODEL").SelectSingleNode("ID").InnerText;
                        GMES_Data.SETID = element2.SelectSingleNode("ELEMENT").SelectSingleNode("SETID").InnerText;
                        GMES_Data.MODEL.NAME = element2.SelectSingleNode("ELEMENT").SelectSingleNode("MODEL").SelectSingleNode("NAME").InnerText;
                        GMES_Data.Get_Product_Infomation(x);
                        //Programm.Line.UI.Update_Gmes_Value();
                        //Programm.Line.UI.UpdateProcessBar(40);
                        DelegateToUI.update("Received Product Infomation Success", LogTitle.Info);
                        GMES_Data.Received_Data = true;
                        SendVisionAccept();
                        //Programm.Line.UI.UpdateProcessBar(15, "Checking Vision Job...");
                        //Programm.Camera_Excute.AutoSettingJob(); //Auto change Model TESTER
                        DelegateToUI.update("Received S6F5 successfully", LogTitle.Info);
                        //Programm.Line.UI.UpdateInfoProduct(); //Update barcode, model name TESTER

                    }
                    else if (s == "S6F6") //Send
                    {
                        element2 = ChangeXmlElement(XElement.Parse(CheckID(ref s)));
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("TID").InnerText = GMES_Data.TID;
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("WOID").InnerText = GMES_Data.WOID;
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("ORGID").InnerText = GMES_Data.ORGID;
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("LINEID").InnerText = GMES_Data.LINEID;
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("EQPID").InnerText = GMES_Data.EQPID;
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("PROCID").InnerText = GMES_Data.PROCID;
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("MODEL").SelectSingleNode("ID").InnerText = GMES_Data.ID;
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("MODEL").SelectSingleNode("SUFFIX").InnerText = GMES_Data.SUFFIX;
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("SETID").InnerText = GMES_Data.SETID;
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("MODEL").SelectSingleNode("NAME").InnerText = GMES_Data.MODEL.NAME;
                        //Programm.Line.UI.UpdateProcessBar(70);
                        Send_GMES_Server(element2.OuterXml);

                    }
                    else if (s == "S6F1") //Send Result Vision Check
                    {
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("TID").InnerText = GMES_Data.TID;
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("WOID").InnerText = GMES_Data.WOID;
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("ORGID").InnerText = GMES_Data.ORGID;
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("LINEID").InnerText = GMES_Data.LINEID;
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("EQPID").InnerText = GMES_Data.EQPID;
                        element2.SelectSingleNode("ELEMENT").SelectSingleNode("PROCID").InnerText = GMES_Data.PROCID;
                        element2.SelectSingleNode("ITEM").SelectSingleNode("SUBITEMLIST").Attributes.Item(0).Value = "2";
                        element2.SelectSingleNode("ITEM").SelectSingleNode("SUBITEMLIST").SelectSingleNode("NAME").InnerText = "RESULT";
                        element2.SelectSingleNode("ITEM").SelectSingleNode("SUBITEMLIST").SelectSingleNode("VALUE").InnerText = ResultBuffer;
                        if (true)
                        {
                            element2.SelectSingleNode("ELEMENT").SelectSingleNode("MODEL").SelectSingleNode("ID").InnerText = GMES_Data.ID;
                            element2.SelectSingleNode("ELEMENT").SelectSingleNode("MODEL").SelectSingleNode("SUFFIX").InnerText = GMES_Data.SUFFIX;
                            element2.SelectSingleNode("ELEMENT").SelectSingleNode("MODEL").SelectSingleNode("NAME").InnerText = GMES_Data.MODEL.NAME;
                            element2.SelectSingleNode("ELEMENT").SelectSingleNode("SETID").InnerText = GMES_Data.SETID;
                            DelegateToUI.update("Send Result Vision to GMES", LogTitle.Send);
                            //Programm.Line.UI.UpdateProcessBar(90);
                        }
                        Send_GMES_Server(element2.OuterXml);
                    }
                    else if (s == "S6F2") // RECV 
                    {
                        string setid = element2.SelectSingleNode("ELEMENT").SelectSingleNode("SETID").InnerText;
                        if (!(element2.SelectSingleNode("ITEM").SelectSingleNode("ACK").InnerText == "1"))
                        {
                            if (setid == GMES_Data.Barcode)
                            {
                                DelegateToUI.update("Sending Inspection to GMES Successfully !", LogTitle.Info);
                                //Programm.Line.UI.UpdateProcessBar(100);
                            }
                        }
                        else  //Báo lỗi đèn còi ra PLC
                        {
                            if (setid == GMES_Data.Barcode)
                            {
                                DelegateToUI.UIUpdateProcessLabel("NG", Color.Black, "Red");
                                GMES_Data.Reason = element2.SelectSingleNode("ITEM").SelectSingleNode("REASON").InnerText;
                                DelegateToUI.update(GMES_Data.Reason, LogTitle.Error);
                                //Programm.Line.UI.UpdateProcessBar(0);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    GMES_Data.CurrentTime = DateTime.Now.ToString("[yyyy:MM:dd HH:mm:ss:fff]");
                    DelegateToUI.update("[ERROR]" + ex.Message, LogTitle.Gmes);
                }
            }
        }
        public static void SendBarcodeValue(string barcode)
        {
            string s = "S6F11";
            Barcodebuffer = barcode;
            MSG_Send(CheckID(ref s));
        }
        public static void SendVisionAccept()
        {
            string s = "S6F6";
            MSG_Send(CheckID(ref s));
        }
        public static void SendValueCheck(string _result)
        {
            ResultBuffer = _result;
            string s = "S6F1";
            MSG_Send(CheckID(ref s));
        }
        public static string Send_GMES_Server(string TempXML)
        {
            try
            {
                string strSendData = "";
                if (TempXML != "")
                {
                    string[] strArray = Strings.Split(TempXML, "</EIF>", -1, CompareMethod.Binary);
                    int index = 0;
                    while (true)
                    {
                        if (strArray[index] != "")
                        {
                            strSendData = strArray[index] + "</EIF>";
                            GMES_Socket.SendToServer(ref strSendData);
                            index++;
                            if (index <= 10)
                            {
                                continue;
                            }
                        }
                        break;
                    }
                }
                return strSendData;
            }
            catch (Exception ex)
            {
                GMES_Data.CurrentTime = DateTime.Now.ToString("[yyyy:MM:dd HH:mm:ss:fff]");
                DelegateToUI.update("[ERROR]" + ex.Message, LogTitle.Gmes);
                return null;
            }
        }

        public static bool TimeSet(long TempYear, long TempMonth, long TempDay, long TempHour, long TempMinute, long TempSecond)
        {
            bool flag;
            try
            {
                string[] textArray1 = new string[] { Conversions.ToString(TempYear), "-", Conversions.ToString(TempMonth), "-", Conversions.ToString(TempDay) };
                DateAndTime.Today = Conversions.ToDate(string.Concat(textArray1));
                string[] textArray2 = new string[] { Conversions.ToString(TempHour), ":", Conversions.ToString(TempMinute), ":", Conversions.ToString(TempSecond) };
                DateAndTime.TimeString = string.Concat(textArray2);
                flag = true;
            }
            catch (Exception exception1)
            {
                MessageBox.Show(exception1.Message);
                flag = false;
            }
            return flag;
        }

    }
}
