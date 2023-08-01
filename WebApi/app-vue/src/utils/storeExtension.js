export default {
    getParams(loadOptions) {
        let params = "?";
        ["filter", "select", "sort", "skip", "take"].forEach(function (i) {
            if (
                i in loadOptions &&
                loadOptions[i] !== undefined &&
                loadOptions[i] !== null &&
                loadOptions[i] !== ""
            ) {
                params += `${i}=${JSON.stringify(loadOptions[i])}&`;
            }
        });
        params = params.slice(0, -1);
        return params;
    }
}