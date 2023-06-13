// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function SendRequest(id) {
    $.ajax({
        url: `/Home/SendFollow/${id}`,
        method: "GET",
        success: function (data) {
            let item = `<div class="alert alert-success">
        Your friend request sent successfully
</div>`;
            SendFollowCall(id);
            GetAllUsers();
            $("#request").html(item);
        },
        error: function (err) {
            console.log(err);
        }
    })
}

function DeclineRequest(id,senderId) {
    $.ajax({
        url: `/Home/DeclineRequest/${id}`,
        method: "GET",
        success: function (data) {
            let item = `<div class="alert alert-danger">
        You decline request.
</div>`;
            SendDeclineCall(senderId);
            GetAllUsers();
            $("#request").html(item);
        },
        error: function (err) {
            console.log(err);
        }
    })
}

function GetFriends() {
    $.ajax({
        url: `/Home/GetFriends/`,
        method: "GET",
        success: function (data) {
            let content = '<div style="display:flex; border: 1px solid blue;">';

            console.log("FRIENDS");
            console.log(data);

            for (var i = 0; i < data.length; i++) {
                var user = data[i];
                var c = `<img src='${user.imageUrl}' style='width:150px; height: 150px; border-radius: 50%;'>`
            }

            content += "</div>";
            $("#friends").html(content  );
        },
        error: function (err) {
            console.log(err);
        }
    })
}


function AcceptRequest(id, requestId) {
    $.ajax({
        url: `/Home/AcceptRequest?userId=` + id + "&requestId=" + requestId,
        method: "GET",
        success: function (data) {
            let item = `<div class="alert alert-success">
        You accept request successfully.
</div>`;
            GetMyRequests();
            GetAllUsers();
            $("#request").html(item);
        },
        error: function (err) {
            console.log(err);
        }
    })
}



function GetMyRequests() {
    $.ajax({
        url: "/Home/GetAllRequests",
        method: "GET",
        success: function (data) {

            let content = "";
            let subContent = "";
            for (var i = 0; i < data.length; i++) {
                if (data[i].status == "Request") {
                    subContent = `<div class='card-body'>
                    <button onclick="AcceptRequest('${data[i].senderId}',${data[i].id})" class='btn btn-success'>Accept</button>
                    <button class='btn btn-outline-secondary' onclick="DeclineRequest(${data[i].id},'${data[i].senderId}')">Decline</button>
                        </div>`;
                }
                else {
                    subContent = `<div class='card-body' >
                            <button class='btn btn-warning'>Delete </button>
</div>`;
                }

                let item = `<div class='card' style='width:15rem;' >
                            <div class='card-body'>
                <h5 class='card-title'>Friend Request</h5>
    </div>  
                <ul class='list-group list-group-flush'>
                <li class='list-group-item'>${data[i].content} </li>
</ul>
${subContent}
</div>
`;
                content += item;
            }
            console.log("worked");
            $("#requests").html(content);
        }
    })
}

GetMyRequests();

function GetAllUsers() {


    $.ajax({
        url: "/Home/GetAllUsers",
        method: "GET",
        success: function (data) {
            console.log(data);
            let content = "";
            for (var i = 0; i < data.length; i++) {
                let disconnectDate = new Date(data[i].disConnectTime);
                let dateContent = '';

                let subContent = "";
                if (data[i].hasRequestPending) {
                    subContent = `<button class='btn btn-outline-secondary'>Already Sent</button>`;
                }
                else {
                subContent = `<button onclick="SendRequest('${data[i].id}')" class='btn btn-outline-primary'> Follow</button>`
                }

                let style = '';
                if (data[i].isOnline) {
                    style = 'border:5px solid springgreen;';
                }
                else {
                    let currentDate = new Date();
                    let diffTime = Math.abs(currentDate - disconnectDate);
                    let diffMinutes = Math.ceil(diffTime / (1000 * 60));
                    if (diffMinutes >= 60) {
                        diffMinutes = Math.ceil(diffMinutes / 60);
                        dateContent = `<span class='btn btn-warning' >Left ${diffMinutes} hours ago</span>`;
                    }
                    else {
                        dateContent = `<span class='btn btn-warning' >Left ${diffMinutes} minutes ago</span>`;
                    }
                    style = 'border:5px solid red;';
                }

                let item = `<div class='card' style='width:14rem;margin:5px;'>
                        <img style='${style}width:220px;height:220px;' src='/images/${data[i].imageUrl}' />
            <div class='card-body'>
            <h5 class='card-title'>${data[i].userName}</h5>
            <p class='card-text'>${data[i].email}</p>
${subContent}
            <p class='card-text mt-2'>${dateContent}</p>
</div>
</div>`
                content += item;
            }

            $("#allusers").html(content);
        }
    })

}

GetFriends();