import { useState } from 'react';
import Form from './components/Form.js'
import './App.css';

function App() {

  const url ="http://localhost:5232";

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [newUsername, setNewUsername] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [data, setData] = useState("");

  const verifyUser = async (e) => {
    e.preventDefault();

    const response = await fetch(`${url}/login-user?username=${username}&password=${password}`, {
      method: 'POST',
      credentials: 'include'
    })
    
    if(response.ok){
      fetchData();
    }else{
      alert("Invalid username or password");
    }

  }

  const registerUser = async (e) => {
    e.preventDefault();

    const response = await fetch(`${url}/register-user?username=${newUsername}&password=${newPassword}`, {
      method: 'POST',
      credentials: 'include'
    })
    
    if(response.ok){
      setUsername(newUsername);
      fetchData();
    }else{
      const errorMsg = await response.json();
      alert(errorMsg.message);
    }

  }

  const logoutUser = async (e) => {
    e.preventDefault();

    const response = await fetch(`${url}/logout-user?username=${username}`, {
      method: 'GET',
      credentials: 'include'
    })
    
    if(response.ok){
      setUsername("");
      setNewUsername("");
      setPassword("");
      setNewPassword("");
      setData("");
      window.location.reload();
    }else{
      const errorMsg = await response.json();
      alert(errorMsg.message);
    }

  }

  const fetchData = async()=>{
    const response = await fetch(`${url}/data`, {
      method: 'GET',
      credentials: 'include'
    })

    if(response.ok){
      const result = await response.json();
      setData(result.message);
    } else {
      setData('Unauthorized');
    }
  }

  
  return (
    <div className="App">
      <header className='App-header'>Log in app</header>
      {!data ? (
      <>
      <Form 
        logUser={registerUser}
        username={newUsername}
        addUsername={e => setNewUsername(e.target.value)}
        password={newPassword}
        addPassword={e => setNewPassword(e.target.value)}
        buttonName="Create new user"/>
      <Form 
        logUser={verifyUser}
        username={username}
        addUsername={e => setUsername(e.target.value)}
        password={password}
        addPassword={e => setPassword(e.target.value)}
        buttonName="Log in"/>
      </>
      ) : (
      <>
      <p>{data}</p>
      <button type="button" onClick={logoutUser}>Log out</button>
      </>
      )}
    </div>
  );
}

export default App;
