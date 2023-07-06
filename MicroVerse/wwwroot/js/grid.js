// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function extractLabels(array) {
    var labels = [];
    for (var i = 0; i < array.size; i++) {
        labels[i] = array[i].id;
    }
    return labels;
}

function extractValue(array) {
    var values = [];
    for (var i = 0; i<array.size; i++) {
        values[i] = array[i].value;
    }
    return values;
}

const userPostingData = {
    labels: ["User1", "User2", "User3", "User4", "User5"],
    datasets: [
        {
            label: "Posts all time",
            data: [5, 24, 6, 22, 8]
        },
        {
            label: "Posts in the last 7 days",
            data: [1, 14, 3, 12, 7]
        }]
};

const userPostingGrid = new Chart("userPosting", {
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

const mostFollowedUserData = {
    labels: ["User1", "User2", "User3", "User4", "User5"],
    datasets: [
        {
            axis: 'y',
            label: "Followers",
            data: [50,45,30,18,10]
        },
        {
            axis:'y',
            label: "Following",
            data: [1, 14, 3, 12, 7]
        }]
};

const mostFollowedUserGrid = new Chart("mostFollowedUsers", {
    type: "bar",
    data: mostFollowedUserData,
    options: {
        indexAxis: 'y',
    }
})

const mostUpvotedPostData = {
    labels: ["User1", "User2", "User3", "User4", "User5"],
    datasets: [
        {
            axis: 'y',
            label: "Followers",
            data: [50, 45, 30, 18, 10]
        },
        {
            axis: 'y',
            label: "Following",
            data: [1, 14, 3, 12, 7]
        }]
};

const mostUpvotedPostGrid = new Chart("mostUpvotedPosts", {
    type: "bar",
    data: mostUpvotedPostData,
    options: {
        indexAxis: 'y',
    }
})