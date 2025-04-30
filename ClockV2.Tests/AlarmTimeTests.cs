using NUnit.Framework;
using ClockV2.Alarm;
using System;

namespace ClockV2.Tests
{
    [TestFixture]
    public class AlarmTimeTests
    {
        private AlarmTime _alarmTime;
        private string _displayTime;
        private DateTime _date;
        private TimeSpan _triggerTime;
        private string _name;
        private string _description;
        private string _uid;
        private DateTime _setDate;

        [SetUp]
        public void SetUp()
        {
            _displayTime = "10:00 AM";
            _date = new DateTime(2025, 1, 1);
            _triggerTime = TimeSpan.FromMinutes(-5);
            _name = "Test Alarm";
            _description = "This is a test alarm";
            _uid = Guid.NewGuid().ToString();
            _setDate = DateTime.Now;

            _alarmTime = new AlarmTime(_displayTime, _date, _triggerTime, _name, _description, _uid, _setDate);
        }

        // Test AlarmTime constructor and properties
        [Test]
        public void Constructor_SetsPropertiesCorrectly()
        {
            Assert.That(_alarmTime.GetDT(), Is.EqualTo(_displayTime));
            Assert.That(_alarmTime.GetDate(), Is.EqualTo(_date));
            Assert.That(_alarmTime.GetName(), Is.EqualTo(_name));
            Assert.That(_alarmTime.GetDescription(), Is.EqualTo(_description));
        }

        // Test AlarmTime returns the correct string representation
        [Test]
        public void ToString_ReturnsFormattedString()
        {
            Assert.That(_alarmTime.ToString(), Is.EqualTo($"{_name}: {_displayTime}"));
        }

        // Test AlarmTime returns the correct calendar event string
        [Test]
        public void ToCalanderEvent_ReturnsICalString()
        {
            string icalString = _alarmTime.ToCalanderEvent();
            Assert.That(icalString, Does.Contain($"UID:{_uid}"));
            Assert.That(icalString, Does.Contain($"DTSTART:{_date.ToUniversalTime():yyyyMMddTHHmmssZ}"));
            Assert.That(icalString, Does.Contain($"TRIGGER:-5M"));
        }

        // Test AlarmTime's capabilty to compare times (True)
        [Test]
        public void Equals_SameValues_ReturnsTrue()
        {
            var sameAlarmTime = new AlarmTime(_displayTime, _date, _triggerTime, _name, _description, _uid, _setDate);
            Assert.That(_alarmTime.Equals(sameAlarmTime), Is.True);
        }

        // Test AlarmTime's capabilty to compare times (False)
        [Test]
        public void Equals_DifferentValues_ReturnsFalse()
        {
            var differentAlarmTime = new AlarmTime("11:00 AM", _date, _triggerTime, _name, _description, _uid, _setDate);
            Assert.That(_alarmTime.Equals(differentAlarmTime), Is.False);
        }
    }
}