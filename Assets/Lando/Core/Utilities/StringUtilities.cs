namespace Lando.Core.Utilities
{
    public static class StringUtilities
    {
        public static string Indent(int level) => "".PadLeft(level * 4);
    }
}
