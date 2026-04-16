"use strict";

const connection = new signalR
    .HubConnectionBuilder()
    .withUrl("/roomhub")
    .build();

const createBtn = document.querySelector('#create-btn');
const joinBtn = document.querySelector('#join-btn');
const nickname = document.querySelector('#nickname');
const roomCode = document.querySelector('#room-code');
const status = document.querySelector('#status');
const users = document.querySelector('#users');

connection.on('CreatedRoom', (roomCode) => {
    status.innerText = `room created with code: ${roomCode}`;
});

connection.on('JoinedRoom', (roomCode) => {
    status.innerText = `room joined with code ${roomCode}`;
});

connection.on('JoinFailed', (msg) => {
    status.innerText = `Failed to join room. ${msg}`
});

connection.on('UserJoined', (user, allUsers) => {
    status.innerText = `${user} joined room`;
    console.log(allUsers);
    users.innerHTML = '';
    for (let i = 0; i < allUsers.length; i++) {
        users.innerHTML += `<li>${allUsers[i].username}</li>`;
    }
});

console.log(connection.state);

connection.start()
    .then(() => {
        console.log('ready');
        createBtn.disabled = false;
        joinBtn.disabled = false;
    })
    .catch((err) => {
        return console.error(err);
    });

createBtn.addEventListener('click', () => {
    if (nickname.value.length < 3) {
        status.innerText = 'nickname too short, please try again';
        return;
    }

    connection.invoke('CreateRoom', nickname.value)
        .catch((err) => {
            return console.error(err);
        });
});

joinBtn.addEventListener('click', () => {
    connection.invoke('JoinRoom', roomCode.value, nickname.value)
        .catch((err) => {
            return console.error(err);
        });
});