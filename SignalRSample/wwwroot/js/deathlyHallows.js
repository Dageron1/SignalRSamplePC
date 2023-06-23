var cloakSpan = document.getElementById("cloakCounter");
var stoneSpan = document.getElementById("stoneCounter");
var wandSpan = document.getElementById("wandCounter");

//create connection
var connectionDeathlyHallows = new signalR.HubConnectionBuilder()
    //.configureLogging(signalR.LogLevel.Information)
    .withUrl("/hubs/deathlyhallows").build();

//connect to methods that hub invokes aka receive notification from hub
//получаем инфу от метода который вызывается в хабе
connectionDeathlyHallows.on("updateDeathlyHallowCount", (cloak, stone, wand) => {
    cloakSpan.innerText = cloak.toString();
    stoneSpan.innerText = stone.toString();
    wandSpan.innerText = wand.toString();
});

function fullfilled() {
    //do something on start
    //используем invoke т к ожидаем ответ от сервера в виде значений
    //с помощью метода этого мы отображаем первичные данные в новом окне, 
    //если не добавить то при первой загрузке ничего не будет, пока не произайдет какое - либо действие
    connectionDeathlyHallows.invoke("GetRaceStatus").then((raceCounter) => {
        cloakSpan.innerText = raceCounter.cloak.toString();
        stoneSpan.innerText = raceCounter.stone.toString();
        wandSpan.innerText = raceCounter.wand.toString();
    })

    console.log("Connection to User Hub Successful");
}
function rejected() {
    //rejected logs
}
connectionDeathlyHallows.start().then(fullfilled, rejected);