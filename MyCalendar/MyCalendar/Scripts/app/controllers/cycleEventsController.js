var CycleEventsController = function (cycleEventsService) {
    var link;

    var init = function (container) {
        $(container).on("click", ".js-remove-event", removeCycleEvent)
    };

    var removeCycleEvent = function (e) {
        link = $(e.target);
        var eventId = link.attr("data-event-id");

        bootbox.confirm({
            message: "Are you sure you want to remove this event?",
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-default'
                }
            },
            callback: function (result) {
                if (result) {
                    cycleEventsService.deleteCycleEvent(eventId, done, fail);
                }
            }
        });
    };

    var done = function () {
        link.parents("li").fadeOut(function () {
            $(this).remove();
        });
    };

    var fail = function () {
        alert("Something failed!");
    };

    return {
        init: init
    }

}(CycleEventsService);
