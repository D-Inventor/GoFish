(function (angular) {

    angular.module('ithotl.gofish')
        .factory('gameservice', ['$interval', function ($interval) {

            var connection = new signalR.HubConnectionBuilder()
                .withAutomaticReconnect()
                .withUrl("/gofish/gamehub")
                .build();
            var onReadySubscribers = [];
            var onErrorSubscribers = [];
            var onGameChangeSubscribers = [];
            var ready = false;
            var userId = null;
            var game = null;

            connection.on('ReceiveGameData', function (id, data) {
                userId = id;
                game = data;
                console.log(data);
                onReadySubscribers.forEach(function (h) {
                    h(data);
                });
                ready = true;
            });

            connection.on('ReceiveError', function (msg) {

                if (typeof msg === 'string') {
                    console.error(msg);
                    onErrorSubscribers.forEach(function (h) {
                        h(msg);
                    });
                }
                else if (Array.isArray(msg)) msg.forEach(function (e) {
                    console.error(e);
                    onErrorSubscribers.forEach(function (h) {
                        h(e);
                    });
                });
            });

            connection.on('ReceiveGameChange', function (msg, newgame) {
                game = newgame;
                onGameChangeSubscribers.forEach(function (h) {
                    h(msg, newgame);
                });
            });

            connection.start().then(function () {
                connection.invoke("initialise");
            }).catch(function (err) {
                alert(err.toString());
            });

            // keep connection alive by pinging every 2 minutes
            $interval(function () {

                connection.invoke("ping");
            }, 120000);


            // Create a service from the signalr connection
            var service = {};

            service.subscribeOnReady = function (handler) {
                onReadySubscribers.push(handler);
            }

            service.subscribeOnError = function (handler) {
                onErrorSubscribers.push(handler);
            }

            service.subscribeOnGameChange = function (handler) {
                onGameChangeSubscribers.push(handler);
            }



            service.getReady = function () { return ready; }
            service.getUserId = function () { return userId; }
            service.getGame = function () { return game; }



            service.createGame = function (username) {
                connection.invoke("create", username);
            }

            service.joinGame = function (username) {
                connection.invoke("join", username);
            }

            service.startGame = function () {
                connection.invoke("start");
            }

            service.passTurn = function (id) {
                connection.invoke("pass", id);
            }

            service.giveCards = function (cards) {
                connection.invoke("give", cards);
            }

            return service;
        }]);

}) (angular);