// ******************************************************************
// XmlParse.cs  ：XML解析クラス
// 作成日　：2012/12/12
// 更新履歴：2012/12/12 水落　　 新規作成。
// Copyright (c) MIZUOCHI SEIICHI 2013
// ******************************************************************
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Mizuochi.Commons.Codes.IO
{
    /// <summary>
    /// XML解析クラス
    /// </summary>
    public static class XmlParse
    {
        #region 静的パブリックメソッド
        /// <summary>
        /// XML文書からオブジェクトを生成します。
        /// </summary>
        /// <typeparam name="XmlType">生成するオブジェクト型</typeparam>
        /// <param name="xmlStr">XML文書</param>
        /// <exception cref="System.InvalidOperationException">xmlStrがXMLデシリアライズできない場合、発生します。</exception>
        /// <returns>XML文書から生成したオブジェクト</returns>
        public static XmlType Deserialize<XmlType>(string xmlStr)
        {
            XmlType xmlTypeObject;

            // XML文書のストリームを取得
            using (Stream srm = new MemoryStream(Encoding.UTF8.GetBytes(xmlStr)))
            {
                using (StreamReader sr = new StreamReader(srm))
                {
                    // 変換するXMLを設定
                    XmlSerializer xs = new XmlSerializer(typeof(XmlType));

                    try
                    {
                        // 逆シリアル化
                        xmlTypeObject = (XmlType)xs.Deserialize(sr);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return xmlTypeObject;
        }

        /// <summary>
        /// オブジェクトからXML文書を生成します。
        /// </summary>
        /// <typeparam name="XmlType">生成元のオブジェクト型</typeparam>
        /// <param name="xmlObject">生成元のオブジェクト</param>
        /// <exception cref="System.InvalidOperationException">xmlObjectがXMLシリアライズできない場合、発生します。</exception>
        /// <returns>オブジェクトから生成したXML文書</returns>
        public static string Serialize<XmlType>(XmlType xmlObject)
        {
            string outputXml;

            // 出力するXML文書のストリームを取得
            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlTextWriter xtw = new XmlTextWriter(ms, new UTF8Encoding(false)))
                {
                    // 変換するXMLを設定
                    XmlSerializer xs = new XmlSerializer(xmlObject.GetType());

                    try
                    {
                        // シリアル化
                        XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                        namespaces.Add("", "");
                        xs.Serialize(xtw, xmlObject, namespaces);
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    // ストリームから文字列に変換
                    outputXml = Encoding.UTF8.GetString(ms.ToArray());
                }
            }

            return outputXml;
        }
        #endregion
    }
}
