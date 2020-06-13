using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotNet.WinForm
{
    public class BaseColpEnumClass
    {
        //Color color = Color.Blue;
        //string colorString = " Blue";
        //int colorValue = 0x0000FF;
        //// 枚举转字符串
        //string enumStringOne = color.ToString(); //效率低，不推荐
        //string enumStringTwo = Enum.GetName(typeof(Color), color);//推荐
        //// 枚举转值
        //int enumValueOne = color.GetHashCode();
        //int enumValueTwo = (int)color;
        //int enumValueThree = Convert.ToInt32(color);
        //// 字符串转枚举
        //Color enumOne = (Color)Enum.Parse(typeof(Color), colorString);
        //// 字符串转值
        //int enumValueFour = (int)Enum.Parse(typeof(Color), colorString);
        //// 值转枚举
        //Color enumTwo = (Color)colorValue;
        //Color enumThree = (Color)Enum.ToObject(typeof(Color), colorValue);
        //// 值转字符串
        //string enumStringThree = Enum.GetName(typeof(Color), colorValue);
        public enum OverallCervixVisibleEnum : int
        {
            /// <summary>
            /// 未知
            /// </summary>
            NULL = 0,

            /// <summary>
            /// 完全可见
            /// </summary>
            VISIBLE = 1,

            /// <summary>
            /// 不完全可见
            /// </summary>
            NOTVISIBLE = 2,

            /// <summary>
            /// 无法判断
            /// </summary>
            ////UNJUDGED = -1,
        }
        public enum OverallSCJVisibleEnum : int
        {
            /// <summary>
            /// 未知
            /// </summary>
            NULL = 0,

            /// <summary>
            /// 完全可见
            /// </summary>
            VISIBLE = 1,

            /// <summary>
            /// 不完全可见
            /// </summary>
            NOTVISIBLE = 2,

            /// <summary>
            /// 无法判断
            /// </summary>
            UNJUDGED = -1,
        }
        //醋白上皮是否出现
        public enum AfterAcidWhiteVisibleEnum : int
        {
            /// <summary>
            /// 未知
            /// </summary>
            NULL = 0,

            /// <summary>
            /// 完全可见
            /// </summary>
            VISIBLE = 1,

            /// <summary>
            /// 不完全可见
            /// </summary>
            NOTVISIBLE = 2, 
        }
        //AfterAcidInfectionVisible 是否出现病灶
        public enum AfterAcidInfectionVisibleEnum : int
        {
            /// <summary>
            /// 未知
            /// </summary>
            NULL = 0,

            /// <summary>
            /// 完全可见
            /// </summary>
            VISIBLE = 1,

            /// <summary>
            /// 不完全可见
            /// </summary>
            NOTVISIBLE = 2,
        }

        //CheckIsFull 是否充分检查
        public enum CheckIsFullEnum : int
        {
            /// <summary>
            /// 未知
            /// </summary>
            NULL = 0,

            /// <summary>
            /// 完全可见
            /// </summary>
            VISIBLE = 1,

            /// <summary>
            /// 不完全可见
            /// </summary>
            NOTVISIBLE = 2,
        }
        public enum ReferralItemEnum : int
        {
            /// <summary>
            /// 未知
            /// </summary>
            NULL = 0,

            /// <summary>
            /// 正常
            /// </summary>
            NORMAL = 1,

            /// <summary>
            /// 低级病变(LSIL,对应病理CIN1)
            /// </summary>
            LSIL = 2,

            /// <summary>
            /// 高级病变(HSIL,对应病理CIN2/CIN3)
            /// </summary>
            HSIL = 3,

            /// <summary>
            /// 可疑浸润癌
            /// </summary>
            InfiltratingCarcinoma = 4,

            /// <summary>
            /// 可疑宫颈腺上皮病变
            /// </summary>
            LandularEpithelium = 5,

            /// <summary>
            /// 阴道镜检查不充分（须补充其它检查）
            /// </summary>
            CheckUnpermissible = 6,

            /// <summary>
            /// 其它
            /// </summary>
            ReferralOther = 6,

        }

        public enum SourceCategoryEnum
        {
            /// <summary>
            /// 未知，Unknown
            /// </summary>
            [Description("未知")]
            U = 0,
            /// <summary>
            /// 导入，Import
            /// </summary>
            [Description("导入")]
            I = 1,
            /// <summary>
            /// 镜头手动，Manual
            /// </summary>
            [Description("镜头手动")]
            M = 2,
            /// <summary>
            /// 脚踏，Foot
            /// </summary>
            [Description("脚踏")]
            F = 3,
            /// <summary>
            /// 软件按钮，Button
            /// </summary>
            [Description("软件按钮")]
            B = 4,
            /// <summary>
            /// 软件自动，Auto
            /// </summary>
            [Description("软件自动")]
            A = 5,
            /// <summary>
            /// 软件计时，Timer
            /// </summary>
            [Description("软件计时")]
            T = 6,
            /// <summary>
            /// 视频预采，Video
            /// </summary>
            [Description("视频预采")]
            V = 7,
            /// <summary>
            /// 冻结预采，freeZe
            /// </summary>
            [Description("冻结预采")]
            Z = 8,
            /// <summary>
            /// 语音自动，Speech
            /// </summary>
            [Description("语音自动")]
            S = 9,
            /// <summary>
            /// 体感，Kinect
            /// </summary>
            [Description("体感")]
            K = 10,
            /// <summary>
            /// 实时采图，Realtime
            /// </summary>
            [Description("实时采图")]
            R = 11,
            /// <summary>
            /// 其他，Other
            /// </summary>
            [Description("其他")]
            O = 0xFF,
        }


        public enum StageCategoryEnum
        {
            /// <summary>
            /// 未知，Unknown
            /// </summary>
            [Description("未知")]
            U = 0,
            /// <summary>
            /// 原始图，Original
            /// </summary>
            [Description("原始图")]
            O = 1,
            /// <summary>
            /// 醋溶液，Vinegar
            /// </summary>
            [Description("醋溶液")]
            V = 2,

            /// <summary>
            /// 醋溶液，Vinegar
            /// </summary>
            [Description("醋染计时结束")]
            W = 3,
            /// <summary>
            /// 碘溶液，Iodine
            /// </summary>
            [Description("碘溶液")]
            I = 4,

            /// <summary>
            /// 碘溶液，Iodine
            /// </summary>
            [Description("碘染计时结束")]
            J = 5,
            /// <summary>
            /// 报告，Report
            /// </summary>
            [Description("报告")]
            R = 6,
        }

        public enum CommandMenuEnum
        {
            /// <summary>
            /// 未知，LoadVideo
            /// </summary>
            [Description("实时视频")]
            LoadVideo = 0,
            /// <summary>
            /// 实时视频，ReturnDisplayVideo
            /// </summary>
            [Description("原始图")]
            ReturnDisplayVideo = 1,
            /// <summary>
            /// 醋溶液，StopDisplayVideo
            /// </summary>
            [Description("醋溶液")]
            StopDisplayVideo = 2,
            /// <summary>
            /// 停止实时视频，DisplayPicture
            /// </summary>
            [Description("停止实时视频")]
            DisplayPicture = 3,
            /// <summary>
            /// 结束显示图片，StopDisplayPicture
            /// </summary>
            [Description("结束显示图片")]
            StopDisplayPicture = 4,
        }

        public static int GetStageSourceTimeByName(String inputFileName, ref StageCategoryEnum StageCategory, ref SourceCategoryEnum SourceCategory)
        {
            int stageTime = -1;
            //20190326101111111-UF000.jpg
            //取-和.之间的字符串、
            try
            {
                String left = "-";
                String right = @"\" + @".";
                bool isBorder = true;
                ////List<string> list = new List<string>();
                String StageSourceTime = null;
                Regex regex = new Regex("(?<=" + left + ")([.\\S\\s]*)(?=" + right + ")");
                foreach (Match eachMatch in regex.Matches(inputFileName))
                {
                    string value = eachMatch.Value;
                    if (!isBorder)
                    {
                        value = value.Substring(left.Length, value.Length - left.Length);
                        value = value.Substring(0, value.Length - right.Length);
                    }
                    ////list.Add(value);
                    StageSourceTime = value;
                }
                if (!String.IsNullOrEmpty(StageSourceTime) && (StageSourceTime.Length >= 4 || StageSourceTime.Length <= 5))
                {
                    String StageCategoryString = StageSourceTime.Substring(0, 1);
                    String SourceCategoryString = StageSourceTime.Substring(1, 1);
                    String StageTimeString = StageSourceTime.Substring(2, StageSourceTime.Length - 2);
                    StageCategory = (StageCategoryEnum)Enum.Parse(typeof(StageCategoryEnum), StageCategoryString);
                    SourceCategory = (SourceCategoryEnum)Enum.Parse(typeof(SourceCategoryEnum), SourceCategoryString);
                    stageTime = int.Parse(StageTimeString);
                }
            }
            catch (Exception catchException)
            {
                ////if (Properties.Settings.Default.ShowcatchOutOfMemoryException) catchException.ProcessException();
            }
            finally
            {

            }
            return stageTime;
        }

        public string strMatch(string str,string pattern) 
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(str, pattern))
            {
                System.Text.RegularExpressions.Match m =
                    System.Text.RegularExpressions.Regex.Match(str, pattern);
                string v = "";
                //if (m.Success)
                //{
                //    v = str;

                //}
                while (m.Success)
                {
                    if (m.Value == ",")
                    {
                        v += m.Value;
                        continue;
                    }
                    v += m.Value;
                    m = m.NextMatch();
                    
                }
                return str;
            }
            else
            {
                return null;
            }

        }
    }
}
