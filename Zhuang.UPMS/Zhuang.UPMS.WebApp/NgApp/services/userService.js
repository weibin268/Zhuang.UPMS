app.factory('userService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var baseUri = ngAuthSettings.apiServiceBaseUri;

    var service = {};

    service.getUser = function ()
    {
        return $http.get(baseUri + 'api/User/GetUserById?userId=1').then(function (results) {
            return results;
        });
    }

    return service;

}]);