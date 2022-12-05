var GigDetailsController = function (followService) {

    var followButton;

    var fail = function () {
        alert("Something Failed");

    };

    var done = function () {
        var text = (followButton.text() == "Following") ? "Follow" : "Following";
        followButton.toggleClass("btn-info").toggleClass("btn-default").text(text);
    }

    var toggleFollowing = function (e) {
        followButton = $(e.target);

        var followeeId = followButton.attr("data-user-id");

        if (followButton.hasClass("btn-default"))
            followService.createFollowing(followeeId, done, fail);
        else
            followService.deleteFollowing(followeeId, done, fail);

    }

    var init = function () {
        $(".js-toggle-follow").click(toggleFollowing);
    }
    

    return {
        init: init
    }
}(FollowService);
