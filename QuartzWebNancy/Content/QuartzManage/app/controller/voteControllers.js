//List Controller
function playerListCtrl($scope, $http, $location, $compile, $interval) {
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
        $location.path("/view/" + id);
    };

    $scope.openTrigger = function (ev, id) {
        ev.preventDefault();
        if (typeof ($scope["myCheck" + id]) === "undefined") {
            $http.get("/quartzmanage/triggers/" + id).then(function (res) {
                $scope["triggers" + id] = res.data;
                var ihtml = '<tr class="triggerWin triggerWin' + id + '"><td colspan="12"><div ng-hide="myCheck' + id + '" class="triggerWinhide triggerWinhide' + id + ' ng-hide">' + GetTriggerHtml(id) + '</div></td></tr>';
                var iElement = $compile(ihtml)($scope);
                angular.element($(ev.target).parents("tr")).after(iElement);
                $scope["myCheck" + id] = false;
                $(".triggerWin" + id + ">td").css({ 'border-bottom': '1px solid #cbcbcb' });
            });
        }
        else {
            if ($scope["myCheck" + id]) {
                $(".triggerWin" + id + ">td").css({ 'border-bottom': '1px solid #cbcbcb' });
            }
            else {
                $(".triggerWin" + id + ">td").css({ 'border-bottom': '0' });
            }
            $scope["myCheck" + id] = !($scope["myCheck" + id]);
        }
    };

    //删除触发器
    $scope.removeTrigger = function (ev, id) {
        ev.preventDefault();
        $http.post("/quartzmanage/deleteTrigger/" + id).then(function (resp) {
            if (resp.data.Status) {
                angular.forEach($scope.triggers, function (val, key) {
                    if (id === val.id) {
                        $scope.triggers.splice(key, 1);
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

    //轮询检查作业状态
    var stop = $interval(function () {
        $http.get("/quartzmanage/scheduleStatus").then(function (resp) {
            $(".header-run").text(resp.data.Result);
        }, function (resp) {
            console.log("Failly! Status: " + resp.Status);
        });
    }, 10000);
    $scope.$on('$ionicView.beforeLeave', function () {
        $interval.cancel(stop);//离开页面后停止轮询
    });

    //重新开启
    $scope.ReStart = function (ev) {
        ev.preventDefault();
        $http.post("/quartzmanage/ReStart").then(function (resp) {
            if (resp.data.Status) {
            }
            else {
                alert("删除失败！");
            }
        }, function (resp) {
            alert("重启失败！");
        });
    };

    //添加到作业队列
    $scope.JobToSchedulePlan = function (ev, id) {
        ev.preventDefault();
        $http.post("/quartzmanage/JobToSchedulePlan/" + id).then(function (resp) {
            if (resp.data.Status) {
                angular.forEach($scope.scheduleDetails, function (val, key) {
                    if (id === val.id) {
                        val.isRunning = true;
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
                    $location.path("/home");
                }, function (resp) {
                    console.log("Saved Failly! Status: " + resp.Status);
                    $location.path("/home");
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
        $location.path("/home");
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

function GetTriggerHtml(id) {
    var triggerHtml = '\
<br/>\
<a href="#!/triggerAdd/'+ id + '" class="pure-button pure-button-primary">新增触发器</a>\
<table class="pure-table pure-table-horizontal">\
<thead>\
    <tr>\
        <th>编号</th>\
        <th>计划名称</th>\
        <th>作业名称</th>\
        <th>作业组</th>\
        <th>触发器</th>\
        <th>触发器组</th>\
        <th>触发规则</th>\
        <th>触发类型</th>\
        <th>触发次数</th>\
        <th>触发间隔</th>\
        <th>开始时间</th>\
        <th>结束时间</th>\
        <th>操作</th>\
    </tr>\
</thead>\
<tbody>\
    <tr ng-repeat="item in triggers'+ id + ' | filter:queryVal | orderBy:orderProp">\
        <td><a href="#!/view/{{item.id}}">{{item.id}}</a></td>\
        <td>{{item.sched_name}}</td>\
        <td>{{item.job_name}}</td>\
        <td>{{item.job_group}}</td>\
        <td>{{item.trigger_name}}</td>\
        <td>{{item.trigger_group}}</td>\
        <td>{{item.cronexpression}}</td>\
        <td>{{item.trigger_type}}</td>\
        <td>{{item.repeat_count}}</td>\
        <td>{{item.repeat_interval}}</td>\
        <td>{{item.startTime}}</td>\
        <td>{{item.endTime}}</td>\
        <td>\
        <a href = "#!/triggerEdit/{{item.id}}">编辑</a>\
        <a href = "#" ng-click="removeTrigger($event, item.id)">删除</a>\
        </td>\
    </tr>\
</tbody>\
</table>\
';
    return triggerHtml;
}

//触发器作业
function triggerAddCtrl($scope, $http, $routeParams, $location, voteSer) {
    if ($routeParams.id) {
        $http.get("/quartzmanage/scheduleDetail/" + $routeParams.id).then(function (res) {
            $scope.trigger = new Object();
            $scope.trigger.sched_name = res.data.sched_name;
            $scope.trigger.job_name = res.data.job_name;
            $scope.trigger.job_group = res.data.job_group;
            $scope.trigger.trigger_type = "cron";
            $scope.sched_id = res.data.id;
        });
    }

    //提交表单
    $scope.submitForm = function () {
        voteSer.getTriggerNames().then(function (data) {
            //判断姓名是否已存在
            if (data.indexOf($scope.trigger.trigger_name.toLowerCase()) >= 0) {
                $scope.isExisted = true;
            } else {
                //提交表单
                $http.post("/quartzmanage/saveTrigger", $scope.trigger).then(function (resp) { //无论是否保存成功，都进行页面跳转
                    console.log("Saved Successfully! Status: " + resp.Status);
                    $location.path("/home");
                }, function (resp) {
                    console.log("Saved Failly! Status: " + resp.Status);
                    $location.path("/home");
                });
            }
        });
    };
}

//Edit Controller
function triggerEditCtrl($scope, $http, $routeParams, $location, voteSer) {
    if ($routeParams.id) {
        $http.get("/quartzmanage/trigger/" + $routeParams.id).then(function (res) {
            if (typeof res.data === "object") {
                $scope.trigger = res.data;
                $scope.oldtrigger_name = $scope.trigger.trigger_name;
            }
        });
    }
    //提交表单
    $scope.submitForm = function () {
        if ($scope.oldtrigger_name == $scope.trigger.trigger_name) {
            EditTrigger($http, $scope, $location);
        }
        else {
            voteSer.getTriggerNames().then(function (data) {
                //判断该球员姓名是否已存在
                if (data.indexOf($scope.trigger.trigger_name.toLowerCase()) >= 0) {
                    $scope.isExisted = true;
                } else {
                    EditTrigger($http, $scope, $location);
                }
            });
        }
    };
}

function EditTrigger($http, $scope, $location) {
    $http.post("/quartzmanage/editTrigger", $scope.trigger).then(function (resp) {
        console.log("Saved Successfully! Status: " + resp.data.Status);
        $location.path("/home");
    }, function (resp) {
        alert("修改失败！");
    });
}

function QuartzRunCtrl($scope, $http, $location) {
    $scope.statusName = "未开始";
    $scope.status = "~/Content/Image/start.png";
}
