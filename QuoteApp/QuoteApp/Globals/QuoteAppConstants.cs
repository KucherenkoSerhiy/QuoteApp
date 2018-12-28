using System.Collections.Generic;
using QuoteApp.Backend.Model;

namespace QuoteApp.Globals
{
    public class QuoteAppConstants
    {
        public const string DatabaseFilePath = "QuoteApp/Database.db3";
        public const string PropertiesFilePath = "QuoteApp/Properties";

        #region Style Colors

        public const string OnSwitchColor = "009956";

        // Day mode
        public static readonly List<ThemeColor> DefaultDayBackgroundColorGradientItems = new List<ThemeColor>
        {
            new ThemeColor{ ColorCode = "fffefc", GradientPosition = 0 },
            new ThemeColor{ ColorCode = "babfa6", GradientPosition = 0.625f },
            new ThemeColor{ ColorCode = "666666", GradientPosition = 0.9f }
        };
        public const string DefaultDayLineColor = "9a9b95";
        public const string DefaultDayTextColor = "000000";
        public const string DefaultDayGrayedOutTextColor = "5e5e5e";
        public const string DefaultDayListItemSelectionBackgroundColor = "ebe8e8";
        public const string DefaultDayButtonBackgroundStartColor = "fffefc";
        public const string DefaultDayButtonBackgroundEndColor = "666666";

        // Night mode
        public static readonly List<ThemeColor> DefaultNightBackgroundColorGradientItems = new List<ThemeColor>
        {
            new ThemeColor{ ColorCode = "141414", GradientPosition = 0 },
            new ThemeColor{ ColorCode = "353535", GradientPosition = 0.625f },
            new ThemeColor{ ColorCode = "bda2a1", GradientPosition = 0.9f }
        };
        public const string DefaultNightLineColor = "666666";
        public const string DefaultNightTextColor = "ffffff";
        public const string DefaultNightGrayedOutTextColor = "a1a1a1";
        public const string DefaultNightListItemSelectionBackgroundColor = "211d1d";
        public const string DefaultNightButtonBackgroundStartColor = "141414";
        public const string DefaultNightButtonBackgroundEndColor = "7fbca2a1";

        #endregion
    }
}