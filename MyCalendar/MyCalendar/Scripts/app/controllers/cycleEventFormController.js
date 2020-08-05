var CycleEventFormController = function () {

    var init = function (dropdownId, eventStartDate, eventEndDateId, eventTimeId) {
        handleFormDisplay(dropdownId, eventStartDate, eventEndDateId, eventTimeId);
    };

    var handleFormDisplay = function (dropdownId, eventStartDate, eventEndDateId, eventTimeId) {
        $(function () {

            if ($(dropdownId).find(":selected").text() !== "Ovulation" || $(dropdownId).find(":selected").text() !== "Period") {
                noEventProvided(eventStartDate, eventEndDateId, eventTimeId);
            }

            $(dropdownId).change(function () {
                var val = $(this).find(":selected").text();

                if (val == "Nothing Selected") {
                    noEventProvided(eventStartDate, eventEndDateId, eventTimeId);
                }
                else if (val == "Ovulation") {
                    ovulationEventDisplay(eventStartDate, eventTimeId, eventEndDateId);
                } else {
                    periodEventDisplay(eventStartDate, eventTimeId, eventEndDateId);
                }
            });
        });
    };

    var noEventProvided = function (eventStartDate, eventEndDateId, eventTimeId) {
        $(eventStartDate).hide()
        $(eventEndDateId).hide();
        $(eventTimeId).hide();

    };

    var periodEventDisplay = function (eventStartDate, eventTimeId, eventEndDateId) {
        $(eventTimeId).hide();
        $(eventStartDate).show();
        $(eventEndDateId).show();
    };

    var ovulationEventDisplay = function (eventStartDate, eventTimeId, eventEndDateId) {
        $(eventStartDate).show();
        $(eventTimeId).show();
        $(eventEndDateId).hide();
    };

    return {
        init: init
    }

}();
