var currentButton = 0;

function NextButton (data) {
    var buttons = self.selectAll("button");

    var active = buttons[currentButton];
    var next;

    if (buttons.length > currentButton + 1){
        next = buttons[currentButton + 1];
        currentButton = currentButton + 1;
    }
    else {
        next = buttons[0];
        currentButton = 0;
    }

    active.style#color = "mediumpurple";
    active.style#background = "black";
    active.style#border = "2px solid mediumpurple";

    active.attributes["selected"] = "false";

    //active.removeClass("active");

    next.style#color = "black";
    next.style#background = "mediumpurple";
    next.style#border = "2px solid mediumpurple";

    next.attributes["selected"] = "true";

    //next.addClass("active");
}