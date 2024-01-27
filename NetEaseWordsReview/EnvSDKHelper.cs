using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NetEaseWordsReview
{
    // Token: 0x02000167 RID: 359
    internal static class EncodingExtend
    {
        // Token: 0x0600124F RID: 4687 RVA: 0x0003FD90 File Offset: 0x0003FD90
        public static byte[] GetBytesNullTerminated(this Encoding encoding, string text)
        {
            byte[] array = new byte[encoding.GetByteCount(text) + 1];
            encoding.GetBytes(text, 0, text.Length, array, 0);
            return array;
        }
    }
    internal class EnvSDKHelper
    {
        // Token: 0x06001245 RID: 4677
        [DllImport("EnvSDKLib.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "EnvSDK_clearSDK")]
        private static extern void ClearSDKFromDll();

        // Token: 0x06001246 RID: 4678
        [DllImport("EnvSDKLib.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "EnvSDK_setSwitch")]
        private static extern int SetSwitchFromDll(byte[] name, int val);

        // Token: 0x06001247 RID: 4679
        [DllImport("EnvSDKLib.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "EnvSDK_getSwitch")]
        private static extern int GetSwitchFromDll(byte[] name, out int val);

        // Token: 0x06001248 RID: 4680
        [DllImport("EnvSDKLib.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "EnvSDK_initSDK")]
        private static extern void InitSDKFromDll(byte[] gameId, byte[] secretKey, byte[] host, EnvSDKHelper.CallbackFromDll callback, IntPtr context);

        // Token: 0x06001249 RID: 4681
        [DllImport("EnvSDKLib.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "EnvSDK_initSDKAsync")]
        private static extern void InitSDKAsyncFromDll(byte[] gameId, byte[] secretKey, byte[] host, EnvSDKHelper.CallbackFromDll callback, IntPtr context);

        // Token: 0x0600124A RID: 4682
        [DllImport("EnvSDKLib.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "EnvSDK_reviewNickname")]
        private static extern void ReviewNicknameFromDll(byte[] nickname, EnvSDKHelper.CallbackFromDll callback, IntPtr context);

        // Token: 0x0600124B RID: 4683
        [DllImport("EnvSDKLib.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "EnvSDK_reviewNicknameAsync")]
        private static extern void ReviewNicknameAsyncFromDll(byte[] nickname, EnvSDKHelper.CallbackFromDll callback, IntPtr context);

        // Token: 0x0600124C RID: 4684
        [DllImport("EnvSDKLib.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "EnvSDK_reviewWords")]
        private static extern void ReviewWordsFromDll(byte[] content, byte[] level, byte[] channel, EnvSDKHelper.CallbackFromDll callback, IntPtr context);

        // Token: 0x0600124D RID: 4685
        [DllImport("EnvSDKLib.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "EnvSDK_reviewWordsAsync")]
        private static extern void ReviewWordsAsyncFromDll(byte[] content, byte[] level, byte[] channel, EnvSDKHelper.CallbackFromDll callback, IntPtr context);
        // Token: 0x02000428 RID: 1064
        // (Invoke) Token: 0x060027A5 RID: 10149
        public delegate void Callback(int code, string result);
        // Token: 0x02000429 RID: 1065
        private class CallbackWrapper
        {
            // Token: 0x060027A8 RID: 10152 RVA: 0x000859FB File Offset: 0x000859FB
            public CallbackWrapper(EnvSDKHelper.Callback callback)
            {
                this.callback_ = callback;
            }
            // Token: 0x060027A9 RID: 10153 RVA: 0x00085A0C File Offset: 0x00085A0C
            public static void Callback(int code, IntPtr result, IntPtr context)
            {
                GCHandle gchandle = GCHandle.FromIntPtr(context);
                EnvSDKHelper.CallbackWrapper callbackWrapper = (EnvSDKHelper.CallbackWrapper)gchandle.Target;
                string text = string.Empty;
                if (result != IntPtr.Zero)
                {
                    int num = 0;
                    while (Marshal.ReadByte(result, num) != 0)
                    {
                        num++;
                    }
                    byte[] array = new byte[num];
                    Marshal.Copy(result, array, 0, array.Length);
                    text = Encoding.UTF8.GetString(array);
                }
                callbackWrapper.callback_(code, text);
                gchandle.Free();
            }
            // Token: 0x040015A1 RID: 5537
            private EnvSDKHelper.Callback callback_;
        }
        // Token: 0x0600123C RID: 4668 RVA: 0x0003FAB8 File Offset: 0x0003FAB8
        public static void ClearSDK()
        {
            EnvSDKHelper.ClearSDKFromDll();
        }

        // Token: 0x0600123D RID: 4669 RVA: 0x0003FAC0 File Offset: 0x0003FAC0
        public static bool SetSwitch(string name, int value)
        {
            byte[] bytesNullTerminated = Encoding.UTF8.GetBytesNullTerminated(name);
            int num = Convert.ToInt32(value);
            return Convert.ToBoolean(EnvSDKHelper.SetSwitchFromDll(bytesNullTerminated, num));
        }

        // Token: 0x0600123E RID: 4670 RVA: 0x0003FAEA File Offset: 0x0003FAEA
        public static bool GetSwitch(string name, out int value)
        {
            return Convert.ToBoolean(EnvSDKHelper.GetSwitchFromDll(Encoding.UTF8.GetBytesNullTerminated(name), out value));
        }

        // Token: 0x0600123F RID: 4671 RVA: 0x0003FB04 File Offset: 0x0003FB04
        public static int InitSDK(string gameId, string secretKey, string host, out string result)
        {
            int tmpCode = 100;
            string tmpResult = string.Empty;
            EnvSDKHelper.CallbackWrapper callbackWrapper = new EnvSDKHelper.CallbackWrapper(delegate (int retCode, string retResult)
            {
                tmpCode = retCode;
                tmpResult = retResult;
            });
            GCHandle gchandle = GCHandle.Alloc(callbackWrapper);
            byte[] bytesNullTerminated = Encoding.UTF8.GetBytesNullTerminated(gameId);
            byte[] bytesNullTerminated2 = Encoding.UTF8.GetBytesNullTerminated(secretKey);
            byte[] bytesNullTerminated3 = Encoding.UTF8.GetBytesNullTerminated(host);
            EnvSDKHelper.InitSDKFromDll(bytesNullTerminated, bytesNullTerminated2, bytesNullTerminated3, new EnvSDKHelper.CallbackFromDll(EnvSDKHelper.CallbackWrapper.Callback), GCHandle.ToIntPtr(gchandle));
            GC.KeepAlive(callbackWrapper);
            GC.KeepAlive(gchandle);
            result = tmpResult;
            return tmpCode;
        }
        // Token: 0x040007D1 RID: 2001
        private static EnvSDKHelper.CallbackWrapper _initSdkWrapper;

        // Token: 0x040007D2 RID: 2002
        private static GCHandle _initSdkHandle;

        // Token: 0x06001240 RID: 4672 RVA: 0x0003FB9C File Offset: 0x0003FB9C
        public static void InitSDKAsync(string gameId, string secretKey, string host, EnvSDKHelper.Callback callback)
        {
            EnvSDKHelper._initSdkWrapper = new EnvSDKHelper.CallbackWrapper(callback);
            EnvSDKHelper._initSdkHandle = GCHandle.Alloc(EnvSDKHelper._initSdkWrapper);
            byte[] bytesNullTerminated = Encoding.UTF8.GetBytesNullTerminated(gameId);
            byte[] bytesNullTerminated2 = Encoding.UTF8.GetBytesNullTerminated(secretKey);
            byte[] bytesNullTerminated3 = Encoding.UTF8.GetBytesNullTerminated(host);
            EnvSDKHelper.InitSDKAsyncFromDll(bytesNullTerminated, bytesNullTerminated2, bytesNullTerminated3, new EnvSDKHelper.CallbackFromDll(EnvSDKHelper.CallbackWrapper.Callback), GCHandle.ToIntPtr(EnvSDKHelper._initSdkHandle));
        }

        // Token: 0x06001241 RID: 4673 RVA: 0x0003FC04 File Offset: 0x0003FC04
        public static int ReviewNickname(string nickname, out string result)
        {
            int tmpCode = 100;
            string tmpResult = string.Empty;
            GCHandle gchandle = GCHandle.Alloc(new EnvSDKHelper.CallbackWrapper(delegate (int retCode, string retResult)
            {
                tmpCode = retCode;
                tmpResult = retResult;
            }));
            EnvSDKHelper.ReviewNicknameFromDll(Encoding.UTF8.GetBytesNullTerminated(nickname), new EnvSDKHelper.CallbackFromDll(EnvSDKHelper.CallbackWrapper.Callback), GCHandle.ToIntPtr(gchandle));
            result = tmpResult;
            return tmpCode;
        }

        // Token: 0x06001242 RID: 4674 RVA: 0x0003FC74 File Offset: 0x0003FC74
        public static void ReviewNicknameAsync(string nickname, EnvSDKHelper.Callback callback)
        {
            GCHandle gchandle = GCHandle.Alloc(new EnvSDKHelper.CallbackWrapper(callback));
            EnvSDKHelper.ReviewNicknameAsyncFromDll(Encoding.UTF8.GetBytesNullTerminated(nickname), new EnvSDKHelper.CallbackFromDll(EnvSDKHelper.CallbackWrapper.Callback), GCHandle.ToIntPtr(gchandle));
        }
        // Token: 0x06001244 RID: 4676 RVA: 0x0003FD38 File Offset: 0x0003FD38
        public static void ReviewWordsAsync(string content, string level, string channel, EnvSDKHelper.Callback callback)
        {
            GCHandle gchandle = GCHandle.Alloc(new EnvSDKHelper.CallbackWrapper(callback));
            byte[] bytesNullTerminated = Encoding.UTF8.GetBytesNullTerminated(content);
            byte[] bytesNullTerminated2 = Encoding.UTF8.GetBytesNullTerminated(level);
            byte[] bytesNullTerminated3 = Encoding.UTF8.GetBytesNullTerminated(channel);
            EnvSDKHelper.ReviewWordsAsyncFromDll(bytesNullTerminated, bytesNullTerminated2, bytesNullTerminated3, new EnvSDKHelper.CallbackFromDll(EnvSDKHelper.CallbackWrapper.Callback), GCHandle.ToIntPtr(gchandle));
        }
        // Token: 0x06001243 RID: 4675 RVA: 0x0003FCB0 File Offset: 0x0003FCB0
        public static int ReviewWords(string content, string level, string channel, out string result)
        {
            int tmpCode = 100;
            string tmpResult = string.Empty;
            GCHandle gchandle = GCHandle.Alloc(new EnvSDKHelper.CallbackWrapper(delegate (int retCode, string retResult)
            {
                tmpCode = retCode;
                tmpResult = retResult;
            }));
            byte[] bytesNullTerminated = Encoding.UTF8.GetBytesNullTerminated(content);
            byte[] bytesNullTerminated2 = Encoding.UTF8.GetBytesNullTerminated(level);
            byte[] bytesNullTerminated3 = Encoding.UTF8.GetBytesNullTerminated(channel);
            EnvSDKHelper.ReviewWordsFromDll(bytesNullTerminated, bytesNullTerminated2, bytesNullTerminated3, new EnvSDKHelper.CallbackFromDll(EnvSDKHelper.CallbackWrapper.Callback), GCHandle.ToIntPtr(gchandle));
            result = tmpResult;
            return tmpCode;
        }
        // Token: 0x0200042A RID: 1066
        // (Invoke) Token: 0x060027AB RID: 10155
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void CallbackFromDll(int code, IntPtr result, IntPtr context);
    }
}
