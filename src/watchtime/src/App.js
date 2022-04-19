import logo from './logo.svg';
import {BrowserRouter as Router,Routes, Route} from "react-router-dom"
import './App.css';
import Login from "./Containers/Login"
import { Fragment } from 'react';
import {useState,useEffect} from "react"
import {connect, StringCodec} from "../node_modules/nats.ws/lib/src/mod"

function App() {
  return (
    <Router>
    <Fragment>
      <Routes>
        <Route path='/login' element={<Login />}/>
      </Routes>
    </Fragment>
  </Router>
  );
}

export default App;
