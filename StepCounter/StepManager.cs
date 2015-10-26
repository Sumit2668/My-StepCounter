using System;
using CoreMotion;
using Foundation;

namespace StepCounter
{
    public class StepManager
    {
        public delegate void DailyStepCountChangedEventHandler(int stepCount);

        private NSOperationQueue _queue;
        private DateTime _resetTime;
        private CMStepCounter _stepCounter;

        public StepManager()
        {
            ForceUpdate();
            _stepCounter.StartStepCountingUpdates(_queue, (nint)1, Updater);
        }

        public void ForceUpdate()
        {
            //If the last reset date wasn't today then we should update this.
            if (_resetTime.Date.Day != DateTime.Now.Date.Day)
            {
                _resetTime = DateTime.Today; //Forces update as the day may have changed.
            }

            var sMidnight = DateTime.SpecifyKind(_resetTime, DateTimeKind.Utc);

            if (_queue == null)
                _queue = NSOperationQueue.CurrentQueue;
            if (_stepCounter == null)
                _stepCounter = new CMStepCounter();

            _stepCounter.QueryStepCount(ToNSDate(sMidnight), NSDate.Now, _queue, DailyStepQueryHandler);
        }

        public static NSDate ToNSDate(DateTime date)
        {
            if (date.Kind == DateTimeKind.Unspecified)
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);

            return (NSDate) date;
        }

        public void StartCountingFrom(DateTime date)
        {
            _resetTime = date;
            ForceUpdate();
        }

        private void DailyStepQueryHandler(nint stepCount, NSError error)
        {
            DailyStepCountChanged((int)stepCount);
        }

        private void Updater(nint stepCount, NSDate date, NSError error)
        {
            var sMidnight = DateTime.SpecifyKind(_resetTime, DateTimeKind.Utc);
            _stepCounter.QueryStepCount(ToNSDate(sMidnight), NSDate.Now, _queue, DailyStepQueryHandler);
        }

        public event DailyStepCountChangedEventHandler DailyStepCountChanged;
    }
}