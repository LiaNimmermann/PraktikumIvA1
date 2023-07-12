// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



const mockData = {
    userPosting : {
        labels: ["User1", "User2", "User3", "User4", "User5"],
        postsAllTime: [5, 24, 6, 22, 8],
        postsLastWeek: [5, 24, 6, 22, 8],
        postsToday: [5, 24, 6, 22, 8]
    },
    mostFollowedUsers : {
        labels: ["User1", "User2", "User3", "User4", "User5"],
        followers: [50, 45, 30, 18, 10],
        following: [1, 14, 3, 12, 7]
    },

    mostUpvotedPost : {     //All votes accumulated to users aka Users with most upvotes
        labels: ["User1", "User2", "User3", "User4", "User5"],
        upvotes: [50, 45, 30, 18, 10],
        downvotes: [1, 14, 3, 12, 7]
    },

    mostDownvotedPost: {   //All votes accumulated to users aka Users with most downvotes
        labels: ["User1", "User2", "User3", "User4", "User5"],
        upvotes: [1, 14, 3, 12, 7],
        downvotes: [50, 45, 30, 18, 10]
    }
}

var userPostingGrid;
var mostFollowedUserGrid;
var mostUpvotedPostsGrid;
var mostDownvotedPostsGrid;

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

initUserPosting(mockData.userPosting);
initMostFollowedUser(mockData.mostFollowedUsers);
initMostUpvotedPosts(mockData.mostUpvotedPost);
initMostDownvotedPosts(mockData.mostDownvotedPost);
//function getPostsLastWeek(array, i,) {
//    return array[i].postsLastWeek
//}

//function extractLabels(array, getValue) {
//    var labels = [];
//    for (var i = 0; i < array.size; i++) {
//        labels[i] = array[i].id;
//    }
//    return labels;
//}

//function extractValue(array) {
//    var values = [];
//    for (var i = 0; i<array.size; i++) {
//        values[i] = array[i].value;
//    }
//    return values;
//}