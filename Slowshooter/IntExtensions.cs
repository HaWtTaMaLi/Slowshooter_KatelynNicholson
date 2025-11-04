using System;

namespace Slowshooter
{
    static class IntExtensions
    {
        public static int Clamp(this int value, int min, int max)
        {
            return Math.Max(min, Math.Min(value, max));
        }
    }
}
