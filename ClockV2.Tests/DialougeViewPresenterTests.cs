using NUnit.Framework;
using ClockV2.Presenter;
using ClockV2.Alarm;
using ClockV2.View;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ClockV2.Tests
{
    [TestFixture]
    public class DialougeViewPresenterTests
    {
        private DialougeView _view;
        private ReverseSortedArray<AlarmTime> _alarmQueue;
        private Action _cancelAlarmCallback;
        private Action _updateAlarmDisplayCallback;
        private Action _onSaveAlarmsCallback;
        private DialougeViewPresenter _presenter;

        [SetUp]
        public void SetUp()
        {
            _alarmQueue = new ReverseSortedArray<AlarmTime>(10);
            _cancelAlarmCallback = () => { };
            _updateAlarmDisplayCallback = () => { };
            _onSaveAlarmsCallback = () => { };
            _view = new DialougeView(_alarmQueue, _cancelAlarmCallback, _updateAlarmDisplayCallback, _onSaveAlarmsCallback);
            _presenter = new DialougeViewPresenter(_view, _alarmQueue, _cancelAlarmCallback, _updateAlarmDisplayCallback, _onSaveAlarmsCallback);
        }

        [TearDown]
        public void TearDown()
        {
            _presenter = null;
            _view.Dispose();
        }

        // Testing the ViewPresenter can remove elements from the queue

        [Test]
        public void OnRemoveAlarm_RemovesAlarmFromQueue()
        {
            // Arrange
            var alarm1 = new AlarmTime("1:00 PM", DateTime.Now.AddHours(1), TimeSpan.Zero, "Alarm 1", "Desc", "uid", DateTime.Now);
            var alarm2 = new AlarmTime("2:00 PM", DateTime.Now.AddHours(2), TimeSpan.Zero, "Alarm 2", "Desc", "uid2", DateTime.Now);
            _alarmQueue.Add(alarm1, 1);
            _alarmQueue.Add(alarm2, 2);
            _view = new DialougeView(_alarmQueue, _cancelAlarmCallback, _updateAlarmDisplayCallback, _onSaveAlarmsCallback);
            _presenter = new DialougeViewPresenter(_view, _alarmQueue, _cancelAlarmCallback, _updateAlarmDisplayCallback, _onSaveAlarmsCallback);

            // Act
            _presenter.OnRemoveAlarm(0);

            // Assert
            Assert.That(_alarmQueue.GetLength(), Is.EqualTo(0));
            Assert.That(_alarmQueue.Contains(alarm2), Is.True);
            Assert.That(_alarmQueue.Contains(alarm1), Is.False);
        }


        // Testing the ViewPresenter invokes the callback when an alarm is removed

        [Test]
        public void OnRemoveAlarm_CallsUpdateAlarmDisplayCallback()
        {
            // Arrange
            _alarmQueue.Add(new AlarmTime("1:00 PM", DateTime.Now.AddHours(1), TimeSpan.Zero, "Alarm 1", "Desc", "uid", DateTime.Now), 1);
            bool callbackInvoked = false;
            _updateAlarmDisplayCallback = () => { callbackInvoked = true; };
            _view = new DialougeView(_alarmQueue, _cancelAlarmCallback, _updateAlarmDisplayCallback, _onSaveAlarmsCallback);
            _presenter = new DialougeViewPresenter(_view, _alarmQueue, _cancelAlarmCallback, _updateAlarmDisplayCallback, _onSaveAlarmsCallback);

            // Act
            _presenter.OnRemoveAlarm(0);

            // Assert
            Assert.That(callbackInvoked, Is.True);
        }

    }
}