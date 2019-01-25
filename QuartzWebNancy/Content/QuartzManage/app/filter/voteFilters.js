//格式化球员位置
angular.module("voteApp").filter("boolFilter", function() {
	return function(val) {
		var pos = "Unknown";
		switch (val) {
            case true:
				pos = "是";
				break;
            case false:
				pos = "否";
				break;
			default:
				break;
		}
		return pos;
	}
});
