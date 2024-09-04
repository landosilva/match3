namespace Match3.Generated
{
    public static class Layer
    {
        public static readonly int Default = 0;
        public static readonly int TransparentFX = 1;
        public static readonly int IgnoreRaycast = 2;
        public static readonly int Water = 4;
        public static readonly int UI = 5;
        
        public static class Mask
        {
            public static readonly int Default = 1 << 0;
            public static readonly int TransparentFX = 1 << 1;
            public static readonly int IgnoreRaycast = 1 << 2;
            public static readonly int Water = 1 << 4;
            public static readonly int UI = 1 << 5;
        }

    }
}
