using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEaseWordsReview
{
    // Token: 0x02000168 RID: 360
    internal class StudioEnvSdkHelper
    {
        // Token: 0x06001250 RID: 4688 RVA: 0x0003FDC0 File Offset: 0x0003FDC0
        public static bool InitialSDK(int openLog = 0)
        {
            bool flag2;
            try
            {
                object obj = StudioEnvSdkHelper.initLock;
                lock (obj)
                {
                    if (StudioEnvSdkHelper.IsInit)
                    {
                        flag2 = true;
                    }
                    else
                    {
                        EnvSDKHelper.SetSwitch("openLog", openLog);
                        string text;
                        int num = EnvSDKHelper.InitSDK("x19", "c42bf7f39d479999", "optsdk.gameyw.netease.com", out text);
                        if (num != 200)
                        {
                            //Logger.Default.Error(string.Format("StudioEnvSdkHelper init error! code: {0} result: {1}", num, text), new object[0]);
                            Console.WriteLine("StudioEnvSdkHelper init error!");
                            flag2 = false;
                        }
                        else
                        {
                            StudioEnvSdkHelper.IsInit = true;
                            flag2 = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("StudioEnvSdkHelper init error");
                flag2 = false;
            }
            return flag2;
        }

        // Token: 0x06001251 RID: 4689 RVA: 0x0003FE88 File Offset: 0x0003FE88
        public static string ReviewName(string word)
        {
            string text2;
            try
            {
                string text = "";
                int num = EnvSDKHelper.ReviewNickname(word, out text);
                if (num != 100)
                {
                    switch (num)
                    {
                        case 200:
                            goto IL_3E;
                        case 201:
                        case 202:
                            return "****";
                        case 206:
                            return text;
                    }
                    Console.WriteLine(string.Format("ReviewName Error: word:{0}; code:{1}", word, num));
                    return word;
                }
            IL_3E:
                text2 = word;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReviewName Error: word:" + word);
                text2 = word;
            }
            return text2;
        }

        // Token: 0x06001252 RID: 4690 RVA: 0x0003FF34 File Offset: 0x0003FF34
        public static bool IsReviewName(string name)
        {
            bool flag;
            try
            {
                string text = "";
                int num = EnvSDKHelper.ReviewNickname(name, out text);
                if (num == 100 || num == 200)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("IsReviewName Error: word:" + name);
                flag = true;
            }
            return flag;
        }

        // Token: 0x06001253 RID: 4691 RVA: 0x0003FF94 File Offset: 0x0003FF94
        public static string ReviewWord(string word)
        {
            string text2;
            try
            {
                string text = "";
                int num = EnvSDKHelper.ReviewWords(word, "0", "item_comment", out text);
                if (num != 100)
                {
                    switch (num)
                    {
                        case 200:
                            goto IL_48;
                        case 201:
                        case 202:
                            return "".PadRight(word.Length, '*');
                        case 206:
                            return text;
                    }
                    Console.WriteLine();
                    return word;
                }
            IL_48:
                text2 = word;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReviewWord Error: word:" + word);
                text2 = word;
            }
            return text2;
        }

        // Token: 0x06001254 RID: 4692 RVA: 0x00040058 File Offset: 0x00040058
        public static bool IsReviewWord(string word, out string result)
        {
            bool flag;
            try
            {
                int num = EnvSDKHelper.ReviewWords(word, "0", "item_comment", out result);
                if (num == 100 || num == 200)
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReviewWord Error: word:" + word);
                result = "";
                flag = true;
            }
            return flag;
        }

        // Token: 0x06001255 RID: 4693 RVA: 0x000400C0 File Offset: 0x000400C0
        public void Clear()
        {
            EnvSDKHelper.ClearSDK();
        }

        // Token: 0x040007D3 RID: 2003
        public static bool IsInit = false;

        // Token: 0x040007D4 RID: 2004
        private static StudioEnvSdkHelper.EnvSdkScopedGuard guard = new StudioEnvSdkHelper.EnvSdkScopedGuard();

        // Token: 0x040007D5 RID: 2005
        private static object initLock = new object();

        // Token: 0x0200042E RID: 1070
        private class EnvSdkScopedGuard : IDisposable
        {
            // Token: 0x060027B4 RID: 10164 RVA: 0x00085AB7 File Offset: 0x00085AB7
            public void Dispose()
            {
                EnvSDKHelper.ClearSDK();
                StudioEnvSdkHelper.IsInit = false;
            }
        }
    }
}
