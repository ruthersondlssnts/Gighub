﻿var GigsController = function (attendanceService) {

    var button;

    var init = function (container) {
        $(".js-toggle-attendance").on("click", toggleAttendance);
    }

    var toggleAttendance = function (e) {
        button = $(e.target);

        var gigId = button.attr("data-gig-id");

        if (button.hasClass("btn-default"))
            attendanceService.createAttendance(gigId, done, fail);
        else
            attendanceService.deleteAttendance(gigId, done, fail);

    }

    var fail = function () {
        alert("Somethin Failed");

    };

    var done = function () {
        var text = (button.text() == "Going") ? "Going?" : "Going";
        button.toggleClass("btn-info").toggleClass("btn-default").text(text);
    }

    return {
        init: init
    }
}(AttendanceService);
