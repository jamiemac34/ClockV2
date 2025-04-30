using System;
using NUnit.Framework;
using ClockV2.Presenter;
using ClockV2.View;
using ClockV2.Alarm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;

namespace ClockV2.Tests
{
    [TestFixture]
    public class DialougeAddPresenterTests
    {
        private DialougeAddPresenter _presenter;
        private DialougeAdd _view;
        private ReverseSortedArray<AlarmTime> _alarmQueue;

        [SetUp]
        public void SetUp()
        {
            _alarmQueue = new ReverseSortedArray<AlarmTime>(10);
            _view = new DialougeAdd(_alarmQueue);
            _presenter = new DialougeAddPresenter(_view, _alarmQueue);
        }


        // Testing the AddPresenter can add elements to the queue
        [Test]
        public void OnBtnFormAddClick_ValidInput_AddsAlarmToQueueAndClosesView()
        {

            var selectedDT = DateTime.Now.AddMinutes(30);
            var displayTime = selectedDT.ToString("yy/MM/dd @ HH:mm:ss");
            var triggerIdx = 1; // 5 minutes before
            var name = "Morning Alarm";
            var desc = "Wake up for work";

            _presenter.OnBtnFormAddClick(displayTime, selectedDT, triggerIdx, name, desc);

            NUnit.Framework.Assert.That(_alarmQueue.GetLength(), Is.EqualTo(0), "Queue should have one alarm");

            var alarm = _alarmQueue.GetEntry(0).Item;
            NUnit.Framework.Assert.That(alarm.GetName(), Is.EqualTo(name));
            NUnit.Framework.Assert.That(alarm.GetDescription(), Is.EqualTo(desc));
            NUnit.Framework.Assert.That(alarm.GetDate(), Is.EqualTo(selectedDT));

            var ical = alarm.ToCalanderEvent();
            NUnit.Framework.Assert.That(ical, Does.Contain("TRIGGER:-5M"));

            NUnit.Framework.Assert.That(_view.Visible, Is.False, "The add-dialog should close on success");
        }

        // Testing the AddPresenter cannot add untriggerable alarms to the queue
        [Test]
        public void OnBtnFormAddClick_PastDate_DoesNotAddAlarm()
        {
            var displayTime = "2025/12/31 @ 23:59:59";
            var selectedDT = DateTime.Now.AddMinutes(-30);
            var triggerIdx = 1;
            var name = "Past Alarm";
            var desc = "This alarm should not be added";

            _presenter.OnBtnFormAddClick(displayTime, selectedDT, triggerIdx, name, desc);

            NUnit.Framework.Assert.That(_alarmQueue.GetLength(), Is.EqualTo(-1));
        }

        // Testing the AddPresenter cannot add duplicate alarms to the queue
        [Test]
        public void OnBtnFormAddClick_DuplicateAlarm_ShowsWarningAndDoesNotAdd()
        {
            var displayTime = "2025/04/25 @ 08:00:00";
            var selectedDT = DateTime.Now.AddMinutes(30);
            var triggerIdx = 1;
            var name = "Existing Alarm";
            var desc = "Description";
            _presenter.OnBtnFormAddClick(displayTime, selectedDT, triggerIdx, name, desc);

            _presenter.OnBtnFormAddClick(displayTime, selectedDT, triggerIdx, name, desc);

            NUnit.Framework.Assert.That(_alarmQueue.GetLength(), Is.EqualTo(0));
        }

        // Testing the AddPresenter warns when the name or description is empty, and does not add the alarm (Name)
        [Test]
        public void OnBtnFormAddClick_EmptyName_ShowsWarningAndDoesNotAdd()
        {

            var displayTime = "2025/04/25 @ 08:00:00";
            var selectedDT = DateTime.Now.AddMinutes(30);
            var triggerIdx = 1;
            var name = "";
            var desc = "Desc";

            _presenter.OnBtnFormAddClick(displayTime, selectedDT, triggerIdx, name, desc);

            NUnit.Framework.Assert.That(_alarmQueue.GetLength(), Is.EqualTo(0));
        }

        // Testing the AddPresenter warns when the name or description is empty, and does not add the alarm (Description)
        [Test]
        public void OnBtnFormAddClick_EmptyDescription_ShowsWarningAndDoesNotAdd()
        {
            var displayTime = "2025/04/25 @ 08:00:00";
            var selectedDT = DateTime.Now.AddMinutes(30);
            var triggerIdx = 1;
            var name = "Name";
            var desc = "";

            _presenter.OnBtnFormAddClick(displayTime, selectedDT, triggerIdx, name, desc);

            NUnit.Framework.Assert.That(_alarmQueue.GetLength(), Is.EqualTo(0));
        }
    }
}