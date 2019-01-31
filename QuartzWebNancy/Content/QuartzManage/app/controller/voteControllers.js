//List Controller
function playerListCtrl($scope, $http) {
    $http.get("/quartzmanage/scheduleDetails").then(function (res) {
        $scope.scheduleDetails = res.data;
    });
    $scope.orderProp = "-id"; //默认按票数降序排列
    //删除
    $scope.removePlayer = function (ev, id) {
        ev.preventDefault();
        angular.forEach($scope.scheduleDetails, function (val, key) {
            if (id === val.id) {
                $scope.scheduleDetails.splice(key, 1);
            }
        });
    };
}

//Add Controller
function playerAddCtrl($scope, $http, $location, voteSer) {
    //初始化位置信息
    $scope.positions = [
        { val: "PG", txt: "控球后卫" },
        { val: "SG", txt: "得分后卫" },
        { val: "SF", txt: "小前锋" },
        { val: "PF", txt: "大前锋" },
        { val: "C", txt: "中锋" }
    ];
    //初始化球队信息
    $scope.teams = ["骑士", "勇士", "尼克斯", "快船", "火箭", "篮网", "公牛", "雷霆"];

    //提交表单
    $scope.submitForm = function () {
        //console.log("Submit Player Form: ", angular.toJson($scope.player));
        voteSer.getPlayerNames().then(function (data) {
            //判断该球员姓名是否已存在
            if (data.indexOf($scope.player.name) >= 0) {
                $scope.isExisted = true;
            } else {
                //提交表单
                $http.post("/backend/acitonUrl", $scope.player).then(function (resp) { //无论是否保存成功，都进行页面跳转
                    console.log("Saved Successfully! Status: " + resp.status);
                    $location.path("#!/list");
                }, function (resp) {
                    console.log("Saved Failly! Status: " + resp.status);
                    $location.path("#!/list");
                });
            }
        });
    };
}

//Edit Controller
function playerEditCtrl($scope, $http, $routeParams, $location) {
    //初始化位置信息
    $scope.positions = [
        { val: "PG", txt: "控球后卫" },
        { val: "SG", txt: "得分后卫" },
        { val: "SF", txt: "小前锋" },
        { val: "PF", txt: "大前锋" },
        { val: "C", txt: "中锋" }
    ];
    //初始化球队信息
    $scope.teams = ["骑士", "勇士", "尼克斯", "快船", "火箭", "篮网", "公牛", "雷霆"];

    //获取被编辑的球员信息
    $http.get("/content/quartzmanage/data/players.json").then(function (res) {
        var i = parseInt($routeParams.playerId) - 1;
        $scope.player = res.data[i];
    });

    //提交表单
    $scope.submitForm = function () {
        //console.log("Submit Player Form: ", angular.toJson($scope.player));
        $http.post("/backend/acitonUrl", $scope.player).then(function (resp) { //无论是否保存成功，都进行页面跳转
            console.log("Saved Successfully! Status: " + resp.status);
            $location.path("#!/list");
        }, function (resp) {
            console.log("Saved Failly! Status: " + resp.status);
            $location.path("#!/list");
        });
    };
}

//View Controller
function playerViewCtrl($scope, $http, $routeParams) {

    $http.get("/quartzmanage/scheduleDetail/" + $routeParams.id).then(function (res) {
        $scope.detail = res.data;
    });

    //获取头像图片名称
    $scope.getThumb = function (playerThumb) {
        console.log("Player Thumb: " + playerThumb);
    };

    //投票
    $scope.voteBtnText = "投票";
    $scope.vote = function () {
        $scope.player.votes = $scope.player.votes + 1;
        $scope.voteBtnText = "已投票";
        $scope.isVoted = true;
    };
}