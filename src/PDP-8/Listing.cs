using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDP_8
{
  public partial class Listing : Form
  {
    public Listing()
    {
      InitializeComponent();
      Clear();
    }

    Dictionary<int, int> addressMap = null;

    int lastAddress = -1;

    void Clear()
    {
      listingText.Text = string.Empty;
      addressMap = null;
      lastAddress = -1;
    }

    public void Load(string listing, Dictionary<int, int> addressMap)
    {
      listingText.Text = listing.Replace("\r\n", "\n").Replace("\n", "\r\n");
      this.addressMap = addressMap;
      lastAddress = -1;
    }

    public void ShowAddress(int address)
    {
      if (address == lastAddress)
        return;

      lastAddress = address;

      if (addressMap != null && addressMap.ContainsKey(address))
      {
        int index = addressMap[address];
        if (index < listingText.TextLength)
        {
          int endLineIndex = listingText.Text.IndexOf('\n', index);
          if (endLineIndex < 0)
            endLineIndex = listingText.TextLength;
          listingText.Select(index, endLineIndex - index);
          listingText.ScrollToCaret();
        }
      }
    }

    private void Listing_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (e.CloseReason == CloseReason.UserClosing)
      {
        e.Cancel = true;
        Hide();
      }
    }
  }
}
