# TestVar Flashcard Client Application

The client is build on the React framework and makes use of the Material UI theme for design elements.
The server is using ASP.NET Core and EntityFramework Core for the API and Database respectively.

## Authentication

The application is confiured as an OAUTH client to the ApiServer that provides and identity services that
implements the OAUTH protocol using the .Net OpenIdDict libraries. Because the service is OAUTH compliant
this client application uses the `oidc-auth-ts` library to handle the authentication and token exchange
processes.

The implementation of both parts is based on the following source article https://andreyka26.com/oauth-authorization-code-react-client-pt1-openIddict

### To run this project

Firstly, ensure you are running ApiServer on https, then cd into the `client/testvar-spa` directory and run `npm start`
the client requires the API server to be running. 

**Please note** - (After testing this myself with a zip from the repos) after cd'ing into the `client/testvar-spa` directory and running `npm start` it caused an error of "react scripts is not recognized as a command"
in the event that this happens, simply run `npm install` and then once this has finished, try again.

**Important** - the application requires a dev certificate to use HTTPS, this is installed by using the following command 
`dotnet dev-certs https --check --trust` in the terminal, this certificate applies to the machine, not the project... 
it may not necessarily need to be run in a specific directory (from memory).
