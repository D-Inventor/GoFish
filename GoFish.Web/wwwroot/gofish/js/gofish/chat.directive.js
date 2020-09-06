(function (angular) {

    angular.module('ithotl.gofish')
        .directive('chat', [function () {

            return {
                restrict: 'A',
                controller: 'ithotl.chatcontroller',
                templateUrl: '/gofish/js/gofish/chatview.html'
            };

        }])

        .controller('ithotl.chatcontroller', ['$scope', 'gameservice', function ($scope, gameservice) {

            var $this = this;

            $scope.messages = [];

            var onMessage = function (msg) {
                $scope.messages.push(msg);
            }

            var onError = function (msg) {
                onMessage({ text: msg, type: "error" });
                $scope.$apply();
            }

            var onChange = function (msg, newworld) {
                if (typeof msg === 'string') onMessage({ text: msg, type: "error" });
                else if (Array.isArray(msg)) msg.forEach(function (e) { onMessage({ text: e, type: "info" }); });
                else if (typeof msg === 'object') msg.feedback.forEach(function (e) { onMessage({ text: e, type: "info" }); });
                $scope.$apply();
            }

            gameservice.subscribeOnError(onError);
            gameservice.subscribeOnGameChange(onChange);
        }]);

}) (angular);