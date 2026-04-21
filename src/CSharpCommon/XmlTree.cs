// **********************************************************
// *                                                        *
// *  XML Tree Structure for Reading, Writing, and Editing  *
// *                                                        *
// **********************************************************

// An XmlLiteNode is a general-purpose tree node that mirrors the XML nodes of
// results in a database results file. Each has a Tag and Value, both strings,
// and zero or more child nodes. I might have just used XmlNode, since XML is
// where the data and structure comes from, but I find XmlNode and its related
// classes to be heavier than I need and at times documented with less precision
// than I'd like. For example, the XML reading style coded in Read has been found
// emperically to work for empty/non-empty elements with and without attributes.
// I could not figure this out from the documentation, and I spent quite some
// time trying. 

using CSharpCommon;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class XmlLiteNode : IXmlElement
{
  public string Tag = null;
  public string Value = string.Empty;
  public List<XmlLiteNode> Children = new List<XmlLiteNode>();
  Dictionary<string, string> attributes;

  public XmlLiteNode() {}

  public XmlLiteNode(string tag, string value = "")
  {
    Tag = tag;
    Value = value;
  }

  // Read xml into this node until an unmatched end element is found, which is consumed.
  private XmlLiteNode Read(XmlReader xml)
  {
    Tag = xml.Name;
    bool empty = xml.IsEmptyElement;
    while (xml.MoveToNextAttribute())
      SetAttribute(xml.Name, xml.Value);
    if (!empty)
    {
      while (xml.NodeType != XmlNodeType.EndElement)
      {
        xml.Read();
        switch (xml.NodeType)
        {
          case XmlNodeType.Element:
            Children.Add(new XmlLiteNode().Read(xml));
            break;

          case XmlNodeType.Text:
            Value += xml.Value;
            break;
        }
      }
      xml.Read();
    }
    return this;
  }

  private static XmlLiteNode ReadFromText(TextReader tr)
  {
    XmlLiteNode root = new XmlLiteNode();
    XmlReaderSettings settings = new XmlReaderSettings();
    settings.ConformanceLevel = ConformanceLevel.Fragment;
    XmlReader xml = XmlReader.Create(tr, settings);
    while (!xml.EOF)
    {
      xml.Read();
      if (xml.NodeType == XmlNodeType.Element)
        root.Children.Add((new XmlLiteNode()).Read(xml));
    }

    tr.Close();
    return root;
  }

  public static XmlLiteNode ReadFromString(string src)
  {
    return ReadFromText(new StringReader(src));
  }

  public static XmlLiteNode ReadFromFile(string filename)
  {
    return ReadFromText(new StreamReader(filename));
  }

  public override string ToString()
  {
    return CSharpCommon.XmlWriter.WriteString(this, 0);
  }

  // Indexer that returns the first child node with the specified tag, or null if none.
  public XmlLiteNode this[string tag]
  {
    get
    {
      foreach (XmlLiteNode node in Children)
        if (node.Tag == tag)
          return node;
      return null;
    }
  }

  // Return an array of all child nodes with the specified tag, containing an attribute:
  //        attr          val           look for
  //        null          -             any node
  //        "key"         null          must not contain key
  //        "key"         ""            must contain key, any value will do
  //        "key"         "val"         must contain key="val"
  public List<XmlLiteNode> FindList(string tag, string attr, string val)
  {
    List<XmlLiteNode> nodes = new List<XmlLiteNode>();
    foreach (XmlLiteNode node in Children)
      if (node.Tag == tag &&
          (attr == null ||
            val  == null         && node.GetAttribute(attr) == null ||
            val  == string.Empty && node.GetAttribute(attr) != null ||
                                    node.GetAttribute(attr) == val))
        nodes.Add(node);
    return nodes;
  }

  public XmlLiteNode[] Find(string tag)
  {
    return FindList(tag, null, null).ToArray();
  }

  public XmlLiteNode[] Find(string tag, string attribute, string value)
  {
    return FindList(tag, attribute, value).ToArray();
  }

  // Add attribute
  public virtual void SetAttribute(string tag, string value)
  {
    if (attributes == null)
      if (value != null)
        attributes = new Dictionary<string,string>();
      else
        return;
    if (value != null)
      attributes[tag] = value;
    else
      attributes.Remove(tag);
  }

  // Lookup attribute
  public string GetAttribute(string key)
  {
    if (attributes == null)
      return null;
    return attributes.ContainsKey(key) ? attributes[key] : null;
  }

  //public XmlLiteNode Difference(XmlLiteNode root)
  //{
  //  XmlLiteNode diff = new XmlLiteNode(Tag);
  //  if (attributes != null)
  //    foreach (string k in attributes.Keys)
  //      if (k != root.GetAttribute(k))
  //}

  // IXmlElement interface
  string IXmlElement.Tag
  {
    get { return Tag; }
  }

  public Dictionary<string, string> Attributes
  {
    get { return attributes; }
  }

  string IXmlElement.Value
  {
    get { return Value; }
  }

  bool IXmlElement.HasChildren
  {
    get { return Children.Count > 0; }
  }

  IEnumerable<IXmlElement> IXmlElement.Children
  {
    get
    {
      foreach (XmlLiteNode node in Children)
        yield return node;
    }
  }

}
