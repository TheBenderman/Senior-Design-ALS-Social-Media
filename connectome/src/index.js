import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import './index.css';
import './CSS/Welcome.css'

var $ = require('jquery/dist/jquery.min');

var Welcome = React.createClass({
  render : function() {
    return (<App />);
  },
  _handleKeyPress : function (e) {
    var currentCard = $(".active-card");

    // left key
    if (e.which === 37){
      currentCard.removeClass("active-card");

      var previousCard = currentCard.prev();
      if (previousCard.length === 0){
        currentCard.addClass("active-card");
      }
      else if (previousCard.length > 0){
        previousCard.addClass("active-card");
      }
    }
    // right key
    else if (e.which === 39) {
      currentCard.removeClass("active-card");

      var nextCard = currentCard.next();
      if (nextCard.length === 0){
        currentCard.addClass("active-card");
      }
      else if (nextCard.length > 0){
        nextCard.addClass("active-card");
      }
    }
    // enter key
    else if (e.which === 13) {
      currentCard.find("a").click();
    }
  },
  componentWillMount : function(){
    document.addEventListener("keydown", this._handleKeyPress, false);
  },
  componentWillUnmount : function(){
    document.removeEventListener("keydown", this._handleKeyPress, false);
  }
});

ReactDOM.render(
  <Welcome />,
  document.getElementById('root')
);
