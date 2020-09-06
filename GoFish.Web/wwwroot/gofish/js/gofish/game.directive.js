(function (angular) {

    angular.module('ithotl.gofish')
        .directive('game', [function () {

            return {
                restrict: 'A',
                controller: 'ithotl.gamecontroller',
                templateUrl: '/gofish/js/gofish/gameview.html'
            };

        }])

        .controller('ithotl.gamecontroller', ['$scope', 'gameservice', function ($scope, gameservice) {

            $scope.showCreate = function () {
                return game === null || game.gameState === 2;
            }

            $scope.showJoin = function () {
                var uid = gameservice.getUserId();
                return game !== null && game.gameState === 0 && game.players.filter(function (p) { return p.id === uid; }).length == 0;
            }

            $scope.showLobby = function () {
                var uid = gameservice.getUserId();
                return game !== null && game.gameState === 0 && game.players.filter(function (p) { return p.id === uid; }).length > 0;
            }

            $scope.showPlaying = function () {
                return game !== null && game.gameState === 1;
            }

            $scope.showFinished = function () {
                return game !== null && game.gameState === 2;
            }

            var onChange = function (msg, newworld) {
                game = newworld;
                $scope.$apply();
            }

            var onReady = function (newworld) {
                game = newworld;
                $scope.$apply();
            }

            gameservice.subscribeOnGameChange(onChange);
            gameservice.subscribeOnReady(onReady);
            var game = gameservice.getGame();
        }]);

}) (angular);