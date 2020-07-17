var CycleEventsService = function () {
    var deleteCycleEvent = function (eventId, done, fail) {
        $.ajax({
            url: "/api/cycleEvents/" + eventId,
            method: "DELETE"
        })
            .done(done)
            .fail(fail);
    };

    return {
        deleteCycleEvent: deleteCycleEvent
    }
}();