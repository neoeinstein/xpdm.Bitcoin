using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Bitcoin
{
    /// <summary>
    /// Defines the network to which a particular message belongs.
    /// </summary>
    public enum MessageNetwork : uint
    {
        Main = 0xD9B4BEF9,
        Test = 0xDAB5BFFA,
    }
}
