import React, { useState } from "react";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";


export default function Login() {
  const search = window.location.search;
  const params = new URLSearchParams(search);
  const video = params.get('v');
  console.log(video)
  return (
<div>
    <h3>Partial Content Demonstration</h3>
    <hr />
    <video id="mainPlayer" width="1280" height="720" 
        autoPlay="autoplay" controls="controls">
        <source src={`http://watchtime.com/api/video/${video}`} />
    </video>
    </div>
  );
}