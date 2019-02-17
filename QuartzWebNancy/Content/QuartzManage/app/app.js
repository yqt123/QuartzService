//定义主模块并注入依赖
angular.module("voteApp", ["ngRoute","ngAnimate"]);
//angular.module('voteApp', ['ngAnimate']);

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
    }).when("/add/:id", {
        templateUrl: "/quartzmanage/add",
        controller: playerAddCtrl
    }).when("/edit/:id", {
        templateUrl: "/quartzmanage/edit",
        controller: playerEditCtrl
    }).when("/triggerList/:id", {
        templateUrl: "/quartzmanage/triggerList",
        controller: triggerListCtrl
    }).when("/welcome", {
        templateUrl: "/quartzmanage/welcome"
    }).otherwise({
        redirectTo: "/welcome"
    });
}]);