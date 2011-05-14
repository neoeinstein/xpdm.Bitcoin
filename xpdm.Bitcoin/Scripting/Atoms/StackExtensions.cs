using System;
using System.Diagnostics.Contracts;
using C5;

namespace xpdm.Bitcoin.Scripting.Atoms
{
    static class StackExtensions
    {
        public static T Peek<T>(this IStack<T> stack)
        {
            Contract.Requires<ArgumentNullException>(stack != null);

            return stack.Peek(0);
        }

        public static T Peek<T>(this IStack<T> stack, int depth)
        {
            Contract.Requires<ArgumentNullException>(stack != null);
            Contract.Requires<ArgumentOutOfRangeException>(depth >= 0);

            return stack[stack.Count - depth - 1];
        }
    }
}
