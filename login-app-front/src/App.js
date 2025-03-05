import { useState } from 'react';
import './App.css';

function App() {

  const url ="http://localhost:5232";

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
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
      {!data ? (
      <>
      <header className='App-header'>Login user</header>
      <form onSubmit={verifyUser}>
        <input
          value={username}
          onInput={e => setUsername(e.target.value)}
          placeholder='Username'/>
        <input
          type="password"
          value={password}
          onInput={e => setPassword(e.target.value)}
          placeholder='Password'/>
        <button type="submit">Log in</button>
      </form>
      </>
      ) : (
      <>
      <header className='App-header'>User logged in!</header>
      <p>{data}</p>
      </>
      )}
    </div>
  );
}

export default App;
