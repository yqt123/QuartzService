//自定义Service
angular.module("voteApp").service("voteSer", ["$q", "$http", function ($q, $http) {

    this.getSchedulerNames = function() {
        return $http.get("/quartzmanage/scheduleDetails").then(function(resp) {
			if(typeof resp.data === "object") {
                var schedulerNames = [];
				angular.forEach(resp.data, function(v, k) {
                    schedulerNames.push(v.job_name.toLowerCase());
				});
                return schedulerNames;
			}else {
				//无效数据
				return $q.reject(resp.data);
			}
		}, function(resp) {
			return $q.reject(resp.status);
		});
    };

    this.getTriggerNames = function () {
        return $http.get("/quartzmanage/allTriggers").then(function (resp) {
            if (typeof resp.data === "object") {
                var schedulerNames = [];
                angular.forEach(resp.data, function (v, k) {
                    schedulerNames.push(v.trigger_name.toLowerCase());
                });
                return schedulerNames;
            } else {
                //无效数据
                return $q.reject(resp.data);
            }
        }, function (resp) {
            return $q.reject(resp.status);
        });
    };
}]);

