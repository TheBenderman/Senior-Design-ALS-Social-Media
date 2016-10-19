import React, { Component } from 'react';
import logo from './frowny_face.png';
import './App.css';
import 'bootstrap/dist/css/bootstrap.css';

window.jQuery = window.$ = require('jquery/dist/jquery.min');
global.Tether = require('tether');
require('bootstrap');

class App extends Component {
  render() {
    return (
      <div className="App">
        <div className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <h2>Welcome to Connectome</h2>
        </div>
        <p className="App-intro">

        </p>
      </div>
    );
  }
}

export default App;
