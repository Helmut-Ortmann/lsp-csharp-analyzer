using System;

namespace LspAnalyzer.Services
{
    /// <summary>
    /// Features for time measurements like time difference.
    /// </summary>
    public class TimeMeasurement
    {
        private readonly DateTime _startTime = DateTime.Now;
        private TimeSpan _span;
        public TimeMeasurement()
        {
            _span = System.TimeSpan.MinValue;
        }

        /// <summary>
        /// TimeSpan as <see cref="TimeSpan"/>
        /// </summary>
        public TimeSpan Span
        {
            get => _span;
        }

        /// <summary>
        /// Stops the timer and returns the time difference in human readable form.
        /// </summary>
        /// <returns>"{Span.Seconds}:{Span.Milliseconds} (mm:ss:ms)"</returns>
        public string TimeSpanAsString()
        {

            _span = DateTime.Now.Subtract(_startTime);
            return  $"{Span.Minutes}:{Span.Seconds}:{Span.Milliseconds} (mm:ss:ms)";
        }
        /// <summary>
        /// Stops the timer and returns the time difference.
        /// </summary>
        /// <returns></returns>
        public TimeSpan TimeSpanStop()
        {

            _span = DateTime.Now.Subtract(_startTime);
            return _span;
        }
        
        
    }
}
