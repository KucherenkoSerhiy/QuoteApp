using System.Collections.Generic;
using QuoteApp.Backend.Model;

namespace QuoteApp.Globals
{
    public class QuoteAppConstants
    {
        public const string DatabaseFilePath = "QuoteApp/Database.db3";
        public const string PropertiesFilePath = "QuoteApp/Properties";

        #region UI Colors

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

        
        public static readonly string[] AutorsToDiscard = {"Adolf Hitler"};
        public static readonly Dictionary<string, string> AutorsToRename =
            new Dictionary<string, string> {{"e.e. cummings", "E.E. Cummings"}};


        public static readonly string[] ThemesToDiscard =
        {
            "Christmas", "Cool", "Dad", "Design", "Food", "Gardening", "Hope", "Mom", "Anniversary", "Diet", "Freedom",
            "Graduation", "Humor", "Marriage", "Medical", "Poetry", "Romantic", "Sad", "Science", "Sports", "War",
            "Wedding", "Architecture", "Art", "Car", "Computers", "Memorialday", "Mothersday", "Movies", "Teen",
            "Thanksgiving", "Valentinesday", "Newyears", "Saintpatricksday", "God"
        };

        public static readonly Dictionary<string, string> ThemesToRename =
            new Dictionary<string, string> {{"Movingon", "Moving On"}};

        public static readonly Dictionary<string, EnEvaluation> ThemeEvaluations =
            new Dictionary<string, EnEvaluation>
            {
                {"Courage", EnEvaluation.Recommended},
                {"Experience", EnEvaluation.Recommended},
                {"Failure", EnEvaluation.Recommended},
                {"Faith", EnEvaluation.Recommended},
                {"Family", EnEvaluation.Recommended},
                {"Forgiveness", EnEvaluation.Recommended},
                {"Funny", EnEvaluation.Recommended},
                {"Happiness", EnEvaluation.Recommended},
                {"Inspirational", EnEvaluation.Recommended},
                {"Knowledge", EnEvaluation.Recommended},
                {"Leadership", EnEvaluation.Recommended},
                {"Love", EnEvaluation.Recommended},
                {"Motivational", EnEvaluation.Recommended},
                {"Moving On", EnEvaluation.Recommended},
                {"Strength", EnEvaluation.Recommended},
                {"Success", EnEvaluation.Recommended},
                {"Time", EnEvaluation.Recommended},
                {"Anger", EnEvaluation.Useful},
                {"Best", EnEvaluation.Useful},
                {"Change", EnEvaluation.Useful},
                {"Dreams", EnEvaluation.Useful},
                {"Learning", EnEvaluation.Useful},
                {"Life", EnEvaluation.Useful},
                {"Patience", EnEvaluation.Useful},
                {"Smile", EnEvaluation.Useful},
                {"Travel", EnEvaluation.Useful},
                {"Truth", EnEvaluation.Useful},
                {"Beauty", EnEvaluation.Useful},
                {"Business", EnEvaluation.Useful},
                {"Easter", EnEvaluation.Useful},
                {"Environmental", EnEvaluation.Useful},
                {"Equality", EnEvaluation.Useful},
                {"Famous", EnEvaluation.Useful},
                {"Fitness", EnEvaluation.Useful},
                {"Future", EnEvaluation.Useful},
                {"Good", EnEvaluation.Useful},
                {"Music", EnEvaluation.Useful},
                {"Nature", EnEvaluation.Useful},
                {"Society", EnEvaluation.Useful},
                {"Thankful", EnEvaluation.Useful},
                {"Trust", EnEvaluation.Useful},
                {"Wisdom", EnEvaluation.Useful},
                {"Age", EnEvaluation.Extension},
                {"Birthday", EnEvaluation.Extension},
                {"Communication", EnEvaluation.Extension},
                {"Education", EnEvaluation.Extension},
                {"Fathersday", EnEvaluation.Extension},
                {"Fear", EnEvaluation.Extension},
                {"Finance", EnEvaluation.Extension},
                {"Government", EnEvaluation.Extension},
                {"Great", EnEvaluation.Extension},
                {"Health", EnEvaluation.Extension},
                {"History", EnEvaluation.Extension},
                {"Imagination", EnEvaluation.Extension},
                {"Intelligence", EnEvaluation.Extension},
                {"Men", EnEvaluation.Extension},
                {"Money", EnEvaluation.Extension},
                {"Morning", EnEvaluation.Extension},
                {"Parenting", EnEvaluation.Extension},
                {"Patriotism", EnEvaluation.Extension},
                {"Women", EnEvaluation.Extension},
                {"Amazing", EnEvaluation.Extension},
                {"Attitude", EnEvaluation.Extension},
                {"Death", EnEvaluation.Extension},
                {"Friendship", EnEvaluation.Extension},
                {"Jealousy", EnEvaluation.Extension},
                {"Legal", EnEvaluation.Extension},
                {"Peace", EnEvaluation.Extension},
                {"Pet", EnEvaluation.Extension},
                {"Positive", EnEvaluation.Extension},
                {"Relationship", EnEvaluation.Extension},
                {"Religion", EnEvaluation.Extension},
                {"Respect", EnEvaluation.Extension},
                {"Sympathy", EnEvaluation.Extension},
                {"Alone", EnEvaluation.Extension},
                {"Home", EnEvaluation.Extension},
                {"Politics", EnEvaluation.Extension},
                {"Power", EnEvaluation.Extension},
                {"Teacher", EnEvaluation.Extension},
                {"Work", EnEvaluation.Extension},
                {"Dating", EnEvaluation.Extension},
                {"Technology", EnEvaluation.Extension}
            };

    }
}