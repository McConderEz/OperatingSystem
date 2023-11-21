using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatingSystem.Model
{
    public class MFT_Entry
    {
        public string Header { get; private set; }
        public Attribute Attributes { get; private set; }

        public readonly string ID = Guid.NewGuid().ToString();
    }
}
