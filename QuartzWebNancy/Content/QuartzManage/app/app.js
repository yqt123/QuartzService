//定义主模块并注入依赖
angular.module("voteApp", ["ngRoute"]);

//路由配置
angular.module("voteApp").config(["$routeProvider", function ($routeProvider) {
    $routeProvider.when("/list", {
        templateUrl: "/quartzmanage/list",
        controller: playerListCtrl
    }).when("/view/:id", {
        templateUrl: "/quartzmanage/view",
        controller: playerViewCtrl
    }).when("/add", {
        templateUrl: "/quartzmanage/add",
        controller: playerAddCtrl
    }).when("/edit/:playerId", {
        templateUrl: "/quartzmanage/edit",
        controller: playerEditCtrl
    }).otherwise({
        redirectTo: "/list"
    });
}]);