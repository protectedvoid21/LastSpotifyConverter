using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.LastFm; 

public static class PeriodMap {
    public static string GetName(TrackPeriod trackPeriod) => trackPeriod switch {
        TrackPeriod.Overall => "overall",
        TrackPeriod.Week => "7day",
        TrackPeriod.OneMonth => "1month",
        TrackPeriod.ThreeMonths => "3month",
        TrackPeriod.HalfYear => "6month",
        TrackPeriod.Year => "12month",
        _ => "overall",
    };
}
