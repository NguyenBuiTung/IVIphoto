using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TakeimgIVI
{
    public class GMES_Data
    {

        public static GMES_EAYT EAYT = new GMES_EAYT();
        public static GMES_EAYT_R EAYT_R = new GMES_EAYT_R();
        public static GMES_ELNT ELNT = new GMES_ELNT();
        public static GMES_ELNT_R ELNT_R = new GMES_ELNT_R();
        public static GMES_EERS EERS = new GMES_EERS();
        public static GMES_EERS_R EERS_R = new GMES_EERS_R();
        public static GMES_EPDS EPDS = new GMES_EPDS();
        public static GMES_EPDS_R EPDS_R = new GMES_EPDS_R();
        public static GMES_EESR EESR = new GMES_EESR();
        public static GMES_EESR_R EESR_R = new GMES_EESR_R();
        public static GMES_EAPD EAPD = new GMES_EAPD();
        public static GMES_EAPD_R EAPD_R = new GMES_EAPD_R();
        public static GMES_EIDS EIDS = new GMES_EIDS();
        public static GMES_EIDS_R EIDS_R = new GMES_EIDS_R();
        public static GMES_EDTS EDTS = new GMES_EDTS();
        public static GMES_EDTS_R EDTS_R = new GMES_EDTS_R();
        public static GMES_EEMR EEMR = new GMES_EEMR();
        public static GMES_EEMR_R EEMR_R = new GMES_EEMR_R();


        public static string DataSend_2 { set; get; }
        public static string EQPID = "";
        public static string CEID = "";// "20100";
        public static string RPTID = "";//"20100";
        public static string EQPNAME = "";
        public static string EQPSTATE = "";
        public static string PROCID = "";
        public static string ORGID = "";
        public static string LINEID = "";
        public static string TID = "";
        public static string WOID = "";
        public static string SETID = "";
        public static string ID = "";
        public static string SUFFIX = "";
        public static string ACK = "";
        public static string REASON = "";
        public static string RESULT = "";
        public static string TIME = "";
        public static string CONTROLSTATE = "";
        public static _MODEL MODEL = new _MODEL();
        public static List<ITEM> SUBITEM_LIST = new List<ITEM>();
        public static string Barcode { set; get; }
        public static string Reason { set; get; }
        public static string CurrentTime { set; get; }
        public static bool Received_Data { set; get; }
        public static bool Ready_To_Online_Check { set; get; }

        public static Dictionary<string, string> Product_Info = new Dictionary<string, string>();
        public static void Get_Product_Infomation(XmlNode xmlcollect)
        {
            Product_Info.Clear();
            if (Product_Info.Count == 0)
            {
                //foreach (var item in SUBITEM_LIST)
                //{
                //    Product_Info[item.NAME] = item.VALUE;
                //}
                for (int i = 0; i < xmlcollect.ChildNodes.Count; i+=2)
                {
                    if(!Product_Info.ContainsKey(xmlcollect.ChildNodes[i].InnerText))
                    {
                        Product_Info.Add(xmlcollect.ChildNodes[i].InnerText, xmlcollect.ChildNodes[i + 1].InnerText);
                        Product_Info[xmlcollect.ChildNodes[i].InnerText] = xmlcollect.ChildNodes[i + 1].InnerText;//.Replace("-","");
                    }
                }
            }
            Ready_To_Online_Check = true;
        }

        public static string Get_Value_By_Key(string key)
        {
            if(Product_Info.ContainsKey(key))
            {
                return Product_Info[key];
            }
            return "NULL";
        }

        public static void Get_Date_Time_Now()
        {
            CurrentTime = DateTime.Now.ToString("[yyyy:MM:dd HH:mm:ss:fff]");
        }
        public static void Get()
        {
            
        }
    }
}
