var CycleEventFormController = function () {

    var init = function (dropdownId, eventTimeId, eventDateId) {
        handleFormDisplay(dropdownId, eventTimeId, eventDateId);
    };

    var handleFormDisplay = function (dropdownId, eventTimeId, eventDateId) {
        $(function () {

            if ($(dropdownId).find(":selected").text() !== "Ovulation") {
                periodEventDisplay(eventTimeId, eventDateId);
            }

            $(dropdownId).change(function () {
                var val = $(this).find(":selected").text();

                if (val === "Ovulation") {
                    ovulationEventDisplay(eventTimeId, eventDateId);
                } else {
                    periodEventDisplay(eventTimeId, eventDateId);
                }
            });
        });
    };

    var periodEventDisplay = function (eventTimeId, eventDateId) {
        $(eventTimeId).hide();
        $(eventDateId).show();
    };

    var ovulationEventDisplay = function (eventTimeId, eventDateId) {
        $(eventTimeId).show();
        $(eventDateId).hide();
    };

    return {
        init: init
    }

}();
