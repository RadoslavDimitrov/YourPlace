using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using static YourPlace.Models.ModelConstants.ModelConstants.RaitingConstants;

namespace YourPlace.Web.Models.Rate
{
    public class AvalibleRaitingViewModel
    {
        [Range(RaitingRateMinValue, RaitingRateMaxValue)]
        public int Raiting { get; set; }
    }
}
