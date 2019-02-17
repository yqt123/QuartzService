//List Controller
function playerListCtrl($scope, $http, $location, $compile) {
    $http.get("/quartzmanage/scheduleDetails").then(function (res) {
        $scope.scheduleDetails = res.data;
    });
    $scope.orderProp = "-id"; //默认按票数降序排列
    //删除
    $scope.removeSchedule = function (ev, id) {
        ev.preventDefault();
        $http.post("/quartzmanage/deleteScheduleDetail/" + id).then(function (resp) {
            if (resp.data.Status) {
                angular.forEach($scope.scheduleDetails, function (val, key) {
                    if (id === val.id) {
                        $scope.scheduleDetails.splice(key, 1);
                    }
                });
            }
            else {
                alert("删除失败！");
            }
        }, function (resp) {
            alert("删除失败！");
        });
    };

    $scope.viewSchedule = function (ev, id) {
        ev.preventDefault();
        var u = "#!/view/" + id;
        //$location.path(u);
        location.href = u;
    };

    $scope.openTrigger = function (ev, id) {
        ev.preventDefault();
        if (typeof ($scope["myCheck" + id]) === "undefined") {
            var ihtml = '<tr class="triggerWin triggerWin' + id + '"><td colspan="12"><div ng-hide="myCheck' + id + '" class="nghide ng-hide"><div>111111</div></div></td></tr>';
            var iElement = $compile(ihtml)($scope);
            angular.element($(ev.target).parents("tr")).after(iElement);
            $scope["myCheck" + id] = false;
        }
        else {
            $scope["myCheck" + id] = !($scope["myCheck" + id]);
            //angular.element(".triggerWin" + id).toggle();
        }
    };
}

//Add Controller
function playerAddCtrl($scope, $http, $routeParams, $location, voteSer) {
    if ($routeParams.id) {
        $http.get("/quartzmanage/scheduleDetail/" + $routeParams.id).then(function (res) {
            $scope.scheduler = res.data;
            $scope.scheduler.is_durable = false;
        });
    }

    //提交表单
    $scope.submitForm = function () {
        voteSer.getSchedulerNames().then(function (data) {
            //判断该球员姓名是否已存在
            if (data.indexOf($scope.scheduler.job_name.toLowerCase()) >= 0) {
                $scope.isExisted = true;
            } else {
                //提交表单
                $http.post("/quartzmanage/saveScheduleDetail", $scope.scheduler).then(function (resp) { //无论是否保存成功，都进行页面跳转
                    console.log("Saved Successfully! Status: " + resp.Status);
                    $location.path("#!/list");
                }, function (resp) {
                    console.log("Saved Failly! Status: " + resp.Status);
                    $location.path("#!/list");
                });
            }
        });
    };
}

//Edit Controller
function playerEditCtrl($scope, $http, $routeParams, $location, voteSer) {
    if ($routeParams.id) {
        $http.get("/quartzmanage/scheduleDetail/" + $routeParams.id).then(function (res) {
            $scope.scheduler = res.data;
            $scope.oldjob_name = res.data.job_name;
        });
    }
    //提交表单
    $scope.submitForm = function () {
        if ($scope.oldjob_name == $scope.scheduler.job_name) {
            EditScheduleDetail($http, $scope, $location);
        }
        else {
            voteSer.getSchedulerNames().then(function (data) {
                //判断该球员姓名是否已存在
                if (data.indexOf($scope.scheduler.job_name.toLowerCase()) >= 0) {
                    $scope.isExisted = true;
                } else {
                    EditScheduleDetail($http, $scope, $location);
                }
            });
        }
    };
}

function EditScheduleDetail($http, $scope, $location) {
    $http.post("/quartzmanage/EditScheduleDetail", $scope.scheduler).then(function (resp) {
        console.log("Saved Successfully! Status: " + resp.data.Status);
        $location.path("#!/list");
    }, function (resp) {
        alert("修改失败！");
    });
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

function triggerListCtrl($scope, $http, $routeParams) {

    if ($routeParams.id) {
        $http.get("/quartzmanage/triggers/" + $routeParams.id).then(function (res) {
            $scope.triggers = res.data;
        });
    }
    $scope.orderProp = "-id"; //默认按票数降序排列
}

