$(document).ready(function () {

    $.fn.quicktoggle = function () {
        this.each(function () {
            var $this = $(this);
            if ($this.is(':visible')) {
                $this.hide();
            } else {
                $this.show();
            }
        });
        return this;
    };

    var selectDay = function (day) {

        $('div.day').removeClass('selected');
        day.addClass('selected');

        var dayId = day.data('day');

        $('div.expenditure').removeClass('selected').hide();
        $('div.expenditure.' + dayId).addClass('selected');//.show();

        if (day.find('input[type=checkbox]').is(':checked')) {
            $('div.expenditure.' + dayId).show();
        }

        $('.save-day').show();
    }

    $('div.day h2').on('click', function () {
        var day = $(this).parents('div.day');

        day.find('input[type=checkbox]').prop('checked', 'checked');
        selectDay(day);
    });

    $('div.day input[type=checkbox]').on('click', function (e) {

        selectDay($(this).parents('div.day'));

        if ($(this).is(':checked')) {
            $('div.expenditure.selected').show();
        }
        else if (!$(this).is(':checked')) {
            $('div.expenditure.selected').hide();
        }
    });

    $('input.have_receipts').click(function (e) {

        if ($(this).is(':checked')) {
            $('div.subsistence_amount').show();
        }
        else if (!$(this).is(':checked')) {
            $('div.subsistence_amount').hide();
        }
    });

    $('input.no_receipts').click(function (e) {

        if ($(this).is(':checked')) {
            $('div.no_receipts_notification').show();
        }
        else if (!$(this).is(':checked')) {
            $('div.no_receipts_notification').hide();
        }
    });

    var travelType = function (val, travel) {
        var $val = val;
        var $travel = travel;

        //hide all in day
        $('div.car.' + $travel).hide();
        $('div.public_transport.' + $travel).hide();
        $('div.bicycle.' + $travel).hide();
        //show selected
        $('.' + $val + '.' + $travel).quicktoggle();
    }

    $('div.day select').on('change', function () {
        travelType(getTravelModeClass($(this).val()), $(this).attr('class'));
    });

    $('div.day select').each(function (index) {
        travelType(getTravelModeClass($(this).val()), $(this).attr('class'));
    });

    selectDay($('div.day:first'));

    setDateTimeProperties($('#DepartHomeTimeMonday'), $('#ArriveHomeTimeMonday'));
    setDateTimeProperties($('#DepartHomeTimeTuesday'), $('#ArriveHomeTimeTuesday'));
    setDateTimeProperties($('#DepartHomeTimeWednesday'), $('#ArriveHomeTimeWednesday'));
    setDateTimeProperties($('#DepartHomeTimeThursday'), $('#ArriveHomeTimeThursday'));
    setDateTimeProperties($('#DepartHomeTimeFriday'), $('#ArriveHomeTimeFriday'));
    setDateTimeProperties($('#DepartHomeTimeSaturday'), $('#ArriveHomeTimeSaturday'));
    setDateTimeProperties($('#DepartHomeTimeSunday'), $('#ArriveHomeTimeSunday'));

    function getTravelModeClass(optionValue) {
        /// <summary>
        /// Gets class for travel mode/type from numeric option value.
        /// </summary>
        /// <param name="option" type="Number">The option value of the select element.</param>

        var classTexts = new Array();
        classTexts[1] = 'None';
        classTexts[2] = 'car';
        classTexts[3] = 'bicycle';
        classTexts[4] = 'public_transport';
        classTexts[5] = 'taxi';

        return classTexts[optionValue];
    }

    function initialiseDepartAndArriveHomeTime($idSelector) {
        /// <summary>
        /// Initialises depart or arrive time properties, e.g., readonly format.
        /// </summary>
        /// <param name="$idSelector" type="Object">The depart or arrive time selector, e.g., $('#departTimeId').</param>
        $idSelector.prop("readonly", true);
    }

    function setDateTimeProperties($departTimeSelector, $arriveTimeSelector) {
        /// <summary>
        /// Sets depart/arrive time properties, e.g., display format.
        /// </summary>
        /// <param name="$departTimeSelector" type="Object">The depart time selector, e.g., $('#departTimeId').</param>
        /// <param name="$arriveTimeSelector" type="Object">The arrive time selector, e.g., $('#arriveTimeId').</param>

        initialiseDepartAndArriveHomeTime($departTimeSelector);
        initialiseDepartAndArriveHomeTime($arriveTimeSelector);

        var dateFormatValue = ''; // Disable date entry
        var timeFormatValue = 'HH:mm'; // e.g.., 12:19

        $departTimeSelector.datetimepicker({
            dateFormat: dateFormatValue,
            timeFormat: timeFormatValue,
            controlType: 'select'
        });
        $departTimeSelector.focus(repositionAndHideDateCalendar);

        $arriveTimeSelector.datetimepicker({
            dateFormat: dateFormatValue,
            timeFormat: timeFormatValue,
            controlType: 'select'
        });
        $arriveTimeSelector.focus(repositionAndHideDateCalendar);
    }

    function repositionAndHideDateCalendar() {
        /// <summary>
        /// Repositions control to dipslay further down and to the right and hides the date part leaving just the time selection.
        /// </summary>

        // Move
        var dim = $(this).offset();
        $("#ui-datepicker-div").offset({
            top: dim.top - 180,
            left: dim.left + 150
        });

        // Hide
        $('.ui-datepicker-header').hide(); // Month header
        $('.ui-datepicker-calendar').hide(); // Calendar
        $('button.ui-datepicker-current').hide(); // Now button
    }
});
