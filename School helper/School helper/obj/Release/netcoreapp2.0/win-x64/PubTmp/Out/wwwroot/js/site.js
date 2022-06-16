
function errorCheck(data, status, jqXHR) {
	// ToDo Мавричев добаавить проверку на наличие переадресации настраницу ошибки во всех аякс запросах
	if (jqXHR.getResponseHeader('content-type').indexOf('text/html') >= 0) {
		var error = $(data).find('#errorLabel').text();
		if (error !== undefined && error !== "") {
			window.location.href = '/Default/Home/Error?EventCode=' + error;
		}
	}
}


var ajaxPost = function (query, dataUrl, fnBeforeSend) {
	return $.ajax({
		url: dataUrl,
		type: "POST",
		contentType: "application/json; charset=utf-8",
		data: query,
		success: errorCheck,
		beforeSend: fnBeforeSend
	});
}