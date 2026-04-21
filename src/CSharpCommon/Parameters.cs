
using System.Collections.Generic;

namespace CSharpCommon
{
  // ***********************
  // *                     *
  // *  Parameters Holder  *
  // *                     *
  // ***********************

  // A ParamHolder holds zero or more (name,value) pairs, both strings, for easy manipulation
  // and conversion to/from "name1 = value1; name2 = value2; ..." strings (assignment format).
  // Note that a Dictionary<string> could be used instead of two List<string>s, but the
  // Dictionary mechanism (a hash table) seems a little too heavy for the small sizes intended.

  public class ParamHolder
  {
    List<string> names;
    List<string> values;

    // Construct empty
    public ParamHolder()
    {
      names  = new List<string>();
      values = new List<string>();
    }

    // Construct from assignment-format string
    public ParamHolder(string p)
      : this()
    {
      Load(p);
    }

    // Construct from other ParamHolder by copying the lists. The underlying strings will be shared
    // (unless replaced), but the lists themselves will be distinct, which is what allows the
    // underlying strings to be safely replaced.
    public ParamHolder(ParamHolder ph)
    {
      names  = new List<string>(ph.names );
      values = new List<string>(ph.values);
    }

    // Load new (name,value) pairs from an assignment-format string. New names are added, existing names
    // have their values replaced.
    public void Load(string paramAssignments)
    {
      string[] pairs = paramAssignments.Split(';');
      foreach (string p in pairs)
        if (p.Trim() != string.Empty)
        {
          string[] pair = p.Split('=');
          for (int i = 0; i < 2; ++i)
            pair[i] = pair[i].Trim();
          int index = IndexOf(pair[0]);
          if (index < 0)
          {
            names .Add(pair[0]);
            values.Add(pair[1]);
          }
          else
            values[index] = pair[1];
        }
    }

    // Remove the (name,value) pair at the indicated index, 0 <= i < Count.
    public void RemoveAt(int i)
    {
      names .RemoveAt(i);
      values.RemoveAt(i);
    }

    // Remove the specified name and its value, do nothing if the name does not exist.
    public void Remove(string name)
    {
      int i = IndexOf(name);
      if (i >= 0)
        RemoveAt(i);
    }

    // Number of pairs.
    public int Count
    {
      get { return names.Count;}
    }

    // Name at specified index, 0 <= index < Count
    public string Name(int index)
    {
      return names[index];
    }

    // Value at specified index, 0 <= index < Count
    public string Value(int index)
    {
      return values[index];
    }

    // Array of all names
    public string[] Names
    {
      get { return names.ToArray();}
    }

    // Array of all values
    public string[] Values
    {
      get { return values.ToArray();}
    }

    // Convert to assignment-format string
    public string Assignments
    {
      get
      {
        string p = string.Empty;
        for (int i = 0; i < names.Count; ++i)
          p += string.Format("{0} = {1};\n", names[i], values[i]);
        return p;
      }
    }

    // Index of specified name, -1 if not found
    public int IndexOf(string name)
    {
      return names.IndexOf(name);
    }

    // Indexer for value at specified index, 0 <= index < Count
    public string this[int index]
    {
      get { return Value(index);}
    }

    // Indexer for value corresponding to specified name, or null if name not found
    public string this[string name]
    {
      get { return names.Contains(name) ? values[names.IndexOf(name)] : null;}
    }

    // Difference between two parameter sets. Specifically, every (name,value) pair that is in ph1
    // and not in ph2, either because the name is not in ph2, or because the values don't match.
    public static ParamHolder operator- (ParamHolder ph1, ParamHolder ph2)
    {
      ParamHolder phDiff = new ParamHolder(ph1);
      if (ph2 != null)
        for (int i = 0; i < ph2.Count; ++i)
        {
          int index = phDiff.IndexOf(ph2.Name(i));
          if (index >= 0 && phDiff[index] == ph2[i])
            phDiff.RemoveAt(index);
        }
      return phDiff;
    }
  }

  // Under construction
  public class ParamDOF
  {
    public void Add(string assignments)
    {

    }

    public int DofCount
    {
      get
      {
        return 0;
      }
    }

  }

}
