// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


var userPostingGrid;
var mostFollowedUserGrid;
var mostUpvotedPostsGrid;
var mostDownvotedPostsGrid;

//initializing Chart of Userposting
function initUserPosting(data) {
    var userPostingData = {
        labels: data.labels,
        datasets: [
            {
                label: "Posts all time",
                data: data.postsAllTime
            },
            {
                label: "Posts in the last 7 days",
                data: data.postsLastWeek
            },
            {
                label: "Posts today",
                data: data.postsToday
            }
        ]
    };
    userPostingGrid = new Chart("userPosting", {
        type: "bar",
        data: userPostingData,
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    })
}

//Initializing Chart of most followed users with data
function initMostFollowedUser(data) {
    var mostFollowedUserData = {
        labels: data.labels,
        datasets: [
            {
                axis: 'y',
                label: "Followers",
                data: data.followers
            },
            {
                axis: 'y',
                label: "Following",
                data: data.following
            }]
    };
    mostFollowedUserGrid = new Chart("mostFollowedUsers", {
        type: "bar",
        data: mostFollowedUserData,
        options: {
            indexAxis: 'y',
        }
    })
}

//Initializing Chart of most upvoted users with data
function initMostUpvotedPosts(data) {
    var mostUpvotedPostsData = {
        labels: data.labels,
        datasets: [
            {
                axis: 'y',
                label: "Upvotes",
                data: data.upvotes
            },
            {
                axis: 'y',
                label: "Downvotes",
                data: data.downvotes
            }]
    };

    mostUpvotedPostsGrid = new Chart("mostUpvotedPosts", {
        type: "bar",
        data: mostUpvotedPostsData,
        options: {
            indexAxis: 'y',
        }
    })
}

//Initializing Chart of most downvoted users with data
function initMostDownvotedPosts(data) {
    var mostDownvotedPostsData = {
        labels: data.labels,
        datasets: [
            {
                axis: 'y',
                label: "Downvotes",
                data: data.downvotes
            },
            {
                axis: 'y',
                label: "Upvotes",
                data: data.upvotes
            }]
    };

    mostDownvotedPostsGrid = new Chart("mostDownvotedPosts", {
        type: "bar",
        data: mostDownvotedPostsData,
        options: {
            indexAxis: 'y',
        }
    })
}


//Getting data from the backend and calling the chart initializer functions with corresponding data
function getData() {
    const xhttp = new XMLHttpRequest();
    xhttp.onload = function () {
        var data = JSON.parse(this.responseText);
        initUserPosting(data.userPosting);
        initMostFollowedUser(data.mostFollowedUsers);
        initMostUpvotedPosts(data.mostUpvotedPost);
        initMostDownvotedPosts(data.mostDownvotedPost);
    }
    xhttp.open("GET", "https://localhost:7028/api/Post/UserPostingStats");
    xhttp.send();
}


getData();