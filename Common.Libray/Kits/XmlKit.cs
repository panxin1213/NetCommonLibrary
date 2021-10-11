using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace ChinaBM.Common
{
    public class XmlKit
    {
        #region 频道参数XML文件

        public static Dictionary<string, string> GetParamXML(string xmlpath, bool isPhysicsPath)
        {
            Dictionary<string, string> dictions = new Dictionary<string, string>();
            XmlDocument document = new XmlDocument();
            if (isPhysicsPath)
            {
                string filepath = HttpKit.GetMapPath(xmlpath);
                if (!File.Exists(filepath))
                {
                    return dictions;
                }
                document.Load(filepath);
                dictions = GetParamXML(document);
                return dictions;
            }
            else
            {
                try
                {
                    document.LoadXml(xmlpath);
                    return GetParamXML(document);
                }
                catch
                {
                    return dictions;
                }
            }
        }

        public static Dictionary<string, string> GetParamXML(string xmlpath)
        {
            return GetParamXML(xmlpath, true);
        }

        public static Dictionary<string, string> GetParamXML(XmlDocument document)
        {
            Dictionary<string, string> dictions = new Dictionary<string, string>();
            if (document != null)
            {
                XmlNode rootnode = document.SelectSingleNode("root");
                XmlNodeList nodeList = rootnode.ChildNodes;
                foreach (XmlNode node in nodeList)
                {
                    dictions.Add(node.Name, node.InnerXml);
                }
            }
            return dictions;
        }

        public static void SaveParamXML(string xmlpath, Dictionary<string, string> paramList)
        {
            SaveParamXML(xmlpath, paramList, null);
        }

        public static void SaveParamXML(string xmlpath, Dictionary<string, string> paramList, Dictionary<string, Dictionary<string, string>> attrList)
        {
            if (paramList == null)
            {
                return;
            }
            if (paramList.Count == 0)
            {
                return;
            }

            XmlDocument document = new XmlDocument();
            string filepath = HttpKit.GetMapPath(xmlpath);

            if (File.Exists(filepath))
            {
                SaveXML(filepath, document, paramList, attrList);
            }
            else
            {
                CreateDocument(ref document, paramList, attrList);
                string file = filepath.Substring(0, filepath.LastIndexOf("\\"));
                if (!File.Exists(file))
                {
                    Directory.CreateDirectory(file);
                }
                document.Save(filepath);
            }
        }

        //nodelist的key为attrList的key
        public static void SaveXML(string xmlpath, XmlDocument document, Dictionary<string, string> nodelist, Dictionary<string, Dictionary<string, string>> attrList)
        {
            document.Load(xmlpath);
            XmlNode rootNode = document.SelectNodes("root")[0];
            foreach (KeyValuePair<string, string> keyValue in nodelist)
            {
                XmlNodeList nodes = rootNode.SelectNodes(keyValue.Key);
                if (nodes.Count == 0)
                {
                    XmlNode node = document.CreateNode(XmlNodeType.Element, keyValue.Key, null);
                    node.InnerXml = keyValue.Value;
                    if (attrList.ContainsKey(keyValue.Key))
                    {
                        foreach (KeyValuePair<string, string> attrKeyValue in attrList[keyValue.Key])
                        {
                            XmlAttribute xmlAttr = document.CreateAttribute(attrKeyValue.Key);
                            xmlAttr.Value = attrKeyValue.Value;
                            node.Attributes.Append(xmlAttr);
                        }
                    }
                    rootNode.AppendChild(node);
                }
                else
                {
                    nodes[0].InnerXml = keyValue.Value;
                    if (attrList.ContainsKey(keyValue.Key))
                    {
                        foreach (KeyValuePair<string, string> attrKeyValue in attrList[keyValue.Key])
                        {
                            nodes[0].Attributes[attrKeyValue.Key].Value = attrKeyValue.Value;
                        }
                    }
                }
            }
            document.Save(xmlpath);
        }

        public static void CreateDocument(ref XmlDocument document, Dictionary<string, string> paramList, Dictionary<string, Dictionary<string, string>> attrlist)
        {
            XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "gb2312", "yes");
            document.AppendChild(declaration);
            XmlNode rootNode = document.CreateNode(XmlNodeType.Element, "root", null);
            document.AppendChild(rootNode);
            foreach (KeyValuePair<string, string> keyValue in paramList)
            {
                XmlNode node = document.CreateNode(XmlNodeType.Element, keyValue.Key, null);
                node.InnerXml = keyValue.Value;
                if (attrlist.ContainsKey(keyValue.Key))
                {
                    foreach (KeyValuePair<string, string> attrKeyValue in attrlist[keyValue.Key])
                    {
                        XmlAttribute xmlAttr = document.CreateAttribute(attrKeyValue.Key);
                        xmlAttr.Value = attrKeyValue.Value;
                        node.Attributes.Append(xmlAttr);
                    }
                }
                rootNode.AppendChild(node);
            }
        }

        public static void CreateDocument(ref XmlDocument document, Dictionary<string, string> paramList)
        {
            CreateDocument(ref document, paramList, null);
        }


        #endregion
    }
}
