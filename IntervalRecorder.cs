using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// +------------------------------------------------------------------------------------------------------------------------------+
/// ¦                                                   TERMS OF USE: MIT License                                                  ¦
/// +------------------------------------------------------------------------------------------------------------------------------¦
/// ¦Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation    ¦
/// ¦files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy,    ¦
/// ¦modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software¦
/// ¦is furnished to do so, subject to the following conditions:                                                                   ¦
/// ¦                                                                                                                              ¦
/// ¦The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.¦
/// ¦                                                                                                                              ¦
/// ¦THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE          ¦
/// ¦WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR         ¦
/// ¦COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,   ¦
/// ¦ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                         ¦
/// +------------------------------------------------------------------------------------------------------------------------------+

namespace Kimono
{
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
    /// <summary>
    /// The records written to the database can be marked if the are the first one
    /// seen in a specified interval. This makes it easy to pick up just the 
    /// records you want for graphing or analysis purposes
    /// 
    /// We cannot rely on just seen a 0 in the minute (for example) we might miss
    /// it. So we look for the first record after the minute field rolls over 
    /// and mark that.
    /// </summary>
    public class IntervalRecorder
    {
        private bool fiveMinuteMarkerFlag = false;
        private DateTime lastFiveMinuteMarkerDateTime = DateTime.MinValue;

        private bool tenMinuteMarkerFlag = false;
        private DateTime lastTenMinuteMarkerDateTime = DateTime.MinValue;
        private bool fifteenMinuteMarkerFlag = false;
        private DateTime lastFifteenMinuteMarkerDateTime = DateTime.MinValue;
        private bool thirtyMinuteMarkerFlag = false;
        private DateTime lastThirtyMinuteMarkerDateTime = DateTime.MinValue;
        private bool hourMarkerFlag = false;
        private DateTime lastHourMarkerDateTime = DateTime.MinValue;
        private bool dayMarkerFlag = false;
        private DateTime lastDayMarkerDateTime = DateTime.MinValue;

        /// +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
        /// <summary>
        /// Updates the marker flags based on the specified time
        /// 
        /// </summary>
        /// <param name="datetimeForMark">the datetimes we are setting the mark for</param>
        public void UpdateMarkerFlags (DateTime datetimeForMark)
        {

            // do the Five minutes. 
            if ((datetimeForMark - lastFiveMinuteMarkerDateTime).TotalMinutes >= 5)
            {
                FiveMinuteMarkerFlag = true;
                lastFiveMinuteMarkerDateTime = datetimeForMark;
            }
            else FiveMinuteMarkerFlag = false;

            // do the Ten minutes. 
            if ((datetimeForMark - lastTenMinuteMarkerDateTime).TotalMinutes >= 10)
            {
                TenMinuteMarkerFlag = true;
                lastTenMinuteMarkerDateTime = datetimeForMark;
            }
            else TenMinuteMarkerFlag = false;

            // do the Fifteen minutes. 
            if ((datetimeForMark - lastFifteenMinuteMarkerDateTime).TotalMinutes >= 15)
            {
                FifteenMinuteMarkerFlag = true;
                lastFifteenMinuteMarkerDateTime = datetimeForMark;
            }
            else FifteenMinuteMarkerFlag = false;

            // do the Thirty minutes. 
            if ((datetimeForMark - lastThirtyMinuteMarkerDateTime).TotalMinutes >= 30)
            {
                ThirtyMinuteMarkerFlag = true;
                lastThirtyMinuteMarkerDateTime = datetimeForMark;
            }
            else ThirtyMinuteMarkerFlag = false;

            // do the Hour marker. These are different, we want them to be as close to  
            // the top of the hour as we can. This means the call here must be more frequent
            // than once every 10 minutes or we will not mark it properly
            if (((datetimeForMark - lastHourMarkerDateTime).TotalMinutes >= 59) && 
                (datetimeForMark.TimeOfDay.Minutes >= 0) && (datetimeForMark.TimeOfDay.Minutes <= 10))
            {
                HourMarkerFlag = true;
                lastHourMarkerDateTime = datetimeForMark;
            }
            else HourMarkerFlag = false;

            // do the Day marker. These are different, we want them to be as close to  
            // the 00:00 start of the day as we can. This means the call here must be more frequent
            // than once every hour or we will not mark it properly
            if (((datetimeForMark - lastDayMarkerDateTime).TotalHours >= 23) &&
                (datetimeForMark.TimeOfDay.Hours == 0))
            {
                DayMarkerFlag = true;
                lastDayMarkerDateTime = datetimeForMark;
            }
            else DayMarkerFlag = false;

        }

        public bool FiveMinuteMarkerFlag { get => fiveMinuteMarkerFlag; set => fiveMinuteMarkerFlag = value; }
        public bool TenMinuteMarkerFlag { get => tenMinuteMarkerFlag; set => tenMinuteMarkerFlag = value; }
        public bool FifteenMinuteMarkerFlag { get => fifteenMinuteMarkerFlag; set => fifteenMinuteMarkerFlag = value; }
        public bool ThirtyMinuteMarkerFlag { get => thirtyMinuteMarkerFlag; set => thirtyMinuteMarkerFlag = value; }
        public bool HourMarkerFlag { get => hourMarkerFlag; set => hourMarkerFlag = value; }
        public bool DayMarkerFlag { get => dayMarkerFlag; set => dayMarkerFlag = value; }
    }
}
