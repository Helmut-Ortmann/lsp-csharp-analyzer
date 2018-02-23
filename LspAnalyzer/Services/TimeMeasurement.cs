using System;

namespace LspAnalyzer.Services
{
    public class TimeMeasurement
    {
        private readonly DateTime _startTime = DateTime.Now;
        private TimeSpan _timeSpan;
        public TimeMeasurement()
        {
            _timeSpan = System.TimeSpan.MinValue;
        }
        /// <summary>
        /// Returns the time difference in human readable form.
        /// </summary>
        /// <returns></returns>
        public string TimeSpan()
        {

            _timeSpan = DateTime.Now.Subtract(_startTime);
            return  $"{_timeSpan.Seconds}:{_timeSpan.Milliseconds} (ss:ms)";
        }
        
        
    }
}
