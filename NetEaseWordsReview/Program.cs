using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NetEaseWordsReview
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("网易屏蔽词检测器");
            Console.WriteLine("[EnvSDK]Init");
            StudioEnvSdkHelper.InitialSDK(0);
            Console.WriteLine("[EnvSDK]Success");

            Console.WriteLine("1.玩家名");
            Console.WriteLine("2.词汇或句子");
            Console.Write("你要检查的类型: ");
            var checkType = Console.ReadLine();
            switch (checkType)
            {
                case "1":
                    while (true)
                    {
                        Console.Write("请输入你要检测的玩家名: ");
                        var input = Console.ReadLine();

                        //检查
                        bool flag = StudioEnvSdkHelper.IsReviewName(input);
                        string WPFOutput = StudioEnvSdkHelper.ReviewName(input);

                        Console.WriteLine($"EnvSDKFlag: {flag}");

                        var isBanned = "";
                        if (flag == false)
                        {
                            isBanned = "是";
                        }
                        else
                        {
                            isBanned = "否";
                        }

                        Console.WriteLine($"是否屏蔽: {isBanned}");
                        Console.WriteLine($"模拟输出: {WPFOutput}");
                    }
                    break;
                case "2":
                    while (true)
                    {
                        Console.Write("请输入你要检测的内容: ");
                        var input = Console.ReadLine();
                        string result = "";

                        //检查
                        bool flag = StudioEnvSdkHelper.IsReviewWord(input, out result);
                        string GameOutput = StudioEnvSdkHelper.ReviewWord(input);

                        Console.WriteLine($"EnvSDKFlag: {flag}");
                        Console.WriteLine($"EnvSDKMsg: {result}");

                        //解析
                        JObject sdkJsonObj = JObject.Parse(result);

                        var isBanned = "";
                        if (sdkJsonObj["message"].ToString() == "shield")
                        {
                            isBanned = "是";
                        }
                        else if (sdkJsonObj["message"].ToString() == "intercept")
                        {
                            isBanned = "拦截";
                        }
                        else
                        {
                            isBanned = "否";
                        }

                        Console.WriteLine($"Code: {sdkJsonObj["code"].ToString()}");
                        Console.WriteLine($"是否屏蔽: {isBanned}");
                        Console.WriteLine($"ID: {sdkJsonObj["regularId"].ToString()}");
                        Console.WriteLine($"游戏内输出: {GameOutput}");
                    }
                    break;
            }
        }
    }
}
