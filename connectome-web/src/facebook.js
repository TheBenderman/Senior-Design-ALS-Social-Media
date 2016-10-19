import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import './index.css';
import './CSS/Facebook.css'

var $ = require('jquery/dist/jquery.min');

var Facebook = React.createClass({
  render : function() {
    return (<App />);
  },
  _handleKeyPress : function (e) {
  },
  componentWillMount : function(){
  },
  componentWillUnmount : function(){
  }
});

ReactDOM.render(
  <Welcome />,
  document.getElementById('root')
);
