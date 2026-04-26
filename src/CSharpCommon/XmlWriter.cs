// *****************
// *               *
// *  XML Classes  *
// *               *
// *****************

using System;
using System.Collections.Generic;
using System.IO;

namespace CSharpCommon
{
  public static class XmlUtil
  {
    public static string xmlText(string s)
    {
      s = s.Replace("&" , "&amp;" );
      s = s.Replace("\"", "&quot;");
      s = s.Replace("'" , "&apos;");
      s = s.Replace("<" , "&lt;"  );
      s = s.Replace(">" , "&gt;"  );
      return s;
    }
  }

  // ****************
  // *              *
  // *  XML Writer  *
  // *              *
  // ****************

  public interface IXmlElement
  {
    string Tag                            { get;}
    Dictionary<string, string> Attributes { get;}
    string Value                          { get;}
    bool HasChildren                       { get;}
    IEnumerable<IXmlElement> Children      { get;}
  }

  public class XmlValue : IXmlElement
  {
    string tag;
    object obj;

    public XmlValue(string tag, object obj)
    {
      this.tag = tag;
      this.obj = obj;
    }

    public string Tag
    {
      get { return tag; }
    }

    public Dictionary<string, string> Attributes
    {
      get { return null; }
    }

    public string Value
    {
      get { return obj.ToString(); }
    }

    public bool HasChildren
    {
      get { return false; }
    }

    public IEnumerable<IXmlElement> Children
    {
      get { yield break;}
    }
  }

  public static class XmlWriter
  {
    public static void WriteFile(IXmlElement xml, string filename, bool backup, int indent)
    {
      if (backup)
        FileUtil.fileBackup(filename);

      using (StreamWriter sw = new StreamWriter(filename, false))
      {
        write(xml, sw, 0, indent);
        sw.Close();
      }
    }

    public static string WriteString(IXmlElement xml, int indent)
    {
      string s;
      using (StringWriter sw = new StringWriter())
      {
        write(xml, sw, 0, indent);
        s = sw.ToString();
        sw.Close();
      }
      return s;
    }

    static void write(IXmlElement xml, TextWriter tw, int level, int indent)
    {
      Dictionary<string, string> attributes = xml.Attributes;
      string a = string.Empty;
      if (attributes != null)
        foreach (string key in attributes.Keys)
          a += string.Format(" {0}='{1}'", key, attributes[key]);

      string v = XmlUtil.xmlText(xml.Value);

      bool noIndent = xml.Attributes != null && xml.Attributes.ContainsKey("noIndent");
      string spaces = string.Empty;
      if (noIndent)
        indent = 0;
      else
        spaces = new string(' ', level * indent);

      if (v == string.Empty && !xml.HasChildren)
      {
        tw.Write(string.Format("{0}<{1}{2}/>{3}", spaces, xml.Tag, a, Environment.NewLine));
        return;
      }

      if (!v.Contains("\n") && !xml.HasChildren)
      {
        tw.Write(string.Format("{0}<{1}{2}>{3}</{1}>{4}", spaces, xml.Tag, a, v, Environment.NewLine));
        return;
      }

      if (indent == 0)
      {
        if (v == string.Empty)
          v = Environment.NewLine;
        tw.Write(string.Format("<{0}{1}>{2}", xml.Tag, a, v));
      }
      else
      {
        string spaces1 = new string(' ', (level + 1) * indent);

        string[] lines = v.Split('\n');
        v = string.Empty;
        foreach (string line in lines)
        {
          string s = line.Trim();
          if (s != string.Empty)
            v += spaces1 + s + Environment.NewLine;
        }

        tw.Write(string.Format("{0}<{1}{2}>{3}{4}", spaces, xml.Tag, a, Environment.NewLine, v));
      }

      foreach (IXmlElement e in xml.Children)
        write(e, tw, level + 1, indent);
      tw.Write(string.Format("{0}</{1}>{2}", spaces, xml.Tag, Environment.NewLine));
    }
  }

}
