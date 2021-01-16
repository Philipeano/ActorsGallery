using System.Collections.Generic;

namespace ActorsGallery.Core.LookUp
{

    public static class GenderOptions
    {

        private static readonly List<string> options = new List<string> { "Male", "Female" };

        public static List<string> Values => options;
    } 
}
