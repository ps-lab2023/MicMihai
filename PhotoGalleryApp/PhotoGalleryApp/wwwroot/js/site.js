"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/commentHub").build();

connection.on("ReceiveComment", function () {
    var badge = document.getElementById("notification-badge");
    var count = parseInt(badge.textContent || '0');
    count++;
    badge.textContent = count;
    badge.style.color = "red";
    badge.style.fontWeight = "bold";
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

// Load initial count of unviewed notifications
$(document).ready(function () {
    $.get("/Notifications/UnviewedCount", function (data) {
        var badge = document.getElementById("notification-badge");
        badge.textContent = data;
    });
});

document.getElementById("notification-badge").addEventListener('click', function () {
    // Send request to server to mark all notifications as viewed
    $.post("/Notifications/MarkAllAsViewed", function () {
        // Reset the notification badge
        var badge = document.getElementById("notification-badge");
        badge.textContent = 0;
        badge.style.color = "";
        badge.style.fontWeight = "";
    });
});
