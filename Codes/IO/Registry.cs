// ******************************************************************
// Registry.cs  ：レジストリ操作クラス
// 作成日　：2013/01/18
// 更新履歴：2013/10/15 水落　　 製品関連キーが削除できない不具合を修正。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mizuochi.Commons.Codes.Informations;
using Microsoft.Win32;

namespace Mizuochi.Commons.Codes.IO
{
    /// <summary>
    /// アプリケーションのレジストリを操作するクラス。
    /// </summary>
    public static class Registry
    {
        /// <summary>
        /// 組織キー
        /// </summary>
        private static string organizationKeyName;

        /// <summary>
        /// 組織キーを設定、取得します。
        /// </summary>
        public static string OrganizationKeyName
        {
            get
            {
                if (default(String) == Registry.organizationKeyName)
                {
                    throw new InvalidOperationException("組織キーが未設定です。");
                }

                return Registry.organizationKeyName;
            }
            set
            {
                Registry.organizationKeyName = value;
            }
        }

        /// <summary>
        /// ルートキーを取得します。
        /// </summary>
        private static string OrganizationKey
        {
            get
            {
                return Path.Combine("Software", Registry.OrganizationKeyName);
            }
        }

        /// <summary>
        /// 製品キーを取得します。
        /// </summary>
        private static string ProductKey
        {
            get
            {
                return Path.Combine(Registry.OrganizationKey, ApplicationInfo.AssemblyTitle);
            }
        }

        /// <summary>
        /// レジストリに指定されたキーの値を設定します。
        /// </summary>
        /// <param name="subKey">サブキー</param>
        /// <param name="valueName">名前</param>
        /// <param name="value">値</param>
        public static void Write(string subKey, string valueName, object value)
        {
            string path = Path.Combine(Registry.ProductKey, subKey);
            using (RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(path))
            {
                regKey.SetValue(valueName, value);
            }

            return;
        }

        /// <summary>
        /// レジストリに指定されたキーの値を取得します。
        /// </summary>
        /// <param name="subKey">サブキー</param>
        /// <param name="valueName">名前</param>
        /// <returns>値</returns>
        public static object Read(string subKey, string valueName)
        {
            object value;

            string path = Path.Combine(Registry.ProductKey, subKey);
            using (RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(path))
            {
                value = regKey.GetValue(valueName);
            }

            return value;
        }

        /// <summary>
        /// レジストリから指定されたキーの値を削除します。
        /// </summary>
        /// <param name="subKey">サブキー</param>
        /// <param name="valueName">名前</param>
        public static void Delete(string subKey, string valueName)
        {
            string path = Path.Combine(Registry.ProductKey, subKey);
            using (RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(path))
            {
                regKey.DeleteValue(valueName);
            }

            return;
        }

        /// <summary>
        /// レジストリに指定されたキーを作成します。
        /// </summary>
        /// <param name="subKey">サブキー</param>
        public static void CreateSubKey(string subKey)
        {
            string path = Registry.ProductKey;
            using (RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(path))
            {
                using (RegistryKey regSubKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(subKey))
                {
                    regSubKey.Close();
                }
            }

            return;
        }

        /// <summary>
        /// レジストリから指定されたキーを削除します。
        /// </summary>
        /// <param name="subKey">サブキー</param>
        public static void DeleteSubKey(string subKey)
        {
            string path = Registry.ProductKey;
            using (RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(path))
            {
                regKey.DeleteSubKeyTree(subKey);
            }

            return;
        }

        /// <summary>
        /// レジストリから製品に関連するキーを削除します。
        /// </summary>
        public static void DeleteProductTree()
        {
            string path = Registry.OrganizationKey;
            using (RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(path))
            {
                regKey.DeleteSubKeyTree(ApplicationInfo.AssemblyTitle);
            }

            return;
        }
    }
}
