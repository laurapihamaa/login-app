function Form({logUser, username, addUsername, password, addPassword, buttonName}){
    return (
          <div>
          <form onSubmit={logUser}>
            <input
              value={username}
              onInput={addUsername}
              placeholder='Username'/>
            <input
              type="password"
              value={password}
              onInput={addPassword}
              placeholder='Password'/>
            <button type="submit">{buttonName}</button>
          </form>
        </div>
      );
}

export default Form;