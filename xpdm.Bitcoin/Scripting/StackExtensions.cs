using System.Diagnostics.Contracts;
using C5;

namespace xpdm.Bitcoin.Scripting
{
    static class StackExtensions
    {
        [Pure]
        public static T Peek<T>(this IStack<T> stack)
        {
            ContractsCommon.NotNull(stack, "stack");

            return stack.Peek(0);
        }

        [Pure]
        public static T Peek<T>(this IStack<T> stack, int depth)
        {
            ContractsCommon.NotNull(stack, "stack");
            ContractsCommon.ValidOffset(0, stack.Count, depth, "depth");

            return stack[stack.Count - depth - 1];
        }
    }
}
