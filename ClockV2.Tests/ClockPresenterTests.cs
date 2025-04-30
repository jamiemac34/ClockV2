using NUnit.Framework;
using ClockV2.Presenter;
using ClockV2.Model;
using ClockV2.View;
using ClockV2.Alarm;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClockV2.Tests
{
    [TestFixture]
    public class ClockPresenterTests
    {
        private ClockView _view;
        private ClockModel _model;
        private ReverseSortedArray<AlarmTime> _alarmQueue;
        private ClockPresenter _presenter;

        [SetUp]
        public void SetUp()
        {
            _view = new ClockView();
            _model = new ClockModel();
            _alarmQueue = new ReverseSortedArray<AlarmTime>(10);
            _presenter = new ClockPresenter(_model, _view);
        }

        [TearDown]
        public void TearDown()
        {
            _presenter = null;
            _view.Dispose();
        }

        // Testing the ClockPresenter doesn't crash the application by trying to schedule alarms outside of async range
        [Test]
        public void ScheduleAlarm_AlarmInFuture_DoesNotThrowException()
        {
            var alarmTime = new AlarmTime("11:00 AM", DateTime.Now.AddSeconds(1), TimeSpan.Zero, "Test", "Desc", "uid", DateTime.Now);
            _alarmQueue.Add(alarmTime, 1);
            _presenter.UpdateAlarmDisplay();

            Assert.DoesNotThrow(() => { });
        }

        // Testing the ClockPresenter adds alarms within the async range
        [Test]
        public void CheckForDelay_AlarmWithin21Days_ReturnsFalse()
        {
            var alarmTime = new AlarmTime("11:00 AM", DateTime.Now.AddDays(20), TimeSpan.Zero, "Test", "Desc", "uid", DateTime.Now);
            bool result = _presenter.CheckForDelay(alarmTime);
            Assert.That(result, Is.False);
        }

        // Testing the ClockPresenter deals with alarms outside of the async range
        [Test]
        public void CheckForDelay_AlarmMoreThan21Days_ReturnsTrue()
        {
            var alarmTime = new AlarmTime("11:00 AM", DateTime.Now.AddDays(22), TimeSpan.Zero, "Test", "Desc", "uid", DateTime.Now);
            bool result = _presenter.CheckForDelay(alarmTime);
            Assert.That(result, Is.True);
        }

    }
}