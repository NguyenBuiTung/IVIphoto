using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace TakeimgIVI.Function
{
    public class Modelcf
    {
        private const int totalitem = 21;
        public Modelcf(string[] modelcf = null) 
        {
            Array = modelcf;
        }
        public string[] Array
        {
            get
            {
                return new string[totalitem] { Index.ToString(), Name, patchImage, Exposuer1.ToString(), Exposuer2.ToString(), Gain1.ToString("0.00"), Gain2.ToString("0.00"), Focus1.ToString(), Focus2.ToString(), Iris1.ToString(), Iris2.ToString(), widthbegin1.ToString("0.00"), widthend1.ToString("0.00"), heightbegin1.ToString("0.00"), heightend1.ToString("0.00"), widthbegin2.ToString("0.00"), widthend2.ToString("0.00"), heightbegin2.ToString("0.00"), heightend2.ToString("0.00"), sensorindex.ToString(),lengthbarcode.ToString() };
            }
            set
            {
                if (value != null && value.Length == totalitem)
                {
                    int index = -1;
                    if (int.TryParse(value[0], out index))
                    { Index = index; }

                    Name = value[1];

                    if (File.Exists(value[2]))
                    {
                        Image = Image.FromFile(value[2]);
                        patchImage = value[2];
                    }
                    int Expo1;
                    if (int.TryParse(value[3], out Expo1))
                        Exposuer1 = Expo1;

                    int Expo2;
                    if (int.TryParse(value[4], out Expo2))
                        Exposuer2 = Expo2;

                    float gain1;
                    if (float.TryParse(value[5], out gain1))
                        Gain1 = gain1;
                    
                    float gain2;
                    if (float.TryParse(value[6], out gain2))
                        Gain2 = gain2;

                    ushort focus1;
                    if (ushort.TryParse(value[7], out focus1))
                        Focus1 = focus1;

                    ushort focus2;
                    if (ushort.TryParse(value[8], out focus2))
                        Focus2 = focus2;

                    ushort iris1;
                    if (ushort.TryParse(value[9], out iris1))
                        Iris1 = iris1;

                    ushort iris2;
                    if (ushort.TryParse(value[10], out iris2))
                        Iris2 = iris2;

                    float widthbg1;
                    if (float.TryParse(value[11], out widthbg1) && widthbg1 >= 0 && widthbg1 <= 1)
                        widthbegin1 = widthbg1;

                    float widthe1;
                    if (float.TryParse(value[12], out widthe1) && widthe1 >= 0 && widthe1 <= 1 && widthe1 >= widthbegin1)
                        widthend1 = widthe1;

                    float heightbg1;
                    if (float.TryParse(value[13], out heightbg1) && heightbg1 >= 0 && heightbg1 <= 1)
                        heightbegin1 = heightbg1;

                    float heighte1;
                    if (float.TryParse(value[14], out heighte1) && heighte1 >= 0 && heighte1 <= 1 && heighte1 >= heightbegin1)
                        heightend1 = heighte1;

                    float widthbg2;
                    if (float.TryParse(value[15], out widthbg2) && widthbg2 >= 0 && widthbg2 <= 1)
                        widthbegin2 = widthbg2;

                    float widthe2;
                    if (float.TryParse(value[16], out widthe2) && widthe2 >= 0 && widthe2 <= 1 && widthe2 >= widthbegin2)
                        widthend2 = widthe2;

                    float heightbg2;
                    if (float.TryParse(value[17], out heightbg2) && heightbg2 >= 0 && heightbg2 <= 1)
                        heightbegin2 = heightbg2;

                    float heighte2;
                    if (float.TryParse(value[18], out heighte2) && heighte2 >= 0 && heighte2 <= 1 && heighte2 >= heightbegin2)
                        heightend2 = heighte2;

                    int ssindex;
                    if (int.TryParse(value[19], out ssindex) && ssindex >= 0 && ssindex <= 1)
                        sensorindex = ssindex;

                    int lengthbr;
                    if (int.TryParse(value[20], out lengthbr) && lengthbr >= 10 && lengthbr <= 40)
                        lengthbarcode = lengthbr;

                }
            }
        }
        public int Index { get; set; } = -1;
        public string Name { get; set; } = "NewModel";
        public Image Image = Image.FromFile(Application.StartupPath + "/Communication/newmodel.jpg");
        public string patchImage { get; set; } = Application.StartupPath + "/Communication/newmodel.jpg";
        public int Exposuer1 { get; set; } = 10000;
        public int Exposuer2 { get; set; } = 10000;
        public float Gain1 { get; set; } = 0.6f;
        public float Gain2 { get; set; } = 0.6f;
        public ushort Focus1 { get; set; } = 3600;
        public ushort Focus2 { get; set; } = 3600;
        public ushort Iris1 { get; set; } = 0;
        public ushort Iris2 { get; set; } = 0;
        public float widthbegin1 { get; set; } = 0.0f;
        public float widthend1 { get; set; } = 1.0f;
        public float heightbegin1 { get; set; } = 0.0f;
        public float heightend1 { get; set; } = 1.0f;
        public float widthbegin2 { get; set; } = 0.0f;
        public float widthend2 { get; set; } = 1.0f;
        public float heightbegin2 { get; set; } = 0.0f;
        public float heightend2 { get; set; } = 1.0f;
        public int sensorindex { get; set; } = 0;
        public int lengthbarcode { get; set; } = 15;
    }

    public class Model
    {
        public static string Pathmodel = Application.StartupPath + "/Communication/ListModel.csv";
        public static List<Modelcf> LsModelcf = new List<Modelcf>();
        public static Modelcf Modelnow;
        public static float[] Modelcfnowarry { get => new float[8] { Modelnow.widthbegin1, Modelnow.widthend1, Modelnow.heightbegin1, Modelnow.heightend1, Modelnow.widthbegin2, Modelnow.widthend2, Modelnow.heightbegin2, Modelnow.heightend2 }; }

        public static void Readmodelinto()
        {
            if (File.Exists(Pathmodel))
            {
                List<string> lsline = File.ReadAllLines(Pathmodel).ToList();
                if(lsline.Count > 1) { LsModelcf.Clear(); }
                for(int i = 1; i < lsline.Count; i++)
                {
                    string[] lsconfig = lsline[i].Split(',');
                    Modelcf item = new Modelcf(lsconfig);
                    LsModelcf.Add(item);
                }
            }
        }

        public static void Savemodelinto(List<Modelcf> lsmodel)
        {
            var lsmodelcf = (from modelcf in lsmodel
                               select modelcf.Array).ToList();
            List<string> AllLine = new List<string>();

            AllLine.Add("ID,Name,PathImage,Exposuer1,Exposuer2,Gain1,Gain2,Focus1,Focus2,Iris1,Iris2,widthbegin1,widthend1,heightbegin1,heightend1,widthbegin2,widthend2,heightbegin2,heightend2,sensorindex,lengthbarcode");

            for (int i = 0; i < lsmodelcf.Count; i++)
            {
                string Line = "";
                for (int j = 0; j < lsmodelcf[i].Length; j++)
                {
                    lsmodelcf[i][j] = lsmodelcf[i][j].Replace(',', '.');
                    Line += j == 0 ? lsmodelcf[i][j] : "," + lsmodelcf[i][j];
                }
                AllLine.Add(Line);  
            }
            
            File.WriteAllLines(Pathmodel, AllLine.ToArray());
        }

        public static bool UpdateModelnow()
        {
            try
            {
                DelegateToUI.Updatemodelname("");
                if (Model.Modelnow != null)
                {
                    //Set Lens Config
                    if (FormMain.lens1.UpdateLens(Modelnow.Focus1, Modelnow.Iris1))
                        DelegateToUI.PushToListBox($"LENS1[]: Set Focus:{Modelnow.Focus1}, Iris:{Modelnow.Iris1} successfully");
                    if (FormMain.lens2.UpdateLens(Modelnow.Focus2, Modelnow.Iris2))
                        DelegateToUI.PushToListBox($"LENS2[]: Set Focus:{Modelnow.Focus2}, Iris:{Modelnow.Iris2} successfully");
                    //Set Exposure CAMERA
                    if (Status.Camera1)
                    {
                        if (FormMain.Camera1.SetCameraParameter(Modelnow.Exposuer1, Modelnow.Gain1))
                            DelegateToUI.PushToListBox($"CAMERA1[]: Set Exposure:{Modelnow.Exposuer1}, Gain:{Modelnow.Gain1} successfully");
                    }
                    if (Status.Camera2)
                    {
                        if (FormMain.Camera2.SetCameraParameter(Modelnow.Exposuer2, Modelnow.Gain2))
                            DelegateToUI.PushToListBox($"CAMERA2[]: Set Exposure:{Modelnow.Exposuer2}, Gain:{Modelnow.Gain2} successfully");
                    }

                    DelegateToUI.Updatemodelname(Modelnow.Name);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR Choose Model {Modelnow.Name}" + ex.Message);
                return false;
            }
        }

        public static bool UpdateModelcf(Modelcf modelcf)
        {
            try
            {
                if (modelcf != null)
                {
                    //Set Lens Config
                    if (FormMain.lens1.UpdateLens(modelcf.Focus1, modelcf.Iris1))
                        DelegateToUI.PushToListBox("LENS1[]: Set Focus successfully");
                    if (FormMain.lens2.UpdateLens(modelcf.Focus2, modelcf.Iris2))
                        DelegateToUI.PushToListBox("LENS2[]: Set Focus successfully");
                    //Set Exposure CAMERA
                    if (Status.Camera1)
                    {
                        if (FormMain.Camera1.SetCameraParameter(modelcf.Exposuer1, modelcf.Gain1))
                            DelegateToUI.PushToListBox("CAMERA1[]: Set Exposure,Gain successfully");
                    }
                    if (Status.Camera2)
                    {
                        if (FormMain.Camera2.SetCameraParameter(modelcf.Exposuer2, modelcf.Gain2))
                            DelegateToUI.PushToListBox("CAMERA2[]: Set Exposure,Gain successfully");
                    }

                    DelegateToUI.Updatemodelname(modelcf.Name);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR Model null {modelcf.Name}" + ex.Message);
                return false;
            }
        }

    }
}
