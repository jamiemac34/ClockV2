using ClockV2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ClockV2.Presenter
{
    public class ClockPresenter
    {
        private readonly ClockModel model;
        private readonly ClockView view;
        private readonly Timer timer;

        public ClockPresenter(ClockModel model, ClockView view)
        {
            this.model = model;
            this.view = view;

            // Link the View to this Presenter
            view.SetPresenter(this);

            // Set up the timer for regular updates
            timer = new Timer(1000); // 1-second interval
            timer.Elapsed += (s, e) => UpdateClock();
            timer.Start();
        }

        private void UpdateClock()
        {
            // Fetch the current time from the Model
            DateTime currentTime = model.GetCurrentTime();

            // Update the View with the new time
            view.Invoke(new Action(() => view.UpdateClock(currentTime)));
        }
    }
}
