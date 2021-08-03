

namespace YourPlace.Models.ModelConstants
{
    public class ModelConstants
    {
        public static class StoreConstants
        {
            public const int StoreNameMinLenght = 3;
            public const int StoreNameMaxLenght = 40;
            public const int StoreMinOpenHour = 0;
            public const int StoreMaxOpenHour = 24;
            public const int StoreDescriptionMinLenght = 10;
        }

        public static class StoreServiceConstants
        {
            public const int StoreServiceNameMaxLenght = 50;
            public const int StoreServiceDescriptionMaxLenght = 50;
            public const double StoreServicePriceMin = 0.01;
            public const double StoreServicePriceMax = double.MaxValue;
        }
        
        public static class CommentConstants
        {
            public const int CommentDescriptionMinLenght = 5;
            public const int CommentDescriptionMaxLenght = 250;
        }

        public static class RaitingConstants
        {
            public const int RaitingRateMaxValue = 10;
            public const int RaitingRateMinValue = 1;
        }

        public static class DistrictConstants
        {
            public const int DistrictNameMaxLenght = 30;
        }

        public static class TownConstants 
        {
            public const int TownNameMaxLenght = 25;
        }

        public static class UserConstants
        {
            public const int PasswordMinLenght = 6;
            public const int PasswordMaxLenght = 100;
        }
    }
}
