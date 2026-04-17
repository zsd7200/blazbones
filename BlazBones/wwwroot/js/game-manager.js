"use strict";

const connection = new signalR
    .HubConnectionBuilder()
    .withUrl("/roomhub")
    .build();

const roomCode = window.location.href.split('/').slice(-1)[0];
const status = document.querySelector('#status');
const users = document.querySelector('#users');

connection.on('ReceiveRoomData', (allUsers) => {
    console.log('hello');
    users.innerHTML = '';
    for (let i = 0; i < allUsers.length; i++) {
        users.innerHTML += `<li>${allUsers[i].username}</li>`;
    }
});

const getRoomData = () => {
    console.log('here');
    console.log(roomCode);
    connection.invoke('GetRoomData', roomCode)
        .then((aaa) => {
            console.log('why');
            console.log(aaa);
        })
        .catch((err) => {
            return console.error(err);
        });
};

connection.start()
    .then(() => {
        console.log('ready');
        console.log(roomCode);
        getRoomData();

    })
    .catch((err) => {
        return console.error(err);
    });