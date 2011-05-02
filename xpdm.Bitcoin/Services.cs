using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xpdm.Bitcoin
{
    /// <summary>
    /// Bitfield of advertised services
    /// </summary>
    [Flags]
    public enum Services : ulong
    {
        None = 0,
        /// <summary>
        /// Indicates that a node can be asked for full blocks
        /// instead of just headers.
        /// </summary>
        Node_Network = 1,
    }
}
