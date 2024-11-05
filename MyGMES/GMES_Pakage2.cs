using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Script.Serialization;
using TakeimgIVI.Function;

namespace TakeimgIVI
{
    public class GMES_Pakage2
    {
        public static string GetMessString(object Mess)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include };
            string jsonstring = JsonConvert.SerializeObject(Mess, Formatting.Indented, jsonSerializerSettings);
            //return GMES_JSON.Serialize(Mess);
            return jsonstring;
        }
        public static string Send_GMES_Server(object Mess)
        {

            try
            {

                string TempJSON = GetMessString(Mess);
                //TempJSON.ToCharArray
                //TempJSON = TempJSON.Replace("\"", "");
                GMES_Socket.SendToServer(ref TempJSON);
                return TempJSON;
            }
            catch (Exception ex)
            {
                GMES_Data.CurrentTime = DateTime.Now.ToString("[yyyy:MM:dd HH:mm:ss:fff]");
                DelegateToUI.update("[ERROR]" + ex.Message, LogTitle.Gmes);
                return null;
            }
        }
        public static void GetData(string Message)
        {
            //Programm.Line.UI.UpdateTestGMES(Message, LogTitle.Recv);
            string sub = Message.Substring(Message.IndexOf("\"") + 1);
            string OjbName = sub.Substring(0, sub.IndexOf("\""));
            try
            {
                //GMES_Data gMES_Data = new GMES_Data();
                //System.Reflection.MemberInfo[] propertyInfos = gMES_Data.GetType().GetMembers();

                string SubItemText = "";
                int start;
                int end;
                JavaScriptSerializer obj = new JavaScriptSerializer();
                if (Message.Contains("SUBITEM_LIST"))
                {
                    start = Message.IndexOf("[");
                    end = Message.IndexOf("]");
                    SubItemText = Message.Substring(start, end - start + 1);
                    Message = Message.Remove(start + 1, end - start - 1);
                }
                switch (OjbName)
                {
                    case "EAYT":
                        GMES_Data.EAYT = obj.Deserialize<GMES_EAYT>(Message);
                        break;
                    case "EAYT_R":
                        GMES_Data.EAYT_R = obj.Deserialize<GMES_EAYT_R>(Message);
                        break;
                    case "ELNT":
                        GMES_Data.ELNT = obj.Deserialize<GMES_ELNT>(Message);
                        break;
                    case "ELNT_R":
                        GMES_Data.ELNT_R = obj.Deserialize<GMES_ELNT_R>(Message);
                        break;
                    case "EERS":
                        GMES_Data.EERS = obj.Deserialize<GMES_EERS>(Message);
                        break;
                    case "EERS_R":
                        GMES_Data.EERS_R = obj.Deserialize<GMES_EERS_R>(Message);
                        break;
                    case "EPDS":
                        GMES_Data.SUBITEM_LIST = new List<ITEM>(16);
                        GMES_Data.EPDS = obj.Deserialize<GMES_EPDS>(Message);
                        break;
                    case "EPDS_R":
                        GMES_Data.EPDS_R = obj.Deserialize<GMES_EPDS_R>(Message);
                        break;
                    case "EESR":
                        GMES_Data.EESR = obj.Deserialize<GMES_EESR>(Message);
                        break;
                    case "EESR_R":
                        GMES_Data.EESR_R = obj.Deserialize<GMES_EESR_R>(Message);
                        break;
                    case "EAPD":
                        GMES_Data.EAPD = obj.Deserialize<GMES_EAPD>(Message);
                        break;
                    case "EAPD_R":
                        GMES_Data.EAPD_R = obj.Deserialize<GMES_EAPD_R>(Message);
                        break;
                    case "EIDS":
                        GMES_Data.EIDS = obj.Deserialize<GMES_EIDS>(Message);
                        break;
                    case "EIDS_R":
                        GMES_Data.EIDS_R = obj.Deserialize<GMES_EIDS_R>(Message);
                        break;
                    case "EDTS":
                        GMES_Data.EDTS = obj.Deserialize<GMES_EDTS>(Message);
                        break;
                    case "EDTS_R":
                        GMES_Data.EDTS_R = obj.Deserialize<GMES_EDTS_R>(Message);
                        break;
                    case "EEMR":
                        GMES_Data.EEMR = obj.Deserialize<GMES_EEMR>(Message);
                        break;
                    case "EEMR_R":
                        GMES_Data.EEMR_R = obj.Deserialize<GMES_EEMR_R>(Message);
                        break;
                    default:
                        DelegateToUI.update($"ERROR: GMES command has not Subcribe yet: {OjbName}", LogTitle.Gmes);
                        break;
                }
                if (Message.Contains("SUBITEM_LIST"))
                {
                    JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
                    GMES_Data.SUBITEM_LIST = JsonConvert.DeserializeObject<List<ITEM>>(SubItemText,jsonSerializerSettings);
                }

                Thread Execute = new Thread(() => ExecuteAndReply(OjbName));
                Execute.IsBackground = true;
                Execute.Start();

            }
            catch (Exception ex)
            {
                DelegateToUI.update("Error when convert data from GMES: " + ex.Message, LogTitle.Gmes);
            }

        }
        public static void ExecuteAndReply(string Ojb)
        {
            switch (Ojb)
            {
                case "EAYT_R":
                    //Programm.Line.UI.UpdateInfoGMES();
                    break;
                case "ELNT":
                    Send_GMES_Server(GMES_Data.ELNT_R);
                    break;
                case "EERS_R":
                    if (GMES_Data.ACK != "0")
                    {
                        Warning.Show("GMES Notification:" + "\n" + "ACK: Not Accepted" + "\nREASON: " + GMES_Data.REASON);
                    }
                    GMES_Data.Received_Data = true;
                    break;
                case "EPDS":
                    //Programm.Line.UI.UpdateProcessBar(15, "Recevied Data");
                    //GMES_Data.Get_Product_Infomation();
                    //Programm.Line.UI.UpdateInfoProduct();
                    GMES_Data.ACK = "0";
                    GMES_Data.REASON = "";
                    Send_GMES_Server(GMES_Data.EPDS_R);
                    GMES_Data.EQPSTATE = "R";
                    Send_GMES_Server(GMES_Data.EESR);
                    //Programm.Line.UI.UpdateProcessBar(30, "Checking Vision Job...");
                    //Programm.Camera_Excute.AutoSettingJob(); CHANGE JOB //TESTER
                    break;
                case "EESR_R":
                    break;
                case "EAPD_R":
                    break;
                case "EIDS_R":
                    GMES_Data.EQPSTATE = "I";
                    Send_GMES_Server(GMES_Data.EESR);
                    break;
                case "EDTS":
                    GMES_Data.ACK = "0";
                    Send_GMES_Server(GMES_Data.EDTS_R);
                    GMES_Data.CONTROLSTATE = "O";
                    Send_GMES_Server(GMES_Data.EEMR);
                    break;
                case "EEMR_R":
                    GMES_Data.EQPSTATE = "I";
                    Send_GMES_Server(GMES_Data.EESR);
                    break;
                default:
                    DelegateToUI.update($"Error: Can not execute GMES request: {Ojb}", LogTitle.Gmes);
                    break;
            }

        }
        public static void WaitAndResend(string WaitString, object ResendObj, int Times, int Delay)
        {
            Thread WnR = new Thread(() =>
            {
                if (WaitString == "EPDS")
                {
                    for (int i = 0; i < Times; i++)
                    {
                        Thread.Sleep(Delay);
                        if (!GMES_Data.Ready_To_Online_Check)
                        {
                            Send_GMES_Server(ResendObj);
                        }
                        else
                        {
                            return;
                        }
                    }
                    GMES_Data.ACK = "1";
                    GMES_Data.REASON = "Time out GMES\nWait for: " + WaitString;
                    Warning.Show(GMES_Data.REASON);
                    GMES_Data.Received_Data = true;

                }
            });

        }
    }

    #region GMES_Object_Class
    public class GMES_EAYT
    {
        public _EAYT EAYT = new _EAYT();
    }
    public class GMES_EAYT_R
    {
        public _EAYT_R EAYT_R = new _EAYT_R();
    }
    public class GMES_ELNT
    {
        public _ELNT ELNT = new _ELNT();
    }
    public class GMES_ELNT_R
    {
        public _ELNT_R ELNT_R = new _ELNT_R();
    }
    public class GMES_EERS
    {
        public _EERS EERS = new _EERS();
    }
    public class GMES_EERS_R
    {
        public _EERS_R EERS_R = new _EERS_R();
    }
    public class GMES_EPDS
    {
        public _EPDS EPDS = new _EPDS();
    }
    public class GMES_EPDS_R
    {
        public _EPDS_R EPDS_R = new _EPDS_R();
    }
    public class GMES_EESR
    {
        public _EESR EESR = new _EESR();
    }
    public class GMES_EESR_R
    {
        public _EESR_R EESR_R = new _EESR_R();
    }
    public class GMES_EAPD
    {
        public _EAPD EAPD = new _EAPD();
    }
    public class GMES_EAPD_R
    {
        public _EAPD_R EAPD_R = new _EAPD_R();
    }
    public class GMES_EIDS
    {
        public _EIDS EIDS = new _EIDS();
    }
    public class GMES_EIDS_R
    {
        public _EIDS_R EIDS_R = new _EIDS_R();
    }
    public class GMES_EDTS
    {
        public _EDTS EDTS = new _EDTS();
    }
    public class GMES_EDTS_R
    {
        public _EDTS_R EDTS_R = new _EDTS_R();
    }
    public class GMES_EEMR
    {
        public _EEMR EEMR = new _EEMR();
    }
    public class GMES_EEMR_R
    {
        public _EEMR_R EEMR_R = new _EEMR_R();
    }
    #endregion

    #region GMES_DATA_ITEM
    public class ITEM
    {
        public string NAME;
        public string VALUE;
        public string JUDGE;
        public string MIN;
        public string MAX;
        public string UNITTIME;
        public ITEM(string Name, string Value, string Judge, string Min, string Max, string Unittime)
        {
            NAME = Name;
            VALUE = Value;
            JUDGE = Judge;
            MIN = Min;
            MAX = Max;
            UNITTIME = Unittime;
        }

    }
    public class DATA_ITEM
    {
        public string NAME = "";
        public string VALUE = "";
        public string JUDGE = "";
        public string MIN = "";
        public string MAX = "";
        public string UNITTIME = "";
    }
    public class _MODEL
    {
        public string ID = "";
        public string NAME = "";
        public string SUFFIX = "";
    }
    #endregion

    #region GMES_OBJECT
    public class _EAYT
    {
    }
    public class _EAYT_R
    {
        public string EQPID
        {
            get { return GMES_Data.EQPID; }
            set { GMES_Data.EQPID = value; }
        }
        public string EQPNAME
        {
            get { return GMES_Data.EQPNAME; }
            set { GMES_Data.EQPNAME = value; }
        }
        public string PROCID
        {
            get { return GMES_Data.PROCID; }
            set { GMES_Data.PROCID = value; }
        }
        public string ORGID
        {
            get { return GMES_Data.ORGID; }
            set { GMES_Data.ORGID = value; }
        }
        public string LINEID
        {
            get { return GMES_Data.LINEID; }
            set { GMES_Data.LINEID = value; }
        }
    }
    public class _ELNT
    {
    }
    public class _ELNT_R
    {
        public string EQPID
        {
            get { return GMES_Data.EQPID; }
            set { GMES_Data.EQPID = value; }
        }
    }
    public class _EERS
    {
        public string EQPID
        {
            get { return GMES_Data.EQPID; }
            set { GMES_Data.EQPID = value; }
        }
        public string CEID
        {
            get { return GMES_Data.CEID; }
            set { GMES_Data.CEID = value; }
        }
        public string RPTID
        {
            get { return GMES_Data.RPTID; }
            set { GMES_Data.RPTID = value; }
        }
        public List<ITEM> SUBITEM_LIST
        {
            get { return GMES_Data.SUBITEM_LIST; }
            set { GMES_Data.SUBITEM_LIST = value; }
        }
    }
    public class _EERS_R
    {
        public string EQPID
        {
            get { return GMES_Data.EQPID; }
            set { GMES_Data.EQPID = value; }
        }
        public string CEID
        {
            get { return GMES_Data.CEID; }
            set { GMES_Data.CEID = value; }
        }
        public string RPTID
        {
            get { return GMES_Data.RPTID; }
            set { GMES_Data.RPTID = value; }
        }
        public string ACK
        {
            get { return GMES_Data.ACK; }
            set { GMES_Data.ACK = value; }
        }
        public string REASON
        {
            get { return GMES_Data.REASON; }
            set { GMES_Data.REASON = value; }
        }

    }
    public class _EPDS
    {
        public string TID
        {
            get { return GMES_Data.TID; }
            set { GMES_Data.TID = value; }
        }
        public string WOID
        {
            get { return GMES_Data.WOID; }
            set { GMES_Data.WOID = value; }
        }
        public string ORGID
        {
            get { return GMES_Data.ORGID; }
            set { GMES_Data.ORGID = value; }
        }
        public string LINEID
        {
            get { return GMES_Data.LINEID; }
            set { GMES_Data.LINEID = value; }
        }
        public string EQPID
        {
            get { return GMES_Data.EQPID; }
            set { GMES_Data.EQPID = value; }
        }
        public string PROCID
        {
            get { return GMES_Data.PROCID; }
            set { GMES_Data.PROCID = value; }
        }
        public string SETID
        {
            get { return GMES_Data.SETID; }
            set { GMES_Data.SETID = value; }
        }
        public _MODEL MODEL
        {
            get { return GMES_Data.MODEL; }
            set { GMES_Data.MODEL = value; }
        }
        public List<ITEM> SUBITEM_LIST
        {
            get { return GMES_Data.SUBITEM_LIST; }
            set { GMES_Data.SUBITEM_LIST = value; }
        }
    }
    public class _EPDS_R
    {
        public string TID
        {
            get { return GMES_Data.TID; }
            set { GMES_Data.TID = value; }
        }
        public string WOID
        {
            get { return GMES_Data.WOID; }
            set { GMES_Data.WOID = value; }
        }
        public string ORGID
        {
            get { return GMES_Data.ORGID; }
            set { GMES_Data.ORGID = value; }
        }
        public string LINEID
        {
            get { return GMES_Data.LINEID; }
            set { GMES_Data.LINEID = value; }
        }
        public string EQPID
        {
            get { return GMES_Data.EQPID; }
            set { GMES_Data.EQPID = value; }
        }
        public string PROCID
        {
            get { return GMES_Data.PROCID; }
            set { GMES_Data.PROCID = value; }
        }
        public string SETID
        {
            get { return GMES_Data.SETID; }
            set { GMES_Data.SETID = value; }
        }
        public _MODEL MODEL
        {
            get { return GMES_Data.MODEL; }
            set { GMES_Data.MODEL = value; }
        }
        public List<ITEM> SUBITEM_LIST
        {
            get { return GMES_Data.SUBITEM_LIST; }
            set { GMES_Data.SUBITEM_LIST = value; }
        }
        public string ACK
        {
            get { return GMES_Data.ACK; }
            set { GMES_Data.ACK = value; }
        }
        public string REASON
        {
            get { return GMES_Data.REASON; }
            set { GMES_Data.REASON = value; }
        }
    }
    public class _EESR
    {
        public string EQPID
        {
            get { return GMES_Data.EQPID; }
            set { GMES_Data.EQPID = value; }
        }
        public string EQPSTATE
        {
            get { return GMES_Data.EQPSTATE; }
            set { GMES_Data.EQPSTATE = value; }
        }

    }
    public class _EESR_R
    {
        public string EQPID
        {
            get { return GMES_Data.EQPID; }
            set { GMES_Data.EQPID = value; }
        }
        public string ACK
        {
            get { return GMES_Data.ACK; }
            set { GMES_Data.ACK = value; }
        }
    }
    public class _EAPD
    {
        public string ORGID
        {
            get { return GMES_Data.ORGID; }
            set { GMES_Data.ORGID = value; }
        }
        public string LINEID
        {
            get { return GMES_Data.LINEID; }
            set { GMES_Data.LINEID = value; }
        }
        public string EQPID
        {
            get { return GMES_Data.EQPID; }
            set { GMES_Data.EQPID = value; }
        }
        public string PROCID
        {
            get { return GMES_Data.PROCID; }
            set { GMES_Data.PROCID = value; }
        }
        public string SETID
        {
            get { return GMES_Data.SETID; }
            set { GMES_Data.SETID = value; }
        }
        public List<ITEM> SUBITEM_LIST
        {
            get { return GMES_Data.SUBITEM_LIST; }
            set { GMES_Data.SUBITEM_LIST = value; }
        }
    }
    public class _EAPD_R
    {
        public string ORGID
        {
            get { return GMES_Data.ORGID; }
            set { GMES_Data.ORGID = value; }
        }
        public string LINEID
        {
            get { return GMES_Data.LINEID; }
            set { GMES_Data.LINEID = value; }
        }
        public string EQPID
        {
            get { return GMES_Data.EQPID; }
            set { GMES_Data.EQPID = value; }
        }
        public string PROCID
        {
            get { return GMES_Data.PROCID; }
            set { GMES_Data.PROCID = value; }
        }
        public string SETID
        {
            get { return GMES_Data.SETID; }
            set { GMES_Data.SETID = value; }
        }
        public string ACK
        {
            get { return GMES_Data.ACK; }
            set { GMES_Data.ACK = value; }
        }
        public string REASON
        {
            get { return GMES_Data.REASON; }
            set { GMES_Data.REASON = value; }
        }
    }
    public class _EIDS
    {
        public string TID
        {
            get { return GMES_Data.TID; }
            set { GMES_Data.TID = value; }
        }
        public string WOID
        {
            get { return GMES_Data.WOID; }
            set { GMES_Data.WOID = value; }
        }
        public string ORGID
        {
            get { return GMES_Data.ORGID; }
            set { GMES_Data.ORGID = value; }
        }
        public string LINEID
        {
            get { return GMES_Data.LINEID; }
            set { GMES_Data.LINEID = value; }
        }
        public string EQPID
        {
            get { return GMES_Data.EQPID; }
            set { GMES_Data.EQPID = value; }
        }
        public string PROCID
        {
            get { return GMES_Data.PROCID; }
            set { GMES_Data.PROCID = value; }
        }
        public string SETID
        {
            get { return GMES_Data.SETID; }
            set { GMES_Data.SETID = value; }
        }
        public _MODEL MODEL
        {
            get { return GMES_Data.MODEL; }
            set { GMES_Data.MODEL = value; }
        }
        public string RESULT
        {
            get { return GMES_Data.RESULT; }
            set { GMES_Data.RESULT = value; }
        }
        public List<ITEM> SUBITEM_LIST
        {
            get { return GMES_Data.SUBITEM_LIST; }
            set { GMES_Data.SUBITEM_LIST = value; }
        }
    }
    public class _EIDS_R
    {
        public string TID
        {
            get { return GMES_Data.TID; }
            set { GMES_Data.TID = value; }
        }
        public string WOID
        {
            get { return GMES_Data.WOID; }
            set { GMES_Data.WOID = value; }
        }
        public string ORGID
        {
            get { return GMES_Data.ORGID; }
            set { GMES_Data.ORGID = value; }
        }
        public string LINEID
        {
            get { return GMES_Data.LINEID; }
            set { GMES_Data.LINEID = value; }
        }
        public string EQPID
        {
            get { return GMES_Data.EQPID; }
            set { GMES_Data.EQPID = value; }
        }
        public string PROCID
        {
            get { return GMES_Data.PROCID; }
            set { GMES_Data.PROCID = value; }
        }
        public string SETID
        {
            get { return GMES_Data.SETID; }
            set { GMES_Data.SETID = value; }
        }
        public _MODEL MODEL
        {
            get { return GMES_Data.MODEL; }
            set { GMES_Data.MODEL = value; }
        }
        public string ACK
        {
            get { return GMES_Data.ACK; }
            set { GMES_Data.ACK = value; }
        }
        public string REASON
        {
            get { return GMES_Data.REASON; }
            set { GMES_Data.REASON = value; }
        }
    }
    public class _EDTS
    {
        public string EQPID
        {
            get { return GMES_Data.EQPID; }
            set { GMES_Data.EQPID = value; }
        }
        public string TIME
        {
            get { return GMES_Data.TIME; }
            set { GMES_Data.TIME = value; }
        }
    }
    public class _EDTS_R
    {
        public string EQPID
        {
            get { return GMES_Data.EQPID; }
            set { GMES_Data.EQPID = value; }
        }
        public string ACK
        {
            get { return GMES_Data.ACK; }
            set { GMES_Data.ACK = value; }
        }
    }
    public class _EEMR
    {
        public string EQPID
        {
            get { return GMES_Data.EQPID; }
            set { GMES_Data.EQPID = value; }
        }
        public string CONTROLSTATE
        {
            get { return GMES_Data.CONTROLSTATE; }
            set { GMES_Data.CONTROLSTATE = value; }
        }
    }
    public class _EEMR_R
    {
        public string EQPID
        {
            get { return GMES_Data.EQPID; }
            set { GMES_Data.EQPID = value; }
        }
        public string ACK
        {
            get { return GMES_Data.ACK; }
            set { GMES_Data.ACK = value; }
        }
    }
    #endregion
}

