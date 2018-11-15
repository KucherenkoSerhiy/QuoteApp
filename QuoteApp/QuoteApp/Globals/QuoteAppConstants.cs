﻿using System.Collections.Generic;
using QuoteApp.Backend.Model;

namespace QuoteApp.Globals
{
    public class QuoteAppConstants
    {
        public const string DatabaseFilePath = "QuoteApp/Database.db3";
        public const string PropertiesFilePath = "QuoteApp/Properties";

        #region Style Colors

        public static readonly List<ThemeColor> DefaultDayBackgroundColorGradientItems = new List<ThemeColor>
        {
            new ThemeColor{ ColorCode = "fffefc", GradientPosition = 0 },
            new ThemeColor{ ColorCode = "babfa6", GradientPosition = 0.625f },
            new ThemeColor{ ColorCode = "666666", GradientPosition = 0.9f }
        };
        public const string DefaultDayLineColor = "9a9b95";
        public const string DefaultDayTextColor = "000000";

        public static readonly List<ThemeColor> DefaultNightBackgroundColorGradientItems = new List<ThemeColor>
        {
            new ThemeColor{ ColorCode = "141414", GradientPosition = 0 },
            new ThemeColor{ ColorCode = "353535", GradientPosition = 0.625f },
            new ThemeColor{ ColorCode = "bda2a1", GradientPosition = 0.9f }
        };
        public const string DefaultNightLineColor = "666666";
        public const string DefaultNightTextColor = "ffffff";

        #endregion
    }
}