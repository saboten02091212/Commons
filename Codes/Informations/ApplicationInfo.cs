// ******************************************************************
// ApplicationInfo.cs ： アプリケーション情報クラス
// 作成日　：2012/12/12
// 更新履歴：2013/04/11 水落　　 取得できないプロパティを取得できるよう修正。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace Mizuochi.Commons.Codes.Informations
{
    /// <summary>
    /// アプリケーションの情報を取得する機能を保持するクラスです。
    /// </summary>
    public static class ApplicationInfo
    {
        #region 静的プロパティ
        /// <summary>
        /// アプリケーションのファイル名を含めたパスを取得します。
        /// </summary>
        public static string FilePath
        {
            get
            {
                return Application.ExecutablePath;
            }
        }

        /// <summary>
        /// アプリケーションの実行ファイルが存在するディレクトリを取得します。
        /// </summary>
        public static string Path
        {
            get
            {
                return System.IO.Path.GetDirectoryName(ApplicationInfo.FilePath);
            }
        }

        /// <summary>
        /// アプリケーションのファイル名を取得します。
        /// </summary>
        public static string FileName
        {
            get
            {
                return System.IO.Path.GetFileName(ApplicationInfo.FilePath);
            }
        }

        /// <summary>
        /// アプリケーションの拡張子を除いたファイル名を取得します。
        /// </summary>
        public static string Name
        {
            get
            {
                return System.IO.Path.ChangeExtension(ApplicationInfo.FileName, String.Empty);
            }
        }

        /// <summary>
        /// アプリケーションのファイルバージョンを取得します。
        /// </summary>
        public static FileVersionInfo FileVersion
        {
            get
            {
                return FileVersionInfo.GetVersionInfo(ApplicationInfo.FilePath);
            }
        }

        /// <summary>
        /// アプリケーションのアセンブリバージョンを取得します。
        /// </summary>
        public static string AssemblyVersion
        {
            get
            {
                return Assembly.GetEntryAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// アプリケーションのアセンブリのタイトルを取得します。
        /// </summary>
        public static string AssemblyTitle
        {
            get
            {
                AssemblyTitleAttribute asmTitle =
                    Attribute.GetCustomAttribute(
                        Assembly.GetEntryAssembly(),
                        typeof(AssemblyTitleAttribute)) as AssemblyTitleAttribute;

                return asmTitle.Title;
            }
        }

        /// <summary>
        /// アプリケーションのアセンブリに記載されている説明を取得します。
        /// </summary>
        public static string AssemblyDescription
        {
            get
            {
                AssemblyDescriptionAttribute asmDescription =
                    Attribute.GetCustomAttribute(
                        Assembly.GetEntryAssembly(),
                        typeof(AssemblyDescriptionAttribute)) as AssemblyDescriptionAttribute;

                return asmDescription.Description;
            }
        }

        /// <summary>
        /// アプリケーションのアセンブリに記載されている会社名を取得します。
        /// </summary>
        public static string AssemblyCompany
        {
            get
            {
                AssemblyCompanyAttribute asmCompany =
                    Attribute.GetCustomAttribute(
                        Assembly.GetEntryAssembly(),
                        typeof(AssemblyCompanyAttribute)) as AssemblyCompanyAttribute;

                return asmCompany.Company;
            }
        }

        /// <summary>
        /// アプリケーションのアセンブリに記載されている製品名を取得します。
        /// </summary>
        public static string AssemblyProduct
        {
            get
            {
                AssemblyProductAttribute asmProduct =
                    Attribute.GetCustomAttribute(
                        Assembly.GetEntryAssembly(),
                        typeof(AssemblyProductAttribute)) as AssemblyProductAttribute;

                return asmProduct.Product;
            }
        }

        /// <summary>
        /// アプリケーションのアセンブリに記載されている著作権を取得します。
        /// </summary>
        public static string AssemblyCopyright
        {
            get
            {
                AssemblyCopyrightAttribute asmCopyright =
                    Attribute.GetCustomAttribute(
                        Assembly.GetEntryAssembly(),
                        typeof(AssemblyCopyrightAttribute)) as AssemblyCopyrightAttribute;

                return asmCopyright.Copyright;
            }
        }

        /// <summary>
        /// アプリケーションのアセンブリに記載されている商標を取得します。
        /// </summary>
        public static string AssemblyTrademark
        {
            get
            {
                AssemblyTrademarkAttribute asmTrademark =
                    Attribute.GetCustomAttribute(
                        Assembly.GetEntryAssembly(),
                        typeof(AssemblyTrademarkAttribute)) as AssemblyTrademarkAttribute;

                return asmTrademark.Trademark;
            }
        }
        #endregion
    }
}
