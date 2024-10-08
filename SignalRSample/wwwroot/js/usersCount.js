﻿
//create connection
var connectionUserCount = new signalR.HubConnectionBuilder()
    //.configureLogging(signalR.LogLevel.Information)
    .withAutomaticReconnect()
    .withUrl("/hubs/userCount", signalR.HttpTransportType.WebSockets).build();

//connect to methods that hub invokes aka receive notification from hub
//получаем инфу от метода который вызывается в хабе
connectionUserCount.on("updateTotalViews", (value, usersCount) => {
    var newCountSpan = document.getElementById("totalViewsCounter");
    var newUserSpan = document.getElementById("totalUsersCounter");
    newCountSpan.innerText = value.toString();
    newUserSpan.innerText = usersCount.toString();
});

connectionUserCount.on("updateTotalUsers", (value) => { 
    var newCountSpan = document.getElementById("totalUsersCounter");
    newCountSpan.innerText = value.toString();
});

//invoke hub methods aka send notification to hub
function newWindowLoadedOnClient() {
    connectionUserCount.invoke("NewWindowLoaded", "admin").then((value) => console.log(value));
}

//start connection
//эта функция будет, если все впорядке и соединение установлено
function fullfilled() {
    //do something on start
    console.log("Connection to User Hub Successful");
    newWindowLoadedOnClient();
}
function rejected() {
    //rejected logs
}

connectionUserCount.onclose((error) => {
    document.body.style.background = "red";
});

connectionUserCount.onreconnected((connectionId) => {
    document.body.style.background = "green";
});

connectionUserCount.onreconnecting((error) => {
    document.body.style.background = "orange";
});

connectionUserCount.start().then(fullfilled, rejected);