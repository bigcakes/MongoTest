using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MongoTestProgram.Extensions
{
    public static class ControlExtensions
    {
        public static void ScrollToBottom(this ListBox listbox)
        {
            if (listbox == null) throw new ArgumentNullException("listbox", "Argument listbox cannot be null");
            if (!listbox.IsInitialized) throw new InvalidOperationException("ListBox is in an invalid state: IsInitialized == false");

            if (listbox.Items.Count == 0)
                return;

            listbox.ScrollIntoView(listbox.Items[listbox.Items.Count - 1]);
        }
    }
}
