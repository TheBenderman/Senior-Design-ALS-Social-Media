document.onkeydown = onkeydownhandler;

function onkeydownhandler(e) {
  var currentCard = $(".active-card");

  // left key
  if (e.which == 37){
    currentCard.removeClass("active-card");

    var previousCard = currentCard.prev();
    if (previousCard.length == 0){
      currentCard.addClass("active-card");
    }
    else if (previousCard.length > 0){
      previousCard.addClass("active-card");
    }
  }
  // right key
  else if (e.which == 39) {
    currentCard.removeClass("active-card");

    var nextCard = currentCard.next();
    if (nextCard.length == 0){
      currentCard.addClass("active-card");
    }
    else if (nextCard.length > 0){
      nextCard.addClass("active-card");
    }
  }
  // enter key
  else if (e.which == 13) {
    currentCard.find("a").click();
  }
}
