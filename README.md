# ClockV2 - Quiet Clock

This repository serves to document, backup and manage an attempt at **Assessment 2** part of the **UG409765 - Software Construction** module of the **BSc Computing** program at the **University of the Highlands and Islands (UHI)**. 

## **Overview**
ClockV2, or as this interpretation is named, Quiet Clock, contains an analouge clock implemented with adherence to the **Model-View-Presenter (MVP)** architecture. Per the requirements it has been expanded to include
adding, editing (deleting), loading & saving alarms using the previous assignment's priorityqueue & adhering to the iCal format when loading & saving.

It then has been expanded beyond those requirements to include displaying a list of the set alarms, scheduling reminders (for iCal programs) & being fully functional while minimised to the taskbar (where it's name was derived from).


## **Repository Structure**

- `ClockView.cs`: The View component of the analogue clock application, responsible for rendering the clock UI.
- `ClockModel.cs`: A Model component of the analogue clock application, responsible for managing the current time.
- `ClockPresenter.cs`: The Presenter component, containing the business logic for updating and controlling the analogue clock.
- `ClockDrawingHelper.cs`: A helper class for drawing the clock face and hands.

- `DialougeAddView.cs`: The View component of the add dialouge, responsible for allowing the user to set alarms.
- `DialougeAdd.cs`: The Presenter component, containing the business logic for adding alarms.
- `DialougeView.cs`: The View component of the view dialouge, responsible for view, remove & export alarms.
- `DialougeViewPresenter.cs`: The Presenter component, containing the business logic for removing & exporting alarms.
- `AlarmTime.cs`: A Model component of the analogue clock application, heavily used throughout by every view & presenter, responsible for storing alarms.
- `ReverseSortedArray.cs`: A modifiction to PriorityQueue's sorted array for better compatibility with ClockV2.

---
