window.addEventListener("load", async (event) => {
    
    console.log("Window loaded");

});


auth0.createAuth0Client({
  domain: "dev-10d1syaroqa6fmth.us.auth0.com",
  clientId: "MliIKruu0WwIxR0oDlcEU8Q1xTnTjBQg",
  authorizationParams: {
    audience:"http://localhost:5000",
    redirect_uri: window.location.origin,
  }
}).then(async (auth0Client) => {
  // Assumes a button with id "login" in the DOM
  const loginButton = document.getElementById("login");
  const callApiButton = document.getElementById("callApi")
  loginButton.addEventListener("click", (e) => {
    console.log('test')
    e.preventDefault();
    auth0Client.loginWithRedirect()
  });

  if (location.search.includes("state=") && 
      (location.search.includes("code=") || 
      location.search.includes("error="))) {
    await auth0Client.handleRedirectCallback();
    window.history.replaceState({}, document.title, "/");
  }

  // Assumes a button with id "logout" in the DOM
  const logoutButton = document.getElementById("logout");
  const maartenLustGeenSpruitjes = document.getElementById("maarten");
  logoutButton.addEventListener("click", (e) => {
    e.preventDefault();
    console.log("logging out")
    auth0Client.logout();
  });

  callApiButton.addEventListener('click', async () => {
    const accessToken = await auth0Client.getTokenSilently();
    const result = await fetch('http://localhost:5000/api/seatholders', {
      method: 'GET',
      headers: {
        Authorization: 'Bearer ' + accessToken
      }
    });
    const data = await result.json();
    console.log(data);
  });

  const isAuthenticated = await auth0Client.isAuthenticated();
  const userProfile = await auth0Client.getUser();

  // Assumes an element with id "profile" in the DOM
  const profileElement = document.getElementById("profile");

  if (isAuthenticated) {
    console.log(await auth0Client.getTokenSilently());
    console.log(await auth0Client.getTokenSilently());
    loginButton.style.display="none";
    logoutButton.style.display="block";
    maartenLustGeenSpruitjes.style.display="block";
    callApiButton.style.display="block";
    maartenLustGeenSpruitjes.style.fontSize = '125px'
    maartenLustGeenSpruitjes.style.fontWeight = 'Bold'
    profileElement.style.display = "block";
    profileElement.innerHTML = `
            <p>${userProfile.name}</p>
            <img src="${userProfile.picture}" />
          `;
  } else {
    loginButton.style.display = "block";
    logoutButton.style.display= "none";
    profileElement.style.display = "none";
    maartenLustGeenSpruitjes.style.display = "none";
    
  }
});