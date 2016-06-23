$(document).ready(function() {
        $(".actionlink").click(function() {
            var action = $(this).data("action");
            var controller = $(this).data("controller");
            var target = $(this).data("target");
            $("#" + target).load("..\\" + controller + "\\" + action);
        });
});