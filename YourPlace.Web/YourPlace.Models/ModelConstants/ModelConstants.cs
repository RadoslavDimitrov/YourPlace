

namespace YourPlace.Models.ModelConstants
{
    public static class ModelConstants
    {
        //Store constants
        public const int StoreNameMinLenght = 3;
        public const int StoreNameMaxLenght = 40;
        public const int StoreMinOpenHour = 0;
        public const int StoreMaxOpenHour = 24;
        public const int StoreDescriptionMinLenght = 10;



        //StoreService constants
        public const int StoreServiceNameMaxLenght = 50;
        public const int StoreServiceDescriptionMaxLenght = 50;
        public const double StoreServicePriceMin = 0.01;
        public const double StoreServicePriceMax = double.MaxValue;


        //Comment constants
        public const int CommentDescriptionMaxLenght = 250;


        //Raiting constants
        public const int RaitingRateMaxValue = 10;
        public const int RaitingRateMinValue = 1;


        //District constants
        public const int DistrictNameMaxLenght = 30;

    }
}
