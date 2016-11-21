
    var secs = 60;
    var interval = 3;
    var barwidth = 3;
    var numbars = 100 / barwidth;
    var count = 0;
    var currbar = 0;
    var currwidth = 0;  

    var p = new Element("progress");
    var l = new Element("div");
    var s = new Element("span");
    var d = new Element("div");

    for (var i = 0; i < 20; i++) {

        p = new Element("progress");
        p.attributes["max"] = 3;
        p.attributes["value"] = 0;
        p.attributes["id"] = "bar-"+i;
        /*l = new Element("div");;
        l.attributes["id"] = "progress-"+i;
        l.attributes["class"] = "progress";
        s = new Element("span");
        s.attributes["id"] = "percent";
        s.attributes["value"] = "30%";
        d = new Element("div");
        d.attributes["id"] = "bar";

        l.append(s);
        l.append(d);*/

        $(#bars).append(p);
        //$(#bars).append(l);

    }

    /*Increments the progress bar*/
    function update() {

        //update bar width
        var id = "#bar-"+count;
        currwidth++;
        var holder = self.select(id);
        holder.value = currwidth;
        holder.refresh();

        if(currwidth == barwidth)
        {
            count++;
            currwidth = 0;
        }

        
    }

    function getIntervalTime()
    {
        var id = "#interval";
        return self.select(id).value;


    }

    function getThreshold()
    {
        var id = "#threshold";
        return self.select(id).value;


    }

    $(#myBtn).on("click", function()
    {
        update();
        getIntervalTime();
        getThreshold();
    });