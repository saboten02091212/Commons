// ******************************************************************
// AppEncryption.cs  ：暗号化クラス
// 作成　　：2012/12/12 水落
// 更新履歴：2013/03/19 水落　　 名前空間を修正。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.Security.Cryptography;

namespace Mizuochi.Commons.Codes.Security
{
    /// <summary>
    /// 暗号化を行うクラスです。
    /// </summary>
    public class Encryption : IDisposable
    {
        #region 定数
        /// <summary>
        /// 反復処理回数
        /// </summary>
        private const int IterationCount = 10000;
        /// <summary>
        /// 暗号化した文字列中の置換する文字
        /// </summary>
        private const char targetChar = '/';
        /// <summary>
        /// targetCharを置換する文字
        /// </summary>
        private const char destinationChar = '!';
        #endregion

        #region 静的メンバ
        /// <summary>
        /// 暗号化のキーを得る際に使用する文字列
        /// </summary>
        private static string salt = "Th45VGnOMpXzzMRRrW9U4zTcQIsEJPL";

        /// <summary>
        /// パスワードを暗号化する際に使用する文字列
        /// </summary>
        private static string passwordKey = "Yrasex0UUmvvX2CCTPNftkmgejJE04y";

        /// <summary>
        /// パスワード用の暗号化/復号化インスタンス
        /// </summary>
        public static readonly Encryption PasswordEncryption = new Encryption(Encryption.PasswordKey);

        /// <summary>
        /// 暗号化のキーを得る際に使用する文字列を設定、取得します。
        /// </summary>
        public static string Salt
        {
            get
            {
                return Encryption.salt;
            }
            set
            {
                Encryption.salt = value;
            }
        }

        /// <summary>
        /// パスワードを暗号化する際に使用する文字列を設定、取得します。
        /// </summary>
        public static string PasswordKey
        {
            get
            {
                return Encryption.passwordKey;
            }
            set
            {
                Encryption.passwordKey = value;
            }
        }
        #endregion

        #region フィールド
        /// <summary>
        /// キー
        /// </summary>
        private string key;
        /// <summary>
        /// 暗号化ロジックインスタンス
        /// </summary>
        private AesCryptoServiceProvider aes;

        /// <summary>
        /// リソースが解放されたかどうかを示します。
        /// </summary>
        private bool disposed;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 指定されたキーでインスタンスを生成します。
        /// </summary>
        /// <param name="key"></param>
        public Encryption(string key)
        {
            // リソース解放フラグ設定
            this.disposed = false;

            // キー設定
            this.key = key;

            // 暗号化ロジックインスタンス生成
            byte[] byteKey;
            byte[] byteIv;
            this.aes = new AesCryptoServiceProvider();
            Encryption.GenerateKeyFromPassword(
                key,
                aes.KeySize,
                out byteKey,
                aes.BlockSize,
                out byteIv);
            aes.Key = byteKey;
            aes.IV = byteIv;
        }

        /// <summary>
        /// インスタンス破棄時に動作します。
        /// </summary>
        ~Encryption()
        {
            this.Dispose(false);
        }
        #endregion

        #region パブリックメソッド
        /// <summary>
        /// 文字列を暗号化する
        /// </summary>
        /// <param name="sourceString">暗号化する文字列</param>
        /// <returns>>暗号化された文字列</returns>
        public string Encrypt(string sourceString)
        {
            //文字列をバイト型配列に変換する
            byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(sourceString);

            // 暗号化
            byte[] encBytes;
            using (ICryptoTransform encryptor = this.aes.CreateEncryptor())
            {
                encBytes = encryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
            }

            // バイト型配列を文字列に変換する
            string encString = System.Convert.ToBase64String(encBytes);
            encString = encString.Replace(Encryption.targetChar, Encryption.destinationChar);
            return encString;
        }

        /// <summary>
        /// 暗号化された文字列を復号化する
        /// </summary>
        /// <param name="sourceString">暗号化された文字列</param>
        /// <returns>>復号化された文字列</returns>
        public string Decrypt(string sourceString)
        {
            //文字列をバイト型配列に変換する
            string replaceSourceString = sourceString.Replace(Encryption.destinationChar, Encryption.targetChar);
            byte[] strBytes = System.Convert.FromBase64String(replaceSourceString);

            // 復号化
            byte[] encBytes;
            using (ICryptoTransform decryptor = this.aes.CreateDecryptor())
            {
                encBytes = decryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
            }

            // バイト型配列を文字列に変換する
            string encString = System.Text.Encoding.UTF8.GetString(encBytes);
            return encString;
        }
        #endregion

        #region プライベートメソッド
        /// <summary>
        /// パスワードから共有キーと初期化ベクタを生成する
        /// </summary>
        /// <param name="password">基になるパスワード</param>
        /// <param name="keySize">共有キーのサイズ（ビット）</param>
        /// <param name="key">作成された共有キー</param>
        /// <param name="blockSize">初期化ベクタのサイズ（ビット）</param>
        /// <param name="iv">作成された初期化ベクタ</param>
        private static void GenerateKeyFromPassword(
            string password,
            int keySize,
            out byte[] key,
            int blockSize,
            out byte[] iv)
        {
            //saltを決める
            byte[] salt = System.Text.Encoding.UTF8.GetBytes(Encryption.Salt);

            //Rfc2898DeriveBytesオブジェクトを作成
            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, salt);

            //反復処理回数を指定
            deriveBytes.IterationCount = Encryption.IterationCount;

            //共有キーと初期化ベクタを生成
            key = deriveBytes.GetBytes(keySize / 8);
            iv = deriveBytes.GetBytes(blockSize / 8);

            return;
        }

        #region リソース解放
        /// <summary>
        /// 保有しているリソースを解放します。
        /// </summary>
        /// <param name="disposing">マネージリソースを解放する場合、true。それ以外の場合、falseを設定します。</param>
        private void Dispose(bool disposing)
        {
            // 既に解放されていた場合、再度 解放しない
            if (true == this.disposed)
            {
                return;
            }

            // マネージリソースを解放
            if (true == disposing)
            {
                this.ReleaseManagedResource();
            }

            // アンマネージリソースを解放
            this.ReleaseUnmanagedResource();

            this.disposed = true;

            return;
        }

        /// <summary>
        /// マネージリソースを解放します。
        /// </summary>
        private void ReleaseManagedResource()
        {
            this.aes.Dispose();
            return;
        }

        /// <summary>
        /// アンマネージリソースを解放します。
        /// </summary>
        private void ReleaseUnmanagedResource()
        {
            return;
        }
        #endregion
        #endregion

        #region IDisposable メンバー
        /// <summary>
        /// このインスタンスで管理しているリソースを解放します。
        /// </summary>
        void IDisposable.Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);

            return;
        }
        #endregion
    }
}
