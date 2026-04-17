"use strict";

const connection = new signalR
    .HubConnectionBuilder()
    .withUrl("/roomhub")
    .build();

const createBtn = document.querySelector('#create-btn');
const joinBtn = document.querySelector('#join-btn');
const nickname = document.querySelector('#nickname');
const status = document.querySelector('#status');
const users = document.querySelector('#users');
const startBtn = document.querySelector('#start-btn');

const roomCode = document.querySelector('#room-code');
let roomCodeValue = roomCode?.value ?? null;

connection.on('CreatedRoom', (code) => {
    status.innerText = `room created with code: ${code}`;
    roomCodeValue = code;
});

connection.on('JoinedRoom', (code) => {
    status.innerText = `room joined with code ${code}`;
});

connection.on('JoinFailed', (msg) => {
    status.innerText = `Failed to join room. ${msg}`
});

connection.on('UserJoined', (user, allUsers) => {
    status.innerText = `${user} joined room`;
    console.log(allUsers);
    if (window.blazorInstance) {
        window.blazorInstance.invokeMethodAsync("SetUsers", allUsers);
    }
    users.innerHTML = '';
    for (let i = 0; i < allUsers.length; i++) {
        users.innerHTML += `<li>${allUsers[i].username}</li>`;
    }

    if (allUsers.length > 1 && startBtn)
        startBtn.disabled = false;
});

connection.on('CreatedGame', (code) => {
    console.log('game start!');
    if (window.blazorInstance) {
        window.blazorInstance.invokeMethodAsync("SetGameStart");
    }
});

connection.start()
    .then(() => {
        console.log('ready');
        if (createBtn)
            createBtn.disabled = false;
        if (joinBtn)
            joinBtn.disabled = false;
    })
    .catch((err) => {
        return console.error(err);
    });

if (createBtn) {
    createBtn.addEventListener('click', () => {
        if (nickname.value.length < 3) {
            status.innerText = 'nickname too short, please try again';
            return;
        }

        nickname.disabled = true;
        createBtn.disabled = true;
        connection.invoke('CreateRoom', nickname.value)
            .catch((err) => {
                return console.error(err);
            });
    });
}

if (joinBtn) {
    joinBtn.addEventListener('click', () => {
        nickname.disabled = true;
        roomCode.disabled = true;
        joinBtn.disabled = true;
        connection.invoke('JoinRoom', roomCode.value, nickname.value)
            .catch((err) => {
                return console.error(err);
            });
    });
}

if (startBtn) {
    startBtn.addEventListener('click', () => {
        connection.invoke('CreateGame', roomCodeValue)
            .catch((err) => {
                return console.error(err);
            });
    });
}

