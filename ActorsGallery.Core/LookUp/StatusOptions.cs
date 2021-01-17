using System.Collections.Generic;

namespace ActorsGallery.Core.LookUp
{
    public static class StatusOptions
    {

        private static readonly List<string> options = new List<string> { "Active", "Dead", "Unknown" };

        public static List<string> Values => options;
    }
}
