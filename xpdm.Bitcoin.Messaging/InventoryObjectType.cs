
namespace xpdm.Bitcoin.Messaging
{
    /// <summary>
    /// Identifies the type of object refered to in an inventory vector.
    /// </summary>
    public enum InventoryObjectType : uint
    {
        /// <summary>
        /// Inventory objects with this value may be ignored.
        /// </summary>
        Error = 0,
        /// <summary>
        /// Inventory object related to a transaction.
        /// </summary>
        Msg_Tx = 1,
        /// <summary>
        /// Inventory object related to a data block.
        /// </summary>
        Msg_Block = 2,
    }
}
