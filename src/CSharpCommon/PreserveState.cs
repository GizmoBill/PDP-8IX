// *********************************
// *                               *
// *  Preserve Form/Control State  *
// *                               *
// *********************************

using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Xml;

namespace CSharpCommon
{
  public static class PreserveState
  {
    // These utilities support saving and restoring the state of controls and forms with minimal
    // action on the part of the programmer. To save/restore the state of a control, the programmer
    // sets its tag property to some unique name. These routines find all controls so tagged and
    // convert their values to/from strings of the form "name1 = value1; name2 = value2;", etc.
    // For forms, the save/restore state consists of its location, size (if its sizable), and normal/
    // maximized state, and all tagged controls that it contains. The state is converted to/from
    // strings in XML format.
 
    // Get state string for all tagged controls recursively found in cc. Currently the only controls
    // that are properly handled are ComboBox, CheckBox, all controls whose state is specified
    // by the Text property, and the Checked state of all menu items. Labels are a special case, used
    // for obsolete parameters. GetControlState ignores tagged labels so that obsolete parameters are
    // not included in state, but setControls processes them as usual. An invisible label can be
    // created for an obsolete parameter, with a textChanged event handler written to do something
    // appropriate with values that might be in older records.
    public static string GetControlState(Control.ControlCollection cc)
    {
      string s = string.Empty;

      foreach (Control c in cc)
      {
        if (c.Tag != null && c.Tag.GetType() == typeof(string))
        {
          string value;
          switch (c.GetType().Name)
          {
            case "ComboBox":
              value = ((ComboBox)c).SelectedIndex.ToString();
              break;

            case "CheckBox":
              value = ((CheckBox)c).Checked.ToString();
              break;

            case "NumericUpDown":
              // Can't use default case because when the spinners are clicked, Text will have
              // the old value during ValueChanged events
              value = ((NumericUpDown)c).Value.ToString();
              break;

            case "Label":
              continue;

            default:
              value = c.Text;
              break;
          }
          s += string.Format("{0} = {1};\n", c.Tag, value);
        }
        if (c.HasChildren)
          s += GetControlState(c.Controls);

        if (c is MenuStrip)
          s += GetMenuState(((MenuStrip)c).Items);
      }

      return s;
    }

    public static string GetMenuState(ToolStripItemCollection tsic)
    {
      string s = string.Empty;
      foreach (ToolStripItem tsi in tsic)
        if (tsi is ToolStripMenuItem)
        {
          ToolStripMenuItem m = (ToolStripMenuItem)tsi;
          if (m.Tag != null && m.Tag.GetType() == typeof(string))
            s += string.Format("{0} = {1};\n", m.Tag, m.Checked.ToString());
          if (m.HasDropDownItems)
            s += GetMenuState(m.DropDownItems);
        }

      return s;
    }

    // Get the form state. The string version is obsolete; use the XmlLiteNode version.
    public static string GetFormState(Form f)
    {
      Rectangle bounds;
      if (f.WindowState == FormWindowState.Normal)
        bounds = f.Bounds;
      else
        bounds = f.RestoreBounds;

      string state = string.Format("<params>\n{0}</params>\n", GetControlState(f.Controls));
      if (f.FormBorderStyle == FormBorderStyle.Sizable)
        state += string.Format("<window>{0},{1},{2},{3},{4}</window>\n",
                               bounds.Left, bounds.Top, bounds.Width, bounds.Height,
                               f.WindowState == FormWindowState.Maximized);
      else
        state += string.Format("<windowPos>{0},{1}</windowPos>\n", bounds.Left, bounds.Top);

      return state;
    }

    public static XmlLiteNode GetFormStateX(Form f, string tag = "formData")
    {
      if (f == null)
        return null;

      Rectangle bounds;
      if (f.WindowState == FormWindowState.Normal)
        bounds = f.Bounds;
      else
        bounds = f.RestoreBounds;

      XmlLiteNode state = new XmlLiteNode(tag);
      state.Children.Add(new XmlLiteNode("params", GetControlState(f.Controls)));
      if (f.FormBorderStyle == FormBorderStyle.Sizable)
        state.Children.Add(
          new XmlLiteNode("window",
                          string.Format("{0},{1},{2},{3},{4}",
                                        bounds.Left, bounds.Top, bounds.Width, bounds.Height,
                                        f.WindowState == FormWindowState.Maximized)));
      else
        state.Children.Add(new XmlLiteNode("windowPos",
                                           string.Format("{0},{1}", bounds.Left, bounds.Top)));
      return state;
    }

    // Helper used by SetControlState to recursively search cc for tagged controls matching
    // elements of ph. Elements found are removed from ph so that those not found can be
    // identified.
    private static void setControls(Control.ControlCollection cc, ParamHolder ph)
    {
      if (ph.Count > 0)
        foreach (Control c in cc)
        {
          int i;
          if (c.Tag != null && c.Tag.GetType() == typeof(string) && (i = ph.IndexOf(c.Tag.ToString())) >= 0)
          {
            switch (c.GetType().Name)
            {
              case "ComboBox":
                ((ComboBox)c).SelectedIndex = int.Parse(ph[i]);
                break;

              case "CheckBox":
                ((CheckBox)c).Checked = bool.Parse(ph[i]);
                break;

              default:
                c.Text = ph[i];
                break;
            }
            ph.RemoveAt(i);
          }
          if (c.HasChildren)
            setControls(c.Controls, ph);
          if (c is MenuStrip)
            setMenus(((MenuStrip)c).Items, ph);
        }
    }

    private static void setMenus(ToolStripItemCollection tsic, ParamHolder ph)
    {
      if (ph.Count > 0)
        foreach (ToolStripItem tsi in tsic)
          if (tsi is ToolStripMenuItem)
          {
            ToolStripMenuItem m = (ToolStripMenuItem)tsi;
            int i;
            if (m.Tag != null && m.Tag.GetType() == typeof(string) && (i = ph.IndexOf(m.Tag.ToString())) >= 0)
            {
              m.Checked = bool.Parse(ph[i]);
              ph.RemoveAt(i);
            }
            if (m.HasDropDownItems)
              setMenus(m.DropDownItems, ph);
          }
    }

    // Restore state of tagged controls in cc from state string p. Return an array of all names
    // in p not found in cc.
    public static string[] SetControlState(Control.ControlCollection cc, string p)
    {
      ParamHolder ph = new ParamHolder(p);
      setControls(cc, ph);
      return ph.Names;
    }

    // Restore form state from string. The string version is obsolete; use the XmlLiteNode version.
    public static void SetFormState(Form f, string state)
    {
      StringReader sr = new StringReader(state);
      XmlReaderSettings settings = new XmlReaderSettings();
      settings.ConformanceLevel = ConformanceLevel.Fragment;
      XmlReader xml = XmlReader.Create(sr, settings);

      xml.Read();
      while (!xml.EOF)
      {
        if (xml.NodeType == XmlNodeType.Element)
          switch (xml.Name)
          {
            case "params":
              SetControlState(f.Controls, xml.ReadElementContentAsString());
              break;

            case "windowPos":
              string[] w = xml.ReadElementContentAsString().Split(',');
              Point p = new Point(int.Parse(w[0]), int.Parse(w[1]));
              Rectangle r = new Rectangle(p, f.Size);
              foreach (Screen s in Screen.AllScreens)
                if (s.WorkingArea.IntersectsWith(r))
                {
                  f.Location = p;
                  break;
                }
              break;

            case "window":
              w = xml.ReadElementContentAsString().Split(',');
              r = new Rectangle(int.Parse(w[0]), int.Parse(w[1]), int.Parse(w[2]), int.Parse(w[3]));
              foreach (Screen s in Screen.AllScreens)
                if (s.WorkingArea.IntersectsWith(r))
                {
                  f.Bounds = r;
                  if (bool.Parse(w[4]))
                    f.WindowState = FormWindowState.Maximized;
                  break;
                }
              break;

            default:
              xml.Skip();
              break;
          }
        else
          xml.Skip();
      }

      xml.Close();
    }

    public static void SetFormState(Form f, XmlLiteNode state, string tag = "formData")
    {
      XmlLiteNode root = state;
      if (root != null && root.Tag != tag)
        root = root[tag];
      if (root == null)
        return;

      XmlLiteNode node = root["params"];
      if (node != null)
        SetControlState(f.Controls, node.Value);

      node = root["windowPos"];
      if (node != null)
      {
        string[] w = node.Value.Split(',');
        Point p = new Point(int.Parse(w[0]), int.Parse(w[1]));
        Rectangle r = new Rectangle(p, f.Size);
        foreach (Screen s in Screen.AllScreens)
          if (s.WorkingArea.IntersectsWith(r))
          {
            f.Location = p;
            break;
          }
      }

      node = root["window"];
      if (node != null)
      {
        string[]w = node.Value.Split(',');
        Rectangle r = new Rectangle(int.Parse(w[0]), int.Parse(w[1]), int.Parse(w[2]), int.Parse(w[3]));
        foreach (Screen s in Screen.AllScreens)
          if (s.WorkingArea.IntersectsWith(r))
          {
            f.Bounds = r;
            if (bool.Parse(w[4]))
              f.WindowState = FormWindowState.Maximized;
            break;
          }
      }
    }
  }

  // *******************
  // *                 *
  // *  Optional Form  *
  // *                 *
  // *******************
  //
  // An optional form is a Form object that is not constructed until called for,
  // but which has state that can be read and written regardless of whether the
  // Form has been constructed. The state is that which is saved and restored
  // by PreserveState. When the form is constructed, previously written state
  // is automatically applied, and the FormCreated event is raised.
  //
  // Optional forms are used when an application has forms that are constructed
  // on first use, and where their state is to be saved and restored. An
  // optional form's state can be restored without actually constructing the form.

  public class OptionalForm<F> where F : Form, new()
  {
    private F form = null;

    private XmlLiteNode state = null;

    private string tag = null;

    public OptionalForm(string tag)
    {
      this.tag = tag;
    }

    public OptionalForm(string tag, FormCreatedEventHandler formCreated)
    {
      this.tag = tag;
      FormCreated += formCreated;
    }

    public delegate void FormCreatedEventHandler(F form);
    public event FormCreatedEventHandler FormCreated;

    public F Form
    {
      get
      {
        if (form == null)
        {
          form = new F();
          if (state != null)
            PreserveState.SetFormState(form, state, tag);
          if (FormCreated != null)
            FormCreated(form);
        }
        return form;
      }
    }

    public XmlLiteNode State
    {
      get
      {
        if (form == null)
          return state;
        else
          return PreserveState.GetFormStateX(form, tag);
      }
      set
      {
        if (form == null)
        {
          state = value;
          if (state != null && tag!= null && state.Tag != tag)
            state = state[tag];
        }
        else
          PreserveState.SetFormState(form, value);
      }
    }

    public XmlLiteNode SaveState(XmlLiteNode node)
    {
      XmlLiteNode state = State;
      if (state != null)
        node.Children.Add(state);
      return node;
    }

    public bool Exists
    {
      get { return form != null;}
    }

    public void Reset()
    {
      if (Exists)
      {
        form.Close();
        form = null;
      }
      state = null;
    }
  }
}
