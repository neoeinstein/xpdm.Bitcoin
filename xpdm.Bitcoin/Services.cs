using System;

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
