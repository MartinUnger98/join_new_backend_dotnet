using JoinBackendDotnet.Models;

namespace JoinBackendDotnet.Helpers
{
    public static class ColorMapper
    {
        public static string GetHexCode(BgColor color)
        {
            return color switch
            {
                BgColor.Color1 => "#FF7A00",
                BgColor.Color2 => "#462F8A",
                BgColor.Color3 => "#FFBB2B",
                BgColor.Color4 => "#FC71FF",
                BgColor.Color5 => "#6E52FF",
                BgColor.Color6 => "#1FD7C1",
                BgColor.Color7 => "#9327FF",
                BgColor.Color8 => "#FF4646",
                _ => "#FF7A00"
            };
        }
    }
}
