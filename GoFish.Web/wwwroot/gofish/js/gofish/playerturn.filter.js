(function (angular) {

    angular.module('ithotl.gofish')
        .filter('playerturn', ['gameservice', function (gameservice) {

            return function (input) {

                var name = "";
                if (input.id === gameservice.getUserId()) name = "your";
                else {

                    var lc = input.username[input.username.length - 1];
                    if (lc === "s") name = input.username + "'";
                    else name = input.username + "'s";
                }

                return "It's " + name + " turn!";
            }
        }]);

}) (angular);