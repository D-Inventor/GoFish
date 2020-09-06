(function (angular) {

    angular.module('ithotl.gofish')
        .directive('gameplaying', [function () {

            return {
                restrict: 'A',
                controller: 'ithotl.gameplayingcontroller',
                templateUrl: '/gofish/js/gofish/gameplayingview.html'
            };

        }])

        .controller('ithotl.gameplayingcontroller', ['$scope', 'gameservice', function ($scope, gameservice) {

            var onChange = function (msg, newgame) {
                $scope.game = newgame;
                $scope.$apply();
            }

            var onReady = function (newgame) {
                $scope.game = newgame;
                $scope.$apply();
            }

            $scope.getCurrentPlayer = function () {
                return $scope.game.players[$scope.game.currentPlayer];
            }

            $scope.isMyTurn = function () {
                return $scope.getCurrentPlayer().id === gameservice.getUserId();
            }

            $scope.selectCard = function (index) {

                $scope.game.userCards.forEach(function (e) { e.selected = false; });
                $scope.game.userCards[index].selected = true;
            }

            $scope.give = function () {
                gameservice.giveCards($scope.game.userCards.filter(function (c) { return c.selected; }));
            }

            $scope.pass = function (index) {
                gameservice.passTurn($scope.game.players[index].id);
            }

            gameservice.subscribeOnGameChange(onChange);
            gameservice.subscribeOnReady(onReady);
            $scope.game = gameservice.getGame();
        }]);

}) (angular);